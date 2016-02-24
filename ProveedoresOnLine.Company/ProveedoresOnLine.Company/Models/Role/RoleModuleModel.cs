using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Role
{
    public class RoleModuleModel
    {
        public int RoleModuleId { get; set; }

        public CatalogModel RoleModuleType { get; set; }

        public string RoleModule { get; set; }

        public List<GenericItemModel> ModuleOption { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
