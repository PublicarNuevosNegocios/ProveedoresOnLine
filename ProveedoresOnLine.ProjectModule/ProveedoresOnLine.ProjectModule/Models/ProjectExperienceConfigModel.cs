using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.Models
{
    public class ProjectExperienceConfigModel
    {
        public bool AmmounEnable { get; set; }

        public bool DefaultAcitvityEnable { get; set; }

        public bool CustomAcitvityEnable { get; set; }

        public bool CurrencyEnable { get; set; }

        public List<string> YearsInterval { get; set; }

        public List<string> QuantityInterval { get; set; }
    }
}
