﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProveedoresOnLine.Company.Models.Company
{
    [ElasticsearchType(Name = "Customer_Info")]
    public class CompanyIndexModel
    {
        public CompanyIndexModel()
        {

        }
        public string Id { get { return CompanyPublicId; } }
        [Number]
        public int IdentificationTypeId { get; set; }
        public string IdentificationType { get; set; }

        [String(Analyzer = "customWhiteSpace", SearchAnalyzer = "customWhiteSpace")]
        public string IdentificationNumber { get; set; }

        [String(Analyzer = "customWhiteSpace", SearchAnalyzer = "customWhiteSpace")]
        public string CompanyName { get; set; }

        [String(Analyzer = "customWhiteSpace", SearchAnalyzer = "customWhiteSpace")]
        public string CommercialCompanyName { get; set; }

        public string CompanyPublicId { get; set; }

        public string LogoUrl { get; set; }

        public string CatlificationRating { get; set; }

        [Number]
        public int PrincipalActivityId { get; set; }
        [String]
        public string PrincipalActivity { get; set; }

        [Number]
        public int CountryId { get; set; }

        [String(Analyzer = "customWhiteSpace", SearchAnalyzer = "customWhiteSpace")]
        public string Country { get; set; }

        [Number]
        public int CityId { get; set; }
        [String(Analyzer = "customWhiteSpace", SearchAnalyzer = "customWhiteSpace")]
        public string City { get; set; }

        [String]
        public string CustomerPublicId { get; set; }

        [Boolean]
        public bool InBlackList { get; set; }

        public int ProviderStatusId { get; set; }
        public string ProviderStatus { get; set; }

        public bool CompanyEnable { get; set; }

        [Number]
        public int ICAId { get; set; }
        [String]
        public string ICA { get; set; }
    }
}
