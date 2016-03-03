using ProveedoresOnLine.Company.Interfaces;
using ProveedoresOnLine.Company.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.Company.Models.Role;

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

        public int TreeUpsert(int? TreeId, string TreeName, int TreeType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vTreeId", TreeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vTreeName", TreeName));
            lstParams.Add(DataInstance.CreateTypedParameter("vTreeType", TreeType));
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

        public List<TreeModel> TreeGetByType(int TreeType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vTreeType", TreeType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Tree_GetByType",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<TreeModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from t in response.DataTableResult.AsEnumerable()
                     where !t.IsNull("TreeId")
                     select new TreeModel()
                     {
                         TreeId = t.Field<int>("TreeId"),
                         TreeName = t.Field<string>("TreeName"),
                         TreeType = new CatalogModel()
                         {
                             ItemId = t.Field<int>("TreeTypeId"),
                             ItemName = t.Field<string>("TreeTypeName"),
                         },
                         Enable = t.Field<UInt64>("TreeEnable") == 1 ? true : false,
                         LastModify = t.Field<DateTime>("TreeLastModify"),
                         CreateDate = t.Field<DateTime>("TreeCreateDate"),
                     }).ToList();
            }

            return oReturn;

        }

        public List<TreeModel> TreeGetFullByType(int TreeType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vTreeType", TreeType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Tree_GetFullByType",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<TreeModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from t in response.DataTableResult.AsEnumerable()
                     where !t.IsNull("TreeId")
                     group t by new
                     {
                         TreeId = t.Field<int>("TreeId"),
                         TreeName = t.Field<string>("TreeName"),
                         TreeTypeId = t.Field<int>("TreeTypeId"),
                         TreeTypeName = t.Field<string>("TreeTypeName"),
                         TreeEnable = t.Field<UInt64>("TreeEnable") == 1 ? true : false,
                         TreeLastModify = t.Field<DateTime>("TreeLastModify"),
                         TreeCreateDate = t.Field<DateTime>("TreeCreateDate"),
                     } into tg
                     select new TreeModel()
                     {
                         TreeId = tg.Key.TreeId,
                         TreeName = tg.Key.TreeName,
                         TreeType = new CatalogModel()
                         {
                             ItemId = tg.Key.TreeTypeId,
                             ItemName = tg.Key.TreeTypeName,
                         },
                         Enable = tg.Key.TreeEnable,
                         LastModify = tg.Key.TreeLastModify,
                         CreateDate = tg.Key.TreeCreateDate,

                         RelatedCategory =
                            (from cat in response.DataTableResult.AsEnumerable()
                             where !cat.IsNull("CategoryId") &&
                                    cat.Field<int>("TreeId") == tg.Key.TreeId
                             group cat by new
                             {
                                 ParentCategory = cat.Field<int?>("ParentCategory"),
                                 CategoryId = cat.Field<int>("CategoryId"),
                                 CategoryName = cat.Field<string>("CategoryName"),
                                 CategoryEnable = cat.Field<UInt64>("CategoryEnable") == 1 ? true : false,
                                 CategoryLastModify = cat.Field<DateTime>("CategoryLastModify"),
                             } into catg
                             select new GenericItemModel()
                             {
                                 ItemId = catg.Key.CategoryId,
                                 ItemName = catg.Key.CategoryName,
                                 ParentItem = catg.Key.ParentCategory == null ? null :
                                    new GenericItemModel()
                                    {
                                        ItemId = catg.Key.ParentCategory.Value,
                                    },
                                 Enable = catg.Key.CategoryEnable,
                                 LastModify = catg.Key.CategoryLastModify,

                                 ItemInfo =
                                    (from catinf in response.DataTableResult.AsEnumerable()
                                     where !catinf.IsNull("CategoryInfoId") &&
                                            catinf.Field<int>("CategoryId") == catg.Key.CategoryId
                                     group catinf by new
                                     {
                                         CategoryInfoId = catinf.Field<int>("CategoryInfoId"),
                                         CategoryInfoTypeId = catinf.Field<int>("CategoryInfoTypeId"),
                                         CategoryInfoTypeName = catinf.Field<string>("CategoryInfoTypeName"),
                                         CategoryInfoTypeValue = catinf.Field<string>("CategoryInfoTypeValue"),
                                         CategoryInfoTypeLargeValue = catinf.Field<string>("CategoryInfoTypeLargeValue"),
                                         CategoryInfoTypeEnable = catinf.Field<UInt64>("CategoryInfoTypeEnable") == 1 ? true : false,
                                         CategoryInfoTypeLastModify = catinf.Field<DateTime>("CategoryInfoTypeLastModify"),
                                     } into catinfg
                                     select new GenericItemInfoModel()
                                     {
                                         ItemInfoId = catinfg.Key.CategoryInfoId,
                                         ItemInfoType = new CatalogModel()
                                         {
                                             ItemId = catinfg.Key.CategoryInfoTypeId,
                                             ItemName = catinfg.Key.CategoryInfoTypeName,
                                         },
                                         Value = catinfg.Key.CategoryInfoTypeValue,
                                         LargeValue = catinfg.Key.CategoryInfoTypeLargeValue,
                                         Enable = catinfg.Key.CategoryInfoTypeEnable,
                                         LastModify = catinfg.Key.CategoryInfoTypeLastModify,
                                     }).ToList(),
                             }).ToList(),
                     }).ToList();
            }

            return oReturn;

        }

        public int CategoryUpsert(int? CategoryId, string CategoryName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();
            CategoryId = CategoryId == 0 ? null : CategoryId;

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

        public List<GeographyModel> CategorySearchByGeography(string SearchParam, int? CityId, int PageNumber, int RowCount, out int TotalRows)
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
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
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

        public List<GeographyModel> CategorySearchByGeographyAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByGeographyAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GeographyModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn =
                     (from g in response.DataTableResult.AsEnumerable()
                      where !g.IsNull("CountryId")
                      group g by new
                      {
                          CountryId = g.Field<int>("CountryId"),
                          CountryName = g.Field<string>("CountryName"),
                          CountryEnable = g.Field<UInt64>("CountryEnable") == 1 ? true : false,

                          StateId = !g.IsNull("StateId") ? g.Field<int>("StateId") : 0,
                          StateName = !g.IsNull("StateName") ? g.Field<string>("StateName") : string.Empty,
                          StateEnable = !g.IsNull("StateEnable") ? g.Field<UInt64>("StateEnable") == 1 ? true : false : false,

                          CityId = !g.IsNull("CityId") ? g.Field<int>("CityId") : 0,
                          CityName = !g.IsNull("CityName") ? g.Field<string>("CityName") : string.Empty,
                          CityEnable = !g.IsNull("CityEnable") ? g.Field<UInt64>("CityEnable") == 1 ? true : false : false,

                      } into gg
                      select new GeographyModel()
                      {
                          Country = new GenericItemModel()
                          {
                              ItemId = gg.Key.CountryId,
                              ItemName = gg.Key.CountryName,
                              Enable = gg.Key.CountryEnable,

                              ItemInfo =
                                 (from ginf in response.DataTableResult.AsEnumerable()
                                  where ginf.Field<int>("CountryId") == gg.Key.CountryId
                                  group ginf by new
                                  {
                                      CountryInfoId = !ginf.IsNull("CountryInfoId") ? ginf.Field<int>("CountryInfoId") : 0,
                                      CountryInfoType = !ginf.IsNull("CountryInfoType") ? ginf.Field<int>("CountryInfoType") : 0,
                                      CountryValue = !ginf.IsNull("CountryValue") ? ginf.Field<string>("CountryValue") : string.Empty,
                                      CountryLargeValue = !ginf.IsNull("CountryValue") ? ginf.Field<string>("CountryLargeValue") : string.Empty,
                                  } into gginfg
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = gginfg.Key.CountryInfoId,
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = gginfg.Key.CountryInfoType,
                                      },
                                      Value = gginfg.Key.CountryValue,
                                      LargeValue = gginfg.Key.CountryLargeValue,
                                  }).ToList(),
                          },
                          State = new GenericItemModel()
                          {
                              ItemId = gg.Key.StateId,
                              ItemName = gg.Key.StateName,
                              Enable = gg.Key.StateEnable,

                              ItemInfo =
                                 (from ginf in response.DataTableResult.AsEnumerable()
                                  where !ginf.IsNull("StateId")
                                        && ginf.Field<int>("StateId") == gg.Key.StateId
                                  group ginf by new
                                  {
                                      StateInfoId = ginf.Field<int>("StateInfoId"),
                                      StateInfoType = ginf.Field<int>("StateInfoType"),
                                      StateValue = ginf.Field<string>("StateValue"),
                                      StateLargeValue = ginf.Field<string>("StateLargeValue"),
                                  } into gginfg
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = gginfg.Key.StateInfoId,
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = gginfg.Key.StateInfoType,
                                      },
                                      Value = gginfg.Key.StateValue,
                                      LargeValue = gginfg.Key.StateLargeValue,
                                  }).ToList(),
                          },
                          City = new GenericItemModel()
                          {
                              ItemId = gg.Key.CityId,
                              ItemName = gg.Key.CityName,
                              Enable = gg.Key.CityEnable,

                              ItemInfo =
                                 (from ginf in response.DataTableResult.AsEnumerable()
                                  where !ginf.IsNull("CityId")
                                        && ginf.Field<int>("CityId") == gg.Key.CityId
                                  group ginf by new
                                  {
                                      CityInfoId = ginf.Field<int>("CityInfoId"),
                                      CityInfoType = ginf.Field<int>("CityInfoType"),
                                      CityValue = ginf.Field<string>("CityValue"),
                                      CityLargeValue = ginf.Field<string>("CityLargeValue"),
                                  } into gginfg
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = gginfg.Key.CityInfoId,
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = gginfg.Key.CityInfoType,
                                      },
                                      Value = gginfg.Key.CityValue,
                                      LargeValue = gginfg.Key.CityLargeValue,
                                  }).ToList(),
                          },
                      }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByICA(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByICA",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from b in response.DataTableResult.AsEnumerable()
                     where !b.IsNull("ICAId")
                     group b by new
                     {
                         ICAId = b.Field<int>("ICAId"),
                         ICAName = b.Field<string>("ICAName"),
                         ICAEnable = b.Field<UInt64>("ICAEnable") == 1 ? true : false,
                     } into bg
                     select new GenericItemModel()
                     {
                         ItemId = bg.Key.ICAId,
                         ItemName = bg.Key.ICAName,
                         Enable = bg.Key.ICAEnable,

                         ItemInfo =
                         (from binf in response.DataTableResult.AsEnumerable()
                          where !binf.IsNull("ICAInfoId")
                                && binf.Field<int>("ICAId") == bg.Key.ICAId
                          group binf by new
                          {
                              ICAInfoId = binf.Field<int>("ICAInfoId"),
                              ICAValue = binf.Field<string>("ICAValue"),
                              ICAInfoType = binf.Field<int>("ICAInfoType"),
                          } into inf
                          select new GenericItemInfoModel()
                          {
                              ItemInfoId = inf.Key.ICAInfoId,
                              Value = inf.Key.ICAValue,
                              ItemInfoType = new CatalogModel()
                              {
                                  ItemId = inf.Key.ICAInfoType,
                              }
                          }).ToList(),
                     }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByBankAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstparams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstparams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByBankAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<GenericItemModel> oReturn = null;
            TotalRows = 0;
            if (response.DataTableResult != null &&
               response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                   (from b in response.DataTableResult.AsEnumerable()
                    where !b.IsNull("BankId")
                    group b by new
                    {
                        BankId = b.Field<int>("BankId"),
                        BankName = b.Field<string>("BankName"),
                        BankEnable = b.Field<UInt64>("BankEnable") == 1 ? true : false,
                    } into bg
                    select new GenericItemModel()
                    {
                        ItemId = bg.Key.BankId,
                        ItemName = bg.Key.BankName,
                        Enable = bg.Key.BankEnable,

                        ItemInfo =
                       (from binf in response.DataTableResult.AsEnumerable()
                        where !binf.IsNull("BankInfoId")
                            && binf.Field<int>("BankId") == bg.Key.BankId
                        group binf by new
                        {
                            BankInfoId = binf.Field<Int64>("BankInfoId"),
                            BankInfoValue = binf.Field<string>("BankInfoValue"),
                            BankInfoType = binf.Field<int>("BankInfoType"),
                        } into bginfg
                        select new GenericItemInfoModel()
                        {
                            ItemInfoId = Convert.ToInt32(bginfg.Key.BankInfoId),
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = bginfg.Key.BankInfoType,
                            },
                            Value = bginfg.Key.BankInfoValue,
                        }).ToList(),
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

        public List<GenericItemModel> CategorySearchByRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByRuleAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn = (from r in response.DataTableResult.AsEnumerable()
                           where (!r.IsNull("RuleId"))
                           group r by new
                           {
                               CompanyRuleId = r.Field<int>("RuleId"),
                               CompanyRuleName = r.Field<string>("RuleName"),
                               CompanyRuleEnable = r.Field<UInt64>("RuleEnable") == 1 ? true : false,
                           } into rr
                           select new GenericItemModel()
                           {
                               ItemId = rr.Key.CompanyRuleId,
                               ItemName = rr.Key.CompanyRuleName,
                               Enable = rr.Key.CompanyRuleEnable,
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

        public List<GenericItemModel> CategorySearchByCompanyRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByCompanyRulesAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn = (from cr in response.DataTableResult.AsEnumerable()
                           where (!cr.IsNull("CompanyRuleId"))
                           group cr by new
                           {
                               CompanyRuleId = cr.Field<int>("CompanyRuleId"),
                               CompanyRuleName = cr.Field<string>("CompanyRuleName"),
                               CompanyRuleEnable = cr.Field<UInt64>("CompanyRuleEnable") == 1 ? true : false,
                           } into crr
                           select new GenericItemModel()
                           {
                               ItemId = crr.Key.CompanyRuleId,
                               ItemName = crr.Key.CompanyRuleName,
                               Enable = crr.Key.CompanyRuleEnable,
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

        public List<GenericItemModel> CategorySearchByResolutionAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByResolutionAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn = (from cr in response.DataTableResult.AsEnumerable()
                           where (!cr.IsNull("ResolutionId"))
                           group cr by new
                           {
                               CompanyRuleId = cr.Field<int>("ResolutionId"),
                               CompanyRuleName = cr.Field<string>("ResolutionName"),
                               CompanyRuleEnable = cr.Field<UInt64>("ResolutionEnable") == 1 ? true : false,
                           } into crr
                           select new GenericItemModel()
                           {
                               ItemId = crr.Key.CompanyRuleId,
                               ItemName = crr.Key.CompanyRuleName,
                               Enable = crr.Key.CompanyRuleEnable,
                           }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByARLCompany(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstparams = new List<IDbDataParameter>();

            lstparams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstparams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstparams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByARL",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstparams,
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from g in response.DataTableResult.AsEnumerable()
                     where !g.IsNull("CompanyARLId")
                     select new GenericItemModel()
                     {
                         ItemId = g.Field<int>("CompanyARLId"),
                         ItemName = g.Field<string>("CompanyARLName"),
                         Enable = g.Field<UInt64>("CompanyARLEnable") == 1 ? true : false,
                         LastModify = g.Field<DateTime>("CompanyARLModify"),
                         CreateDate = g.Field<DateTime>("CompanyARLCreate"),
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

        public List<GenericItemModel> CategorySearchByBank(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByBank",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ba in response.DataTableResult.AsEnumerable()
                     where !ba.IsNull("BankId")
                     group ba by new
                     {
                         ItemId = ba.Field<int>("BankId"),
                         ItemName = ba.Field<string>("BankName"),
                     }
                         into bank
                     select new GenericItemModel()
                     {
                         ItemId = bank.Key.ItemId,
                         ItemName = bank.Key.ItemName,
                     }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategoryGetFinantialAccounts()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_GetFinantialAccounts",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from fa in response.DataTableResult.AsEnumerable()
                     where !fa.IsNull("AccountId")
                     group fa by new
                     {
                         AccountId = fa.Field<int>("AccountId"),
                         AccountName = fa.Field<string>("AccountName"),
                         AccountEnable = fa.Field<UInt64>("AccountEnable") == 1 ? true : false,

                         ParentAccountId = fa.Field<int?>("ParentAccountId"),

                     } into fag
                     select new GenericItemModel()
                     {
                         ItemId = fag.Key.AccountId,
                         ItemName = fag.Key.AccountName,
                         Enable = fag.Key.AccountEnable,
                         ParentItem = fag.Key.ParentAccountId == null ? null : new GenericItemModel()
                         {
                             ItemId = (int)fag.Key.ParentAccountId,
                         },
                         ItemInfo =
                            (from fai in response.DataTableResult.AsEnumerable()
                             where !fai.IsNull("AccountInfoId") &&
                                     fai.Field<int>("AccountId") == fag.Key.AccountId
                             group fai by new
                             {
                                 AccountInfoId = fai.Field<int>("AccountInfoId"),
                                 AccountInfoTypeId = fai.Field<int>("AccountInfoTypeId"),
                                 AccountInfoTypeName = fai.Field<string>("AccountInfoTypeName"),
                                 Value = fai.Field<string>("Value"),
                                 LargeValue = fai.Field<string>("LargeValue"),
                             } into faig
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = faig.Key.AccountInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = faig.Key.AccountInfoTypeId,
                                     ItemName = faig.Key.AccountInfoTypeName,
                                 },
                                 Value = faig.Key.Value,
                                 LargeValue = faig.Key.LargeValue,
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<CurrencyExchangeModel> CurrencyExchangeGetByMoneyType(int? MoneyTypeFrom, int? MoneyTypeTo, int? Year)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vMoneyTypeFrom", MoneyTypeFrom));
            lstParams.Add(DataInstance.CreateTypedParameter("vMoneyTypeTo", MoneyTypeTo));
            lstParams.Add(DataInstance.CreateTypedParameter("vYear", Year));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_CurrencyExchange_GetByMoneyType",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CurrencyExchangeModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ce in response.DataTableResult.AsEnumerable()
                     where !ce.IsNull("CurrencyExchangeId")
                     select new CurrencyExchangeModel()
                     {
                         CurrencyExchangeId = ce.Field<int>("CurrencyExchangeId"),
                         IssueDate = ce.Field<DateTime>("IssueDate"),
                         MoneyTypeFrom = new CatalogModel()
                         {
                             ItemId = ce.Field<int>("MoneyTypeFromId"),
                             ItemName = ce.Field<string>("MoneyTypeFromName"),
                         },
                         MoneyTypeTo = new CatalogModel()
                         {
                             ItemId = ce.Field<int>("MoneyTypeToId"),
                             ItemName = ce.Field<string>("MoneyTypeToName"),
                         },
                         Rate = ce.Field<decimal>("Rate"),
                         LastModify = ce.Field<DateTime>("LastModify"),
                         CreateDate = ce.Field<DateTime>("CreateDate"),
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> CategorySearchByEcoActivityAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));
            lstParams.Add(DataInstance.CreateTypedParameter("vTreeId", TreeId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByEcoActivityAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn =
                     (from cat in response.DataTableResult.AsEnumerable()
                      where !cat.IsNull("CategoryId")
                      group cat by new
                      {
                          CategoryId = cat.Field<int>("CategoryId"),
                          CategoryName = cat.Field<string>("CategoryName"),
                          CategoryEnable = cat.Field<UInt64>("CategoryEnable") == 1 ? true : false,
                      } into catg
                      select new GenericItemModel()
                      {
                          ItemId = catg.Key.CategoryId,
                          ItemName = catg.Key.CategoryName,
                          Enable = catg.Key.CategoryEnable,

                          ItemInfo =
                              (from catinf in response.DataTableResult.AsEnumerable()
                               where !catinf.IsNull("CategoryInfoId") &&
                                    catinf.Field<int>("CategoryId") == catg.Key.CategoryId
                               group catinf by new
                               {
                                   CategoryInfoId = catinf.Field<int>("CategoryInfoId"),
                                   CategoryInfoTypeId = catinf.Field<int>("CategoryInfoTypeId"),
                                   CategoryInfoTypeName = catinf.Field<string>("CategoryInfoTypeName"),
                                   CategoryInfoValue = catinf.Field<string>("CategoryInfoValue"),
                                   CategoryInfoLargeValue = catinf.Field<string>("CategoryInfoLargeValue"),
                               } into catinfg
                               select new GenericItemInfoModel()
                               {
                                   ItemInfoId = catinfg.Key.CategoryInfoId,
                                   ItemInfoType = new CatalogModel()
                                   {
                                       ItemId = catinfg.Key.CategoryInfoTypeId,
                                       ItemName = catinfg.Key.CategoryInfoTypeName,
                                   },
                                   Value = catinfg.Key.CategoryInfoValue,
                                   LargeValue = catinfg.Key.CategoryInfoLargeValue,
                                   ValueName = string.Join(";",
                                    (from cminfvn in response.DataTableResult.AsEnumerable()
                                     where !cminfvn.IsNull("CategoryInfoId") &&
                                            cminfvn.Field<int>("CategoryInfoId") == catinfg.Key.CategoryInfoId &&
                                            !cminfvn.IsNull("CategoryInfoValueName")
                                     select cminfvn.Field<string>("CategoryInfoValueName")).Distinct().ToList()),
                               }).ToList(),
                      }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByEcoGroupAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));
            lstParams.Add(DataInstance.CreateTypedParameter("vTreeId", TreeId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByEcoGroupAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn = (from cr in response.DataTableResult.AsEnumerable()
                           where (!cr.IsNull("GroupId"))
                           group cr by new
                           {
                               CompanyRuleId = cr.Field<int>("GroupId"),
                               CompanyRuleName = cr.Field<string>("GroupName"),
                               CompanyRuleEnable = cr.Field<UInt64>("GroupEnable") == 1 ? true : false,
                           } into crr
                           select new GenericItemModel()
                           {
                               ItemId = crr.Key.CompanyRuleId,
                               ItemName = crr.Key.CompanyRuleName,
                               Enable = crr.Key.CompanyRuleEnable,
                           }).ToList();
            }

            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByTreeAdmin(string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchByTreeAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from tr in response.DataTableResult.AsEnumerable()
                           where (!tr.IsNull("TreeId"))
                           group tr by new
                           {
                               TreeAdminId = tr.Field<int>("TreeId"),
                               TreeAdminName = tr.Field<string>("TreeName"),
                               TreeAdminEnable = tr.Field<UInt64>("TreeEnable") == 1 ? true : false,
                           } into trr
                           select new GenericItemModel()
                           {
                               ItemId = trr.Key.TreeAdminId,
                               ItemName = trr.Key.TreeAdminName,
                               Enable = trr.Key.TreeAdminEnable,
                           }).ToList();
            }

            return oReturn;
        }

        public List<CurrencyExchangeModel> CurrentExchangeGetAllAdmin()
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_CurrentExchange_GetAllAdmin",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CurrencyExchangeModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ce in response.DataTableResult.AsEnumerable()
                     where !ce.IsNull("CurrencyExchangeId")
                     select new CurrencyExchangeModel()
                     {
                         CurrencyExchangeId = ce.Field<int>("CurrencyExchangeId"),
                         IssueDate = ce.Field<DateTime>("IssueDate"),
                         MoneyTypeFrom = new CatalogModel()
                         {
                             ItemId = ce.Field<int>("MoneyTypeFromId"),
                             ItemName = ce.Field<string>("MoneyTypeFromName"),
                         },
                         MoneyTypeTo = new CatalogModel()
                         {
                             ItemId = ce.Field<int>("MoneyTypeToId"),
                             ItemName = ce.Field<string>("MoneyTypeToName"),
                         },
                         Rate = ce.Field<decimal>("Rate"),
                         LastModify = ce.Field<DateTime>("LastModify"),
                         CreateDate = ce.Field<DateTime>("CreateDate"),
                     }).ToList();
            }
            return oReturn;
        }

        public int CurrencyExchangeInsert(DateTime IssueDate, int MoneyTypeFrom, int MoneyTypeTo, decimal Rate)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vIssueDate", IssueDate));
            lstParams.Add(DataInstance.CreateTypedParameter("vMoneyTypeFrom", MoneyTypeFrom));
            lstParams.Add(DataInstance.CreateTypedParameter("vMoneyTypeTo", MoneyTypeTo));
            lstParams.Add(DataInstance.CreateTypedParameter("vRate", Rate));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "U_CurrencyExchange_Insert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<GeographyModel> CategorySearchByCountryAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchCountryAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GeographyModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn =
                     (from g in response.DataTableResult.AsEnumerable()
                      where !g.IsNull("CountryId")
                      group g by new
                      {
                          CountryId = g.Field<int>("CountryId"),
                          CountryName = g.Field<string>("CountryName"),
                          CountryEnable = g.Field<UInt64>("CountryEnable") == 1 ? true : false,
                      } into gg
                      select new GeographyModel()
                      {
                          Country = new GenericItemModel()
                          {
                              ItemId = gg.Key.CountryId,
                              ItemName = gg.Key.CountryName,
                              Enable = gg.Key.CountryEnable,
                          }
                      }).ToList();
            }

            return oReturn;
        }

        public List<GeographyModel> CategorySearchByStateAdmin(string CountrySearchParam, string StateSearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCountrySearchParam", CountrySearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vStateSearchParam", StateSearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchStateAdmin",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GeographyModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");
                oReturn =
                     (from g in response.DataTableResult.AsEnumerable()
                      where !g.IsNull("CountryId")
                      group g by new
                      {
                          CountryId = g.Field<int>("CountryId"),
                          CountryName = g.Field<string>("CountryName"),
                          CountryEnable = g.Field<UInt64>("CountryEnable") == 1 ? true : false,

                          StateId = !g.IsNull("StateId") ? g.Field<int>("StateId") : 0,
                          StateName = !g.IsNull("StateName") ? g.Field<string>("StateName") : string.Empty,
                          StateEnable = !g.IsNull("StateEnable") ? g.Field<UInt64>("StateEnable") == 1 ? true : false : false,
                      } into gg
                      select new GeographyModel()
                      {
                          Country = new GenericItemModel()
                          {
                              ItemId = gg.Key.CountryId,
                              ItemName = gg.Key.CountryName,
                              Enable = gg.Key.CountryEnable,
                          },
                          State = new GenericItemModel()
                          {
                              ItemId = gg.Key.StateId,
                              ItemName = gg.Key.StateName,
                              Enable = gg.Key.StateEnable,
                          }
                      }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemModel> CategorySearchBySurveyGroup(int TreeId, string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vTreeId", TreeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Category_SearchBySurveyGroup",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from cat in response.DataTableResult.AsEnumerable()
                     where !cat.IsNull("CategoryId")
                     group cat by new
                     {
                         CategoryId = cat.Field<int>("CategoryId"),
                         CategoryName = cat.Field<string>("CategoryName"),
                         CategoryEnable = cat.Field<UInt64>("CategoryEnable") == 1 ? true : false,
                         CategoryLastModify = cat.Field<DateTime>("CategoryLastModify"),
                         CategoryCreateDate = cat.Field<DateTime>("CategoryCreateDate"),

                         ParentCategory = cat.Field<int?>("ParentCategory"),
                     } into catg
                     select new GenericItemModel()
                     {
                         ItemId = catg.Key.CategoryId,
                         ItemName = catg.Key.CategoryName,
                         Enable = catg.Key.CategoryEnable,
                         LastModify = catg.Key.CategoryLastModify,
                         CreateDate = catg.Key.CategoryCreateDate,

                         ParentItem = catg.Key.ParentCategory == null ? null :
                            new GenericItemModel()
                            {
                                ItemId = catg.Key.ParentCategory.Value,
                            },

                         ItemInfo =
                            (from catinfo in response.DataTableResult.AsEnumerable()
                             where !catinfo.IsNull("CategoryInfoId") &&
                                    catinfo.Field<int>("CategoryId") == catg.Key.CategoryId
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = catinfo.Field<int>("CategoryInfoId"),
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = catinfo.Field<int>("CategoryInfoTypeId"),
                                     ItemName = catinfo.Field<string>("CategoryInfoTypeName"),
                                 },
                                 Value = catinfo.Field<string>("CategoryInfoValue"),
                                 LargeValue = catinfo.Field<string>("CategoryInfoLargeValue"),
                                 Enable = catinfo.Field<UInt64>("CategoryInfoEnable") == 1 ? true : false,
                                 LastModify = catinfo.Field<DateTime>("CategoryInfoLastModify"),
                                 CreateDate = catinfo.Field<DateTime>("CategoryInfoCreateDate"),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public MinimumWageModel MinimumWageSearchByYear(int Year, int CountryType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCountryType", CountryType));
            lstParams.Add(DataInstance.CreateTypedParameter("vYear", Year));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_MinimumWage_SearchByYear",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            MinimumWageModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new MinimumWageModel()
                {
                    MinimumWageId = response.DataTableResult.Rows[0].Field<int>("MinimumWageId"),
                    Country = new CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("CountryId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("CountryName"),
                    },
                    Year = response.DataTableResult.Rows[0].Field<int>("Year"),

                    MoneyType = new CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("MoneyType"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("MoneyTypeName"),
                        ItemEnable = response.DataTableResult.Rows[0].Field<UInt64>("MoneyTypeEnable") == 1 ? true : false,
                    },
                    Value = response.DataTableResult.Rows[0].Field<decimal>("Value"),
                    Enable = response.DataTableResult.Rows[0].Field<UInt64>("Enable") == 1 ? true : false,
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("LastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("CreateDate")
                };

            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CatalogGetAllModuleOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetAllModuleOptions",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.CatalogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ItemId")
                     select new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                     {
                         CatalogId = c.Field<int>("CatalogId"),
                         CatalogName = c.Field<string>("CatalogName"),
                         ItemId = c.Field<int>("ItemId"),
                         ItemName = c.Field<string>("ItemName"),
                     }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CatalogGetAllRoleByCompany()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "",
                CommandType = CommandType.StoredProcedure,
            });

            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<CatalogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                
            }

            return oReturn;
        }

        #endregion

        #region Util MP

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCategorySearchByActivity(int TreeId, string SearchParam, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vTreeId", TreeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_U_Category_SearchByActivity",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cat in response.DataTableResult.AsEnumerable()
                     where !cat.IsNull("CategoryId")
                     select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                     {
                         ItemId = cat.Field<int>("CategoryId"),
                         ItemName = cat.Field<string>("CategoryName"),
                     }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region Company CRUD

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

        public int RoleCompanyUpsert(string CompanyPublicId, int? RoleCompanyId, string RoleCompanyName, int? ParentRoleCompanyId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyName", RoleCompanyName));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentRoleCompanyId", ParentRoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

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

        public void CompanyFilterFill(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "U_Tmp_CompanyFilter_Fill",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void CompanySearchFill(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "U_Tmp_CompanySearch_Fill",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        #endregion

        #region Company Search

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

        public List<GenericFilterModel> CompanySearchFilter(string CompanyType, string SearchParam, string SearchFilter)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyType", CompanyType));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchFilter", SearchFilter));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Company_SearchFilter",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericFilterModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sf in response.DataTableResult.AsEnumerable()
                     where !sf.IsNull("FilterTypeId")
                     select new GenericFilterModel()
                     {
                         FilterType = new GenericItemModel()
                         {
                             ItemId = (int)sf.Field<Int64>("FilterTypeId"),
                             ItemName = sf.Field<string>("FilterTypeName"),
                         },
                         FilterValue = new GenericItemModel()
                         {
                             ItemId = Convert.ToInt32(sf.Field<string>("FilterValueId")),
                             ItemName = sf.Field<string>("FilterValueName"),
                         },
                         Quantity = Convert.ToInt32(sf.Field<Int64>("Quantity")),
                     }).ToList();
            }
            return oReturn;
        }

        public List<CompanyModel> CompanySearch(string CompanyType, string SearchParam, string SearchFilter, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyType", CompanyType));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchFilter", SearchFilter));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_Company_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CompanyModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from sr in response.DataTableResult.AsEnumerable()
                     where !sr.IsNull("CompanyPublicId")
                     group sr by new
                     {
                         CompanyPublicId = sr.Field<string>("CompanyPublicId"),
                         CompanyName = sr.Field<string>("CompanyName"),
                         IdentificationTypeId = sr.Field<int>("IdentificationTypeId"),
                         IdentificationTypeName = sr.Field<string>("IdentificationTypeName"),
                         IdentificationNumber = sr.Field<string>("IdentificationNumber"),
                         CompanyTypeId = sr.Field<int>("CompanyTypeId"),
                         CompanyTypeName = sr.Field<string>("CompanyTypeName"),
                         CompanyEnable = sr.Field<UInt64>("CompanyEnable") == 1 ? true : false,
                         CompanyLastModify = sr.Field<DateTime>("CompanyLastModify"),
                         CompanyCreateDate = sr.Field<DateTime>("CompanyCreateDate"),
                     } into srg
                     select new CompanyModel()
                     {
                         CompanyPublicId = srg.Key.CompanyPublicId,
                         CompanyName = srg.Key.CompanyName,
                         IdentificationType = new Models.Util.CatalogModel()
                         {
                             ItemId = srg.Key.IdentificationTypeId,
                             ItemName = srg.Key.IdentificationTypeName,
                         },
                         IdentificationNumber = srg.Key.IdentificationNumber,
                         CompanyType = new Models.Util.CatalogModel()
                         {
                             ItemId = srg.Key.CompanyTypeId,
                             ItemName = srg.Key.CompanyTypeName,
                         },
                         Enable = srg.Key.CompanyEnable,
                         LastModify = srg.Key.CompanyLastModify,
                         CreateDate = srg.Key.CompanyCreateDate,

                         CompanyInfo =
                             (from ci in response.DataTableResult.AsEnumerable()
                              where !ci.IsNull("CompanyInfoId") &&
                                    ci.Field<string>("CompanyPublicId") == srg.Key.CompanyPublicId
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
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Contact

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

        public List<GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactType", ContactType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

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

        #region User Roles

        public CompanyModel RoleCompany_GetByPublicId(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_RoleCompany_GetRoleByPublicId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProveedoresOnLine.Company.Models.Company.CompanyModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = response.DataTableResult.Rows[0].Field<string>("CompanyPublicId"),
                    RelatedRole =
                        (from cr in response.DataTableResult.AsEnumerable()
                         where !cr.IsNull("RoleCompanyId")
                         group cr by new
                         {
                             RoleCompanyId = cr.Field<int>("RoleCompanyId"),
                             RoleCompanyName = cr.Field<string>("RoleCompanyName"),
                             RoleCompanyEnable = cr.Field<UInt64>("RoleCompanyEnable") == 1 ? true : false,
                         } into cri
                         select new GenericItemModel()
                         {
                             ItemId = cri.Key.RoleCompanyId,
                             ItemName = cri.Key.RoleCompanyName,
                             Enable = cri.Key.RoleCompanyEnable,
                         }).ToList()
                };
            }

            return oReturn;
        }

        public List<UserCompany> RoleCompany_GetUsersByPublicId(string CompanyPublicId, bool ViewEnable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vViewEnable", ViewEnable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_RoleCompany_GetUsersByPublicId",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<ProveedoresOnLine.Company.Models.Company.UserCompany> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from u in response.DataTableResult.AsEnumerable()
                     where !u.IsNull("UserCompanyId")
                     group u by new
                     {
                         UserCompanyId = u.Field<int>("UserCompanyId"),
                         User = u.Field<string>("User"),
                         UserCompanyEnable = u.Field<UInt64>("UserCompanyEnable") == 1 ? true : false,
                         RoleCompanyId = u.Field<int>("RoleCompanyId"),
                         RoleCompanyName = u.Field<string>("RoleCompanyName"),
                         RoleCompanyEnable = u.Field<UInt64>("RoleCompanyEnable") == 1 ? true : false,
                     }
                         into ui
                     select new ProveedoresOnLine.Company.Models.Company.UserCompany()
                     {
                         UserCompanyId = ui.Key.UserCompanyId,
                         User = ui.Key.User,
                         Enable = ui.Key.UserCompanyEnable,
                         RelatedRole = new GenericItemModel()
                         {
                             ItemId = ui.Key.RoleCompanyId,
                             ItemName = ui.Key.RoleCompanyName,
                             Enable = ui.Key.RoleCompanyEnable,
                         }
                     }).ToList();
            }

            return oReturn;
        }

        public List<CompanyModel> MP_RoleCompanyGetByUser(string User)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_C_RoleCompany_GetByUser",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CompanyModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("CompanyPublicId")
                     group c by new
                     {
                         CompanyPublicId = c.Field<string>("CompanyPublicId"),
                         CompanyName = c.Field<string>("CompanyName"),
                         IdentificationTypeId = c.Field<int>("IdentificationTypeId"),
                         IdentificationTypeName = c.Field<string>("IdentificationTypeName"),
                         IdentificationNumber = c.Field<string>("IdentificationNumber"),
                         CompanyTypeId = c.Field<int>("CompanyTypeId"),
                         CompanyTypeName = c.Field<string>("CompanyTypeName"),
                     } into cg
                     select new CompanyModel()
                     {
                         CompanyPublicId = cg.Key.CompanyPublicId,
                         CompanyName = cg.Key.CompanyName,
                         IdentificationType = new Models.Util.CatalogModel()
                         {
                             ItemId = cg.Key.IdentificationTypeId,
                             ItemName = cg.Key.IdentificationTypeName,
                         },
                         IdentificationNumber = cg.Key.IdentificationNumber,
                         CompanyType = new Models.Util.CatalogModel()
                         {
                             ItemId = cg.Key.CompanyTypeId,
                             ItemName = cg.Key.CompanyTypeName,
                         },

                         CompanyInfo =
                             (from ci in response.DataTableResult.AsEnumerable()
                              where !ci.IsNull("CompanyInfoId") &&
                                    ci.Field<string>("CompanyPublicId") == cg.Key.CompanyPublicId
                              group ci by new
                              {
                                  CompanyInfoId = ci.Field<int>("CompanyInfoId"),
                                  CompanyInfoTypeId = ci.Field<int>("CompanyInfoTypeId"),
                                  CompanyInfoTypeName = ci.Field<string>("CompanyInfoTypeName"),
                                  CompanyInfoValue = ci.Field<string>("CompanyInfoValue"),
                                  CompanyInfoLargeValue = ci.Field<string>("CompanyInfoLargeValue"),
                                  CompanyInfoValueName = ci.Field<string>("CompanyInfoValueName"),
                              } into cig
                              select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                              {
                                  ItemInfoId = cig.Key.CompanyInfoId,
                                  ItemInfoType = new Models.Util.CatalogModel()
                                  {
                                      ItemId = cig.Key.CompanyInfoTypeId,
                                      ItemName = cig.Key.CompanyInfoTypeName,
                                  },
                                  Value = cig.Key.CompanyInfoValue,
                                  LargeValue = cig.Key.CompanyInfoLargeValue,
                                  ValueName = cig.Key.CompanyInfoValueName,
                              }).ToList(),

                         RelatedUser =
                            (from uc in response.DataTableResult.AsEnumerable()
                             where !uc.IsNull("UserCompanyId") &&
                                   uc.Field<string>("CompanyPublicId") == cg.Key.CompanyPublicId
                             group uc by new
                             {
                                 UserCompanyId = uc.Field<int>("UserCompanyId"),
                                 User = uc.Field<string>("User"),
                             } into ucg
                             select new ProveedoresOnLine.Company.Models.Company.UserCompany()
                             {
                                 UserCompanyId = ucg.Key.UserCompanyId,
                                 User = ucg.Key.User,

                                 RelatedRole =
                                    (from ucr in response.DataTableResult.AsEnumerable()
                                     where !ucr.IsNull("RoleCompanyId") &&
                                           ucr.Field<string>("CompanyPublicId") == cg.Key.CompanyPublicId
                                     group ucr by new
                                     {
                                         RoleCompanyId = ucr.Field<int>("RoleCompanyId"),
                                         RoleCompanyName = ucr.Field<string>("RoleCompanyName"),
                                         ParentRoleCompany = ucr.Field<int?>("ParentRoleCompany"),
                                     } into ucrg
                                     select new GenericItemModel()
                                     {
                                         ItemId = ucrg.Key.RoleCompanyId,
                                         ItemName = ucrg.Key.RoleCompanyName,
                                         ParentItem = ucrg.Key.ParentRoleCompany == null ? null : new GenericItemModel()
                                         {
                                             ItemId = ucrg.Key.ParentRoleCompany.Value,
                                         },
                                         ItemInfo =
                                            (from ucrinf in response.DataTableResult.AsEnumerable()
                                             where !ucrinf.IsNull("RoleCompanyInfoId") &&
                                                   ucrinf.Field<int>("RoleCompanyId") == ucrg.Key.RoleCompanyId
                                             group ucrinf by new
                                             {
                                                 RoleCompanyInfoId = ucrinf.Field<int>("RoleCompanyInfoId"),
                                                 RoleCompanyInfoTypeId = ucrinf.Field<int>("RoleCompanyInfoTypeId"),
                                                 RoleCompanyInfoTypeName = ucrinf.Field<string>("RoleCompanyInfoTypeName"),
                                                 RoleCompanyInfoValue = ucrinf.Field<string>("RoleCompanyInfoValue"),
                                                 RoleCompanyInfoLargeValue = ucrinf.Field<string>("RoleCompanyInfoLargeValue"),
                                             } into ucrinfg
                                             select new GenericItemInfoModel()
                                             {
                                                 ItemInfoId = ucrinfg.Key.RoleCompanyInfoId,
                                                 ItemInfoType = new CatalogModel()
                                                 {
                                                     ItemId = ucrinfg.Key.RoleCompanyInfoTypeId,
                                                     ItemName = ucrinfg.Key.RoleCompanyInfoTypeName,
                                                 },
                                                 Value = ucrinfg.Key.RoleCompanyInfoValue,
                                                 LargeValue = ucrinfg.Key.RoleCompanyInfoLargeValue,
                                             }).ToList()
                                     }).FirstOrDefault(),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<UserCompany> MP_UserCompanySearch(string CompanyPublicId, string SearchParam, int? RoleCompanyId, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_C_UserCompany_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<UserCompany> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from uc in response.DataTableResult.AsEnumerable()
                     where !uc.IsNull("User")
                     select new UserCompany()
                     {
                         User = uc.Field<string>("User"),
                     }).ToList();
            }
            return oReturn;
        }

        public int RoleModuleUpsert(int RoleCompanyId, int? RoleModuleId, int RoleModuleType, string RoleModule, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleModuleId", RoleModuleId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleModuleType", RoleModuleType));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleModule", RoleModule));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_RoleModule_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ModuleOptionUpsert(int RoleModuleId, int? ModuleOptionId, int ModuleOptionType, string ModuleOption, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vRoleModuleId", RoleModuleId));
            lstParams.Add(DataInstance.CreateTypedParameter("vModuleOptionId", ModuleOptionId));
            lstParams.Add(DataInstance.CreateTypedParameter("vModuleOptionType", ModuleOptionType));
            lstParams.Add(DataInstance.CreateTypedParameter("vModuleOption", ModuleOption));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_ModuleOption_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int ModuleOptionInfoUpsert(int ModuleOptionId, int? ModuleOptionInfoId, int ModuleOptionInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vModuleOptionId", ModuleOptionId));
            lstParams.Add(DataInstance.CreateTypedParameter("vModuleOptionInfoId", ModuleOptionInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vModuleOptionInfoType", ModuleOptionInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "C_ModuleOptionInfo_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<RoleCompanyModel> GetRoleCompanySearch(string vSearchParam, bool Enable, out int TotalRows)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", vSearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_RoleCompany_Search",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<RoleCompanyModel> oReturn = null;
            TotalRows = 0;

            if (response.DataTableResult != null
                && response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("oTotalRows");

                oReturn =
                    (from rc in response.DataTableResult.AsEnumerable()
                     where !rc.IsNull("RoleCompanyId")
                     group rc by new
                     {
                         RoleCompanyId = rc.Field<int>("RoleCompanyId"),
                         RoleCompanyName = rc.Field<string>("RoleCompanyName"),
                         ParentRoleCompany = rc.Field<Int64>("ParentRoleCompany"),
                         Enable = rc.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = rc.Field<DateTime>("LastModify"),
                         CreateDate = rc.Field<DateTime>("CreateDate"),

                         CompanyPublicId = rc.Field<string>("CompanyPublicId"),
                         CompanyName = rc.Field<string>("CompanyName"),
                     } into rci
                     select new RoleCompanyModel()
                     {
                         RelatedCompany = new CompanyModel()
                         {
                             CompanyPublicId = rci.Key.CompanyPublicId,
                             CompanyName = rci.Key.CompanyName,
                         },
                         RoleCompanyId = rci.Key.RoleCompanyId,
                         RoleCompanyName = rci.Key.RoleCompanyName,
                         ParentRoleCompany = Convert.ToInt32(rci.Key.ParentRoleCompany),
                         Enable = rci.Key.Enable,
                         LastModify = rci.Key.LastModify,
                         CreateDate = rci.Key.CreateDate,
                     }).ToList();
            }

            return oReturn;
        }

        public RoleCompanyModel GetRoleCompanyByRoleCompanyId(int RoleCompanyId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_RoleCompany_GetByRoleCompanyId",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            RoleCompanyModel oReturn = null;

            if (response.DataTableResult != null
                && response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from rc in response.DataTableResult.AsEnumerable()
                     where !rc.IsNull("RoleCompanyId")
                     group rc by new
                     {
                         RoleCompanyId = rc.Field<int>("RoleCompanyId"),
                         RoleCompanyName = rc.Field<string>("RoleCompanyName"),
                         ParentRoleCompany = rc.Field<Int64>("ParentRoleCompany"),
                         Enable = rc.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = rc.Field<DateTime>("LastModify"),
                         CreateDate = rc.Field<DateTime>("CreateDate"),

                         CompanyPublicId = rc.Field<string>("CompanyPublicId"),
                         CompanyName = rc.Field<string>("CompanyName"),
                     } into rci
                     select new RoleCompanyModel()
                     {
                         RelatedCompany = new CompanyModel()
                         {
                             CompanyPublicId = rci.Key.CompanyPublicId,
                             CompanyName = rci.Key.CompanyName,
                         },
                         RoleCompanyId = rci.Key.RoleCompanyId,
                         RoleCompanyName = rci.Key.RoleCompanyName,
                         ParentRoleCompany = Convert.ToInt32(rci.Key.ParentRoleCompany),
                         Enable = rci.Key.Enable,
                         LastModify = rci.Key.LastModify,
                         CreateDate = rci.Key.CreateDate,
                     }).FirstOrDefault();
            }

            return oReturn;
        }

        public RoleCompanyModel GetRoleModuleSearch(int RoleCompanyId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vRoleCompanyId", RoleCompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_RoleModule_GetByRoleCompanyId",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            RoleCompanyModel oReturn = null;

            if (response.DataTableResult != null
                && response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from rc in response.DataTableResult.AsEnumerable()
                     where !rc.IsNull("RoleModuleId")
                     group rc by new
                     {
                         RoleCompanyId = rc.Field<int>("RoleCompanyId"),
                         RoleCompanyName = rc.Field<string>("RoleCompanyName"),
                     } into rcg
                     select new RoleCompanyModel()
                     {
                         RoleCompanyId = rcg.Key.RoleCompanyId,
                         RoleCompanyName = rcg.Key.RoleCompanyName,
                         RoleModule =
                            (from rm in response.DataTableResult.AsEnumerable()
                             where !rm.IsNull("RoleModuleId") &&
                                   rm.Field<int>("RoleCompanyId") == rcg.Key.RoleCompanyId
                             group rm by new
                             {
                                 RoleModuleId = rm.Field<int>("RoleModuleId"),
                                 RoleModule = rm.Field<string>("RoleModule"),
                                 RoleModuleTypeId = rm.Field<int>("RoleModuleTypeId"),
                                 RoleModuleTypeName = rm.Field<string>("RoleModuleTypeName"),
                                 Enable = rm.Field<UInt64>("Enable") == 1 ? true : false,
                                 LastModify = rm.Field<DateTime>("LastModify"),
                                 CreateDate = rm.Field<DateTime>("CreateDate"),
                             } into rmg
                             select new RoleModuleModel()
                             {
                                 RoleModuleId = rmg.Key.RoleModuleId,
                                 RoleModule = rmg.Key.RoleModule,
                                 RoleModuleType = new CatalogModel()
                                 {
                                     ItemId = rmg.Key.RoleModuleTypeId,
                                     ItemName = rmg.Key.RoleModuleTypeName,
                                 },
                                 Enable = rmg.Key.Enable,
                                 LastModify = rmg.Key.LastModify,
                                 CreateDate = rmg.Key.CreateDate,
                             }).ToList(),
                     }).FirstOrDefault();
            }

            return oReturn;
        }

        public List<GenericItemModel> GetModuleOptionSearch(int RoleModuleId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vRoleModuleId", RoleModuleId));
            lstParams.Add(DataInstance.CreateTypedParameter("vViewEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_ModuleOption_Search",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemModel> oReturn = new List<GenericItemModel>();

            if (response.DataTableResult != null
                && response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from mo in response.DataTableResult.AsEnumerable()
                     where !mo.IsNull("ModuleOptionId")
                     group mo by new
                     {
                         ModuleOptionId = mo.Field<int>("ModuleOptionId"),
                         ModuleOptionTypeId = mo.Field<int>("ModuleOptionTypeId"),
                         ModuleOptionTypeName = mo.Field<string>("ModuleOptionTypeName"),
                         ModuleOption = mo.Field<string>("ModuleOption"),
                         Enable = mo.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = mo.Field<DateTime>("LastModify"),
                         CreateDate = mo.Field<DateTime>("CreateDate"),
                     }
                    into mog
                     select new GenericItemModel()
                     {
                         ItemId = mog.Key.ModuleOptionId,
                         ItemName = mog.Key.ModuleOption,
                         ItemType = new CatalogModel()
                         {
                             ItemId = mog.Key.ModuleOptionTypeId,
                             ItemName = mog.Key.ModuleOptionTypeName,
                         },
                         Enable = mog.Key.Enable,
                         LastModify = mog.Key.LastModify,
                         CreateDate = mog.Key.CreateDate,
                     }).ToList();
            }

            return oReturn;
        }

        public List<GenericItemInfoModel> GetModuleOptionInfoSearch(int ModuleOptionId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vModuleOptionId", ModuleOptionId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_ModuleOptionInfo_Search",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<GenericItemInfoModel> oReturn = new List<GenericItemInfoModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from moi in response.DataTableResult.AsEnumerable()
                     where !moi.IsNull("ModuleOptionInfoId")
                     group moi by new
                     {
                         ModuleOptionInfoId = moi.Field<int>("ModuleOptionInfoId"),
                         ModuleOptionInfoTypeId = moi.Field<int>("ModuleOptionInfoTypeId"),
                         ModuleOptionInfoTypeName = moi.Field<string>("ModuleOptionInfoTypeName"),
                         ModuleOptionInfoValue = moi.Field<string>("ModuleOptionInfoValue"),
                         ModuleOptionInfoLargeValue = moi.Field<string>("ModuleOptionInfoLargeValue"),
                         Enable = moi.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = moi.Field<DateTime>("LastModify"),
                         CreateDate = moi.Field<DateTime>("CreateDate"),
                     } into moig
                     select new GenericItemInfoModel()
                     {
                         ItemInfoId = moig.Key.ModuleOptionInfoId,
                         ItemInfoType = new CatalogModel()
                         {
                             ItemId = moig.Key.ModuleOptionInfoTypeId,
                             ItemName = moig.Key.ModuleOptionInfoTypeName,
                         },
                         Value = moig.Key.ModuleOptionInfoValue,
                         LargeValue = moig.Key.ModuleOptionInfoLargeValue,
                         Enable = moig.Key.Enable,
                         LastModify = moig.Key.LastModify,
                         CreateDate = moig.Key.CreateDate,
                     }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region Restrictive List

        public List<GenericItemModel> BlackListGetByCompanyPublicId(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_BlackListInfo_GetInfoByCompanyPublicId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bl in response.DataTableResult.AsEnumerable()
                     where !bl.IsNull("BlackListId")
                     group bl by new
                     {
                         BlackListId = bl.Field<int>("BlackListId"),
                         BlackListInfoId = bl.Field<int>("BlackListInfoId"),
                         BlackListInfoType = bl.Field<string>("BlackListInfoType"),
                         Value = bl.Field<string>("Value")
                     } into blg
                     select new GenericItemModel()
                     {
                         ItemId = blg.Key.BlackListId,
                         ItemType = new CatalogModel()
                         {
                             ItemName = blg.Key.BlackListInfoType,
                         },
                         ItemName = blg.Key.Value,
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Index

        public void CompanyBasicInfoIndex()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandText = "CP_BasicInfoIndex",
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandType = CommandType.StoredProcedure,
            });
        }

        public void CompanyActivityInfoIndex()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "CP_ActivityInfoIndex",
                CommandType = CommandType.StoredProcedure,
            });
        }

        #endregion
    }
}

