using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProveedoresOnLine.SurveyModule.Models;
using ProveedoresOnLine.Company.Models.Util;

namespace ProveedoresOnLine.SurveyModule.DAL.MySQLDAO
{
    internal class Survey_MySqlDao : ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Survey_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.SurveyModule.Models.Constants.C_POL_SurveyConnectionName);
        }

        #region Survey config

        public int SurveyConfigUpsert(string CustomerPublicId, int? SurveyConfigId, string SurveyName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyName", SurveyName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_SurveyConfig_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyConfigInfoUpsert(int? SurveyConfigInfoId, int SurveyConfigId, int SurveyConfigInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigInfoId", SurveyConfigInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigInfoType", SurveyConfigInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_SurveyConfigInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyConfigItemUpsert(int? SurveyConfigItemId, int SurveyConfigId, string SurveyConfigItemName, int SurveyConfigItemType, int? ParentSurveyConfigItem, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigItemId", SurveyConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigItemName", SurveyConfigItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigItemType", SurveyConfigItemType));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentSurveyConfigItem", ParentSurveyConfigItem));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_SurveyConfigItem_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyConfigItemInfoUpsert(int? SurveyConfigItemInfoId, int SurveyConfigItemId, int SurveyConfigItemInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigItemInfoId", SurveyConfigItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigItemId", SurveyConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigItemInfoType", SurveyConfigItemInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_SurveyConfigItemInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<SurveyConfigModel> SurveyConfigSearch
            (string CustomerPublicId, string SearchParam, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_SurveyConfig_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<SurveyConfigModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from sc in response.DataTableResult.AsEnumerable()
                     where !sc.IsNull("SurveyConfigId")
                     group sc by new
                     {
                         SurveyConfigId = sc.Field<int>("SurveyConfigId"),
                         SurveyName = sc.Field<string>("SurveyName"),
                         SurveyConfigEnable = sc.Field<UInt64>("SurveyConfigEnable") == 1 ? true : false,
                         SurveyConfigLastModify = sc.Field<DateTime>("SurveyConfigLastModify"),
                         SurveyConfigCreateDate = sc.Field<DateTime>("SurveyConfigCreateDate"),
                         HasEvaluations = sc.Field<Int32>("HasEvaluations") == 1 ? true : false,
                     } into scg
                     select new SurveyConfigModel()
                     {
                         ItemId = scg.Key.SurveyConfigId,
                         ItemName = scg.Key.SurveyName,
                         Enable = scg.Key.SurveyConfigEnable,
                         LastModify = scg.Key.SurveyConfigLastModify,
                         CreateDate = scg.Key.SurveyConfigCreateDate,

                         HasEvaluations = scg.Key.HasEvaluations,

                         ItemInfo =
                            (from scinf in response.DataTableResult.AsEnumerable()
                             where !scinf.IsNull("SurveyConfigInfoId") &&
                                    scinf.Field<int>("SurveyConfigId") == scg.Key.SurveyConfigId
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                     ItemName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                 },
                                 Value = scinf.Field<string>("SurveyConfigInfoValue"),
                                 LargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                 ValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                                 LastModify = scinf.Field<DateTime>("SurveyConfigInfoLastModify"),
                                 CreateDate = scinf.Field<DateTime>("SurveyConfigInfoCreateDate"),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public SurveyConfigModel SurveyConfigGetById(int SurveyConfigId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_SurveyConfig_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            SurveyConfigModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new SurveyConfigModel()
                {
                    ItemId = response.DataTableResult.Rows[0].Field<int>("SurveyConfigId"),
                    ItemName = response.DataTableResult.Rows[0].Field<string>("SurveyName"),
                    Enable = response.DataTableResult.Rows[0].Field<UInt64>("SurveyConfigEnable") == 1 ? true : false,
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("SurveyConfigLastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("SurveyConfigCreateDate"),

                    HasEvaluations = response.DataTableResult.Rows[0].Field<Int64>("HasEvaluations") == 1 ? true : false,

                    ItemInfo =
                       (from scinf in response.DataTableResult.AsEnumerable()
                        where !scinf.IsNull("SurveyConfigInfoId")
                        select new GenericItemInfoModel()
                        {
                            ItemInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                            ItemInfoType = new CatalogModel()
                            {
                                ItemId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                ItemName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                            },
                            Value = scinf.Field<string>("SurveyConfigInfoValue"),
                            LargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                            ValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                            LastModify = scinf.Field<DateTime>("SurveyConfigInfoLastModify"),
                            CreateDate = scinf.Field<DateTime>("SurveyConfigInfoCreateDate"),
                        }).ToList(),
                };
            }
            return oReturn;
        }

        public List<GenericItemModel> SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? ParentSurveyConfigItem, int? SurveyItemType,  bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentSurveyConfigItem", ParentSurveyConfigItem));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemType", SurveyItemType));

            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_SurveyConfigItem_GetBySurveyConfigId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from scit in response.DataTableResult.AsEnumerable()
                     where !scit.IsNull("SurveyConfigItemId")
                     group scit by new
                     {
                         SurveyConfigItemId = scit.Field<int>("SurveyConfigItemId"),
                         SurveyConfigItemName = scit.Field<string>("SurveyConfigItemName"),
                         SurveyConfigItemTypeId = scit.Field<int>("SurveyConfigItemTypeId"),
                         SurveyConfigItemTypeName = scit.Field<string>("SurveyConfigItemTypeName"),
                         ParentSurveyConfigItem = scit.Field<int?>("ParentSurveyConfigItem"),
                         SurveyConfigItemEnable = scit.Field<UInt64>("SurveyConfigItemEnable") == 1 ? true : false,
                         SurveyConfigItemLastModify = scit.Field<DateTime>("SurveyConfigItemLastModify"),
                         SurveyConfigItemCreateDate = scit.Field<DateTime>("SurveyConfigItemCreateDate"),

                     } into scitg
                     select new GenericItemModel()
                     {
                         ItemId = scitg.Key.SurveyConfigItemId,
                         ItemName = scitg.Key.SurveyConfigItemName,
                         ItemType = new CatalogModel()
                         {
                             ItemId = scitg.Key.SurveyConfigItemTypeId,
                             ItemName = scitg.Key.SurveyConfigItemTypeName,
                         },
                         ParentItem = ParentSurveyConfigItem == null ? null :
                            new GenericItemModel()
                            {
                                ItemId = (int)scitg.Key.ParentSurveyConfigItem,
                            },
                         Enable = scitg.Key.SurveyConfigItemEnable,
                         LastModify = scitg.Key.SurveyConfigItemLastModify,
                         CreateDate = scitg.Key.SurveyConfigItemCreateDate,

                         ItemInfo =
                            (from scitinf in response.DataTableResult.AsEnumerable()
                             where !scitinf.IsNull("SurveyConfigItemInfoId") &&
                                    scitinf.Field<int>("SurveyConfigItemId") == scitg.Key.SurveyConfigItemId
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = scitinf.Field<int>("SurveyConfigItemInfoId"),
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = scitinf.Field<int>("SurveyConfigItemInfoTypeId"),
                                     ItemName = scitinf.Field<string>("SurveyConfigItemInfoName"),
                                 },
                                 Value = scitinf.Field<string>("SurveyConfigItemInfoValue"),
                                 LargeValue = scitinf.Field<string>("SurveyConfigItemInfoLargeValue"),
                                 LastModify = scitinf.Field<DateTime>("SurveyConfigItemInfoLastModify"),
                                 CreateDate = scitinf.Field<DateTime>("SurveyConfigItemInfoCreateDate"),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<SurveyConfigModel> MP_SurveyConfigSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CC_SurveyConfig_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<SurveyConfigModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sc in response.DataTableResult.AsEnumerable()
                     where !sc.IsNull("SurveyConfigId")
                     group sc by new
                     {
                         SurveyConfigId = sc.Field<int>("SurveyConfigId"),
                         SurveyName = sc.Field<string>("SurveyName"),
                     } into scg
                     select new SurveyConfigModel()
                     {
                         ItemId = scg.Key.SurveyConfigId,
                         ItemName = scg.Key.SurveyName,

                         ItemInfo =
                            (from scinf in response.DataTableResult.AsEnumerable()
                             where !scinf.IsNull("SurveyConfigInfoId") &&
                                    scinf.Field<int>("SurveyConfigId") == scg.Key.SurveyConfigId
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                     ItemName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                 },
                                 Value = scinf.Field<string>("SurveyConfigInfoValue"),
                                 LargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                 ValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<GenericItemModel> MP_SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? SurveyItemType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));            
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemType", SurveyItemType));           

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CC_SurveyConfigItem_GetBySurveyConfigId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from scit in response.DataTableResult.AsEnumerable()
                     where !scit.IsNull("SurveyConfigItemId")
                     group scit by new
                     {
                         SurveyConfigItemId = scit.Field<int>("SurveyConfigItemId"),
                         SurveyConfigItemName = scit.Field<string>("SurveyConfigItemName"),
                         SurveyConfigItemTypeId = scit.Field<int>("SurveyConfigItemTypeId"),
                         SurveyConfigItemTypeName = scit.Field<string>("SurveyConfigItemTypeName"),
                         ParentSurveyConfigItem = scit.Field<int?>("ParentSurveyConfigItem"),
                         SurveyConfigItemEnable = scit.Field<UInt64>("SurveyConfigItemEnable") == 1 ? true : false,
                         SurveyConfigItemLastModify = scit.Field<DateTime>("SurveyConfigItemLastModify"),
                         SurveyConfigItemCreateDate = scit.Field<DateTime>("SurveyConfigItemCreateDate"),

                     } into scitg
                     select new GenericItemModel()
                     {
                         ItemId = scitg.Key.SurveyConfigItemId,
                         ItemName = scitg.Key.SurveyConfigItemName,
                         ItemType = new CatalogModel()
                         {
                             ItemId = scitg.Key.SurveyConfigItemTypeId,
                             ItemName = scitg.Key.SurveyConfigItemTypeName,
                         },
                         ParentItem = (int)scitg.Key.ParentSurveyConfigItem == null ? null :
                            new GenericItemModel()
                            {
                                ItemId = (int)scitg.Key.ParentSurveyConfigItem,
                            },
                         Enable = scitg.Key.SurveyConfigItemEnable,
                         LastModify = scitg.Key.SurveyConfigItemLastModify,
                         CreateDate = scitg.Key.SurveyConfigItemCreateDate,

                         ItemInfo =
                            (from scitinf in response.DataTableResult.AsEnumerable()
                             where !scitinf.IsNull("SurveyConfigItemInfoId") &&
                                    scitinf.Field<int>("SurveyConfigItemId") == scitg.Key.SurveyConfigItemId
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = scitinf.Field<int>("SurveyConfigItemInfoId"),
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = scitinf.Field<int>("SurveyConfigItemInfoTypeId"),
                                     ItemName = scitinf.Field<string>("SurveyConfigItemInfoName"),
                                 },
                                 Value = scitinf.Field<string>("SurveyConfigItemInfoValue"),
                                 LargeValue = scitinf.Field<string>("SurveyConfigItemInfoLargeValue"),
                                 LastModify = scitinf.Field<DateTime>("SurveyConfigItemInfoLastModify"),
                                 CreateDate = scitinf.Field<DateTime>("SurveyConfigItemInfoCreateDate"),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region Survey

        public string SurveyUpsert(string SurveyPublicId, string ProviderPublicId, int SurveyConfigId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyPublicId", SurveyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_Survey_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return response.ScalarResult.ToString();
        }

        public int SurveyInfoUpsert(int? SurveyInfoId, string SurveyPublicId, int SurveyInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyInfoId", SurveyInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyPublicId", SurveyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyInfoType", SurveyInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_SurveyInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyItemUpsert(int? SurveyItemId, string SurveyPublicId, int SurveyConfigItemId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemId", SurveyItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyPublicId", SurveyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigItemId", SurveyConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_SurveyItem_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyItemInfoUpsert(int? SurveyItemInfoId, int SurveyItemId, int SurveyItemInfoType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemInfoId", SurveyItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemId", SurveyItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemInfoType", SurveyItemInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CP_SurveyItemInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<SurveyModel> SurveySearch(string CustomerPublicId, string ProviderPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSearchOrderType", SearchOrderType));
            lstParams.Add(DataInstance.CreateTypedParameter("vOrderOrientation", OrderOrientation));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Survey_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<SurveyModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from sv in response.DataTableResult.AsEnumerable()
                     where !sv.IsNull("SurveyPublicId")
                     group sv by new
                     {
                         SurveyPublicId = sv.Field<string>("SurveyPublicId"),
                         LastModify = sv.Field<DateTime>("LastModify"),
                         CreateDate = sv.Field<DateTime>("CreateDate"),
                         SurveyConfigId = sv.Field<int>("SurveyConfigId"),
                         SurveyName = sv.Field<string>("SurveyName"),
                     } into svg
                     select new SurveyModel()
                     {
                         SurveyPublicId = svg.Key.SurveyPublicId,
                         LastModify = svg.Key.LastModify,
                         CreateDate = svg.Key.CreateDate,

                         RelatedSurveyConfig = new SurveyConfigModel()
                         {
                             ItemId = svg.Key.SurveyConfigId,
                             ItemName = svg.Key.SurveyName,

                             ItemInfo =
                                (from scinf in response.DataTableResult.AsEnumerable()
                                 where !scinf.IsNull("SurveyConfigInfoId") &&
                                        scinf.Field<int>("SurveyConfigId") == svg.Key.SurveyConfigId &&
                                        scinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                                 group scinf by new
                                 {
                                     SurveyConfigInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                     SurveyConfigInfoTypeId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                     SurveyConfigInfoTypeName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                     SurveyConfigInfoValue = scinf.Field<string>("SurveyConfigInfoValue"),
                                     SurveyConfigInfoLargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                     SurveyConfigInfoValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                                 } into scinfg
                                 select new GenericItemInfoModel()
                                 {
                                     ItemInfoId = scinfg.Key.SurveyConfigInfoId,
                                     ItemInfoType = new CatalogModel()
                                     {
                                         ItemId = scinfg.Key.SurveyConfigInfoTypeId,
                                         ItemName = scinfg.Key.SurveyConfigInfoTypeName,
                                     },
                                     Value = scinfg.Key.SurveyConfigInfoValue,
                                     LargeValue = scinfg.Key.SurveyConfigInfoLargeValue,
                                     ValueName = scinfg.Key.SurveyConfigInfoValueName,
                                 }).ToList(),
                         },
                         SurveyInfo =
                            (from svinf in response.DataTableResult.AsEnumerable()
                             where !svinf.IsNull("SurveyInfoId") &&
                                    svinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                             group svinf by new
                             {
                                 SurveyInfoId = svinf.Field<int>("SurveyInfoId"),
                                 SurveyInfoTypeId = svinf.Field<int>("SurveyInfoTypeId"),
                                 SurveyInfoTypeName = svinf.Field<string>("SurveyInfoTypeName"),
                                 SurveyInfoValue = svinf.Field<string>("SurveyInfoValue"),
                                 SurveyInfoLargeValue = svinf.Field<string>("SurveyInfoLargeValue"),
                                 SurveyInfoValueName = svinf.Field<string>("SurveyInfoValueName"),

                             } into svinfg
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = svinfg.Key.SurveyInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = svinfg.Key.SurveyInfoTypeId,
                                     ItemName = svinfg.Key.SurveyInfoTypeName,
                                 },
                                 Value = svinfg.Key.SurveyInfoValue,
                                 LargeValue = svinfg.Key.SurveyInfoLargeValue,
                                 ValueName = svinfg.Key.SurveyInfoValueName,
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public SurveyModel SurveyGetById(string SurveyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyPublicId", SurveyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CP_Survey_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            SurveyModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 1 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[1].Rows.Count > 0)
            {
                oReturn = new SurveyModel()
                {
                    SurveyPublicId = response.DataSetResult.Tables[1].Rows[0].Field<string>("SurveyPublicId"),
                    LastModify = response.DataSetResult.Tables[1].Rows[0].Field<DateTime>("SurveyLastModify"),

                    RelatedProvider = new CompanyProvider.Models.Provider.ProviderModel()
                    {
                        RelatedCompany = new Company.Models.Company.CompanyModel()
                        {
                            CompanyPublicId = response.DataSetResult.Tables[1].Rows[0].Field<string>("ProviderPublicId"),
                        },
                    },

                    SurveyInfo =
                        (from svinf in response.DataSetResult.Tables[1].AsEnumerable()
                         where !svinf.IsNull("SurveyInfoId") &&
                                svinf.Field<string>("SurveyPublicId") == response.DataSetResult.Tables[1].Rows[0].Field<string>("SurveyPublicId")
                         group svinf by new
                         {
                             SurveyInfoId = svinf.Field<int>("SurveyInfoId"),
                             SurveyInfoTypeId = svinf.Field<int>("SurveyInfoTypeId"),
                             SurveyInfoTypeName = svinf.Field<string>("SurveyInfoTypeName"),
                             SurveyInfoValue = svinf.Field<string>("SurveyInfoValue"),
                             SurveyInfoLargeValue = svinf.Field<string>("SurveyInfoLargeValue"),
                             SurveyInfoValueName = svinf.Field<string>("SurveyInfoValueName"),
                         } into svinfg
                         select new GenericItemInfoModel()
                         {
                             ItemInfoId = svinfg.Key.SurveyInfoId,
                             ItemInfoType = new CatalogModel()
                             {
                                 ItemId = svinfg.Key.SurveyInfoTypeId,
                                 ItemName = svinfg.Key.SurveyInfoTypeName,
                             },
                             Value = svinfg.Key.SurveyInfoValue,
                             LargeValue = svinfg.Key.SurveyInfoLargeValue,
                             ValueName = svinfg.Key.SurveyInfoValueName,
                         }).ToList(),

                    RelatedSurveyItem =
                        (from svit in response.DataSetResult.Tables[1].AsEnumerable()
                         where !svit.IsNull("SurveyItemId") &&
                                svit.Field<string>("SurveyPublicId") == response.DataSetResult.Tables[1].Rows[0].Field<string>("SurveyPublicId")
                         group svit by new
                         {
                             SurveyItemId = svit.Field<int>("SurveyItemId"),
                             SurveyItemSurveyConfigItemId = svit.Field<int>("SurveyItemSurveyConfigItemId"),
                             SurveyItemLastModify = svit.Field<DateTime>("SurveyItemLastModify"),
                         } into svitg
                         select new SurveyItemModel()
                         {
                             ItemId = svitg.Key.SurveyItemId,
                             RelatedSurveyConfigItem = new GenericItemModel()
                             {
                                 ItemId = svitg.Key.SurveyItemSurveyConfigItemId,
                             },
                             LastModify = svitg.Key.SurveyItemLastModify,

                             ItemInfo =
                                (from svitinf in response.DataSetResult.Tables[1].AsEnumerable()
                                 where !svitinf.IsNull("SurveyItemInfoId") &&
                                        svitinf.Field<int>("SurveyItemId") == svitg.Key.SurveyItemId
                                 group svitinf by new
                                 {
                                     SurveyItemInfoId = svitinf.Field<int>("SurveyItemInfoId"),
                                     SurveyItemInfoTypeId = svitinf.Field<int>("SurveyItemInfoTypeId"),
                                     SurveyItemInfoTypeName = svitinf.Field<string>("SurveyItemInfoTypeName"),
                                     SurveyItemInfoValue = svitinf.Field<string>("SurveyItemInfoValue"),
                                     SurveyItemInfoLargeValue = svitinf.Field<string>("SurveyItemInfoLargeValue"),
                                 } into svitinfg
                                 select new GenericItemInfoModel()
                                 {
                                     ItemInfoId = svitinfg.Key.SurveyItemInfoId,
                                     ItemInfoType = new CatalogModel()
                                     {
                                         ItemId = svitinfg.Key.SurveyItemInfoTypeId,
                                         ItemName = svitinfg.Key.SurveyItemInfoTypeName,
                                     },
                                     Value = svitinfg.Key.SurveyItemInfoValue,
                                     LargeValue = svitinfg.Key.SurveyItemInfoLargeValue,
                                 }).ToList(),
                         }).ToList(),

                    RelatedSurveyConfig = new SurveyConfigModel()
                    {
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("SurveyConfigId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("SurveyName"),

                        RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                        {
                            RelatedCompany = new Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerPublicId"),
                            },
                        },

                        ItemInfo =
                           (from scinf in response.DataSetResult.Tables[0].AsEnumerable()
                            where !scinf.IsNull("SurveyConfigInfoId") &&
                                   scinf.Field<int>("SurveyConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<int>("SurveyConfigId")
                            group scinf by new
                            {
                                SurveyConfigInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                SurveyConfigInfoTypeId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                SurveyConfigInfoTypeName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                SurveyConfigInfoValue = scinf.Field<string>("SurveyConfigInfoValue"),
                                SurveyConfigInfoLargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                SurveyConfigInfoValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                            } into scinfg
                            select new GenericItemInfoModel()
                            {
                                ItemInfoId = scinfg.Key.SurveyConfigInfoId,
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = scinfg.Key.SurveyConfigInfoTypeId,
                                    ItemName = scinfg.Key.SurveyConfigInfoTypeName,
                                },
                                Value = scinfg.Key.SurveyConfigInfoValue,
                                LargeValue = scinfg.Key.SurveyConfigInfoLargeValue,
                                ValueName = scinfg.Key.SurveyConfigInfoValueName,
                            }).ToList(),

                        RelatedSurveyConfigItem =
                            (from scit in response.DataSetResult.Tables[0].AsEnumerable()
                             where !scit.IsNull("SurveyConfigItemId") &&
                                    scit.Field<int>("SurveyConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<int>("SurveyConfigId")
                             group scit by new
                             {
                                 SurveyConfigItemId = scit.Field<int>("SurveyConfigItemId"),
                                 SurveyConfigItemName = scit.Field<string>("SurveyConfigItemName"),
                                 SurveyConfigItemTypeId = scit.Field<int>("SurveyConfigItemTypeId"),
                                 SurveyConfigItemTypeName = scit.Field<string>("SurveyConfigItemTypeName"),
                                 ParentSurveyConfigItem = scit.Field<int?>("ParentSurveyConfigItem"),
                             } into scitg
                             select new GenericItemModel()
                             {
                                 ItemId = scitg.Key.SurveyConfigItemId,
                                 ItemName = scitg.Key.SurveyConfigItemName,
                                 ItemType = new CatalogModel()
                                 {
                                     ItemId = scitg.Key.SurveyConfigItemTypeId,
                                     ItemName = scitg.Key.SurveyConfigItemTypeName,
                                 },
                                 ParentItem = scitg.Key.ParentSurveyConfigItem == null ? null :
                                    new GenericItemModel()
                                    {
                                        ItemId = scitg.Key.ParentSurveyConfigItem.Value,
                                    },

                                 ItemInfo =
                                     (from scitinf in response.DataSetResult.Tables[0].AsEnumerable()
                                      where !scitinf.IsNull("SurveyConfigItemInfoId") &&
                                             scitinf.Field<int>("SurveyConfigItemId") == scitg.Key.SurveyConfigItemId
                                      group scitinf by new
                                      {
                                          SurveyConfigItemInfoId = scitinf.Field<int>("SurveyConfigItemInfoId"),
                                          SurveyConfigItemInfoTypeId = scitinf.Field<int>("SurveyConfigItemInfoTypeId"),
                                          SurveyConfigItemInfoTypeName = scitinf.Field<string>("SurveyConfigItemInfoTypeName"),
                                          SurveyConfigItemInfoValue = scitinf.Field<string>("SurveyConfigItemInfoValue"),
                                          SurveyConfigItemInfoLargeValue = scitinf.Field<string>("SurveyConfigItemInfoLargeValue"),
                                      } into scitinfg
                                      select new GenericItemInfoModel()
                                      {
                                          ItemInfoId = scitinfg.Key.SurveyConfigItemInfoId,
                                          ItemInfoType = new CatalogModel()
                                          {
                                              ItemId = scitinfg.Key.SurveyConfigItemInfoTypeId,
                                              ItemName = scitinfg.Key.SurveyConfigItemInfoTypeName,
                                          },
                                          Value = scitinfg.Key.SurveyConfigItemInfoValue,
                                          LargeValue = scitinfg.Key.SurveyConfigItemInfoLargeValue,
                                      }).ToList(),
                             }).ToList(),
                    },
                };
            }
            return oReturn;
        }

        public List<SurveyModel> SurveyGetByCustomerProvider(string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Survey_GetByCustomerProvider",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<SurveyModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sv in response.DataTableResult.AsEnumerable()
                     where !sv.IsNull("SurveyPublicId")
                     group sv by new
                     {
                         SurveyPublicId = sv.Field<string>("SurveyPublicId"),
                         LastModify = sv.Field<DateTime>("LastModify"),
                         SurveyConfigId = sv.Field<int>("SurveyConfigId"),
                         SurveyName = sv.Field<string>("SurveyName"),
                     } into svg
                     select new SurveyModel()
                     {
                         SurveyPublicId = svg.Key.SurveyPublicId,
                         LastModify = svg.Key.LastModify,

                         RelatedSurveyConfig = new SurveyConfigModel()
                         {
                             ItemId = svg.Key.SurveyConfigId,
                             ItemName = svg.Key.SurveyName,

                             ItemInfo =
                                (from scinf in response.DataTableResult.AsEnumerable()
                                 where !scinf.IsNull("SurveyConfigInfoId") &&
                                        scinf.Field<int>("SurveyConfigId") == svg.Key.SurveyConfigId &&
                                        scinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                                 group scinf by new
                                 {
                                     SurveyConfigInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                     SurveyConfigInfoTypeId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                     SurveyConfigInfoTypeName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                     SurveyConfigInfoValue = scinf.Field<string>("SurveyConfigInfoValue"),
                                     SurveyConfigInfoLargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                     SurveyConfigInfoValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                                 } into scinfg
                                 select new GenericItemInfoModel()
                                 {
                                     ItemInfoId = scinfg.Key.SurveyConfigInfoId,
                                     ItemInfoType = new CatalogModel()
                                     {
                                         ItemId = scinfg.Key.SurveyConfigInfoTypeId,
                                         ItemName = scinfg.Key.SurveyConfigInfoTypeName,
                                     },
                                     Value = scinfg.Key.SurveyConfigInfoValue,
                                     LargeValue = scinfg.Key.SurveyConfigInfoLargeValue,
                                     ValueName = scinfg.Key.SurveyConfigInfoValueName,
                                 }).ToList(),
                         },
                         SurveyInfo =
                            (from svinf in response.DataTableResult.AsEnumerable()
                             where !svinf.IsNull("SurveyInfoId") &&
                                    svinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                             group svinf by new
                             {
                                 SurveyInfoId = svinf.Field<int>("SurveyInfoId"),
                                 SurveyInfoTypeId = svinf.Field<int>("SurveyInfoTypeId"),
                                 SurveyInfoTypeName = svinf.Field<string>("SurveyInfoTypeName"),
                                 SurveyInfoValue = svinf.Field<string>("SurveyInfoValue"),
                                 SurveyInfoLargeValue = svinf.Field<string>("SurveyInfoLargeValue"),
                                 SurveyInfoValueName = svinf.Field<string>("SurveyInfoValueName"),

                             } into svinfg
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = svinfg.Key.SurveyInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = svinfg.Key.SurveyInfoTypeId,
                                     ItemName = svinfg.Key.SurveyInfoTypeName,
                                 },
                                 Value = svinfg.Key.SurveyInfoValue,
                                 LargeValue = svinfg.Key.SurveyInfoLargeValue,
                                 ValueName = svinfg.Key.SurveyInfoValueName,
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region SurveyBatch

        public List<SurveyModel> BPSurveyGetNotification()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "BP_MP_Survey_GetNotification",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<SurveyModel> oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 2 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[1].Rows.Count > 0 &&
                response.DataSetResult.Tables[2] != null &&
                response.DataSetResult.Tables[2].Rows.Count > 0)
            {
                oReturn =
                    (from sv in response.DataSetResult.Tables[1].AsEnumerable()
                     group sv by new
                     {
                         SurveyPublicId = sv.Field<string>("SurveyPublicId"),
                         LastModify = sv.Field<DateTime>("SurveyLastModify"),
                         ProviderPublicId = sv.Field<string>("ProviderPublicId"),
                     } into svg
                     select new SurveyModel()
                     {
                         SurveyPublicId = svg.Key.SurveyPublicId,
                         LastModify = svg.Key.LastModify,

                         SurveyInfo =
                            (from svinf in response.DataSetResult.Tables[1].AsEnumerable()
                             where !svinf.IsNull("SurveyInfoId") &&
                                     svinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                             group svinf by new
                             {
                                 SurveyInfoId = svinf.Field<int>("SurveyInfoId"),
                                 SurveyInfoTypeId = svinf.Field<int>("SurveyInfoTypeId"),
                                 SurveyInfoTypeName = svinf.Field<string>("SurveyInfoTypeName"),
                                 SurveyInfoValue = svinf.Field<string>("SurveyInfoValue"),
                                 SurveyInfoLargeValue = svinf.Field<string>("SurveyInfoLargeValue"),
                                 SurveyInfoValueName = svinf.Field<string>("SurveyInfoValueName"),
                             } into svinfg
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = svinfg.Key.SurveyInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = svinfg.Key.SurveyInfoTypeId,
                                     ItemName = svinfg.Key.SurveyInfoTypeName,
                                 },
                                 Value = svinfg.Key.SurveyInfoValue,
                                 LargeValue = svinfg.Key.SurveyInfoLargeValue,
                                 ValueName = svinfg.Key.SurveyInfoValueName,
                             }).ToList(),

                         RelatedProvider =
                            (from prv in response.DataSetResult.Tables[2].AsEnumerable()
                             where !prv.IsNull("CompanyPublicId") &&
                                    prv.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId &&
                                    prv.Field<string>("CompanyPublicId") == svg.Key.ProviderPublicId
                             group prv by new
                             {
                                 CompanyPublicId = prv.Field<string>("CompanyPublicId"),
                                 CompanyName = prv.Field<string>("CompanyName"),
                                 IdentificationTypeId = prv.Field<int>("IdentificationTypeId"),
                                 IdentificationTypeName = prv.Field<string>("IdentificationTypeName"),
                                 IdentificationNumber = prv.Field<string>("IdentificationNumber"),
                                 CompanyTypeId = prv.Field<int>("CompanyTypeId"),
                                 CompanyTypeName = prv.Field<string>("CompanyTypeName"),
                             } into prvg
                             select new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                             {
                                 RelatedCompany = new Company.Models.Company.CompanyModel()
                                 {
                                     CompanyPublicId = prvg.Key.CompanyPublicId,
                                     CompanyName = prvg.Key.CompanyName,
                                     IdentificationType = new CatalogModel()
                                     {
                                         ItemId = prvg.Key.IdentificationTypeId,
                                         ItemName = prvg.Key.IdentificationTypeName,
                                     },
                                     IdentificationNumber = prvg.Key.IdentificationNumber,
                                     CompanyType = new CatalogModel()
                                     {
                                         ItemId = prvg.Key.CompanyTypeId,
                                         ItemName = prvg.Key.CompanyTypeName,
                                     },

                                     CompanyInfo =
                                        (from prvinf in response.DataSetResult.Tables[2].AsEnumerable()
                                         where !prvinf.IsNull("CompanyInfoId") &&
                                                prvinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId &&
                                                prvinf.Field<string>("CompanyPublicId") == svg.Key.ProviderPublicId
                                         group prvinf by new
                                         {
                                             CompanyInfoId = prvinf.Field<int>("CompanyInfoId"),
                                             CompanyInfoTypeId = prvinf.Field<int>("CompanyInfoTypeId"),
                                             CompanyInfoTypeName = prvinf.Field<string>("CompanyInfoTypeName"),
                                             CompanyInfoValue = prvinf.Field<string>("CompanyInfoValue"),
                                             CompanyInfoLargeValue = prvinf.Field<string>("CompanyInfoLargeValue"),
                                         } into prvinfg
                                         select new GenericItemInfoModel()
                                         {
                                             ItemInfoId = prvinfg.Key.CompanyInfoId,
                                             ItemInfoType = new CatalogModel()
                                             {
                                                 ItemId = prvinfg.Key.CompanyInfoTypeId,
                                                 ItemName = prvinfg.Key.CompanyInfoTypeName,
                                             },
                                             Value = prvinfg.Key.CompanyInfoValue,
                                             LargeValue = prvinfg.Key.CompanyInfoLargeValue,
                                         }).ToList(),
                                 },
                             }).FirstOrDefault(),

                         RelatedSurveyConfig =
                            (from sc in response.DataSetResult.Tables[0].AsEnumerable()
                             where !sc.IsNull("SurveyConfigId") &&
                                     sc.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                             group sc by new
                             {
                                 SurveyConfigId = sc.Field<int>("SurveyConfigId"),
                                 SurveyName = sc.Field<string>("SurveyName"),
                                 CustomerPublicId = sc.Field<string>("CustomerPublicId"),
                             } into scg
                             select new SurveyConfigModel()
                             {
                                 ItemId = scg.Key.SurveyConfigId,
                                 ItemName = scg.Key.SurveyName,

                                 ItemInfo =
                                    (from scinf in response.DataSetResult.Tables[0].AsEnumerable()
                                     where !scinf.IsNull("SurveyConfigInfoId") &&
                                            scinf.Field<int>("SurveyConfigId") == scg.Key.SurveyConfigId
                                     group scinf by new
                                     {
                                         SurveyConfigInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                         SurveyConfigInfoTypeId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                         SurveyConfigInfoTypeName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                         SurveyConfigInfoValue = scinf.Field<string>("SurveyConfigInfoValue"),
                                         SurveyConfigInfoLargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                         SurveyConfigInfoValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                                     } into scinfg
                                     select new GenericItemInfoModel()
                                     {
                                         ItemInfoId = scinfg.Key.SurveyConfigInfoId,
                                         ItemInfoType = new CatalogModel()
                                         {
                                             ItemId = scinfg.Key.SurveyConfigInfoTypeId,
                                             ItemName = scinfg.Key.SurveyConfigInfoTypeName,
                                         },
                                         Value = scinfg.Key.SurveyConfigInfoValue,
                                         LargeValue = scinfg.Key.SurveyConfigInfoLargeValue,
                                         ValueName = scinfg.Key.SurveyConfigInfoValueName,
                                     }).ToList(),

                                 RelatedCustomer =
                                     (from cst in response.DataSetResult.Tables[2].AsEnumerable()
                                      where !cst.IsNull("CompanyPublicId") &&
                                             cst.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId &&
                                             cst.Field<string>("CompanyPublicId") == scg.Key.CustomerPublicId
                                      group cst by new
                                      {
                                          CompanyPublicId = cst.Field<string>("CompanyPublicId"),
                                          CompanyName = cst.Field<string>("CompanyName"),
                                          IdentificationTypeId = cst.Field<int>("IdentificationTypeId"),
                                          IdentificationTypeName = cst.Field<string>("IdentificationTypeName"),
                                          IdentificationNumber = cst.Field<string>("IdentificationNumber"),
                                          CompanyTypeId = cst.Field<int>("CompanyTypeId"),
                                          CompanyTypeName = cst.Field<string>("CompanyTypeName"),
                                      } into cstg
                                      select new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel()
                                      {
                                          RelatedCompany = new Company.Models.Company.CompanyModel()
                                          {
                                              CompanyPublicId = cstg.Key.CompanyPublicId,
                                              CompanyName = cstg.Key.CompanyName,
                                              IdentificationType = new CatalogModel()
                                              {
                                                  ItemId = cstg.Key.IdentificationTypeId,
                                                  ItemName = cstg.Key.IdentificationTypeName,
                                              },
                                              IdentificationNumber = cstg.Key.IdentificationNumber,
                                              CompanyType = new CatalogModel()
                                              {
                                                  ItemId = cstg.Key.CompanyTypeId,
                                                  ItemName = cstg.Key.CompanyTypeName,
                                              },

                                              CompanyInfo =
                                                 (from cstinf in response.DataSetResult.Tables[2].AsEnumerable()
                                                  where !cstinf.IsNull("CompanyInfoId") &&
                                                         cstinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId &&
                                                         cstinf.Field<string>("CompanyPublicId") == scg.Key.CustomerPublicId
                                                  group cstinf by new
                                                  {
                                                      CompanyInfoId = cstinf.Field<int>("CompanyInfoId"),
                                                      CompanyInfoTypeId = cstinf.Field<int>("CompanyInfoTypeId"),
                                                      CompanyInfoTypeName = cstinf.Field<string>("CompanyInfoTypeName"),
                                                      CompanyInfoValue = cstinf.Field<string>("CompanyInfoValue"),
                                                      CompanyInfoLargeValue = cstinf.Field<string>("CompanyInfoLargeValue"),
                                                  } into cstinfg
                                                  select new GenericItemInfoModel()
                                                  {
                                                      ItemInfoId = cstinfg.Key.CompanyInfoId,
                                                      ItemInfoType = new CatalogModel()
                                                      {
                                                          ItemId = cstinfg.Key.CompanyInfoTypeId,
                                                          ItemName = cstinfg.Key.CompanyInfoTypeName,
                                                      },
                                                      Value = cstinfg.Key.CompanyInfoValue,
                                                      LargeValue = cstinfg.Key.CompanyInfoLargeValue,
                                                  }).ToList(),
                                          },
                                      }).FirstOrDefault(),
                             }).FirstOrDefault(),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region SurveyCharts

        public List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByResponsable(string ResponsableEmail, DateTime Year)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vResponable", ResponsableEmail));
            lstParams.Add(DataInstance.CreateTypedParameter("vCurrentDate", Year));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_SurveyCharts_GetByResponsable",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericChartsModelInfo> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                   (from sv in response.DataTableResult.AsEnumerable()
                    where !sv.IsNull("Count")
                    select new GenericChartsModelInfo()
                    {
                        Title = sv.Field<string>("Title"),
                        ItemType = sv.Field<string>("ItemType"),
                        ItemName = sv.Field<string>("ItemName"),
                        Count = (int)sv.Field<Int64>("Count"),
                    }).ToList();
            }

            return oReturn;
        }

        #endregion        
    }
}
