using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompareModule.Models
{
    public class CompareCompanyModel : ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel
    {
        public int CompareCompanyId { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
