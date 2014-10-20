using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.DAL.Controller
{
    internal class CustomerDataFactory
    {
        public DocumentManagement.Customer.Interfaces.ICustomerData GetCustomerInstance()
        {
            Type typetoreturn = Type.GetType("DocumentManagement.Customer.DAL.MySQLDAO.Customer_MySqlDao,DocumentManagement.Customer");
            DocumentManagement.Customer.Interfaces.ICustomerData oRetorno = (DocumentManagement.Customer.Interfaces.ICustomerData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
