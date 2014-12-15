
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

        public string CP_PartnerNameItem { get; set; }
        public string CP_PartnerNameItemId { get; set; }

        public string CP_PartnerIdentificationNumberItem { get; set; }
        public string CP_PartnerIdentificationNumberItemId { get; set; }
                
        public string CP_PartnerRankItem { get; set; }
        public string CP_PartnerRankItemId { get; set; }
        #endregion

        #region Designations

        public string CD_PartnerName { get; set; }
        public string CD_PartnerNameId { get; set; }

        public string CD_PartnerIdentificationNumber { get; set; }
        public string CD_PartnerIdentificationNumberId { get; set; }

        public string CD_PartnerRank { get; set; }
        public string CD_PartnerRankId { get; set; }
        #endregion

        #region RUT

        public string R_PersonType { get; set; }
        public string R_LargeContributor { get; set; }
        public string R_LargeContributorReceipt { get; set; }
        public string R_LargeContributorDate { get; set; }
        public string R_SelfRetainer { get; set; }
        public string R_SelfRetainerReciept { get; set; }
        public string R_SelfRetainerDate { get; set; }
        public string R_EntityType { get; set; }
        public string R_IVA { get; set; }
        public string R_TaxPayerType { get; set; }
        public string R_ICA { get; set; }
        public string R_RUTFile { get; set; }
        public string R_LargeContributorFile { get; set; }
        public string R_SelfRetainerFile { get; set; }

        #endregion

        #region CIFIN

        #endregion

        #region SARLAFT

        #endregion

        #region Resolutions

        #endregion

        public ProviderLegalViewModel() { }

        public ProviderLegalViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedLegal)
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

            #endregion

            #region Designations

            CD_PartnerName = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerName).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CD_PartnerNameId = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerName).
               Select(y => y.ItemInfoId.ToString()).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CD_PartnerIdentificationNumber = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerIdentificationNumber).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CD_PartnerIdentificationNumberId = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerIdentificationNumber).
                 Select(y => y.ItemInfoId.ToString()).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CD_PartnerRank = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerRank).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            CD_PartnerRankId = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.CD_PartnerRank).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            #endregion

            #region RUT

            R_PersonType = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_PersonType).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();
            R_LargeContributor = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributor).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();
            R_LargeContributorReceipt = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributorReceipt).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            R_LargeContributorDate = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributorDate).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_SelfRetainer = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainer).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_SelfRetainerReciept = RelatedLegal.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainerReciept).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();
            R_SelfRetainerDate = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainerDate).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_EntityType = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_EntityType).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_IVA = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_IVA).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_TaxPayerType = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_TaxPayerType).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_ICA = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_ICA).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_RUTFile = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_RUTFile).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_LargeContributorFile = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_LargeContributorFile).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            R_SelfRetainerFile = RelatedLegal.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumLegalInfoType.R_SelfRetainerFile).
               Select(y => y.Value).
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
