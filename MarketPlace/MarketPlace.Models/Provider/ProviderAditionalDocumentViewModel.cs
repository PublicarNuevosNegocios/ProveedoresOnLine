using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderAditionalDocumentViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAditionalDocument { get; private set; }

        #region Aditional Document

        private string oAD_Title;

        public string AD_Title
        {
            get
            {
                if (oAD_Title == null)
                {
                    oAD_Title = RelatedAditionalDocument.ItemName.ToString();
                }
                return oAD_Title;
            }
        }

        private string oAD_File;

        public string AD_File
        {
            get
            {
                if (oAD_File == null)
                {
                    oAD_File = RelatedAditionalDocument.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumAditionalDocumentInfoType.AD_File).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oAD_File;
            }
        }

        private string oAD_RelatedCustomer;

        public string AD_RelatedCustomer
        {
            get
            {
                if (oAD_RelatedCustomer == null)
                {
                    oAD_RelatedCustomer = RelatedAditionalDocument.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumAditionalDocumentInfoType.AD_RelatedCustomer).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oAD_RelatedCustomer;
            }
        }

        private string oAD_RelatedUser;

        public string AD_RelatedUser
        {
            get
            {
                if (oAD_RelatedUser == null)
                {
                    oAD_RelatedUser = RelatedAditionalDocument.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumAditionalDocumentInfoType.AD_RelatedUser).
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oAD_RelatedUser;
            }
        }

        private string oAD_UploadDate;

        public string AD_UploadDate
        {
            get
            {
                if (oAD_UploadDate == null)
                {
                    oAD_UploadDate = RelatedAditionalDocument.CreateDate.ToString();
                }

                return oAD_UploadDate;
            }
        }

        #endregion

        public ProviderAditionalDocumentViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedAditionalDocument)
        {
            RelatedAditionalDocument = oRelatedAditionalDocument;
        }

        public ProviderAditionalDocumentViewModel() { }
    }
}
