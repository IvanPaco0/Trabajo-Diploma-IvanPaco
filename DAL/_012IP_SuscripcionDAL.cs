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

       
        // Actualiza (o crea) la suscripción del socio. El DVH lo calcula la BLL.
        public void _012IP_ActualizarSuscripcion(string dni, int idMembresia, DateTime fechaInicio, DateTime fechaVencimiento, string dvh)
        {
            try
            {
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
    
        // Última suscripción de cada socio activo, para el informe de vencimientos (CUN-05).
        public List<_012IP_VencimientoSocioBE> _012IP_ListarSuscripciones()
        {
            var lista = new List<_012IP_VencimientoSocioBE>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT s.DNI, s.Nombre, s.Apellido, m.Nombre AS Membresia, su.FechaVencimiento
                    FROM Suscripcion su
                    JOIN Socio s ON s.IdSocio = su.IdSocio
                    JOIN Membresia m ON m.IdMembresia = su.IdMembresia
                    WHERE s.Activo = 1
                      AND su.FechaVencimiento = (SELECT MAX(x.FechaVencimiento) FROM Suscripcion x WHERE x.IdSocio = su.IdSocio)
                    ORDER BY su.FechaVencimiento;";

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new _012IP_VencimientoSocioBE
                        {
                            DNI = (string)reader["DNI"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            Membresia = (string)reader["Membresia"],
                            FechaVencimiento = (DateTime)reader["FechaVencimiento"]
                        });
                    }
                }
                return lista;
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
