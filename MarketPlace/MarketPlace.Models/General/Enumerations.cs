namespace MarketPlace.Models.General
{
    #region General Enum

    public enum enumCatalog
    {
        ProviderStatus = 902,

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
}
