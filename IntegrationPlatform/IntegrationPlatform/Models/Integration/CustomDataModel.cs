using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlatform.Models.Integration
{
    public class CustomDataModel
    {
        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CustomData { get; set; }

        public List<Models.Integration.CustomFieldModel> CustomField { get; set; }
    }
}
