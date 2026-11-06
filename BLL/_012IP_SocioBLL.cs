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

        public _012IP_SocioBLL()
        {
            socioDAL = new _012IP_SocioDAL();
            suscripcionBLL = new _012IP_SuscripcionBLL();
            bitacoraBLL = new BitacoraBLL();
        }

        // ===================== Reporte de socios =====================
        public const string ESTADO_TODOS = "Todos";
        public const string ESTADO_VIGENTE = "Vigente";
        public const string ESTADO_POR_VENCER = "PorVencer";
        public const string ESTADO_VENCIDA = "Vencida";
        public const string ESTADO_SIN_MEMBRESIA = "SinMembresia";
        private const int MaximoFilasReporte = 100000;

        // Arma el reporte: socios activos filtrados por texto (DNI, nombre o apellido),
        // por estado de la membresía y por tipo de membresía (null = todas).
        public List<_012IP_ReporteSocioBE> _012IP_GenerarReporteSocios(string texto, string estado, int? idMembresia)
        {
            List<string> palabras = (texto ?? "")
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Take(4).ToList();

            List<_012IP_SocioBE> socios;
            try
            {
                socios = socioDAL._012IP_BuscarSocioPorTexto(palabras, MaximoFilasReporte);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText3"), ex);
            }

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
                Estado = _012IP_EstadoMembresia(s.Suscripcion)
            });

            if (!string.IsNullOrEmpty(estado) && estado != ESTADO_TODOS)
                filas = filas.Where(f => f.Estado == estado);
            if (idMembresia.HasValue)
                filas = filas.Where(f => f.IdMembresia == idMembresia.Value);

            return filas.OrderBy(f => f.Apellido).ThenBy(f => f.Nombre).ToList();
        }

        public string _012IP_EstadoMembresia(_012IP_SuscripcionBE? suscripcion)
        {
            if (suscripcion == null) return ESTADO_SIN_MEMBRESIA;
            if (suscripcion.DiasRestantes < 0) return ESTADO_VENCIDA;
            if (suscripcion.DiasRestantes <= _012IP_SuscripcionBLL.DiasAvisoVencimiento) return ESTADO_POR_VENCER;
            return ESTADO_VIGENTE;
        }

        public _012IP_ResumenReporteSociosBE _012IP_ResumirReporte(List<_012IP_ReporteSocioBE> filas)
        {
            return new _012IP_ResumenReporteSociosBE
            {
                Total = filas.Count,
                Vigentes = filas.Count(f => f.Estado == ESTADO_VIGENTE),
                PorVencer = filas.Count(f => f.Estado == ESTADO_POR_VENCER),
                Vencidas = filas.Count(f => f.Estado == ESTADO_VENCIDA),
                SinMembresia = filas.Count(f => f.Estado == ESTADO_SIN_MEMBRESIA)
            };
        }

        // Deja constancia en la bitácora de que se exportó el reporte.
        public void _012IP_RegistrarExportacionReporte(int cantidad, string archivo)
        {
            bitacoraBLL.RegistrarEvento(new Bitacora
            {
                Login = SessionManager.Instance.UsuarioActual().Username,
                Modulo = "Reportes",
                Evento = $"Exportar reporte de socios a PDF - {cantidad} socio(s) - {System.IO.Path.GetFileName(archivo)}",
                Criticidad = 4
            });
        }

        public const int MaximoResultadosBusqueda = 50;

        // Busqueda dinamica de un solo cuadro: acepta DNI, nombre, apellido o combinaciones ("perez juan").
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
                return socioDAL._012IP_BuscarSocioPorTexto(palabras, incluirInactivos ? MaximoFilasReporte : MaximoResultadosBusqueda, incluirInactivos);
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

        public List<_012IP_CuotaBE> _012IP_ValidarEstadoCuenta(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception(LanguageManager.Instance.GetTraduction("DNIRequerido"));

            try
            {
                _012IP_SuscripcionBE suscripcion = suscripcionBLL._012IP_ObtenerSuscripcion(dni);

                if (suscripcion == null)
                    return new List<_012IP_CuotaBE>();

                return suscripcion.Cuotas.Where(c => c.EstaVencida).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText5"), ex);
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
                Modulo = "Socios",
                Evento = $"Registrar socio - DNI {socio.DNI} - {socio.Nombre} {socio.Apellido}",
                Criticidad = 2
            };
            bitacoraBLL.RegistrarEvento(bitacora);

            return socio;
        }

        // ===================== Modificación de datos (genera versión en la bitácora de cambios) =====================

        // Cadena del DVH de Socio (igual que en el alta).
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
                Modulo = "Socios",
                Evento = $"Modificar socio - DNI {socio.DNI} - {socio.Nombre} {socio.Apellido}",
                Criticidad = 2
            });
            return socio;
        }

        // Baja lógica (Activo = 0) o reactivación del socio. No se borra nada: el trigger de Socio
        // guarda la versión en la bitácora de cambios y el socio deja de aparecer en Renovar membresía.
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
                Modulo = "Socios",
                Evento = (activo ? "Reactivar socio" : "Baja lógica de socio") + $" - DNI {socio.DNI} - {socio.Nombre} {socio.Apellido}",
                Criticidad = 2
            });
        }

        // Valida DNI único (sin contar al mismo socio), calcula el DVH y actualiza la tabla Socio.
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
