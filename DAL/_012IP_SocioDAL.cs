using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _012IP_DAL
{
    public class _012IP_SocioDAL : _012IP_AbstractDAL<_012IP_SocioBE>
    {
        public _012IP_SocioDAL() : base() { }

        public List<_012IP_SocioBE> _012IP_BuscarSocio(string dni, string nombre, string apellido)
        {
            var socios = new List<_012IP_SocioBE>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT s.IdSocio, s.DNI, s.Nombre, s.Apellido, s.Email, s.Telefono, s.Activo,
                           su.IdSuscripcion, su.IdMembresia, su.FechaInicio, su.FechaVencimiento, su.Estado,
                           m.Nombre AS NombreMembresia, m.CostoMensual
                    FROM Socio s
                    OUTER APPLY (SELECT TOP 1 x.IdSuscripcion, x.IdMembresia, x.FechaInicio, x.FechaVencimiento, x.Estado
                                 FROM Suscripcion x
                                 WHERE x.IdSocio = s.IdSocio
                                 ORDER BY x.FechaVencimiento DESC) su
                    LEFT JOIN Membresia m ON m.IdMembresia = su.IdMembresia
                    WHERE s.Activo = 1
                      AND (@dni = '' OR s.DNI = @dni)
                      AND (@nombre = '' OR s.Nombre LIKE '%' + @nombre + '%')
                      AND (@apellido = '' OR s.Apellido LIKE '%' + @apellido + '%')
                    ORDER BY s.Apellido, s.Nombre;";

                _sqlcommand.Parameters.AddWithValue("@dni", (dni ?? "").Trim());
                _sqlcommand.Parameters.AddWithValue("@nombre", (nombre ?? "").Trim());
                _sqlcommand.Parameters.AddWithValue("@apellido", (apellido ?? "").Trim());

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var socio = new _012IP_SocioBE
                        {
                            IdSocio = (int)reader["IdSocio"],
                            DNI = (string)reader["DNI"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            Email = reader["Email"] as string ?? string.Empty,
                            Telefono = reader["Telefono"] as string ?? string.Empty,
                            Activo = (bool)reader["Activo"]
                        };

                        if (reader["IdSuscripcion"] != DBNull.Value)
                        {
                            socio.Suscripcion = new _012IP_SuscripcionBE
                            {
                                IdSuscripcion = (int)reader["IdSuscripcion"],
                                IdSocio = socio.IdSocio,
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
                        socios.Add(socio);
                    }
                }
                return socios;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public bool _012IP_ExisteSocioConDNI(string dni)
        {
            try
            {
                _sqlcommand.CommandText = "SELECT COUNT(1) FROM Socio WHERE DNI = @dni;";
                _sqlcommand.Parameters.AddWithValue("@dni", dni);

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

        public bool _012IP_RegistrarNuevoSocio(_012IP_SocioBE socio, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    INSERT INTO Socio (DNI, Nombre, Apellido, Email, Telefono, Activo, DVH)
                    OUTPUT INSERTED.IdSocio
                    VALUES (@dni, @nombre, @apellido, @email, @telefono, 1, @dvh);";

                _sqlcommand.Parameters.AddWithValue("@dni", socio.DNI);
                _sqlcommand.Parameters.AddWithValue("@nombre", socio.Nombre);
                _sqlcommand.Parameters.AddWithValue("@apellido", socio.Apellido);
                _sqlcommand.Parameters.AddWithValue("@email", (object)socio.Email ?? "");
                _sqlcommand.Parameters.AddWithValue("@telefono", (object)socio.Telefono ?? "");
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);

                _sqlserver.Open();
                object idGenerado = _sqlcommand.ExecuteScalar();

                if (idGenerado == null || idGenerado == DBNull.Value)
                    return false;

                socio.IdSocio = Convert.ToInt32(idGenerado);
                socio.Activo = true;
                return true;
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