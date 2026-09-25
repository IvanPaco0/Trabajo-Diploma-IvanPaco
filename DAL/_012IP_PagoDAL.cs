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

        /// <summary>
        /// RegistrarPago(Pago, DVH, IdCuota): CU-03 paso 7. Inserta el pago con su DVH ya
        /// calculado por el BLL (nunca se persiste el pago sin su DVH). Al insertar completa
        /// pago.IdPago con el identity generado por la base.
        /// Columnas reales de la tabla Pago: IdPago, IdSocio, IdCuota, IdMembresia, Monto,
        /// MedioDePago, FechaPago, DVH.
        /// </summary>
        public bool _012IP_RegistrarPago(_012IP_PagoBE pago, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    INSERT INTO Pago (IdSocio, IdCuota, IdMembresia, Monto, MedioDePago, FechaPago, DVH)
                    OUTPUT INSERTED.IdPago
                    VALUES (@idSocio, @idCuota, @idMembresia, @monto, @medioDePago, @fechaPago, @dvh);";

                _sqlcommand.Parameters.AddWithValue("@idSocio", pago.IdSocio);
                _sqlcommand.Parameters.AddWithValue("@idCuota", (object)pago.IdCuota ?? DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@idMembresia", (object)pago.IdMembresia ?? DBNull.Value);
                _sqlcommand.Parameters.AddWithValue("@monto", pago.Monto);
                _sqlcommand.Parameters.AddWithValue("@medioDePago", pago.MedioDePago);
                _sqlcommand.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);

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

        /// <summary>
        /// ActualizarEstadoPago(): CU-03 paso 9. Marca la cuota como Pagada con la fecha del
        /// pago. Solo se llama cuando el pago corresponde a una cuota ya existente
        /// (regularización de deuda); en una renovación nueva idCuota llega null y este
        /// paso se omite (lo controla el BLL).
        /// </summary>
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
    }
}
