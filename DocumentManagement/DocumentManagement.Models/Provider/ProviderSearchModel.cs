using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderSearchModel
    {
        public int TotalRows { get; set; }

        public List<ProviderItemSearchModel> RelatedProvider { get; set; }

        public List<DocumentManagement.Customer.Models.Customer.CustomerModel> Customers { get; set; }

        public List<DocumentManagement.Customer.Models.Form.FormModel> Forms { get; set; }
    }
}
