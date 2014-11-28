namespace BackOffice.Models.General
{
    /// <summary>
    /// system catalogs
    /// </summary>
    public enum enumCatalog
    {
        CompanyIdentificationType = 1,
        CompanyType = 2,
        PersonIdentificationType = 3,

        ContactType = 11,
        CompanyContactType = 12,
        ContactInfoType = 18,
    }

    #region Company

    /// <summary>
    /// Company info type
    /// </summary>
    public enum enumCompanyInfoType
    {
        SalesforceId = 4001,
    }

    /// <summary>
    /// Contact type
    /// </summary>
    public enum enumContactType
    {
        CompanyContact = 11001,
        PersonContact = 11002,
        Brach = 11003,
        Distributor = 11004
    }
    /// <summary>
    /// Contact info type
    /// </summary>
    public enum enumContactInfoType
    {
        CC_CompanyContactType = 18001,
        CC_Value = 18002,
    }

    #endregion

    #region Legal
    /// <summary>
    /// Legal type
    /// </summary>
    public enum enumLegalType
    {
        ChaimberOfCommerce = 17001,
        RUT = 17002,
        CIFIN = 17003,
        SARLAFT = 17004,
        Resolution = 17005
    }
    #endregion

    #region HSEQ

    /// <summary>
    /// HSEQ type
    /// </summary>
    public enum enumHSEQType
    {
        Certifications = 16001,
        CompanyHealtyPolitic = 16002,
        CompanyRiskPolicies = 16003
    }

    #endregion

}
