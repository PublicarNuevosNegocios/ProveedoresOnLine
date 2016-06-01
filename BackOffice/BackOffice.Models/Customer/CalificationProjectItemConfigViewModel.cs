using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class CalificationProjectItemConfigViewModel
    {
        public ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemModel RelatedCalificationProjectConfigItemModel { get; private set; }

        public string CalificationProjectConfigItemId { get; set; }

        public string CalificationProjectConfigItemName { get; set; }

        public string CalificationProjectModule { get; set; }

        public bool Enable { get; set; }

        public CalificationProjectItemConfigViewModel() { }

        public CalificationProjectItemConfigViewModel(ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemModel oRelatedCalificationProjectConfigItemModel)
        {
            RelatedCalificationProjectConfigItemModel = oRelatedCalificationProjectConfigItemModel;

            CalificationProjectConfigItemId = RelatedCalificationProjectConfigItemModel.CalificationProjectConfigItemId.ToString();

            CalificationProjectConfigItemName = RelatedCalificationProjectConfigItemModel.CalificationProjectConfigItemName;

            CalificationProjectModule = RelatedCalificationProjectConfigItemModel.CalificationProjectConfigItemType.ItemId.ToString();

            Enable = RelatedCalificationProjectConfigItemModel.Enable;
        }
    }
}
