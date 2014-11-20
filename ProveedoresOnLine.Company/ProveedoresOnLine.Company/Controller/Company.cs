using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Controller
{
    public class Company
    {
        #region Util

        public TreeModel UpsertTree(TreeModel TreeToUpsert)
        {
            TreeToUpsert.TreeId = DAL.Controller.CompanyDataController.Instance.UpsertTree
                (TreeToUpsert.TreeId > 0 ? (int?)TreeToUpsert.TreeId : null,
                TreeToUpsert.TreeName,
                TreeToUpsert.Enable);

            return TreeToUpsert;
        }

        public GenericItemModel UpsertCategory(int? TreeId, GenericItemModel CategoryToUpsert)
        {
            CategoryToUpsert.ItemId = DAL.Controller.CompanyDataController.Instance.UpsertCategory
                (CategoryToUpsert.ItemId,
                CategoryToUpsert.ItemName,
                CategoryToUpsert.Enable);

            UpsertCategoryInfo(CategoryToUpsert);

            if (TreeId != null && TreeId > 0)
            {
                UpsertTreeCategory((int)TreeId, CategoryToUpsert);
            }

            return CategoryToUpsert;
        }

        public GenericItemModel UpsertCategoryInfo(GenericItemModel CategoryToUpsert)
        {
            if (CategoryToUpsert.ItemId > 0 &&
                CategoryToUpsert.ItemInfo != null &&
                CategoryToUpsert.ItemInfo.Count > 0)
            {
                CategoryToUpsert.ItemInfo.All(catinfo =>
                {
                    catinfo.ItemInfoId = DAL.Controller.CompanyDataController.Instance.UpsertCategoryInfo
                        (CategoryToUpsert.ItemId,
                        catinfo.ItemInfoId > 0 ? (int?)catinfo.ItemInfoId : null,
                        catinfo.ItemInfoType.ItemId,
                        catinfo.Value,
                        catinfo.LargeValue,
                        catinfo.Enable);

                    return true;
                });
            }

            return CategoryToUpsert;
        }

        public void UpsertTreeCategory(int TreeId, GenericItemModel CategoryToUpsert)
        {
            DAL.Controller.CompanyDataController.Instance.UpsertTreeCategory
                (TreeId,
                CategoryToUpsert.ParentItem != null && CategoryToUpsert.ParentItem.ItemId > 0 ? (int?)CategoryToUpsert.ParentItem.ItemId : null,
                CategoryToUpsert.ItemId,
                CategoryToUpsert.Enable);
        }

        public CatalogModel UpsertCatalogItem(CatalogModel CatalogItemToUpsert)
        {
            CatalogItemToUpsert.ItemId = DAL.Controller.CompanyDataController.Instance.UpsertCatalogItem
                (CatalogItemToUpsert.CatalogId,
                CatalogItemToUpsert.ItemId > 0 ? (int?)CatalogItemToUpsert.ItemId : null,
                CatalogItemToUpsert.ItemName,
                CatalogItemToUpsert.ItemEnable);

            return CatalogItemToUpsert;
        }

        #endregion

        #region Company

        public CompanyModel UpsertCompany(CompanyModel CompanyToUpsert)
        {
            CompanyToUpsert.CompanyPublicId = DAL.Controller.CompanyDataController.Instance.UpsertCompany
                (CompanyToUpsert.CompanyPublicId,
                CompanyToUpsert.CompanyName,
                CompanyToUpsert.IdentificationType.ItemId,
                CompanyToUpsert.IdentificationNumber,
                CompanyToUpsert.CompanyType,
                CompanyToUpsert.Enable);

            UpsertCompanyInfo(CompanyToUpsert);
            UpsertContact(CompanyToUpsert);

            return CompanyToUpsert;
        }

        public CompanyModel UpsertCompanyInfo(CompanyModel CompanyToUpsert)
        {
            if (!string.IsNullOrEmpty(CompanyToUpsert.CompanyPublicId) &&
                CompanyToUpsert.CompanyInfo != null &&
                CompanyToUpsert.CompanyInfo.Count > 0)
            {
                CompanyToUpsert.CompanyInfo.All(cmpinf =>
                {
                    cmpinf.ItemInfoId = DAL.Controller.CompanyDataController.Instance.UpsertCompanyInfo
                        (CompanyToUpsert.CompanyPublicId,
                        cmpinf.ItemInfoId > 0 ? (int?)cmpinf.ItemInfoId : null,
                        cmpinf.ItemInfoType.ItemId,
                        cmpinf.Value,
                        cmpinf.LargeValue,
                        cmpinf.Enable);

                    return true;
                });

            }

            return CompanyToUpsert;
        }

        public CompanyModel UpsertContact(CompanyModel CompanyToUpsert)
        {
            if (!string.IsNullOrEmpty(CompanyToUpsert.CompanyPublicId) &&
                CompanyToUpsert.RelatedContact != null &&
                CompanyToUpsert.RelatedContact.Count > 0)
            {
                CompanyToUpsert.RelatedContact.All(cmpinf =>
                {
                    cmpinf.ItemId = DAL.Controller.CompanyDataController.Instance.UpsertContact
                        (CompanyToUpsert.CompanyPublicId,
                        cmpinf.ItemId > 0 ? (int?)cmpinf.ItemId : null,
                        cmpinf.ItemType.ItemId,
                        cmpinf.ItemName,
                        cmpinf.Enable);

                    UpsertContactInfo(cmpinf);

                    return true;
                });

            }

            return CompanyToUpsert;
        }

        public GenericItemModel UpsertContactInfo(GenericItemModel ContactToUpsert)
        {
            if (ContactToUpsert.ItemId > 0 &&
                ContactToUpsert.ItemInfo != null &&
                ContactToUpsert.ItemInfo.Count > 0)
            {
                ContactToUpsert.ItemInfo.All(ctinf =>
                {
                    ctinf.ItemInfoId = DAL.Controller.CompanyDataController.Instance.UpsertContactInfo
                        (ContactToUpsert.ItemId,
                        ctinf.ItemInfoId > 0 ? (int?)ctinf.ItemInfoId : null,
                        ctinf.ItemInfoType.ItemId,
                        ctinf.Value,
                        ctinf.LargeValue,
                        ctinf.Enable);

                    return true;
                });

            }

            return ContactToUpsert;
        }

        public CompanyModel UpsertRoleCompany(CompanyModel CompanyToUpsert)
        {
            if (!string.IsNullOrEmpty(CompanyToUpsert.CompanyPublicId) &&
                CompanyToUpsert.RelatedRole != null &&
                CompanyToUpsert.RelatedRole.Count > 0)
            {
                CompanyToUpsert.RelatedRole.All(cmpinf =>
                {
                    cmpinf.ItemId = DAL.Controller.CompanyDataController.Instance.UpsertRoleCompany
                        (CompanyToUpsert.CompanyPublicId,
                        cmpinf.ItemId > 0 ? (int?)cmpinf.ItemId : null,
                        cmpinf.ItemName,
                        cmpinf.ParentItem != null && cmpinf.ParentItem.ItemId > 0 ? (int?)cmpinf.ParentItem.ItemId : null,
                        cmpinf.Enable);

                    UpsertRoleCompanyInfo(cmpinf);

                    return true;
                });

            }

            return CompanyToUpsert;
        }

        public GenericItemModel UpsertRoleCompanyInfo(GenericItemModel RoleCompanyToUpsert)
        {
            if (RoleCompanyToUpsert.ItemId > 0 &&
                RoleCompanyToUpsert.ItemInfo != null &&
                RoleCompanyToUpsert.ItemInfo.Count > 0)
            {
                RoleCompanyToUpsert.ItemInfo.All(ctinf =>
                {
                    ctinf.ItemInfoId = DAL.Controller.CompanyDataController.Instance.UpsertRoleCompanyInfo
                        (RoleCompanyToUpsert.ItemId,
                        ctinf.ItemInfoId > 0 ? (int?)ctinf.ItemInfoId : null,
                        ctinf.ItemInfoType.ItemId,
                        ctinf.Value,
                        ctinf.LargeValue,
                        ctinf.Enable);

                    return true;
                });

            }

            return RoleCompanyToUpsert;
        }

        public UserCompany UpsertUserCompany(UserCompany UserCompanyToUpsert)
        {
            UserCompanyToUpsert.UserCompanyId = DAL.Controller.CompanyDataController.Instance.UpsertUserCompany
                (UserCompanyToUpsert.UserCompanyId > 0 ? (int?)UserCompanyToUpsert.UserCompanyId : null,
                UserCompanyToUpsert.User,
                UserCompanyToUpsert.RelatedRole.ItemId,
                UserCompanyToUpsert.Enable);

            return UserCompanyToUpsert;
        }

        #endregion
    }
}
