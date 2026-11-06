using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _012IP_BLL
{
    public class _012IP_EjemplarBLL
    {
        public const string ESTADO_BUENO = "Bueno";
        public const string ESTADO_DETERIORADO = "Deteriorado";
        public const string ESTADO_RESTAURACION = "EnRestauracion";
        public const string ESTADO_BAJA = "Baja";

        public const string ORIGEN_COMPRA = "Compra";
        public const string ORIGEN_DONACION = "Donacion";

        private const string MODULO_BITACORA = "Inventario";
        private const string RAIZ_XML = "Ejemplares";
        private const int MAX_MOTIVO = 100;
        private const int MAX_EVENTO = 200;

        private readonly _012IP_EjemplarDAL ejemplarDAL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly DigitoVerificadorBLL digitoVerificadorBLL;

        public _012IP_EjemplarBLL()
        {
            ejemplarDAL = new _012IP_EjemplarDAL();
            bitacoraBLL = new BitacoraBLL();
            digitoVerificadorBLL = new DigitoVerificadorBLL();
        }

        // ------------------------------------------------------------------
        //  Utilidades (publicas / estaticas: las usan la GUI y los digitos verificadores)
        // ------------------------------------------------------------------

        /// <summary>DVH del ejemplar. Unica fuente de verdad: la usan el alta, las modificaciones y la auditoria de DV.</summary>
        public static string _012IP_CalcularDVH(_012IP_EjemplarBE e)
        {
            return DigitoVerificador.CalcularDVH(
                e.CodigoBarras + e.NumeroInventario + e.Titulo + e.Autor + e.Origen +
                e.Estado + e.Pasillo + e.Seccion + e.Estanteria);
        }

        public static string _012IP_NombreEstado(string estado)
        {
            return LanguageManager.Instance.GetTraduction("InvEstado" + estado);
        }

        public static string _012IP_NombreOrigen(string origen)
        {
            return LanguageManager.Instance.GetTraduction("InvOrigen" + origen);
        }

        public static string _012IP_TextoUbicacion(_012IP_EjemplarBE e)
        {
            if (!e.EstaUbicado)
                return LanguageManager.Instance.GetTraduction("InvPendienteUbicar");

            return e.Pasillo + " / " + e.Seccion + " / " + e.Estanteria;
        }

        /// <summary>Estados a los que puede pasar un ejemplar segun su estado actual (Baja incluida; es irreversible).</summary>
        public static List<string> _012IP_EstadosPermitidos(string estadoActual)
        {
            switch (estadoActual)
            {
                case ESTADO_BUENO:
                    return new List<string> { ESTADO_DETERIORADO, ESTADO_BAJA };
                case ESTADO_DETERIORADO:
                    return new List<string> { ESTADO_RESTAURACION, ESTADO_BAJA };
                case ESTADO_RESTAURACION:
                    return new List<string> { ESTADO_BUENO, ESTADO_BAJA };
                default:
                    return new List<string>();
            }
        }

        private static string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private static DateTime _012IP_Ahora()
        {
            // Se trunca a segundos para que el valor guardado en SQL Server no cambie al releerlo.
            DateTime ahora = DateTime.Now;
            return ahora.AddTicks(-(ahora.Ticks % TimeSpan.TicksPerSecond));
        }

        private static string _012IP_Recortar(string texto, int maximo)
        {
            if (texto == null) return string.Empty;
            return texto.Length <= maximo ? texto : texto.Substring(0, maximo);
        }

        private static string _012IP_EstadoBitacora(string estado)
        {
            return estado == ESTADO_RESTAURACION ? "En Restauración" : estado;
        }

        private void _012IP_RegistrarBitacora(string evento, int criticidad)
        {
            Bitacora bitacora = new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Modulo = MODULO_BITACORA,
                Evento = _012IP_Recortar(evento, MAX_EVENTO),
                Criticidad = criticidad
            };
            bitacoraBLL.RegistrarEvento(bitacora);
        }

        // ------------------------------------------------------------------
        //  Validaciones
        // ------------------------------------------------------------------

        public bool _012IP_ValidarFormato(string codigoBarras, string numeroInventario, string titulo, string autor, string origen)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras) || !Regex.IsMatch(codigoBarras.Trim(), @"^[0-9]{8,13}$"))
                return false;

            if (string.IsNullOrWhiteSpace(numeroInventario) || !Regex.IsMatch(numeroInventario.Trim(), @"^[A-Za-z0-9\-]{3,20}$"))
                return false;

            if (string.IsNullOrWhiteSpace(titulo) || titulo.Trim().Length > 150)
                return false;

            if (string.IsNullOrWhiteSpace(autor) || autor.Trim().Length > 100)
                return false;

            if (origen != ORIGEN_COMPRA && origen != ORIGEN_DONACION)
                return false;

            return true;
        }

        public bool _012IP_ValidarUbicacion(string pasillo, string seccion, string estanteria)
        {
            const string patron = @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ0-9 \-\.]+$";

            if (string.IsNullOrWhiteSpace(pasillo) || pasillo.Trim().Length > 20 || !Regex.IsMatch(pasillo.Trim(), patron))
                return false;

            if (string.IsNullOrWhiteSpace(seccion) || seccion.Trim().Length > 30 || !Regex.IsMatch(seccion.Trim(), patron))
                return false;

            if (string.IsNullOrWhiteSpace(estanteria) || estanteria.Trim().Length > 20 || !Regex.IsMatch(estanteria.Trim(), patron))
                return false;

            return true;
        }

        /// <summary>Estado nuevo valido segun el ciclo de vida: Bueno -> Deteriorado -> En Restauracion -> Bueno, y cualquiera -> Baja.</summary>
        public bool _012IP_TransicionPermitida(string estadoActual, string nuevoEstado)
        {
            return _012IP_EstadosPermitidos(estadoActual).Contains(nuevoEstado);
        }

        private void _012IP_ValidarDuplicados(string codigoBarras, string numeroInventario)
        {
            bool codigoExiste;
            bool inventarioExiste;

            try
            {
                codigoExiste = ejemplarDAL._012IP_ExisteCodigoBarras(codigoBarras);
                inventarioExiste = ejemplarDAL._012IP_ExisteNumeroInventario(numeroInventario);
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLErrorRegistrar"), ex);
            }

            if (codigoExiste)
                throw new Exception(_012IP_Texto("InvBLLCodigoDuplicado"));

            if (inventarioExiste)
                throw new Exception(_012IP_Texto("InvBLLInventarioDuplicado"));
        }

        // ------------------------------------------------------------------
        //  Consultas
        // ------------------------------------------------------------------

        public List<_012IP_EjemplarBE> _012IP_ListarEjemplares(string filtro, string texto)
        {
            try
            {
                return ejemplarDAL._012IP_Listar(filtro, texto);
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLErrorConsulta"), ex);
            }
        }

        public _012IP_StockBE _012IP_ObtenerStock()
        {
            try
            {
                return ejemplarDAL._012IP_ObtenerStock();
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLErrorConsulta"), ex);
            }
        }

        private _012IP_EjemplarBE _012IP_ObtenerExistente(string codigoBarras)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                throw new Exception(_012IP_Texto("InvBLLNoExiste"));

            _012IP_EjemplarBE? ejemplar;
            try
            {
                ejemplar = ejemplarDAL._012IP_ObtenerPorCodigo(codigoBarras.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLErrorConsulta"), ex);
            }

            if (ejemplar == null)
                throw new Exception(_012IP_Texto("InvBLLNoExiste"));

            return ejemplar;
        }

        // ------------------------------------------------------------------
        //  PN2.1 - Registrar ingreso (alta)
        // ------------------------------------------------------------------

        public _012IP_EjemplarBE _012IP_RegistrarIngreso(string codigoBarras, string numeroInventario, string titulo, string autor, string origen)
        {
            codigoBarras = (codigoBarras ?? "").Trim();
            numeroInventario = (numeroInventario ?? "").Trim();
            titulo = (titulo ?? "").Trim();
            autor = (autor ?? "").Trim();
            origen = (origen ?? "").Trim();

            // PN2.4 - validacion automatica antes de guardar
            if (!_012IP_ValidarFormato(codigoBarras, numeroInventario, titulo, autor, origen))
                throw new Exception(_012IP_Texto("InvBLLFormato"));

            _012IP_ValidarDuplicados(codigoBarras, numeroInventario);

            var ejemplar = new _012IP_EjemplarBE
            {
                CodigoBarras = codigoBarras,
                NumeroInventario = numeroInventario,
                Titulo = titulo,
                Autor = autor,
                Origen = origen,
                Estado = ESTADO_BUENO,
                Pasillo = string.Empty,
                Seccion = string.Empty,
                Estanteria = string.Empty,
                FechaIngreso = _012IP_Ahora()
            };

            string dvh = _012IP_CalcularDVH(ejemplar);

            bool registrado;
            try
            {
                registrado = ejemplarDAL._012IP_RegistrarEjemplar(ejemplar, dvh);
                if (registrado)
                    digitoVerificadorBLL.RecalcularDVV_Ejemplar();
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLErrorRegistrar"), ex);
            }

            if (!registrado)
                throw new Exception(_012IP_Texto("InvBLLErrorRegistrar"));

            ejemplar.DVH = dvh;

            _012IP_RegistrarBitacora(
                $"Ingreso de ejemplar - Código {ejemplar.CodigoBarras} - Inv. {ejemplar.NumeroInventario} - {_012IP_Recortar(ejemplar.Titulo, 40)} - Origen {ejemplar.Origen}",
                2);

            return ejemplar;
        }

        // ------------------------------------------------------------------
        //  PN2.2 - Ubicacion topografica
        // ------------------------------------------------------------------

        public _012IP_EjemplarBE _012IP_UbicarEjemplar(string codigoBarras, string pasillo, string seccion, string estanteria)
        {
            pasillo = (pasillo ?? "").Trim();
            seccion = (seccion ?? "").Trim();
            estanteria = (estanteria ?? "").Trim();

            _012IP_EjemplarBE ejemplar = _012IP_ObtenerExistente(codigoBarras);

            if (ejemplar.Estado == ESTADO_BAJA)
                throw new Exception(_012IP_Texto("InvBLLBajaInmutable"));

            if (!_012IP_ValidarUbicacion(pasillo, seccion, estanteria))
                throw new Exception(_012IP_Texto("InvBLLUbicacionInvalida"));

            if (ejemplar.Pasillo == pasillo && ejemplar.Seccion == seccion && ejemplar.Estanteria == estanteria)
                throw new Exception(_012IP_Texto("InvBLLUbicacionIgual"));

            string anterior = ejemplar.EstaUbicado
                ? ejemplar.Pasillo + "/" + ejemplar.Seccion + "/" + ejemplar.Estanteria
                : "Sin ubicar";

            ejemplar.Pasillo = pasillo;
            ejemplar.Seccion = seccion;
            ejemplar.Estanteria = estanteria;
            string dvh = _012IP_CalcularDVH(ejemplar);

            bool actualizado;
            try
            {
                actualizado = ejemplarDAL._012IP_ActualizarUbicacion(ejemplar.CodigoBarras, pasillo, seccion, estanteria, dvh);
                if (actualizado)
                    digitoVerificadorBLL.RecalcularDVV_Ejemplar();
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLErrorUbicar"), ex);
            }

            if (!actualizado)
                throw new Exception(_012IP_Texto("InvBLLErrorUbicar"));

            ejemplar.DVH = dvh;

            _012IP_RegistrarBitacora(
                $"Ubicar ejemplar - Código {ejemplar.CodigoBarras} - {anterior} -> {pasillo}/{seccion}/{estanteria}",
                3);

            return ejemplar;
        }

        // ------------------------------------------------------------------
        //  PN2.3 - Estado fisico y baja (ciclo de vida del ejemplar)
        // ------------------------------------------------------------------

        public _012IP_EjemplarBE _012IP_CambiarEstado(string codigoBarras, string nuevoEstado, string motivo)
        {
            nuevoEstado = (nuevoEstado ?? "").Trim();
            motivo = (motivo ?? "").Trim();

            string[] estadosValidos = { ESTADO_BUENO, ESTADO_DETERIORADO, ESTADO_RESTAURACION, ESTADO_BAJA };
            if (Array.IndexOf(estadosValidos, nuevoEstado) < 0)
                throw new Exception(_012IP_Texto("InvBLLEstadoInvalido"));

            if (motivo.Length == 0 || motivo.Length > MAX_MOTIVO)
                throw new Exception(_012IP_Texto("InvBLLMotivo"));

            _012IP_EjemplarBE ejemplar = _012IP_ObtenerExistente(codigoBarras);

            // La baja es irreversible: no admite ninguna modificacion posterior.
            if (ejemplar.Estado == ESTADO_BAJA)
                throw new Exception(_012IP_Texto("InvBLLBajaInmutable"));

            if (!_012IP_TransicionPermitida(ejemplar.Estado, nuevoEstado))
                throw new Exception(string.Format(
                    _012IP_Texto("InvBLLTransicion"),
                    _012IP_NombreEstado(ejemplar.Estado),
                    _012IP_NombreEstado(nuevoEstado)));

            string estadoAnterior = ejemplar.Estado;
            DateTime ahora = _012IP_Ahora();

            DateTime? fechaEnvio = ejemplar.FechaEnvioRestauracion;
            DateTime? fechaBaja = ejemplar.FechaBaja;

            if (nuevoEstado == ESTADO_RESTAURACION) fechaEnvio = ahora;   // queda no disponible y se registra el envio
            if (nuevoEstado == ESTADO_BUENO) fechaEnvio = null;           // vuelve de restauracion
            if (nuevoEstado == ESTADO_BAJA) fechaBaja = ahora;

            ejemplar.Estado = nuevoEstado;
            string dvh = _012IP_CalcularDVH(ejemplar);

            bool actualizado;
            try
            {
                actualizado = ejemplarDAL._012IP_ActualizarEstado(ejemplar.CodigoBarras, nuevoEstado, fechaEnvio, fechaBaja, dvh);
                if (actualizado)
                    digitoVerificadorBLL.RecalcularDVV_Ejemplar();
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLErrorEstado"), ex);
            }

            if (!actualizado)
                throw new Exception(_012IP_Texto("InvBLLErrorEstado"));

            ejemplar.DVH = dvh;
            ejemplar.FechaEnvioRestauracion = fechaEnvio;
            ejemplar.FechaBaja = fechaBaja;

            string motivoEvento = _012IP_Recortar(motivo, 60);
            if (nuevoEstado == ESTADO_BAJA)
            {
                _012IP_RegistrarBitacora(
                    $"Baja de ejemplar - Código {ejemplar.CodigoBarras} - Estado previo {_012IP_EstadoBitacora(estadoAnterior)} - Motivo: {motivoEvento}",
                    1);
            }
            else
            {
                _012IP_RegistrarBitacora(
                    $"Cambio de estado - Código {ejemplar.CodigoBarras} - {_012IP_EstadoBitacora(estadoAnterior)} -> {_012IP_EstadoBitacora(nuevoEstado)} - Motivo: {motivoEvento}",
                    2);
            }

            return ejemplar;
        }

        public _012IP_EjemplarBE _012IP_DarDeBaja(string codigoBarras, string motivo)
        {
            return _012IP_CambiarEstado(codigoBarras, ESTADO_BAJA, motivo);
        }

        // ------------------------------------------------------------------
        //  Serializacion XML (sin base de datos): lo que hay en la grilla <-> archivo XML
        // ------------------------------------------------------------------

        private static string _012IP_ValidarRutaXml(string ruta, bool debeExistir)
        {
            ruta = (ruta ?? "").Trim();

            if (ruta.Length == 0 || !ruta.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                throw new Exception(_012IP_Texto("InvBLLXmlRuta"));

            if (debeExistir)
            {
                if (!File.Exists(ruta))
                    throw new Exception(_012IP_Texto("InvBLLXmlNoExiste"));
            }
            else
            {
                string carpeta = Path.GetDirectoryName(Path.GetFullPath(ruta));
                if (string.IsNullOrEmpty(carpeta) || !Directory.Exists(carpeta))
                    throw new Exception(_012IP_Texto("InvBLLXmlRuta"));
            }

            return ruta;
        }

        /// <summary>SERIALIZAR: guarda en un XML los ejemplares que se estan mostrando en la grilla.</summary>
        public void _012IP_SerializarEjemplares(List<_012IP_EjemplarBE> ejemplares, string ruta)
        {
            if (ejemplares == null || ejemplares.Count == 0)
                throw new Exception(_012IP_Texto("InvBLLXmlSinDatos"));

            ruta = _012IP_ValidarRutaXml(ruta, false);

            try
            {
                _012IP_SerializadorXmlBLL._012IP_Serializar(ejemplares, ruta, RAIZ_XML);
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLXmlErrorSerializar"), ex);
            }

            _012IP_RegistrarBitacora(
                $"Serializar ejemplares a XML - {ejemplares.Count} registros - {_012IP_Recortar(Path.GetFileName(ruta), 60)}",
                3);
        }

        /// <summary>DES-SERIALIZAR: lee un XML y devuelve los ejemplares para mostrarlos en la grilla. No escribe en la base.</summary>
        public List<_012IP_EjemplarBE> _012IP_DesserializarEjemplares(string ruta)
        {
            ruta = _012IP_ValidarRutaXml(ruta, true);

            List<_012IP_EjemplarBE> lista;
            try
            {
                lista = _012IP_SerializadorXmlBLL._012IP_Desserializar<_012IP_EjemplarBE>(ruta, RAIZ_XML)
                        ?? new List<_012IP_EjemplarBE>();
            }
            catch (Exception ex)
            {
                throw new Exception(_012IP_Texto("InvBLLXmlErrorDesserializar"), ex);
            }

            string[] estadosValidos = { ESTADO_BUENO, ESTADO_DETERIORADO, ESTADO_RESTAURACION, ESTADO_BAJA };
            var codigos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (_012IP_EjemplarBE e in lista)
            {
                bool valido = _012IP_ValidarFormato(e.CodigoBarras, e.NumeroInventario, e.Titulo, e.Autor, e.Origen)
                              && Array.IndexOf(estadosValidos, e.Estado) >= 0
                              && codigos.Add(e.CodigoBarras.Trim());

                if (!valido)
                    throw new Exception(_012IP_Texto("InvBLLXmlDatosInvalidos"));
            }

            _012IP_RegistrarBitacora(
                $"Des-serializar ejemplares desde XML - {lista.Count} registros - {_012IP_Recortar(Path.GetFileName(ruta), 60)}",
                3);

            return lista;
        }
    }
}