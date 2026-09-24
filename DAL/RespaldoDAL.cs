using Microsoft.Data.SqlClient;
using System;
using System.IO;

namespace DAL
{
    public class Respaldo
    {
    
    }

    public class RespaldoDAL : _012IP_AbstractDAL<Respaldo>
    {
        private const string NombreBaseDatos = "Alejandria_DB";

        public RespaldoDAL() { }

        public void GenerarBackup(string rutaCompletaArchivo)
        {
            try
            {
                string carpeta = Path.GetDirectoryName(rutaCompletaArchivo);
                if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                _sqlcommand.CommandText = $@"BACKUP DATABASE [{NombreBaseDatos}]
                                              TO DISK = @Ruta
                                              WITH FORMAT, INIT, NAME = @NombreRespaldo,
                                              SKIP, NOREWIND, NOUNLOAD, STATS = 10";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@Ruta", rutaCompletaArchivo);
                _sqlcommand.Parameters.AddWithValue("@NombreRespaldo", $"Backup {NombreBaseDatos} - Full");
                _sqlcommand.CommandTimeout = 0;

                _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo generar el respaldo de la base de datos.", ex);
            }
            finally
            {
                _sqlserver.Close();
            }
        }

        public void RestaurarBackup(string rutaCompletaArchivo)
        {
            SqlConnection connMaster = new SqlConnection(
                "Data Source=.;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True");

            try
            {
                if (!File.Exists(rutaCompletaArchivo))
                    throw new FileNotFoundException("No se encontró el archivo de respaldo indicado.", rutaCompletaArchivo);

                connMaster.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connMaster;
                cmd.CommandTimeout = 0;

           
                cmd.CommandText = $@"
            SELECT session_id FROM sys.dm_exec_sessions 
            WHERE database_id = DB_ID('{NombreBaseDatos}')";

                var sesiones = new System.Collections.Generic.List<int>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                        sesiones.Add(reader.GetInt16(0));

                foreach (int sid in sesiones)
                {
                    cmd.CommandText = $"KILL {sid}";
                    try { cmd.ExecuteNonQuery(); } catch { }
                }

                cmd.CommandText = $@"RESTORE DATABASE [{NombreBaseDatos}]
                              FROM DISK = @Ruta
                              WITH REPLACE, STATS = 10";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Ruta", rutaCompletaArchivo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo restaurar la base de datos.", ex);
            }
            finally
            {
                connMaster.Close();
            }
        }
        public bool ValidarArchivoBackup(string rutaCompletaArchivo)
        {
            try
            {
                if (!File.Exists(rutaCompletaArchivo))
                    return false;

                _sqlcommand.CommandText = "RESTORE VERIFYONLY FROM DISK = @Ruta";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@Ruta", rutaCompletaArchivo);
                _sqlcommand.CommandTimeout = 0;

                _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("El archivo seleccionado no es un respaldo válido de SQL Server.", ex);
            }
            finally
            {
                _sqlserver.Close();
            }
        }
    }
}