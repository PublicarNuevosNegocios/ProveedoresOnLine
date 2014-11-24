using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Models.Customer
{
    public class SurveyConfigModel : ProveedoresOnLine.Company.Models.Util.GenericItemModel
    {
        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedSurveyItem { get; set; }
    }
}
