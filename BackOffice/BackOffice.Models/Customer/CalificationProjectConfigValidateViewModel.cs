using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class CalificationProjectConfigValidateViewModel
    {
        public ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel RelatedCalificationProjectConfigValidateModel { get; set; }
        public int CalificationProjectConfigValidateId { get; set; }
        public int CalificationProjectConfigId { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string Result { get; set; }
        public bool Enable { get; set; }

        public CalificationProjectConfigValidateViewModel() { }
        public CalificationProjectConfigValidateViewModel(ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigValidateModel oRelatedCalificationProjectConfigValidateModel) 
        {
            RelatedCalificationProjectConfigValidateModel = oRelatedCalificationProjectConfigValidateModel;
            CalificationProjectConfigId = RelatedCalificationProjectConfigValidateModel.CalificationProjectConfigId;
            CalificationProjectConfigValidateId = RelatedCalificationProjectConfigValidateModel.CalificationProjectConfigValidateId;
            Operator = RelatedCalificationProjectConfigValidateModel.Operator.ItemId.ToString();
            Value = RelatedCalificationProjectConfigValidateModel.Value.ToString();
            Result = RelatedCalificationProjectConfigValidateModel.Result.ToString();
            Enable = RelatedCalificationProjectConfigValidateModel.Enable;
        }
    }
}
