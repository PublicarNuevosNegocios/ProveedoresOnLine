using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderUpsertModel
    {
        public DocumentManagement.Provider.Models.Provider.ProviderModel RelatedCustomer { get; set; }

        public List<DocumentManagement.Provider.Models.Util.CatalogModel> ProviderOptions { get; set; }
    }
}
