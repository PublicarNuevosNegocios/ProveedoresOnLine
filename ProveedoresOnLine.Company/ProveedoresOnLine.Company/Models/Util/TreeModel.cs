using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class TreeModel
    {
        public int TreeId { get; set; }

        public string TreeName { get; set; }

        public CatalogModel TreeType { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
