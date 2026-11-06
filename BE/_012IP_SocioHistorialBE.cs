using System;

namespace _012IP_BE
{
    public class _012IP_SocioHistorialBE
    {
        public int IdHistorial { get; set; }
        public int IdSocio { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCambio { get; set; }
        public string Operacion { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public bool VersionActiva { get; set; }

        public _012IP_SocioHistorialBE() { }
    }
}
