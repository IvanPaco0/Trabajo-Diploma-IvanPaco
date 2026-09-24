using _012IP_BE;
using _012IP_DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _012IP_BLL
{
    public class _012IP_SocioBLL
    {
        private readonly _012IP_SocioDAL socioDAL;
        private readonly _012IP_SuscripcionBLL suscripcionBLL;

        public _012IP_SocioBLL()
        {
            socioDAL = new _012IP_SocioDAL();
            suscripcionBLL = new _012IP_SuscripcionBLL();
        }

        /// <summary>
        /// Buscar socio(DNI, Nombre, Apellido): pasos 1-3. Lista vacía = "Socio no encontrado"
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
        /// validarEstadoCuenta(DNI): paso 4. Devuelve las cuotas vencidas e impagas.
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
    }
}
