using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_DAL
{
    public class _012IP_SuscripcionDAL : _012IP_AbstractDAL<_012IP_SuscripcionBE>
    {
        public _012IP_SuscripcionDAL() : base() { }

        /// <summary>Devuelve la suscripción más reciente del socio, o null si nunca tuvo una.</summary>
        public _012IP_SuscripcionBE _012IP_ObtenerSuscripcion(string dni)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT TOP 1 su.IdSuscripcion, su.IdSocio, su.IdMembresia, su.FechaInicio,
                           su.FechaVencimiento, su.Estado, m.Nombre AS NombreMembresia, m.CostoMensual
                    FROM Suscripcion su
                    JOIN Socio s ON s.IdSocio = su.IdSocio
                    JOIN Membresia m ON m.IdMembresia = su.IdMembresia
                    WHERE s.DNI = @dni
                    ORDER BY su.FechaVencimiento DESC;";
                _sqlcommand.Parameters.AddWithValue("@dni", dni);

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new _012IP_SuscripcionBE
                        {
                            IdSuscripcion = (int)reader["IdSuscripcion"],
                            IdSocio = (int)reader["IdSocio"],
                            IdMembresia = (int)reader["IdMembresia"],
                            FechaInicio = (DateTime)reader["FechaInicio"],
                            FechaVencimiento = (DateTime)reader["FechaVencimiento"],
                            Estado = (string)reader["Estado"],
                            Membresia = new _012IP_MembresiaBE
                            {
                                IdMembresia = (int)reader["IdMembresia"],
                                Nombre = (string)reader["NombreMembresia"],
                                CostoMensual = (decimal)reader["CostoMensual"]
                            }
                        };
                    }
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

        /// <summary>
        /// Actualiza la suscripción del socio (paso 10). Si el socio todavía no tenía
        /// ninguna (recién registrado por CU-02), la crea.
        /// </summary>
        public void _012IP_ActualizarSuscripcion(string dni, int idMembresia, DateTime fechaInicio, DateTime fechaVencimiento)
        {
            try
            {
                // DVH de la fila (dígito verificador horizontal): antes no se calculaba y la
                // columna es NOT NULL -> el INSERT (socio sin suscripción previa) tiraba error.
                string dvh = DigitoVerificador.CalcularDVH(
                    dni + idMembresia + fechaInicio.ToString("yyyy-MM-dd") + fechaVencimiento.ToString("yyyy-MM-dd") + "Activa");

                _sqlcommand.CommandText = @"
                    UPDATE su
                    SET su.IdMembresia = @idMembresia,
                        su.FechaInicio = @fechaInicio,
                        su.FechaVencimiento = @fechaVencimiento,
                        su.Estado = 'Activa',
                        su.DVH = @dvh
                    FROM Suscripcion su
                    JOIN Socio s ON s.IdSocio = su.IdSocio
                    WHERE s.DNI = @dni;";

                _sqlcommand.Parameters.AddWithValue("@dni", dni);
                _sqlcommand.Parameters.AddWithValue("@idMembresia", idMembresia);
                _sqlcommand.Parameters.AddWithValue("@fechaInicio", fechaInicio.Date);
                _sqlcommand.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento.Date);
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);

                _sqlserver.Open();
                int filas = _sqlcommand.ExecuteNonQuery();

                if (filas == 0)
                {
                    // El socio (recién dado de alta por CU-02) todavía no tenía suscripción: la creamos.
                    _sqlcommand.CommandText = @"
                        INSERT INTO Suscripcion (IdSocio, IdMembresia, FechaInicio, FechaVencimiento, Estado, DVH)
                        SELECT IdSocio, @idMembresia, @fechaInicio, @fechaVencimiento, 'Activa', @dvh
                        FROM Socio WHERE DNI = @dni;";
                    _sqlcommand.ExecuteNonQuery();
                }
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
