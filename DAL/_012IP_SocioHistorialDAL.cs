using _012IP_BE;
using DAL;
using System;
using System.Collections.Generic;

namespace _012IP_DAL
{
    // Bitácora de cambios de la tabla maestra Socio.
    // La tabla SocioHistorial la llenan los triggers TR_Socio_Insert y TR_Socio_Update;
    // desde la aplicación solo se consulta.
    public class _012IP_SocioHistorialDAL : _012IP_AbstractDAL<_012IP_SocioHistorialBE>
    {
        public _012IP_SocioHistorialDAL() : base() { }

        private const string SelectHistorial = @"
                    SELECT h.IdHistorial, h.IdSocio, h.DNI, h.Nombre, h.Apellido, h.Email, h.Telefono, h.Activo,
                           h.FechaCambio, h.Operacion, h.Usuario, h.VersionActiva
                    FROM SocioHistorial h";

        // Filtros: palabras (DNI que empieza con / nombre o apellido que contiene) y rango de fechas del cambio.
        public List<_012IP_SocioHistorialBE> _012IP_ListarHistorial(string dni, List<string> palabras, DateTime? desde, DateTime? hasta, int maximo)
        {
            try
            {
                var condiciones = new System.Text.StringBuilder(" WHERE 1 = 1");
                if (!string.IsNullOrEmpty(dni))
                {
                    condiciones.Append(" AND h.DNI LIKE @dni + '%'");
                    _sqlcommand.Parameters.AddWithValue("@dni", dni);
                }
                for (int i = 0; i < palabras.Count; i++)
                {
                    string p = "@p" + i;
                    condiciones.Append(" AND (h.Nombre LIKE '%' + " + p + " + '%' OR h.Apellido LIKE '%' + " + p + " + '%')");
                    _sqlcommand.Parameters.AddWithValue(p, palabras[i]);
                }
                if (desde.HasValue)
                {
                    condiciones.Append(" AND h.FechaCambio >= @desde");
                    _sqlcommand.Parameters.AddWithValue("@desde", desde.Value.Date);
                }
                if (hasta.HasValue)
                {
                    condiciones.Append(" AND h.FechaCambio < @hasta");
                    _sqlcommand.Parameters.AddWithValue("@hasta", hasta.Value.Date.AddDays(1));
                }

                _sqlcommand.CommandText = SelectHistorial.Replace("SELECT h.IdHistorial", "SELECT TOP (@maximo) h.IdHistorial")
                    + condiciones + " ORDER BY h.IdSocio, h.FechaCambio DESC, h.IdHistorial DESC;";
                _sqlcommand.Parameters.AddWithValue("@maximo", maximo);

                return _012IP_Leer();
            }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public _012IP_SocioHistorialBE _012IP_ObtenerVersion(int idHistorial)
        {
            try
            {
                _sqlcommand.CommandText = SelectHistorial + " WHERE h.IdHistorial = @id;";
                _sqlcommand.Parameters.AddWithValue("@id", idHistorial);
                List<_012IP_SocioHistorialBE> lista = _012IP_Leer();
                return lista.Count > 0 ? lista[0] : null;
            }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        private List<_012IP_SocioHistorialBE> _012IP_Leer()
        {
            var lista = new List<_012IP_SocioHistorialBE>();
            _sqlserver.Open();
            using (var reader = _sqlcommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new _012IP_SocioHistorialBE
                    {
                        IdHistorial = (int)reader["IdHistorial"],
                        IdSocio = (int)reader["IdSocio"],
                        DNI = (string)reader["DNI"],
                        Nombre = (string)reader["Nombre"],
                        Apellido = (string)reader["Apellido"],
                        Email = reader["Email"] as string ?? string.Empty,
                        Telefono = reader["Telefono"] as string ?? string.Empty,
                        Activo = (bool)reader["Activo"],
                        FechaCambio = (DateTime)reader["FechaCambio"],
                        Operacion = (string)reader["Operacion"],
                        Usuario = (string)reader["Usuario"],
                        VersionActiva = (bool)reader["VersionActiva"]
                    });
                }
            }
            return lista;
        }
    }
}
