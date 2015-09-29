using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MessageModule.Client.Models;

namespace MessageModule.Client.DAL.MySQLDAO
{
    internal class Client_MySqlDao : MessageModule.Client.Interfaces.IClientData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Client_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(MessageModule.Client.Models.Constants.C_MS_ClientConnection);
        }

        public int MessageQueueCreate(string Agent, DateTime ProgramTime, string User)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAgent", Agent));
            lstParams.Add(DataInstance.CreateTypedParameter("vProgramTime", ProgramTime));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "B_MessageQueue_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public void MessageQueueInfoCreate(int MessageQueueId, string Parameter, string Value)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vMessageQueueId", MessageQueueId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParameter", Parameter));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "B_MessageQueueInfo_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        #region Notifications

        public int NotificationUpsert(int? NotificationId, int CompanyId, string Label, string User, string Url, int NotificationType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vNotificationId", NotificationId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyId", CompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLabel", Label));
            lstParams.Add(DataInstance.CreateTypedParameter("vUrl", Url));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vNotificationType", NotificationType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "N_Notification_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<NotificationModel> NotificationGetByUser(int CompanyId, string User, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyId", CompanyId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "N_Notification_GetByUser",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            List<NotificationModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from n in response.DataTableResult.AsEnumerable()
                     where !n.IsNull("NotificationId")
                     group n by new
                     {
                         NotificationId = n.Field<int>("NotificationId"),
                         CompanyId = n.Field<int>("CompanyId"),
                         Label = n.Field<string>("Label"),
                         Url = n.Field<string>("Url"),
                         User = n.Field<string>("User"),
                         NotificationType = n.Field<int>("NotificationType"),
                         NotificationEnable = n.Field<UInt64>("NotificationEnable"),
                         LastModify = n.Field<DateTime>("LastModify"),
                         CreateDate = n.Field<DateTime>("CreateDate"),
                     }
                     into ng
                     select new NotificationModel()
                     {
                         NotificationId = ng.Key.NotificationId,
                         CompanyId = ng.Key.CompanyId,
                         Label = ng.Key.Label,
                         Url = ng.Key.Url,
                         User = ng.Key.User,
                         NotificationType = ng.Key.NotificationType,
                         Enable = ng.Key.NotificationEnable == 1 ? true : false,
                         LastModify = ng.Key.LastModify,
                         CreateDate = ng.Key.CreateDate,
                     }).ToList();
            }

            return oReturn;
        }

        #endregion
    }
}
