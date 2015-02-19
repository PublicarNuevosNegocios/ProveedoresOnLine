using ProveedoresOnLine.Company.Models.Util;
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
        public string CP_UndefinedDate { get; set; }

        #endregion

        #region RUT

        public string R_PersonType { get; set; }

        public bool R_LargeContributor { get; set; }

        public string R_LargeContributorReceipt { get; set; }

        public string R_LargeContributorDate { get; set; }

        public bool R_SelfRetainer { get; set; }

        public string R_SelfRetainerReciept { get; set; }

        public string R_SelfRetainerDate { get; set; }

        public string R_EntityType { get; set; }

        public bool R_IVA { get; set; }

        public string R_TaxPayerType { get; set; }

        public string R_ICAName { get; set; }
        public string R_ICA { get; set; }

        public string R_RUTFile { get; set; }

        public string R_LargeContributorFile { get; set; }

        public string R_SelfRetainerFile { get; set; }


        #endregion

        #region CIFIN

        public string CF_QueryDate { get; set; }
        public string CF_ResultQuery { get; set; }
        public string CF_AutorizationFile { get; set; }

        #endregion

        #region SARLAFT

        public string SF_ProcessDate { get; set; }
        public string SF_PersonType { get; set; }
        public string SF_SARLAFTFile { get; set; }

        #endregion

        #region Resolutions

        public string RS_EntityType { get; set; }
        public string RS_ResolutionFile { get; set; }
        public string RS_StartDate { get; set; }
        public string RS_EndDate { get; set; }
        public string RS_Description { get; set; }

        #endregion

        public ProviderLegalViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal,
                                        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oEconomiActivity,
                                        List<CatalogModel> oEntitieType,
                                        List<CatalogModel> oOptions,
                                        List<GeographyModel> oCities)
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

            CP_InscriptionCity = !string.IsNullOrEmpty(CP_InscriptionCity) ? oCities.Where(x => x.City.ItemId == Convert.ToInt32(CP_InscriptionCity)).Select(x => x.City.ItemName + "/" + x.Country.ItemName).FirstOrDefault() : "N/A";

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

            CP_UndefinedDate = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_UndefinedDate).
                 Select(y => y.Value).
                 DefaultIfEmpty("False").
                 FirstOrDefault();

            #endregion

            #region RUT

            R_PersonType = RelatedLegal.ItemInfo.
                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_PersonType).
                 Select(y => y.Value).
                 DefaultIfEmpty(string.Empty).
                 FirstOrDefault();

            R_PersonType = !string.IsNullOrEmpty(R_PersonType) && oOptions != null && oOptions.Count > 0 ?
              oOptions.Where(x => x.ItemId.ToString() == R_PersonType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            R_LargeContributor = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributor).
                Select(y => (y.Value)).
                FirstOrDefault() == "True" ? true : false;

            R_LargeContributorReceipt = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributorReceipt).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_LargeContributorDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributorDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_SelfRetainer = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainer).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault() == "True" ? true : false;

            R_SelfRetainerReciept = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainerReciept).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_SelfRetainerDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainerDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_EntityType = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_EntityType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_EntityType = !string.IsNullOrEmpty(R_EntityType) && oOptions != null && oOptions.Count > 0 ?
              oOptions.Where(x => x.ItemId.ToString() == R_EntityType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            R_IVA = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_IVA).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault() == "True" ? true : false;

            R_TaxPayerType = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_TaxPayerType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_TaxPayerType = !string.IsNullOrEmpty(R_TaxPayerType) && oOptions != null && oOptions.Count > 0 ?
            oOptions.Where(x => x.ItemId.ToString() == R_TaxPayerType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            R_ICA = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_ICA).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            if (oEconomiActivity != null && oEconomiActivity.Count > 0)
            {
                R_ICAName = oEconomiActivity.
                    Where(x => x.ItemId.ToString() == R_ICA).
                    Select(x => x.ItemName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }

            R_RUTFile = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_RUTFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_LargeContributorFile = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributorFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            R_SelfRetainerFile = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainerFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region CIFIN

            CF_QueryDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CF_QueryDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CF_ResultQuery = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CF_ResultQuery).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CF_AutorizationFile = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CF_AutorizationFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region SARLAFT

            SF_ProcessDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.SF_ProcessDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SF_PersonType = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.SF_PersonType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SF_PersonType = !string.IsNullOrEmpty(SF_PersonType) && oOptions != null && oOptions.Count > 0 ?
             oOptions.Where(x => x.ItemId.ToString() == SF_PersonType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            SF_SARLAFTFile = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.SF_SARLAFTFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region Resolutions

            RS_EntityType = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_EntityType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            RS_EntityType = oEntitieType != null && oEntitieType.Count > 0 ? oEntitieType.Where(x => x.ItemId.ToString() == RS_EntityType).Select(x => x.ItemName).FirstOrDefault() : RS_EntityType;

            RS_ResolutionFile = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_ResolutionFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            RS_StartDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_StartDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            RS_EndDate = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_EndDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            RS_Description = RelatedLegal.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_Description).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion
        }

        public ProviderLegalViewModel() { }
    }
}
