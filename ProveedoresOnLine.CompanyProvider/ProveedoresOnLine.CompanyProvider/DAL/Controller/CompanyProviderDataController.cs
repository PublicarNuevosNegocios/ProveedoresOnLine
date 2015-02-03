﻿using System;
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

        #region Provider Commercial

        public int CommercialUpsert(string CompanyPublicId, int? CommercialId, int CommercialTypeId, string CommercialName, bool Enable)
        {
            return DataFactory.CommercialUpsert(CompanyPublicId, CommercialId, CommercialTypeId, CommercialName, Enable);
        }

        public int CommercialInfoUpsert(int CommercialId, int? CommercialInfoId, int CommercialInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CommercialInfoUpsert(CommercialId, CommercialInfoId, CommercialInfoTypeId, Value, LargeValue, Enable);
        }

        public List<Company.Models.Util.GenericItemModel> CommercialGetBasicInfo(string CompanyPublicId, int? CommercialType)
        {
            return DataFactory.CommercialGetBasicInfo(CompanyPublicId, CommercialType);
        }

        #endregion

        #region Provider certification

        public int CertificationUpsert(string CompanyPublicId, int? CertificationId, int CertificationTypeId, string CertificationName, bool Enable)
        {
            return DataFactory.CertificationUpsert(CompanyPublicId, CertificationId, CertificationTypeId, CertificationName, Enable);
        }

        public int CertificationInfoUpsert(int CertificationId, int? CertificationInfoId, int CertificationInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CertificationInfoUpsert(CertificationId, CertificationInfoId, CertificationInfoTypeId, Value, LargeValue, Enable);
        }

        public List<Company.Models.Util.GenericItemModel> CertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            return DataFactory.CertificationGetBasicInfo(CompanyPublicId, CertificationType);
        }

        #endregion

        #region Provider financial

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

        public List<Company.Models.Util.GenericItemModel> FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType)
        {
            return DataFactory.FinancialGetBasicInfo(CompanyPublicId, FinancialType);
        }

        public List<Models.Provider.BalanceSheetDetailModel> BalanceSheetGetByFinancial(int FinancialId)
        {
            return DataFactory.BalanceSheetGetByFinancial(FinancialId);
        }

        #endregion

        #region Provider Legal

        public int LegalUpsert(string CompanyPublicId, int? LegalId, int LegalTypeId, string LegalName, bool Enable)
        {
            return DataFactory.LegalUpsert(CompanyPublicId, LegalId, LegalTypeId, LegalName, Enable);
        }

        public int LegalInfoUpsert(int LegalId, int? LegalInfoId, int LegalInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.LegalInfoUpsert(LegalId, LegalInfoId, LegalInfoTypeId, Value, LargeValue, Enable);
        }

        public List<Company.Models.Util.GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            return DataFactory.LegalGetBasicInfo(CompanyPublicId, LegalType);
        }

        #endregion

        #region Util

        public List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions()
        {
            return DataFactory.CatalogGetProviderOptions();
        }

        #endregion

        #region MarketPlace

        public List<Models.Provider.ProviderModel> MPProviderSearch(string CustomerPublicId, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.MPProviderSearch(CustomerPublicId, SearchParam, SearchFilter, SearchOrderType, OrderOrientation, PageNumber, RowCount, out TotalRows);
        }

        public List<Company.Models.Util.GenericFilterModel> MPProviderSearchFilter(string CustomerPublicId, string SearchParam, string SearchFilter)
        {
            return DataFactory.MPProviderSearchFilter(CustomerPublicId, SearchParam, SearchFilter);
        }

        public List<Models.Provider.ProviderModel> MPProviderSearchById(string CustomerPublicId, string lstProviderPublicId)
        {
            return DataFactory.MPProviderSearchById(CustomerPublicId, lstProviderPublicId);
        }

        public Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            return DataFactory.MPContactGetBasicInfo(CompanyPublicId, ContactType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCommercialGetBasicInfo(string CompanyPublicId, int? CommercialType)
        {
            return DataFactory.MPCommercialGetBasicInfo(CompanyPublicId, CommercialType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)        
        {
            return DataFactory.MPCertificationGetBasicInfo(CompanyPublicId, CertificationType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType)
        {
            return DataFactory.MPFinancialGetBasicInfo(CompanyPublicId, FinancialType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            return DataFactory.MPLegalGetBasicInfo(CompanyPublicId, LegalType);
        }
        #endregion
    }
}
