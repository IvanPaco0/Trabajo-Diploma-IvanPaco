using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _012IP_BLL
{
    public class _012IP_AuditoriaBLL
    {
        public const string RES_FALTANTE = "Faltante";
        public const string RES_FUERA_DE_LUGAR = "FueraDeLugar";
        public const string RES_DESCONOCIDO = "Desconocido";

        public const string RESOL_BAJA = "Baja";
        public const string RESOL_REUBICADO = "Reubicado";

        private const string MODULO_BITACORA = "Inventario";
        private const string MOTIVO_PERDIDA = "Pérdida detectada en auditoría";

        private readonly _012IP_EjemplarDAL ejemplarDAL;
        private readonly _012IP_AuditoriaDAL auditoriaDAL;
        private readonly _012IP_EjemplarBLL ejemplarBLL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly DigitoVerificadorBLL digitoVerificadorBLL;

        public _012IP_AuditoriaBLL()
        {
            ejemplarDAL = new _012IP_EjemplarDAL();
            auditoriaDAL = new _012IP_AuditoriaDAL();
            ejemplarBLL = new _012IP_EjemplarBLL();
            bitacoraBLL = new BitacoraBLL();
            digitoVerificadorBLL = new DigitoVerificadorBLL();
        }

        // ------------------------------------------------------------------
        //  Utilidades
        // ------------------------------------------------------------------

        /// <summary>DVH de la auditoria. Unica fuente de verdad: la usan el cierre y los digitos verificadores.</summary>
        public static string _012IP_CalcularDVH(_012IP_AuditoriaBE a)
        {
            return DigitoVerificador.CalcularDVH(
                a.Fecha.ToString("yyyy-MM-dd HH:mm:ss") + a.Seccion + a.Responsable +
                a.Esperados + a.Encontrados + a.Faltantes + a.Sobrantes);
        }

        public static string _012IP_TextoResultado(string resultado)
        {
            return LanguageManager.Instance.GetTraduction("AudRes" + resultado);
        }

        public static string _012IP_TextoResolucion(_012IP_DiferenciaAuditoriaBE d)
        {
            if (d.Resultado != RES_FALTANTE)
                return "-";

            switch (d.Resolucion)
            {
                case RESOL_BAJA: return LanguageManager.Instance.GetTraduction("AudResolBaja");
                case RESOL_REUBICADO: return LanguageManager.Instance.GetTraduction("AudResolReubicado");
                default: return LanguageManager.Instance.GetTraduction("AudResolPendiente");
            }
        }

        private static string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private static DateTime _012IP_Ahora()
        {
            DateTime ahora = DateTime.Now;
            return ahora.AddTicks(-(ahora.Ticks % TimeSpan.TicksPerSecond));
        }

        private void _012IP_RegistrarBitacora(string evento, int criticidad)
        {
            Bitacora bitacora = new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Modulo = MODULO_BITACORA,
                Evento = evento,
                Criticidad = criticidad
            };
            bitacoraBLL.RegistrarEvento(bitacora);
        }

        // ------------------------------------------------------------------
        //  Listado esperado
        // ------------------------------------------------------------------

        public List<string> _012IP_ObtenerSecciones()
        {
            try
            {
                return ejemplarDAL._012IP_ObtenerSecciones();
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("AudBLLErrorListado"), ex);
            }
        }

        public List<_012IP_EjemplarBE> _012IP_GenerarListadoEsperado(string seccion)
        {
            seccion = (seccion ?? "").Trim();
            if (seccion.Length == 0)
                throw new Exception(_012IP_Texto("AudBLLSeccion"));

            try
            {
                return ejemplarDAL._012IP_ObtenerEsperadosPorSeccion(seccion);
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("AudBLLErrorListado"), ex);
            }
        }

        // ------------------------------------------------------------------
        //  Conciliacion: lo esperado vs. lo efectivamente encontrado
        // ------------------------------------------------------------------

        public _012IP_ConciliacionBE _012IP_Conciliar(string seccion, IEnumerable<string> codigosEncontrados)
        {
            seccion = (seccion ?? "").Trim();
            if (seccion.Length == 0)
                throw new Exception(_012IP_Texto("AudBLLSeccion"));

            var encontrados = new HashSet<string>(
                (codigosEncontrados ?? Enumerable.Empty<string>())
                    .Select(c => (c ?? "").Trim())
                    .Where(c => c.Length > 0),
                StringComparer.OrdinalIgnoreCase);

            if (encontrados.Count == 0)
                throw new Exception(_012IP_Texto("AudBLLSinEncontrados"));

            try
            {
                List<_012IP_EjemplarBE> esperados = ejemplarDAL._012IP_ObtenerEsperadosPorSeccion(seccion);
                var codigosEsperados = new HashSet<string>(esperados.Select(e => e.CodigoBarras), StringComparer.OrdinalIgnoreCase);

                var conciliacion = new _012IP_ConciliacionBE
                {
                    Seccion = seccion,
                    Esperados = esperados.Count,
                    TotalEscaneados = encontrados.Count
                };

                foreach (string codigo in encontrados)
                {
                    if (codigosEsperados.Contains(codigo))
                    {
                        conciliacion.Correctos++;
                        continue;
                    }

                    _012IP_EjemplarBE? registrado = ejemplarDAL._012IP_ObtenerPorCodigo(codigo);
                    if (registrado == null)
                    {
                        conciliacion.Diferencias.Add(new _012IP_DiferenciaAuditoriaBE
                        {
                            CodigoBarras = codigo,
                            Resultado = RES_DESCONOCIDO
                        });
                    }
                    else
                    {
                        conciliacion.Diferencias.Add(new _012IP_DiferenciaAuditoriaBE
                        {
                            CodigoBarras = registrado.CodigoBarras,
                            NumeroInventario = registrado.NumeroInventario,
                            Titulo = registrado.Titulo,
                            Resultado = RES_FUERA_DE_LUGAR,
                            Detalle = _012IP_EjemplarBLL._012IP_NombreEstado(registrado.Estado) + " | " + _012IP_EjemplarBLL._012IP_TextoUbicacion(registrado)
                        });
                    }
                }

                foreach (_012IP_EjemplarBE esperado in esperados.Where(e => !encontrados.Contains(e.CodigoBarras)))
                {
                    conciliacion.Diferencias.Add(new _012IP_DiferenciaAuditoriaBE
                    {
                        CodigoBarras = esperado.CodigoBarras,
                        NumeroInventario = esperado.NumeroInventario,
                        Titulo = esperado.Titulo,
                        Resultado = RES_FALTANTE,
                        Detalle = _012IP_EjemplarBLL._012IP_TextoUbicacion(esperado)
                    });
                }

                return conciliacion;
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("AudBLLErrorConciliar"), ex);
            }
        }

        // ------------------------------------------------------------------
        //  Decision ante un faltante: baja por perdida o reubicacion
        // ------------------------------------------------------------------

        private static void _012IP_ValidarResoluble(_012IP_DiferenciaAuditoriaBE diferencia)
        {
            if (diferencia == null || diferencia.Resultado != RES_FALTANTE || diferencia.Resolucion != "")
                throw new Exception(_012IP_Texto("AudBLLNoResoluble"));
        }

        public void _012IP_ResolverFaltanteBaja(_012IP_DiferenciaAuditoriaBE diferencia)
        {
            _012IP_ValidarResoluble(diferencia);

            // La baja registra su propio evento en la bitacora (criticidad alta) y recalcula los DV.
            ejemplarBLL._012IP_DarDeBaja(diferencia.CodigoBarras, MOTIVO_PERDIDA);
            diferencia.Resolucion = RESOL_BAJA;
        }

        public void _012IP_ResolverFaltanteReubicar(_012IP_DiferenciaAuditoriaBE diferencia, string pasillo, string seccion, string estanteria)
        {
            _012IP_ValidarResoluble(diferencia);

            ejemplarBLL._012IP_UbicarEjemplar(diferencia.CodigoBarras, pasillo, seccion, estanteria);
            diferencia.Resolucion = RESOL_REUBICADO;
        }

        // ------------------------------------------------------------------
        //  Cierre e informe
        // ------------------------------------------------------------------

        public _012IP_AuditoriaBE _012IP_CerrarAuditoria(_012IP_ConciliacionBE conciliacion)
        {
            if (conciliacion == null)
                throw new Exception(_012IP_Texto("AudBLLErrorCerrar"));

            var auditoria = new _012IP_AuditoriaBE
            {
                Fecha = _012IP_Ahora(),
                Seccion = conciliacion.Seccion,
                Responsable = SessionManager.Instance.UsuarioActual().Username,
                Esperados = conciliacion.Esperados,
                Encontrados = conciliacion.TotalEscaneados,
                Faltantes = conciliacion.Faltantes,
                Sobrantes = conciliacion.Sobrantes
            };

            string dvh = _012IP_CalcularDVH(auditoria);

            bool registrada;
            try
            {
                registrada = auditoriaDAL._012IP_RegistrarAuditoria(auditoria, dvh);
                if (registrada)
                    digitoVerificadorBLL.RecalcularDVV_AuditoriaInventario();
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("AudBLLErrorCerrar"), ex);
            }

            if (!registrada)
                throw new Exception(_012IP_Texto("AudBLLErrorCerrar"));

            auditoria.DVH = dvh;

            _012IP_RegistrarBitacora(
                $"Auditoría de inventario cerrada - Sección {auditoria.Seccion} - Esperados {auditoria.Esperados} - Faltantes {auditoria.Faltantes} ({conciliacion.FaltantesSinResolver} sin resolver) - Sobrantes {auditoria.Sobrantes}",
                2);

            return auditoria;
        }

        // CUN-10: emite el informe de inventario y patrimonio de una auditoría cerrada.
        // Calcula el stock actual y registra la emisión en la bitácora.
        public _012IP_StockBE _012IP_EmitirInforme(_012IP_AuditoriaBE auditoria)
        {
            if (auditoria == null || auditoria.IdAuditoria == 0 && string.IsNullOrEmpty(auditoria.DVH))
                throw new Exception(_012IP_Texto("InfInvSinCerrar"));

            _012IP_StockBE stock = ejemplarBLL._012IP_ObtenerStock();

            _012IP_RegistrarBitacora(
                $"Emitir informe de inventario y patrimonio - Sección {auditoria.Seccion}", 4);
            return stock;
        }

        public string _012IP_GenerarInforme(_012IP_AuditoriaBE auditoria, _012IP_ConciliacionBE conciliacion, _012IP_StockBE stock)
        {
            var sb = new StringBuilder(_012IP_GenerarInforme(auditoria, conciliacion));
            if (stock != null)
            {
                sb.AppendLine();
                sb.AppendLine(_012IP_Texto("InfInvStock"));
                sb.AppendLine(new string('-', 60));
                sb.AppendLine(_012IP_Texto("InfInvTotal") + ": " + stock.Total);
                sb.AppendLine(_012IP_Texto("InfInvDisponibles") + ": " + stock.Disponibles);
                sb.AppendLine(_012IP_Texto("InfInvDeteriorados") + ": " + stock.Deteriorados);
                sb.AppendLine(_012IP_Texto("InfInvRestauracion") + ": " + stock.EnRestauracion);
                sb.AppendLine(_012IP_Texto("InfInvBajas") + ": " + stock.Bajas);
                sb.AppendLine(_012IP_Texto("InfInvPendientes") + ": " + stock.PendientesUbicar);
            }
            return sb.ToString();
        }

        public string _012IP_GenerarInforme(_012IP_AuditoriaBE auditoria, _012IP_ConciliacionBE conciliacion)
        {
            var sb = new StringBuilder();
            sb.AppendLine(_012IP_Texto("AudInfTitulo"));
            sb.AppendLine(new string('=', 60));
            sb.AppendLine(_012IP_Texto("AudInfFecha") + ": " + auditoria.Fecha.ToString("dd/MM/yyyy HH:mm"));
            sb.AppendLine(_012IP_Texto("AudInfSeccion") + ": " + auditoria.Seccion);
            sb.AppendLine(_012IP_Texto("AudInfUsuario") + ": " + auditoria.Responsable);
            sb.AppendLine();
            sb.AppendLine(_012IP_Texto("AudInfEsperados") + ": " + auditoria.Esperados);
            sb.AppendLine(_012IP_Texto("AudInfCorrectos") + ": " + conciliacion.Correctos);
            sb.AppendLine(_012IP_Texto("AudInfFaltantes") + ": " + auditoria.Faltantes);
            sb.AppendLine(_012IP_Texto("AudInfSobrantes") + ": " + auditoria.Sobrantes);
            sb.AppendLine();
            sb.AppendLine(_012IP_Texto("AudInfDiferencias"));
            sb.AppendLine(new string('-', 60));

            if (conciliacion.Diferencias.Count == 0)
            {
                sb.AppendLine(_012IP_Texto("AudInfSinDiferencias"));
            }
            else
            {
                foreach (_012IP_DiferenciaAuditoriaBE d in conciliacion.Diferencias)
                {
                    sb.AppendLine(
                        "[" + _012IP_TextoResultado(d.Resultado) + "] " + d.CodigoBarras +
                        (d.Titulo.Length > 0 ? " - " + d.Titulo : "") +
                        (d.Detalle.Length > 0 ? " (" + d.Detalle + ")" : "") +
                        " -> " + _012IP_TextoResolucion(d));
                }
            }

            return sb.ToString();
        }
    }
}