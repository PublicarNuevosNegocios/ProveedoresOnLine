using ProveedoresOnLine.Company.Models;
using ProveedoresOnLine.Company.Models.Company;
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

        int UpsertTree(int? TreeId, string TreeName, bool Enable);

        int UpsertCategory(int? CategoryId, string CategoryName, bool Enable);

        int UpsertCategoryInfo(int CategoryId, int? CategoryInfoId, int CategoryInfoType, string Value, string LargeValue, bool Enable);

        void UpsertTreeCategory(int TreeId, int? ParentCategoryId, int ChildCategoryId, bool Enable);

        int UpsertCatalogItem(int CatalogId, int? ItemId, string Name, bool Enable);

        #endregion

        #region Company

        string UpsertCompany(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, enumCompanyType CompanyType, bool Enable);

        int UpsertCompanyInfo(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertContact(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable);

        int UpsertContactInfo(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertRoleCompany(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable);

        int UpsertRoleCompanyInfo(int RoleCompanyId, int? RoleCompanyInfoId, int RoleCompanyInfoTypeId, string Value, string LargeValue, bool Enable);

        int UpsertUserCompany(int? UserCompanyId, string User, int RoleCompanyId, bool Enable);

        #endregion
    }
}
