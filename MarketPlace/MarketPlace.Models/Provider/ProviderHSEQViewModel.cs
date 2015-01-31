using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderHSEQViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedHSEQInfo { get; set; }

        #region Certifications

        public string C_CertificationCompanyName { get; set; }
        public string C_CertificationCompany { get; set; }
        public string C_RuleName { get; set; }
        public string C_Rule { get; set; }

        public string C_StartDateCertification { get; set; }
        public string C_EndDateCertification { get; set; }
        public string C_CCS { get; set; }
        public string C_CertificationFile { get; set; }
        public string C_Scope { get; set; }

        #endregion

        #region HealtyPolitics

        public string CH_Year { get; set; }
        public string CH_PoliticsSecurity { get; set; }
        public string CH_PoliticsNoAlcohol { get; set; }
        public string CH_ProgramOccupationalHealth { get; set; }
        public string CH_RuleIndustrialSecurity { get; set; }
        public string CH_MatrixRiskControl { get; set; }
        public string CH_CorporateSocialResponsability { get; set; }
        public string CH_ProgramEnterpriseSecurity { get; set; }
        public string CH_PoliticsRecruiment { get; set; }
        public string CH_CertificationsForm { get; set; }

        #endregion

        #region RiskPolicies

        public string CR_SystemOccupationalHazards { get; set; }
        public string CR_SystemOccupationalHazardsName { get; set; }
        public string CR_RateARL { get; set; }
        public string CR_CertificateAffiliateARL { get; set; }

        #endregion

        #region CertificatesAccident

        public string CA_Year { get; set; }       
        public string CA_ManHoursWorked { get; set; }        
        public string CA_Fatalities { get; set; }
        public string CA_NumberAccident { get; set; }
        public string CA_NumberAccidentDisabling { get; set; }        
        public string CA_DaysIncapacity { get; set; }
        public string CA_CertificateAccidentARL { get; set; }
        public string CA_LTIFResult { get; set; }
        public string CA_YearsResult { get; set; }

        #endregion

        public ProviderHSEQViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedCertification,
                                    List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oRule,
                                    List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCompanyRule,
                                    List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oARL,
                                    List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oLTIFResult)
        {
            #region Certifications


            C_CertificationCompany = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_CertificationCompany).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            if (oCompanyRule != null && oCompanyRule.Count > 0)
            {
                C_CertificationCompanyName = oCompanyRule.
                    Where(x => x.ItemId.ToString() == C_CertificationCompany).
                    Select(x => x.ItemName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }

            C_Rule = RelatedCertification.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_Rule).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            if (oRule != null && oRule.Count > 0)
            {
                C_RuleName = oRule.
                    Where(x => x.ItemId.ToString() == C_Rule).
                    Select(x => x.ItemName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }

            C_StartDateCertification = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_StartDateCertification).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();


            C_EndDateCertification = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_EndDateCertification).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_CCS = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_CCS).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();


            C_CertificationFile = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_CertificationFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_Scope = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.C_Scope).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region HealtyPolitics

            CH_Year = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_Year).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsSecurity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_PoliticsSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsNoAlcohol = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_PoliticsNoAlcohol).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_ProgramOccupationalHealth = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_ProgramOccupationalHealth).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_RuleIndustrialSecurity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_RuleIndustrialSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_MatrixRiskControl = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_MatrixRiskControl).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_CorporateSocialResponsability = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_CorporateSocialResponsability).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_ProgramEnterpriseSecurity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_ProgramEnterpriseSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsRecruiment = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_PoliticsRecruiment).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_CertificationsForm = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CH_CertificationsForm).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();


            #endregion

            #region RiskPolicies

            CR_SystemOccupationalHazards = oLTIFResult.
               Where(y => y.ItemType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CR_SystemOccupationalHazards).
               Select(y => y.ItemType.).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
          
            if (oARL != null && oARL.Count > 0)
            {
                CR_SystemOccupationalHazardsName = oARL.
                    Where(x => x.ItemId.ToString() == CR_SystemOccupationalHazards).
                    Select(x => x.ItemName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }

            CR_RateARL = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CR_RateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
        

            CR_CertificateAffiliateARL = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CR_CertificateAffiliateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
         
            #endregion

            #region CertificatesAcccident

            //CA_Year = oLTIFResult.
            //    Where(y => y.ItemInfo.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_Year).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            CA_ManHoursWorked = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_ManHoursWorked).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
         
            CA_Fatalities = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_Fatalities).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CA_NumberAccident = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_NumberAccident).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CA_NumberAccidentDisabling = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_NumberAccidentDisabling).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CA_DaysIncapacity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_DaysIncapacity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();           

            CA_CertificateAccidentARL = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_CertificateAccidentARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
           
            #endregion

        }

        public ProviderHSEQViewModel() { }
    }
}
