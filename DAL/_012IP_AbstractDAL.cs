using Microsoft.Data.SqlClient;
using Servicios;
using System;

namespace DAL
{
    public abstract class _012IP_AbstractDAL<T> where T : class
    {
        protected SqlConnection _sqlserver;
        protected SqlCommand _sqlcommand;

        protected _012IP_AbstractDAL()
        {
            string cadenaConexion = ConexionConfig.ObtenerCadenaConexion();

            _sqlserver = new SqlConnection(cadenaConexion);
            _sqlcommand = new SqlCommand();
            _sqlcommand.Connection = _sqlserver;
            _sqlcommand.CommandType = System.Data.CommandType.Text;
        }
    }
}