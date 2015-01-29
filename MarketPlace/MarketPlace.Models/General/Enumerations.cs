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
