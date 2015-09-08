using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderAditionalDocumentViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAditionalDocument { get; private set; }

        public string AditionalDocumentId { get; set; }

        public string AditionalDocumentName { get; set; }

        public bool Enable { get; set; }

        #region Aditional Documents

        public string AD_Title { get; set; }

        public string AD_File { get; set; }

        public string AD_FileId { get; set; }

        public string AD_RelatedCustomer { get; set; }

        public string AD_RelatedCustomerId { get; set; }

        public string AD_RelatedUser { get; set; }

        public string AD_RelatedUserId { get; set; }

        public string AD_CreateDate { get; set; }

        #endregion

        public ProviderAditionalDocumentViewModel() { }

        public ProviderAditionalDocumentViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedAditionalDocument)
        {
            RelatedAditionalDocument = oRelatedAditionalDocument;

            AditionalDocumentId = RelatedAditionalDocument.ItemId.ToString();
            AditionalDocumentName = RelatedAditionalDocument.ItemName;
            Enable = RelatedAditionalDocument.Enable;

            #region Aditional Document

            AD_FileId = RelatedAditionalDocument.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumAditionalDocumentInfoType.AD_File).
                Select(x => x.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            AD_File = RelatedAditionalDocument.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumAditionalDocumentInfoType.AD_File).
                Select(x => x.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            AD_RelatedCustomerId = RelatedAditionalDocument.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumAditionalDocumentInfoType.AD_RelatedCustomer).
                Select(x => x.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            AD_RelatedCustomer = RelatedAditionalDocument.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumAditionalDocumentInfoType.AD_RelatedCustomer).
                Select(x => x.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            AD_RelatedUserId = RelatedAditionalDocument.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumAditionalDocumentInfoType.AD_RelatedUser).
                Select(x => x.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            AD_RelatedUser = RelatedAditionalDocument.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumAditionalDocumentInfoType.AD_RelatedUser).
                Select(x => x.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            AD_CreateDate = RelatedAditionalDocument.LastModify.ToString();

            AD_Title = RelatedAditionalDocument.ItemName;

            #endregion
        }
    }
}
