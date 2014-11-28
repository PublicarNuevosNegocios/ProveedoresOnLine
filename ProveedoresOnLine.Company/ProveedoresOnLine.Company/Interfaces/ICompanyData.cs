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

        int TreeUpsert(int? TreeId, string TreeName, bool Enable);

        int CategoryUpsert(int? CategoryId, string CategoryName, bool Enable);

        int CategoryInfoUpsert(int CategoryId, int? CategoryInfoId, int CategoryInfoType, string Value, string LargeValue, bool Enable);

        void TreeCategoryUpsert(int TreeId, int? ParentCategoryId, int ChildCategoryId, bool Enable);

        int CatalogItemUpsert(int CatalogId, int? ItemId, string Name, bool Enable);

        #endregion

        #region Company

        string CompanyUpsert(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, int CompanyType, bool Enable);

        int CompanyInfoUpsert(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable);

        int ContactUpsert(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable);

        int ContactInfoUpsert(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable);

        int RoleCompanyUpsert(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable);

        int RoleCompanyInfoUpsert(int RoleCompanyId, int? RoleCompanyInfoId, int RoleCompanyInfoTypeId, string Value, string LargeValue, bool Enable);

        int UserCompanyUpsert(int? UserCompanyId, string User, int RoleCompanyId, bool Enable);

        CompanyModel CompanyGetBasicInfo(string CompanyPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType);

        #endregion
    }
}
