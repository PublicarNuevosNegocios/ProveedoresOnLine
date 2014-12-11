using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.General
{
    public class EconomicActivityViewModel
    {
        public int EconomicActivityId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Group { get; set; }

        public string Category { get; set; }
    }
}
