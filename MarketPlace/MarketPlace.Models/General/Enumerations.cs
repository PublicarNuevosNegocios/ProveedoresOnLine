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
        Telefono = 209001 ,
        Celular = 209002,
        PaginaWeb = 209003,
        CódigoPostal = 209004,	
        Fax = 209005,
        Email = 209006
    }
    #endregion
}
