using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Models.Util
{
    public class BlackListProcessModel
    {
        public int BlackListProcessId { get; set; }
        public string FilePath { get; set; }
        public bool ProcessStatus { get; set; }
        public bool IsSuccess { get; set; }
        public string ProviderStatus { get; set; }
        public bool Enable { get; set; }
        public string LastModify { get; set; }
        public string CreateDate { get; set; }
    }
}
