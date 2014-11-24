using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.DAL.Controller
{
    internal class CompanyProviderDataFactory
    {
        public ProveedoresOnLine.CompanyProvider.Interfaces.ICompanyProviderData GetCompanyProviderInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.CompanyProvider.DAL.MySQLDAO.CompanyProvider_MySqlDao,ProveedoresOnLine.CompanyProvider");
            ProveedoresOnLine.CompanyProvider.Interfaces.ICompanyProviderData oRetorno = (ProveedoresOnLine.CompanyProvider.Interfaces.ICompanyProviderData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
