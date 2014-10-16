using DocumentManagement.Customer.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.Models.Customer
{
    public class CustomerModel
    {
        public string CustomerPublicId { get; set; }

        public string Name { get; set; }

        public CatalogModel IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
