using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Customer
{
    public class CustomerSearchModel
    {
        public int TotalRows { get; set; }

        public List<DocumentManagement.Customer.Models.Customer.CustomerModel> RelatedCustomer { get; set; }
    }
}
