using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_DAL
{
    public class _012IP_PagoDAL : _012IP_AbstractDAL<_012IP_PagoBE>
    {
        public _012IP_PagoDAL() : base() { }

      
        public bool _012IP_RegistrarPago(_012IP_PagoBE pago, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    INSERT INTO Pago (IdSocio, IdMembresia, Monto, IdMedioDePago, FechaPago, DVH, MarcaTarjeta, UltimosDigitos)
                    OUTPUT INSERTED.IdPago
                    VALUES (@idSocio, @idMembresia, @monto, @medioDePago, @fechaPago, @dvh, @marca, @ultimos);";

                _sqlcommand.Parameters.AddWithValue("@idSocio", pago.IdSocio);
                _sqlcommand.Parameters.AddWithValue("@idMembresia", (object)pago.IdMembresia ?? DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@monto", pago.Monto);
                _sqlcommand.Parameters.AddWithValue("@medioDePago", pago.IdMedioDePago);
                _sqlcommand.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);
                _sqlcommand.Parameters.AddWithValue("@marca", (object)pago.MarcaTarjeta ?? DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@ultimos", (object)pago.UltimosDigitos ?? DBNull.Value);

                _sqlserver.Open();
                object idGenerado = _sqlcommand.ExecuteScalar();

                if (idGenerado == null || idGenerado == DBNull.Value)
                    return false;

                pago.IdPago = Convert.ToInt32(idGenerado);
                return true;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }


        public List<_012IP_PagoBE> _012IP_ListarPagos(DateTime desde, DateTime hasta)
        {
            var lista = new List<_012IP_PagoBE>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT p.IdPago, p.IdSocio, p.IdMembresia, p.Monto, p.IdMedioDePago, mp.Nombre AS MedioDePago, p.FechaPago,
                           ISNULL(m.Nombre, '') AS NombreMembresia
                    FROM Pago p
                    INNER JOIN MedioDePago mp ON mp.IdMedioDePago = p.IdMedioDePago
                    LEFT JOIN Membresia m ON m.IdMembresia = p.IdMembresia
                    WHERE p.FechaPago >= @desde AND p.FechaPago < @hasta
                    ORDER BY p.FechaPago;";
                _sqlcommand.Parameters.AddWithValue("@desde", desde.Date);
                _sqlcommand.Parameters.AddWithValue("@hasta", hasta.Date.AddDays(1));

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new _012IP_PagoBE
                        {
                            IdPago = (int)reader["IdPago"],
                            IdSocio = (int)reader["IdSocio"],
                            IdMembresia = reader["IdMembresia"] == DBNull.Value ? (int?)null : (int)reader["IdMembresia"],
                            Monto = (decimal)reader["Monto"],
                            IdMedioDePago = (int)reader["IdMedioDePago"],
                            MedioDePago = (string)reader["MedioDePago"],
                            FechaPago = (DateTime)reader["FechaPago"],
                            NombreMembresia = (string)reader["NombreMembresia"]
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
