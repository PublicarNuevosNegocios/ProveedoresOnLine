using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System.Collections.Generic;

namespace ProveedoresOnLine.CompanyProvider.Interfaces
{
    internal interface ICompanyProviderData
    {
        #region Commercial

        int CommercialUpsert(string CompanyPublicId, int? CommercialId, int CommercialTypeId, string CommercialName, bool Enable);

        int CommercialInfoUpsert(int CommercialId, int? CommercialInfoId, int CommercialInfoTypeId, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, bool Enable);

        #endregion Commercial

        #region Certification

        int CertificationUpsert(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable);

        int CertificationInfoUpsert(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CertificationGetBasicInfo(string CompanyPublicId, int? CertificationType, bool Enable);

        #endregion Certification

        #region Financial

        int FinancialUpsert(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable);

        int FinancialInfoUpsert(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable);

        int BalanceSheetUpsert(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable);

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> BalanceSheetGetByFinancial(int FinancialId);

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> BalanceSheetGetCompanyAverage(string CompanyPublicId, int Year);

        #endregion Financial

        #region Legal

        int LegalUpsert(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable);

        int LegalInfoUpsert(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType, bool Enable);

        #endregion Legal 

        #region BlackList

        int BlackListInsert(string CompanyPublicId, int BlackListStatus, string User, string FileUrl);

        int BlackListInfoInsert(int BlackListId, string BlackListInfoType, string Value);

        List<BlackListModel> BlackListGetBasicInfo(string CompanyPublicId);

        bool BlackListClearProvider(string CompanyPublicId);

        #endregion BlackList

        #region Aditional Documents

        int AditionalDocumentUpsert(string CompanyPublicId, int? AditionalDocumentId, int AditionalDocumentTypeId, string AditionalDocumentName, bool Enable);

        int AditionalDocumentInfoUpsert(int? AditionalDocumentId, int? AditionalDocumentInfoId, int AditionalDocumentInfoTypeId, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> AditionalDocumentGetByType(string CompanyPublicId, int? AditionalDocumentType, bool Enable);

        #endregion Aditional Documents

        #region Util

        List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions();

        #endregion Util

        #region MarketPlace

        #region SearchProviders

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> MPProviderSearch(string CustomerPublicId, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> MPProviderSearchNew(string CustomerPublicId, bool OtherProviders, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> MPProviderSearchFilterNew(string CustomerPublicId, string SearchParam, string SearchFilter, bool OtherProviders);

        List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> MPProviderSearchFilter(string CustomerPublicId, string SearchParam, string SearchFilter);

        List<CompanyModel> GetAllProvidersByCustomerPublicId(string CustomerPublicId);
        #endregion SearchProviders

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> MPProviderSearchById(string CustomerPublicId, string lstProviderPublicId);

        ProviderModel MPGetBasicInfo(string ProviderPublicId);

        Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPContactGetBasicInfo(string CompanyPublicId, int? ContactType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, string CustomerPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType);

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> MPBalanceSheetGetByYear(string CompanyPublicId, int? Year);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPFinancialGetLastyearInfoDeta(string ProviderPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> MPCertificationGetSpecificCert(string ProviderPublicId);

        ProveedoresOnLine.Company.Models.Util.GenericItemModel MPCustomerProviderGetAllTracking(string CustomerPublicId, string ProviderPublicId);

        List<GenericItemModel> MPReportGetBasicInfo(string CompanyPublicId, int? ReportType);

        int MPReportUpsert(string CompanyPublicId, int? ReportId, int ReportTypeId, string ReportName, bool Enable);

        int MPReportInfoUpsert(int ReportId, int? ReportInfoId, int ReportInfoTypeId, string Value, string LargeValue, bool Enable);

        List<GenericItemModel> MPAditionalDocumentGetInfoByType(string CustomerPublicId, int? AditionalDocumentType, string ProviderPublicId);

        #endregion MarketPlace

        #region BatchProcess

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> BPGetRecruitmentProviders();

        #endregion BatchProcess

        #region Charts

        List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetProvidersByState(string CompanyPublicId);

        #endregion Charts
    }
}