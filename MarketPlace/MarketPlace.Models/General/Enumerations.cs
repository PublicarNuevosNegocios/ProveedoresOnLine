namespace MarketPlace.Models.General
{
    #region General Enum

    public enum enumCatalog
    {
        ProviderStatus = 902,
        personContactType = 210,
    }

    #endregion

    #region Site Enum

    public enum enumSiteArea
    {
        Desktop
    }

    public enum enumMarketPlaceCustomerModules
    {
        ProviderSearch = 802001,
        ProviderDetail = 802002,
        ProviderCompare = 802003,
        ProviderSelectionCreate = 802004,
        ProviderAprovalHSEQ = 802005,
        ProviderAprovalLegal = 802006,
        ProviderAprovalFinantial = 802007,
        ProviderSelectionAward = 802008,
        ProviderSelectionAudit = 802009,
        ProviderRatingCreate = 802010,
        ProviderRatingView = 802011,
    }

    public enum enumMarketPlaceProviderModules
    {
        MarketComparison = 803001,
        ProviderStatistics = 803002,
    }

    #endregion

    #region Util

    public enum enumCategoryInfoType
    {
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
        AI_IsValue = 109002,
        AI_Formula = 109003,
        AI_ValidationRule = 109004,
        AI_Description = 109005,
        AI_FormulaText = 109006,
        AI_Unit = 109007,
        AI_VerticalFormula = 109008,

        //ICA Info
        I_ICACode = 608001,

        //Bank Info
        B_Location = 505001,
        B_Code = 505011,
    }

    #endregion

    #region Company enum

    public enum enumCompanyType
    {
        Buyer = 202001,
        Provider = 202002,
        BuyerProvider = 202003,
    }

    public enum enumCompanyInfoType
    {
        SalesforceId = 203001,
        ProviderStatus = 203002,
        ProviderPaymentInfo = 203003,
        CertificationDate = 203004,
        CompanyLogo = 203005,
        AlertRisk = 203008,
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

    public enum enumRoleCompanyInfoType
    {
        Modules = 801001,
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
        CR_LTIFResult = 704004,

        //CertficatesAccident
        CA_Year = 705001,
        CA_ManHoursWorked = 705002,
        CA_Fatalities = 705003,
        CA_NumberAccident = 705004,
        CA_NumberAccidentDisabling = 705005,
        CA_DaysIncapacity = 705006,
        CA_CertificateAccidentARL = 705007,
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

        //Basic Info
        BI_TotalActive = 3115,
        BI_TotalPassive = 3116,
        BI_TotalPatrimony = 3117,
        BI_OperationIncome = 3821,
        BI_IncomeBeforeTaxes = 3818,
        BI_CurrentActive = 3118,
        BI_CurrentPassive = 3120,
        BI_Altman = 4930,
    }

    #endregion

    #region Provider enum

    public enum enumSearchOrderType
    {
        CustomerRate = 113001,
        Relevance = 113002,
        Alphabetic = 113003,
    }

    public enum enumFilterType
    {
        EconomicActivity = 111006,
        CustomEconomicActivity = 111007,
        City = 111008,

        ProviderStatus = 112001,
        ProviderRate = 112002,
        IsRelatedProvider = 112003,
    }

    public enum enumCustomerProviderInfoType
    {
        CustomerNotes = 901002,
        ProviderRate = 901003
    }

    public enum enumContactInfoType
    {
        //CompanyContact
        CompanyContactType = 205001,
        Value = 205002,

        //PersonContact
        PersonContactType = 206001,
        IdentificationType = 206002,
        IdentificationNumber = 206003,
        IdentificationCity = 206004,
        IdentificationFile = 206005,
        Phone = 206006,
        Email = 206007,
        Negotiation = 206008,

        //Branch
        B_Representative = 207001,
        B_Address = 207002,
        B_City = 207003,
        B_Phone = 207004,
        B_Fax = 207005,
        B_Email = 207006,
        B_Website = 207007,
        B_Latitude = 207008,
        B_Longitude = 207009,
        BR_IsPrincipal = 207010,

        //Distributor
        D_DistributorType = 208001,
        D_Representative = 208002,
        D_Email = 208003,
        D_Phone = 208004,
        D_City = 208005,
        D_DateIssue = 208006,
        D_DueDate = 208007,
        D_DistributorFile = 208008,


        C_Telefono = 209001,
        C_Celular = 209002,
        C_PaginaWeb = 209003
    }
    #endregion

    #region Alert Risk

    public enum enumBlackListStatus
    {
        ShowAlert = 1101001,
        DontShowAlert = 1101002,
    }

    #endregion
}
