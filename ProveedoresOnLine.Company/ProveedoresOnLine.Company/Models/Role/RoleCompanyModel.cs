using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Role
{
    public class RoleCompanyModel
    {
        public CompanyModel RelatedCompany { get; set; }

        public int RoleCompanyId { get; set; }

        public int CompanyId { get; set; }

        public string RoleCompanyName { get; set; }

        public int? ParentRoleCompany { get; set; }

        public List<RoleModuleModel> RoleModule { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
