using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Models.Provider
{
    public class ProviderCustomerInfo
    {
        public string ProviderPublicId { get; set; }

        public string CustomerPublicId { get; set; }

        public CatalogModel ProviderCustomerInfoType { get; set; }

        public string Value { get; set; }

        public string Largevalue { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
