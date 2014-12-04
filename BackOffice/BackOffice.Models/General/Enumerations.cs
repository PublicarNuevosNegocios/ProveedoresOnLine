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

        ChaimberOfCommerceInfoType = 602,
        RUTInfoType = 603,
        CIFINInfoType = 604,
        SARLAFTInfoType = 605,
        ResolucionesInfoType = 606,
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
    }

    public enum enumLegalInfoType
    {
        CP_ConstitutionDate = 602001,
        CP_ConstitutionEndDate = 602002,
        CP_State = 602003,
        CP_InscriptionCity = 602004,
        CP_InscriptionNumber = 602005,
        CP_ExistenceAndLegalPersonCertificate = 602006,
        CP_CertificateExpeditionDate = 602007,
        CP_SocialObject = 602008,
        CP_PartnerName = 602009,
        CP_PartnerIdentificationNumber = 602010,
        CP_PartnerRank = 602011,
    }

    #endregion

    #region HSEQ

    /// <summary>
    /// Company info type
    /// </summary>
    public enum enumHSEQType
    {
        Certifications = 701001,
        CompanyHealtyPolitic = 701002,
        CompanyRiskPolicies = 702003
    }

    #endregion

    #region Finantial
    #endregion

    #region Legal
    #endregion

}
