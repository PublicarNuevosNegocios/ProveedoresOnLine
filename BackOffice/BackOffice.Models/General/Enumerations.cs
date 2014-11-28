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

}
