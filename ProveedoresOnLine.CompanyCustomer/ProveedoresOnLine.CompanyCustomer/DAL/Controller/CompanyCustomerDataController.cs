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

        public int UpsertCustomerProvider(string CustomerPublicId, string ProviderPublicId, int StatusId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertCustomerProviderInfo(int CustomerProviderId, int? CustomerProviderInfoId, int CustomerProviderInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Survey

        public int UpsertSurveyConfig(string CompanyPublicId, int? SurveyConfigId, string SurveyName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertSurveyItem(int SurveyConfigId, int? SurveyItemId, string SurveyItemName, int SurveyItemTypeId, int? ParentSurveyItemId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertSurveyItemInfo(int SurveyItemId, int? SurveyItemInfoId, int SurveyItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Project

        public int UpsertProjectConfig(string CompanyPublicId, int? ProjectConfigId, string ProjectConfigName, int StatusId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertEvaluationItem(int ProjectConfigId, int? EvaluationItemId, string EvaluationItemName, int EvaluationItemTypeId, int? ParentEvaluationItemId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertEvaluationItemInfo(int EvaluationItemId, int? EvaluationItemInfoId, int EvaluationItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
