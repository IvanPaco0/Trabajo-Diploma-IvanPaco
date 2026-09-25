using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_MembresiaBE
    {
        public int IdMembresia { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal CostoMensual { get; set; }
        public bool Activa { get; set; } = true;

        public _012IP_MembresiaBE() { }

        
        public override string ToString() => $"{Nombre} - ${CostoMensual:N0}";
    }
}
