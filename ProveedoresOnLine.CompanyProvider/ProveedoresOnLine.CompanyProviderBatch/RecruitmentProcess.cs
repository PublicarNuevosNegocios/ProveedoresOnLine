using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProviderBatch
{
    public class RecruitmentProcess
    {
        public static void StartProcess()
        {
            ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BPGetRecruitmentProviders();
        }
    }
}
