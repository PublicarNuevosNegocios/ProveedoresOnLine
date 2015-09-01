using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Models.Customer
{
    public class CustomerModel
    {
        public Company.Models.Company.CompanyModel RelatedCompany { get; set; }

        #region Customer Provider

        public List<CustomerProviderModel> RelatedProvider { get; set; }

        #endregion

        #region Aditional Documents

        public Company.Models.Util.GenericItemModel AditionalDocuments { get; set; }

        #endregion
    }
}
