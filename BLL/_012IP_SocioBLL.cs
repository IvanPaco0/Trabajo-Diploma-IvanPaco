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
               
                registrado = socioDAL._012IP_RegistrarNuevoSocio(socio, dvh);
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
    }
}