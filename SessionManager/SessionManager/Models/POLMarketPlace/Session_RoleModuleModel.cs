using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.POLMarketPlace
{
    public class Session_RoleModuleModel
    {
        public int RoleModuleId { get; set; }

        public Session_CatalogModel RoleModuleType { get; set; }

        public string RoleModule { get; set; }

        public List<Session_GenericItemModel> ModuleOption { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
