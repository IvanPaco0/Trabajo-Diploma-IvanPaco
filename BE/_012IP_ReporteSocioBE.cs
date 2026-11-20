using System;

namespace _012IP_BE
{
    public class _012IP_ReporteSocioBE
    {
        public string DNI { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int? IdMembresia { get; set; }
        public string Membresia { get; set; } = string.Empty;
        public DateTime? FechaVencimiento { get; set; }
        public int? DiasRestantes { get; set; }
        public _012IP_EstadoMembresia Estado { get; set; }
    }

    public class _012IP_ResumenReporteSociosBE
    {
        public int Total { get; set; }
        public int Vigentes { get; set; }
        public int PorVencer { get; set; }
        public int Vencidas { get; set; }
        public int SinMembresia { get; set; }
    }
}
