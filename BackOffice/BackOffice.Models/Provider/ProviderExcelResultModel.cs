using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderExcelResultModel
    {
        public ProviderExcelModel PrvModel { get; set; }

        public bool Success { get; set; }

        public string Error { get; set; }
    }
}
