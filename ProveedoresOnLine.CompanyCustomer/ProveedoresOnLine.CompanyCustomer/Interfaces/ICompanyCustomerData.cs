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

        CompanyCustomer.Models.Customer.CustomerModel GetCustomerByProvider(string ProviderPublicId, int vCustomerRelated, bool Enable);

        CompanyCustomer.Models.Customer.CustomerModel GetCustomerInfoByProvider(int CustomerProviderId, bool Enable);

        List<Company.Models.Util.CatalogModel> CatalogGetCustomerOptions();
    }
}
