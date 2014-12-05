namespace BackOffice.Models.General
{
    /// <summary>
    /// system catalogs
    /// </summary>
    public enum enumCatalog
    {
        PersonIdentificationType = 101,
        CategoryInfoType = 102,
        ProductType = 103,

        CompanyIdentificationType = 201,
        CompanyType = 202,
        CompanyInfoType = 203,

        ContactType = 204,

        CompanyContactInfoType = 205,
        PersonContactInfoType = 206,
        BrachInfoType = 207,
        DistributorInfoType = 208,

        CompanyContactType = 209,
        PersonContactType = 210,


        LegalType = 601,
    }

    #region Util

    #endregion

    #region Company

    /// <summary>
    /// Company info type
    /// </summary>
    public enum enumCompanyInfoType
    {
        SalesforceId = 203001,
    }

    /// <summary>
    /// Contact type
    /// </summary>
    public enum enumContactType
    {
        CompanyContact = 204001,
        PersonContact = 204002,
        Brach = 204003,
        Distributor = 204004
    }

    /// <summary>
    /// Contact info type
    /// </summary>
    public enum enumContactInfoType
    {
        //CompanyContact
        CC_CompanyContactType = 205001,
        CC_Value = 205002,

        //PersonContact
        CP_PersonContactType = 206001,
        CP_IdentificationType = 206002,
        CP_IdentificationNumber = 206003,
        CP_IdentificationCity = 206004,
        CP_IdentificationFile = 206005,
        CP_Phone = 206006,
        CP_Email = 206007,
        CP_Negotiation = 206008,
    }

    #endregion

    #region Legal

    #endregion

    #region HSEQ

    /// <summary>
    /// HSEQ type
    /// </summary>
    public enum enumHSEQType
    {
        Certifications = 701001,
        CompanyHealtyPolitic = 701002,
        CompanyRiskPolicies = 702003
    }

    /// <summary>
    /// HSEQ info type
    /// </summary>
    public enum enumHSEQInfoType
    {
        //Certifications
        C_CertificationCompany = 702001,
        C_Rule = 702002,
        C_StartDateCertification = 702003,
        C_EndDataCertification = 702004,
        C_CCS = 702005,
        C_CertificationFile = 702006,
        C_Scope = 702007,

        //CompanyHealtyPolitic
        CH_Year = 703001,
        CH_PoliticsSecurity = 703002,
        CH_PoliticsNoAlcohol = 703003,
        CH_ProgramOccupationalHealth = 703004,
        CH_RuleIndustrialSecurity = 703005,
        CH_MatrixRiskControl = 703006,
        CH_CorporateSocialResponsability = 703007,
        CH_ProgramEnterpriseSecurity = 703008,
        CH_PoliticsRecruiment = 703009,
        CH_CertificationsForm = 703010,

        //CompanyRiskPolicies
        CR_SystemOccupationalHazards = 704001,
        CR_RateARL = 704002,
        CR_CertificateAffiliateARL = 704003,
        CR_CertificateAccidentARL = 704004,
        CR_Year = 704005,
        CR_ManHoursWorked = 704006,
        CR_Fatalities = 704007,
        CR_NumberAccident = 704008,
        CR_NumberAccidentDisabling = 704009,
        CR_DaysIncapacity = 704010,
    }

    #endregion

    #region Finantial
    #endregion

    #region Legal
    #endregion

}