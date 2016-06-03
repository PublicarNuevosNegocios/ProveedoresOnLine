using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Models.CalificationProject
{
    public class CalificationProjectCategoryModel
    {
        public int TreeId { get; set; }

        public string TreeName { get; set; }

        public bool TreeEnable { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool CategoryEnable { get; set; }
    }
}
