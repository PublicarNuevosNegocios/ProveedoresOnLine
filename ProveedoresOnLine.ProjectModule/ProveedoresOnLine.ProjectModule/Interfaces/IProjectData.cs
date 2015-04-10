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

        //List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetProjectConfigByCustomer(string CustomerPublicId, int PageNumber, int RowCount, out int TotalRows);

        #endregion
    }
}
