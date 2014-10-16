using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagementClient.Manager.Models.Customer
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        public string CustomerPublicId { get; set; }

        public string Name { get; set; }

        public enumIdentificationType IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
