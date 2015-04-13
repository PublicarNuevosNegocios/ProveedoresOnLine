using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Models.Customer
{
    public class CustomerModel
    {
        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; set; }

        #region Customer Provider

        public List<CustomerProviderModel> RelatedProvider { get; set; }

        #endregion
    }
}
