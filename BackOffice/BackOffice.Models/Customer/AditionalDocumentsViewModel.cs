using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class AditionalDocumentsViewModel
    {
        public string AditionalDataId { get; set; }

        public string AditionalDataTypeId { get; set; }

        public string AditionalDataType { get; set; }

        public string Title { get; set; }

        public bool RenderScripts { get; set; }

        public bool Enable { get; set; }

        public AditionalDocumentsViewModel() { }

        public AditionalDocumentsViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oAditionalData)
        {
            AditionalDataId = oAditionalData.ItemId.ToString();
            AditionalDataTypeId = oAditionalData.ItemType.ItemId.ToString();
            AditionalDataType = oAditionalData.ItemType.ItemName;
            Title = oAditionalData.ItemInfo.Where(x => x.Value != null).Select(x => x.Value).DefaultIfEmpty(string.Empty).FirstOrDefault();
        }
    }
}
