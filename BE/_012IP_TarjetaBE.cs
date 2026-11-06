using System;

namespace _012IP_BE
{
    public class _012IP_TarjetaBE
    {
        public bool Valida { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string UltimosDigitos { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;

        public _012IP_TarjetaBE() { }
    }
}
