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
        List<SanofiGeneralInfoModel> GetInfoByProvider(string vProviderPublicId);

        List<SanofiComercialInfoModel> GetComercialInfoByProvider(string vProviderPublicId);

        List<SanofiComercialInfoModel> GetComercialBasicInfoByProvider(string vProviderPublicId);

        List<SanofiContableInfoModel> GetContableInfoByProvider(string vProviderPublicId);

        int SanofiProcessLogInsert(string ProviderPublicId, string ProcessName, bool IsSuccess, bool Enable);

        List<SanofiProcessLogModel> GetSanofiProcessLog(bool IsSuccess);

        SanofiProcessLogModel GetSanofiLastProcessLog();
    }    
}
