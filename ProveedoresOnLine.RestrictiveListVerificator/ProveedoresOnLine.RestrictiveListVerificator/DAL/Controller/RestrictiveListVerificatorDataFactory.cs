using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListVerificator.DAL.Controller
{
    internal class RestrictiveListVerificatorDataFactory
    {
        public ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData GetCompanyInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.Company.DAL.MySQLDAO.Company_MySqlDao,ProveedoresOnLine.Company");
            ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData oRetorno = (ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
