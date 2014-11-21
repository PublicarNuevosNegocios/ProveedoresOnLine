using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Models.Customer
{
    public class ProjectConfigModel : ProveedoresOnLine.Company.Models.Util.GenericItemModel
    {
        public ProveedoresOnLine.Company.Models.Util.CatalogModel Status { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedEvaluation { get; set; }


    }
}
