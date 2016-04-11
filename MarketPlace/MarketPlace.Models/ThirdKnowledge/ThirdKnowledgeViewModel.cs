using MarketPlace.Models.General;
using MarketPlace.Models.ThirdKnowledge;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.ThirdKnowledge
{
    public class ThirdKnowledgeViewModel
    {
        public bool RenderScripts { get; set; }

        public List<ProveedoresOnLine.ThirdKnowledge.Models.PlanModel> RelatedPlanModel { get; set; }

        public ProveedoresOnLine.ThirdKnowledge.Models.PlanModel CurrentPlanModel { get; set; }

        public ProveedoresOnLine.ThirdKnowledge.Models.PeriodModel CurrentPeriodModel { get; set; }

        public List<MarketPlace.Models.General.GenericMenu> ThirdKnowledgeMenu { get; set; }

        public ThirdKnowledgeSearchViewModel RelatedThidKnowledgePager { get; set; }

        public bool ShowAlerForQueries { get; set; }

        public int CurrentSale { get; set; }

        public bool HasPlan { get; set; }

        //Params to Research
        public string SearchNameParam { get; set; }

        public string SearchIdNumberParam { get; set; }

        public bool ReSearch { get; set; }

        //Temp Cols
        public TDQueryModel CollumnsResult { get; set; }

        public string ReturnUrl { get; set; }

        public string QueryBasicPublicId { get; set; }

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

        public ThirdKnowledgeViewModel(List<TDQueryDetailInfoModel> oDetail)
        {
            QueryBasicPublicId = oDetail.FirstOrDefault().QueryBasicPublicId;
            RequestName = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RequestName).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            IdNumberRequest = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberRequest).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            QueryId = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.QueryId).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();            
            GroupName = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.GroupName).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Priority = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Priotity).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            TypeDocument = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.TypeDocument).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            IdentificationNumberResult = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdentificationNumberResult).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            NameResult = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.NameResult).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            IdList = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdList).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            ListName = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.ListName).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Alias = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Alias).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Offense = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Offense).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Peps = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Peps).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Status = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Status).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Zone = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Zone).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Link = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Link).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            MoreInfo = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.MoreInfo).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            RegisterDate = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RegisterDate).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            LastModifyDate = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.LastModifyDate).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            Message = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Message).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            FileURL = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.FileURL).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();            
            Status = oDetail.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Status).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault().ToString().ToLower();
        }

        public ThirdKnowledgeViewModel()
        {

        }      
    }
}
