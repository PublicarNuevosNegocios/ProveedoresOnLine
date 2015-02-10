using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Interfaces
{
    public interface IMessageModuleData
    {
        int QueueProcessCreate();

        void QueueProcessInfoCreate();

        void MessageQueueDelete(int MessageQueueId);
    }
}
