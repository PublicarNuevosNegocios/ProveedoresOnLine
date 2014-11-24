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
            throw new NotImplementedException();
        }

        public int UpsertExperienceInfo(int ExperienceId, int? ExperienceInfoId, int ExperienceInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Provider certification

        public int UpsertCertification(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertCertificationInfo(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Provider financial

        public int UpsertFinancial(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertFinancialInfo(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertBalanceSheet(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Provider Legal

        public int UpsertLegal(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertLegalInfo(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
