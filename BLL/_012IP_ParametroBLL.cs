using _012IP_DAL;
using Servicios;
using System;

namespace _012IP_BLL
{
    /// <summary>Valores configurables del sistema. Se leen de la tabla Parametro, no estan escritos en las BLL.</summary>
    public class _012IP_ParametroBLL
    {
        private readonly _012IP_ParametroDAL parametroDAL;

        public _012IP_ParametroBLL()
        {
            parametroDAL = new _012IP_ParametroDAL();
        }

        public int _012IP_DiasAvisoVencimiento() => _012IP_ObtenerEntero("DiasAvisoVencimiento");

        public int _012IP_MaximoMesesPorPago() => _012IP_ObtenerEntero("MaximoMesesPorPago");

        public int _012IP_MaximoFilasReporteSocios() => _012IP_ObtenerEntero("MaximoFilasReporteSocios");

        public int _012IP_MaximoResultadosBusquedaSocios() => _012IP_ObtenerEntero("MaximoResultadosBusquedaSocios");

        public int _012IP_MaximoFilasBitacoraCambios() => _012IP_ObtenerEntero("MaximoFilasBitacoraCambios");

        public string _012IP_MotivoBajaPorPerdida() => _012IP_Obtener("MotivoBajaPorPerdida");

        public string _012IP_MotivoDeterioroEnAuditoria() => _012IP_Obtener("MotivoDeterioroEnAuditoria");

        private string _012IP_Obtener(string nombre)
        {
            string? valor;
            try
            {
                valor = parametroDAL._012IP_ObtenerValor(nombre);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(LanguageManager.Instance.GetTraduction("ParamErrorLectura"), nombre), ex);
            }

            if (string.IsNullOrWhiteSpace(valor))
                throw new Exception(string.Format(LanguageManager.Instance.GetTraduction("ParamNoEncontrado"), nombre));

            return valor!.Trim();
        }

        private int _012IP_ObtenerEntero(string nombre)
        {
            if (!int.TryParse(_012IP_Obtener(nombre), out int valor) || valor <= 0)
                throw new Exception(string.Format(LanguageManager.Instance.GetTraduction("ParamInvalido"), nombre));

            return valor;
        }
    }
}
