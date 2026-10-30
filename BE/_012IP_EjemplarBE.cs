using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace _012IP_BE
{
    [XmlType("Ejemplar")]
    public class _012IP_EjemplarBE
    {
        public int IdEjemplar { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string NumeroInventario { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Origen { get; set; } = "Compra";          // Compra | Donacion
        public string Estado { get; set; } = "Bueno";           // Bueno | Deteriorado | EnRestauracion | Baja
        public string Pasillo { get; set; } = string.Empty;
        public string Seccion { get; set; } = string.Empty;
        public string Estanteria { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaEnvioRestauracion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string DVH { get; set; } = string.Empty;

        public bool EstaUbicado =>
            !string.IsNullOrWhiteSpace(Pasillo) &&
            !string.IsNullOrWhiteSpace(Seccion) &&
            !string.IsNullOrWhiteSpace(Estanteria);

        public bool EstaDisponible => Estado == "Bueno" && EstaUbicado;

        public _012IP_EjemplarBE() { }
    }
}