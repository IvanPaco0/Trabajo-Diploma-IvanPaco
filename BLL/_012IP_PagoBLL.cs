using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;

namespace _012IP_BLL
{
    public class _012IP_PagoBLL
    {
        private readonly _012IP_PagoDAL pagoDAL;
        private readonly BitacoraBLL bitacoraBLL;

        private static readonly string[] MediosDePagoValidos =
            { "Efectivo", "Tarjeta de débito", "Tarjeta de crédito", "Transferencia" };

        public _012IP_PagoBLL()
        {
            pagoDAL = new _012IP_PagoDAL();
            bitacoraBLL = new BitacoraBLL();
        }

        public bool _012IP_ValidarDatosPago(int idSocio, int? idCuota, decimal monto, string medioDePago)
        {
            if (idSocio <= 0)
                return false;

            if (idCuota.HasValue && idCuota.Value <= 0)
                return false;

            if (monto <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(medioDePago) || Array.IndexOf(MediosDePagoValidos, medioDePago.Trim()) < 0)
                return false;

            return true;
        }

        public _012IP_PagoBE _012IP_RegistrarPago(int idSocio, int? idCuota, int? idMembresia, decimal monto, string medioDePago)
        {
            medioDePago = (medioDePago ?? "").Trim();

            if (!_012IP_ValidarDatosPago(idSocio, idCuota, monto, medioDePago))
                throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText1"));

            var pago = new _012IP_PagoBE
            {
                IdSocio = idSocio,
                IdCuota = idCuota,
                IdMembresia = idMembresia,
                Monto = monto,
                MedioDePago = medioDePago,
                FechaPago = DateTime.Now
            };

            string cadenaDVH = pago.IdSocio + (pago.IdCuota?.ToString() ?? "") + (pago.IdMembresia?.ToString() ?? "") +
                                pago.Monto + pago.MedioDePago + pago.FechaPago.ToString("yyyy-MM-dd HH:mm:ss");
            string dvh = DigitoVerificador.CalcularDVH(cadenaDVH);

            bool registrado;
            try
            {
                registrado = pagoDAL._012IP_RegistrarPago(pago, dvh);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText2"), ex);
            }

            if (!registrado)
                throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText2"));

            pago.DVH = dvh;

            if (pago.IdCuota.HasValue)
            {
                try
                {
                    pagoDAL._012IP_ActualizarEstadoPago(pago.IdCuota.Value, pago.FechaPago);
                }
                catch (Exception ex)
                {
                    throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText3"), ex);
                }
            }

            Bitacora bitacora = new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Modulo = "Pagos",
                Evento = $"Registrar pago - Socio {pago.IdSocio} - {_012IP_Moneda(pago.Monto)} - {pago.MedioDePago}",
                Criticidad = 2
            };
            bitacoraBLL.RegistrarEvento(bitacora);

            return pago;
        }

        public void _012IP_MarcarCuotasComoPagadas(List<int> idsCuota, DateTime fechaPago)
        {
            if (idsCuota == null) return;

            try
            {
                foreach (int idCuota in idsCuota)
                    pagoDAL._012IP_ActualizarEstadoPago(idCuota, fechaPago);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText3"), ex);
            }
        }

        private string _012IP_Moneda(decimal monto) => "$" + monto.ToString("N0");
    }
}