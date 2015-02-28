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

        public string oCP_ConstitutionDate { get; set; }
        public string CP_ConstitutionDate
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_ConstitutionDate))
                {
                    oCP_ConstitutionDate = RelatedLegalInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_ConstitutionDate).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCP_ConstitutionDate;
            }
        }

        public string oCP_ConstitutionEndDate { get; set; }
        public string CP_ConstitutionEndDate
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_ConstitutionEndDate))
                {
                    oCP_ConstitutionEndDate = RelatedLegalInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_ConstitutionEndDate).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCP_ConstitutionEndDate;
            }
        }

        public string oCP_State { get; set; }
        public string CP_State
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_State))
                {
                    oCP_State = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_State).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCP_State;
            }
        }

        public string oCP_InscriptionCity { get; set; }
        public string CP_InscriptionCity
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_InscriptionCity))
                {
                    oCP_InscriptionCity = oCP_InscriptionCity = MarketPlace.Models.Company.CompanyUtil.GetCityName(RelatedLegalInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_InscriptionCity).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault());
                }
                return oCP_InscriptionCity;
            }
        }

        public string oCP_InscriptionNumber { get; set; }
        public string CP_InscriptionNumber
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_InscriptionNumber))
                {
                    oCP_InscriptionNumber = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_InscriptionNumber).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCP_InscriptionNumber;
            }
        }

        public string oCP_ExistenceAndLegalPersonCertificate { get; set; }
        public string CP_ExistenceAndLegalPersonCertificate
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_ExistenceAndLegalPersonCertificate))
                {
                    oCP_ExistenceAndLegalPersonCertificate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_ExistenceAndLegalPersonCertificate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCP_ExistenceAndLegalPersonCertificate;
            }
        }

        public string oCP_CertificateExpeditionDate { get; set; }
        public string CP_CertificateExpeditionDate
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_CertificateExpeditionDate))
                {
                    oCP_CertificateExpeditionDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_CertificateExpeditionDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCP_CertificateExpeditionDate;
            }
        }

        public string oCP_SocialObject { get; set; }
        public string CP_SocialObject
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_SocialObject))
                {
                    oCP_SocialObject = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_SocialObject).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCP_SocialObject;
            }
        }

        public string oCP_PartnerNameItem { get; set; }
        public string CP_PartnerNameItem
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_PartnerNameItem))
                {
                    oCP_PartnerNameItem = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CD_PartnerName).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCP_PartnerNameItem;
            }
        }

        public string oCP_PartnerIdentificationNumberItem { get; set; }
        public string CP_PartnerIdentificationNumberItem { get; set; }

        public string oCP_PartnerRankItem { get; set; }
        public string CP_PartnerRankItem { get; set; }

        public string oCP_UndefinedDate { get; set; }
        public string CP_UndefinedDate
        {
            get
            {
                if (string.IsNullOrEmpty(oCP_UndefinedDate))
                {
                    oCP_UndefinedDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CP_UndefinedDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCP_UndefinedDate;
            }
        }

        #endregion

        #region RUT

        public string oR_PersonType { get; set; }
        public string R_PersonType
        {
            get
            {
                if (string.IsNullOrEmpty(oR_PersonType))
                {
                    oR_PersonType =  MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(RelatedLegalInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_PersonType).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault());
                }
                return oR_PersonType;
            }
        }

        public bool oR_LargeContributor { get; set; }
        public bool R_LargeContributor
        {
            get
            {
                return oR_LargeContributor = RelatedLegalInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributor).
                        Select(y => (y.Value)).
                        FirstOrDefault() == "True" ? true : false;
            }
        }

        public string oR_LargeContributorReceipt { get; set; }
        public string R_LargeContributorReceipt
        {
            get
            {
                if (string.IsNullOrEmpty(oR_LargeContributorReceipt))
                {
                    oR_LargeContributorReceipt = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributorReceipt).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oR_LargeContributorReceipt;
            }
        }

        public string oR_LargeContributorDate { get; set; }
        public string R_LargeContributorDate
        {
            get
            {
                if (string.IsNullOrEmpty(oR_LargeContributorDate))
                {
                    oR_LargeContributorDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributorDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oR_LargeContributorDate;
            }
        }

        public bool oR_SelfRetainer { get; set; }
        public bool R_SelfRetainer
        {
            get
            {
                return oR_SelfRetainer = RelatedLegalInfo.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainer).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault() == "True" ? true : false;
            }
        }

        public string oR_SelfRetainerReciept { get; set; }
        public string R_SelfRetainerReciept
        {
            get
            {
                if (string.IsNullOrEmpty(oR_SelfRetainerReciept))
                {
                    oR_SelfRetainerReciept = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainerReciept).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oR_SelfRetainerReciept;
            }
        }

        public string oR_SelfRetainerDate { get; set; }
        public string R_SelfRetainerDate
        {
            get
            {
                if (string.IsNullOrEmpty(oR_SelfRetainerDate))
                {
                    oR_SelfRetainerDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainerDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oR_SelfRetainerDate;
            }
        }

        public string oR_EntityType { get; set; }
        public string R_EntityType
        {
            get
            {
                if (string.IsNullOrEmpty(oR_EntityType))
                {
                    oR_EntityType = MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_EntityType).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault());
                }
                return oR_EntityType;
            }
        }

        public bool oR_IVA { get; set; }
        public bool R_IVA
        {
            get
            {                
                return oR_IVA = RelatedLegalInfo.ItemInfo.
                    Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_IVA).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault() == "True" ? true : false;                
            }
        }

        public string oR_TaxPayerType { get; set; }
        public string R_TaxPayerType
        {
            get
            {
                if (string.IsNullOrEmpty(oR_TaxPayerType))
                {
                    oR_TaxPayerType = MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_TaxPayerType).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault());
                }
                return oR_TaxPayerType;
            }
        }

        public string oR_ICAName { get; set; }
        public string R_ICAName
        {
            get
            {
                if (string.IsNullOrEmpty(oR_ICAName))
                {
                    oR_ICAName = MarketPlace.Models.Company.CompanyUtil.GetICAName(RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_ICA).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault());
                }
                return oR_ICAName;
            }
        }      

        public string oR_RUTFile { get; set; }
        public string R_RUTFile
        {
            get
            {
                if (string.IsNullOrEmpty(oR_RUTFile))
                {
                    oR_RUTFile = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_RUTFile).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oR_RUTFile;
            }
        }

        public string oR_LargeContributorFile { get; set; }
        public string R_LargeContributorFile
        {
            get
            {
                if (string.IsNullOrEmpty(oR_LargeContributorFile))
                {
                    oR_LargeContributorFile = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_LargeContributorFile).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oR_LargeContributorFile;
            }
        }

        public string oR_SelfRetainerFile { get; set; }
        public string R_SelfRetainerFile
        {
            get
            {
                if (string.IsNullOrEmpty(oR_SelfRetainerFile))
                {
                    oR_SelfRetainerFile = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.R_SelfRetainerFile).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oR_SelfRetainerFile;
            }
        }


        #endregion

        #region CIFIN

        public string oCF_QueryDate { get; set; }
        public string CF_QueryDate
        {
            get
            {
                if (string.IsNullOrEmpty(oCF_QueryDate))
                {
                    oCF_QueryDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CF_QueryDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCF_QueryDate;
            }
        }

        public string oCF_ResultQuery { get; set; }
        public string CF_ResultQuery
        {
            get
            {
                if (string.IsNullOrEmpty(oCF_ResultQuery))
                {
                    oCF_ResultQuery = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CF_ResultQuery).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCF_ResultQuery;
            }
        }

        public string oCF_AutorizationFile { get; set; }
        public string CF_AutorizationFile
        {
            get
            {
                if (string.IsNullOrEmpty(oCF_AutorizationFile))
                {
                    oCF_AutorizationFile = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.CF_AutorizationFile).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oCF_AutorizationFile;
            }
        }

        #endregion

        #region SARLAFT

        public string oSF_ProcessDate { get; set; }
        public string SF_ProcessDate
        {
            get
            {
                if (string.IsNullOrEmpty(oSF_ProcessDate))
                {
                    oSF_ProcessDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.SF_ProcessDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oSF_ProcessDate;
            }
        }

        public string oSF_PersonType { get; set; }
        public string SF_PersonType
        {
            get
            {
                if (string.IsNullOrEmpty(oSF_PersonType))
                {
                    oSF_PersonType =  MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.SF_ProcessDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault());
                }
                return oSF_PersonType;
            }
        }

        public string oSF_SARLAFTFile { get; set; }
        public string SF_SARLAFTFile
        {
            get
            {
                if (string.IsNullOrEmpty(oSF_SARLAFTFile))
                {
                    oSF_SARLAFTFile = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.SF_SARLAFTFile).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oSF_SARLAFTFile;
            }
        }

        #endregion

        #region Resolutions

        public string oRS_EntityType { get; set; }
        public string RS_EntityType
        {
            get
            {
                if (string.IsNullOrEmpty(oRS_EntityType))
                {
                    oRS_EntityType =  MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_EntityType).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault());
                }
                return oRS_EntityType;
            }
        }

        public string oRS_ResolutionFile { get; set; }
        public string RS_ResolutionFile
        {
            get
            {
                if (string.IsNullOrEmpty(oRS_ResolutionFile))
                {
                    oRS_ResolutionFile = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_ResolutionFile).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRS_ResolutionFile;
            }
        }

        public string oRS_StartDate { get; set; }
        public string RS_StartDate
        {
            get
            {
                if (string.IsNullOrEmpty(oRS_StartDate))
                {
                    oRS_StartDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_StartDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRS_StartDate;
            }
        }

        public string oRS_EndDate { get; set; }
        public string RS_EndDate
        {
            get
            {
                if (string.IsNullOrEmpty(oRS_EndDate))
                {
                    oRS_EndDate = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_EndDate).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRS_EndDate;
            }
        }

        public string oRS_Description { get; set; }
        public string RS_Description
        {
            get
            {
                if (string.IsNullOrEmpty(oRS_Description))
                {
                    oRS_Description = RelatedLegalInfo.ItemInfo.
                         Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumLegalInfoType.RS_Description).
                         Select(y => y.Value).
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault();
                }
                return oRS_Description;
            }
        }

        #endregion

        public ProviderLegalViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal)
        {            
            RelatedLegalInfo = RelatedLegal;                          
        }

        public ProviderLegalViewModel() { }
    }
}
