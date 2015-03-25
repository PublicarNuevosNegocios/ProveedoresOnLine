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

        public List<GenericItemModel> SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? ParentSurveyConfigItem, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentSurveyConfigItem", ParentSurveyConfigItem));
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

        #endregion

    }
}
