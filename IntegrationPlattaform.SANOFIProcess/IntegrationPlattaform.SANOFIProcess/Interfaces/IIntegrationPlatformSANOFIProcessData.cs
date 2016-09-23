using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationPlattaform.SANOFIProcess.Models;

namespace IntegrationPlattaform.SANOFIProcess.Interfaces
{
    internal interface IIntegrationPlatformSANOFIProcessData
    {
        List<SanofiGeneralInfoModel> GetInfoByProvider(string vProviderPublicId, DateTime vStartDate);

        List<SanofiComercialInfoModel> GetComercialInfoByProvider(string vProviderPublicId, DateTime vStartDate);

        List<SanofiComercialInfoModel> GetComercialBasicInfoByProvider(string vProviderPublicId, DateTime vStartDate);

        List<SanofiContableInfoModel> GetContableInfoByProvider(string vProviderPublicId, DateTime vStartDate);

        int SanofiProcessLogInsert(string ProviderPublicId, string ProcessName, string FileName, bool SendStatus, bool IsSuccess, bool Enable);

        int SanofiProcessLogUpdate(int SanofiProcessLogId, string ProviderPublicId, string ProcessName, string FileName, bool IsSuccess, bool SendStatus, bool Enable);

        List<SanofiProcessLogModel> GetSanofiProcessLog(bool IsSuccess);

        SanofiProcessLogModel GetSanofiLastProcessLog();

        #region Sanofi Message Proccess

        List<SanofiProcessLogModel> GetSanofiProcessLogBySendStatus(bool SendStatus);

        #endregion
    }    
}
