using _012IP_BE;
using _012IP_DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BLL
{
    public class _012IP_CuotaBLL
    {
        private readonly _012IP_CuotaDAL cuotaDAL;

        public _012IP_CuotaBLL()
        {
            cuotaDAL = new _012IP_CuotaDAL();
        }

        public List<_012IP_CuotaBE> _012IP_ObtenerCuotas(int idSuscripcion)
        {
            if (idSuscripcion <= 0)
                throw new Exception(LanguageManager.Instance.GetTraduction("CuotaBLLText1"));

            try
            {
                return cuotaDAL._012IP_ObtenerCuotas(idSuscripcion);
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("CuotaBLLText2"), ex);
            }
        }
    }
}
