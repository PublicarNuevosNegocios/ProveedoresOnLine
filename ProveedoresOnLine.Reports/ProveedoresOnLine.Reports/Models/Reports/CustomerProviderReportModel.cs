using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.Models.Reports
{
    public class CustomerProviderReportModel
    {
        public int ProviderId { get; set; }

        public string ProviderPublicId { get; set; }

        public string ProviderIdentificationType { get; set; }

        public string ProviderIdentificationNumber { get; set; }

        public string ProviderName { get; set; }

        public string ProviderStatus { get; set; }

        public int CustomerId { get; set; }

        public string CustomerPublicId { get; set; }

        public string CustomerName { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Representant { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }
    }
}
