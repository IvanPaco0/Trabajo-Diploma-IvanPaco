using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BE
{
    public class _012IP_PagoBE
    {
        public int IdPago { get; set; }
        public int IdSocio { get; set; }

        /// <summary>
        /// Cuota que este pago cancela. Null cuando el pago corresponde a una renovación
        /// nueva (CU-01) para la cual todavía no existe la fila de Cuota (se crea recién
        /// al renovar la suscripción, paso posterior al pago).
        /// </summary>
        public int? IdCuota { get; set; }

        /// <summary>Tipo de membresía que se está pagando (columna IdMembresia de la tabla Pago).</summary>
        public int? IdMembresia { get; set; }

        public decimal Monto { get; set; }
        public string MedioDePago { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
        public string DVH { get; set; }

        public _012IP_PagoBE() { }
    }
}
