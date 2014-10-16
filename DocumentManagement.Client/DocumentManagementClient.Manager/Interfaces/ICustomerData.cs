using DocumentManagementClient.Manager.Models;
using DocumentManagementClient.Manager.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagementClient.Manager.Interfaces
{
    interface ICustomerData
    {
        string CustomerUpsert(string CustomerPublicId, string Name, enumIdentificationType IdentificationType, string IdentificationNumber);

        List<CustomerModel> CustomerSearch(string IdentificationNumber, string Name);
    }
}
