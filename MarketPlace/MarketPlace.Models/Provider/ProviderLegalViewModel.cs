using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderLegalViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegalInfo { get; set; }

        #region ChaimberOfComerce

        public string CP_ConstitutionDate { get; set; }      
        public string CP_ConstitutionEndDate { get; set; }
        public string CP_State { get; set; }
        public string CP_InscriptionCity { get; set; }
        public string CP_InscriptionNumber { get; set; }
        public string CP_ExistenceAndLegalPersonCertificate { get; set; }
        public string CP_CertificateExpeditionDate { get; set; }
        public string CP_SocialObject { get; set; }
        public string CP_PartnerNameItem { get; set; }
        public string CP_PartnerIdentificationNumberItem { get; set; }
        public string CP_PartnerRankItem { get; set; }
        
        #endregion

        #region Designations

        public string CD_PartnerName { get; set; }
        public string CD_PartnerIdentificationNumber { get; set; }
        public string CD_PartnerRank { get; set; }
        
        #endregion

        public ProviderLegalViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal, List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oEconomiActivity)
        {
            #region ChaimberOfComerce
            CP_ConstitutionDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_ConstitutionDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();       

            CP_ConstitutionEndDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_ConstitutionEndDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();          

            CP_State = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_State).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_InscriptionCity = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_InscriptionCity).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_InscriptionNumber = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_InscriptionNumber).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_ExistenceAndLegalPersonCertificate = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_CertificateExpeditionDate = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_CertificateExpeditionDate).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_SocialObject = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_SocialObject).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            #endregion

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

        public ProviderLegalViewModel() { }
    }
}
