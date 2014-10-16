using DocumentManagement.Provider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.DAL.Controller
{
    internal class ProviderDataController : IProviderData
    {
        #region Singleton Instance
        private static IProviderData oInstance;
        internal static IProviderData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ProviderDataController();
                return oInstance;
            }
        }

        private IProviderData DataFactory;
        #endregion

        #region Constructor
        public ProviderDataController()
        {
            ProviderDataFactory factory = new ProviderDataFactory();
            DataFactory = factory.DocumentManagementProviderInstance();
        }

        #endregion

        #region Provider
        public string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Models.Enumerations.enumIdentificationType IdentificationType, string IdentificationNumber, string Email, Models.Enumerations.enumProcessStatus Status)
        {
            return DataFactory.ProviderUpsert(CustomerPublicId, ProviderPublicId, Name, IdentificationType, IdentificationNumber, Email, Status);
        }

        public string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, Models.Enumerations.enumProviderInfoType ProviderInfoType, string Value, string LargeValue)
        {
            return DataFactory.ProviderInfoUpsert(ProviderInfoId, ProviderPublicId, ProviderInfoType, Value, LargeValue);
        }

        public string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, Models.Enumerations.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue)
        {
            return DataFactory.ProviderCustomerInfoUpsert(ProviderCustomerInfoId, ProviderPublicId, CustomerPublicId, ProviderCustomerInfoType, Value, LargeValue);
        } 
        #endregion
    }
}
