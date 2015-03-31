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

        public int SurveyConfigId
        {
            get
            {
                return RelatedSurvey.RelatedSurveyConfig.ItemId;
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

        public int SurveyIssueDateId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.IssueDate).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
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

        public string SurveyResponsible
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

        public int SurveyEvaluatorId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Evaluator).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
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

        public string SurveyStatusName
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

        public MarketPlace.Models.General.enumSurveyStatus SurveyStatus
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Status).
                    Select(y => (MarketPlace.Models.General.enumSurveyStatus)Convert.ToInt32(y.Value)).
                    DefaultIfEmpty(MarketPlace.Models.General.enumSurveyStatus.Program).
                    FirstOrDefault();
            }
        }

        public int SurveyProgress
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Progress).
                    Select(y => Convert.ToInt32(y.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public int SurveyRating
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Rating).
                    Select(y => Convert.ToInt32(y.Value)).
                    DefaultIfEmpty(0).
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
