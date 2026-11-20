using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;

namespace _012IP_DAL
{
    public class _012IP_ParametroDAL : _012IP_AbstractDAL<_012IP_ParametroBE>
    {
        public _012IP_ParametroDAL() : base() { }

        /// <summary>Valor guardado en la tabla Parametro; null si el parametro no existe.</summary>
        public string? _012IP_ObtenerValor(string nombre)
        {
            try
            {
                _sqlcommand.CommandText = "SELECT Valor FROM Parametro WHERE Nombre = @nombre;";
                _sqlcommand.Parameters.AddWithValue("@nombre", nombre);

                _sqlserver.Open();
                object valor = _sqlcommand.ExecuteScalar();
                return valor == null || valor == DBNull.Value ? null : Convert.ToString(valor);
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
