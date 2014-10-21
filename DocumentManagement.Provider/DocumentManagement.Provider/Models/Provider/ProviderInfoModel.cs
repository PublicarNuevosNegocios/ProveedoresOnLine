using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Models.Provider
{
    public class ProviderInfoModel
    {
        public int ProviderInfoId { get; set; }

        public CatalogModel ProviderInfoType { get; set; }

        public string Value { get; set; }

        public string LargeValue { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
