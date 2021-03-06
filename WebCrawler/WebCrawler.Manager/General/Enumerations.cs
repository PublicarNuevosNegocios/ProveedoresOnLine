﻿namespace WebCrawler.Manager.General
{
    /// <summary>
    /// system catalogs
    /// </summary>
    public enum enumMenu
    {
        GeneralInfo = 1, 
        Certifications = 2, 
        Experience = 3, 
        GeneralBalance = 4, 
        LegalInfo = 5, 
        HSE = 6, 
        Representation = 7, 
        OtherInfo = 8,
        Finantial = 9,
        BalanceInfo = 10,
    }

    #region Util

    public enum enumCategoryInfoType
    {
        //identification type
        RUT = 201001,
        NIT = 201002,
        TAX_ID = 201003,

        //company type
        Customer = 202001,
        Provider = 202002,
        Customer_Provider = 202003,

        //economic activity info
        EA_Type = 102001,
        EA_Category = 102002,
        EA_Group = 102003,
        EA_IsCustom = 102004,

        //geography type
        GI_GeographyType = 106001,
        GI_CapitalType = 106002,
        GI_DirespCode = 106003,

        //GeographyInfo Type
        GIT_Country = 105001,
        GIT_State = 105002,
        GIT_City = 105003,

        //CapitalType
        CT_CountryCapital = 107001,
        CT_CapitalState = 107002,
        CT_IsNotCapital = 107003,

        //Account info
        AI_Order = 109001,

        //ICA Info
        I_ICACode = 608001,

        //Bank Info
        B_Location = 505001,
        B_Code = 505011,

        //Payment Info
        NotApplicate = 217001,

        //Company Type Info
        CC_Cellphone = 209002,
        CC_PostalCode = 209004,
        CC_Email = 209006,
        CC_Fax = 209005,
        CC_WebPage = 209003,
        CC_Telephone = 209001,

        //Type Person Contact Info
        CP_Commercial = 210001,
        CP_Legal = 210002,
        CP_CommercialLegal = 210003,
        CP_Finantial = 210004,
        CP_HSEQ = 210005,
        CP_Judicial = 210006,
        CP_Technical = 210007,
        CP_Buy = 210008,
        CP_Security = 210009,

        //Identifiation Type
        CP_CC = 101001,
        CP_Passport = 101002,
    }

    #endregion

    #region Company

    public enum enumCompanyType
    {
        Buyer = 202001,
        Provider = 202002,
        BuyerProvider = 202003,
    }

    /// <summary>
    /// Company info type
    /// </summary>
    public enum enumCompanyInfoType
    {
        SalesforceId = 203001,
        ProviderPaymentInfo = 203003,
        CertificationDate = 203004,
        CompanyLogo = 203005,
        ComercialName = 203007,
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
        BR_IsPrincipal = 207010,

        //Distributor
        DT_DistributorType = 208001,
        DT_Representative = 208002,
        DT_Email = 208003,
        DT_Phone = 208004,
        DT_City = 208005,
        DT_DateIssue = 208006,
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
        CP_UndefinedDate = 602012,

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

        //SARLAFT
        SF_ProcessDate = 605001,
        SF_PersonType = 605002,
        SF_SARLAFTFile = 605003,

        //Resolutions
        RS_EntityType = 606001,
        RS_ResolutionFile = 606002,
        RS_StartDate = 606003,
        RS_EndDate = 606004,
        RS_Description = 606005,

        //Designations
        CD_PartnerName = 607001,
        CD_PartnerIdentificationNumber = 607002,
        CD_PartnerRank = 607003,

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
        CompanyRiskPolicies = 701003,

        CertificatesAccident = 701004
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

        //CertficatesAccident
        CA_Year = 705001,
        CA_ManHoursWorked = 705002,
        CA_Fatalities = 705003,
        CA_NumberAccident = 705004,
        CA_NumberAccidentDisabling = 705005,
        CA_DaysIncapacity = 705006,
        CA_CertificateAccidentARL = 705007,

        //CR_Year = 704005,
        //CR_ManHoursWorked = 704006,
        //CR_Fatalities = 704007,
        //CR_NumberAccident = 704008,
        //CR_NumberAccidentDisabling = 704009,
        //CR_DaysIncapacity = 704010,
        //CR_CertificateAccidentARL = 704004,
    }

    #endregion

    #region Finantial

    /// <summary>
    /// Finantial type
    /// </summary>
    public enum enumFinancialType
    {
        BalanceSheetInfoType = 501001,
        TaxInfoType = 501002,
        IncomeStatementInfoType = 501003,
        BankInfoType = 501004,
    }

    /// <summary>
    /// Commercial infor types
    /// </summary>
    public enum enumFinancialInfoType
    {
        //Balance Sheet
        SH_Year = 502001,
        SH_BalanceSheetFile = 502002,
        SH_Currency = 502003,

        //Taxes Info
        TX_Year = 503001,
        TX_TaxFile = 503002,

        //IncomeStatement Info
        IS_Year = 504001,
        IS_GrossIncome = 504002,
        IS_NetIncome = 504003,
        IS_GrossEstate = 504004,
        IS_LiquidHeritage = 504005,
        IS_FileIncomeStatement = 504006,

        //Bank Info
        IB_Bank = 505002,
        IB_AccountType = 505003,
        IB_AccountNumber = 505004,
        IB_AccountHolder = 505005,
        IB_ABA = 505006,
        IB_Swift = 505007,
        IB_IBAN = 505008,
        IB_Customer = 505009,
        IB_AccountFile = 505010,
    }

    #endregion

    #region ProviderCustomer

    public enum enumProviderCustomerType
    {
        InternalMonitoring = 901001,
        CustomerMonitoring = 901002,
        RateCustomer = 901003,
    }

    public enum enumProviderCustomerStatus
    {
        Creation = 902001,
        Process = 902002,
        Upgrade = 902003,
        Basic_Validate = 902004,
        Full_Validate = 902005,
    }

    #endregion
}
