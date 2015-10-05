using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Models
{
    public class TDQueryDetailInfoModel
    {
        public int QueryDetailInfoId { get; set; }

        public string QueryBasicPublicId { get; set; }
        
        public TDCatalogModel ItemInfoType { get; set; }

        public string Value { get; set; }

        public string LargeValue { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Enable { get; set; }
    }
}
