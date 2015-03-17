using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.Models
{
    public class SurveyConfigModel : ProveedoresOnLine.Company.Models.Util.GenericItemModel
    {
        public ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel RelatedCustomer { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedSurveyConfigItem { get; set; }
    }
}
