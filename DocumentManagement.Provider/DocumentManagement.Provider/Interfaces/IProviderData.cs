using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Interfaces
{
    interface IProviderData
    {
        string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, DocumentManagement.Provider.Models.Enumerations.enumIdentificationType IdentificationType, string IdentificationNumber, string Email, DocumentManagement.Provider.Models.Enumerations.enumProcessStatus Status);

        string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, DocumentManagement.Provider.Models.Enumerations.enumProviderInfoType ProviderInfoType, string Value, string LargeValue);

        string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue);
    }
}
