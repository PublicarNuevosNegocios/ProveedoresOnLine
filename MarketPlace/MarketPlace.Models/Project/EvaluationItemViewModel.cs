using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Project
{
    public class EvaluationItemViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedEvaluationItem { get; private set; }

        public int EvaluationItemId { get { return RelatedEvaluationItem.ItemId; } }

        public string EvaluationItemName { get { return RelatedEvaluationItem.ItemName; } }

        public MarketPlace.Models.General.enumEvaluationItemUnitType EvaluationItemUnit
        {
            get
            {
                return RelatedEvaluationItem.ItemInfo.
                    Where(eiinf => eiinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemInfoType.Unit &&
                                 !string.IsNullOrEmpty(eiinf.Value)).
                    Select(eiinf => (MarketPlace.Models.General.enumEvaluationItemUnitType)Convert.ToInt32(eiinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(MarketPlace.Models.General.enumEvaluationItemUnitType.Informative).
                    FirstOrDefault();
            }
        }

        public string EvaluationItemUnitName
        {
            get
            {
                switch (EvaluationItemUnit)
                {
                    case General.enumEvaluationItemUnitType.LooseWin:
                        return "Pasa/No Pasa";
                    case General.enumEvaluationItemUnitType.Percent:
                        return "%";
                    default:
                        return "Informativo";
                }
            }
        }

        public decimal AprobalPercent
        {
            get
            {
                if (EvaluationItemUnit == General.enumEvaluationItemUnitType.Percent)
                {
                    return RelatedEvaluationItem.ItemInfo.
                        Where(eiinf => eiinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemInfoType.AprobalPercent &&
                                     !string.IsNullOrEmpty(eiinf.Value)).
                        Select(eiinf => Convert.ToDecimal(eiinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
                        DefaultIfEmpty(0).
                        FirstOrDefault();
                }
                else
                {
                    return 100;
                }
            }
        }

        public MarketPlace.Models.General.enumEvaluationCriteria EvaluationCriteria
        {
            get
            {
                return RelatedEvaluationItem.ItemInfo.
                    Where(eiinf => eiinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumEvaluationItemInfoType.EvaluationCriteria &&
                                 !string.IsNullOrEmpty(eiinf.Value)).
                    Select(eiinf => (MarketPlace.Models.General.enumEvaluationCriteria)Convert.ToInt32(eiinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(MarketPlace.Models.General.enumEvaluationCriteria.None).
                    FirstOrDefault();
            }
        }

        public EvaluationItemViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedEvaluationItem)
        {
            RelatedEvaluationItem = oRelatedEvaluationItem;
        }
    }
}
