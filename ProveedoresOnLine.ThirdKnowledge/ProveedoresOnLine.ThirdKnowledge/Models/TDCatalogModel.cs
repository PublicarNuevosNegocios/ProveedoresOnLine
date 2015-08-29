using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Models
{
    public class TDCatalogModel
    {
        public bool CatalogEnable { get; set; }
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }
        public bool ItemEnable { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
