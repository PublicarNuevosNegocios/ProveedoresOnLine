using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class EvaluationItemViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedEvaluationItem { get; private set; }

        public string EvaluationItemId { get; set; }
        public string EvaluationItemName { get; set; }
        public string EvaluationItemTypeId { get; set; }
        public string EvaluationItemTypeName { get; set; }
        public string ParentEvaluationItem { get; set; }
        public bool EvaluationItemEnable { get; set; }

        #region EvaluationArea

        public string EA_EvaluatorTypeId { get; set; }
        public BackOffice.Models.General.enumEvaluatorType EA_EvaluatorType { get; set; }

        public string EA_EvaluatorId { get; set; }
        public string EA_Evaluator { get; set; }
        public string EA_EvaluatorName { get; set; }

        public string EA_UnitId { get; set; }
        public BackOffice.Models.General.enumEvaluationItemUnitType EA_Unit { get; set; }

        public string EA_OrderId { get; set; }
        public string EA_Order { get; set; }

        public string EA_ApprovePercentageId { get; set; }
        public string EA_ApprovePercentage { get; set; }

        #endregion

        #region EvaluationCriteria

        public string EC_EvaluatorTypeId { get; set; }
        public BackOffice.Models.General.enumEvaluatorType? EC_EvaluatorType { get; set; }

        public string EC_EvaluatorId { get; set; }
        public string EC_Evaluator { get; set; }

        public string EC_UnitId { get; set; }
        public BackOffice.Models.General.enumEvaluationItemUnitType EC_Unit { get; set; }

        public string EC_RatingId { get; set; }
        public string EC_Rating { get; set; }

        public string EC_EvaluationCriteriaId { get; set; }
        public BackOffice.Models.General.enumEvaluationCriteriaType EC_EvaluationCriteria { get; set; }

        public string EC_InfoType_Value_OperatorId { get; set; }
        public string EC_InfoType_Value_Operator { get; set; }

        public string EC_YearsQuantityId { get; set; }
        public string EC_YearsQuantity { get; set; }

        public string EC_ExperienceConfigId { get; set; }
        public string EC_ExperienceConfig { get; set; }

        private ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel oProjectConfigExperience;
        public ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel ProjectConfigExperience
        {
            get
            {
                if (oProjectConfigExperience == null)
                {
                    string strConfig = string.Empty;

                    if (RelatedEvaluationItem != null)
                    {
                        strConfig = RelatedEvaluationItem.ItemInfo.
                                        Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumEvaluationItemInfoType.ExperienceConfig).
                                        Select(x => x.LargeValue).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault();                                        
                    }

                    if (!string.IsNullOrEmpty(strConfig))
                    {
                        oProjectConfigExperience = (ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel)
                            (new System.Web.Script.Serialization.JavaScriptSerializer()).
                            Deserialize(strConfig, typeof(ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel));
                    }
                    else
                    {
                        oProjectConfigExperience = new ProveedoresOnLine.ProjectModule.Models.ProjectExperienceConfigModel()
                        {
                            AmmounEnable = false,
                            CurrencyEnable = false,
                            CustomAcitvityEnable = false,
                            DefaultAcitvityEnable = false,
                        };
                    }
                }

                return oProjectConfigExperience;
            }
        }

        public string EC_OrderId { get; set; }
        public string EC_Order { get; set; }

        public string EC_ApprovePercentageId { get; set; }
        public string EC_ApprovePercentage { get; set; }

        #endregion

        public bool RenderScripts { get; set; }

        public EvaluationItemViewModel() { }

        public EvaluationItemViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedEvaluationItem)
        {
            RelatedEvaluationItem = oRelatedEvaluationItem;

            EvaluationItemId = RelatedEvaluationItem.ItemId.ToString();
            EvaluationItemName = RelatedEvaluationItem.ItemName;
            EvaluationItemTypeId = RelatedEvaluationItem.ItemType.ItemId.ToString();
            EvaluationItemTypeName = RelatedEvaluationItem.ItemType.ItemName;
            ParentEvaluationItem = RelatedEvaluationItem.ParentItem == null ? null : RelatedEvaluationItem.ParentItem.ToString();
            EvaluationItemEnable = RelatedEvaluationItem.Enable;

            EA_EvaluatorTypeId = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                Select(x => x.ItemInfoId.ToString()).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();
            EA_EvaluatorType = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                Select(x => (BackOffice.Models.General.enumEvaluatorType)Convert.ToInt32(x.Value.Replace(" ", ""))).
                                DefaultIfEmpty(BackOffice.Models.General.enumEvaluatorType.None).
                                FirstOrDefault();

            EA_EvaluatorId = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                                Select(x => x.ItemInfoId.ToString()).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();
            EA_Evaluator = RelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                            Select(x => x.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
            if (RelatedEvaluationItem.ItemInfo != null &&
                RelatedEvaluationItem.ItemInfo.Count > 0 &&
                EA_EvaluatorType == Models.General.enumEvaluatorType.AnyoneRole)
            {
                EA_EvaluatorName = RelatedEvaluationItem.ItemInfo.
                                   Where(x => x.ItemInfoId.ToString() == EA_EvaluatorId).
                                   Select(x => x.Value).
                                   DefaultIfEmpty(string.Empty).
                                   FirstOrDefault();
            }

            EA_UnitId = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                        Select(x => x.ItemInfoId.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
            EA_Unit = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                        Select(x => (BackOffice.Models.General.enumEvaluationItemUnitType)Convert.ToInt32(x.Value.Replace(" ", ""))).
                        DefaultIfEmpty(BackOffice.Models.General.enumEvaluationItemUnitType.None).
                        FirstOrDefault();

            EA_OrderId = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                        Select(x => x.ItemInfoId.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
            EA_Order = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

            EA_ApprovePercentageId = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ApprovePercentage).
                                    Select(x => x.ItemInfoId.ToString()).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();
            EA_ApprovePercentage = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ApprovePercentage).
                                    Select(x => x.Value).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();

            EC_EvaluatorTypeId = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                    Select(x => x.ItemInfoId.ToString()).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();
            EC_EvaluatorType = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                    Select(x => (BackOffice.Models.General.enumEvaluatorType)Convert.ToInt32(x.Value.Replace(" ", ""))).
                                    DefaultIfEmpty(BackOffice.Models.General.enumEvaluatorType.None).
                                    FirstOrDefault();

            EC_EvaluatorId = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                                Select(x => x.ItemInfoId.ToString()).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();
            EC_Evaluator = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                                Select(x => x.Value).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();

            EC_UnitId = RelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                            Select(x => x.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
            EC_Unit = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                        Select(x => (BackOffice.Models.General.enumEvaluationItemUnitType)Convert.ToInt32(x.Value.Replace(" ", ""))).
                        DefaultIfEmpty(BackOffice.Models.General.enumEvaluationItemUnitType.None).
                        FirstOrDefault();

            EC_RatingId = RelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Rating).
                            Select(x => x.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
            EC_Rating = RelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Rating).
                            Select(x => x.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();

            EC_EvaluationCriteriaId = RelatedEvaluationItem.ItemInfo.
                                           Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluationCriteria).
                                           Select(x => x.ItemInfoId.ToString()).
                                           DefaultIfEmpty(string.Empty).
                                           FirstOrDefault();
            EC_EvaluationCriteria = RelatedEvaluationItem.ItemInfo.
                                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluationCriteria).
                                        Select(x => (BackOffice.Models.General.enumEvaluationCriteriaType)Convert.ToInt32(x.Value.Replace(" ", ""))).
                                        DefaultIfEmpty(BackOffice.Models.General.enumEvaluationCriteriaType.None).
                                        FirstOrDefault();

            EC_InfoType_Value_OperatorId = RelatedEvaluationItem.ItemInfo.
                                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.InfoType_Value_Operator).
                                            Select(x => x.ItemInfoId.ToString()).
                                            DefaultIfEmpty(string.Empty).
                                            FirstOrDefault();
            EC_InfoType_Value_Operator = RelatedEvaluationItem.ItemInfo.
                                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.InfoType_Value_Operator).
                                            Select(x => x.Value).
                                            DefaultIfEmpty(string.Empty).
                                            FirstOrDefault();

            EC_YearsQuantityId = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.YearsQuantity).
                                    Select(x => x.ItemInfoId.ToString()).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();
            EC_YearsQuantity = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.YearsQuantity).
                                    Select(x => x.Value).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();

            EC_ExperienceConfigId = RelatedEvaluationItem.ItemInfo.
                                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ExperienceConfig).
                                        Select(x => x.ItemInfoId.ToString()).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault();
            EC_ExperienceConfig = RelatedEvaluationItem.ItemInfo.
                                       Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ExperienceConfig).
                                       Select(x => x.LargeValue).
                                       DefaultIfEmpty(string.Empty).
                                       FirstOrDefault();

            EC_OrderId = RelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                            Select(x => x.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
            EC_Order = RelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                            Select(x => x.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();

            EC_ApprovePercentageId = RelatedEvaluationItem.ItemInfo.
                                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                                        Select(x => x.ItemInfoId.ToString()).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault();
            EC_ApprovePercentage = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                                    Select(x => x.Value).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();
        }

        #region Methods

        public Dictionary<string, string> GetExperienceYearValues()
        {
            Dictionary<string, string> oReturn = new Dictionary<string, string>();

            if (ProjectConfigExperience != null &&
                ProjectConfigExperience.YearsInterval != null &&
                ProjectConfigExperience.YearsInterval.Count > 0)
            {
                oReturn = ProjectConfigExperience.YearsInterval.
                    Where(qint => qint.Split('_').Length >= 4).
                    Select(qint => new
                    {
                        oKey = qint,
                        oValue = qint.Split('_')[3],
                    }).ToDictionary(k => k.oKey, v => v.oValue);
            }
            return oReturn;
        }

        public Dictionary<string, string> GetExperienceQuantityValues()
        {
            Dictionary<string, string> oReturn = new Dictionary<string, string>();

            if (ProjectConfigExperience != null &&
                ProjectConfigExperience.QuantityInterval != null &&
                ProjectConfigExperience.QuantityInterval.Count > 0)
            {
                oReturn = ProjectConfigExperience.QuantityInterval.
                    Where(qint => qint.Split('_').Length >= 2).
                    Select(qint => new
                    {
                        oKey = qint.Split('_')[0],
                        oValue = qint.Split('_')[1],
                    }).ToDictionary(k => k.oKey, v => v.oValue);
            }
            return oReturn;
        }

        #endregion
    }
}
