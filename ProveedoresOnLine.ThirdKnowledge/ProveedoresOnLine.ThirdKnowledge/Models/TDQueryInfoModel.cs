using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Models
{
    public class TDQueryInfoModel
    {
        public int QueryInfoId { get; set; }

        public string QueryPublicId { get; set; }

        public TDCatalogModel ItemInfoType { get; set; }

        public string Value { get; set; }

        public string LargeValue { get; set; }

        public bool Enable { get; set; }
    }
}
