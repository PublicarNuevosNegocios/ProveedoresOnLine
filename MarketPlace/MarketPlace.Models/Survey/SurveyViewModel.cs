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

        public decimal Average { get; set; }

        List<MarketPlace.Models.General.FileModel> oSurveyFile;
        public List<MarketPlace.Models.General.FileModel> SurveyFile
        {
            get
            {
                if (oSurveyFile == null)
                {
                    oSurveyFile = RelatedSurvey.SurveyInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.File &&
                                       !string.IsNullOrEmpty(pjinf.LargeValue) &&
                                       pjinf.LargeValue.Split(',').Length >= 2).
                        Select(pjinf => new MarketPlace.Models.General.FileModel()
                        {
                            FileObjectId = pjinf.ItemInfoId.ToString(),
                            ServerUrl = pjinf.LargeValue.Split(',')[0],
                            FileName = pjinf.LargeValue.Split(',')[1]
                        }).
                        ToList();

                    if (oSurveyFile == null)
                        oSurveyFile = new List<MarketPlace.Models.General.FileModel>();
                }
                return oSurveyFile;
            }
        }

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

        public int SurveyExpirationDateId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.ExpirationDate).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string SurveyExpirationDate
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.ExpirationDate).
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
                //return RelatedSurvey.SurveyInfo.
                //    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Evaluator).
                //    Select(y => y.Value).
                //    DefaultIfEmpty(string.Empty).
                //    FirstOrDefault();

                if (RelatedSurvey.ParentSurveyPublicId != null)
                {
                    return RelatedSurvey.User;                    
                }
                else
                {
                    return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Evaluator).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
                }
            }
        }

        public List<int> SurveyEvaluatorIdList
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Evaluator).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    ToList();
            }
        }

        public List<string> SurveyEvaluatorList
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Evaluator).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    ToList();
            }
        }

        public int SurveyStartDateId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.StartDate).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string SurveyStartDate
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.StartDate).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public int SurveyEndDateId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.EndDate).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string SurveyEndDate
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.EndDate).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public int SurveyContractId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Contract).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string SurveyContract
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Contract).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public int SurveyCommentsId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Comments).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string SurveyComments
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Comments).
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

        public decimal SurveyProgress
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Progress).
                    Select(y => !string.IsNullOrEmpty(y.Value) ? Convert.ToDecimal(y.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")) : 0).
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
                return ((RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Rating).
                    Select(y => Convert.ToDecimal(y.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()));
            }
        }        

        public int SurveyRelatedProjectId
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Project).
                    Select(y => y.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string SurveyRelatedProject
        {
            get
            {
                return RelatedSurvey.SurveyInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyInfoType.Project).
                    Select(y => y.ValueName).
                    DefaultIfEmpty("").
                    FirstOrDefault();
            }
        }

        public DateTime FilterDateIni { get; set; }

        public DateTime FilterEndDate { get; set; }
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


        public Tuple<int, int> GetMandatoryAnsweredQuestions()
        {
            var MandatoryAux = RelatedSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.
                Where(scit => scit.ItemType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemType.Question &&
                            scit.ItemInfo.Any(scitinf => scitinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.IsMandatory &&
                                                        !string.IsNullOrEmpty(scitinf.Value) &&
                                                        scitinf.Value.Replace(" ", "") == "true")).
                Select(scit => new
                {
                    Answered = Convert.ToInt32(RelatedSurvey.RelatedSurveyItem.Any(svit => svit.RelatedSurveyConfigItem.ItemId == scit.ItemId)),
                });

            Tuple<int, int> oReturn = new Tuple<int, int>(0, 0);

            if (MandatoryAux != null && MandatoryAux.Count() > 0)
            {
                oReturn = new Tuple<int, int>(MandatoryAux.Count(), (int)MandatoryAux.Sum(ma => ma.Answered));
            }

            return oReturn;
        }

        #endregion
    }
}
