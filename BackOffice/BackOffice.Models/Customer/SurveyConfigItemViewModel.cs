using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class SurveyConfigItemViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedConfigItem { get; private set; }

        public string SurveyConfigItemId { get; set; }

        public string SurveyConfigItemName { get; set; }

        public string SurveyConfigItemTypeId { get; set; }

        public string ParentSurveyConfigItem { get; set; }

        public bool SurveyConfigItemEnable { get; set; }

        public string SurveyConfigItemInfoOrder { get; set; }
        public string SurveyConfigItemInfoOrderId { get; set; }

        public string SurveyConfigItemInfoWeight { get; set; }
        public string SurveyConfigItemInfoWeightId { get; set; }

        public bool SurveyConfigItemInfoHasDescription { get; set; }
        public string SurveyConfigItemInfoHasDescriptionId { get; set; }

        public bool SurveyConfigItemInfoIsMandatory { get; set; }
        public string SurveyConfigItemInfoIsMandatoryId { get; set; }

        public string SurveyConfigItemInfoQuestionType { get; set; }
        public string SurveyConfigItemInfoQuestionTypeId { get; set; }

        public string SurveyConfigItemInfoRol { get; set; }
        public string SurveyConfigItemInfoRolId { get; set; }

        public string SurveyConfigItemInfoRolWeight { get; set; }
        public string SurveyConfigItemInfoRolWeightId { get; set; }

        public SurveyConfigItemViewModel() { }

        public SurveyConfigItemViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedConfigItem)
        {
            RelatedConfigItem = oRelatedConfigItem;

            //generic values
            SurveyConfigItemId = RelatedConfigItem.ItemId.ToString();
            SurveyConfigItemName = RelatedConfigItem.ItemName;
            SurveyConfigItemTypeId = RelatedConfigItem.ItemType.ItemId.ToString();
            ParentSurveyConfigItem = RelatedConfigItem.ParentItem == null ? null : RelatedConfigItem.ParentItem.ItemId.ToString();
            SurveyConfigItemEnable = RelatedConfigItem.Enable;

            //generic infos
            SurveyConfigItemInfoOrder = RelatedConfigItem.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.Order).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SurveyConfigItemInfoOrderId = RelatedConfigItem.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.Order).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();


            SurveyConfigItemInfoWeight = RelatedConfigItem.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.Weight).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SurveyConfigItemInfoWeightId = RelatedConfigItem.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.Weight).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            if (RelatedConfigItem.ItemType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemType.Question)
            {
                #region Question

                SurveyConfigItemInfoHasDescription = RelatedConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.HasDescription).
                    Select(y => y.Value.ToLower() == "true").
                    DefaultIfEmpty(false).
                    FirstOrDefault();

                SurveyConfigItemInfoHasDescriptionId = RelatedConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.HasDescription).
                    Select(y => y.ItemInfoId.ToString()).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();

                SurveyConfigItemInfoIsMandatory = RelatedConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.IsMandatory).
                    Select(y => y.Value.ToLower() == "true").
                    DefaultIfEmpty(false).
                    FirstOrDefault();

                SurveyConfigItemInfoIsMandatoryId = RelatedConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.IsMandatory).
                    Select(y => y.ItemInfoId.ToString()).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();

                SurveyConfigItemInfoQuestionTypeId = RelatedConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.QuestionType).
                    Select(y => y.ItemInfoId.ToString()).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();

                SurveyConfigItemInfoQuestionType = RelatedConfigItem.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigItemInfoType.QuestionType).
                    Select(y => y.Value.ToString()).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();

                #endregion
            }
        }
    }
}
