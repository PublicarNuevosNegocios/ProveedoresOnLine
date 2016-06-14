using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.Models
{
    public class SanofiProcessLogModel
    {
        public int SanofiProcessLogId { get; set; }

        public string ProviderPublicId { get; set; }

        public string ProcessName { get; set; }

        public bool IsSucces { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModify { get; set; }

        public bool Enable { get; set; }
    }
}
