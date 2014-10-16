using DocumentManagementClient.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagementClient.Manager.DAL.Controller
{
    internal class CustomerDataFactory
    {
        public ICustomerData GetCustomerInstance()
        {
            Type typetoreturn = Type.GetType("DocumentManagementClient.Manager.DAL.MySQLDAO.Customer_MySqlDao,DocumentManagementClient.Manager");
            ICustomerData oRetorno = (ICustomerData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
