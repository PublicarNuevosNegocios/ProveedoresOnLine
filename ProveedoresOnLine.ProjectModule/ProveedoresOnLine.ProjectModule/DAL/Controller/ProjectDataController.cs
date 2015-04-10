using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.DAL.Controller
{
    internal class ProjectDataController : ProveedoresOnLine.ProjectModule.Interfaces.IProjectData
    {
        #region singleton instance

        private static ProveedoresOnLine.ProjectModule.Interfaces.IProjectData oInstance;
        internal static ProveedoresOnLine.ProjectModule.Interfaces.IProjectData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ProjectDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.ProjectModule.Interfaces.IProjectData DataFactory;

        #endregion

        #region Constructor

        public ProjectDataController()
        {
            ProjectDataFactory factory = new ProjectDataFactory();
            DataFactory = factory.GetProjectInstance();
        }

        #endregion

        #region Project Config

        public int ProjectConfigUpsert(string CustomerPublicId, int? ProjectConfigId, string ProjectName, bool Enable)
        {
            return DataFactory.ProjectConfigUpsert(CustomerPublicId, ProjectConfigId, ProjectName, Enable);
        }

        public int EvaluationItemUpsert(int? EvaluationItemId, int ProjectConfigId, string EvaluationItemName, int EvaluationItemType, int? ParentEvaluationItem, bool Enable)
        {
            return DataFactory.EvaluationItemUpsert(EvaluationItemId, ProjectConfigId, EvaluationItemName, EvaluationItemType, ParentEvaluationItem, Enable);
        }

        public int EvaluationItemInfoUpsert(int? EvaluationItemInfoId, int EvaluationItemId, int EvaluationItemInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.EvaluationItemInfoUpsert(EvaluationItemInfoId, EvaluationItemId, EvaluationItemInfoType, Value, LargeValue, Enable);
        }

        //public List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetProjectConfigByCustomer(string CustomerPublicId, int PageNumber, int RowCount, out int TotalRows)
        //{
        //    return DataFactory.GetProjectConfigByCustomer(CustomerPublicId, PageNumber, RowCount, out TotalRows);
        //}

        #endregion
    }
}
