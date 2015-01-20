using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Models.Customer
{
    public class CustomerProviderModel
    {
        public int CustomerProviderId { get; set; }

        public Company.Models.Company.CompanyModel RelatedProvider { get; set; }

        public Company.Models.Util.CatalogModel Status { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<Company.Models.Util.GenericItemInfoModel> CustomerProviderInfo { get; set; }

    }
}
