using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class CustomerViewModel
    {
        public bool RenderScripts { get; set; }

        public bool IsForm { get; set; }

        public ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel RelatedCustomer { get; set; }

        public List<BackOffice.Models.General.GenericMenu> CustomerMenu { get; set; }

        public BackOffice.Models.General.GenericMenu CurrentSubMenu
        {
            get
            {
                if (CustomerMenu != null)
                {
                    return CustomerMenu.
                                Where(x => x.ChildMenu.Any(y => y.IsSelected && !string.IsNullOrEmpty(y.Url))).
                                Select(x => x.ChildMenu.
                                    Where(y => y.IsSelected && !string.IsNullOrEmpty(y.Url)).
                                    FirstOrDefault()).
                                FirstOrDefault();
                }
                return null;
            }
        }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> CustomerOptions { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.CatalogModel> ProjectConfigOptions { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.TreeModel> CustomActivityTree { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.TreeModel> SurveyGroup { get; set; }

        public List<BackOffice.Models.Customer.CustomerRoleViewModel> RelatedRoleCompanyList { get; set; }

        public SurveyConfigViewModel RelatedSurveyConfig { get; set; }

        public ProjectConfigViewModel RelatedProjectConfig { get; set; }

        public string GridToSave { get; set; }

        
    }
}
