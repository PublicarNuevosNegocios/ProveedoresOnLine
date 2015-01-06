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

        public int TotalRows { get; set; }

        public string ImageUrl { get; set; }

        public ProviderSearchViewModel(ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany)
        {
            RelatedCompany = oRelatedCompany;
        }
    }
}
