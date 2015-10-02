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

        public ThirdKnowledgeViewModel(List<TDQueryInfoModel> oCollumnsResult)
        {
            this.RequestName = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RequestName)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.IdNumberRequest = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberRequest)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.QueryId = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.QueryId)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.GroupName = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.GroupName)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Priority = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Priotity)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.TypeDocument = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.TypeDocument)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.IdentificationNumberResult = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberResult)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.NameResult = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.NameResult)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.IdList = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdList)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.ListName = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.ListName)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Alias = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Alias)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Offense = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Offense)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Peps = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Peps)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Status = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Status)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Zone = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Zone)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Link = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Link)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.MoreInfo = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.MoreInfo)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.RegisterDate = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RegisterDate)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.LastModifyDate = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.LastModifyDate)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.Message = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Message)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.FileURL = oCollumnsResult.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.FileURL)
                .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
        }

        public ThirdKnowledgeViewModel()
        {

        }
    }
}
