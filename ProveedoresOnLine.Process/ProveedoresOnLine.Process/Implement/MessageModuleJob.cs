using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Process.Implement
{
    public class MessageModuleJob : Quartz.IJob
    {
        public void Execute(Quartz.IJobExecutionContext context)
        {
            MessageModule.Controller.MessageProcess.StartProcess();
        }
    }
}
