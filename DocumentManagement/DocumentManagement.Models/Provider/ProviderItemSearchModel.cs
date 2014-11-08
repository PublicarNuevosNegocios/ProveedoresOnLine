using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderItemSearchModel
    {
        public DocumentManagement.Provider.Models.Provider.ProviderModel RelatedProvider { get; set; }

        public string codSalesforce { get; set; }

        public int CustomerInfoTypeId { get; set; }

        public int oTotalRows { get; set; }
    }
}
