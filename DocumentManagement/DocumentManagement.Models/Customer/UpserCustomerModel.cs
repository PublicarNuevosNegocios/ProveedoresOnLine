using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Customer
{
    public class UpserCustomerModel
    {
        public DocumentManagement.Customer.Models.Customer.CustomerModel RelatedCustomer { get; set; }

        public List<DocumentManagement.Customer.Models.Util.CatalogModel> CustomerOptions { get; set; }
    }
}
