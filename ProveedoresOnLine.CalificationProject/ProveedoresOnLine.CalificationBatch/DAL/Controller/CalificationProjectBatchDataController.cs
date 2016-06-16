using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.DAL.Controller 
{
    internal class CalificationProjectBatchDataController : ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData
    {
        #region singleton instance

        private static ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData oInstance;

        internal static ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CalificationProjectBatchDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.CalificationBatch.Interfaces.ICalificationProjectBatchData DataFactory;

        #endregion

         #region constructor

        public CalificationProjectBatchDataController()
        {
            CalificationBatch.DAL.Controller.CalificationProjectBatchDataFactory factory = new CalificationBatch.DAL.Controller.CalificationProjectBatchDataFactory();
            DataFactory = factory.GetCalificationProjectBatchInstance();
        }

        #endregion

        #region CalificationProjectBatch

        public List<Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool Enable)
        {
            return DataFactory.CalificationProject_GetByCustomer(vCustomerPublicid, vProviderPublicId, Enable);
        }

        public int CalificationProjectUpsert(int vCalificationProjectId, string vCalificatonProjectPublicId, int vCalificationProjectConfigId, string vCompanyPublicId, int vTotalScore, bool vEnable)
        {
            return DataFactory.CalificationProjectUpsert(vCalificationProjectId, vCalificatonProjectPublicId, vCalificationProjectConfigId, vCompanyPublicId, vTotalScore, vEnable);
        }

        public int CalificationProjectItemUpsert(int vCalificationProjectItemId, int vCalificationProjectId, int vCalificationProjectConfigItemId, int vItemScore, bool vEnable)
        {
            return DataFactory.CalificationProjectItemUpsert(vCalificationProjectItemId, vCalificationProjectId, vCalificationProjectConfigItemId, vItemScore, vEnable);
        }

        public int CalificationProjectItemInfoUpsert(int vCalificationProjectItemInfoId, int vCalificationProjectItemId, int vCalificationProjectConfigItemInfoId, int vItemInfoScore, bool vEnable)
        {
            return DataFactory.CalificationProjectItemInfoUpsert(vCalificationProjectItemInfoId, vCalificationProjectItemId, vCalificationProjectConfigItemInfoId, vItemInfoScore, vEnable);
        }

        #endregion

        #region Calification Project Batch Util

        #region Legal Module

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> LegalModuleInfo(string CompanyPublicId, int LegalInfoType)
        {
            return DataFactory.LegalModuleInfo(CompanyPublicId, LegalInfoType);
        }

        #endregion

        #region Financial Module

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> FinancialModuleInfo(string CompanyPublicId, int FinancialInfoType)
        {
            return DataFactory.FinancialModuleInfo(CompanyPublicId, FinancialInfoType);
        }
        
        #endregion

        #endregion
    }
}
