using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.DAL.Controller
{
    internal class CompanyDataFactory
    {
        public ProveedoresOnLine.Company.Interfaces.ICompanyData GetCompanyInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.Company.DAL.MySQLDAO.Company_MySqlDao,ProveedoresOnLine.Company");
            ProveedoresOnLine.Company.Interfaces.ICompanyData oRetorno = (ProveedoresOnLine.Company.Interfaces.ICompanyData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
