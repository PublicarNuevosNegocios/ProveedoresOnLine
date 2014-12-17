﻿namespace BackOffice.Models.General
{
    /// <summary>
    /// system catalogs
    /// </summary>
    public enum enumCatalog
    {
        PersonIdentificationType = 101,
        ActivityInfoType = 102,
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
        DistributorType = 211,

        ARLRate = 212,

        PersonType = 213,
        EntityType = 214,       
        TaxPayerType = 215,

        CommercialType = 301,
        ExperienceInfoType = 302,

        LegalType = 601,

        ChaimberOfCommerceInfoType = 602,
        RUTInfoType = 603,
        CIFINInfoType = 604,
        SARLAFTInfoType = 605,
        ResolucionesInfoType = 606,
    }

    #region Util

    public enum enumCategoryInfoType
    {
        //economic activity info
        Type = 102001,
        Category = 102002,
        Group = 102003,
        IsCustom = 102004,

        //geography info
        GeographyType = 106001,
        CapitalType = 106002,
        DirespCode = 106003,

        //Account info
        Order = 109001,
    }

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

        //Branch
        BR_Representative = 207001,
        BR_Address = 207002,
        BR_City = 207003,
        BR_Phone = 207004,
        BR_Fax = 207005,
        BR_Email = 207006,
        BR_Website = 207007,
        BR_Latitude = 207008,
        BR_Longitude = 207009,

        //Distributor
        DT_DistributorType = 208001,
        DT_Representative = 208002,
        DT_Email = 208003,
        DT_Phone = 208004,
        DT_City = 208005,
        DT_DateIssue = 208006,
        DT_DueDate = 208007,
        DT_DistributorFile = 208008,
    }

    #endregion

    #region Comercial

    /// <summary>
    /// Comercial type
    /// </summary>
    public enum enumCommercialType
    {
        Experience = 301001,
    }

    /// <summary>
    /// Commercial infor types
    /// </summary>
    public enum enumCommercialInfoType
    {
        //Experience
        EX_ContractType = 302001,
        EX_Currency = 302002,
        EX_DateIssue = 302003,
        EX_DueDate = 302004,
        EX_Client = 302005,
        EX_ContractNumber = 302006,
        EX_ContractValue = 302007,
        EX_Phone = 302008,
        EX_BuiltArea = 302009,
        EX_BuiltUnit = 302010,
        EX_ExperienceFile = 302011,
        EX_ContractSubject = 302012,
        EX_EconomicActivity = 302013,
        EX_CustomEconomicActivity = 302014,

    }

    #endregion

    #region Legal
    public enum enumLegalType
    {
        ChaimberOfCommerce = 601001,
        RUT = 601002,
        CIFIN = 601003,
        SARLAFT = 601004,
        Resoluciones = 601005,

        Designations = 601007,
    }

    public enum enumLegalInfoType
    {
        //Chaimber Of Comerce
        CP_ConstitutionDate = 602001,
        CP_ConstitutionEndDate = 602002,
        CP_State = 602003,
        CP_InscriptionCity = 602004,
        CP_InscriptionNumber = 602005,
        CP_ExistenceAndLegalPersonCertificate = 602006,
        CP_CertificateExpeditionDate = 602007,
        CP_SocialObject = 602008,

        //RUT
        R_PersonType = 603001,
        R_LargeContributor = 603002,
        R_LargeContributorReceipt = 603003,
        R_LargeContributorDate = 603004,
        R_SelfRetainer = 603005,
        R_SelfRetainerReciept = 603006,
        R_SelfRetainerDate = 603007,
        R_EntityType = 603008,
        R_IVA = 603009,
        R_TaxPayerType = 603010,
        R_ICA = 603011,
        R_RUTFile = 603012,
        R_LargeContributorFile = 603013,
        R_SelfRetainerFile = 603014,

        //CIFIN
        CF_QueryDate = 604001,
        CF_ResultQuery = 604002,
        CF_AutorizationFile = 604003,

        //Designations
        CD_PartnerName = 607001,
        CD_PartnerIdentificationNumber = 607002,
        CD_PartnerRank = 607003,

        //CP_

    }

    #endregion

    #region HSEQ

    /// <summary>
    /// HSEQ type
    /// </summary>
    public enum enumHSEQType
    {
        Certifications = 701001,
        CompanyHealtyPolitic = 701002,
        CompanyRiskPolicies = 701003
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
        C_EndDateCertification = 702004,
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

    /// <summary>
    /// Finantial type
    /// </summary>
    public enum enumfinancialType
    {
        BalanceSheetInfoType = 501001,
        TaxInfoType = 501002,
        IncomeStatementInfoType = 501003,
        BankInfoType = 501004,
    }

    /// <summary>
    /// Commercial infor types
    /// </summary>
    public enum enumfinancialInfoType
    {
        //Balance Sheet
        SH_Year = 502001,
        SH_BalanceSheetFile = 502002,
        SH_Currency = 502003,
    }

    #endregion

    #region Legal
    #endregion

}
