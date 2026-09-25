using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_SocioBE
    {
        public int IdSocio { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public _012IP_SuscripcionBE? Suscripcion { get; set; }
        public _012IP_SocioBE() { }
        public string DVH { get; set; }
    }
}
