using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Interfaces
{
    internal interface ICompanyCustomerData
    {
        int CustomerProviderUpsert(string CustomerPublicId, string ProviderPublicId, int StatusId, bool Enable);

        int CustomerProviderInfoUpsert(int CustomerProviderId, int? CustomerProviderInfoId, int CustomerProviderInfoTypeId, string Value, string LargeValue, bool Enable);

        CompanyCustomer.Models.Customer.CustomerModel GetCustomerByProvider(string ProviderPublicId, string vCustomerRelated);

        CompanyCustomer.Models.Customer.CustomerModel GetCustomerInfoByProvider(int CustomerProviderId, bool Enable, int PageNumber, int RowCount, out int TotalRows);

        List<Company.Models.Util.CatalogModel> CatalogGetCustomerOptions();

        #region Integration

        List<ProveedoresOnLine.Company.Models.Company.CompanyModel> GetCustomerProviderByCustomData(string ProviderPublicId);

        #endregion

        #region Aditional Documents

        int AditionalDocumentsUpsert(string CompanyPublicId, int? AditionalDocumentsId, string Title, bool Enable);

        int AditionalDocumentsInfoUpsert(int AditionalDocumentsId, int? AditionalDocumentsInfoId, int AditionalDocumentsType, string Value, string LargeValue, bool Enable);

        ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel GetAditionalDocumentsByCompany(string CustomerPublicId, bool Enable, int PageNumber, int RowCount, out int TotalRows);

        #endregion
    }
}
