using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.Interfaces
{
    internal interface IClientData
    {
        int MessageQueueCreate(string Agent, DateTime ProgramTime, string User);

        void MessageQueueInfoCreate(int MessageQueueId, string Parameter, string Value);
    }
}
