using ProveedoresOnLine.CalificationProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ProveedoresOnLine.CalificationProject.DAL.MySqlDAO
{
    internal class CalificationProject_MySqlDao : ICalificationProjectData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CalificationProject_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CalificationProject.Models.Constants.C_POL_CalificatioProjectConnectionName);
        }
        #region ProjectConfig
        public int CalificationProjectConfigUpsert(int CalificationProjectConfigId, string Company, string CalificationProjectConfigName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", Company));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigName", CalificationProjectConfigName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", (Enable == true) ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfig_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });
            return Convert.ToInt32(response.ScalarResult);
        }
        #endregion

        #region ConfigItem

        public int CalificationProjectConfigItemUpsert(int CalificationProjectConfigId, int CalificationProjectConfigItemId, string CalificationProjectConfigItemName, int CalificationProjectConfigItemType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", CalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemName", CalificationProjectConfigItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemType", CalificationProjectConfigItemType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfigItem_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<Models.CalificationProject.ConfigItemModel> CalificationProjectConfigItem_GetByCalificationProjectConfigId(int CalificationProjectConfigId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_CalificationProjectConfigItem_GetByConfigId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<Models.CalificationProject.ConfigItemModel> oReturn = new List<Models.CalificationProject.ConfigItemModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cit in response.DataTableResult.AsEnumerable()
                     where !cit.IsNull("CalificationProjectConfigId")
                     group cit by new
                     {
                         CalificationProjectConfigItemId = cit.Field<int>("CalificationProjectConfigItemId"),
                         CalificationProjectConfigId = cit.Field<int>("CalificationProjectConfigId"),
                         CalificationProjectConfigItemName = cit.Field<string>("CalificationProjectConfigItemName"),
                         CalicationProjecConfigItemTypeId = cit.Field<int>("CalicationProjecConfigItemTypeId"),
                         CalicationProjecConfigItemTypeName = cit.Field<string>("CalicationProjecConfigItemTypeName"),
                         Enable = cit.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = cit.Field<DateTime>("LastModify"),
                         CreateDate = cit.Field<DateTime>("CreateDate"),
                     }
                         into citg
                         select new Models.CalificationProject.ConfigItemModel()
                         {
                             CalificationProjectConfigItemId = citg.Key.CalificationProjectConfigItemId,
                             CalificationProjectConfigId = citg.Key.CalificationProjectConfigId,
                             CalificationProjectConfigItemName = citg.Key.CalificationProjectConfigItemName,
                             CalificationProjectConfigItemType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = citg.Key.CalicationProjecConfigItemTypeId,
                                 ItemName = citg.Key.CalicationProjecConfigItemTypeName,
                             },
                             Enable = citg.Key.Enable,
                             LastModify = citg.Key.LastModify,
                             CreateDate = citg.Key.CreateDate,
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #region ConfigItemInfo

        public int CalificationProjectConfigItemInfoUpsert(int CalificationProjectConfigItemId, int CalificationProjectConfigItemInfoId, int Question, int Rule, int ValueType, string Value, string Score, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", CalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemInfoId", CalificationProjectConfigItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vQuestion", Question));
            lstParams.Add(DataInstance.CreateTypedParameter("vRule", Rule));
            lstParams.Add(DataInstance.CreateTypedParameter("vValueType", ValueType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vScore", Score));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfigItemInfo_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<Models.CalificationProject.ConfigItemInfoModel> CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(int CalificationProjectConfigItemId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", CalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<Models.CalificationProject.ConfigItemInfoModel> oReturn = new List<Models.CalificationProject.ConfigItemInfoModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cinf in response.DataTableResult.AsEnumerable()
                     where !cinf.IsNull("CalificationProjectConfigItemInfoId")
                     group cinf by new
                     {
                         CalificationProjectConfigItemInfoId = cinf.Field<int>("CalificationProjectConfigItemInfoId"),
                         CalificationProjectConfigItemId = cinf.Field<int>("CalificationProjectConfigItemId"),
                         QuestionId = cinf.Field<int>("QuestionId"),
                         QuestionName = cinf.Field<string>("QuestionName"),
                         RuleId = cinf.Field<int>("RuleId"),
                         RuleName = cinf.Field<string>("RuleName"),
                         ValueTypeId = cinf.Field<int>("ValueTypeId"),
                         ValueTypeName = cinf.Field<string>("ValueTypeName"),
                         Value = cinf.Field<string>("Value"),
                         Score = cinf.Field<string>("Score"),
                         Enable = cinf.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = cinf.Field<DateTime>("LastModify"),
                         CreateDate = cinf.Field<DateTime>("CreateDate"),
                     }
                         into cinfg
                         select new Models.CalificationProject.ConfigItemInfoModel()
                         {
                             CalificationProjectConfigItemInfoId = cinfg.Key.CalificationProjectConfigItemInfoId,
                             CalificationProjectConfigItemId = cinfg.Key.CalificationProjectConfigItemId,
                             Question = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = cinfg.Key.QuestionId,
                                 ItemName = cinfg.Key.QuestionName,
                             },
                             Rule = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = cinfg.Key.RuleId,
                                 ItemName = cinfg.Key.RuleName,
                             },
                             ValueType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = cinfg.Key.ValueTypeId,
                                 ItemName = cinfg.Key.ValueTypeName,
                             },
                             Value = cinfg.Key.Value,
                             Score = cinfg.Key.Score,
                             Enable = cinfg.Key.Enable,
                             LastModify = cinfg.Key.LastModify,
                             CreateDate = cinfg.Key.CreateDate,
                         }).ToList();
            }

            return oReturn;
        }

        #endregion        


    
        
    }
}
