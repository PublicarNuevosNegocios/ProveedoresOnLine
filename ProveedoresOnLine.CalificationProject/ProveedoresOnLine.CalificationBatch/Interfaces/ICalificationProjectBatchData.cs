using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.Company.Models.Util;

namespace ProveedoresOnLine.CalificationBatch.Interfaces
{
    internal interface ICalificationProjectBatchData
    {
        #region CalificationProjectBatch

        List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool Enable);

        int CalificationProjectUpsert(int vCalificationProjectId, string vCalificatonProjectPublicId, int vCalificationProjectConfigId, string vCompanyPublicId, int vTotalScore, bool vEnable);

        int CalificationProjectItemUpsert(int vCalificationProjectItemId, int vCalificationProjectId, int vCalificationProjectConfigItemId, int vItemScore, bool vEnable);

        int CalificationProjectItemInfoUpsert(int vCalificationProjectItemInfoId, int vCalificationProjectItemId, int vCalificationProjectConfigItemInfoId, int vItemInfoScore, bool vEnable);
        
        #endregion

        #region Calification Project Batch Util

        #region Legal Module Info

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> LegalModuleInfo(string CompanyPublicId, int LegalInfoType);

        #endregion

        #region Financial Module Info

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> FinancialModuleInfo(string CompanyPublicId, int FinancialInfoType);

        #endregion

        #region Commercial Module Info

        List<GenericItemModel> CommercialModuleInfo(string CompanyPublicId, int CommercialInfoType);

        #endregion

        #region HSEQ Module Info

        List<GenericItemModel> CertificationModuleInfo(string CompanyPublicId, int CertificationInfoType);

        #endregion

        #region Balance Module Info

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> BalanceModuleInfo(string CompanyPublicId, int BalanceAccount);

        #endregion

        #endregion

    }
}
