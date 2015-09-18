using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System.Collections.Generic;

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

        #endregion singleton instance

        #region Constructor

        public CompanyProviderDataController()
        {
            CompanyProvider.DAL.Controller.CompanyProviderDataFactory factory = new CompanyProvider.DAL.Controller.CompanyProviderDataFactory();
            DataFactory = factory.GetCompanyProviderInstance();
        }

        #endregion Constructor

        #region Commercial

        public int CommercialUpsert(string CompanyPublicId, int? CommercialId, int CommercialTypeId, string CommercialName, bool Enable)
        {
            return DataFactory.CommercialUpsert(CompanyPublicId, CommercialId, CommercialTypeId, CommercialName, Enable);
        }

        public int CommercialInfoUpsert(int CommercialId, int? CommercialInfoId, int CommercialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CommercialInfoUpsert(CommercialId, CommercialInfoId, CommercialInfoTypeId, Value, LargeValue, Enable);
        }

        public List<Company.Models.Util.GenericItemModel> CommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, bool Enable)
        {
            return DataFactory.CommercialGetBasicInfo(CompanyPublicId, CommercialType, Enable);
        }

        #endregion Commercial

        #region Certification

        public int CertificationUpsert(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable)
        {
            return DataFactory.CertificationUpsert(CompanyPublicId, CertificationId, CertificationTypeId, CertificationName, Enable);
        }

        public int CertificationInfoUpsert(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CertificationInfoUpsert(CertificationId, CertificationInfoId, CertificationInfoTypeId, Value, LargeValue, Enable);
        }

        public List<Company.Models.Util.GenericItemModel> CertificationGetBasicInfo(string CompanyPublicId, int? CertificationType, bool Enable)
        {
            return DataFactory.CertificationGetBasicInfo(CompanyPublicId, CertificationType, Enable);
        }

        #endregion Certification

        #region Financial

        public int FinancialUpsert(string CompanyPublicId, int? FinancialId, int FinancialTypeId, string FinancialName, bool Enable)
        {
            return DataFactory.FinancialUpsert(CompanyPublicId, FinancialId, FinancialTypeId, FinancialName, Enable);
        }

        public int FinancialInfoUpsert(int FinancialId, int? FinancialInfoId, int FinancialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.FinancialInfoUpsert(FinancialId, FinancialInfoId, FinancialInfoTypeId, Value, LargeValue, Enable);
        }

        public int BalanceSheetUpsert(int FinancialId, int? BalanceSheetId, int AccountId, decimal Value, bool Enable)
        {
            return DataFactory.BalanceSheetUpsert(FinancialId, BalanceSheetId, AccountId, Value, Enable);
        }

        public List<Company.Models.Util.GenericItemModel> FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable)
        {
            return DataFactory.FinancialGetBasicInfo(CompanyPublicId, FinancialType, Enable);
        }

        public List<Models.Provider.BalanceSheetDetailModel> BalanceSheetGetByFinancial(int FinancialId)
        {
            return DataFactory.BalanceSheetGetByFinancial(FinancialId);
        }

        public List<Models.Provider.BalanceSheetDetailModel> BalanceSheetGetCompanyAverage(string CompanyPublicId, int Year)
        {
            return DataFactory.BalanceSheetGetCompanyAverage(CompanyPublicId, Year);
        }

        #endregion Financial

        #region Legal

        public int LegalUpsert(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable)
        {
            return DataFactory.LegalUpsert(CompanyPublicId, LegalId, LegalTypeId, LegalName, Enable);
        }

        public int LegalInfoUpsert(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.LegalInfoUpsert(LegalId, LegalInfoId, LegalInfoTypeId, Value, LargeValue, Enable);
        }

        public List<Company.Models.Util.GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType, bool Enable)
        {
            return DataFactory.LegalGetBasicInfo(CompanyPublicId, LegalType, Enable);
        }

        #endregion Legal

        #region Aditional Documents

        public int AditionalDocumentUpsert(string CompanyPublicId, int? AditionalDocumentId, int AditionalDocumentTypeId, string AditionalDocumentName, bool Enable)
        {
            return DataFactory.AditionalDocumentUpsert(CompanyPublicId, AditionalDocumentId, AditionalDocumentTypeId, AditionalDocumentName, Enable);
        }

        public int AditionalDocumentInfoUpsert(int? AditionalDocumentId, int? AditionalDocumentInfoId, int AditionalDocumentInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.AditionalDocumentInfoUpsert(AditionalDocumentId, AditionalDocumentInfoId, AditionalDocumentInfoTypeId, Value, LargeValue, Enable);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> AditionalDocumentGetBasicInfo(string CompanyPublicId, bool Enable)
        {
            return DataFactory.AditionalDocumentGetBasicInfo(CompanyPublicId, Enable);
        }

        #endregion Aditional Documents

        #region BlackList

        public int BlackListInsert(string CompanyPublicId, int BlackListStatus, string User, string FileUrl)
        {
            return DataFactory.BlackListInsert(CompanyPublicId, BlackListStatus, User, FileUrl);
        }

        public int BlackListInfoInsert(int BlackListId, string BlackListInfoType, string Value)
        {
            return DataFactory.BlackListInfoInsert(BlackListId, BlackListInfoType, Value);
        }

        public List<BlackListModel> BlackListGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.BlackListGetBasicInfo(CompanyPublicId);
        }
        public bool BlackListClearProvider(string CompanyPublicId)
        {
            return DataFactory.BlackListClearProvider(CompanyPublicId);
        }

        #endregion BlackList

        #region Util

        public List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions()
        {
            return DataFactory.CatalogGetProviderOptions();
        }

        #endregion Util

        #region MarketPlace

        #region SearchProviders

        public List<Models.Provider.ProviderModel> MPProviderSearch(string CustomerPublicId, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.MPProviderSearch(CustomerPublicId, SearchParam, SearchFilter, SearchOrderType, OrderOrientation, PageNumber, RowCount, out TotalRows);
        }

        public List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> MPProviderSearchNew(string CustomerPublicId, bool OtherProviders, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.MPProviderSearchNew(CustomerPublicId, OtherProviders, SearchParam, SearchFilter, SearchOrderType, OrderOrientation, PageNumber, RowCount, out TotalRows);
        }

        public List<Company.Models.Util.GenericFilterModel> MPProviderSearchFilter(string CustomerPublicId, string SearchParam, string SearchFilter)
        {
            return DataFactory.MPProviderSearchFilter(CustomerPublicId, SearchParam, SearchFilter);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> MPProviderSearchFilterNew(string CustomerPublicId, string SearchParam, string SearchFilter, bool OtherProviders)
        {
            return DataFactory.MPProviderSearchFilterNew(CustomerPublicId, SearchParam, SearchFilter, OtherProviders);
        }

        #endregion SearchProviders

        public List<Models.Provider.ProviderModel> MPProviderSearchById(string CustomerPublicId, string lstProviderPublicId)
        {
            return DataFactory.MPProviderSearchById(CustomerPublicId, lstProviderPublicId);
        }

        public ProviderModel MPGetBasicInfo(string ProviderPublicId)
        {
            return DataFactory.MPGetBasicInfo(ProviderPublicId);
        }

        public Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            return DataFactory.MPContactGetBasicInfo(CompanyPublicId, ContactType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, string CustomerPublicId)
        {
            return DataFactory.MPCommercialGetBasicInfo(CompanyPublicId, CommercialType, CustomerPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            return DataFactory.MPCertificationGetBasicInfo(CompanyPublicId, CertificationType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType)
        {
            return DataFactory.MPFinancialGetBasicInfo(CompanyPublicId, FinancialType);
        }

        public List<Models.Provider.BalanceSheetModel> MPBalanceSheetGetByYear(string CompanyPublicId, int? Year)
        {
            return DataFactory.MPBalanceSheetGetByYear(CompanyPublicId, Year);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            return DataFactory.MPLegalGetBasicInfo(CompanyPublicId, LegalType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId)
        {
            return DataFactory.MPCustomerProviderGetTracking(CustomerPublicId, ProviderPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPFinancialGetLastyearInfoDeta(string ProviderPublicId)
        {
            return DataFactory.MPFinancialGetLastyearInfoDeta(ProviderPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> MPCertificationGetSpecificCert(string ProviderPublicId)
        {
            return DataFactory.MPCertificationGetSpecificCert(ProviderPublicId);
        }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel MPCustomerProviderGetAllTracking(string CustomerPublicId, string ProviderPublicId)
        {
            return DataFactory.MPCustomerProviderGetAllTracking(CustomerPublicId, ProviderPublicId);
        }

        public List<Company.Models.Util.GenericItemModel> MPReportGetBasicInfo(string CompanyPublicId, int? ReportType)
        {
            return DataFactory.MPReportGetBasicInfo(CompanyPublicId, ReportType);
        }

        public int MPReportUpsert(string CompanyPublicId, int? ReportId, int ReportTypeId, string ReportName, bool Enable)
        {
            return DataFactory.MPReportUpsert(CompanyPublicId, ReportId, ReportTypeId, ReportName, Enable);
        }

        public int MPReportInfoUpsert(int ReportId, int? ReportInfoId, int ReportInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.MPReportInfoUpsert(ReportId, ReportInfoId, ReportInfoTypeId, Value, LargeValue, Enable);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPAditionalDocumentGetBasicInfo(string CustomerPublicId, string ProviderPublicId)
        {
            return DataFactory.MPAditionalDocumentGetBasicInfo(CustomerPublicId, ProviderPublicId);
        }

        #endregion MarketPlace

        #region BatchProcess

        List<Models.Provider.ProviderModel> Interfaces.ICompanyProviderData.BPGetRecruitmentProviders()
        {
            return DataFactory.BPGetRecruitmentProviders();
        }

        #endregion BatchProcess

        #region Charts

        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetProvidersByState(string CompanyPublicId)
        {
            return DataFactory.GetProvidersByState(CompanyPublicId);
        }

        #endregion Charts
    }
}