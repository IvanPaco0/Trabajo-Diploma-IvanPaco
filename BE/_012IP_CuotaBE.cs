using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_CuotaBE
    {
        public int IdCuota { get; set; }
        public int IdSuscripcion { get; set; }
        public DateTime Periodo { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; } = "Pendiente";   // Pendiente | Pagada
        public DateTime? FechaPago { get; set; }

        public bool EstaVencida => Estado == "Pendiente" && FechaVencimiento.Date < DateTime.Today;

        public _012IP_CuotaBE() { }
    }
}
