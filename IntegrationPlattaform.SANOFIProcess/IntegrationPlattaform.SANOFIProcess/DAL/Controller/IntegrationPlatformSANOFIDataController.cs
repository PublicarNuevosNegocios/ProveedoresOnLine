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


        public List<Models.SanofiGeneralInfoModel> GetInfoByProvider(string vProviderPublicId, DateTime vStartDate)
        {
            return DataFactory.GetInfoByProvider(vProviderPublicId, vStartDate);
        }

        public List<Models.SanofiComercialInfoModel> GetComercialInfoByProvider(string vProviderPublicId, DateTime vStartDate)
        {
            return DataFactory.GetComercialInfoByProvider(vProviderPublicId, vStartDate);
        }

        public List<Models.SanofiComercialInfoModel> GetComercialBasicInfoByProvider(string vProviderPublicId, DateTime vStartDate)
        {
            return DataFactory.GetComercialBasicInfoByProvider(vProviderPublicId, vStartDate);
        }

        public List<Models.SanofiContableInfoModel> GetContableInfoByProvider(string vProviderPublicId, DateTime vStartDate)
        {
            return DataFactory.GetContableInfoByProvider(vProviderPublicId, vStartDate);
        }

        public int SanofiProcessLogInsert(string ProviderPublicId, string ProcessName, string FileName, bool SendStatus, bool IsSuccess, bool Enable)
        {
            return DataFactory.SanofiProcessLogInsert(ProviderPublicId, ProcessName, FileName, SendStatus, IsSuccess, Enable);
        }

        public int SanofiProcessLogUpdate(int SanofiProcessLogId, string ProviderPublicId, string ProcessName, string FileName, bool IsSuccess, bool SendStatus, bool Enable)
        {
            return DataFactory.SanofiProcessLogUpdate(SanofiProcessLogId, ProviderPublicId, ProcessName, FileName, IsSuccess, SendStatus, Enable);
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
