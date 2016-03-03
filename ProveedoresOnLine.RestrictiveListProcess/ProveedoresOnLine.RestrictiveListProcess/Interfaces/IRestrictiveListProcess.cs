using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
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

        List<RestrictiveListProcessModel> GetAllProvidersInProcess();

        int BlackListProcessUpsert(int BlackListProcessId, string FilePath, bool ProcessStatus, bool IsSuccess, string ProviderStatus, bool Enable);
        
    }
}
