using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.Interfaces
{
    internal interface ICalificationProjectBatchData
    {
        #region CalificationProjectBatch

        List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool Enable);

        int CalificationProjectUpsert(int vCalificationProjectId, string vCalificatonProjectPublicId, int vCalificationProjectConfigId, int vCompanyId, int vTotalScore, bool vEnable);

        int CalificationProjectItemUpsert(int vCalificationProjectItemId, int vCalificationProjectId, int vCalificationProjectConfigItemId, int vItemScore, bool vEnable);

        int CalificationProjectItemInfoUpsert(int vCalificationProjectItemInfoId, int vCalificationProjectItemId, int vCalificationProjectConfigItemInfoId, int vItemInfoScore, bool vEnable);
        #endregion

        #region CalificationProjectBatchUtil

        ProveedoresOnLine.Company.Models.Util.GenericItemModel LegalModuleInfo(string CompanyPublicId, int LegalInfoType);

        #endregion

    }
}
