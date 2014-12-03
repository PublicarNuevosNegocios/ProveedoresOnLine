using LogManager.Interfaces;
using LogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LogManager.DAL.MySqlDao
{
    public class Log_MySqlDao : ILogData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Log_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(LogManager.Models.Constants.C_ConnectionName);
        }

        #region Generic Log

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

        public List<LogModel> LogSearch(string LogInfoType, string Value)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vLogInfoType", LogInfoType));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "L_Log_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<LogModel> oReturn = new List<LogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from l in response.DataTableResult.AsEnumerable()
                     group l by
                     new
                     {
                         LogId = l.Field<int>("LogId"),
                         User = l.Field<string>("User"),
                         Application = l.Field<string>("Application"),
                         Source = l.Field<string>("Source"),
                         IsSuccess = l.Field<UInt64>("IsSuccess") == 1 ? true : false,
                         Message = l.Field<string>("Message"),
                         CreateDate = l.Field<DateTime>("CreateDate"),
                     } into lg
                     select new LogModel()
                     {
                         LogId = lg.Key.LogId,
                         User = lg.Key.User,
                         Application = lg.Key.Application,
                         Source = lg.Key.Source,
                         IsSuccess = lg.Key.IsSuccess,
                         Message = lg.Key.Message,
                         CreateDate = lg.Key.CreateDate,

                         RelatedLogInfo =
                            (from li in response.DataTableResult.AsEnumerable()
                             where li.Field<int>("LogId") == lg.Key.LogId
                             select new LogInfoModel()
                             {
                                 LogInfoId = li.Field<int>("LogInfoId"),
                                 LogInfoType = li.Field<string>("LogInfoType"),
                                 Value = li.Field<string>("Value"),
                             }).ToList(),
                     }).ToList();
            }
            return oReturn;

        }

        #endregion

        #region File

        public void FileUploadCreate(string Url)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUrl", Url));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_FileUpload_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void FileUsedCreate(string Url)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUrl", Url));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "F_FileUsed_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        #endregion
    }
}
