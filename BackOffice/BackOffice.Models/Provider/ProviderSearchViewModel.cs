using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderSearchViewModel
    {
        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; private set; }

        //public string ProviderPublicId

        public ProviderSearchViewModel(ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany)
        {
            RelatedCompany = oRelatedCompany;
        }
    }
}
