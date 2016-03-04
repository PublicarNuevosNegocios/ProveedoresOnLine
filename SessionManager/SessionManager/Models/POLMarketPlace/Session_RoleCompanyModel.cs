using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.POLMarketPlace
{
    public class Session_RoleCompanyModel
    {
        public Session_CompanyModel RelatedCompany { get; set; }

        public int RoleCompanyId { get; set; }

        public int CompanyId { get; set; }

        public string RoleCompanyName { get; set; }

        public int? ParentRoleCompany { get; set; }

        public List<Session_RoleModuleModel> RoleModule { get; set; }

        public List<Session_GenericItemModel> RelatedReport { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
