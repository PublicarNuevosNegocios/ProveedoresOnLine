using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationPlatform.Models.Integration;
using ProveedoresOnLine.Company.Models.Company;

namespace IntegrationPlatform.DAL.Controller
{
    internal class IntegrationPlatformDataController : IntegrationPlatform.Interfaces.IIntegrationPlatformData
    {
        #region singleton instance

        private static IntegrationPlatform.Interfaces.IIntegrationPlatformData oInstance;

        internal static IntegrationPlatform.Interfaces.IIntegrationPlatformData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new IntegrationPlatformDataController();
                return oInstance;
            }
        }

        private IntegrationPlatform.Interfaces.IIntegrationPlatformData DataFactory;

        #endregion

        #region Constructor

        public IntegrationPlatformDataController()
        {
            IntegrationPlatformDataFactory factory = new IntegrationPlatformDataFactory();
            DataFactory = factory.GetIntegrationPlatformInstance();
        }

        #endregion

        #region Integration

        public CustomDataModel CustomerProvider_GetCustomData(CompanyModel Customer, string ProviderPublicId)
        {
            return DataFactory.CustomerProvider_GetCustomData(Customer, ProviderPublicId);
        }

        #region Integration Sanofi

        public int Sanofi_AditionalData_Upsert(int AditionalDataId, string ProviderPublicId, int AditionalFieldId, string AditionalDataName, bool Enable)
        {
            return DataFactory.Sanofi_AditionalData_Upsert(AditionalDataId, ProviderPublicId, AditionalFieldId, AditionalDataName, Enable);
        }

        public int Sanofi_AditionalDataInfo_Upsert(int AditionalDataInfoId, int AditionalDataId, int? AditionalDataInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.Sanofi_AditionalDataInfo_Upsert(AditionalDataInfoId, AditionalDataId, AditionalDataInfoType, Value, LargeValue, Enable);
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CatalogGetSanofiOptions()
        {
            return DataFactory.CatalogGetSanofiOptions();
        }

        #endregion

        #endregion
    }
}
