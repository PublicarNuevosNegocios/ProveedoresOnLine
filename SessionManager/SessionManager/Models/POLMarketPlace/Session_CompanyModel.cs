using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.POLMarketPlace
{
    public class Session_CompanyModel
    {
        public string CompanyPublicId { get; set; }

        public string CompanyName { get; set; }

        public Session_CatalogModel IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public Session_CatalogModel CompanyType { get; set; }

        public List<Session_UserCompany> RelatedUser { get; set; }

        public bool CurrentSessionCompany { get; set; }
    }
}
