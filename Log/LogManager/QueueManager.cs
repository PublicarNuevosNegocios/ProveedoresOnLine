using LogManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LogManager
{
    internal class QueueManager
    {
        #region Properties

        private static QueueManager oInstance;

        internal static QueueManager Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new QueueManager();
                return oInstance;
            }
        }

        private List<LogModel> QueueLog { get; set; }

        private Timer ProccessLogTimer { get; set; }

        private int LogTimer
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[Constants.C_LogTimer]);
            }
        }

        #endregion

        #region constructors

        QueueManager()
        {
            //init log queue
            QueueLog = new List<LogModel>();

            //init queue log timer
            InitTimer();
        }

        #endregion

        #region Queue methods

        internal void AddQueueLog(LogModel oLogToAdd)
        {
            QueueLog.Add(oLogToAdd);
        }

        #endregion

        #region Timer methods

        private void InitTimer()
        {
            if (ProccessLogTimer == null)
            {
                ProccessLogTimer = new Timer();
                ProccessLogTimer.Elapsed += ProccessLogTimer_Elapsed;
                ProccessLogTimer.Interval = LogTimer;
            }

            ProccessLogTimer.Enabled = true;
        }

        private void StopTimer()
        {
            ProccessLogTimer.Enabled = false;
        }

        private void ProccessLogTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StopTimer();

            QueueLog.Where(ql => ql.LogId == 0).All(ql =>
            {
                try
                {
                    ql.LogId = DAL.Controller.LogDataController.Instance.LogCreate
                        (ql.User,
                        ql.Application,
                        ql.Source,
                        ql.IsSuccess,
                        ql.Message);

                    if (ql.RelatedLogInfo != null)
                    {
                        ql.RelatedLogInfo.All(qli =>
                        {
                            DAL.Controller.LogDataController.Instance.LogInfoCreate
                                (ql.LogId,
                                qli.LogInfoType,
                                qli.Value);

                            return true;
                        });
                    }

                    if (ql.LogInfoObject != null)
                    {
                        ql.LogInfoObject.All(qlo =>
                        {
                            DAL.Controller.LogDataController.Instance.LogInfoCreate
                                (ql.LogId,
                                qlo.LogInfoType,
                                qlo.Value);

                            return true;
                        });
                    }
                }
                catch { }

                return true;
            });

            QueueLog = QueueLog.Where(lg => lg.LogId == 0).Select(lg => lg).ToList();

            InitTimer();
        }

        #endregion
    }
}
