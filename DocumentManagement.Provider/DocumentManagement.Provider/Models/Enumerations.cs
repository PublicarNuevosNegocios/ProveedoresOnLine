namespace DocumentManagement.Provider.Models
{
    public class Enumerations
    {
        public enum enumProcessStatus
        {
            New = 201,
            FullRegister = 202,
            AgentDocumentaryReviewer = 203,
            CertificationStart = 204,
            CertificationEnds = 205
        }

        public enum enumChangesStatus
        {
            IsValidated = 7001,
            NotValidated = 7002
        }

        public enum enumProviderInfoType
        {
            Company = 2,
            Commercial = 3,
            HSEQ = 7,
            Finantial = 5,
            Lega = 6
        }

        #region Contact

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
            BR_Cellphone = 207011,

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
            CH_PoliticIntegral = 703011,

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
            OrganizationStructure = 501005,
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
            IB_Location = 505001,
            IB_Bank = 505002,
            IB_AccountType = 505003,
            IB_AccountNumber = 505004,
            IB_AccountHolder = 505005,
            IB_ABA = 505006,
            IB_Swift = 505007,
            IB_IBAN = 505008,
            IB_Customer = 505009,
            IB_AccountFile = 505010,
            IB_Code = 505011,
        }

        #endregion

        public enum enumCatalog
        {
            //Contact
            CompanyContactInfoType = 209,
            PersonContactInfoType = 206,
            BrachInfoType = 207,
            DistributorInfoType = 208,

            //Legal
            ChaimberOfCommerceInfoType = 602,
            RUTInfoType = 603,
            CIFINInfoType = 604,
            SARLAFTInfoType = 605,
            ResolucionesInfoType = 606,
        }
    }
}
