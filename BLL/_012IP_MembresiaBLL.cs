using _012IP_BE;
using _012IP_DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace _012IP_BLL
{
    public class _012IP_MembresiaBLL
    {
        private readonly _012IP_MembresiaDAL membresiaDAL;

        public _012IP_MembresiaBLL()
        {
            membresiaDAL = new _012IP_MembresiaDAL();
        }

        public List<_012IP_MembresiaBE> _012IP_ListarMembresias()
        {
            List<_012IP_MembresiaBE> membresias;
            try
            {
                membresias = membresiaDAL._012IP_ListarMembresias();
            }
            catch (Exception ex)
            {
                throw new Exception(LanguageManager.Instance.GetTraduction("MembBLLText2"), ex);
            }

            if (membresias.Count == 0)
                throw new Exception(LanguageManager.Instance.GetTraduction("MembBLLText1"));

            return membresias;
        }
    }
}
