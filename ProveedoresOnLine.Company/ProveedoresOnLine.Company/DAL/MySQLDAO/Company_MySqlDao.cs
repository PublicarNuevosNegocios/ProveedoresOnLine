using ProveedoresOnLine.Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.DAL.MySQLDAO
{
    internal class Company_MySqlDao : ICompanyData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Company_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.Company.Models.Constants.C_POL_CompanyConnectionName);
        }

        #region Util

        public int UpsertTree(int? TreeId, string TreeName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertCategory(int? CategoryId, string CategoryName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertCategoryInfo(int CategoryId, int? CategoryInfoId, int CategoryInfoType, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        public void UpsertTreeCategory(int TreeId, int? ParentCategoryId, int ChildCategoryId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertCatalogItem(int CatalogId, int? ItemId, string Name, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Company

        public string UpsertCompany(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, Models.enumCompanyType CompanyType, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertCompanyInfo(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertContact(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertContactInfo(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertRoleCompany(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertRoleCompanyInfo(int RoleCompanyId, int? RoleCompanyInfoId, int RoleCompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            throw new NotImplementedException();
        }

        public int UpsertUserCompany(int? UserCompanyId, string User, int RoleCompanyId, bool Enable)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
