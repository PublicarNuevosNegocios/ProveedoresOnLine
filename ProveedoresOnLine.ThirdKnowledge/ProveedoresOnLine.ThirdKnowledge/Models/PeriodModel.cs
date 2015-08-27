using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Models
{
    public class PeriodModel
    {
        public string PeriodPublicId { get; set; }

        public string PlanPublicId { get; set; }

        public int AssignedQueries { get; set; }

        public DateTime InitDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalQueries { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
