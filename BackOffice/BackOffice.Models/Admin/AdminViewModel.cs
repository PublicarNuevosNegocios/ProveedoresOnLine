using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminViewModel
    {
        public bool RenderScripts { get; set; }

        public bool IsForm { get; set; }

        public List<BackOffice.Models.General.GenericMenu> AdminMenu { get; set; }

        public BackOffice.Models.General.GenericMenu CurrentSubMenu
        {
            get
            {
                if (AdminMenu != null)
                {
                    return AdminMenu.
                                Where(x => x.ChildMenu.Any(y => y.IsSelected && !string.IsNullOrEmpty(y.Url))).
                                Select(x => x.ChildMenu.
                                    Where(y => y.IsSelected && !string.IsNullOrEmpty(y.Url)).
                                    FirstOrDefault()).
                                FirstOrDefault();
                }
                return null;
            }
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> AdminOptions { get; set; }        
    }
}
