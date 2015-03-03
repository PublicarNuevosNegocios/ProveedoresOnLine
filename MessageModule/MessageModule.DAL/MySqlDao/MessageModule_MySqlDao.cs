using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.DAL.MySqlDao
{
    internal class MessageModule_MySqlDao : MessageModule.Interfaces.IMessageModuleData
    {
        private ADO.Interfaces.IADO DataInstance;

        public MessageModule_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(MessageModule.Interfaces.General.Constants.C_MS_MessageConnection);
        }

        public List<Interfaces.Models.MessageModel> MessageQueueGetMessageToProcess()
        {
            throw new NotImplementedException();
        }

        public int QueueProcessProcessMessage(int MessageQueueId, bool IsSuccess, string ProcessResult)
        {
            throw new NotImplementedException();
        }
    }
}
