using ProveedoresOnLine.ThirdKnowledge.Models;
using System.Collections.Generic;

namespace BackOffice.Models.Customer
{
    public class ThirdKnowledgeViewModel
    {
        #region Plan Model

        public PlanModel RelatedPlan { get; set; }

        public string CompanyPublicId { get; set; }
        public string CreateDate { get; set; }
        public int DaysByPeriod { get; set; }
        public bool Enable { get; set; }
        public bool PlanIsLimited { get; set; }
        public string EndDate { get; set; }
        public string InitDate { get; set; }
        public string LastModify { get; set; }
        public string PlanPublicId { get; set; }
        public int QueriesByPeriod { get; set; }
        public List<PeriodModel> RelatedPeriodoModel { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }

        #endregion Plan Model

        #region Period

        public int AssignedQueries { get; set; }
        public string PeriodPublicId { get; set; }
        public string PeriodInitDate { get; set; }
        public string PeriodEndDate { get; set; }
        public bool PerIsLimited { get; set; }
        public int TotalQueries { get; set; }
        public bool PeriodEnable { get; set; }
        public string PeriodLastModify { get; set; }
        public string PeriodCreateDate { get; set; }

        #endregion Period

        #region Query

        public string QueryCreateDate { get; set; }

        public string QueryStatus { get; set; }

        public string QueryType { get; set; }

        public string QueryUser { get; set; }

        #endregion Query

        public ThirdKnowledgeViewModel(PlanModel RelatedPlan)
        {
            CompanyPublicId = RelatedPlan.CompanyPublicId;
            PlanPublicId = RelatedPlan.PlanPublicId;
            CreateDate = RelatedPlan.CreateDate.ToString();
            DaysByPeriod = RelatedPlan.DaysByPeriod;
            Enable = RelatedPlan.Enable;
            PlanIsLimited = RelatedPlan.IsLimited;
            EndDate = RelatedPlan.EndDate.ToString();
            InitDate = RelatedPlan.InitDate.ToString();
            LastModify = RelatedPlan.LastModify.ToString();
            QueriesByPeriod = RelatedPlan.QueriesByPeriod;

            RelatedPeriodoModel = RelatedPlan.RelatedPeriodModel;
            Status = RelatedPlan.Status.ItemId;
            StatusName = RelatedPlan.Status.ItemName;
        }

        public ThirdKnowledgeViewModel(PeriodModel RelatedPeriodModel)
        {
            AssignedQueries = RelatedPeriodModel.AssignedQueries;
            PeriodPublicId = RelatedPeriodModel.PeriodPublicId;
            PeriodInitDate = RelatedPeriodModel.InitDate.ToString();
            PeriodEndDate = RelatedPeriodModel.EndDate.ToString();
            PerIsLimited = RelatedPeriodModel.IsLimited;
            TotalQueries = RelatedPeriodModel.TotalQueries;
            PeriodEnable = RelatedPeriodModel.Enable;
            PeriodLastModify = RelatedPeriodModel.LastModify.ToString();
            PeriodCreateDate = RelatedPeriodModel.CreateDate.ToString();
        }

        public ThirdKnowledgeViewModel(TDQueryModel RelatedQuery)
        {
            this.QueryCreateDate = RelatedQuery.CreateDate.ToString();

            this.QueryStatus = RelatedQuery.IsSuccess.ToString();

            this.QueryType = RelatedQuery.SearchType.ItemName;

            this.QueryUser = RelatedQuery.User;
        }

        public ThirdKnowledgeViewModel()
        {
        }
    }
}