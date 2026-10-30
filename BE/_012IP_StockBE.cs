using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_StockBE
    {
        public int Total { get; set; }
        public int Disponibles { get; set; }
        public int Deteriorados { get; set; }
        public int EnRestauracion { get; set; }
        public int Bajas { get; set; }
        public int PendientesUbicar { get; set; }

        public _012IP_StockBE() { }
    }
}