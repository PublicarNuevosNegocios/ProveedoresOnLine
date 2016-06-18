using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Process.Implement
{
    public class SANOFIProcess : Quartz.IJob
    {
        public void Execute(Quartz.IJobExecutionContext context)
        {
            IntegrationPlattaform.SANOFIProcess.Controller.IntegrationPlatformSANOFIIProcess.StartProcess();
        }
    }
}
