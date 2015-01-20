using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class GeographyModel
    {
        public GenericItemModel Country { get; set; }

        public GenericItemModel State { get; set; }

        public GenericItemModel City { get; set; }
    }
}
