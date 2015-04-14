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

        public List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetAllProjectConfigByCustomerPublicId(string CustomerPublicId, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.GetAllProjectConfigByCustomerPublicId(CustomerPublicId, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Project

        public string ProjectUpsert(string ProjectPublicId, string ProjectName, int ProjectConfigId, int ProjectStatus, bool Enable)
        {
            return DataFactory.ProjectUpsert(ProjectPublicId, ProjectName, ProjectConfigId, ProjectStatus, Enable);
        }

        public int ProjectInfoUpsert(int? ProjectInfoId, string ProjectPublicId, int ProjectInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.ProjectInfoUpsert(ProjectInfoId, ProjectPublicId, ProjectInfoType, Value, LargeValue, Enable);
        }

        public int ProjectCompanyUpsert(string ProjectPublicId, string CompanyPublicId, bool Enable)
        {
            return DataFactory.ProjectCompanyUpsert(ProjectPublicId, CompanyPublicId, Enable);
        }

        public int ProjectCompanyInfoUpsert(int? ProjectCompanyInfoId, int ProjectCompanyId, int? EvaluationItemId, int ProjectCompanyInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.ProjectCompanyInfoUpsert(ProjectCompanyInfoId, ProjectCompanyId, EvaluationItemId, ProjectCompanyInfoType, Value, LargeValue, Enable);
        }

        public List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> MPProjectConfigGetByCustomer(string CustomerPublicId)
        {
            return DataFactory.MPProjectConfigGetByCustomer(CustomerPublicId);
        }

        public List<ProveedoresOnLine.ProjectModule.Models.ProjectModel> ProjectSearch(string CustomerPublicId, string SearchParam, int ProjectStatus, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ProjectSearch(CustomerPublicId, SearchParam, ProjectStatus, PageNumber, RowCount, out  TotalRows);
        }

        public ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetById(string ProjectPublicId, string CustomerPublicId)
        {
            return DataFactory.ProjectGetById(ProjectPublicId, CustomerPublicId);
        }

        public ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetByIdLite(string ProjectPublicId, string CustomerPublicId)
        {
            return DataFactory.ProjectGetByIdLite(ProjectPublicId, CustomerPublicId);
        }

        #endregion
    }
}
