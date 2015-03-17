using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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

        #endregion
    }
}
