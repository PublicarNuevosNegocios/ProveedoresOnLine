using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Customer
{
    public class FieldSearchModel
    {
        public List<DocumentManagement.Customer.Models.Form.FieldModel> RelatedField { get; set; }

        public List<DocumentManagement.Customer.Models.Util.CatalogModel> ProviderInfoType { get; set; }
    }
}
