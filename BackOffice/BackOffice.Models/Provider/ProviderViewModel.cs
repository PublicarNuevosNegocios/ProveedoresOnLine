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

        public ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel RelatedProvider { get; set; }

        public List<BackOffice.Models.General.GenericMenu> ProviderMenu { get; set; }
    }
}
