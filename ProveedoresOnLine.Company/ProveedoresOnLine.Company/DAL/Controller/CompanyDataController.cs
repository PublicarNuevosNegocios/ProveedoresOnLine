﻿using ProveedoresOnLine.Company.Models.Role;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.DAL.Controller
{
    internal class CompanyDataController : ProveedoresOnLine.Company.Interfaces.ICompanyData
    {
        #region singleton instance

        private static ProveedoresOnLine.Company.Interfaces.ICompanyData oInstance;
        internal static ProveedoresOnLine.Company.Interfaces.ICompanyData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CompanyDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.Company.Interfaces.ICompanyData DataFactory;

        #endregion

        #region Constructor

        public CompanyDataController()
        {
            CompanyDataFactory factory = new CompanyDataFactory();
            DataFactory = factory.GetCompanyInstance();
        }

        #endregion

        #region Util

        public int TreeUpsert(int? TreeId, string TreeName, int TreeType, bool Enable)
        {
            return DataFactory.TreeUpsert(TreeId, TreeName, TreeType, Enable);
        }

        public List<Models.Util.TreeModel> TreeGetByType(int TreeType)
        {
            return DataFactory.TreeGetByType(TreeType);
        }

        public List<Models.Util.TreeModel> TreeGetFullByType(int TreeType)
        {
            return DataFactory.TreeGetFullByType(TreeType);
        }

        public int CategoryUpsert(int? CategoryId, string CategoryName, bool Enable)
        {
            return DataFactory.CategoryUpsert(CategoryId, CategoryName, Enable);
        }

        public int CategoryInfoUpsert(int CategoryId, int? CategoryInfoId, int CategoryInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CategoryInfoUpsert(CategoryId, CategoryInfoId, CategoryInfoType, Value, LargeValue, Enable);
        }

        public void TreeCategoryUpsert(int TreeId, int? ParentCategoryId, int ChildCategoryId, bool Enable)
        {
            DataFactory.TreeCategoryUpsert(TreeId, ParentCategoryId, ChildCategoryId, Enable);
        }

        public int CatalogItemUpsert(int CatalogId, int? ItemId, string Name, bool Enable)
        {
            return DataFactory.CatalogItemUpsert(CatalogId, ItemId, Name, Enable);
        }

        public List<Models.Util.GeographyModel> CategorySearchByGeography(string SearchParam, int? CityId, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByGeography(SearchParam, CityId, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GeographyModel> CategorySearchByGeographyAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByGeographyAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByRules(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByRules(SearchParam, PageNumber, RowCount);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByRulesAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByCompanyRules(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByCompanyRules(SearchParam, PageNumber, RowCount);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByCompanyRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByCompanyRulesAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByResolution(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByResolution(SearchParam, PageNumber, RowCount);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByResolutionAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByResolutionAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByActivity(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByActivity(SearchParam, PageNumber, RowCount);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByCustomActivity(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByCustomActivity(SearchParam, PageNumber, RowCount);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByARLCompany(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByARLCompany(SearchParam, PageNumber, RowCount);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByICA(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByICA(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByBank(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByBank(SearchParam, PageNumber, RowCount);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByBankAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByBankAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GenericItemModel> CategoryGetFinantialAccounts()
        {
            return DataFactory.CategoryGetFinantialAccounts();
        }

        public List<Models.Util.CurrencyExchangeModel> CurrencyExchangeGetByMoneyType(int? MoneyTypeFrom, int? MoneyTypeTo, int? Year)
        {
            return DataFactory.CurrencyExchangeGetByMoneyType(MoneyTypeFrom, MoneyTypeTo, Year);
        }

        public List<Models.Util.GenericItemModel> CategorySearchByEcoActivityAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows)
        {
            return DataFactory.CategorySearchByEcoActivityAdmin(SearchParam, PageNumber, RowCount, TreeId, out TotalRows);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByEcoGroupAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows)
        {
            return DataFactory.CategorySearchByEcoGroupAdmin(SearchParam, PageNumber, RowCount, TreeId, out TotalRows);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByTreeAdmin(string SearchParam, int PageNumber, int RowCount)
        {
            return DataFactory.CategorySearchByTreeAdmin(SearchParam, PageNumber, RowCount);
        }

        public List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> CurrentExchangeGetAllAdmin()
        {
            return DataFactory.CurrentExchangeGetAllAdmin();
        }

        public int CurrencyExchangeInsert(DateTime IssueDate, int MoneyTypeFrom, int MoneyTypeTo, decimal Rate)
        {
            return DataFactory.CurrencyExchangeInsert(IssueDate, MoneyTypeFrom, MoneyTypeTo, Rate);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByCountryAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByCountryAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByStateAdmin(string CountrySearchParam, string StateSearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchByStateAdmin(CountrySearchParam, StateSearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Models.Util.GenericItemModel> CategorySearchBySurveyGroup(int TreeId, string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CategorySearchBySurveyGroup(TreeId, SearchParam, PageNumber, RowCount, out  TotalRows);
        }

        public Models.Util.MinimumWageModel MinimumWageSearchByYear(int Year, int CountryType)
        {
            return DataFactory.MinimumWageSearchByYear(Year, CountryType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CatalogGetAllModuleOptions()
        {
            return DataFactory.CatalogGetAllModuleOptions();
        }

        #endregion

        #region Util MP

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCategorySearchByActivity(int TreeId, string SearchParam, int RowCount)
        {
            return DataFactory.MPCategorySearchByActivity(TreeId, SearchParam, RowCount);
        }

        #endregion

        #region Company CRUD

        public string CompanyUpsert(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, int CompanyType, bool Enable)
        {
            return DataFactory.CompanyUpsert(CompanyPublicId, CompanyName, IdentificationType, IdentificationNumber, CompanyType, Enable);
        }

        public int CompanyInfoUpsert(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CompanyInfoUpsert(CompanyPublicId, CompanyInfoId, CompanyInfoTypeId, Value, LargeValue, Enable);
        }

        public int RoleCompanyUpsert(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable)
        {
            return DataFactory.RoleCompanyUpsert(CompanyPublicId, RoleCompanyId, RoleCompanyName, ParentRoleCompanyId, Enable);
        }

        public int RoleCompanyInfoUpsert(int RoleCompanyId, int? RoleCompanyInfoId, int RoleCompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.RoleCompanyInfoUpsert(RoleCompanyId, RoleCompanyInfoId, RoleCompanyInfoTypeId, Value, LargeValue, Enable);
        }

        public int UserCompanyUpsert(int? UserCompanyId, string User, int RoleCompanyId, bool Enable)
        {
            return DataFactory.UserCompanyUpsert(UserCompanyId, User, RoleCompanyId, Enable);
        }

        public void CompanyFilterFill(string CompanyPublicId)
        {
            DataFactory.CompanyFilterFill(CompanyPublicId);
        }

        public void CompanySearchFill(string CompanyPublicId)
        {
            DataFactory.CompanySearchFill(CompanyPublicId);
        }

        #endregion

        #region Company Search

        public Models.Company.CompanyModel CompanyGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.CompanyGetBasicInfo(CompanyPublicId);
        }

        public List<Models.Util.GenericFilterModel> CompanySearchFilter(string CompanyType, string SearchParam, string SearchFilter)
        {
            return DataFactory.CompanySearchFilter(CompanyType, SearchParam, SearchFilter);
        }

        public List<Models.Company.CompanyModel> CompanySearch(string CompanyType, string SearchParam, string SearchFilter, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CompanySearch(CompanyType, SearchParam, SearchFilter, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Contact

        public int ContactUpsert(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable)
        {
            return DataFactory.ContactUpsert(CompanyPublicId, ContactId, ContactTypeId, ContactName, Enable);
        }

        public int ContactInfoUpsert(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.ContactInfoUpsert(ContactId, ContactInfoId, ContactInfoTypeId, Value, LargeValue, Enable);
        }

        public List<Models.Util.GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType, bool GetAll)
        {
            return DataFactory.ContactGetBasicInfo(CompanyPublicId, ContactType, GetAll);
        }

        #endregion

        #region User Roles

        public ProveedoresOnLine.Company.Models.Company.CompanyModel RoleCompany_GetByPublicId(string CompanyPublicId)
        {
            return DataFactory.RoleCompany_GetByPublicId(CompanyPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Company.UserCompany> RoleCompany_GetUsersByPublicId(string CompanyPublicId, bool ViewEnable)
        {
            return DataFactory.RoleCompany_GetUsersByPublicId(CompanyPublicId, ViewEnable);
        }

        public List<ProveedoresOnLine.Company.Models.Company.CompanyModel> MP_RoleCompanyGetByUser(string User)
        {
            return DataFactory.MP_RoleCompanyGetByUser(User);
        }

        public List<ProveedoresOnLine.Company.Models.Company.CompanyModel> MP_RoleCompanyGetByUserNew(string User)
        {
            return DataFactory.MP_RoleCompanyGetByUserNew(User);
        }

        public List<Models.Company.UserCompany> MP_UserCompanySearch(string CompanyPublicId, string SearchParam, int? RoleCompanyId, int PageNumber, int RowCount)
        {
            return DataFactory.MP_UserCompanySearch(CompanyPublicId, SearchParam, RoleCompanyId, PageNumber, RowCount);
        }

        public int RoleModuleUpsert(int RoleCompanyId, int? RoleModuleId, int RoleModuleType, string RoleModule, bool Enable)
        {
            return DataFactory.RoleModuleUpsert(RoleCompanyId, RoleModuleId, RoleModuleType, RoleModule, Enable);
        }

        public int ModuleOptionUpsert(int RoleModuleId, int? ModuleOptionId, int ModuleOptionType, string ModuleOption, bool Enable)
        {
            return DataFactory.ModuleOptionUpsert(RoleModuleId, ModuleOptionId, ModuleOptionType, ModuleOption, Enable);
        }

        public int ModuleOptionInfoUpsert(int ModuleOptionId, int? ModuleOptionInfoId, int ModuleOptionInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.ModuleOptionInfoUpsert(ModuleOptionId, ModuleOptionInfoId, ModuleOptionInfoType, Value, LargeValue, Enable);
        }

        public int ReportRoleUpsert(int RoleCompanyId, int? ReportRoleId, int ReportRoleType, string ReportRole, bool Enable)
        {
            return DataFactory.ReportRoleUpsert(RoleCompanyId, ReportRoleId, ReportRoleType, ReportRole, Enable);
        }

        public List<RoleCompanyModel> GetRoleCompanySearch(string vSearchParam, bool Enable, out int TotalRows)
        {
            return DataFactory.GetRoleCompanySearch(vSearchParam, Enable, out TotalRows);
        }

        public RoleCompanyModel GetRoleCompanyByRoleCompanyId(int RoleCompanyId)
        {
            return DataFactory.GetRoleCompanyByRoleCompanyId(RoleCompanyId);
        }

        public RoleCompanyModel GetRoleModuleSearch(int RoleCompanyId, bool Enable)
        {
            return DataFactory.GetRoleModuleSearch(RoleCompanyId, Enable);
        }

        public List<GenericItemModel> GetModuleOptionSearch(int RoleModuleId, bool Enable)
        {
            return DataFactory.GetModuleOptionSearch(RoleModuleId, Enable);
        }

        public List<GenericItemInfoModel> GetModuleOptionInfoSearch(int ModuleOptionId, bool Enable)
        {
            return DataFactory.GetModuleOptionInfoSearch(ModuleOptionId, Enable);
        }

        public List<GenericItemModel> GetReportRoleSearch(string vSearch, bool Enable)
        {
            return DataFactory.GetReportRoleSearch(vSearch, Enable);
        }

        #endregion

        #region Restrictive List

        List<Models.Util.GenericItemModel> Interfaces.ICompanyData.BlackListGetByCompanyPublicId(string CompanyPublicId)
        {
            return DataFactory.BlackListGetByCompanyPublicId(CompanyPublicId);
        }
        #endregion

        #region Index

        public void CompanyBasicInfoIndex()
        {
            DataFactory.CompanyBasicInfoIndex();
        }

        public void CompanyActivityInfoIndex()
        {
            DataFactory.CompanyActivityInfoIndex();
        }

        #endregion
    }
}
