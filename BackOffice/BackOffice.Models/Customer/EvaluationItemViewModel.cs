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
        public string EA_EvaluatorType { get; set; }

        public string EA_EvaluatorId { get; set; }
        public string EA_Evaluator { get; set; }
        public string EA_EvaluatorName { get; set; }

        public string EA_UnitId { get; set; }
        public string EA_Unit { get; set; }

        public string EA_OrderId { get; set; }
        public string EA_Order { get; set; }

        public string EA_ApprovePercentageId { get; set; }
        public string EA_ApprovePercentage { get; set; }

        #endregion

        #region EvaluationCriteria

        public string EC_EvaluatorTypeId { get; set; }
        public string EC_EvaluatorType { get; set; }

        public string EC_EvaluatorId { get; set; }
        public string EC_Evaluator { get; set; }

        public string EC_UnitId { get; set; }
        public string EC_Unit { get; set; }

        public string EC_RatingId { get; set; }
        public string EC_Rating { get; set; }

        public string EC_EvaluationCriteriaId { get; set; }
        public string EC_EvaluationCriteria { get; set; }

        public string EC_InfoType_Value_OperatorId { get; set; }
        public string EC_InfoType_Value_Operator { get; set; }

        public string EC_YearsQuantityId { get; set; }
        public string EC_YearsQuantity { get; set; }

        public string EC_ExperienceConfigId { get; set; }
        public string EC_ExperienceConfig { get; set; }

        public string EC_OrderId { get; set; }
        public string EC_Order { get; set; }

        public string EC_ApprovePercentageId { get; set; }
        public string EC_ApprovePercentage { get; set; }

        #endregion

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
                                Select(x => x.Value).
                                DefaultIfEmpty(string.Empty).
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
                EA_EvaluatorType == ((int)Models.General.enumEvaluatorType.AnyoneRole).ToString())
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
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
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
                                    Select(x => x.Value).
                                    DefaultIfEmpty(string.Empty).
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
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
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
                                        Select(x => x.Value).
                                        DefaultIfEmpty(string.Empty).
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
    }
}
