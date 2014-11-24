using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.DAL.Controller
{
    internal class CompanyCustomerDataFactory
    {
        public ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData GetCompanyCustomerInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.CompanyCustomer.DAL.MySQLDAO.CompanyCustomer_MySqlDao,ProveedoresOnLine.CompanyCustomer");
            ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData oRetorno = (ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
