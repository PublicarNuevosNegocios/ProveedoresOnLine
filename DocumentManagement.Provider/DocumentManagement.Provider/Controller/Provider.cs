using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Controller
{
    public class Provider
    {
        static public string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Models.Enumerations.enumIdentificationType IdentificationType, string IdentificationNumber, string Email, Models.Enumerations.enumProcessStatus Status)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderUpsert(CustomerPublicId, ProviderPublicId, Name, IdentificationType, IdentificationNumber, Email, Status);
        }     
    }
}
