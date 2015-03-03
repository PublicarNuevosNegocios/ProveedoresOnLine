using MessageModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Agent.AWSMail
{
    public class AWSMail : IMessageAgent
    {
        public Interfaces.Models.MessageModel SendMessage
            (Interfaces.Models.MessageModel MessageToSend)
        {
            throw new NotImplementedException();
        }
    }
}
