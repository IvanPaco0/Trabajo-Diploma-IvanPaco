using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class IdiomaDAL : _012IP_AbstractDAL<IdiomaDAL>
    {
        public string ObtenerIdioma(string userName)
        {
            try
            {
                _sqlcommand.Parameters.Clear();

                _sqlcommand.CommandText = @"
                    SELECT CodigoIdioma
                    FROM Idioma
                    WHERE UserName = @UserName";

                _sqlcommand.Parameters.AddWithValue(
                    "@UserName",
                    userName);

                _sqlserver.Open();

                object resultado =
                    _sqlcommand.ExecuteScalar();

                if (resultado == null)
                    return "es";

                return resultado.ToString();
            }
            finally
            {
                if (_sqlserver.State ==
                    System.Data.ConnectionState.Open)
                {
                    _sqlserver.Close();
                }

                _sqlcommand.Parameters.Clear();
            }
        }
        public List<Idioma> listarTodosLosIdiomas()
        {
            List<Idioma> lista = new List<Idioma>();
            try
            {
                _sqlcommand.CommandText = @"SELECT UserName, CodigoIdioma, DVH FROM Idioma";
                _sqlcommand.Parameters.Clear();
                _sqlserver.Open();
                using (SqlDataReader reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Idioma idioma = new Idioma
                        {
                            UserName = reader["UserName"].ToString(),
                            CodigoIdioma = reader["CodigoIdioma"].ToString(),
                            DVH = reader["DVH"].ToString()
                        };
                        lista.Add(idioma);
                    }
                }
            }
            finally
            {
                if (_sqlserver.State == System.Data.ConnectionState.Open)
                {
                    _sqlserver.Close();
                }
                _sqlcommand.Parameters.Clear();
            }
            return lista;
        }

        public void GuardarIdioma(
            string userName,
            string codigoIdioma,
            string DVH)
        {
            try
            {
                _sqlcommand.Parameters.Clear();

                _sqlcommand.CommandText = @"
                IF EXISTS
                (
                    SELECT 1
                    FROM Idioma
                    WHERE UserName = @UserName
                )
                BEGIN
                    UPDATE Idioma
                    SET 
                        CodigoIdioma = @CodigoIdioma,
                        DVH = @dvh
                    WHERE UserName = @UserName
                END
                ELSE
                BEGIN
                    INSERT INTO Idioma
                    (
                        UserName,
                        CodigoIdioma,
                        DVH
                    )
                    VALUES
                    (
                        @UserName,
                        @CodigoIdioma,
                        @dvh
                    )
                END";

                _sqlcommand.Parameters.AddWithValue(
                    "@UserName",
                    userName);

                _sqlcommand.Parameters.AddWithValue(
                    "@CodigoIdioma",
                    codigoIdioma);

                _sqlcommand.Parameters.AddWithValue(
                    "@dvh",
                    DVH);

                _sqlserver.Open();

                _sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                if (_sqlserver.State ==
                    System.Data.ConnectionState.Open)
                {
                    _sqlserver.Close();
                }

                _sqlcommand.Parameters.Clear();
            }
        }

        public void ActualizarDVH(string userName, string dvhCalculado)
        {
            try
            {
                _sqlcommand.CommandText = @"UPDATE Idioma SET DVH = @DVH WHERE UserName = @UserName";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@DVH", dvhCalculado);
                _sqlcommand.Parameters.AddWithValue("@UserName", userName);
                _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (_sqlserver.State == System.Data.ConnectionState.Open)
                {
                    _sqlserver.Close();
                }
                _sqlcommand.Parameters.Clear();
            }
        }
    }
}
