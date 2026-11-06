using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _012IP_BLL
{
    public class _012IP_BitacoraCambiosBLL
    {
        public const int MaximoFilas = 2000;

        private readonly _012IP_SocioHistorialDAL historialDAL;
        private readonly _012IP_SocioBLL socioBLL;
        private readonly BitacoraBLL bitacoraBLL;

        public _012IP_BitacoraCambiosBLL()
        {
            historialDAL = new _012IP_SocioHistorialDAL();
            socioBLL = new _012IP_SocioBLL();
            bitacoraBLL = new BitacoraBLL();
        }

        public List<_012IP_SocioHistorialBE> _012IP_ConsultarHistorial(string dni, string nombre, DateTime? desde, DateTime? hasta)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                throw new Exception(LanguageManager.Instance.GetTraduction("BitCamFechasInvalidas"));

            dni = (dni ?? "").Trim();
            if (dni.Length > 0 && !dni.All(char.IsDigit))
                throw new Exception(LanguageManager.Instance.GetTraduction("BitCamDniInvalido"));

            List<string> palabras = (nombre ?? "")
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Take(4).ToList();
            try
            {
                return historialDAL._012IP_ListarHistorial(dni, palabras, desde, hasta, MaximoFilas);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitCamErrorConsulta"), ex);
            }
        }

        public _012IP_SocioHistorialBE _012IP_ActivarVersion(int idHistorial)
        {
            _012IP_SocioHistorialBE version;
            try
            {
                version = historialDAL._012IP_ObtenerVersion(idHistorial);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitCamErrorConsulta"), ex);
            }

            if (version == null)
                throw new Exception(LanguageManager.Instance.GetTraduction("BitCamVersionNoExiste"));
            if (version.VersionActiva)
                throw new Exception(LanguageManager.Instance.GetTraduction("BitCamYaVigente"));

            var socio = new _012IP_SocioBE
            {
                IdSocio = version.IdSocio,
                DNI = version.DNI,
                Nombre = version.Nombre,
                Apellido = version.Apellido,
                Email = version.Email,
                Telefono = version.Telefono,
                Activo = version.Activo
            };

            socioBLL._012IP_GuardarCambios(socio, "Activación");

            bitacoraBLL.RegistrarEvento(new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Modulo = "Bitácora de cambios",
                Evento = $"Activar versión {version.IdHistorial} del socio DNI {version.DNI}",
                Criticidad = 1
            });
            return version;
        }
    }
}
