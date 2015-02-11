using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.Models
{
    public class MessageQueueInfoModel
    {
        public int MessageQueueInfoId { get; set; }

        public string Parameter { get; set; }

        public string Value { get; set; }
    }
}
