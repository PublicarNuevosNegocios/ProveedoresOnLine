using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client
{
    public class ClientController
    {
        public static int CreateMessage(MessageModule.Client.Models.MessageQueueModel MessageToUpsert)
        {
            MessageToUpsert.MessageQueueId = DAL.Controller.ClientDataController.Instance.MessageQueueCreate
                (MessageToUpsert.Agent,
                MessageToUpsert.ProgramTime,
                MessageToUpsert.User);

            if (MessageToUpsert.MessageQueueInfo != null && MessageToUpsert.MessageQueueInfo.Count > 0)
            {
                MessageToUpsert.MessageQueueInfo.All(minf =>
                {
                    DAL.Controller.ClientDataController.Instance.MessageQueueInfoCreate
                        (MessageToUpsert.MessageQueueId,
                        minf.Parameter,
                        minf.Value);
                    return true;
                });
            }

            return MessageToUpsert.MessageQueueId;
        }
    }
}
