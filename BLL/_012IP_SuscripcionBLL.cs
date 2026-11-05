using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace _012IP_BLL
{
    public class _012IP_SuscripcionBLL
    {
        private readonly _012IP_SuscripcionDAL suscripcionDAL;
        private readonly _012IP_CuotaBLL cuotaBLL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly _012IP_PagoBLL pagoBLL;

        public _012IP_SuscripcionBLL()
        {
            suscripcionDAL = new _012IP_SuscripcionDAL();
            cuotaBLL = new _012IP_CuotaBLL();
            bitacoraBLL = new BitacoraBLL();
            pagoBLL = new _012IP_PagoBLL();
        }

      
        public _012IP_SuscripcionBE _012IP_ObtenerSuscripcion(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception(LanguageManager.Instance.GetTraduction("DNIRequerido"));

            try
            {
                _012IP_SuscripcionBE suscripcion = suscripcionDAL._012IP_ObtenerSuscripcion(dni.Trim());
                if (suscripcion != null)
                    suscripcion.Cuotas = cuotaBLL._012IP_ObtenerCuotas(suscripcion.IdSuscripcion);
                return suscripcion;
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText4"), ex);
            }
        }

        
        public _012IP_SuscripcionBE _012IP_ConsultarSuscripcion(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception(LanguageManager.Instance.GetTraduction("DNIRequerido"));

            try
            {
                return suscripcionDAL._012IP_ObtenerSuscripcion(dni.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText4"), ex);
            }
        }

       
        // Días antes del vencimiento a partir de los cuales se avisa "por vencer".
        public const int DiasAvisoVencimiento = 7;
        public const int MaximoMesesPorPago = 12;

        // Membresía prepaga: el nuevo período arranca cuando vence la vigente (o hoy si ya venció)
        // y se extiende la cantidad de meses que el socio paga.
        public void _012IP_CalcularPeriodoRenovacion(_012IP_SuscripcionBE actual, int cantidadMeses, out DateTime fechaInicio, out DateTime fechaVencimiento)
        {
            DateTime hoy = DateTime.Today;

            if (actual != null && actual.FechaVencimiento.Date >= hoy)
                fechaInicio = actual.FechaVencimiento.Date;
            else
                fechaInicio = hoy;

            fechaVencimiento = fechaInicio.AddMonths(cantidadMeses > 0 ? cantidadMeses : 1);
        }

        public decimal _012IP_CalcularImporte(_012IP_MembresiaBE membresia, int cantidadMeses)
        {
            if (membresia == null) return 0;
            return membresia.CostoMensual * cantidadMeses;
        }

        // Renueva la membresía prepaga en una sola transacción:
        // registra el pago de los meses elegidos y extiende la fecha de vencimiento.
        // Si algo falla, no queda ni el pago ni la renovación.
        public _012IP_PagoBE _012IP_RenovarSuscripcion(_012IP_SocioBE socio, _012IP_MembresiaBE membresia, int cantidadMeses,
            DateTime fechaInicio, DateTime fechaVencimiento, string medioDePago, string? marcaTarjeta = null, string? ultimosDigitos = null)
        {
            if (socio == null || string.IsNullOrWhiteSpace(socio.DNI))
                throw new Exception(LanguageManager.Instance.GetTraduction("DNIRequerido"));
            if (membresia == null || membresia.IdMembresia <= 0)
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText2"));
            if (cantidadMeses < 1 || cantidadMeses > MaximoMesesPorPago || fechaVencimiento.Date <= fechaInicio.Date)
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText3"));

            string dni = socio.DNI.Trim();
            decimal importe = _012IP_CalcularImporte(membresia, cantidadMeses);
            try
            {
                _012IP_PagoBE pago;
                using (var transaccion = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    pago = pagoBLL._012IP_RegistrarPago(socio.IdSocio, null, membresia.IdMembresia, importe, medioDePago, marcaTarjeta, ultimosDigitos);

                    string dvh = DigitoVerificador.CalcularDVH(
                        dni + membresia.IdMembresia + fechaInicio.ToString("yyyy-MM-dd") + fechaVencimiento.ToString("yyyy-MM-dd") + "Activa");
                    suscripcionDAL._012IP_ActualizarSuscripcion(dni, membresia.IdMembresia, fechaInicio, fechaVencimiento, dvh);

                    Bitacora bitacora = new Bitacora();
                    bitacora.Login = SessionManager.Instance.UsuarioActual().Username;
                    bitacora.Modulo = "Membresias";
                    bitacora.Evento = $"Renovar membresia - DNI {dni} - {membresia.Nombre} - {cantidadMeses} mes(es) - vence {fechaVencimiento:dd/MM/yyyy}";
                    bitacora.Criticidad = 2;
                    bitacoraBLL.RegistrarEvento(bitacora);

                    transaccion.Complete();
                }
                return pago;
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText5"), ex);
            }
        }
    
        // ===================== CUN-05 Informe de vencimientos =====================
        // Socios con membresía vencida o que vence dentro de los próximos "diasAviso" días.
        public List<_012IP_VencimientoSocioBE> _012IP_ListarVencimientos(int diasAviso)
        {
            try
            {
                var todos = suscripcionDAL._012IP_ListarSuscripciones();
                var lista = new List<_012IP_VencimientoSocioBE>();
                foreach (var v in todos)
                    if (v.DiasRestantes <= diasAviso) lista.Add(v);
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("InfMemError"), ex);
            }
        }
    }
}
