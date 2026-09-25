using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BLL
{
    public class _012IP_SuscripcionBLL
    {
        private readonly _012IP_SuscripcionDAL suscripcionDAL;
        private readonly _012IP_CuotaBLL cuotaBLL;
        private readonly BitacoraBLL bitacoraBLL;

        public _012IP_SuscripcionBLL()
        {
            suscripcionDAL = new _012IP_SuscripcionDAL();
            cuotaBLL = new _012IP_CuotaBLL();
            bitacoraBLL = new BitacoraBLL();
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

       
        public void _012IP_CalcularPeriodoRenovacion(_012IP_SuscripcionBE actual, out DateTime fechaInicio, out DateTime fechaVencimiento)
        {
            DateTime hoy = DateTime.Today;

            if (actual != null && actual.FechaVencimiento.Date >= hoy)
                fechaInicio = actual.FechaVencimiento.Date;
            else
                fechaInicio = hoy;

            fechaVencimiento = fechaInicio.AddMonths(1);
        }

       
        public void _012IP_RenovarSuscripcion(string dni, int idMembresia, DateTime fechaInicio, DateTime fechaVencimiento)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception(LanguageManager.Instance.GetTraduction("DNIRequerido"));
            if (idMembresia <= 0)
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText2"));
            if (fechaVencimiento.Date <= fechaInicio.Date)
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText3"));

            try
            {
                suscripcionDAL._012IP_ActualizarSuscripcion(dni.Trim(), idMembresia, fechaInicio, fechaVencimiento);

                Bitacora bitacora = new Bitacora();
                bitacora.Login = SessionManager.Instance.UsuarioActual().Username;
                bitacora.Modulo = "Membresias";
                bitacora.Evento = $"Renovar membresia - DNI {dni.Trim()} - membresia {idMembresia} - vence {fechaVencimiento:dd/MM/yyyy}";
                bitacora.Criticidad = 2;
                bitacoraBLL.RegistrarEvento(bitacora);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SuscBLLText5"), ex);
            }
        }
    }
}
