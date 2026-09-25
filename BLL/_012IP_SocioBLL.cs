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

        // ===================================================== CU-01 (ya programado)

        /// <summary>
        /// Buscar socio(DNI, Nombre, Apellido): CU-01 pasos 1-3. Lista vacía = "Socio no encontrado"
        /// (la GUI ofrece entonces el CU-02 Registrar socio).
        /// </summary>
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

        /// <summary>
        /// validarEstadoCuenta(DNI): CU-01 paso 4. Devuelve las cuotas vencidas e impagas.
        /// Lista vacía = el socio está al día.
        /// </summary>
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

        // ===================================================== CU-02 (nuevo)

        /// <summary>ValidarFormato(DNI, Nombre, Apellido, Email, Telefono): CU-02 paso 4.</summary>
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

        /// <summary>ValidarDNIUnico(DNI): CU-02 paso 5. True si el DNI está libre para registrar.</summary>
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

        /// <summary>
        /// RegistrarNuevoSocio(DNI, Nombre, Apellido, Email, Telefono): CU-02 pasos 6-9.
        /// Arma el socio, calcula el DVH (antes de persistir), lo registra y recién ahí
        /// escribe el evento en Bitácora. Si algo falló, no llega a tocar la Bitácora
        /// (flujo alternativo 6.1: excepción -> la GUI informa el error).
        /// </summary>
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

            // 1° se calcula el DVH...
            string cadenaDVH = socio.DNI + socio.Nombre + socio.Apellido + socio.Email + socio.Telefono + socio.Activo;
            string dvh = DigitoVerificador.CalcularDVH(cadenaDVH);

            bool registrado;
            try
            {
                // 2° recién acá se persiste, socio y dvh juntos en el mismo INSERT (6.1 si falla)
                registrado = socioDAL._012IP_RegistrarNuevoSocio(socio, dvh);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText8"), ex);
            }

            if (!registrado)
                throw new Exception(LanguageManager.Instance.GetTraduction("SocioBLLText8"));

            socio.DVH = dvh;

            // El BLL llama a Bitácora directamente, nunca la GUI (cohesión/acoplamiento)
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
    }
}