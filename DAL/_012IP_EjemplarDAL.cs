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
            "IdEjemplar, CodigoBarras, NumeroInventario, Titulo, Autor, IdOrigen, IdEstado, Pasillo, Seccion, Estanteria, Valor, " +
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
                Origen = (_012IP_OrigenEjemplar)(int)reader["IdOrigen"],
                Estado = (_012IP_EstadoEjemplar)(int)reader["IdEstado"],
                Pasillo = (string)reader["Pasillo"],
                Seccion = (string)reader["Seccion"],
                Estanteria = (string)reader["Estanteria"],
                Valor = (decimal)reader["Valor"],
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
                    INSERT INTO Ejemplar (CodigoBarras, NumeroInventario, Titulo, Autor, IdOrigen, IdEstado,
                                          Pasillo, Seccion, Estanteria, Valor, FechaIngreso, DVH)
                    OUTPUT INSERTED.IdEjemplar
                    VALUES (@codigo, @inventario, @titulo, @autor, @origen, @estado,
                            @pasillo, @seccion, @estanteria, @valor, @fechaIngreso, @dvh);";

                _sqlcommand.Parameters.AddWithValue("@codigo", ejemplar.CodigoBarras);
                _sqlcommand.Parameters.AddWithValue("@inventario", ejemplar.NumeroInventario);
                _sqlcommand.Parameters.AddWithValue("@titulo", ejemplar.Titulo);
                _sqlcommand.Parameters.AddWithValue("@autor", ejemplar.Autor);
                _sqlcommand.Parameters.AddWithValue("@origen", (int)ejemplar.Origen);
                _sqlcommand.Parameters.AddWithValue("@estado", (int)ejemplar.Estado);
                _sqlcommand.Parameters.AddWithValue("@pasillo", ejemplar.Pasillo);
                _sqlcommand.Parameters.AddWithValue("@seccion", ejemplar.Seccion);
                _sqlcommand.Parameters.AddWithValue("@estanteria", ejemplar.Estanteria);
                _sqlcommand.Parameters.AddWithValue("@valor", ejemplar.Valor);
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

        /// <param name="estado">null = todos los estados</param>
        /// <param name="soloPendientes">solo los que no tienen ubicacion (y no estan de baja)</param>
        public List<_012IP_EjemplarBE> _012IP_Listar(_012IP_EstadoEjemplar? estado, bool soloPendientes, string texto)
        {
            var ejemplares = new List<_012IP_EjemplarBE>();
            try
            {
                texto = (texto ?? "").Trim();

                string textoLike = texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");

                _sqlcommand.CommandText = @"
                    SELECT " + COLUMNAS + @"
                    FROM Ejemplar
                    WHERE (@estado = 0 OR IdEstado = @estado)
                      AND (@pendientes = 0 OR (IdEstado <> @baja AND (Pasillo = '' OR Seccion = '' OR Estanteria = '')))
                      AND (@texto = '' OR CodigoBarras LIKE '%' + @texto + '%'
                                       OR NumeroInventario LIKE '%' + @texto + '%'
                                       OR Titulo LIKE '%' + @texto + '%'
                                       OR Autor LIKE '%' + @texto + '%')
                    ORDER BY Titulo, CodigoBarras;";

                _sqlcommand.Parameters.AddWithValue("@estado", estado.HasValue ? (int)estado.Value : 0);
                _sqlcommand.Parameters.AddWithValue("@baja", (int)_012IP_EstadoEjemplar.Baja);
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

        public void _012IP_RegistrarMovimiento(string codigoBarras, _012IP_TipoMovimientoEjemplar tipo, _012IP_EstadoEjemplar? estadoAnterior, _012IP_EstadoEjemplar? estadoNuevo, string detalle, string usuario)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    INSERT INTO EjemplarMovimiento (IdEjemplar, Fecha, IdTipoMovimiento, IdEstadoAnterior, IdEstadoNuevo, Detalle, Usuario)
                    SELECT IdEjemplar, GETDATE(), @tipo, @anterior, @nuevo, @detalle, @usuario
                    FROM Ejemplar WHERE CodigoBarras = @codigo;";

                _sqlcommand.Parameters.AddWithValue("@codigo", codigoBarras);
                _sqlcommand.Parameters.AddWithValue("@tipo", (int)tipo);
                _sqlcommand.Parameters.AddWithValue("@anterior", estadoAnterior.HasValue ? (object)(int)estadoAnterior.Value : DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@nuevo", estadoNuevo.HasValue ? (object)(int)estadoNuevo.Value : DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@detalle", detalle.Length > 200 ? detalle.Substring(0, 200) : detalle);
                _sqlcommand.Parameters.AddWithValue("@usuario", usuario.Length > 50 ? usuario.Substring(0, 50) : usuario);

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

        public bool _012IP_ActualizarEstado(string codigoBarras, _012IP_EstadoEjemplar nuevoEstado, DateTime? fechaEnvioRestauracion, DateTime? fechaBaja, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    UPDATE Ejemplar
                    SET IdEstado = @estado,
                        FechaEnvioRestauracion = @envio,
                        FechaBaja = @baja,
                        DVH = @dvh
                    WHERE CodigoBarras = @codigo;";

                _sqlcommand.Parameters.AddWithValue("@codigo", codigoBarras);
                _sqlcommand.Parameters.AddWithValue("@estado", (int)nuevoEstado);
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
                        SUM(CASE WHEN IdEstado <> @baja THEN 1 ELSE 0 END) AS Total,
                        SUM(CASE WHEN IdEstado = @bueno AND Pasillo <> '' AND Seccion <> '' AND Estanteria <> '' THEN 1 ELSE 0 END) AS Disponibles,
                        SUM(CASE WHEN IdEstado = @deteriorado THEN 1 ELSE 0 END) AS Deteriorados,
                        SUM(CASE WHEN IdEstado = @restauracion THEN 1 ELSE 0 END) AS EnRestauracion,
                        SUM(CASE WHEN IdEstado = @baja THEN 1 ELSE 0 END) AS Bajas,
                        SUM(CASE WHEN IdEstado <> @baja AND (Pasillo = '' OR Seccion = '' OR Estanteria = '') THEN 1 ELSE 0 END) AS PendientesUbicar,
                        SUM(CASE WHEN IdEstado <> @baja THEN Valor ELSE 0 END) AS ValorPatrimonio
                    FROM Ejemplar;";
                _sqlcommand.Parameters.AddWithValue("@bueno", (int)_012IP_EstadoEjemplar.Bueno);
                _sqlcommand.Parameters.AddWithValue("@deteriorado", (int)_012IP_EstadoEjemplar.Deteriorado);
                _sqlcommand.Parameters.AddWithValue("@restauracion", (int)_012IP_EstadoEjemplar.EnRestauracion);
                _sqlcommand.Parameters.AddWithValue("@baja", (int)_012IP_EstadoEjemplar.Baja);

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
                        stock.ValorPatrimonio = reader["ValorPatrimonio"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValorPatrimonio"]);
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
                    WHERE Seccion <> '' AND IdEstado <> @baja
                    ORDER BY Seccion;";
                _sqlcommand.Parameters.AddWithValue("@baja", (int)_012IP_EstadoEjemplar.Baja);

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
                      AND IdEstado IN (@bueno, @deteriorado)
                      AND Pasillo <> '' AND Estanteria <> ''
                    ORDER BY Pasillo, Estanteria, Titulo;";
                _sqlcommand.Parameters.AddWithValue("@seccion", seccion);
                _sqlcommand.Parameters.AddWithValue("@bueno", (int)_012IP_EstadoEjemplar.Bueno);
                _sqlcommand.Parameters.AddWithValue("@deteriorado", (int)_012IP_EstadoEjemplar.Deteriorado);

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

        // ------------------------------------------------------------------
        //  Tablas de valores: EstadoEjemplar, OrigenEjemplar y transiciones de estado
        // ------------------------------------------------------------------

        private List<int> _012IP_LeerIds(string consulta, Action? parametros = null)
        {
            var ids = new List<int>();
            try
            {
                _sqlcommand.CommandText = consulta;
                parametros?.Invoke();

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                        ids.Add(reader.GetInt32(0));
                }
                return ids;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public List<_012IP_EstadoEjemplar> _012IP_ListarEstados()
        {
            return _012IP_LeerIds("SELECT IdEstado FROM EstadoEjemplar ORDER BY IdEstado;")
                .ConvertAll(id => (_012IP_EstadoEjemplar)id);
        }

        public List<_012IP_OrigenEjemplar> _012IP_ListarOrigenes()
        {
            return _012IP_LeerIds("SELECT IdOrigen FROM OrigenEjemplar ORDER BY IdOrigen;")
                .ConvertAll(id => (_012IP_OrigenEjemplar)id);
        }

        /// <summary>Estados a los que puede pasar un ejemplar desde el estado indicado (tabla EstadoEjemplarTransicion).</summary>
        public List<_012IP_EstadoEjemplar> _012IP_ListarEstadosSiguientes(_012IP_EstadoEjemplar estadoActual)
        {
            return _012IP_LeerIds(
                    "SELECT IdEstadoDestino FROM EstadoEjemplarTransicion WHERE IdEstadoOrigen = @origen ORDER BY IdEstadoDestino;",
                    () => _sqlcommand.Parameters.AddWithValue("@origen", (int)estadoActual))
                .ConvertAll(id => (_012IP_EstadoEjemplar)id);
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