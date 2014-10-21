using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderFormModel
    {
        public DocumentManagement.Customer.Models.Customer.CustomerModel RealtedCustomer { get; set; }

        public DocumentManagement.Customer.Models.Form.FormModel RealtedForm { get; set; }

        public DocumentManagement.Customer.Models.Form.StepModel RealtedStep { get; set; }

        public DocumentManagement.Provider.Models.Provider.ProviderModel RealtedProvider { get; set; }

        public Dictionary<int, List<DocumentManagement.Provider.Models.Util.CatalogModel>> ProviderOptions { get; set; }
    }
}
