using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_DAL
{
    public class _012IP_CuotaDAL : _012IP_AbstractDAL<_012IP_CuotaBE>
    {
        public _012IP_CuotaDAL() : base() { }

        /// <summary>Consultar cuotas(IdSuscripcion): devuelve todas las cuotas de la suscripción.</summary>
        public List<_012IP_CuotaBE> _012IP_ObtenerCuotas(int idSuscripcion)
        {
            var cuotas = new List<_012IP_CuotaBE>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT IdCuota, IdSuscripcion, Periodo, Importe, FechaVencimiento, Estado, FechaPago
                    FROM Cuota
                    WHERE IdSuscripcion = @idSuscripcion
                    ORDER BY Periodo;";
                _sqlcommand.Parameters.AddWithValue("@idSuscripcion", idSuscripcion);

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cuotas.Add(new _012IP_CuotaBE
                        {
                            IdCuota = (int)reader["IdCuota"],
                            IdSuscripcion = (int)reader["IdSuscripcion"],
                            Periodo = (DateTime)reader["Periodo"],
                            Importe = (decimal)reader["Importe"],
                            FechaVencimiento = (DateTime)reader["FechaVencimiento"],
                            Estado = (string)reader["Estado"],
                            FechaPago = reader["FechaPago"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["FechaPago"]
                        });
                    }
                }
                return cuotas;
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
