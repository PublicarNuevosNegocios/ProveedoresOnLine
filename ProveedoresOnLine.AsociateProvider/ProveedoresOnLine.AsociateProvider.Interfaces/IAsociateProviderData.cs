using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Interfaces
{
    public interface IAsociateProviderData
    {
        List<ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.AsociateProviderModel> GetAllAsociateProvider(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.AsociateProviderModel> GetAsociateProviderByProviderPublicId(string vProviderPublicIdDM, string vProviderPublicIdBO);

        int AsociateProviderUpsertEmail(int AsociateProviderId, string Email);
    }
}
