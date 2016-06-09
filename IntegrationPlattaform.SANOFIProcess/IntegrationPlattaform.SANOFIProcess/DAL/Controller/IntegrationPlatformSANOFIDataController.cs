
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationPlattaform.SANOFIProcess.Interfaces;

namespace IntegrationPlattaform.SANOFIProcess.DAL.Controller
{
    internal class IntegrationPlatformSANOFIDataController : IIntegrationPlatformSANOFIProcessData
    {
        #region singleton instance

        private static IntegrationPlattaform.SANOFIProcess.Interfaces.IIntegrationPlatformSANOFIProcessData oInstance;

        internal static IntegrationPlattaform.SANOFIProcess.Interfaces.IIntegrationPlatformSANOFIProcessData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new IntegrationPlatformSANOFIDataController();
                return oInstance;
            }
        }

        private IntegrationPlattaform.SANOFIProcess.Interfaces.IIntegrationPlatformSANOFIProcessData DataFactory;

        #endregion

        #region constructor

        public IntegrationPlatformSANOFIDataController()
        {
            IntegrationPlattaform.SANOFIProcess.DAL.Controller.IntegrationPlatformSANOFIDataFactory factory = new IntegrationPlatformSANOFIDataFactory();
            DataFactory = factory.GetCalificationProjectInstance();
        }

        #endregion


        public List<Models.SanofiGeneralInfoModel> GetInfo_ByProvider(string vProviderPublicId)
        {
            return DataFactory.GetInfo_ByProvider(vProviderPublicId);
        }
    }
}
