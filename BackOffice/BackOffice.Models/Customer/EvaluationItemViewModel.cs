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
        public bool EvaluationItemEnable { get; set; }
        public string EvaluationItemLastModify { get; set; }

        public int EvaluationItemInfoId { get; set; }

        //ei.EvaluationItemId
        //,ei.EvaluationItemName
        //,ei_item.ItemId as EvaluationItemTypeId
        //,ei_item.Name as EvaluationItemTypeName
        //,ei.ParentEvaluationItem
        //,ei.Enable as EvaluationItemEnable
        //,ei.LastModify as EvaluationItemLastModify
        //,ei.CreateDate as EvaluationItemCreateDate

        //,ei_inf.EvaluationItemInfoId
        //,ei_inf_item.ItemId as EvaluationItemInfoTypeId
        //,ei_inf_item.Name as EvaluationItemInfoTypeName
        //,ei_inf.Value as EvaluationItemInfoValue
        //,ei_inf.LargeValue as EvaluationItemInfoLargeValue
        //,ei_inf.Enable as EvaluationItemInfoEnable
        //,ei_inf.CreateDate as EvaluationItemInfoCreateDate
        //,ei_inf.LastModify as EvaluationItemInfoLastModify

        public EvaluationItemViewModel() { }

        public EvaluationItemViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedEvaluationItem)
        {
            RelatedEvaluationItem = oRelatedEvaluationItem;

            EvaluationItemId = oRelatedEvaluationItem.ItemId;
            EvaluationItemName = oRelatedEvaluationItem.ItemName;
            EvaluationItemEnable = oRelatedEvaluationItem.Enable;
        }
    }
}
