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

        public int AditionalDataTypeId { get; set; }

        public string AditionalDataType { get; set; }

        public int ModuleId { get; set; }

        public string Module { get; set; }

        public string Title { get; set; }

        public bool RenderScripts { get; set; }

        public bool Enable { get; set; }

        public AditionalDocumentsViewModel() { }

        public AditionalDocumentsViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oAditionalData)
        {
            AditionalDataId = oAditionalData.ItemId.ToString();
            Enable = oAditionalData.Enable;
            Title = oAditionalData.ItemName;

            AditionalDataTypeId = oAditionalData.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCompanyInfoType.AditionalDocumentType).
                Select(x => x.ItemInfoId).
                DefaultIfEmpty(0).
                FirstOrDefault();

            AditionalDataType = oAditionalData.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCompanyInfoType.AditionalDocumentType).
                Select(x => x.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            ModuleId = oAditionalData.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCompanyInfoType.ModuleType).
                Select(x => x.ItemInfoId).
                DefaultIfEmpty(0).
                FirstOrDefault();

            Module = oAditionalData.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCompanyInfoType.ModuleType).
                Select(x => x.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
        }
    }
}
