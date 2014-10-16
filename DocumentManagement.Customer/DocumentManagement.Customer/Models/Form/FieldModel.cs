using DocumentManagement.Customer.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.Models.Form
{
    public class FieldModel
    {
        public int FieldId { get; set; }

        public string Name { get; set; }

        public CatalogModel ProviderInfoType { get; set; }

        public CatalogModel FieldType { get; set; }

        public bool IsRequired { get; set; }

        public int Position { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
