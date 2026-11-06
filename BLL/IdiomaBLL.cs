using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class IdiomaBLL
    {
        private readonly IdiomaDAL _idiomaDAL;
        private readonly BitacoraBLL bll;
        private readonly DigitoVerificadorBLL digitoVerificadorBLL = new DigitoVerificadorBLL();
        private const string MODULO_BITACORA = "Idioma";

        public IdiomaBLL()
        {
            _idiomaDAL = new IdiomaDAL();
            bll = new BitacoraBLL();
        }

        public string ObtenerIdioma(string userName)
        {
            return _idiomaDAL.ObtenerIdioma(userName);

        }

        public void GuardarIdioma(string userName, string idioma)
        {
            string cadenaDHV = $"{userName}{idioma}";

            _idiomaDAL.GuardarIdioma(userName, idioma, DigitoVerificador.CalcularDVH(cadenaDHV));
            digitoVerificadorBLL.RecalcularDVV_Idioma();
            Bitacora bita = new Bitacora();
            bita.Criticidad = 5;
            bita.Modulo = MODULO_BITACORA;
            bita.Evento = $"Cambiar idioma - {idioma}";
            bita.Login = userName;
            bll.RegistrarEvento(bita);
        }
    }
}
