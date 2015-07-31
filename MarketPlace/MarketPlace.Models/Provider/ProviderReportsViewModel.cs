using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderReportsViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedReportInfo { get; set; }

        public MarketPlace.Models.General.enumReportType ReportType { get { return (MarketPlace.Models.General.enumReportType)RelatedReportInfo.ItemType.ItemId; } }

        #region Survey

        public string oRP_Observation { get; set; }
        public string RP_Observation
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_Observation))
                {
                    oRP_Observation = RelatedReportInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_Observation).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oRP_Observation;
            }
        }

        public string oRP_ImprovementPlan { get; set; }
        public string RP_ImprovementPlan
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_ImprovementPlan))
                {
                    oRP_ImprovementPlan = RelatedReportInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ImprovementPlan).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oRP_ImprovementPlan;
            }
        }

        public string oRP_InitDateReport { get; set; }
        public string RP_InitDateReport
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_InitDateReport))
                {
                    oRP_InitDateReport = RelatedReportInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_InitDateReport).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRP_InitDateReport;
            }
        }

        public string oRP_EndDateReport { get; set; }
        public string RP_EndDateReport
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_EndDateReport))
                {
                    oRP_EndDateReport = RelatedReportInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_EndDateReport).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRP_EndDateReport;
            }
        }

        public string oRP_ReportAverage { get; set; }
        public string RP_ReportAverage
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_ReportAverage))
                {
                    oRP_ReportAverage = RelatedReportInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportAverage).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRP_ReportAverage;
            }
        }

        public string oRP_ReportDate { get; set; }
        public string RP_ReportDate
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_ReportDate))
                {
                    oRP_ReportDate = RelatedReportInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRP_ReportDate;
            }
        }

        public string oRP_ReportURL { get; set; }
        public string RP_ReportURL
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_ReportURL))
                {
                    oRP_ReportURL = RelatedReportInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportURL).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRP_ReportURL;
            }
        }

        public string oRP_ProviderPublicId { get; set; }
        public string RP_ProviderPublicId
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_ProviderPublicId))
                {
                    oRP_ProviderPublicId = RelatedReportInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ProviderPublicId).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRP_ProviderPublicId;
            }
        }

        public string oRP_ReportResponsable { get; set; }
        public string RP_ReportResponsable
        {
            get
            {
                if (string.IsNullOrEmpty(oRP_ReportResponsable))
                {
                    oRP_ReportResponsable = RelatedReportInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportResponsable).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRP_ReportResponsable;
            }
        }
        #endregion        

        public ProviderReportsViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedReport)
        {            
            RelatedReportInfo = RelatedReport;                          
        }

        public ProviderReportsViewModel() { }
    }
}
