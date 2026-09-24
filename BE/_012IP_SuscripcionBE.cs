using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_SuscripcionBE
    {
        public int IdSuscripcion { get; set; }
        public int IdSocio { get; set; }
        public int IdMembresia { get; set; }
        public _012IP_MembresiaBE? Membresia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; } = "Activa";   // Activa | Vencida
        public List<_012IP_CuotaBE> Cuotas { get; set; } = new List<_012IP_CuotaBE>();

        public _012IP_SuscripcionBE() { }
    }
}
