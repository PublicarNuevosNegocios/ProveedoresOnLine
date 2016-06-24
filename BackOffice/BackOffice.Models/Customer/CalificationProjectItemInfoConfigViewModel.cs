using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class CalificationProjectItemInfoConfigViewModel
    {
        public ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemInfoModel RelatedCalificationProjectConfigItemInfoModel { get; private set; }

        public string CalificationProjectConfigItemInfoId { get; set; }

        public string Question { get; set; }

        public string QuestionName { get; set; }

        public string Rule { get; set; }

        public string ValueType { get; set; }

        public string Value { get; set; }

        public string Score { get; set; }

        public bool Enable { get; set; }

        public CalificationProjectItemInfoConfigViewModel() { }

        public CalificationProjectItemInfoConfigViewModel(ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemInfoModel oRelatedCalificationProjectConfigItemInfoModel)
        {
            RelatedCalificationProjectConfigItemInfoModel = oRelatedCalificationProjectConfigItemInfoModel;

            CalificationProjectConfigItemInfoId = RelatedCalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId.ToString();

            Question = RelatedCalificationProjectConfigItemInfoModel.Question.ItemId.ToString();

            Rule = RelatedCalificationProjectConfigItemInfoModel.Rule.ItemId.ToString();

            ValueType = RelatedCalificationProjectConfigItemInfoModel.ValueType.ItemId.ToString();

            Value = RelatedCalificationProjectConfigItemInfoModel.Value;

            Score = RelatedCalificationProjectConfigItemInfoModel.Score;

            Enable = RelatedCalificationProjectConfigItemInfoModel.Enable;
        }
    }
}
