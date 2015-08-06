using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class SurveyConfigViewModel
    {
        public ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel RelatedSurveyConfig { get; private set; }

        public int TotalRows { get; set; }

        public string SurveyConfigId { get; set; }
        public string SurveyName { get; set; }
        public bool SurveyEnable { get; set; }

        public string Group { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }

        public bool StepEnable { get; set; }
        public string StepEnableId { get; set; }

        public SurveyConfigViewModel() { }
       
        public SurveyConfigViewModel(ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel oRelatedSurveyConfig)
        {            
            RelatedSurveyConfig = oRelatedSurveyConfig;

            SurveyConfigId = RelatedSurveyConfig.ItemId.ToString();
            SurveyName = RelatedSurveyConfig.ItemName;
            SurveyEnable = RelatedSurveyConfig.Enable;

            Group = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.Group).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            GroupName = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.Group).
                Select(y => y.ValueName).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            GroupId = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.Group).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();


            StepEnable = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.StepEnable).
                Select(y => y.Value.ToLower() == "true").
                DefaultIfEmpty(false).
                FirstOrDefault();

            StepEnableId = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.StepEnable).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

        }

        public SurveyConfigViewModel(ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel oRelatedSurveyConfig, int oTotalRows)
        {
            TotalRows = oTotalRows;
            RelatedSurveyConfig = oRelatedSurveyConfig;

            SurveyConfigId = RelatedSurveyConfig.ItemId.ToString();
            SurveyName = RelatedSurveyConfig.ItemName;
            SurveyEnable = RelatedSurveyConfig.Enable;

            Group = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.Group).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            GroupName = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.Group).
                Select(y => y.ValueName).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            GroupId = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.Group).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();


            StepEnable = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.StepEnable).
                Select(y => y.Value.ToLower() == "true").
                DefaultIfEmpty(false).
                FirstOrDefault();

            StepEnableId = RelatedSurveyConfig.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumSurveyConfigInfoType.StepEnable).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
        }


    }
}
