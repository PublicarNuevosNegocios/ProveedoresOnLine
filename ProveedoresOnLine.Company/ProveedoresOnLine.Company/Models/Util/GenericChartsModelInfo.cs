using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class GenericChartsModelInfo
    {
        public string ChartModuleInfoType { get; set; }

        public string ChartModuleType { get; set; }

        public string Title { get; set; }

        public string ItemType { get; set; }
        
        public string ItemName { get; set; }

        public int Count { get; set; }

        public string AxisX { get; set; }

        public int CountX { get; set; }

        public string AxisY { get; set; }

        public int CountY { get; set; }

        public DateTime Date { get; set; }
    }
}

