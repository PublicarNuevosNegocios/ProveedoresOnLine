using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.DAL.Controller
{
    internal class CompanyCustomerDataController : ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData
    {
        #region singleton instance

        private static ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData oInstance;
        internal static ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CompanyCustomerDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData DataFactory;

        #endregion

        #region Constructor

        public CompanyCustomerDataController()
        {
            CompanyCustomer.DAL.Controller.CompanyCustomerDataFactory factory = new CompanyCustomer.DAL.Controller.CompanyCustomerDataFactory();
            DataFactory = factory.GetCompanyCustomerInstance();
        }

        #endregion

        #region Aditional Documents

        public int AditionalDocumentsUpsert(string CompanyPublicId, int? AditionalDocumentsId, string Title, bool Enable)
        {
            return DataFactory.AditionalDocumentsUpsert(CompanyPublicId, AditionalDocumentsId, Title, Enable);
        }

        public int AditionalDocumentsInfoUpsert(int AditionalDocumentsId, int? AditionalDocumentsInfoId, int AditionalDocumentsType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.AditionalDocumentsInfoUpsert(AditionalDocumentsId, AditionalDocumentsInfoId, AditionalDocumentsType, Value, LargeValue, Enable);
        }

        public ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel GetAditionalDocumentsByCompany(string CustomerPublicId, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.GetAditionalDocumentsByCompany(CustomerPublicId, Enable, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Customer Provider

        public int CustomerProviderUpsert(string CustomerPublicId, string ProviderPublicId, int StatusId, bool Enable)
        {
            return DataFactory.CustomerProviderUpsert(CustomerPublicId, ProviderPublicId, StatusId, Enable);
        }

        public int CustomerProviderInfoUpsert(int CustomerProviderId, int? CustomerProviderInfoId, int CustomerProviderInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CustomerProviderInfoUpsert(CustomerProviderId, CustomerProviderInfoId, CustomerProviderInfoTypeId, Value, LargeValue, Enable);
        }

        public CompanyCustomer.Models.Customer.CustomerModel GetCustomerByProvider(string ProviderPublicId, string vCustomerRelated)
        {
            return DataFactory.GetCustomerByProvider(ProviderPublicId, vCustomerRelated);
        }

        public CompanyCustomer.Models.Customer.CustomerModel GetCustomerInfoByProvider(int CustomerProviderId, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.GetCustomerInfoByProvider(CustomerProviderId, Enable, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Util

        public List<Company.Models.Util.CatalogModel> CatalogGetCustomerOptions()
        {
            return DataFactory.CatalogGetCustomerOptions();
        }

        #endregion
    }
}
