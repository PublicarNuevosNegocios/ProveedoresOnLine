using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationPlattaform.SANOFIProcess.Interfaces;
using IntegrationPlattaform.SANOFIProcess.Models;

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


        public List<Models.SanofiGeneralInfoModel> GetInfoByProvider(string vProviderPublicId)
        {
            return DataFactory.GetInfoByProvider(vProviderPublicId);
        }

        public List<Models.SanofiComercialInfoModel> GetComercialInfoByProvider(string vProviderPublicId)
        {
            return DataFactory.GetComercialInfoByProvider(vProviderPublicId);
        }

        public List<Models.SanofiComercialInfoModel> GetComercialBasicInfoByProvider(string vProviderPublicId)
        {
            return DataFactory.GetComercialBasicInfoByProvider(vProviderPublicId);
        }

        public List<Models.SanofiContableInfoModel> GetContableInfoByProvider(string vProviderPublicId)
        {
            return DataFactory.GetContableInfoByProvider(vProviderPublicId);
        }

        public int SanofiProcessLogInsert(string ProviderPublicId, string ProcessName, string FileName, bool SendStatus, bool IsSuccess, bool Enable)
        {
            return DataFactory.SanofiProcessLogInsert(ProviderPublicId, ProcessName, FileName, SendStatus, IsSuccess, Enable);
        }

        public List<Models.SanofiProcessLogModel> GetSanofiProcessLog(bool IsSuccess)
        {
            return DataFactory.GetSanofiProcessLog(IsSuccess);
        }

        public SanofiProcessLogModel GetSanofiLastProcessLog()
        {
            return DataFactory.GetSanofiLastProcessLog();
        }

        #region Sanofi Message Proccess

        public List<SanofiProcessLogModel> GetSanofiProcessLogBySendStatus(bool SendStatus)
        {
            return DataFactory.GetSanofiProcessLogBySendStatus(SendStatus);
        }

        #endregion
    }
}
