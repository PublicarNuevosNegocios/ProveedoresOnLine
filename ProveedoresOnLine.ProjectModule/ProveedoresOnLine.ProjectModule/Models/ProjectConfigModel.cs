using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.Models
{
    public class ProjectConfigModel : ProveedoresOnLine.Company.Models.Util.GenericItemModel
    {
        public ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel RelatedCustomer { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedEvaluationItem { get; set; }

        public bool HasProjects { get; set; }
    }
}
