using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.Models
{
    public class CompanyIndexSearchModel
    {
        public string CompanyPublicId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyIdentificationTypeId { get; set; }

        public string CompanyIdentificationType { get; set; }

        public string CompanyIdentificationNumber { get; set; }

        public string CountryId { get; set; }

        public string Country { get; set; }

        public string CityId { get; set; }

        public string City { get; set; }

        public string StatusId { get; set; }

        public string Status { get; set; }

        public string EconomicActivityId { get; set; }

        public string EconomicActivity { get; set; }
    }
}
