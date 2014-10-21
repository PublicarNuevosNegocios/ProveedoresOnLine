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

        public string ProviderUpsert(string ProviderPublicId, string Name, int IdentificationTypeId, string IdentificationNumber, string Email)
        {
            return DataFactory.ProviderUpsert(ProviderPublicId, Name, IdentificationTypeId, IdentificationNumber, Email);
        }

        public int ProviderInfoUpsert(string ProviderPublicId, int? ProviderInfoId, int ProviderInfoTypeId, string Value, string LargeValue)
        {
            return DataFactory.ProviderInfoUpsert(ProviderPublicId, ProviderInfoId, ProviderInfoTypeId, Value, LargeValue);
        }

        public int ProviderCustomerInfoUpsert(string ProviderPublicId, string CustomerPublicId, int? ProviderCustomerInfoId, int ProviderCustomerInfoTypeId, string Value, string LargeValue)
        {
            return DataFactory.ProviderCustomerInfoUpsert(ProviderPublicId, CustomerPublicId, ProviderCustomerInfoId, ProviderCustomerInfoTypeId, Value, LargeValue);
        }

        public List<Models.Provider.ProviderModel> ProviderSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ProviderSearch(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public Models.Provider.ProviderModel ProviderGetByIdentification(string IdentificationNumber, int IdenificationTypeId, string CustomerPublicId)
        {
            return DataFactory.ProviderGetByIdentification(IdentificationNumber, IdenificationTypeId, CustomerPublicId);
        }

        public Models.Provider.ProviderModel ProviderGetById(string ProviderPublicId, int? StepId)
        {
            return DataFactory.ProviderGetById(ProviderPublicId, StepId);
        }
    }
}
