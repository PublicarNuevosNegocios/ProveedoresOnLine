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
                    if (MessageModule.Interfaces.Models.MessageConfig.AgentConfig.ContainsKey(mts.Agent))
                    {
                        //get message instance
                        string strAssemblie = MessageModule.Interfaces.Models.MessageConfig.
                            AgentConfig
                            [mts.Agent]
                            [MessageModule.Interfaces.General.Constants.C_Agent_Assemblie];
                        MessageModule.Interfaces.IMessageAgent oConcretAgent = MessageFactory(strAssemblie);

                        //send message
                        mts = oConcretAgent.SendMessage(mts);
                    }

                    //register process message
                    mts.QueueProcessId =
                        MessageModule.DAL.Controller.MessageDataController.Instance.QueueProcessProcessMessage
                        (mts.MessageQueueId,
                        mts.IsSuccess,
                        mts.ProcessResult);
                    return true;
                });
            }
        }

        #region Message factory

        private static MessageModule.Interfaces.IMessageAgent MessageFactory(string Assemblie)
        {
            Type typetoreturn = Type.GetType(Assemblie);
            MessageModule.Interfaces.IMessageAgent oRetorno = (MessageModule.Interfaces.IMessageAgent)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }

        #endregion
    }
}
