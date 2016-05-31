using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class CalificationProjectConfigViewModel
    {
        public ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel RelatedCalificationProjectConfigModel { get; set; }

        public string CalificationProjectConfigId { get; set; }

        public string CalificationProjectConfigName { get; set; }

        public bool Enable { get; set; }

        public CalificationProjectConfigViewModel() { }

        public CalificationProjectConfigViewModel(ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel oRelatedCalificationProjectConfigModel)
        {
            RelatedCalificationProjectConfigModel = oRelatedCalificationProjectConfigModel;

            CalificationProjectConfigId = RelatedCalificationProjectConfigModel.CalificationProjectConfigId.ToString();

            CalificationProjectConfigName = RelatedCalificationProjectConfigModel.CalificationProjectConfigName;

            Enable = RelatedCalificationProjectConfigModel.Enable;
        }
    }
}
