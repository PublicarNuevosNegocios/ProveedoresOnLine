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

        public int CalificationProjectUpsert(int vCalificationProjectId, string vCalificatonProjectPublicId, int vCalificationProjectConfigId, int vCompanyId, int vTotalScore, bool vEnable)
        {
            throw new NotImplementedException();
        }

        public int CalificationProjectItemUpsert(int vCalificationProjectItemId, int vCalificationProjectId, int vCalificationProjectConfigItemId, int vItemScore, bool vEnable)
        {
            throw new NotImplementedException();
        }

        public int CalificationProjectItemInfoUpsert(int vCalificationProjectItemInfoId, int vCalificationProjectItemId, int vCalificationProjectConfigItemInfoId, int vItemInfoScore, bool vEnable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CalificationProjectBatchUtil

        #region LegalModule

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel LegalModuleInfo(string CompanyPublicId, int LegalInfoType)
        {
            return DataFactory.LegalModuleInfo(CompanyPublicId, LegalInfoType);
        }

        #endregion

        #endregion


        
    }
}
