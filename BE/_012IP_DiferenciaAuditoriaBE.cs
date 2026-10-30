using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_DiferenciaAuditoriaBE
    {
        public string CodigoBarras { get; set; } = string.Empty;
        public string NumeroInventario { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Resultado { get; set; } = "Faltante";     // Faltante | FueraDeLugar | Desconocido
        public string Detalle { get; set; } = string.Empty;     // ubicacion / estado registrado en el sistema
        public string Resolucion { get; set; } = string.Empty;  // "" (pendiente) | Baja | Reubicado

        public _012IP_DiferenciaAuditoriaBE() { }
    }
}