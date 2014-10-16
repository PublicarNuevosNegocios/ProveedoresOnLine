using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogManager.Models
{
    public class LogModel
    {
        public int LogId { get; set; }

        public string User { get; set; }

        public string Application { get; set; }

        public string Source { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<LogInfoModel> RelatedLogInfo { get; set; }

        public object LogObject { get; set; }

        public List<LogInfoModel> LogInfoObject
        {
            get
            {
                List<LogInfoModel> oReturn = new List<LogInfoModel>();

                if (LogObject != null)
                {
                    try
                    {
                        if (LogObject.GetType().UnderlyingSystemType.FullName.IndexOf("System") == -1)
                        {
                            LogObject.GetType().GetProperties().All(prop =>
                            {
                                object oVal = prop.GetValue(LogObject, null);

                                if (oVal != null)
                                {
                                    oReturn.Add(new LogInfoModel()
                                    {
                                        LogInfoType = prop.Name,
                                        Value = oVal.ToString(),
                                    });
                                }
                                return true;
                            });
                        }
                        else
                        {
                            oReturn.Add(new LogInfoModel()
                            {
                                LogInfoType = LogObject.GetType().Name,
                                Value = LogObject.ToString(),
                            });
                        }
                    }
                    catch { }
                }
                return oReturn;
            }
        }
    }
}
