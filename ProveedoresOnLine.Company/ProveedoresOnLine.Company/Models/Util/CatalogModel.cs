using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class CatalogModel
    {
        public int CatalogId { get; set; }

        public string CatalogName { get; set; }

        public bool CatalogEnable { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public bool ItemEnable { get; set; }
    }
}

