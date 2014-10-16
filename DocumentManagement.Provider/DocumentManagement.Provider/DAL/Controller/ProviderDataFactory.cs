using DocumentManagement.Provider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.DAL.Controller
{
    internal class ProviderDataFactory
    {
        public IProviderData DocumentManagementProviderInstance()
        {
            Type typetoreturn = Type.GetType("DocumentManagement.Provider.DAL.MySQLDAO.DocumentManagement.Provider_MySqlDao,DocumentManagement.Provider");
            IProviderData oRetorno = (IProviderData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
