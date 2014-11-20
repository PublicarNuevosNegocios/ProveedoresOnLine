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

        public int UpsertTree(int? TreeId, string TreeName, bool Enable)
        {
            return DataFactory.UpsertTree(TreeId, TreeName, Enable);
        }

        public int UpsertCategory(int? CategoryId, string CategoryName, bool Enable)
        {
            return DataFactory.UpsertCategory(CategoryId, CategoryName, Enable);
        }

        public int UpsertCategoryInfo(int CategoryId, int? CategoryInfoId, int CategoryInfoType, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertCategoryInfo(CategoryId, CategoryInfoId, CategoryInfoType, Value, LargeValue, Enable);
        }

        public void UpsertTreeCategory(int TreeId, int? ParentCategoryId, int ChildCategoryId, bool Enable)
        {
            DataFactory.UpsertTreeCategory(TreeId, ParentCategoryId, ChildCategoryId, Enable);
        }

        public int UpsertCatalogItem(int CatalogId, int? ItemId, string Name, bool Enable)
        {
            return DataFactory.UpsertCatalogItem(CatalogId, ItemId, Name, Enable);
        }

        #endregion

        #region Company

        public string UpsertCompany(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, ProveedoresOnLine.Company.Models.enumCompanyType CompanyType, bool Enable)
        {
            return DataFactory.UpsertCompany(CompanyPublicId, CompanyName, IdentificationType, IdentificationNumber, CompanyType, Enable);
        }

        public int UpsertCompanyInfo(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertCompanyInfo(CompanyPublicId, CompanyInfoId, CompanyInfoTypeId, Value, LargeValue, Enable);
        }

        public int UpsertContact(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable)
        {
            return DataFactory.UpsertContact(CompanyPublicId, ContactId, ContactTypeId, ContactName, Enable);
        }

        public int UpsertContactInfo(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertContactInfo(ContactId, ContactInfoId, ContactInfoTypeId, Value, LargeValue, Enable);
        }

        public int UpsertRoleCompany(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable)
        {
            return DataFactory.UpsertRoleCompany(CompanyPublicId, RoleCompanyId, RoleCompanyName, ParentRoleCompanyId, Enable);
        }

        public int UpsertRoleCompanyInfo(int RoleCompanyId, int? RoleCompanyInfoId, int RoleCompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            return DataFactory.UpsertRoleCompanyInfo(RoleCompanyId, RoleCompanyInfoId, RoleCompanyInfoTypeId, Value, LargeValue, Enable);
        }

        public int UpsertUserCompany(int? UserCompanyId, string User, int RoleCompanyId, bool Enable)
        {
            return DataFactory.UpsertUserCompany(UserCompanyId, User, RoleCompanyId, Enable);
        }

        #endregion

    }
}
