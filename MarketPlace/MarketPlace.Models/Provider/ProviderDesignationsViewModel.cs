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

        public string CD_PartnerName { get; set; }
        public string CD_PartnerIdentificationNumber { get; set; }
        public string CD_PartnerRank { get; set; }
        
        #endregion

        public ProviderDesignationsViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal)
        {
            #region Designations

            CD_PartnerName = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CD_PartnerName).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();
   
            CD_PartnerIdentificationNumber = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CD_PartnerIdentificationNumber).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
               FirstOrDefault();
        
            CD_PartnerRank = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CD_PartnerRank).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            #endregion
        }

        public ProviderDesignationsViewModel() { }
    }
}
