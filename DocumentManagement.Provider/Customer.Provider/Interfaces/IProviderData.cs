using Customer.Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Provider.Interfaces
{
    interface IProviderData
    {
        string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, enumIdentificationType IdentificationType, string IdentificationNumber, string Email, enumProcessStatus Status);

        string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, enumProviderInfoType ProviderInfoType, string Value, string LargeValue);

        string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue);

    }
}
