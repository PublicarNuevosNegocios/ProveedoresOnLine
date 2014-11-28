using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Interfaces
{
    internal interface ICompanyProviderData
    {
        int ProviderCategoryUpsert(string CompanyPublicId, int CategoryId, bool Enable);

        int ExperienceUpsert(string CompanyPublicId, int? ExperienceId, string ExperienceName, bool Enable);

        int ExperienceInfoUpsert(int ExperienceId, int? ExperienceInfoId, int ExperienceInfoTypeId, string Value, string LargeValue, bool Enable);

        int CertificationUpsert(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable);

        int CertificationInfoUpsert(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable);

        int FinancialUpsert(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable);

        int FinancialInfoUpsert(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable);

        int BalanceSheetUpsert(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable);

        int LegalUpsert(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable);

        int LegalInfoUpsert(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable);

        List<GenericItemModel> CertificationGetBasicInfo(string CompanyPublicId, int? CertificationType);

        List<GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType);

        List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions();
    }
}
