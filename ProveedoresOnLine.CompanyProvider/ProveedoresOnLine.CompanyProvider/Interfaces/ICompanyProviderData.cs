using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Interfaces
{
    internal interface ICompanyProviderData
    {
        int CommercialUpsert(string CompanyPublicId, int? CommercialId, int CommercialTypeId, string CommercialName, bool Enable);

        int CommercialInfoUpsert(int CommercialId, int? CommercialInfoId, int CommercialInfoTypeId, string Value, string LargeValue, bool Enable);

        int CertificationUpsert(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable);

        int CertificationInfoUpsert(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable);

        int FinancialUpsert(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable);

        int FinancialInfoUpsert(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable);

        int BalanceSheetUpsert(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable);

        int LegalUpsert(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable);

        int LegalInfoUpsert(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CommercialGetBasicInfo(string CompanyPublicId, int? CommercialType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CertificationGetBasicInfo(string CompanyPublicId, int? CertificationType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType);

        List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions();

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType);

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> BalanceSheetGetByFinancial(int FinancialId);

        #region MarketPlace

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> MPProviderSearch(string CustomerPublicId, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> MPProviderSearchFilter(string CustomerPublicId, string SearchParam, string SearchFilter);

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> MPProviderSearchById(string CustomerPublicId, string lstProviderPublicId);

        Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId);
        #endregion

    }
}
