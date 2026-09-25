using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_PagoBE
    {
        public int IdPago { get; set; }
        public int IdSocio { get; set; }

        public int? IdCuota { get; set; }

        public int? IdMembresia { get; set; }

        public decimal Monto { get; set; }
        public string MedioDePago { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
        public string DVH { get; set; }

        public _012IP_PagoBE() { }
    }
}
