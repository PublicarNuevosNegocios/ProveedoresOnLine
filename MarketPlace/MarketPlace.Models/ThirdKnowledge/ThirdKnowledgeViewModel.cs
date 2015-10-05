using MarketPlace.Models.General;
using ProveedoresOnLine.ThirdKnowledge.Models;
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
        public TDQueryModel CollumnsResult { get; set; }

        //Cosl
        public string RequestName { get; set; }
        public string IdNumberRequest { get; set; }
        public string QueryId { get; set; }
        public string IdGroup { get; set; }
        public string GroupName { get; set; }
        public string Priority { get; set; }
        public string TypeDocument { get; set; }
        public string IdentificationNumberResult { get; set; }
        public string NameResult { get; set; }
        public string IdList { get; set; }
        public string ListName { get; set; }
        public string Alias { get; set; }
        public string Offense { get; set; }
        public string Peps { get; set; }
        public string Status { get; set; }
        public string Zone { get; set; }
        public string Link { get; set; }
        public string MoreInfo { get; set; }
        public string RegisterDate { get; set; }
        public string LastModifyDate { get; set; }
        public string Message { get; set; }
        public string FileURL { get; set; }

        #region Third Knowledge Search

        public List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> ThirdKnowledgeResult { get; set; }

        public DateTime InitDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion

        public ThirdKnowledgeViewModel(ProveedoresOnLine.ThirdKnowledge.Models.PlanModel oCurrenPlan)
        {
            CurrentSale = oCurrenPlan.RelatedPeriodModel.FirstOrDefault().TotalQueries;
        }
       
        public ThirdKnowledgeViewModel()
        {

        }
    }
}
