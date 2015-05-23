using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class ChartsModel
    {
        public string Title { get; set; }

        public int ItemType { get; set; }
        
        public string ItemName { get; set; }

        public int Count { get; set; }

        public string AxisX { get; set; }

        public string AxisY { get; set; }
    }
}

