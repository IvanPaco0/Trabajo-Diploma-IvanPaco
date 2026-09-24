using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class BitacoraDAL : _012IP_AbstractDAL<Bitacora>
    {
        public BitacoraDAL() { }

        public void RegistrarEvento(Bitacora unEvento, string DVH)
        {
            try
            {
                _sqlcommand.CommandText = @"INSERT INTO Bitacora (Login, Fecha, Modulo, Evento, Criticidad, DVH) 
                           VALUES (@Login, @Fecha, @Modulo, @Evento, @Criticidad, @dvh)";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@Login", unEvento.Login);
                _sqlcommand.Parameters.AddWithValue("@Fecha", unEvento.Fecha);
                _sqlcommand.Parameters.AddWithValue("@Modulo", unEvento.Modulo);
                _sqlcommand.Parameters.AddWithValue("@Evento", unEvento.Evento);
                _sqlcommand.Parameters.AddWithValue("@Criticidad", unEvento.Criticidad);
                _sqlcommand.Parameters.AddWithValue("@dvh", DVH);

                _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitText1"), ex);
            }
            finally
            {
                _sqlserver.Close();
            }
        }


        public List<Bitacora> FiltrarEventos(string nombre, string apellido, string login, string modulo, string evento, DateTime desde, DateTime hasta, int? criticidad)
        {
            List<Bitacora> lista = new List<Bitacora>();
            try
            {
                string query = @"SELECT b.Id_Evento, b.Login, b.Fecha, b.Modulo, 
                                 b.Evento, b.Criticidad
                                 FROM Bitacora b 
                                 INNER JOIN Usuario u ON b.Login = u.UserName
                                 WHERE 1=1";

                _sqlcommand.Parameters.Clear();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query += " AND u.Nombre LIKE @Nombre";
                    _sqlcommand.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");
                }
                if (!string.IsNullOrEmpty(apellido))
                {
                    query += " AND u.Apellido LIKE @Apellido";
                    _sqlcommand.Parameters.AddWithValue("@Apellido", "%" + apellido + "%");
                }
                if (!string.IsNullOrEmpty(login))
                {
                    query += " AND b.Login = @Login";
                    _sqlcommand.Parameters.AddWithValue("@Login", login);
                }
                if (!string.IsNullOrEmpty(modulo))
                {
                    query += " AND b.Modulo = @Modulo";
                    _sqlcommand.Parameters.AddWithValue("@Modulo", modulo);
                }
                if (!string.IsNullOrEmpty(evento))
                {
                    query += " AND b.Evento LIKE @Evento";
                    _sqlcommand.Parameters.AddWithValue("@Evento", "%" + evento + "%");
                }
                if (criticidad.HasValue)
                {
                    query += " AND b.Criticidad = @Criticidad";
                    _sqlcommand.Parameters.AddWithValue("@Criticidad", criticidad.Value);
                }

                query += " AND b.Fecha BETWEEN @Desde AND @Hasta";
                _sqlcommand.Parameters.AddWithValue("@Desde", desde.Date);
                _sqlcommand.Parameters.AddWithValue("@Hasta", hasta.Date.AddDays(1).AddTicks(-1));

                query += " ORDER BY b.Fecha DESC, b.Id_Evento DESC";

                _sqlcommand.CommandText = query;
                _sqlserver.Open();

                using (SqlDataReader reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Bitacora
                        {
                            Id_Evento = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            Fecha = reader.GetDateTime(2),
                            Modulo = reader.GetString(3),
                            Evento = reader.GetString(4),
                            Criticidad = reader.GetInt32(5)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitText2"), ex);
            }
            finally
            {
                _sqlserver.Close();
            }
            return lista;
        }

        public List<string> ObtenerLogins()
        {
            List<string> logins = new List<string>();
            try
            {
                _sqlcommand.CommandText = "SELECT DISTINCT UserName FROM Usuario ORDER BY UserName";
                _sqlcommand.Parameters.Clear();
                _sqlserver.Open();

                using (SqlDataReader reader = _sqlcommand.ExecuteReader())
                    while (reader.Read())
                        logins.Add(reader.GetString(0));
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitText3"), ex);
            }
            finally
            {
                _sqlserver.Close();
            }
            return logins;
        }

        public Usuario ObtenerUsuarioPorLogin(string login)
        {
            Usuario usuario = null;
            try
            {
                _sqlcommand.CommandText = @"SELECT DNI, Nombre, Apellido, UserName 
                                            FROM Usuario 
                                            WHERE UserName = @Login";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@Login", login);
                _sqlserver.Open();

                using (SqlDataReader reader = _sqlcommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            DNI = reader.GetString(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Username = reader.GetString(3)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitText4"), ex);
            }
            finally
            {
                _sqlserver.Close();
            }
            return usuario;
        }

        public List<Bitacora> listarTodosLosEventos()
        {
            try
            {
                _sqlcommand.CommandText = @"SELECT Id_Evento, Login, Fecha, Modulo, Evento, Criticidad, DVH FROM Bitacora";
                _sqlserver.Open();
                var reader = _sqlcommand.ExecuteReader();
                var Bitacoralst = new List<Bitacora>();
                while (reader.Read())
                {
                    Bitacoralst.Add(new Bitacora(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetString(3), reader.GetString(4),
                           reader.GetInt32(5), reader.GetString(6)));
                }
                return Bitacoralst;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public void ActualizarDVH(string v, string dvhCalculado)
        {
            try
            {
                _sqlcommand.CommandText = @"UPDATE Bitacora SET DVH = @DVH WHERE Id_Evento = @IdEvento";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@DVH", dvhCalculado);
                _sqlcommand.Parameters.AddWithValue("@IdEvento", v);
                _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }
    }
}