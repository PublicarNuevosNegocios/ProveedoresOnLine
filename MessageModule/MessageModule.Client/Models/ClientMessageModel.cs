using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.Models
{
    public class ClientMessageModel
    {
        public int MessageQueueId { get; set; }

        public string Agent { get; set; }

        public DateTime ProgramTime { get; set; }

        public string User { get; set; }

        /// <summary>
        /// <param,value>
        /// </summary>
        public List<Tuple<string, string>> MessageQueueInfo { get; set; }
    }
}
