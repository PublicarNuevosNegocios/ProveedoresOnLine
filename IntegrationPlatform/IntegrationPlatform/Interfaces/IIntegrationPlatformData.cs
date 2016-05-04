using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlatform.Interfaces
{
    internal interface IIntegrationPlatformData
    {
        #region Integration

        Models.Integration.CustomDataModel CustomerProvider_GetCustomData(ProveedoresOnLine.Company.Models.Company.CompanyModel Customer, string ProviderPublicId);

        #region Integration Sanofi

        int Sanofi_AditionalData_Upsert(int AditionalDataId, string ProviderPublicId, int AditionalFieldId, string AditionalDataName, bool Enable);

        int Sanofi_AditionalDataInfo_Upsert(int AditionalDataInfoId, int AditionalDataId, int? AditionalDataInfoType, string Value, string LargeValue, bool Enable);

        #endregion

        #endregion
    }
}
