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

        /// <summary>
        /// ValidarDatosPago(IdSocio, IdCuota, Monto): CU-03 paso 3 (loop "datos válidos").
        /// Es una validación de formato/rango, no toca la base (igual que ValidarFormato en
        /// SocioBLL): idSocio válido, monto positivo y, si viene, idCuota positivo, y el
        /// medio de pago debe ser uno de los admitidos.
        /// </summary>
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

        /// <summary>
        /// RegistrarPago(IdSocio, IdCuota, IdMembresia, Monto, MedioDePago): CU-03 pasos 5-12.
        /// Arma el pago, calcula el DVH (antes de persistir), lo registra, marca la cuota
        /// como pagada (si corresponde) y recién ahí escribe el evento en Bitácora.
        /// Si algo falla no llega a tocar la Bitácora (flujo alternativo: excepción -> la
        /// GUI informa el error y el recepcionista puede reintentar).
        /// </summary>
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

            // 1° se calcula el DVH (CalcularDVH(Pago), paso 6 del diagrama)...
            string cadenaDVH = pago.IdSocio + (pago.IdCuota?.ToString() ?? "") + (pago.IdMembresia?.ToString() ?? "") +
                                pago.Monto + pago.MedioDePago + pago.FechaPago.ToString("yyyy-MM-dd HH:mm:ss");
            string dvh = DigitoVerificador.CalcularDVH(cadenaDVH);

            bool registrado;
            try
            {
                // 2° recién acá se persiste, pago y dvh juntos en el mismo INSERT (paso 7-8)
                registrado = pagoDAL._012IP_RegistrarPago(pago, dvh);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText2"), ex);
            }

            if (!registrado)
                throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText2"));

            pago.DVH = dvh;

            // Paso 9: si el pago cancela una cuota existente (regularización de deuda),
            // se marca como Pagada. En una renovación nueva no hay cuota todavía y se omite.
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

            // El BLL llama a Bitácora directamente, nunca la GUI (cohesión/acoplamiento).
            // Pasos 11-12 del diagrama (RegistrarEvento).
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

        /// <summary>
        /// Uso desde "Regularizar deuda" (CU-01): cuando el pago cubre VARIAS cuotas
        /// vencidas a la vez, se registra un único Pago por el total (IdCuota null, porque
        /// no corresponde a una sola fila de Cuota) y acá se marcan todas esas cuotas como
        /// Pagada, con la misma fecha del pago recién confirmado.
        /// </summary>
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