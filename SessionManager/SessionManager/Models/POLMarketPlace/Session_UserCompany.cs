using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.POLMarketPlace
{
    public class Session_UserCompany
    {
        public int UserCompanyId { get; set; }

        public string User { get; set; }

        public Session_GenericItemModel RelatedRole { get; set; }

        public Session_RoleCompanyModel RelatedCompanyRole { get; set; }
    }
}
