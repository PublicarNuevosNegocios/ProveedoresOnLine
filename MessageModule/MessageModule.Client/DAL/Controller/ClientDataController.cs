using MessageModule.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.DAL.Controller
{
    internal class ClientDataController : MessageModule.Client.Interfaces.IClientData
    {
        #region singleton instance

        private static MessageModule.Client.Interfaces.IClientData oInstance;
        internal static MessageModule.Client.Interfaces.IClientData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ClientDataController();
                return oInstance;
            }
        }

        private MessageModule.Client.Interfaces.IClientData DataFactory;

        #endregion

        #region Constructor

        public ClientDataController()
        {
            ClientDataFactory factory = new ClientDataFactory();
            DataFactory = factory.GetClientInstance();
        }

        #endregion

        public int MessageQueueCreate(string Agent, DateTime ProgramTime, string User)
        {
            return DataFactory.MessageQueueCreate(Agent, ProgramTime, User);
        }

        public void MessageQueueInfoCreate(int MessageQueueId, string Parameter, string Value)
        {
            DataFactory.MessageQueueInfoCreate(MessageQueueId, Parameter, Value);
        }

        #region Notifications

        public int NotificationUpsert(int? NotificationId, int CompanyId, string Label, string User, string Url, int NotificationType, bool Enable)
        {
            return DataFactory.NotificationUpsert(NotificationId, CompanyId, Label, User, Url, NotificationType, Enable);
        }

        public List<NotificationModel> NotificationGetByUser(int CompanyId, string User, bool Enable)
        {
            return DataFactory.NotificationGetByUser(CompanyId, User, Enable);
        }

        #endregion
    }
}
