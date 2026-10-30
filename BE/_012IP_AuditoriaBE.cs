using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_AuditoriaBE
    {
        public int IdAuditoria { get; set; }
        public DateTime Fecha { get; set; }
        public string Seccion { get; set; } = string.Empty;
        public string Responsable { get; set; } = string.Empty;
        public int Esperados { get; set; }
        public int Encontrados { get; set; }
        public int Faltantes { get; set; }
        public int Sobrantes { get; set; }
        public string DVH { get; set; } = string.Empty;

        public _012IP_AuditoriaBE() { }
    }
}