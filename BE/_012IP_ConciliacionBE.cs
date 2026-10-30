using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_ConciliacionBE
    {
        public string Seccion { get; set; } = string.Empty;
        public int Esperados { get; set; }
        public int Correctos { get; set; }
        public int TotalEscaneados { get; set; }
        public List<_012IP_DiferenciaAuditoriaBE> Diferencias { get; set; } = new List<_012IP_DiferenciaAuditoriaBE>();

        public int Faltantes => Diferencias.Count(d => d.Resultado == "Faltante");
        public int Sobrantes => Diferencias.Count(d => d.Resultado != "Faltante");
        public int FaltantesSinResolver => Diferencias.Count(d => d.Resultado == "Faltante" && d.Resolucion == "");

        public _012IP_ConciliacionBE() { }
    }
}