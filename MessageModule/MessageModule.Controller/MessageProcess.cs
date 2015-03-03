using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Controller
{
    public class MessageProcess
    {
        public static void StartProcess()
        {
            //get message to send
            List<MessageModule.Interfaces.Models.MessageModel> lstMessageToSend =
                MessageModule.DAL.Controller.MessageDataController.Instance.MessageQueueGetMessageToProcess();

            if (lstMessageToSend != null && lstMessageToSend.Count > 0)
            {
                lstMessageToSend.All(mts =>
                {

                    //get agent configuration
                    var config = MessageModule.Interfaces.Models.MessageConfig.AgentConfig[mts.Agent];


                    return true;
                });

            }
        }
    }
}
