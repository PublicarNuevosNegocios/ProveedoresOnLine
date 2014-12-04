﻿using System;
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

        public List<Models.Util.GeographyModel> CategorySearchByGeography(string SearchParam, int? CityId, int vPageNumber, int vRowCount)
        {
            return DataFactory.CategorySearchByGeography(SearchParam, CityId, vPageNumber, vRowCount);
        }

        #endregion

        #region Company

        public string CompanyUpsert(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, int CompanyType, bool Enable)
        {
            return DataFactory.CompanyUpsert(CompanyPublicId, CompanyName, IdentificationType, IdentificationNumber, CompanyType, Enable);
        }

        public int CompanyInfoUpsert(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.CompanyInfoUpsert(CompanyPublicId, CompanyInfoId, CompanyInfoTypeId, Value, LargeValue, Enable);
        }

        public int ContactUpsert(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable)
        {
            return DataFactory.ContactUpsert(CompanyPublicId, ContactId, ContactTypeId, ContactName, Enable);
        }

        public int ContactInfoUpsert(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.ContactInfoUpsert(ContactId, ContactInfoId, ContactInfoTypeId, Value, LargeValue, Enable);
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

        public Models.Company.CompanyModel CompanyGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.CompanyGetBasicInfo(CompanyPublicId);
        }

        public List<Models.Util.GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            return DataFactory.ContactGetBasicInfo(CompanyPublicId, ContactType);
        }

        #endregion
    }
}
