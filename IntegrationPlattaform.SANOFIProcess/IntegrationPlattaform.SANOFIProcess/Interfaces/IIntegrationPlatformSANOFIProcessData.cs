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
        List<SanofiGeneralInfoModel> GetInfo_ByProvider(string vProviderPublicId);
    }
}
