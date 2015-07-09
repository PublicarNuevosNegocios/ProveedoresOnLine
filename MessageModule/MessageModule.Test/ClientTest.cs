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
            int oMessageId = MessageModule.Client.Controller.ClientController.CreateMessage
                (new Client.Models.ClientMessageModel()
                {
                    Agent = "POL_SurveyWriteBackNotification_Mail",
                    User = "usario@sistema.com",
                    ProgramTime = DateTime.Now,

                    MessageQueueInfo = new System.Collections.Generic.List<Tuple<string,string>>()
                    {
                        new Tuple<string,string>("To","Jose Forero<josexandthecity@gmail.com>"),
                        new Tuple<string,string>("RememberUrl","https://www.proveedoresonline.co/marketplace/home/TermsAndConditios"),
                    },
                });

            Assert.AreEqual(true, oMessageId > 0);
        }
    }
}
