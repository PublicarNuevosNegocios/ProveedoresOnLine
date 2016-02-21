using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListVerificator.Models
{
    public class ProviderStatusModel
    {
        public CompanyModel RelatedProvider { get; set; }

        public CatalogModel RelatedStatus { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }
        
        public string Representant { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }
    }
}
