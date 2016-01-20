using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Interfaces
{
    internal interface IAsociateProviderClientData
    {
        int DMProviderUpsert(string CustomerPublicId, string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber);

        int BOProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber);
        
        void AP_AsociateProviderUpsert(string BOProviderPublicId, string DMProviderPublicId, string Email);

        ProveedoresOnLine.AsociateProvider.Client.Models.HomologateModel GetHomologateItemBySourceID(Int32 SourceCode);

        ProveedoresOnLine.AsociateProvider.Client.Models.AsociateProviderModel GetAsociateProviderByProviderPublicId(string vProviderPublicIdDM, string vProviderPublicIdBO);

    }
}
