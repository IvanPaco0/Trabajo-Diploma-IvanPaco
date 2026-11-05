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
                    INSERT INTO Pago (IdSocio, IdCuota, IdMembresia, Monto, MedioDePago, FechaPago, DVH, MarcaTarjeta, UltimosDigitos)
                    OUTPUT INSERTED.IdPago
                    VALUES (@idSocio, @idCuota, @idMembresia, @monto, @medioDePago, @fechaPago, @dvh, @marca, @ultimos);";

                _sqlcommand.Parameters.AddWithValue("@idSocio", pago.IdSocio);
                _sqlcommand.Parameters.AddWithValue("@idCuota", (object)pago.IdCuota ?? DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@idMembresia", (object)pago.IdMembresia ?? DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@monto", pago.Monto);
                _sqlcommand.Parameters.AddWithValue("@medioDePago", pago.MedioDePago);
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

        
        public bool _012IP_ActualizarEstadoPago(int idCuota, DateTime fechaPago)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    UPDATE Cuota
                    SET Estado = 'Pagada',
                        FechaPago = @fechaPago
                    WHERE IdCuota = @idCuota;";

                _sqlcommand.Parameters.AddWithValue("@idCuota", idCuota);
                _sqlcommand.Parameters.AddWithValue("@fechaPago", fechaPago);

                _sqlserver.Open();
                int filas = _sqlcommand.ExecuteNonQuery();
                return filas > 0;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }
    
        // Pagos registrados en el período (informe de recaudación, CUN-05).
        public List<_012IP_PagoBE> _012IP_ListarPagos(DateTime desde, DateTime hasta)
        {
            var lista = new List<_012IP_PagoBE>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT p.IdPago, p.IdSocio, p.IdCuota, p.IdMembresia, p.Monto, p.MedioDePago, p.FechaPago,
                           ISNULL(m.Nombre, '') AS NombreMembresia
                    FROM Pago p
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
                            IdCuota = reader["IdCuota"] == DBNull.Value ? (int?)null : (int)reader["IdCuota"],
                            IdMembresia = reader["IdMembresia"] == DBNull.Value ? (int?)null : (int)reader["IdMembresia"],
                            Monto = (decimal)reader["Monto"],
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
