using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace _012IP_BLL
{
    /// <summary>
    /// Utilidad generica de serializacion XML para operaciones ABM (no usa base de datos).
    /// Se comparte entre maestros: sirve para cualquier BE con constructor publico sin parametros.
    /// </summary>
    public static class _012IP_SerializadorXmlBLL
    {
        public static void _012IP_Serializar<T>(List<T> datos, string ruta, string nombreRaiz)
        {
            var serializador = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(nombreRaiz));
            var opciones = new XmlWriterSettings
            {
                Indent = true,
                Encoding = new UTF8Encoding(false)
            };

            using (XmlWriter escritor = XmlWriter.Create(ruta, opciones))
            {
                serializador.Serialize(escritor, datos);
            }
        }

        public static List<T> _012IP_Desserializar<T>(string ruta, string nombreRaiz)
        {
            var serializador = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(nombreRaiz));

            // DTD prohibido y sin resolver externo: evita XXE al leer archivos elegidos por el operador.
            var opciones = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            using (XmlReader lector = XmlReader.Create(ruta, opciones))
            {
                return (List<T>)serializador.Deserialize(lector);
            }
        }
    }
}