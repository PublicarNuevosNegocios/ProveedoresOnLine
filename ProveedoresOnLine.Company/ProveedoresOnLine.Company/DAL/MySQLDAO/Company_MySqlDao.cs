using ProveedoresOnLine.Company.Interfaces;
using ProveedoresOnLine.Company.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProveedoresOnLine.Company.Models.Util;

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

        public int TreeUpsert(int? TreeId, string TreeName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vTreeId", TreeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vTreeName", TreeName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "U_Tree_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int CategoryUpsert(int? CategoryId, string CategoryName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryName", CategoryName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "U_Category_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);

        }

        public int CategoryInfoUpsert(int CategoryId, int? CategoryInfoId, int CategoryInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryId", CategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryInfoId", CategoryInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCategoryInfoType", CategoryInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "U_CategoryInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public void TreeCategoryUpsert(int TreeId, int? ParentCategoryId, int ChildCategoryId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vTreeId", TreeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentCategoryId", ParentCategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vChildCategoryId", ChildCategoryId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "U_TreeCategory_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });
        }

        public int CatalogItemUpsert(int CatalogId, int? ItemId, string Name, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCatalogId", CatalogId));
            lstParams.Add(DataInstance.CreateTypedParameter("vItemId", ItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "U_CatalogItem_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<GeographyModel> CategorySearchByGeography(string SearchParam, int? CityId, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vCityId", CityId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByGeography",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GeographyModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from g in response.DataTableResult.AsEnumerable()
                     where !g.IsNull("CityId")
                     select new GeographyModel()
                     {
                         Country = new GenericItemModel()
                         {
                             ItemId = g.Field<int>("CountryId"),
                             ItemName = g.Field<string>("CountryName"),
                         },
                         State = new GenericItemModel()
                         {
                             ItemId = g.Field<int>("StateId"),
                             ItemName = g.Field<string>("StateName"),
                         },
                         City = new GenericItemModel()
                         {
                             ItemId = g.Field<int>("CityId"),
                             ItemName = g.Field<string>("CityName"),
                         },
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByRules(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstparams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstparams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByRule",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from g in response.DataTableResult.AsEnumerable()
                     where !g.IsNull("RuleId")
                     select new GenericItemModel() 
                     {
                         ItemId = g.Field<int>("RuleId"),
                         ItemName = g.Field<string>("RuleName"),
                         Enable = g.Field<UInt64>("RuleEnable") == 1 ? true : false,
                         LastModify = g.Field<DateTime>("RuleModify"),
                         CreateDate = g.Field<DateTime>("RuleCreate"),
                     }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByCompanyRules(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstparams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstparams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByCompanyRules",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from g in response.DataTableResult.AsEnumerable()
                     where !g.IsNull("CompanyRulesId")
                     select new GenericItemModel()
                     {
                         ItemId = g.Field<int>("CompanyRulesId"),
                         ItemName = g.Field<string>("CompanyRulesName"),
                         Enable = g.Field<UInt64>("CompanyRulesEnable") == 1 ? true : false,
                         LastModify = g.Field<DateTime>("CompanyRulesModify"),
                         CreateDate = g.Field<DateTime>("CompanyRulesCreate"),
                     }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByResolution(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstparams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstparams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByResolution",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from g in response.DataTableResult.AsEnumerable()
                     where !g.IsNull("ResolutionId")
                     select new GenericItemModel()
                     {
                         ItemId = g.Field<int>("ResolutionId"),
                         ItemName = g.Field<string>("ResolutionName"),
                         Enable = g.Field<UInt64>("ResolutionEnable") == 1 ? true : false,
                         LastModify = g.Field<DateTime>("ResolutionModify"),
                         CreateDate = g.Field<DateTime>("ResolutionCreate"),
                     }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByActivity(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByActivity",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ea in response.DataTableResult.AsEnumerable()
                     where !ea.IsNull("CategoryId") &&
                            !ea.IsNull("TreeId")
                     group ea by new
                     {
                         TreeId = ea.Field<int>("TreeId"),
                         TreeName = ea.Field<string>("TreeName"),
                         TreeEnable = ea.Field<UInt64>("TreeEnable") == 1 ? true : false,

                         CategoryId = ea.Field<int>("CategoryId"),
                         CategoryName = ea.Field<string>("CategoryName"),
                         CategoryEnable = ea.Field<UInt64>("CategoryEnable") == 1 ? true : false,

                     } into eag
                     select new GenericItemModel()
                     {
                         ItemId = eag.Key.CategoryId,
                         ItemName = eag.Key.CategoryName,
                         Enable = eag.Key.CategoryEnable,
                         ParentItem = new GenericItemModel()
                         {
                             ItemId = eag.Key.TreeId,
                             ItemName = eag.Key.TreeName,
                             Enable = eag.Key.TreeEnable,
                         },
                         ItemInfo =
                            (from eai in response.DataTableResult.AsEnumerable()
                             where !eai.IsNull("CategoryInfoId") &&
                                     eai.Field<int>("CategoryId") == eag.Key.CategoryId
                             group eai by new
                             {
                                 CategoryInfoId = eai.Field<int>("CategoryInfoId"),
                                 CategoryInfoType = eai.Field<int>("CategoryInfoType"),
                                 Value = eai.Field<string>("Value"),
                                 ValueName = !eai.IsNull("CategoryTypeName") ? eai.Field<string>("CategoryTypeName") :
                                    !eai.IsNull("CategoryCategoryName") ? eai.Field<string>("CategoryCategoryName") :
                                    !eai.IsNull("CategoryGorupName") ? eai.Field<string>("CategoryGorupName") : string.Empty,
                                 LargeValue = eai.Field<string>("LargeValue"),
                             } into eaig
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = eaig.Key.CategoryInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = eaig.Key.CategoryInfoType,
                                 },
                                 Value = eaig.Key.Value,
                                 ValueName = eaig.Key.ValueName,
                                 LargeValue = eaig.Key.LargeValue,
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByCustomActivity(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByCustomActivity",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ea in response.DataTableResult.AsEnumerable()
                     where !ea.IsNull("CategoryId") &&
                            !ea.IsNull("TreeId")
                     group ea by new
                     {
                         TreeId = ea.Field<int>("TreeId"),
                         TreeName = ea.Field<string>("TreeName"),
                         TreeEnable = ea.Field<UInt64>("TreeEnable") == 1 ? true : false,

                         CategoryId = ea.Field<int>("CategoryId"),
                         CategoryName = ea.Field<string>("CategoryName"),
                         CategoryEnable = ea.Field<UInt64>("CategoryEnable") == 1 ? true : false,

                     } into eag
                     select new GenericItemModel()
                     {
                         ItemId = eag.Key.CategoryId,
                         ItemName = eag.Key.CategoryName,
                         Enable = eag.Key.CategoryEnable,
                         ParentItem = new GenericItemModel()
                         {
                             ItemId = eag.Key.TreeId,
                             ItemName = eag.Key.TreeName,
                             Enable = eag.Key.TreeEnable,
                         },
                         ItemInfo =
                            (from eai in response.DataTableResult.AsEnumerable()
                             where !eai.IsNull("CategoryInfoId") &&
                                     eai.Field<int>("CategoryId") == eag.Key.CategoryId
                             group eai by new
                             {
                                 CategoryInfoId = eai.Field<int>("CategoryInfoId"),
                                 CategoryInfoType = eai.Field<int>("CategoryInfoType"),
                                 Value = eai.Field<string>("Value"),
                                 ValueName = !eai.IsNull("CategoryTypeName") ? eai.Field<string>("CategoryTypeName") :
                                    !eai.IsNull("CategoryCategoryName") ? eai.Field<string>("CategoryCategoryName") :
                                    !eai.IsNull("CategoryGorupName") ? eai.Field<string>("CategoryGorupName") : string.Empty,
                                 LargeValue = eai.Field<string>("LargeValue"),
                             } into eaig
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = eaig.Key.CategoryInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = eaig.Key.CategoryInfoType,
                                 },
                                 Value = eaig.Key.Value,
                                 ValueName = eaig.Key.ValueName,
                                 LargeValue = eaig.Key.LargeValue,
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Company

        public string CompanyUpsert(string CompanyPublicId, string CompanyName, int IdentificationType, string IdentificationNumber, int CompanyType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyName", CompanyName));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationType", IdentificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vIdentificationNumber", IdentificationNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyType", CompanyType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_Company_UpSert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        public int CompanyInfoUpsert(string CompanyPublicId, int? CompanyInfoId, int CompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyInfoId", CompanyInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyInfoType", CompanyInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "C_CompanyInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ContactUpsert(string CompanyPublicId, int? ContactId, int ContactTypeId, string ContactName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactId", ContactId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactTypeId", ContactTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactName", ContactName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "C_Contact_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ContactInfoUpsert(int ContactId, int? ContactInfoId, int ContactInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vContactId", ContactId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactInfoId", ContactInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactInfoTypeId", ContactInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "C_ContactInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int RoleCompanyUpsert(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentRoleCompanyId", ParentRoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "C_RoleCompany_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);

        }

        public int RoleCompanyInfoUpsert(int RoleCompanyId, int? RoleCompanyInfoId, int RoleCompanyInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyInfoId", RoleCompanyInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyInfoTypeId", RoleCompanyInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "C_RoleCompanyInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int UserCompanyUpsert(int? UserCompanyId, string User, int RoleCompanyId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserCompanyId", UserCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "C_UserCompany_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }


        public CompanyModel CompanyGetBasicInfo(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Company_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            CompanyModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new CompanyModel()
                {
                    CompanyPublicId = response.DataTableResult.Rows[0].Field<string>("CompanyPublicId"),
                    CompanyName = response.DataTableResult.Rows[0].Field<string>("CompanyName"),
                    IdentificationType = new Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("IdentificationTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("IdentificationTypeName"),
                    },
                    IdentificationNumber = response.DataTableResult.Rows[0].Field<string>("IdentificationNumber"),
                    CompanyType = new Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("CompanyTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("CompanyTypeName"),
                    },
                    Enable = response.DataTableResult.Rows[0].Field<UInt64>("CompanyEnable") == 1 ? true : false,
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("CompanyLastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("CompanyCreateDate"),

                    CompanyInfo =
                        (from ci in response.DataTableResult.AsEnumerable()
                         where !ci.IsNull("CompanyInfoId")
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = ci.Field<int>("CompanyInfoId"),
                             ItemInfoType = new Models.Util.CatalogModel()
                             {
                                 ItemId = ci.Field<int>("CompanyInfoTypeId"),
                                 ItemName = ci.Field<string>("CompanyInfoTypeName"),
                             },
                             Value = ci.Field<string>("Value"),
                             LargeValue = ci.Field<string>("LargeValue"),
                             Enable = ci.Field<UInt64>("CompanyInfoEnable") == 1 ? true : false,
                             LastModify = ci.Field<DateTime>("CompanyInfoLastModify"),
                             CreateDate = ci.Field<DateTime>("CompanyInfoCreateDate"),
                         }).ToList(),
                };
            }
            return oReturn;
        }

        public List<GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactType", ContactType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Contact_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from co in response.DataTableResult.AsEnumerable()
                     where !co.IsNull("ContactId")
                     group co by new
                     {
                         ContactId = co.Field<int>("ContactId"),
                         ContactTypeId = co.Field<int>("ContactTypeId"),
                         ContactTypeName = co.Field<string>("ContactTypeName"),
                         ContactName = co.Field<string>("ContactName"),
                         ContactEnable = co.Field<UInt64>("ContactEnable") == 1 ? true : false,
                         ContactLastModify = co.Field<DateTime>("ContactLastModify"),
                         ContactCreateDate = co.Field<DateTime>("ContactCreateDate"),
                     } into cog
                     select new GenericItemModel()
                     {
                         ItemId = cog.Key.ContactId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cog.Key.ContactTypeId,
                             ItemName = cog.Key.ContactTypeName
                         },
                         ItemName = cog.Key.ContactName,
                         Enable = cog.Key.ContactEnable,
                         LastModify = cog.Key.ContactLastModify,
                         CreateDate = cog.Key.ContactCreateDate,
                         ItemInfo =
                             (from coinf in response.DataTableResult.AsEnumerable()
                              where !coinf.IsNull("ContactInfoId") &&
                                      coinf.Field<int>("ContactId") == cog.Key.ContactId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = coinf.Field<int>("ContactInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = coinf.Field<int>("ContactInfoTypeId"),
                                      ItemName = coinf.Field<string>("ContactInfoTypeName"),
                                  },
                                  Value = coinf.Field<string>("Value"),
                                  LargeValue = coinf.Field<string>("LargeValue"),
                                  Enable = coinf.Field<UInt64>("ContactInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("ContactInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("ContactInfoCreateDate"),
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        #endregion

    }
}
