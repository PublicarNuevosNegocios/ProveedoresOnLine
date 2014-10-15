using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagementClient.Manager.Controller
{
    public class Customer
    {
        public static string CustomerUpsert(string CustomerPublicId, string Name, Models.enumIdentificationType IdentificationType, string IdentificationNumber)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerUpsert(CustomerPublicId, Name, IdentificationType, IdentificationNumber);
        }
    }
}
