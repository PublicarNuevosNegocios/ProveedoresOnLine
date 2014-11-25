using ProveedoresOnLine.CompanyProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.DAL.MySQLDAO
{
    internal class CompanyProvider_MySqlDao : ICompanyProviderData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CompanyProvider_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CompanyProvider.Models.Constants.C_POL_CompanyProviderConnectionName);
        }

        #region Provider Experience

        public int UpsertProviderCategory(string CompanyPublicId, int CategoryId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertExperience(string CompanyPublicId, int? ExperienceId, string ExperienceName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceId", ExperienceId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceName", ExperienceName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CP_Experience_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int UpsertExperienceInfo(int ExperienceId, int? ExperienceInfoId, int ExperienceInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceId", ExperienceId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceInfoId", ExperienceInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vExperienceInfoTypeId", ExperienceInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_ExperienceInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Provider certification

        public int UpsertCertification(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationId", CertificationId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationTypeId", CertificationTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationName", CertificationName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Certification_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int UpsertCertificationInfo(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationId", CertificationId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationInfoId", CertificationInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationInfoTypeId", CertificationInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_CertificationInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Provider financial

        public int UpsertFinancial(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFunancualId", FinancialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialTypeId", FinancialTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialName", FinancialName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Financial_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int UpsertFinancialInfo(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialId", FinancialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialInfoId", FinancialInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialInfoTypeId", FinancialInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_FinancialInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int UpsertBalanceSheet(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialId", FinancialId));
            lstParams.Add(DataInstance.CreateTypedParameter("vBalanceSheetId", BalanceSheetId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAccountId", AccountId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_BalanceSheet_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Provider Legal

        public int UpsertLegal(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalId", LegalId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalTypeId", LegalTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalName", LegalName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Legal_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int UpsertLegalInfo(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vLegalId", LegalId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalInfoId", LegalInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLelalInfoTypeId", LegalInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CP_Legal_UpSert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion
    }
}
