using MessageModule.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.Controller
{
    public class ClientController
    {
        public static int CreateMessage(MessageModule.Client.Models.ClientMessageModel MessageToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {
                oLog.User = MessageToUpsert.User;

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
                            minf.Item1,
                            minf.Item2);
                        return true;
                    });
                }

                return MessageToUpsert.MessageQueueId;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = MessageToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static LogManager.Models.LogModel GetGenericLogModel()
        {
            LogManager.Models.LogModel oReturn = new LogManager.Models.LogModel()
            {
                RelatedLogInfo = new List<LogManager.Models.LogInfoModel>(),
            };

            try
            {
                System.Diagnostics.StackTrace oStack = new System.Diagnostics.StackTrace();

                if (oStack.FrameCount > 0)
                {
                    oReturn.Source = oStack.GetFrame(oStack.FrameCount - 1).GetMethod().Module.Assembly.Location;
                }

                //get appname
                if (oStack.FrameCount > 1)
                {
                    oReturn.Application = oStack.GetFrame(1).GetMethod().Module.Name + " - " + oStack.GetFrame(1).GetMethod().Name;
                }
                else
                {
                    oReturn.Application = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                }
            }
            catch { }

            return oReturn;
        }

        #region Notifications

        public static int NotificationUpsert(int? NotificationId, string CompanyPublicId, string Label, string User, string Url, int NotificationType, bool Enable)
        {
            return DAL.Controller.ClientDataController.Instance.NotificationUpsert(NotificationId, CompanyPublicId, Label, User, Url, NotificationType, Enable);
        }

        public static List<NotificationModel> NotificationGetByUser(string CompanyPublicId, string User, bool Enable)
        {
            return DAL.Controller.ClientDataController.Instance.NotificationGetByUser(CompanyPublicId, User, Enable);
        }

        #endregion
    }
}
