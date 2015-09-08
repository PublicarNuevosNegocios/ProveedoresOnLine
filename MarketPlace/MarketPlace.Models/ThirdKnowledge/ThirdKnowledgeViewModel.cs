using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Company
{
    public class ThirdKnowledgeViewModel
    {
        public bool RenderScripts { get; set; }

        public List<ProveedoresOnLine.ThirdKnowledge.Models.PlanModel> RelatedPlanModel { get; set; }

        public ProveedoresOnLine.ThirdKnowledge.Models.PlanModel CurrentPlanModel { get; set; }

        public ProveedoresOnLine.ThirdKnowledge.Models.PeriodModel CurrentPeriodModel { get; set; }

        public List<MarketPlace.Models.General.GenericMenu> ThirdKnowledgeMenu { get; set; }

        public bool ShowAlerForQueries { get; set; }

        public int CurrentSale { get; set; }

        public bool HasPlan { get; set; }

        //Temp Cols
        public List<string[]> CollumnsResult { get; set; }

        public ThirdKnowledgeViewModel(ProveedoresOnLine.ThirdKnowledge.Models.PlanModel oCurrenPlan)
        {
            CurrentSale = oCurrenPlan.RelatedPeriodModel.FirstOrDefault().TotalQueries;
        }
        public ThirdKnowledgeViewModel()
        {

        }
    }
}
