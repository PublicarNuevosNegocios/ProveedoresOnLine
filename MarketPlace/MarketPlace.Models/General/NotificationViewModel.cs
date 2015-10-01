using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public class NotificationViewModel
    {
        private MessageModule.Client.Models.NotificationModel oNotificationModel { get; set; }

        private int oNotificationId;
        public int NotificationId
        {
            get
            {
                if (oNotificationId == 0)
                {
                    oNotificationId = oNotificationModel.NotificationId;
                }

                return oNotificationId;
            }
        }

        private string oCompanyPublicId;
        public string CompanyPublicId
        {
            get
            {
                if (string.IsNullOrEmpty(oCompanyPublicId))
                {
                    oCompanyPublicId = oNotificationModel.CompanyPublicId;
                }

                return oCompanyPublicId;
            }
        }

        private string oLabel;
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(oLabel))
                {
                    oLabel = oNotificationModel.Label;
                }

                return oLabel;
            }
        }

        private string oNotificationType;
        public string NotificationType
        {
            get
            {
                if (string.IsNullOrEmpty(oNotificationType))
                {
                    oNotificationType = oNotificationModel.NotificationType.ToString();
                }

                return oNotificationType;
            }
        }

        private string oUser;
        public string User
        {
            get
            {
                if (string.IsNullOrEmpty(oUser))
                {
                    oUser = oNotificationModel.User;
                }

                return oUser;
            }
        }

        private string oUrl;

        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(oUrl))
                {
                    oUrl = oNotificationModel.Url;
                }

                return oUrl;
            }
        }

        public NotificationViewModel(MessageModule.Client.Models.NotificationModel oNotification)
        {
            oNotificationModel = oNotification;
        }
    }
}
