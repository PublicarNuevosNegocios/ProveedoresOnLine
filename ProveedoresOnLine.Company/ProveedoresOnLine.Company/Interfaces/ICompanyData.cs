using ProveedoresOnLine.Company.Models;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Interfaces
{
    internal interface ICompanyData
    {
        #region Util

        int TreeUpsert(int? TreeId, string TreeName, int TreeType, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.TreeModel> TreeGetByType(int TreeType);

        List<ProveedoresOnLine.Company.Models.Util.TreeModel> TreeGetFullByType(int TreeType);

        int CategoryUpsert(int? CategoryId, string CategoryName, bool Enable);

        int CategoryInfoUpsert(int CategoryId, int? CategoryInfoId, int CategoryInfoType, string Value, string LargeValue, bool Enable);

        void TreeCategoryUpsert(int TreeId, int? ParentCategoryId, int ChildCategoryId, bool Enable);

        int CatalogItemUpsert(int CatalogId, int? ItemId, string Name, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByGeography(string SearchParam, int? CityId, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByGeographyAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByRules(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByCompanyRules(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByCompanyRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByResolution(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByActivity(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByCustomActivity(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByARLCompany(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByICA(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByBank(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByBankAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByResolutionAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategoryGetFinantialAccounts();

        List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> CurrencyExchangeGetByMoneyType(int? MoneyTypeFrom, int? MoneyTypeTo, int? Year);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByEcoActivityAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByEcoGroupAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByTreeAdmin(string SearchParam, int PageNumber, int RowCount);

        List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> CurrentExchangeGetAllAdmin();

        int CurrencyExchangeInsert(DateTime IssueDate, int MoneyTypeFrom, int MoneyTypeTo, decimal Rate);

        List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByCountryAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByStateAdmin(string CountrySearchParam, string StateSearchParam, int PageNumber, int RowCount, out int TotalRows);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchBySurveyGroup(int TreeId, string SearchParam, int PageNumber, int RowCount, out int TotalRows);

        ProveedoresOnLine.Company.Models.Util.MinimumWageModel MinimumWageSearchByYear(int Year, int CountryType);

        List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CatalogGetAllModuleOptions();

        #endregion

        #region Util MP

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCategorySearchByActivity(int TreeId, string SearchParam, int RowCount);

        #endregion

        #region Company CRUD

        string CompanyUpsert(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, int CompanyType, bool Enable);

        int CompanyInfoUpsert(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable);

        int RoleCompanyUpsert(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable);

        int RoleCompanyInfoUpsert(int RoleCompanyId, int? RoleCompanyInfoId, int RoleCompanyInfoTypeId, string Value, string LargeValue, bool Enable);

        int UserCompanyUpsert(int? UserCompanyId, string User, int RoleCompanyId, bool Enable);

        void CompanyFilterFill(string CompanyPublicId);

        void CompanySearchFill(string CompanyPublicId);

        #endregion

        #region Company Search

        CompanyModel CompanyGetBasicInfo(string CompanyPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> CompanySearchFilter(string CompanyType, string SearchParam, string SearchFilter);

        List<ProveedoresOnLine.Company.Models.Company.CompanyModel> CompanySearch(string CompanyType, string SearchParam, string SearchFilter, int PageNumber, int RowCount, out int TotalRows);

        #endregion

        #region Contact

        int ContactUpsert(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable);

        int ContactInfoUpsert(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType, bool GetAll);

        #endregion

        #region User Roles

        ProveedoresOnLine.Company.Models.Company.CompanyModel RoleCompany_GetByPublicId(string CompanyPublicId);

        List<ProveedoresOnLine.Company.Models.Company.UserCompany> RoleCompany_GetUsersByPublicId(string CompanyPublicId, bool ViewEnable);

        List<ProveedoresOnLine.Company.Models.Company.CompanyModel> MP_RoleCompanyGetByUser(string User);

        List<ProveedoresOnLine.Company.Models.Company.UserCompany> MP_UserCompanySearch(string CompanyPublicId, string SearchParam, int? RoleCompanyId, int PageNumber, int RowCount);

        int RoleModuleUpsert(int RoleCompanyId, int? RoleModuleId, int RoleModuleType, string RoleModule, bool Enable);

        int ModuleOptionUpsert(int RoleModuleId, int? ModuleOptionId, int ModuleOptionType, string ModuleOption, bool Enable);

        int ModuleOptionInfoUpsert(int ModuleOptionId, int? ModuleOptionInfoId, int ModuleOptionInfoType, string Value, string LargeValue, bool Enable);

        List<RoleCompanyModel> GetRoleCompanySearch(string vSearchParam, bool Enable, out int TotalRows);

        RoleCompanyModel GetRoleModuleSearch(int RoleCompanyId, bool Enable);

        #endregion

        #region Restrictive List

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> BlackListGetByCompanyPublicId(string CompanyPublicId);

        #endregion

        #region Index

        void CompanyBasicInfoIndex();

        void CompanyActivityInfoIndex();

        #endregion
    }
}
