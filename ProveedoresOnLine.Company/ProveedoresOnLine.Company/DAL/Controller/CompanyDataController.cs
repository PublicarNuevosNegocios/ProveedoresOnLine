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

        public int TreeUpsert(int? TreeId, string TreeName, bool Enable)
        {
            return DataFactory.TreeUpsert(TreeId, TreeName, Enable);
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

        public List<Models.Util.GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            return DataFactory.ContactGetBasicInfo(CompanyPublicId, ContactType);
        }

        #endregion
    }
}
