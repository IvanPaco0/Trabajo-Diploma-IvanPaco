using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _012IP_DAL
{
    public class _012IP_MedioDePagoDAL : _012IP_AbstractDAL<_012IP_MedioDePagoBE>
    {
        public _012IP_MedioDePagoDAL() : base() { }

        public List<_012IP_MedioDePagoBE> _012IP_Listar()
        {
            var medios = new List<_012IP_MedioDePagoBE>();
            try
            {
                _sqlcommand.CommandText = "SELECT IdMedioDePago, Nombre, EsTarjeta FROM MedioDePago ORDER BY IdMedioDePago;";

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medios.Add(new _012IP_MedioDePagoBE
                        {
                            IdMedioDePago = (int)reader["IdMedioDePago"],
                            Nombre = (string)reader["Nombre"],
                            EsTarjeta = (bool)reader["EsTarjeta"]
                        });
                    }
                }
                return medios;
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
