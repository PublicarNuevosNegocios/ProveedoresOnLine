using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListVerificator.DAL.Controller
{
    internal class RestrictiveListVerificatorDataFactory
    {
        public ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData GetRestrictiveListVerificatorInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.RestrictiveListVerificator.DAL.MySQLDAO.RestrictiveListVerificator_MySqlDao, ProveedoresOnLine.RestrictiveListVerificator");
            ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData oRetorno = (ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
