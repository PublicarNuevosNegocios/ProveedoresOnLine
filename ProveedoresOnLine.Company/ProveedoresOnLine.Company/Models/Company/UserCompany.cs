using ProveedoresOnLine.Company.Models.Role;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Company
{
    public class UserCompany
    {
        public int UserCompanyId { get; set; }

        public string User { get; set; }

        public GenericItemModel RelatedRole { get; set; }

        public RoleCompanyModel RelatedCompanyRole { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
