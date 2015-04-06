using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Survey
{
    public class SurveyViewModel
    {
        public ProveedoresOnLine.SurveyModule.Models.SurveyModel RelatedSurvey { get; private set; }

        public int TotalRows { get; set; }

        public int CurrentStepId { get; set; }

        public MarketPlace.Models.General.GenericMenu CurrentActionMenu { get; set; }

        public string SurveyPublicId { get { return RelatedSurvey.SurveyPublicId; } }

        #region Survey Info Fields

        public string SurveyLastModify
        {
            get
            {
                return RelatedSurvey.LastModify.ToString
                    (MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_DateFormat_Server].Value);
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

        public int SurveyStatusId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Status).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
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

        public int SurveyProgressId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Progress).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
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

        public int SurveyRatingId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Rating).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public decimal SurveyRating
        {
            get
            {
                return ((5 * RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Rating).
                    Select(y => Convert.ToDecimal(y.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()) / 100);
            }
        }

        #endregion

        #region Survey Config Fields

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

        public bool SurveyConfigStepEnable
        {
            get
            {
                bool oReturn = false;

                if (RelatedSurvey.RelatedSurveyConfig != null && RelatedSurvey.RelatedSurveyConfig.ItemInfo != null)
                {
                    oReturn = RelatedSurvey.RelatedSurveyConfig.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigInfoType.StepEnable).
                        Select(y => !string.IsNullOrEmpty(y.Value) && y.Value.Trim().ToLower() == "true").
                        DefaultIfEmpty(false).
                        FirstOrDefault();
                }

                return oReturn;
            }
        }

        #endregion

        public SurveyViewModel(ProveedoresOnLine.SurveyModule.Models.SurveyModel oRelatedSurvey)
        {
            RelatedSurvey = oRelatedSurvey;
        }

        #region public methods

        public List<SurveyConfigItemViewModel> GetSurveyConfigItem
            (MarketPlace.Models.General.enumSurveyConfigItemType vSurveyConfigItemType,
            int? vParentSurveyConfigItem)
        {
            List<SurveyConfigItemViewModel> oReturn =
                RelatedSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.
                Where(x => x.ItemType.ItemId == (int)vSurveyConfigItemType &&
                        (vParentSurveyConfigItem != null && x.ParentItem != null ?
                        vParentSurveyConfigItem.Value == x.ParentItem.ItemId : true)).
                Select(x => new SurveyConfigItemViewModel(x)).
                OrderBy(x => x.Order).
                ToList();

            return oReturn;
        }

        public SurveyItemViewModel GetSurveyItem(int vSurveyConfigItemId)
        {
            SurveyItemViewModel oReturn =
                RelatedSurvey.RelatedSurveyItem.
                Where(x => x.RelatedSurveyConfigItem.ItemId == vSurveyConfigItemId).
                Select(x => new SurveyItemViewModel(x)).
                FirstOrDefault();

            return oReturn;
        }

        /// <summary>
        /// get config steps from CurrentStepId
        /// </summary>
        /// <returns>LastStepId,NextStepId</returns>
        public Tuple<int?, int?> GetSurveyConfigSteps()
        {
            List<SurveyConfigItemViewModel> CurrentSurveyAreas = GetSurveyConfigItem(MarketPlace.Models.General.enumSurveyConfigItemType.EvaluationArea, null);

            int? LastStepId = null;
            int? NextStepId = null;

            LastStepId = CurrentSurveyAreas.
                Where(x => x.Order < CurrentStepId).
                Select(x => (int?)x.Order).
                OrderByDescending(x => x).
                DefaultIfEmpty(null).
                FirstOrDefault();

            NextStepId = CurrentSurveyAreas.
                Where(x => x.Order > CurrentStepId).
                Select(x => (int?)x.Order).
                OrderBy(x => x).
                DefaultIfEmpty(null).
                FirstOrDefault();

            return new Tuple<int?, int?>(LastStepId, NextStepId);
        }

        /// <summary>
        /// get config first stepid
        /// </summary>
        /// <returns>FirstStepId</returns>
        public int GetSurveyConfigFirstStep()
        {
            List<SurveyConfigItemViewModel> CurrentSurveyAreas = GetSurveyConfigItem(MarketPlace.Models.General.enumSurveyConfigItemType.EvaluationArea, null);
            return CurrentSurveyAreas.
                Min(x => x.Order);
        }

        #endregion
    }
}
