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
        public _012IP_ResultadoDiferencia Resultado { get; set; } = _012IP_ResultadoDiferencia.Faltante;
        public string Detalle { get; set; } = string.Empty;     // ubicacion / estado registrado en el sistema
        public _012IP_ResolucionDiferencia Resolucion { get; set; } = _012IP_ResolucionDiferencia.Pendiente;

        public bool EsFaltantePendiente =>
            Resultado == _012IP_ResultadoDiferencia.Faltante && Resolucion == _012IP_ResolucionDiferencia.Pendiente;

        public _012IP_DiferenciaAuditoriaBE() { }
    }
}