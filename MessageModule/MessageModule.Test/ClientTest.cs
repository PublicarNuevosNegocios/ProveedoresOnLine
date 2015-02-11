using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageModule.Test
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void CreateMessage()
        {
            int oMessageId = MessageModule.Client.ClientController.CreateMessage
                (new Client.Models.MessageQueueModel()
                {
                    Agent = "agentecorreo",
                    User = "usario@sistema.com",
                    ProgramTime = DateTime.Now,

                    MessageQueueInfo = new System.Collections.Generic.List<Client.Models.MessageQueueInfoModel>() 
                    { 
                        new Client.Models.MessageQueueInfoModel()
                        {
                            Parameter = "param 1",
                            Value = "valor 1",
                        },
                        new Client.Models.MessageQueueInfoModel()
                        {
                            Parameter = "param 2",
                            Value = "valor 2",
                        },
                    }
                });

            Assert.AreEqual(true, oMessageId > 0);
        }
    }
}
