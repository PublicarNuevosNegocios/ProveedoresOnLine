using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Survey
{
    public class SurveyItemViewModel
    {
        public ProveedoresOnLine.SurveyModule.Models.SurveyItemModel RelatedSurveyItem { get; private set; }

        public int Answer
        {
            get
            {
                return RelatedSurveyItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyItemInfoType.Answer).
                    Select(y => Convert.ToInt32(y.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public decimal Ratting
        {
            get
            {
                return ((RelatedSurveyItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyItemInfoType.Ratting).
                    Select(y => Convert.ToDecimal(y.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()));
            }
        }

        public string DescriptionText
        {
            get
            {
                return RelatedSurveyItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyItemInfoType.DescriptionText).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string AreaDescriptionText
        {
            get
            {
                return RelatedSurveyItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyItemInfoType.AreaDescription).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string EvaluatorRol
        {
            get
            {
                return RelatedSurveyItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyItemInfoType.EvaluatorRol).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public SurveyItemViewModel(ProveedoresOnLine.SurveyModule.Models.SurveyItemModel oRelatedSurveyItem)
        {
            RelatedSurveyItem = oRelatedSurveyItem;
        }
    }
}
