using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Interfaces
{
    internal interface ICompanyProviderData
    {
        int UpsertProviderCategory(string CompanyPublicId, int CategoryId, bool Enable);

        int UpsertExperience(string CompanyPublicId, int? ExperienceId, string ExperienceName, bool Enable);

        int UpsertExperienceInfo(int ExperienceId, int? ExperienceInfoId, int ExperienceInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertCertification(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable);

        int UpsertCertificationInfo(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertFinancial(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable);

        int UpsertFinancialInfo(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertBalanceSheet(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable);

        int UpsertLegal(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable);

        int UpsertLegalInfo(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable);
    }
}
