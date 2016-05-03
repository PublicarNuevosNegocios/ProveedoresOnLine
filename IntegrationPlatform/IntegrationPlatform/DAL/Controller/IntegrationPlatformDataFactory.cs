using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlatform.DAL.Controller
{
    internal class IntegrationPlatformDataFactory
    {
        public IntegrationPlatform.Interfaces.IIntegrationPlatformData GetIntegrationPlatformInstance()
        {
            Type typetoreturn = Type.GetType("IntegrationPlatform.DAL.MySQLDAO.IntegrationPlatform_MySqlDao,IntegrationPlatform");
            IntegrationPlatform.Interfaces.IIntegrationPlatformData oRetorno = (IntegrationPlatform.Interfaces.IIntegrationPlatformData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }        
    }
}
