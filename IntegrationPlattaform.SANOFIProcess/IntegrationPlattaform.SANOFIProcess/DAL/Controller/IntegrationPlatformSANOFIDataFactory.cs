using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.DAL.Controller
{
    internal class IntegrationPlatformSANOFIDataFactory
    {
        public IntegrationPlattaform.SANOFIProcess.Interfaces.IIntegrationPlatformSANOFIProcessData GetCalificationProjectInstance() 
        {
            Type typetoreturn = Type.GetType("IntegrationPlattaform.SANOFIProcess.DAL.MySqlDAO.SANOFIProcess_MySqlDao,IntegrationPlattaform.SANOFIProcess.");
            IntegrationPlattaform.SANOFIProcess.Interfaces.IIntegrationPlatformSANOFIProcessData oRetorno = (IntegrationPlattaform.SANOFIProcess.Interfaces.IIntegrationPlatformSANOFIProcessData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
