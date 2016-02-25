using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Interfaces
{
    internal interface IRestrictiveListProcess
    {
        List<CompanyModel> GetProviderByStatus(int Status, string CustomerPublicId);
    }
}
