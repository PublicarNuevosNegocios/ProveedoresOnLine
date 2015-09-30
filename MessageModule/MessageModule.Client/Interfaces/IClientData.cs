using MessageModule.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.Interfaces
{
    internal interface IClientData
    {
        int MessageQueueCreate(string Agent, DateTime ProgramTime, string User);

        void MessageQueueInfoCreate(int MessageQueueId, string Parameter, string Value);

        #region Notifications

        int NotificationUpsert(int? NotificationId, string CompanyPublicId, string Label, string User, string Url, int NotificationType, bool Enable);

        List<NotificationModel> NotificationGetByUser(string CompanyPublicId, string User, bool Enable);

        #endregion
    }
}
