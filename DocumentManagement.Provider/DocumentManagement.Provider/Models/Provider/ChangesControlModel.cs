using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Models.Provider
{
    public class ChangesControlModel : ProviderModel
    {
        public int ChangesControlId { get; set; }

        public string ChangesPublicId { get; set; }

        public int ProviderInfoId { get; set; }

        public string FormUrl { get; set; }

        public int StepId { get; set; }

        public CatalogModel Status { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
