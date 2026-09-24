using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class DigitoVerificadorDAL : _012IP_AbstractDAL<ControlDVV>
    {
        public List<string> ObtenerTodosLosTotalesTablas(string tablaActual, string nuevoTotalActual)
        {
            List<string> totales = new List<string>();

            // Lista fija con los nombres de todas tus tablas del script de la BD
            string[] tablasDelSistema = { "Usuario", "Bitacora", "Idioma", "Perfil", "Familia" };

            foreach (string tabla in tablasDelSistema)
            {
                if (tabla == tablaActual)
                {
                    totales.Add(nuevoTotalActual); // Usamos el que acabamos de calcular en memoria
                }
                else
                {
                    // Buscamos el que ya estaba guardado en la base de datos para las otras tablas
                    string totalGuardado = ObtenerDVV(tabla, "TOTAL_TABLA") ?? "0";
                    totales.Add(totalGuardado);
                }
            }

            return totales;
        }

        public string? ObtenerDVV(string nomTabla, string nomColumna)
        {
            try
            {
                _sqlcommand.CommandText = @"SELECT DVV
                                            FROM ControlDVV
                                            WHERE NombreTabla = @NomTabla AND ColumnaNombre = @NomColumna";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@NomTabla", nomTabla);
                _sqlcommand.Parameters.AddWithValue("@NomColumna", nomColumna);
                _sqlserver.Open();

                return _sqlcommand.ExecuteScalar()?.ToString();
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
            ;
        }

        public void ActualizarDVV(string nomTabla, string nomColumna, string DVV)
        {
            try
            {
                _sqlcommand.CommandText = @"IF EXISTS (SELECT 1 FROM ControlDVV WHERE NombreTabla = @NomTabla AND ColumnaNombre = @NomColumn)
                                            BEGIN
                                                UPDATE ControlDVV 
                                                SET DVV = @dvv 
                                                WHERE NombreTabla = @NomTabla AND ColumnaNombre = @NomColumn
                                            END
                                            ELSE
                                            BEGIN
                                                INSERT INTO ControlDVV (NombreTabla, ColumnaNombre, DVV) 
                                                VALUES (@NomTabla, @NomColumn, @dvv)
                                            END";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@NomTabla", nomTabla);
                _sqlcommand.Parameters.AddWithValue("@NomColumn", nomColumna);
                _sqlcommand.Parameters.AddWithValue("@dvv", DVV);
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
