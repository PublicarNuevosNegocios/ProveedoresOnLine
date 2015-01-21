using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderCustomerViewModel
    {
        public string ProviderCustomerId { get; set; }

        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; set; }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel RelatedStatus { get; set; }

        public bool Enable { get; set; }

        public ProviderCustomerViewModel() { }

        public ProviderCustomerViewModel(string oProviderCustomerId
                                        , ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany
                                        , ProveedoresOnLine.Company.Models.Util.CatalogModel oRelatedStatus
                                        , bool oEnable)
        {
            ProviderCustomerId = oProviderCustomerId;
            RelatedCompany = oRelatedCompany;
            RelatedStatus = oRelatedStatus;
            Enable = oEnable;
        }        
    }
}
