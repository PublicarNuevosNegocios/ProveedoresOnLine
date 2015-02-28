using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderDesignationsViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegalInfo { get; set; }

        #region Designations

        public string oCD_PartnerName { get; set; }
        public string CD_PartnerName
        {
            get
            {
                if (string.IsNullOrEmpty(oCD_PartnerName))
                {
                    oCD_PartnerName = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CD_PartnerName).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCD_PartnerName;
            }
        }

        public string oCD_PartnerIdentificationNumber { get; set; }
        public string CD_PartnerIdentificationNumber
        {
            get
            {
                if (string.IsNullOrEmpty(oCD_PartnerIdentificationNumber))
                {
                    oCD_PartnerIdentificationNumber = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CD_PartnerIdentificationNumber).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCD_PartnerIdentificationNumber;
            }
        }

        public string oCD_PartnerRank { get; set; }
        public string CD_PartnerRank
        {
            get
            {
                if (string.IsNullOrEmpty(oCD_PartnerRank))
                {
                    oCD_PartnerRank = MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CD_PartnerRank).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault());
                }
                return oCD_PartnerRank;
            }
        }

        #endregion

        public ProviderDesignationsViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal)
        {
            RelatedLegalInfo = RelatedLegal;         
        }

        public ProviderDesignationsViewModel() { }
    }
}
