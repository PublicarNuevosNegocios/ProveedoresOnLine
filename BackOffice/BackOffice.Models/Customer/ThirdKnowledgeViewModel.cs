using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string EndDate { get; set; }
        public string InitDate { get; set; }
        public string LastModify { get; set; }
        public string PlanPublicId { get; set; }
        public int QueriesByPeriod { get; set; }
        public List<PeriodModel> RelatedPeriodoModel { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        #endregion

        #region Period

        public int AssignedQueries { get; set; }
        public string PeriodPublicId { get; set; }
        public string PeriodInitDate { get; set; }
        public string PeriodEndDate { get; set; }
        public int TotalQueries { get; set; }
        public bool PeriodEnable { get; set; }
        public string PeriodLastModify { get; set; }
        public string PeriodCreateDate { get; set; }

        #endregion

        public ThirdKnowledgeViewModel(PlanModel RelatedPlan)
        {
            CompanyPublicId = RelatedPlan.CompanyPublicId;
            PlanPublicId = RelatedPlan.PlanPublicId;
            CreateDate = RelatedPlan.CreateDate.ToString();
            DaysByPeriod = RelatedPlan.DaysByPeriod;
            Enable = RelatedPlan.Enable;
            EndDate = RelatedPlan.EndDate.ToString();
            InitDate = RelatedPlan.InitDate.ToString();
            LastModify = RelatedPlan.LastModify.ToString();
            QueriesByPeriod = RelatedPlan.QueriesByPeriod;

            RelatedPeriodoModel = RelatedPlan.RelatedPeriodoModel;
            Status = RelatedPlan.Status.ItemId;
            StatusName = RelatedPlan.Status.ItemName;
        }

        public ThirdKnowledgeViewModel(PeriodModel RelatedPeriodModel)
        {
            AssignedQueries = RelatedPeriodModel.AssignedQueries;
            PeriodPublicId = RelatedPeriodModel.PeriodPublicId;
            PeriodInitDate = RelatedPeriodModel.InitDate.ToString();
            PeriodEndDate = RelatedPeriodModel.EndDate.ToString();
            TotalQueries = RelatedPeriodModel.TotalQueries;
            PeriodEnable = RelatedPeriodModel.Enable;
            PeriodLastModify = RelatedPeriodModel.LastModify.ToString();
            PeriodCreateDate = RelatedPeriodModel.CreateDate.ToString();

        }

        public ThirdKnowledgeViewModel()
        {

        }
    }
}
