using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Customer
{
    public class ProviderSearchModel
    {
        public int TotalRows { get; set; }

        public List<DocumentManagement.Provider.Models.Provider.ProviderModel> RelatedProvider { get; set; }
    }
}
