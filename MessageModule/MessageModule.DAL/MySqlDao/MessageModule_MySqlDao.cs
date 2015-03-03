using MessageModule.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MessageModule.DAL.MySqlDao
{
    internal class MessageModule_MySqlDao : MessageModule.Interfaces.IMessageModuleData
    {
        private ADO.Interfaces.IADO DataInstance;

        public MessageModule_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(MessageModule.Interfaces.General.Constants.C_MS_MessageConnection);
        }

        public List<MessageModel> MessageQueueGetMessageToProcess()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "P_MessageQueue_GetMessageToProcess",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<MessageModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ms in response.DataTableResult.AsEnumerable()
                     where !ms.IsNull("MessageQueueId")
                     group ms by
                     new
                     {
                         MessageQueueId = ms.Field<int>("MessageQueueId"),
                         Agent = ms.Field<string>("Agent"),
                         ProgramTime = ms.Field<DateTime>("ProgramTime"),
                         User = ms.Field<string>("User"),
                     } into msg
                     select new MessageModel()
                     {
                         MessageQueueId = msg.Key.MessageQueueId,
                         Agent = msg.Key.Agent,
                         ProgramTime = msg.Key.ProgramTime,
                         User = msg.Key.User,

                         QueueProcessInfo =
                            (from msinf in response.DataTableResult.AsEnumerable()
                             where !msinf.IsNull("MessageQueueInfoId") &&
                                 msinf.Field<int>("MessageQueueId") == msg.Key.MessageQueueId
                             select new Tuple<int, string, string>(
                                 msinf.Field<int>("MessageQueueInfoId"),
                                 msinf.Field<string>("Parameter"),
                                 msinf.Field<string>("Value"))).
                            ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public int QueueProcessProcessMessage(int MessageQueueId, bool IsSuccess, string ProcessResult)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vMessageQueueId", MessageQueueId));
            lstParams.Add(DataInstance.CreateTypedParameter("vIsSuccess", IsSuccess));
            lstParams.Add(DataInstance.CreateTypedParameter("vProcessResult", ProcessResult));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "P_QueueProcess_ProcessMessage",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }
    }
}
