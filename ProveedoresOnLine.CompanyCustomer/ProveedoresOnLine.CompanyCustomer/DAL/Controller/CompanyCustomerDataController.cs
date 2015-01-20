using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.DAL.Controller
{
    internal class CompanyCustomerDataController : ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData
    {
        #region singleton instance

        private static ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData oInstance;
        internal static ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CompanyCustomerDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.CompanyCustomer.Interfaces.ICompanyCustomerData DataFactory;

        #endregion

        #region Constructor

        public CompanyCustomerDataController()
        {
            CompanyCustomer.DAL.Controller.CompanyCustomerDataFactory factory = new CompanyCustomer.DAL.Controller.CompanyCustomerDataFactory();
            DataFactory = factory.GetCompanyCustomerInstance();
        }

        #endregion

        #region Customer Provider

        public int CustomerProviderUpsert(string CustomerPublicId, string ProviderPublicId, int StatusId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int CustomerProviderInfoUpsert(int CustomerProviderId, int? CustomerProviderInfoId, int CustomerProviderInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Survey

        public int SurveyConfigUpsert(string CompanyPublicId, int? SurveyConfigId, string SurveyName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int SurveyItemUpsert(int SurveyConfigId, int? SurveyItemId, string SurveyItemName, int SurveyItemTypeId, int? ParentSurveyItemId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int SurveyItemInfoUpsert(int SurveyItemId, int? SurveyItemInfoId, int SurveyItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Project

        public int ProjectConfigUpsert(string CompanyPublicId, int? ProjectConfigId, string ProjectConfigName, int StatusId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int EvaluationItemUpsert(int ProjectConfigId, int? EvaluationItemId, string EvaluationItemName, int EvaluationItemTypeId, int? ParentEvaluationItemId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int EvaluationItemInfoUpsert(int EvaluationItemId, int? EvaluationItemInfoId, int EvaluationItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
