using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Interfaces.Models
{
    public class MessageModel
    {
        public int QueueProcessId { get; set; }

        public int MessageQueueId { get; set; }

        public string Agent { get; set; }

        public DateTime ProgramTime { get; set; }

        public string User { get; set; }

        public bool IsSuccess { get; set; }

        public string ProcessResult { get; set; }

        /// <summary>
        /// <MessageQueueInfoId,param,value>
        /// </summary>
        public List<Tuple<int, string, string>> QueueProcessInfo { get; set; }
    }
}
