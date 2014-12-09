using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderHSEQViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedCertification { get; private set; }

        public string CertificationId { get; set; }

        public string CertificationName { get; set; }

        public bool Enable { get; set; }

        #region Certifications

        public string C_CertificationCompany { get; set; }
        public string C_CertificationCompanyId { get; set; }

        public string C_Rule { get; set; }
        public string C_RuleId { get; set; }

        public string C_StartDateCertification { get; set; }
        public string C_StartDateCertificationId { get; set; }

        public string C_EndDateCertification { get; set; }
        public string C_EndDateCertificationId { get; set; }

        public string C_CCS { get; set; }
        public string C_CCSId { get; set; }

        public string C_CertificationFile { get; set; }
        public string C_CertificationFileId { get; set; }

        public string C_Scope { get; set; }
        public string C_ScopeId { get; set; }

        #endregion

        #region HealtyPolitics

        public string CH_Year { get; set; }
        public string CH_YearId { get; set; }

        public string CH_PoliticsSecurity { get; set; }
        public string CH_PoliticsSecurityId { get; set; }

        public string CH_PoliticsNoAlcohol { get; set; }
        public string CH_PoliticsNoAlcoholId { get; set; }

        public string CH_ProgramOccupationalHealth { get; set; }
        public string CH_ProgramOccupationalHealthId { get; set; }

        public string CH_RuleIndustrialSecurity { get; set; }
        public string CH_RuleIndustrialSecurityId { get; set; }

        public string CH_MatrixRiskControl { get; set; }
        public string CH_MatrixRiskControlId { get; set; }
        
        public string CH_CorporateSocialResponsability { get; set; }
        public string CH_CorporateSocialResponsabilityId { get; set; }

        public string CH_ProgramEnterpriseSecurity { get; set; }
        public string CH_ProgramEnterpriseSecurityId { get; set; }

        public string CH_PoliticsRecruiment { get; set; }
        public string CH_PoliticsRecruimentId { get; set; }

        public string CH_CertificationsForm { get; set; }
        public string CH_CertificationsFormId { get; set; }

        #endregion

        #region RiskPolicies

        public string CR_SystemOccupationalHazards { get; set; }
        public string CR_SystemOccupationalHazardsId { get; set; }
        
        public string CR_RateARL { get; set; }
        public string CR_RateARLId { get; set; }
        
        public string CR_CertificateAffiliateARL { get; set; }
        public string CR_CertificateAffiliateARLId { get; set; }
        
        public string CR_CertificateAccidentARL { get; set; }
        public string CR_CertificateAccidentARLId { get; set; }
        
        public string CR_Year { get; set; }
        public string CR_YearId { get; set; }
        
        public string CR_ManHoursWorked { get; set; }
        public string CR_ManHoursWorkedId { get; set; }
        
        public string CR_Fatalities { get; set; }
        public string CR_FatalitiesId { get; set; }
        
        public string CR_NumberAccident { get; set; }
        public string CR_NumberAccidentId { get; set; }
                
        public string CR_NumberAccidentDisabling { get; set; }
        public string CR_NumberAccidentDisablingId { get; set; }
                
        public string CR_DaysIncapacity { get; set; }
        public string CR_DaysIncapacityId { get; set; }

        #endregion

        public ProviderHSEQViewModel() { }

        public ProviderHSEQViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedCertification)
        {
            RelatedCertification = oRelatedCertification;
            CertificationId = RelatedCertification.ItemId.ToString();
            CertificationName = RelatedCertification.ItemName;
            Enable = RelatedCertification.Enable;

            #region Certifications

            C_CertificationCompany = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_CertificationCompany).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_CertificationCompanyId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_CertificationCompany).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_Rule = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_Rule).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_RuleId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_Rule).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_StartDateCertification = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_StartDateCertification).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_StartDateCertificationId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_StartDateCertification).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_EndDateCertification = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_EndDateCertification).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_EndDateCertificationId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_EndDateCertification).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_CCS = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_CCS).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_CCSId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_CCS).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_CertificationFile = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_CertificationFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_CertificationFileId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_CertificationFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_Scope = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_Scope).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            C_ScopeId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.C_Scope).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region HealtyPolitics

            CH_Year = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_Year).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_YearId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_Year).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsSecurity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsSecurityId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsNoAlcohol = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsNoAlcohol).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsNoAlcoholId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsNoAlcohol).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_ProgramOccupationalHealth = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_ProgramOccupationalHealth).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_ProgramOccupationalHealthId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_ProgramOccupationalHealth).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_RuleIndustrialSecurity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_RuleIndustrialSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_RuleIndustrialSecurityId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_RuleIndustrialSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_MatrixRiskControl = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_MatrixRiskControl).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_MatrixRiskControlId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_MatrixRiskControl).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_CorporateSocialResponsability = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_CorporateSocialResponsability).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_CorporateSocialResponsabilityId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_CorporateSocialResponsability).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_ProgramEnterpriseSecurity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_ProgramEnterpriseSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_ProgramEnterpriseSecurityId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_ProgramEnterpriseSecurity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsRecruiment = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsRecruiment).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_PoliticsRecruimentId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_PoliticsRecruiment).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_CertificationsForm = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_CertificationsForm).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CH_CertificationsFormId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CH_CertificationsForm).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region RiskPolicies

            CR_SystemOccupationalHazards = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_SystemOccupationalHazards).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_SystemOccupationalHazardsId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_SystemOccupationalHazards).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_RateARL = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_RateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_RateARLId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_RateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_CertificateAffiliateARL = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_CertificateAffiliateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_CertificateAffiliateARLId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_CertificateAffiliateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_CertificateAccidentARL = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_CertificateAffiliateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_CertificateAccidentARLId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_CertificateAffiliateARL).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_Year = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_Year).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_YearId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_Year).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_ManHoursWorked = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_ManHoursWorked).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_ManHoursWorkedId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_ManHoursWorked).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_Fatalities = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_Fatalities).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_FatalitiesId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_Fatalities).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_NumberAccident = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_NumberAccident).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_NumberAccidentId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_NumberAccident).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_NumberAccidentDisabling = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_NumberAccidentDisabling).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_NumberAccidentDisablingId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_NumberAccidentDisabling).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_DaysIncapacity = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_DaysIncapacity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CR_DaysIncapacityId = RelatedCertification.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumHSEQInfoType.CR_DaysIncapacity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion
        }
    }
}
