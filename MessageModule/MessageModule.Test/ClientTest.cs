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
                    Agent = "POL_RememberPassword_Mail",
                    User = "usario@sistema.com",
                    ProgramTime = DateTime.Now,

                    MessageQueueInfo = new System.Collections.Generic.List<Tuple<string,string>>()
                    {
                        new Tuple<string,string>("To","Jairo Andres Guzman Duran<jairo.guzman@publicar.com>"),
                        new Tuple<string,string>("To","Jairo Andres Guzman Duran<jairo.guzman@publicar.com>"),
                        new Tuple<string,string>("To","Jairo Andres Guzman Duran<jairo.guzman@publicar.com>"),
                        new Tuple<string,string>("param1","value 1"),
                        new Tuple<string,string>("param2","value 2"),
                        new Tuple<string,string>("param3","value 3"),
                        new Tuple<string,string>("param4","value 4"),
                    },
                });

            Assert.AreEqual(true, oMessageId > 0);
        }
    }
}
