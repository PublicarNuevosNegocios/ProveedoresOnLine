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
    }
}
