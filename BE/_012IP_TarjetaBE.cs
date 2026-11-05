using System;

namespace _012IP_BE
{
    // Resultado de la validación de una tarjeta (CUN-04). No se persiste.
    public class _012IP_TarjetaBE
    {
        public bool Valida { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string UltimosDigitos { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;   // clave de traducción del motivo de rechazo

        public _012IP_TarjetaBE() { }
    }
}
