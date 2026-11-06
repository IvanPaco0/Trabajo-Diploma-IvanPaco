using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BLL
{
    public class RespaldoBLL
    {
        RespaldoDAL DAL;
        BitacoraBLL bitacoraBLL;

        public RespaldoBLL()
        {
            DAL = new RespaldoDAL();
            bitacoraBLL = new BitacoraBLL();
        }

        public string GenerarNombreArchivo()
        {
            string fechaHora = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return $"Alejandria_DB_{fechaHora}.bak";
        }

        public List<string> ObtenerListaBackups()
        {
            string rutaCarpetaPrograma = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");

            if (!Directory.Exists(rutaCarpetaPrograma))
            {
                Directory.CreateDirectory(rutaCarpetaPrograma);
            }

            DirectoryInfo directorio = new DirectoryInfo(rutaCarpetaPrograma);

            FileInfo[] archivosBak = directorio.GetFiles("*.bak")
                                               .OrderByDescending(f => f.CreationTime)
                                               .ToArray();

            List<string> nombresDeArchivos = new List<string>();
            foreach (FileInfo archivo in archivosBak)
            {
                nombresDeArchivos.Add(archivo.Name);
            }

            return nombresDeArchivos;
        }

        public string ObtenerRutaBakcup(string nombreArchivo)
        {
            string carpetaBackups = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            return Path.Combine(carpetaBackups, nombreArchivo);
        }

        public string RealizarBackup(string carpetaDestino)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(carpetaDestino))
                    throw new Exception(LanguageManager.Instance.GetTraduction("TextDV14"));

                if (!Directory.Exists(carpetaDestino))
                    throw new Exception(LanguageManager.Instance.GetTraduction("TextDV15"));

                string nombreArchivo = GenerarNombreArchivo();
                string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

                DAL.GenerarBackup(rutaCompleta);
                RegistrarEvento("Backup generado exitosamente: " + nombreArchivo, 1);

                return rutaCompleta;
            }
            catch (Exception ex)
            {
                RegistrarEvento("Error al generar Backup: " + ex.Message, 1);
                throw new Exception(LanguageManager.Instance.GetTraduction("TextDV16"), ex);
            }
        }

        public void RealizarRestore(string rutaArchivoBackup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rutaArchivoBackup))
                    throw new Exception(LanguageManager.Instance.GetTraduction("TextDV17"));

                if (!File.Exists(rutaArchivoBackup))
                    throw new Exception(LanguageManager.Instance.GetTraduction("TextDV18"));

                if (!rutaArchivoBackup.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                    throw new Exception(LanguageManager.Instance.GetTraduction("TextDV19"));

                if (!DAL.ValidarArchivoBackup(rutaArchivoBackup))
                    throw new Exception(LanguageManager.Instance.GetTraduction("TextDV20"));

                DAL.RestaurarBackup(rutaArchivoBackup);
                RegistrarEvento("Restore ejecutado desde: " + Path.GetFileName(rutaArchivoBackup), 1);
            }
            catch (Exception ex)
            {
                RegistrarEvento("Error al ejecutar Restore: " + ex.Message, 1);
                throw new Exception(LanguageManager.Instance.GetTraduction("TextDV21"), ex);
            }
        }

        private void RegistrarEvento(string descripcion, int criticidad)
        {
            try
            {
                string login = SessionManager.Instance?.UsuarioActual()?.Username ?? "Sistema";
                Bitacora evento = new Bitacora
                {
                    Login = login,
                    Modulo = "Respaldos",
                    Evento = descripcion,
                    Criticidad = criticidad
                };
                bitacoraBLL.RegistrarEvento(evento);
            }
            catch { }
        }
    }
}