using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Models
{
    public class PlanModel
    {
        public string PlanPublicId { get; set; }

        public string CompanyPublicId { get; set; }

        public int QueriesByPeriod { get; set; }
        
        public bool  IsLimited { get; set; }

        public DateTime InitDate { get; set; }

        public DateTime EndDate { get; set; }

        public TDCatalogModel Status { get; set; }

        public int DaysByPeriod { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<PeriodModel> RelatedPeriodModel { get; set; }
    }
}
