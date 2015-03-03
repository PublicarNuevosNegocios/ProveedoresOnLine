using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Interfaces
{
    public interface IMessageAgent
    {
        MessageModule.Interfaces.Models.MessageModel SendMessage(MessageModule.Interfaces.Models.MessageModel MessageToSend);
    }
}
