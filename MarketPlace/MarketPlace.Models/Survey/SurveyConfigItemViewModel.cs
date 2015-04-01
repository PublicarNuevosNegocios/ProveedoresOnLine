using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Survey
{
    public class SurveyConfigItemViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedSurveyConfigItem { get; private set; }

        public int SurveyConfigItemId { get { return RelatedSurveyConfigItem.ItemId; } }

        public string Name { get { return RelatedSurveyConfigItem.ItemName; } }

        public int Order
        {
            get
            {
                return RelatedSurveyConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.Order).
                    Select(y => Convert.ToInt32(y.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public bool HasDescription
        {
            get
            {
                return RelatedSurveyConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.HasDescription).
                    Select(y => !string.IsNullOrEmpty(y.Value) && y.Value.Trim().ToLower() == "true").
                    DefaultIfEmpty(false).
                    FirstOrDefault();
            }
        }

        public SurveyConfigItemViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedSurveyConfigItem)
        {
            RelatedSurveyConfigItem = oRelatedSurveyConfigItem;
        }
    }
}
