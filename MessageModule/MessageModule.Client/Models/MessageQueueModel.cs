using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.Models
{
    public class MessageQueueModel
    {
        public int MessageQueueId { get; set; }

        public string Agent { get; set; }

        public DateTime ProgramTime { get; set; }

        public string User { get; set; }

        public List<MessageQueueInfoModel> MessageQueueInfo { get; set; }
    }
}
