using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderViewModel
    {
        public bool RenderScripts { get; set; }

        public bool IsForm { get; set; }

        public ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel RelatedProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Role.RoleCompanyModel RelatedRole { get; set; }

        public List<BackOffice.Models.General.GenericMenu> ProviderMenu { get; set; }

        public BackOffice.Models.General.GenericMenu CurrentSubMenu
        {
            get
            {
                if (ProviderMenu != null)
                {
                    return ProviderMenu.
                                Where(x => x.ChildMenu.Any(y => y.IsSelected && !string.IsNullOrEmpty(y.Url))).
                                Select(x => x.ChildMenu.
                                    Where(y => y.IsSelected && !string.IsNullOrEmpty(y.Url)).
                                    FirstOrDefault()).
                                FirstOrDefault();
                }
                return null;
            }
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> ProviderOptions { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericFilterModel> SearchFilter { get; set; }

        public string GridToSave { get; set; }
    }
}
