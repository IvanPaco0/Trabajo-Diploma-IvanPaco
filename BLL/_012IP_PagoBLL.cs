using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        public static bool _012IP_EsTarjeta(string medioDePago)
        {
            return (medioDePago ?? "").Trim().StartsWith("Tarjeta", StringComparison.OrdinalIgnoreCase);
        }

        public _012IP_TarjetaBE _012IP_ValidarTarjeta(string numero, string vencimiento, string cvv)
        {
            var r = new _012IP_TarjetaBE();
            string digitos = new string((numero ?? "").Where(char.IsDigit).ToArray());

            if (digitos.Length < 13 || digitos.Length > 19 || !_012IP_VerificarLuhn(digitos))
            { r.Motivo = "TarjetaNumeroInvalido"; return r; }

            string marca = _012IP_ObtenerMarca(digitos);
            if (marca.Length == 0)
            { r.Motivo = "TarjetaMarcaDesconocida"; return r; }

            if (!_012IP_ValidarVencimiento(vencimiento))
            { r.Motivo = "TarjetaVencida"; return r; }

            if (!_012IP_ValidarFormatoCVV(cvv, marca))
            { r.Motivo = "TarjetaCVVInvalido"; return r; }

            r.Valida = true;
            r.Marca = marca;
            r.UltimosDigitos = digitos.Substring(digitos.Length - 4);
            return r;
        }

        public bool _012IP_VerificarLuhn(string digitos)
        {
            int suma = 0;
            bool duplicar = false;
            for (int i = digitos.Length - 1; i >= 0; i--)
            {
                int d = digitos[i] - '0';
                if (duplicar) { d *= 2; if (d > 9) d -= 9; }
                suma += d;
                duplicar = !duplicar;
            }
            return suma % 10 == 0;
        }

        public string _012IP_ObtenerMarca(string digitos)
        {
            if (digitos.StartsWith("589562")) return "Naranja";
            if (digitos.StartsWith("589657") || digitos.StartsWith("6042")) return "Cabal";
            if (digitos.StartsWith("34") || digitos.StartsWith("37")) return "American Express";
            if (digitos.StartsWith("4")) return "Visa";
            int p2 = int.Parse(digitos.Substring(0, 2));
            int p4 = int.Parse(digitos.Substring(0, 4));
            if ((p2 >= 51 && p2 <= 55) || (p4 >= 2221 && p4 <= 2720)) return "Mastercard";
            if (p2 == 50 || (p2 >= 56 && p2 <= 69)) return "Maestro";
            return "";
        }

        public bool _012IP_ValidarVencimiento(string vencimiento)
        {
            DateTime fecha;
            string v = (vencimiento ?? "").Trim().Replace("-", "/");
            if (v.Length == 4 && v.All(char.IsDigit)) v = v.Substring(0, 2) + "/" + v.Substring(2);
            if (!DateTime.TryParseExact(v, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                return false;
            DateTime finDeMes = new DateTime(fecha.Year, fecha.Month, 1).AddMonths(1).AddDays(-1);
            return finDeMes >= DateTime.Today;
        }

        public bool _012IP_ValidarFormatoCVV(string cvv, string marca)
        {
            cvv = (cvv ?? "").Trim();
            int largo = marca == "American Express" ? 4 : 3;
            return cvv.Length == largo && cvv.All(char.IsDigit);
        }

        public _012IP_InformeMembresiaBE _012IP_ObtenerRecaudacion(DateTime desde, DateTime hasta)
        {
            if (hasta.Date < desde.Date)
                throw new Exception(LanguageManager.Instance.GetTraduction("InfMemRangoInvalido"));

            try
            {
                List<_012IP_PagoBE> pagos = pagoDAL._012IP_ListarPagos(desde, hasta);
                var informe = new _012IP_InformeMembresiaBE
                {
                    Desde = desde.Date,
                    Hasta = hasta.Date,
                    CantidadPagos = pagos.Count,
                    TotalRecaudado = pagos.Sum(p => p.Monto)
                };
                informe.PorMedioDePago = pagos.GroupBy(p => p.MedioDePago)
                    .Select(g => new _012IP_ResumenRecaudacionBE { Concepto = g.Key, Cantidad = g.Count(), Importe = g.Sum(p => p.Monto) })
                    .OrderByDescending(x => x.Importe).ToList();
                informe.PorMembresia = pagos.GroupBy(p => p.NombreMembresia.Length > 0 ? p.NombreMembresia : "-")
                    .Select(g => new _012IP_ResumenRecaudacionBE { Concepto = g.Key, Cantidad = g.Count(), Importe = g.Sum(p => p.Monto) })
                    .OrderByDescending(x => x.Importe).ToList();

                Bitacora bitacora = new Bitacora
                {
                    Login = SessionManager.Instance.UsuarioActual().Username,
                    Modulo = "Reportes",
                    Evento = $"Emitir informe de recaudación {desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}",
                    Criticidad = 4
                };
                bitacoraBLL.RegistrarEvento(bitacora);
                return informe;
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("InfMemError"), ex);
            }
        }

        public _012IP_PagoBE _012IP_RegistrarPago(int idSocio, int? idCuota, int? idMembresia, decimal monto, string medioDePago,
            string? marcaTarjeta = null, string? ultimosDigitos = null)
        {
            medioDePago = (medioDePago ?? "").Trim();

            if (!_012IP_ValidarDatosPago(idSocio, idCuota, monto, medioDePago))
                throw new Exception(LanguageManager.Instance.GetTraduction("PagoBLLText1"));

            if (_012IP_EsTarjeta(medioDePago) && (string.IsNullOrWhiteSpace(marcaTarjeta) || string.IsNullOrWhiteSpace(ultimosDigitos)))
                throw new Exception(LanguageManager.Instance.GetTraduction("TarjetaRequerida"));

            var pago = new _012IP_PagoBE
            {
                IdSocio = idSocio,
                IdCuota = idCuota,
                IdMembresia = idMembresia,
                Monto = monto,
                MedioDePago = medioDePago,
                FechaPago = DateTime.Now,
                MarcaTarjeta = _012IP_EsTarjeta(medioDePago) ? marcaTarjeta : null,
                UltimosDigitos = _012IP_EsTarjeta(medioDePago) ? ultimosDigitos : null
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