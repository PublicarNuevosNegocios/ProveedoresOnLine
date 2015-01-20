using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogManager
{
    public class ClientLog
    {
        public static void AddLog(LogManager.Models.LogModel LogToAdd)
        {
            QueueManager.Instance.AddQueueLog(LogToAdd);
        }

        public static List<LogManager.Models.LogModel> LogSearch(string LogInfoType, string Value)
        {
            return DAL.Controller.LogDataController.Instance.LogSearch(LogInfoType, Value);
        }

        //static on line methods
        public static void FileUploadCreate(List<string> LogFiles)
        {
            if (LogFiles != null && LogFiles.Count > 0)
            {
                LogFiles.All(lf =>
                {
                    try
                    {
                        DAL.Controller.LogDataController.Instance.FileUploadCreate(lf);
                    }
                    catch { }
                    return true;
                });
            }

        }

        public static void FileUsedCreate(List<string> LogFiles)
        {
            if (LogFiles != null && LogFiles.Count > 0)
            {
                LogFiles.All(lf =>
                {
                    try
                    {
                        DAL.Controller.LogDataController.Instance.FileUsedCreate(lf);
                    }
                    catch { }
                    return true;
                });
            }

        }
    }
}
