using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ExcelProviderResultModel : ExcelProviderModel
    {
        public ExcelProviderModel PrvModel { get; set; }

        public bool Success { get; set; }

        public string Error { get; set; }
    }
}
