using DocumentManagement.Provider.Interfaces;
using DocumentManagement.Provider.Models;
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
        public string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Enumerations.enumIdentificationType IdentificationType, DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType CustomerProviderInfoType, string IdentificationNumber, string Email, Enumerations.enumProcessStatus Status)
        {
            return DataFactory.ProviderUpsert(CustomerPublicId, ProviderPublicId, Name, IdentificationType, CustomerProviderInfoType, IdentificationNumber, Email, Status);
        }

        public string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, Models.Enumerations.enumProviderInfoType ProviderInfoType, string Value, string LargeValue)
        {
            return DataFactory.ProviderInfoUpsert(ProviderInfoId, ProviderPublicId, ProviderInfoType, Value, LargeValue);
        }

        public string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, Models.Enumerations.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue)
        {
            return DataFactory.ProviderCustomerInfoUpsert(ProviderCustomerInfoId, ProviderPublicId, CustomerPublicId, ProviderCustomerInfoType, Value, LargeValue);
        }
        
        public Models.Provider.ProviderModel GetProviderByIdentificationNumberAndDocumentType(string IdentificationNumber, Models.Enumerations.enumIdentificationType IdenificationType)
        {
            return DataFactory.GetProviderByIdentificationNumberAndDocumentType(IdentificationNumber, IdenificationType);
        }

        public bool GetRelationProviderAndCustomer(string CustomerPublicId, string ProviderPublicId)
        {
            return DataFactory.GetRelationProviderAndCustomer(CustomerPublicId, ProviderPublicId);
        }

        #endregion        
    }
}
