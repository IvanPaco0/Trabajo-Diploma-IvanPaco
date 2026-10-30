using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _012IP_DAL
{
    public class _012IP_EjemplarDAL : _012IP_AbstractDAL<_012IP_EjemplarBE>
    {
        private const string COLUMNAS =
            "IdEjemplar, CodigoBarras, NumeroInventario, Titulo, Autor, Origen, Estado, Pasillo, Seccion, Estanteria, " +
            "FechaIngreso, FechaEnvioRestauracion, FechaBaja, DVH";

        public _012IP_EjemplarDAL() : base() { }

        private static _012IP_EjemplarBE _012IP_Mapear(SqlDataReader reader)
        {
            return new _012IP_EjemplarBE
            {
                IdEjemplar = (int)reader["IdEjemplar"],
                CodigoBarras = (string)reader["CodigoBarras"],
                NumeroInventario = (string)reader["NumeroInventario"],
                Titulo = (string)reader["Titulo"],
                Autor = (string)reader["Autor"],
                Origen = (string)reader["Origen"],
                Estado = (string)reader["Estado"],
                Pasillo = (string)reader["Pasillo"],
                Seccion = (string)reader["Seccion"],
                Estanteria = (string)reader["Estanteria"],
                FechaIngreso = (DateTime)reader["FechaIngreso"],
                FechaEnvioRestauracion = reader["FechaEnvioRestauracion"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["FechaEnvioRestauracion"],
                FechaBaja = reader["FechaBaja"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["FechaBaja"],
                DVH = (string)reader["DVH"]
            };
        }

        public bool _012IP_ExisteCodigoBarras(string codigoBarras)
        {
            try
            {
                _sqlcommand.CommandText = "SELECT COUNT(1) FROM Ejemplar WHERE CodigoBarras = @codigo;";
                _sqlcommand.Parameters.AddWithValue("@codigo", codigoBarras);

                _sqlserver.Open();
                int cantidad = (int)_sqlcommand.ExecuteScalar();
                return cantidad > 0;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public bool _012IP_ExisteNumeroInventario(string numeroInventario)
        {
            try
            {
                _sqlcommand.CommandText = "SELECT COUNT(1) FROM Ejemplar WHERE NumeroInventario = @inventario;";
                _sqlcommand.Parameters.AddWithValue("@inventario", numeroInventario);

                _sqlserver.Open();
                int cantidad = (int)_sqlcommand.ExecuteScalar();
                return cantidad > 0;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public bool _012IP_RegistrarEjemplar(_012IP_EjemplarBE ejemplar, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    INSERT INTO Ejemplar (CodigoBarras, NumeroInventario, Titulo, Autor, Origen, Estado,
                                          Pasillo, Seccion, Estanteria, FechaIngreso, DVH)
                    OUTPUT INSERTED.IdEjemplar
                    VALUES (@codigo, @inventario, @titulo, @autor, @origen, @estado,
                            @pasillo, @seccion, @estanteria, @fechaIngreso, @dvh);";

                _sqlcommand.Parameters.AddWithValue("@codigo", ejemplar.CodigoBarras);
                _sqlcommand.Parameters.AddWithValue("@inventario", ejemplar.NumeroInventario);
                _sqlcommand.Parameters.AddWithValue("@titulo", ejemplar.Titulo);
                _sqlcommand.Parameters.AddWithValue("@autor", ejemplar.Autor);
                _sqlcommand.Parameters.AddWithValue("@origen", ejemplar.Origen);
                _sqlcommand.Parameters.AddWithValue("@estado", ejemplar.Estado);
                _sqlcommand.Parameters.AddWithValue("@pasillo", ejemplar.Pasillo);
                _sqlcommand.Parameters.AddWithValue("@seccion", ejemplar.Seccion);
                _sqlcommand.Parameters.AddWithValue("@estanteria", ejemplar.Estanteria);
                _sqlcommand.Parameters.AddWithValue("@fechaIngreso", ejemplar.FechaIngreso);
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);

                _sqlserver.Open();
                object idGenerado = _sqlcommand.ExecuteScalar();

                if (idGenerado == null || idGenerado == DBNull.Value)
                    return false;

                ejemplar.IdEjemplar = Convert.ToInt32(idGenerado);
                return true;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public _012IP_EjemplarBE? _012IP_ObtenerPorCodigo(string codigoBarras)
        {
            try
            {
                _sqlcommand.CommandText = "SELECT " + COLUMNAS + " FROM Ejemplar WHERE CodigoBarras = @codigo;";
                _sqlcommand.Parameters.AddWithValue("@codigo", codigoBarras);

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    if (reader.Read())
                        return _012IP_Mapear(reader);
                }
                return null;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        /// <param name="filtro">"" / "Todos" = sin filtro; "Pendientes" = sin ubicar; o un estado (Bueno, Deteriorado, EnRestauracion, Baja)</param>
        public List<_012IP_EjemplarBE> _012IP_Listar(string filtro, string texto)
        {
            var ejemplares = new List<_012IP_EjemplarBE>();
            try
            {
                filtro = (filtro ?? "").Trim();
                texto = (texto ?? "").Trim();

                bool soloPendientes = filtro == "Pendientes";
                string estado = (filtro == "" || filtro == "Todos" || soloPendientes) ? "" : filtro;

                string textoLike = texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");

                _sqlcommand.CommandText = @"
                    SELECT " + COLUMNAS + @"
                    FROM Ejemplar
                    WHERE (@estado = '' OR Estado = @estado)
                      AND (@pendientes = 0 OR (Estado <> 'Baja' AND (Pasillo = '' OR Seccion = '' OR Estanteria = '')))
                      AND (@texto = '' OR CodigoBarras LIKE '%' + @texto + '%'
                                       OR NumeroInventario LIKE '%' + @texto + '%'
                                       OR Titulo LIKE '%' + @texto + '%'
                                       OR Autor LIKE '%' + @texto + '%')
                    ORDER BY Titulo, CodigoBarras;";

                _sqlcommand.Parameters.AddWithValue("@estado", estado);
                _sqlcommand.Parameters.AddWithValue("@pendientes", soloPendientes ? 1 : 0);
                _sqlcommand.Parameters.AddWithValue("@texto", textoLike);

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                        ejemplares.Add(_012IP_Mapear(reader));
                }
                return ejemplares;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public bool _012IP_ActualizarUbicacion(string codigoBarras, string pasillo, string seccion, string estanteria, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    UPDATE Ejemplar
                    SET Pasillo = @pasillo, Seccion = @seccion, Estanteria = @estanteria, DVH = @dvh
                    WHERE CodigoBarras = @codigo;";

                _sqlcommand.Parameters.AddWithValue("@codigo", codigoBarras);
                _sqlcommand.Parameters.AddWithValue("@pasillo", pasillo);
                _sqlcommand.Parameters.AddWithValue("@seccion", seccion);
                _sqlcommand.Parameters.AddWithValue("@estanteria", estanteria);
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);

                _sqlserver.Open();
                return _sqlcommand.ExecuteNonQuery() > 0;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public bool _012IP_ActualizarEstado(string codigoBarras, string nuevoEstado, DateTime? fechaEnvioRestauracion, DateTime? fechaBaja, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    UPDATE Ejemplar
                    SET Estado = @estado,
                        FechaEnvioRestauracion = @envio,
                        FechaBaja = @baja,
                        DVH = @dvh
                    WHERE CodigoBarras = @codigo;";

                _sqlcommand.Parameters.AddWithValue("@codigo", codigoBarras);
                _sqlcommand.Parameters.AddWithValue("@estado", nuevoEstado);
                _sqlcommand.Parameters.AddWithValue("@envio", fechaEnvioRestauracion.HasValue ? (object)fechaEnvioRestauracion.Value : DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@baja", fechaBaja.HasValue ? (object)fechaBaja.Value : DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);

                _sqlserver.Open();
                return _sqlcommand.ExecuteNonQuery() > 0;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        /// <summary>Stock en tiempo real: se calcula siempre contra la tabla, nunca se guarda un contador.</summary>
        public _012IP_StockBE _012IP_ObtenerStock()
        {
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT
                        SUM(CASE WHEN Estado <> 'Baja' THEN 1 ELSE 0 END) AS Total,
                        SUM(CASE WHEN Estado = 'Bueno' AND Pasillo <> '' AND Seccion <> '' AND Estanteria <> '' THEN 1 ELSE 0 END) AS Disponibles,
                        SUM(CASE WHEN Estado = 'Deteriorado' THEN 1 ELSE 0 END) AS Deteriorados,
                        SUM(CASE WHEN Estado = 'EnRestauracion' THEN 1 ELSE 0 END) AS EnRestauracion,
                        SUM(CASE WHEN Estado = 'Baja' THEN 1 ELSE 0 END) AS Bajas,
                        SUM(CASE WHEN Estado <> 'Baja' AND (Pasillo = '' OR Seccion = '' OR Estanteria = '') THEN 1 ELSE 0 END) AS PendientesUbicar
                    FROM Ejemplar;";

                var stock = new _012IP_StockBE();
                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        stock.Total = reader["Total"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Total"]);
                        stock.Disponibles = reader["Disponibles"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Disponibles"]);
                        stock.Deteriorados = reader["Deteriorados"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Deteriorados"]);
                        stock.EnRestauracion = reader["EnRestauracion"] == DBNull.Value ? 0 : Convert.ToInt32(reader["EnRestauracion"]);
                        stock.Bajas = reader["Bajas"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Bajas"]);
                        stock.PendientesUbicar = reader["PendientesUbicar"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PendientesUbicar"]);
                    }
                }
                return stock;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public List<string> _012IP_ObtenerSecciones()
        {
            var secciones = new List<string>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT DISTINCT Seccion
                    FROM Ejemplar
                    WHERE Seccion <> '' AND Estado <> 'Baja'
                    ORDER BY Seccion;";

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                        secciones.Add((string)reader["Seccion"]);
                }
                return secciones;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        /// <summary>Ejemplares que deberian estar fisicamente en la seccion: ubicados y en estado Bueno o Deteriorado.</summary>
        public List<_012IP_EjemplarBE> _012IP_ObtenerEsperadosPorSeccion(string seccion)
        {
            var ejemplares = new List<_012IP_EjemplarBE>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT " + COLUMNAS + @"
                    FROM Ejemplar
                    WHERE Seccion = @seccion
                      AND Estado IN ('Bueno', 'Deteriorado')
                      AND Pasillo <> '' AND Estanteria <> ''
                    ORDER BY Pasillo, Estanteria, Titulo;";
                _sqlcommand.Parameters.AddWithValue("@seccion", seccion);

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                        ejemplares.Add(_012IP_Mapear(reader));
                }
                return ejemplares;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public List<_012IP_EjemplarBE> _012IP_ListarTodosDVH()
        {
            var ejemplares = new List<_012IP_EjemplarBE>();
            try
            {
                _sqlcommand.CommandText = "SELECT " + COLUMNAS + " FROM Ejemplar ORDER BY IdEjemplar;";

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                        ejemplares.Add(_012IP_Mapear(reader));
                }
                return ejemplares;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public void _012IP_ActualizarDVH(string codigoBarras, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = "UPDATE Ejemplar SET DVH = @dvh WHERE CodigoBarras = @codigo;";
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);
                _sqlcommand.Parameters.AddWithValue("@codigo", codigoBarras);

                _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }
    }
}