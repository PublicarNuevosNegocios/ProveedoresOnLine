using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Company
{
    [ElasticsearchType(Name = "CustomerProvider_Info")]
    public class CustomerProviderIndexModel
    {
        public CustomerProviderIndexModel()
        {

        }

        public int Id { get { return CustomerProviderId; } }

        public int CustomerProviderId { get; set; }

        public string CustomerPublicId { get; set; }

        public string ProviderPublicId { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public bool CustomerProviderEnable { get; set; }
    }
}
