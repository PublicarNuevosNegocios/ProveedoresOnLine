using LogManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogManager.DAL.MySqlDao
{
    public class Log_MySqlDao : ILogData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Log_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(LogManager.Models.Constants.C_ConnectionName);
        }

        #region Implemented methods

        public int LogCreate(string User, string Application, string Source, bool IsSuccess, string Message)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vApplication", Application));
            lstParams.Add(DataInstance.CreateTypedParameter("vSource", Source));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsSuccess", IsSuccess));
            lstParams.Add(DataInstance.CreateTypedParameter("vMessage", Message));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "L_Log_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public void LogInfoCreate(int LogId, string LogInfoType, string Value)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vLogId", LogId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLogInfoType", LogInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "L_LogInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        #endregion
    }
}
