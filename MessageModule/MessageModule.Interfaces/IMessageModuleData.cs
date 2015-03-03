using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Interfaces
{
    public interface IMessageModuleData
    {
        List<MessageModule.Interfaces.Models.MessageModel> MessageQueueGetMessageToProcess();

        int QueueProcessProcessMessage(int MessageQueueId, bool IsSuccess, string ProcessResult);
    }
}
