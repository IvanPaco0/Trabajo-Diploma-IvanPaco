using DAL;
using Servicios;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BitacoraBLL
    {
        BitacoraDAL DAL;
        DigitoVerificadorBLL digitoVerificadorBLL = new DigitoVerificadorBLL();

        public BitacoraBLL()
        {
            DAL = new BitacoraDAL();
        }

        public void RegistrarEvento(Bitacora unEvento)
        {
            try
            {
                unEvento.Fecha = DateTime.Now;
                // Las columnas de la tabla Bitacora tienen largo fijo (Login 50, Modulo 50, Evento 100).
                // Se recorta antes de calcular el DVH para que el texto guardado y el DVH coincidan.
                unEvento.Login = Recortar(unEvento.Login, 50);
                unEvento.Modulo = Recortar(unEvento.Modulo, 50);
                unEvento.Evento = Recortar(unEvento.Evento, 100);
                string cadenaDVH = unEvento.Login + unEvento.Fecha.ToString("yyyy-MM-dd HH:mm:ss") + unEvento.Modulo + unEvento.Evento + unEvento.Criticidad.ToString();

                DAL.RegistrarEvento(unEvento, DigitoVerificador.CalcularDVH(cadenaDVH));
                digitoVerificadorBLL.RecalcularDVV_Bitacora();
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitBlltext1"), ex);
            }
        }


        private static string Recortar(string texto, int largo)
        {
            texto = texto ?? string.Empty;
            return texto.Length <= largo ? texto : texto.Substring(0, largo - 3) + "...";
        }

        public List<Bitacora> FiltrarEventos(string nombre, string apellido, string login, string modulo, string evento, DateTime desde, DateTime hasta, int? criticidad)
        {
            try
            {
                if (desde > hasta)
                    throw new Exception(LanguageManager.Instance.GetTraduction("BitBlltext2"));

                if (modulo == "Todos") modulo = null;

                return DAL.FiltrarEventos(nombre, apellido, login, modulo, evento, desde, hasta, criticidad);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitBlltext3"), ex);
            }
        }

        public List<string> ObtenerLogins()
        {
            try
            {
                return DAL.ObtenerLogins();
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitBlltext4"), ex);
            }
        }

        public Usuario ObtenerUsuarioPorLogin(string login)
        {
            try
            {
                return DAL.ObtenerUsuarioPorLogin(login);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("BitBlltext5"), ex);
            }
        }
    }
}