using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Models.Provider
{
    public class ProviderModel
    {
        public string CustomerPublicId { get; set; }

        public string CustomerName { get; set; }

        public string FormPublicId { get; set; }

        public string FormName { get; set; }

        public List<ProviderInfoModel> RelatedProviderCustomerInfo { get; set; }

        public List<ProviderInfoModel> RelatedProviderInfo { get; set; }

        public string ProviderPublicId { get; set; }

        public string Name { get; set; }

        public CatalogModel IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public string Email { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public Int64 CustomerCount { get; set; }

        public string LogCreateDate { get; set; }

        public string LogUser { get; set; }
    }
}
