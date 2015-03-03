using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.DAL.Controller
{
    public class MessageDataController : MessageModule.Interfaces.IMessageModuleData
    {
        #region singleton instance

        private static MessageModule.Interfaces.IMessageModuleData oInstance;
        public static MessageModule.Interfaces.IMessageModuleData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new MessageDataController();
                return oInstance;
            }
        }

        private MessageModule.Interfaces.IMessageModuleData DataFactory;

        #endregion

        #region Constructor

        public MessageDataController()
        {
            MessageDataFactory factory = new MessageDataFactory();
            DataFactory = factory.GetMessageInstance();
        }

        #endregion

        public List<Interfaces.Models.MessageModel> MessageQueueGetMessageToProcess()
        {
            return DataFactory.MessageQueueGetMessageToProcess();
        }

        public int QueueProcessProcessMessage(int MessageQueueId, bool IsSuccess, string ProcessResult)
        {
            return DataFactory.QueueProcessProcessMessage(MessageQueueId, IsSuccess, ProcessResult);
        }
    }
}
