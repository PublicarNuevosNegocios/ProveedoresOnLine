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

        public bool IsMandatory
        {
            get
            {
                return RelatedSurveyConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.IsMandatory).
                    Select(y => !string.IsNullOrEmpty(y.Value) && y.Value.Trim().ToLower() == "true").
                    DefaultIfEmpty(false).
                    FirstOrDefault();
            }
        }

        public decimal Weight
        {
            get
            {
                return RelatedSurveyConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.Weight).
                    Select(y => Convert.ToDecimal(y.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }
        public string QuestionType
        {
            get
            {
                return RelatedSurveyConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.QuestionType).
                    Select(y => (y.Value)).
                    DefaultIfEmpty("").
                    FirstOrDefault();
            }
        }

        public string SurveyConfigItemInfoRolWeight { get; set; }
        public string SurveyConfigItemInfoRolWeightId { get; set; }

        public string SurveyConfigItemInfoRol { get; set; }
        public string SurveyConfigItemInfoRolId { get; set; }
        public string SurveyConfigItemInfoRolName { get; set; }

        public SurveyConfigItemViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedSurveyConfigItem)
        {
            RelatedSurveyConfigItem = oRelatedSurveyConfigItem;

            SurveyConfigItemInfoRol = oRelatedSurveyConfigItem.ItemInfo.
             Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.RolId).
             Select(y => y.Value).
             DefaultIfEmpty(string.Empty).
             FirstOrDefault();

            SurveyConfigItemInfoRolId = oRelatedSurveyConfigItem.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.RolId).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SurveyConfigItemInfoRolName = !string.IsNullOrEmpty(SurveyConfigItemInfoRol) ? MarketPlace.Models.Company.CompanyUtil.CompanyRole.RelatedRole.
                                          Where(x => x.ItemId == Convert.ToInt32(SurveyConfigItemInfoRol))
                                          .Select(x => x.ItemName).FirstOrDefault() : string.Empty;               

            SurveyConfigItemInfoRolWeight = oRelatedSurveyConfigItem.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.RolWeight).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SurveyConfigItemInfoRolWeightId = oRelatedSurveyConfigItem.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumSurveyConfigItemInfoType.RolWeight).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
        }
    }
}
