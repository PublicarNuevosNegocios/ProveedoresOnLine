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

        public int CustomerProviderUpsert(string CustomerPublicId, string ProviderPublicId, int StatusId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatusId", StatusId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_CustomerProvider_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int CustomerProviderInfoUpsert(int CustomerProviderId, int? CustomerProviderInfoId, int CustomerProviderInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderId", CustomerProviderId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderInfoId", CustomerProviderInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderInfoTypeId", CustomerProviderInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_CustomerProviderInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Survey

        public int SurveyConfigUpsert(string CompanyPublicId, int? SurveyConfigId, string SurveyName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyName", SurveyName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_SurveyConfig_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyItemUpsert(int SurveyConfigId, int? SurveyItemId, string SurveyItemName, int SurveyItemTypeId, int? ParentSurveyItemId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemName", SurveyItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemTypeId", SurveyItemTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentSurveyItemId", ParentSurveyItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_SurveyItem_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyItemInfoUpsert(int SurveyItemId, int? SurveyItemInfoId, int SurveyItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemId", SurveyItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemInfoId", SurveyItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemInfoTypeId", SurveyItemInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_SurveyItemInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Project

        public int ProjectConfigUpsert(string CompanyPublicId, int? ProjectConfigId, string ProjectConfigName, int StatusId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigName", ProjectConfigName));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatusId", StatusId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_ProjectConfig_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int EvaluationItemUpsert(int ProjectConfigId, int? EvaluationItemId, string EvaluationItemName, int EvaluationItemTypeId, int? ParentEvaluationItemId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemName", EvaluationItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemTypeId", EvaluationItemTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentEvaluationItemId", ParentEvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_EvaluationItem_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int EvaluationItemInfoUpsert(int EvaluationItemId, int? EvaluationItemInfoId, int EvaluationItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemInfoId", EvaluationItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemInfoTypeId", EvaluationItemInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_EvaluationItemInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion
    }
}
