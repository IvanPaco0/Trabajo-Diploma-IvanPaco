using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_DAL
{
    public class _012IP_MembresiaDAL : _012IP_AbstractDAL<_012IP_MembresiaBE>
    {
        public _012IP_MembresiaDAL() : base() { }

        public List<_012IP_MembresiaBE> _012IP_ListarMembresias()
        {
            var lista = new List<_012IP_MembresiaBE>();
            try
            {
                _sqlcommand.CommandText =
                    "SELECT IdMembresia, Nombre, CostoMensual, Activa FROM Membresia WHERE Activa = 1 ORDER BY CostoMensual;";

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new _012IP_MembresiaBE
                        {
                            IdMembresia = (int)reader["IdMembresia"],
                            Nombre = (string)reader["Nombre"],
                            CostoMensual = (decimal)reader["CostoMensual"],
                            Activa = (bool)reader["Activa"]
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
