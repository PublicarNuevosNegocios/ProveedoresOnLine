using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageModule.Client.Models;
using System.Collections.Generic;

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

        [TestMethod]
        public void NotificationUpsert()
        {
            NotificationModel oReturn = new NotificationModel()
            {
                CompanyPublicId = "DA5C572E",
                Label = "pruebas",
                User = "david.moncayo@publicar.com",
                Url = "www.google.com.co",
                NotificationType = 1801001,
                Enable = true,
            };

            oReturn.NotificationId = MessageModule.Client.Controller.ClientController.NotificationUpsert(oReturn);

            Assert.AreEqual(true, oReturn != null && oReturn.NotificationId > 0);
        }

        [TestMethod]
        public void NotificationGetByUser()
        {
            List<NotificationModel> oReturn = MessageModule.Client.Controller.ClientController.NotificationGetByUser(
                "DA5C572E",
                "david.moncayo@publicar.com",
                true);

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }
    }
}
