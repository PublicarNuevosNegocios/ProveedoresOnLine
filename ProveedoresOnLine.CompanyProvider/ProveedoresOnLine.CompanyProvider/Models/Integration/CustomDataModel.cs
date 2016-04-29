using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Models.Integration
{
    public class CustomDataModel
    {
        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCustomer { get; set; }

        public List<AditionalFieldModel> AditionalField { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> AditionalData { get; set; }
    }
}
