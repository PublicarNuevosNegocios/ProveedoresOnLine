using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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
    }
}
