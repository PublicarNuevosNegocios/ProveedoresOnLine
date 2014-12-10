
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderLegalViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal { get; private set; }

        public string LegalId { get; set; }

        public string LegalName { get; set; }

        public bool Enable { get; set; }

        #region ChaimberOfComerce

        public string CP_ConstitutionDate { get; set; }
        public string CP_ConstitutionDateId { get; set; }

        public string CP_ConstitutionEndDate { get; set; }
        public string CP_ConstitutionEndDateId { get; set; }

        public string CP_State { get; set; }
        public string CP_StateId { get; set; }

        public string CP_InscriptionCity { get; set; }
        public string CP_InscriptionCityId { get; set; }

        public string CP_InscriptionNumber { get; set; }
        public string CP_InscriptionNumberId { get; set; }

        public string CP_ExistenceAndLegalPersonCertificate { get; set; }
        public string CP_ExistenceAndLegalPersonCertificateId { get; set; }

        public string CP_CertificateExpeditionDate { get; set; }
        public string CP_CertificateExpeditionDateId { get; set; }

        public string CP_SocialObject { get; set; }
        public string CP_SocialObjectId { get; set; }

        public string CP_PartnerName { get; set; }
        public string CP_PartnerNameId { get; set; }

        public string CP_PartnerIdentificationNumber { get; set; }
        public string CP_PartnerIdentificationNumberId { get; set; }

        public string CP_PartnerRank { get; set; }
        public string CP_PartnerRankId { get; set; }
        #endregion

        #region RUT

        #endregion

        #region CIFIN

        #endregion

        #region SARLAFT

        #endregion

        #region Resolutions

        #endregion

        public ProviderLegalViewModel() { }

        public ProviderLegalViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedLegal)
        {
            RelatedLegal = oRelatedLegal;

            LegalId = oRelatedLegal.ItemId.ToString();

            LegalName = oRelatedLegal.ItemName;

            Enable = oRelatedLegal.Enable;

            #region ChaimberOfComerce

            CP_ConstitutionDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_ConstitutionDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_ConstitutionDateId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_ConstitutionDate).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_ConstitutionEndDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_ConstitutionEndDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_ConstitutionEndDateId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_ConstitutionEndDate).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_State = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_State).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_StateId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_State).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_InscriptionCity = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_InscriptionCity).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_InscriptionCityId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_InscriptionCity).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_InscriptionNumber = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_InscriptionNumber).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_InscriptionNumberId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_InscriptionNumber).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_ExistenceAndLegalPersonCertificate = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_ExistenceAndLegalPersonCertificateId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_CertificateExpeditionDate = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_CertificateExpeditionDate).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_CertificateExpeditionDateId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_CertificateExpeditionDate).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_SocialObject = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_SocialObject).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_SocialObjectId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_SocialObject).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_PartnerName = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerName).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_PartnerNameId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerName).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_PartnerIdentificationNumber = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerIdentificationNumber).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_PartnerIdentificationNumberId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerIdentificationNumber).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_PartnerRank = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerRank).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CP_PartnerRankId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CP_PartnerRank).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();
            #endregion

            #region CIFIN

            #endregion

            #region SARLAFT

            #endregion

            #region Resolutions

            #endregion

        }
    }
}
