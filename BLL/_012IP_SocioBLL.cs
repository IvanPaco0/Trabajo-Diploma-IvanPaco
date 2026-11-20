using _012IP_BE;
using _012IP_DAL;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _012IP_BLL
{
    public class _012IP_SocioBLL
    {
        private readonly _012IP_SocioDAL socioDAL;
        private readonly _012IP_SuscripcionBLL suscripcionBLL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly _012IP_ParametroBLL parametroBLL;

        public _012IP_SocioBLL()
        {
            socioDAL = new _012IP_SocioDAL();
            suscripcionBLL = new _012IP_SuscripcionBLL();
            bitacoraBLL = new BitacoraBLL();
            parametroBLL = new _012IP_ParametroBLL();
        }

        /// <param name="estado">null = todos</param>
        public List<_012IP_ReporteSocioBE> _012IP_GenerarReporteSocios(string texto, _012IP_EstadoMembresia? estado, int? idMembresia)
        {
            List<string> palabras = (texto ?? "")
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Take(4).ToList();

            List<_012IP_SocioBE> socios;
            try
            {
                socios = socioDAL._012IP_BuscarSocioPorTexto(palabras, parametroBLL._012IP_MaximoFilasReporteSocios());
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText3"), ex);
            }

            int diasAviso = parametroBLL._012IP_DiasAvisoVencimiento();
            var filas = socios.Select(s => new _012IP_ReporteSocioBE
            {
                DNI = s.DNI,
                Nombre = s.Nombre,
                Apellido = s.Apellido,
                Email = s.Email,
                Telefono = s.Telefono,
                IdMembresia = s.Suscripcion?.IdMembresia,
                Membresia = s.Suscripcion?.Membresia?.Nombre ?? "-",
                FechaVencimiento = s.Suscripcion?.FechaVencimiento,
                DiasRestantes = s.Suscripcion?.DiasRestantes,
                Estado = _012IP_CalcularEstadoMembresia(s.Suscripcion, diasAviso)
            });

            if (estado.HasValue)
                filas = filas.Where(f => f.Estado == estado.Value);
            if (idMembresia.HasValue)
                filas = filas.Where(f => f.IdMembresia == idMembresia.Value);

            return filas.OrderBy(f => f.Apellido).ThenBy(f => f.Nombre).ToList();
        }

        private static _012IP_EstadoMembresia _012IP_CalcularEstadoMembresia(_012IP_SuscripcionBE? suscripcion, int diasAviso)
        {
            if (suscripcion == null) return _012IP_EstadoMembresia.SinMembresia;
            if (suscripcion.DiasRestantes < 0) return _012IP_EstadoMembresia.Vencida;
            if (suscripcion.DiasRestantes <= diasAviso) return _012IP_EstadoMembresia.PorVencer;
            return _012IP_EstadoMembresia.Vigente;
        }

        public _012IP_ResumenReporteSociosBE _012IP_ResumirReporte(List<_012IP_ReporteSocioBE> filas)
        {
            return new _012IP_ResumenReporteSociosBE
            {
                Total = filas.Count,
                Vigentes = filas.Count(f => f.Estado == _012IP_EstadoMembresia.Vigente),
                PorVencer = filas.Count(f => f.Estado == _012IP_EstadoMembresia.PorVencer),
                Vencidas = filas.Count(f => f.Estado == _012IP_EstadoMembresia.Vencida),
                SinMembresia = filas.Count(f => f.Estado == _012IP_EstadoMembresia.SinMembresia)
            };
        }

        public void _012IP_RegistrarExportacionReporte(int cantidad, string archivo)
        {
            bitacoraBLL.RegistrarEvento(new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Evento = $"Exportar reporte de socios a PDF - {cantidad} socio(s) - {System.IO.Path.GetFileName(archivo)}",
            });
        }

        public List<_012IP_SocioBE> _012IP_BuscarSocioPorTexto(string texto, bool incluirInactivos = false)
        {
            List<string> palabras = (texto ?? "")
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => p.Length > 0)
                .Take(4)
                .ToList();

            try
            {
                int maximo = incluirInactivos ? parametroBLL._012IP_MaximoFilasReporteSocios() : parametroBLL._012IP_MaximoResultadosBusquedaSocios();
                return socioDAL._012IP_BuscarSocioPorTexto(palabras, maximo, incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText3"), ex);
            }
        }

        public List<_012IP_SocioBE> _012IP_BuscarSocio(string dni, string nombre, string apellido)
        {
            dni = (dni ?? "").Trim();
            nombre = (nombre ?? "").Trim();
            apellido = (apellido ?? "").Trim();

            if (dni == "" && nombre == "" && apellido == "")
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText1"));

            if (dni != "" && !dni.All(char.IsDigit))
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText2"));

            try
            {
                return socioDAL._012IP_BuscarSocio(dni, nombre, apellido);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText3"), ex);
            }
        }

        
        public bool _012IP_ValidarFormato(string dni, string nombre, string apellido, string email, string telefono)
        {
            if (string.IsNullOrWhiteSpace(dni) || dni.Length < 6 || dni.Length > 10 || !dni.All(char.IsDigit))
                return false;

            if (string.IsNullOrWhiteSpace(nombre) || !Regex.IsMatch(nombre.Trim(), @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]+$"))
                return false;

            if (string.IsNullOrWhiteSpace(apellido) || !Regex.IsMatch(apellido.Trim(), @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]+$"))
                return false;

            if (!string.IsNullOrWhiteSpace(email) && !Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return false;

            if (!string.IsNullOrWhiteSpace(telefono) && !Regex.IsMatch(telefono.Trim(), @"^[0-9\-\+\s]{6,20}$"))
                return false;

            return true;
        }

        public bool _012IP_ValidarDNIUnico(string dni)
        {
            try
            {
                return !socioDAL._012IP_ExisteSocioConDNI(dni.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText3"), ex);
            }
        }

        public _012IP_SocioBE _012IP_RegistrarNuevoSocio(string dni, string nombre, string apellido, string email, string telefono)
        {
            dni = (dni ?? "").Trim();
            nombre = (nombre ?? "").Trim();
            apellido = (apellido ?? "").Trim();
            email = (email ?? "").Trim();
            telefono = (telefono ?? "").Trim();

            if (!_012IP_ValidarFormato(dni, nombre, apellido, email, telefono))
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText6"));

            if (!_012IP_ValidarDNIUnico(dni))
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText7"));

            var socio = new _012IP_SocioBE
            {
                DNI = dni,
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Telefono = telefono,
                Activo = true
            };

         
            string cadenaDVH = socio.DNI + socio.Nombre + socio.Apellido + socio.Email + socio.Telefono + socio.Activo;
            string dvh = DigitoVerificador.CalcularDVH(cadenaDVH);

            bool registrado;
            try
            {
               
                registrado = socioDAL._012IP_RegistrarNuevoSocio(socio, dvh, SessionManager.Instance.UsuarioActual()?.Username);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText8"), ex);
            }

            if (!registrado)
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText8"));

            socio.DVH = dvh;

            
            Bitacora bitacora = new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Evento = $"Registrar socio - DNI {socio.DNI} - {socio.Nombre} {socio.Apellido}",
            };
            bitacoraBLL.RegistrarEvento(bitacora);

            return socio;
        }

        public static string _012IP_CalcularDVH(_012IP_SocioBE socio)
        {
            return DigitoVerificador.CalcularDVH(socio.DNI + socio.Nombre + socio.Apellido + socio.Email + socio.Telefono + socio.Activo);
        }

        public _012IP_SocioBE _012IP_ModificarSocio(int idSocio, string dni, string nombre, string apellido, string email, string telefono, bool activo = true)
        {
            var socio = new _012IP_SocioBE
            {
                IdSocio = idSocio,
                DNI = (dni ?? "").Trim(),
                Nombre = (nombre ?? "").Trim(),
                Apellido = (apellido ?? "").Trim(),
                Email = (email ?? "").Trim(),
                Telefono = (telefono ?? "").Trim(),
                Activo = activo
            };

            if (!_012IP_ValidarFormato(socio.DNI, socio.Nombre, socio.Apellido, socio.Email, socio.Telefono))
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText6"));

            _012IP_GuardarCambios(socio, "Modificación");

            bitacoraBLL.RegistrarEvento(new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Evento = $"Modificar socio - DNI {socio.DNI} - {socio.Nombre} {socio.Apellido}",
            });
            return socio;
        }

        public void _012IP_CambiarEstadoSocio(_012IP_SocioBE seleccionado, bool activo)
        {
            if (seleccionado == null)
                throw new Exception(LanguageManager.Instance.GetTraduction("GesSocSeleccione"));
            if (seleccionado.Activo == activo)
                return;

            var socio = new _012IP_SocioBE
            {
                IdSocio = seleccionado.IdSocio,
                DNI = seleccionado.DNI,
                Nombre = seleccionado.Nombre,
                Apellido = seleccionado.Apellido,
                Email = seleccionado.Email ?? "",
                Telefono = seleccionado.Telefono ?? "",
                Activo = activo
            };
            _012IP_GuardarCambios(socio, activo ? "Reactivación" : "Baja lógica");
            seleccionado.Activo = activo;

            bitacoraBLL.RegistrarEvento(new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Evento = (activo ? "Reactivar socio" : "Baja lógica de socio") + $" - DNI {socio.DNI} - {socio.Nombre} {socio.Apellido}",
            });
        }

        internal void _012IP_GuardarCambios(_012IP_SocioBE socio, string operacion)
        {
            bool dniRepetido;
            try
            {
                dniRepetido = socioDAL._012IP_ExisteOtroSocioConDNI(socio.DNI, socio.IdSocio);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText3"), ex);
            }
            if (dniRepetido)
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText7"));

            socio.DVH = _012IP_CalcularDVH(socio);

            bool ok;
            try
            {
                ok = socioDAL._012IP_ModificarSocio(socio, socio.DVH, SessionManager.Instance.UsuarioActual()?.Username, operacion);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText9"), ex);
            }
            if (!ok)
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText9"));
        }
    }
}
