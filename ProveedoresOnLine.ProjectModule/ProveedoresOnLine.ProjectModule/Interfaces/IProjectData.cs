using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.Interfaces
{
    internal interface IProjectData
    {
        #region Project Config

        int ProjectConfigUpsert(string CustomerPublicId, int? ProjectConfigId, string ProjectName, bool Enable);

        int EvaluationItemUpsert(int? EvaluationItemId, int ProjectConfigId, string EvaluationItemName, int EvaluationItemType, int? ParentEvaluationItem, bool Enable);

        int EvaluationItemInfoUpsert(int? EvaluationItemInfoId, int EvaluationItemId, int EvaluationItemInfoType, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetAllProjectConfigByCustomerPublicId(string CustomerPublicId, int PageNumber, int RowCount, out int TotalRows);

        #endregion

        #region Project

        string ProjectUpsert(string ProjectPublicId, string ProjectName, int ProjectConfigId, int ProjectStatus, bool Enable);

        int ProjectInfoUpsert(int? ProjectInfoId, string ProjectPublicId, int ProjectInfoType, string Value, string LargeValue, bool Enable);

        int ProjectCompanyUpsert(string ProjectPublicId, string CompanyPublicId, bool Enable);

        int ProjectCompanyInfoUpsert(int? ProjectCompanyInfoId, int ProjectCompanyId, int? EvaluationItemId, int ProjectCompanyInfoType, string Value, string LargeValue, bool Enable);

        #endregion
    }
}
