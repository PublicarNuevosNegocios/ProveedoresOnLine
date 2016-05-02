using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Models.Integration
{
    public class AditionalFieldModel
    {
        public int AditionalFieldId { get; set; }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel AditionalFieldType { get; set; }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel AditionalFieldTypeInfo { get; set; }

        public string Label { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
