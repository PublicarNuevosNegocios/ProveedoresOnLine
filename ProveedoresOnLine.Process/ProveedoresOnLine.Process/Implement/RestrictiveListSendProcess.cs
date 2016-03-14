using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Process.Implement
{
    public class RestrictiveListSendProcess
    {
        public void Execute(Quartz.IJobExecutionContext context)
        {
            ProveedoresOnLine.RestrictiveListProcessBatch.RestrictiveListSendProcess.StartProcess();
        }
    }
}
