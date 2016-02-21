using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListVerificator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListVerificator.Interfaces
{
    internal interface IRestrictiveListVerificatorData
    {
        List<ProviderStatusModel> getProviderByStatus(int Status, string CustomerPublicId);
    }
}
