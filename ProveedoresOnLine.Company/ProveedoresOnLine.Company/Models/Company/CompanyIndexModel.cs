using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Company
{
    public class CompanyIndexModel
    {
        public int IdentificatioTypeId { get; set; }
        public string IdentificationNumber { get; set; }

        public string CompanyName { get; set; }
        public string CommercialCompanyName { get; set; }
        public string CompanyPublicId { get; set; }

        public int PrincipalActivityId { get; set; }
        public string PrincipalActivity { get; set; }

        public int CountryId { get; set; }
        public string Country { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public string CustomerPublicId { get; set; }

        public bool InBlackList { get; set; }

        public int ProviderStatusId { get; set; }
        public string ProviderStatus { get; set; }

        //Survey
    }
}
