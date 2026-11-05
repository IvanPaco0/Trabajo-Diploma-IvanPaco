using System;
using System.Collections.Generic;

namespace _012IP_BE
{
    // Informe de recaudación y vencimientos (CUN-05).
    public class _012IP_InformeMembresiaBE
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public int CantidadPagos { get; set; }
        public decimal TotalRecaudado { get; set; }
        public List<_012IP_ResumenRecaudacionBE> PorMedioDePago { get; set; } = new List<_012IP_ResumenRecaudacionBE>();
        public List<_012IP_ResumenRecaudacionBE> PorMembresia { get; set; } = new List<_012IP_ResumenRecaudacionBE>();
        public List<_012IP_VencimientoSocioBE> Vencimientos { get; set; } = new List<_012IP_VencimientoSocioBE>();

        public _012IP_InformeMembresiaBE() { }
    }

    public class _012IP_ResumenRecaudacionBE
    {
        public string Concepto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Importe { get; set; }

        public _012IP_ResumenRecaudacionBE() { }
    }

    public class _012IP_VencimientoSocioBE
    {
        public string DNI { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Membresia { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public int DiasRestantes => (FechaVencimiento.Date - DateTime.Today).Days;

        public _012IP_VencimientoSocioBE() { }
    }
}
