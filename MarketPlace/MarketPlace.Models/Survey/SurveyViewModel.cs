using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Survey
{
    public class SurveyViewModel
    {
        public ProveedoresOnLine.SurveyModule.Models.SurveyModel RelatedSurvey { get; set; }

        public int TotalRows { get; set; }

        public string SurveyPublicId { get { return RelatedSurvey.SurveyPublicId; } }

        #region Survey Search Fields

        public string SurveyLastModify
        {
            get
            {
                return RelatedSurvey.LastModify.ToString
                    (MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_DateFormat_Server].Value);
            }
        }


        public string SurveyConfigName
        {
            get
            {
                return RelatedSurvey.RelatedSurveyConfig.ItemName;
            }
        }

        public string SurveyConfigGroup
        {
            get
            {
                string oReturn = string.Empty;

                if (RelatedSurvey.RelatedSurveyConfig != null && RelatedSurvey.RelatedSurveyConfig.ItemInfo != null)
                {
                    oReturn = RelatedSurvey.RelatedSurveyConfig.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigInfoType.Group).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oReturn;
            }
        }


        public string SurveyIssueDate
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.IssueDate).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string SurveyResponsable
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Responsible).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string SurveyEvaluator
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Evaluator).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string SurveyStatus
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Status).
                    Select(y => y.ValueName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string SurveyProgress
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Progress).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string SurveyRating
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Rating).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        #endregion

        public SurveyViewModel(ProveedoresOnLine.SurveyModule.Models.SurveyModel oRelatedSurvey)
        {
            RelatedSurvey = oRelatedSurvey;
        }
    }
}
