using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class GenericFilterModel
    {
        public GenericItemModel FilterType { get; set; }

        public GenericItemModel FilterValue { get; set; }

        public int Quantity { get; set; }
    }
}
