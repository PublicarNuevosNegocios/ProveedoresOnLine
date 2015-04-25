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

        public int EvaluationItemId { get; set; }
        public string EvaluationItemName { get; set; }
        public int EvaluationItemTypeId { get; set; }
        public string EvaluationItemTypeName { get; set; }
        public int ParentEvaluationItem { get; set; }
        public bool EvaluationItemEnable { get; set; }
        public DateTime EvaluationItemCreateDate { get; set; }
        public DateTime EvaluationItemLastModify { get; set; }

        public int EvaluatorTypeId { get; set; }
        public string EvaluatorType { get; set; }

        public int EvaluatorId { get; set; }
        public string Evaluator { get; set; }

        public int UnitId { get; set; }
        public string Unit { get; set; }

        public int OrderId { get; set; }
        public string Order { get; set; }

        public int ApprovePercentageId { get; set; }
        public string ApprovePercentage { get; set; }

        public EvaluationItemViewModel() { }

        public EvaluationItemViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedEvaluationItem)
        {
            RelatedEvaluationItem = oRelatedEvaluationItem;
            
            EvaluationItemId = oRelatedEvaluationItem.ItemId;
            EvaluationItemName = oRelatedEvaluationItem.ItemName;
            EvaluationItemTypeId = oRelatedEvaluationItem.ItemType.ItemId;
            EvaluationItemTypeName = oRelatedEvaluationItem.ItemType.ItemName;
            ParentEvaluationItem = oRelatedEvaluationItem.ParentItem == null ? 0 : oRelatedEvaluationItem.ParentItem.ItemId;
            EvaluationItemEnable = oRelatedEvaluationItem.Enable;
            EvaluationItemCreateDate = oRelatedEvaluationItem.CreateDate;
            EvaluationItemLastModify = oRelatedEvaluationItem.LastModify;

            EvaluatorTypeId = oRelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                Select(x => x.ItemInfoId).
                                DefaultIfEmpty(0).
                                FirstOrDefault();
            EvaluatorType = oRelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.EvaluatorType).
                                Select(x => x.Value).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault();

            EvaluatorId = oRelatedEvaluationItem.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                                Select(x => x.ItemInfoId).
                                DefaultIfEmpty(0).
                                FirstOrDefault();
            Evaluator = oRelatedEvaluationItem.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Evaluator).
                            Select(x => x.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();

            UnitId = oRelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                        Select(x => x.ItemInfoId).
                        DefaultIfEmpty(0).
                        FirstOrDefault();
            Unit = oRelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Unit).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

            OrderId = oRelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                        Select(x => x.ItemInfoId).
                        DefaultIfEmpty(0).
                        FirstOrDefault();
            Order = oRelatedEvaluationItem.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.Order).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

            ApprovePercentageId = oRelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ApprovePercentage).
                                    Select(x => x.ItemInfoId).
                                    DefaultIfEmpty(0).
                                    FirstOrDefault();
            ApprovePercentage = oRelatedEvaluationItem.ItemInfo.
                                    Where(x => x.ItemInfoType.ItemId == (int)Models.General.enumEvaluationItemInfoType.ApprovePercentage).
                                    Select(x => x.Value).
                                    DefaultIfEmpty(string.Empty).
                                    FirstOrDefault();
        }
    }
}
