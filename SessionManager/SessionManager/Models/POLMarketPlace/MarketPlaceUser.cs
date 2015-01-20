using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.POLMarketPlace
{
    public class MarketPlaceUser
    {
        public SessionManager.Models.Auth.User RelatedUser { get; set; }

        public List<Session_CompanyModel> RelatedCompany { get; set; }
    }
}
