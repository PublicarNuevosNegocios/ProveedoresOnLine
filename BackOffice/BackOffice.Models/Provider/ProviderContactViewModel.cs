using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderContactViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedContact { get; private set; }

        public string ContactId { get; set; }

        public string ContactName { get; set; }

        public bool Enable { get; set; }

        public string CC_Value { get; set; }

        public string CC_ValueId { get; set; }

        public string CC_CompanyContactType { get; set; }

        public string CC_CompanyContactTypeId { get; set; }

        public ProviderContactViewModel() { }

        public ProviderContactViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedContact)
        {
            RelatedContact = oRelatedContact;

            #region CompanyContact

            ContactId = RelatedContact.ItemId.ToString();

            ContactName = RelatedContact.ItemName;

            Enable = RelatedContact.Enable;

            CC_Value = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_Value).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CC_ValueId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_Value).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CC_CompanyContactType = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CC_CompanyContactTypeId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

        }
    }
}
