using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.DAL.Controller
{
    internal class CompanyProviderDataController : ProveedoresOnLine.CompanyProvider.Interfaces.ICompanyProviderData
    {
        #region singleton instance

        private static ProveedoresOnLine.CompanyProvider.Interfaces.ICompanyProviderData oInstance;
        internal static ProveedoresOnLine.CompanyProvider.Interfaces.ICompanyProviderData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CompanyProviderDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.CompanyProvider.Interfaces.ICompanyProviderData DataFactory;

        #endregion

        #region Constructor

        public CompanyProviderDataController()
        {
            CompanyProvider.DAL.Controller.CompanyProviderDataFactory factory = new CompanyProvider.DAL.Controller.CompanyProviderDataFactory();
            DataFactory = factory.GetCompanyProviderInstance();
        }

        #endregion

        #region Provider Experience

        public int UpsertProviderCategory(string CompanyPublicId, int CategoryId, bool Enable)
        {
            return DataFactory.UpsertProviderCategory(CompanyPublicId, CategoryId, Enable);
        }

        public int UpsertExperience(string CompanyPublicId, int? ExperienceId, string ExperienceName, bool Enable)
        {
            return DataFactory.UpsertExperience(CompanyPublicId, ExperienceId, ExperienceName, Enable);
        }

        public int UpsertExperienceInfo(int ExperienceId, int? ExperienceInfoId, int ExperienceInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertExperienceInfo(ExperienceId, ExperienceInfoId, ExperienceInfoTypeId, Value, LargeValue, Enable);
        }

        #endregion

        #region Provider certification

        public int UpsertCertification(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable)
        {
            return DataFactory.UpsertCertification(CompanyPublicId, CertificationId, CertificationTypeId, CertificationName, Enable);
        }

        public int UpsertCertificationInfo(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertCertificationInfo(CertificationId, CertificationInfoId, CertificationInfoTypeId, Value, LargeValue, Enable);
        }

        #endregion

        #region Provider financial

        public int UpsertFinancial(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable)
        {
            return DataFactory.UpsertFinancial(CompanyPublicId, FinancialId, FinancialTypeId, FinancialName, Enable);
        }

        public int UpsertFinancialInfo(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertFinancialInfo(FinancialId, FinancialInfoId, FinancialInfoTypeId, Value, LargeValue, Enable);
        }

        public int UpsertBalanceSheet(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable)
        {
            return DataFactory.UpsertBalanceSheet(FinancialId, BalanceSheetId, AccountId, Value, Enable);
        }

        #endregion

        #region Provider Legal

        public int UpsertLegal(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable)
        {
            return DataFactory.UpsertLegal(CompanyPublicId, LegalId, LegalTypeId, LegalName, Enable);
        }

        public int UpsertLegalInfo(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertLegalInfo(LegalId, LegalInfoId, LegalInfoTypeId, Value, LargeValue, Enable);
        }

        #endregion
    }
}
