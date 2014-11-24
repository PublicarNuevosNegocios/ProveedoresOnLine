using ProveedoresOnLine.CompanyCustomer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.DAL.MySQLDAO
{
    internal class CompanyCustomer_MySqlDao : ICompanyCustomerData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CompanyCustomer_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CompanyCustomer.Models.Constants.C_POL_CompanyCustomerConnectionName);
        }

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
