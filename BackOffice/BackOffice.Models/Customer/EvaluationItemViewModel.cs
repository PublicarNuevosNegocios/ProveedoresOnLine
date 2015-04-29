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

        public string EvaluatorTypeId { get; set; }
        public string EvaluatorType { get; set; }

        public string EvaluatorId { get; set; }
        public string Evaluator { get; set; }

        public string UnitId { get; set; }
        public string Unit { get; set; }

        public string OrderId { get; set; }
        public string Order { get; set; }

        public string ApprovePercentageId { get; set; }
        public string ApprovePercentage { get; set; }

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

            EvaluatorTypeId = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                Select(x => x.ItemInfoId.ToString()).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();
            EvaluatorType = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                Select(x => x.Value).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();

            EvaluatorId = RelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                                Select(x => x.ItemInfoId.ToString()).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();
            Evaluator = RelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                            Select(x => x.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();

            UnitId = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                        Select(x => x.ItemInfoId.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
            Unit = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

            OrderId = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                        Select(x => x.ItemInfoId.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
            Order = RelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

            ApprovePercentageId = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ApprovePercentage).
                                    Select(x => x.ItemInfoId.ToString()).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();
            ApprovePercentage = RelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ApprovePercentage).
                                    Select(x => x.Value).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();
        }
    }
}
