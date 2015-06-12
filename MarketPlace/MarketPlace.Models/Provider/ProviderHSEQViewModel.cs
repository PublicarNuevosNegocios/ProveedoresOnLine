using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderHSEQViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedHSEQInfo { get; private set; }

        public MarketPlace.Models.General.enumHSEQType HSEQType { get { return (MarketPlace.Models.General.enumHSEQType)RelatedHSEQInfo.ItemType.ItemId; } }

        #region Certifications

        private ProveedoresOnLine.Company.Models.Util.GenericItemModel oC_CertificationCompany;
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel C_CertificationCompany
        {
            get
            {
                if (oC_CertificationCompany == null)
                {
                    oC_CertificationCompany = MarketPlace.Models.Company.CompanyUtil.CompanyRule.
                        Where(x => x.ItemId.ToString() == RelatedHSEQInfo.ItemInfo.
                                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_CertificationCompany).
                                        Select(y => y.Value).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault()).
                        FirstOrDefault();
                }
                return oC_CertificationCompany;
            }
        }

        private ProveedoresOnLine.Company.Models.Util.GenericItemModel oC_Rule;
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel C_Rule
        {
            get
            {
                if (oC_Rule == null)
                {
                    oC_Rule = MarketPlace.Models.Company.CompanyUtil.Rule.
                        Where(x => x.ItemId.ToString() == RelatedHSEQInfo.ItemInfo.
                                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_Rule).
                                        Select(y => y.Value).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault()).
                        FirstOrDefault();
                }
                return oC_Rule;
            }
        }

        private string oC_StartDateCertification;
        public string C_StartDateCertification
        {
            get
            {
                if (string.IsNullOrEmpty(oC_StartDateCertification))
                {
                    oC_StartDateCertification = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_StartDateCertification).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oC_StartDateCertification;
            }
        }

        private string oC_EndDateCertification;
        public string C_EndDateCertification
        {
            get
            {
                if (string.IsNullOrEmpty(oC_EndDateCertification))
                {
                    oC_EndDateCertification = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_EndDateCertification).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oC_EndDateCertification;
            }
        }

        private string oC_CCS;
        public string C_CCS
        {
            get
            {
                if (string.IsNullOrEmpty(oC_CCS))
                {
                    oC_CCS = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_CCS).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oC_CCS;
            }
        }

        private string oC_CertificationFile;
        public string C_CertificationFile
        {
            get
            {
                if (string.IsNullOrEmpty(oC_CertificationFile))
                {
                    oC_CertificationFile = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_CertificationFile).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oC_CertificationFile;
            }
        }

        private string oC_Scope;
        public string C_Scope
        {
            get
            {
                if (string.IsNullOrEmpty(oC_Scope))
                {
                    oC_Scope = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_Scope).
                        Select(y => y.LargeValue).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oC_Scope;
            }
        }

        #endregion

        #region HealtyPolitics

        private string oCH_Year;
        public string CH_Year
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_Year))
                {
                    oCH_Year = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_Year).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_Year;
            }
        }

        private string oCH_PoliticsSecurity;
        public string CH_PoliticsSecurity
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_PoliticsSecurity))
                {
                    oCH_PoliticsSecurity = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_PoliticsSecurity).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_PoliticsSecurity;
            }
        }

        private string oCH_PoliticIntegral;
        public string CH_PoliticIntegral
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_PoliticIntegral))
                {
                    oCH_PoliticIntegral = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_PoliticIntegral).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_PoliticIntegral;
            }
        }

        private string oCH_PoliticsNoAlcohol;
        public string CH_PoliticsNoAlcohol
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_PoliticsNoAlcohol))
                {
                    oCH_PoliticsNoAlcohol = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_PoliticsNoAlcohol).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_PoliticsNoAlcohol;
            }
        }

        private string oCH_ProgramOccupationalHealth;
        public string CH_ProgramOccupationalHealth
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_ProgramOccupationalHealth))
                {
                    oCH_ProgramOccupationalHealth = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_ProgramOccupationalHealth).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_ProgramOccupationalHealth;
            }
        }

        private string oCH_RuleIndustrialSecurity;
        public string CH_RuleIndustrialSecurity
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_RuleIndustrialSecurity))
                {
                    oCH_RuleIndustrialSecurity = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_RuleIndustrialSecurity).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_RuleIndustrialSecurity;
            }
        }

        private string oCH_MatrixRiskControl;
        public string CH_MatrixRiskControl
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_MatrixRiskControl))
                {
                    oCH_MatrixRiskControl = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_MatrixRiskControl).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_MatrixRiskControl;
            }
        }

        private string oCH_CorporateSocialResponsability;
        public string CH_CorporateSocialResponsability
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_CorporateSocialResponsability))
                {
                    oCH_CorporateSocialResponsability = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_CorporateSocialResponsability).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_CorporateSocialResponsability;
            }
        }

        private string oCH_ProgramEnterpriseSecurity;
        public string CH_ProgramEnterpriseSecurity
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_ProgramEnterpriseSecurity))
                {
                    oCH_ProgramEnterpriseSecurity = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_ProgramEnterpriseSecurity).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_ProgramEnterpriseSecurity;
            }
        }

        private string oCH_PoliticsRecruiment;
        public string CH_PoliticsRecruiment
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_PoliticsRecruiment))
                {
                    oCH_PoliticsRecruiment = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_PoliticsRecruiment).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_PoliticsRecruiment;
            }
        }

        private string oCH_CertificationsForm;
        public string CH_CertificationsForm
        {
            get
            {
                if (string.IsNullOrEmpty(oCH_CertificationsForm))
                {
                    oCH_CertificationsForm = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_CertificationsForm).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCH_CertificationsForm;
            }
        }

        #endregion

        #region RiskPolicies

        private string oCR_SystemOccupationalHazards;
        public string CR_SystemOccupationalHazards
        {
            get
            {
                if (oCR_SystemOccupationalHazards == null)
                {
                    oCR_SystemOccupationalHazards = RelatedHSEQInfo.ItemInfo.
                                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CR_SystemOccupationalHazards).
                                        Select(y => y.ValueName).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault();
                }
                return oCR_SystemOccupationalHazards;
            }
        }

        private string oCR_RateARL;
        public string CR_RateARL
        {
            get
            {
                if (string.IsNullOrEmpty(oCR_RateARL))
                {
                    oCR_RateARL = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CR_RateARL).
                        Select(y => y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCR_RateARL;
            }
        }

        private string oCR_CertificateAffiliateARL;
        public string CR_CertificateAffiliateARL
        {
            get
            {
                if (string.IsNullOrEmpty(oCR_CertificateAffiliateARL))
                {
                    oCR_CertificateAffiliateARL = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CR_CertificateAffiliateARL).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCR_CertificateAffiliateARL;
            }
        }

        private string oCA_Year;
        public string CA_Year
        {
            get
            {
                if (string.IsNullOrEmpty(oCA_Year))
                {
                    oCA_Year = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_Year).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oCA_Year;
            }
        }

        private string oCA_CertificateAccidentARL;
        public string CA_CertificateAccidentARL
        {
            get
            {
                if (string.IsNullOrEmpty(oCA_CertificateAccidentARL))
                {
                    oCA_CertificateAccidentARL = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_CertificateAccidentARL).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oCA_CertificateAccidentARL;
            }
        }

        private string oCR_LTIFResult;
        public string CR_LTIFResult
        {
            get
            {
                if (string.IsNullOrEmpty(oCR_LTIFResult))
                {
                    oCR_LTIFResult = RelatedHSEQInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CR_LTIFResult).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oCR_LTIFResult;
            }
        }

        #endregion

        public ProviderHSEQViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedCertification)
        {
            RelatedHSEQInfo = RelatedCertification;
        }

        public ProviderHSEQViewModel() { }
    }
}
