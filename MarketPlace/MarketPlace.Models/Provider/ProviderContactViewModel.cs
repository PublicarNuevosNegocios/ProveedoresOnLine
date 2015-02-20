using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderContactViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedContactInfo { get; private set; }

        #region CompanyContact

        public string CI_ContactName { get { return RelatedContactInfo.ItemName; } }

        private string oCI_ContactType;
        public string CI_ContactType
        {
            get
            {
                if (string.IsNullOrEmpty(oCI_ContactType))
                {
                    oCI_ContactType = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.CompanyContactType).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oCI_ContactType;
            }
        }

        private string oCI_Value;
        public string CI_Value
        {
            get
            {
                if (string.IsNullOrEmpty(oCI_Value))
                {
                    oCI_Value = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Value).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oCI_ContactType;
            }
        }

        #endregion

        #region PersonContact

        public string PC_ContactName { get { return RelatedContactInfo.ItemName; } }

        private string oPC_RepresentantType;
        public string PC_RepresentantType
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_RepresentantType))
                {
                    oPC_RepresentantType = MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(
                        RelatedContactInfo.ItemInfo.
                           Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.PersonContactType).
                           Select(y => y.Value).
                           DefaultIfEmpty(string.Empty).
                           FirstOrDefault());
                }
                return oPC_RepresentantType;
            }
        }

        private string oPC_IdentificationType;
        public string PC_IdentificationType
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_IdentificationType))
                {
                    oPC_IdentificationType = MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(
                        RelatedContactInfo.ItemInfo.
                           Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationType).
                           Select(y => y.Value).
                           DefaultIfEmpty(string.Empty).
                           FirstOrDefault());
                }
                return oPC_IdentificationType;
            }
        }

        private string oPC_IdentificationNumber;
        public string PC_IdentificationNumber
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_IdentificationNumber))
                {
                    oPC_IdentificationNumber = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationNumber).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oPC_IdentificationNumber;
            }
        }

        private string oPC_ExpeditionDocumentCity;
        public string PC_ExpeditionDocumentCity
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_ExpeditionDocumentCity))
                {
                    oPC_ExpeditionDocumentCity = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationCity).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oPC_ExpeditionDocumentCity;
            }
        }

        private string oPC_TelephoneNumber;
        public string PC_TelephoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_TelephoneNumber))
                {
                    oPC_TelephoneNumber = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Phone).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oPC_TelephoneNumber;
            }
        }

        private string oPC_Email;
        public string PC_Email
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_Email))
                {
                    oPC_Email = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Email).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oPC_Email;
            }
        }

        private string oPC_Negotiation;
        public string PC_Negotiation
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_Negotiation))
                {
                    oPC_Negotiation = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Negotiation).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oPC_Negotiation;
            }
        }

        private string oPC_LegalFile;
        public string PC_LegalFile
        {
            get
            {
                if (string.IsNullOrEmpty(oPC_LegalFile))
                {
                    oPC_LegalFile = RelatedContactInfo.ItemInfo.
                      Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationFile).
                      Select(y => y.Value).
                      DefaultIfEmpty(string.Empty).
                      FirstOrDefault();
                }
                return oPC_LegalFile;
            }
        }

        #endregion

        #region Brach

        public string BR_Name { get { return RelatedContactInfo.ItemName; } }

        private string oBR_Representative;
        public string BR_Representative
        {
            get
            {
                if (string.IsNullOrEmpty(oBR_Representative))
                {
                    oBR_Representative = RelatedContactInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Representative).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oBR_Representative;
            }
        }

        private string oBR_Address;
        public string BR_Address
        {
            get
            {
                if (string.IsNullOrEmpty(oBR_Address))
                {
                    oBR_Address = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Address).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oBR_Address;
            }
        }

        private string oBR_City;
        public string BR_City
        {
            get
            {
                if (string.IsNullOrEmpty(oBR_City))
                {
                    oBR_City = oDT_City = MarketPlace.Models.Company.CompanyUtil.GetCityName(
                        RelatedContactInfo.ItemInfo.
                                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).
                                 Select(y => y.Value).
                                 DefaultIfEmpty(string.Empty).
                                 FirstOrDefault());
                }
                return oBR_City;
            }
        }

        private string oBR_Phone;
        public string BR_Phone
        {
            get
            {
                if (string.IsNullOrEmpty(oBR_Phone))
                {
                    oBR_Phone = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Phone).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oBR_Phone;
            }
        }

        private string oBR_Fax;
        public string BR_Fax
        {
            get
            {
                if (string.IsNullOrEmpty(oBR_Fax))
                {
                    oBR_Fax = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Fax).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oBR_Fax;
            }
        }

        private string oBR_Email;
        public string BR_Email
        {
            get
            {
                if (string.IsNullOrEmpty(oBR_Email))
                {
                    oBR_Email = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Email).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oBR_Email;
            }
        }

        private string oBR_Website;
        public string BR_Website
        {
            get
            {
                if (string.IsNullOrEmpty(oBR_Website))
                {
                    oBR_Website = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Website).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }
                return oBR_Website;
            }
        }

        private bool? oBR_IsPrincipal;
        public bool BR_IsPrincipal
        {
            get
            {
                if (oBR_IsPrincipal == null)
                {
                    oBR_IsPrincipal = RelatedContactInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal).
                       Select(y => y.Value == "1" ? true : false).
                       DefaultIfEmpty(false).
                       FirstOrDefault();
                }
                return oBR_IsPrincipal.Value;
            }
        }

        #endregion

        #region Distributor

        public string DT_SocialReazon { get { return RelatedContactInfo.ItemName; } }

        private string oDT_DistributorType;
        public string DT_DistributorType
        {
            get
            {
                if (string.IsNullOrEmpty(oDT_DistributorType))
                {
                    oDT_DistributorType = RelatedContactInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_DistributorType).
                        Select(y => MarketPlace.Models.Company.CompanyUtil.ProviderOptions.
                                        Where(x => x.ItemId.ToString() == y.Value).
                                        Select(x => x.ItemName).
                                        DefaultIfEmpty("N/A").
                                        FirstOrDefault()).
                        DefaultIfEmpty("N/A").
                        FirstOrDefault();
                }

                return oDT_DistributorType;
            }
        }

        public string oDT_Representative;
        public string DT_Representative
        {
            get
            {
                if (string.IsNullOrEmpty(oDT_Representative))
                {
                    oDT_Representative = RelatedContactInfo.ItemInfo.
                      Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_Representative).
                      Select(y => y.Value).
                      DefaultIfEmpty(string.Empty).
                      FirstOrDefault();
                }

                return oDT_Representative;
            }
        }

        private string oDT_Email;
        public string DT_Email
        {
            get
            {
                if (string.IsNullOrEmpty(oDT_Email))
                {
                    oDT_Email = RelatedContactInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_Email).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oDT_Email;
            }
        }

        private string oDT_Phone;
        public string DT_Phone
        {
            get
            {
                if (string.IsNullOrEmpty(oDT_Phone))
                {
                    oDT_Phone = RelatedContactInfo.ItemInfo.
                      Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_Phone).
                      Select(y => y.Value).
                      DefaultIfEmpty(string.Empty).
                      FirstOrDefault();
                }

                return oDT_Phone;
            }
        }

        private string oDT_City;
        public string DT_City
        {
            get
            {
                if (string.IsNullOrEmpty(oDT_City))
                {
                    oDT_City = MarketPlace.Models.Company.CompanyUtil.GetCityName(
                        RelatedContactInfo.ItemInfo.
                                 Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_City).
                                 Select(y => y.Value).
                                 DefaultIfEmpty(string.Empty).
                                 FirstOrDefault());
                }
                return oDT_City;
            }
        }

        private string oDT_DateIssue;
        public string DT_DateIssue
        {
            get
            {
                if (string.IsNullOrEmpty(oDT_DateIssue))
                {
                    oDT_DateIssue = RelatedContactInfo.ItemInfo.
                      Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_DateIssue).
                      Select(y => y.Value).
                      DefaultIfEmpty(string.Empty).
                      FirstOrDefault();
                }

                return oDT_DateIssue;
            }
        }

        private string oDT_DistributorFile;
        public string DT_DistributorFile
        {
            get
            {
                if (string.IsNullOrEmpty(oDT_Representative))
                {
                    oDT_DistributorFile = RelatedContactInfo.ItemInfo.
                      Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_DistributorFile).
                      Select(y => y.Value).
                      DefaultIfEmpty(string.Empty).
                      FirstOrDefault();
                }

                return oDT_Representative;
            }
        }

        #endregion

        public ProviderContactViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedInfo)
        {
            RelatedContactInfo = oRelatedInfo;
        }

        public ProviderContactViewModel() { }
    }
}
