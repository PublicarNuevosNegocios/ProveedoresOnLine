﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderContactViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedContact { get; private set; }

        public string ContactId { get; set; }

        public string ContactName { get; set; }

        public bool Enable { get; set; }

        #region CompanyContact

        public string CC_Value { get; set; }
        public string CC_ValueId { get; set; }

        public string CC_CompanyContactType { get; set; }
        public string CC_CompanyContactTypeId { get; set; }

        #endregion

        #region PersonContact

        public string CP_PersonContactType { get; set; }
        public string CP_PersonContactTypeId { get; set; }

        public string CP_IdentificationType { get; set; }
        public string CP_IdentificationTypeId { get; set; }

        public string CP_IdentificationNumber { get; set; }
        public string CP_IdentificationNumberId { get; set; }

        public string CP_IdentificationCity { get; set; }
        public string CP_IdentificationCityId { get; set; }

        public string CP_IdentificationFile { get; set; }
        public string CP_IdentificationFileId { get; set; }

        public string CP_Phone { get; set; }
        public string CP_PhoneId { get; set; }

        public string CP_Email { get; set; }
        public string CP_EmailId { get; set; }

        public string CP_Negotiation { get; set; }
        public string CP_NegotiationId { get; set; }

        #endregion

        #region Branch

        public string BR_Representative { get; set; }
        public string BR_RepresentativeId { get; set; }

        public string BR_Address { get; set; }
        public string BR_AddressId { get; set; }

        public string BR_City { get; set; }
        public string BR_CityId { get; set; }

        public string BR_Phone { get; set; }
        public string BR_PhoneId { get; set; }

        public string BR_Fax { get; set; }
        public string BR_FaxId { get; set; }

        public string BR_Email { get; set; }
        public string BR_EmailId { get; set; }

        public string BR_Website { get; set; }
        public string BR_WebsiteId { get; set; }

        public string BR_Latitude { get; set; }
        public string BR_LatitudeId { get; set; }

        public string BR_Longitude { get; set; }
        public string BR_LongitudeId { get; set; }

        #endregion

        public ProviderContactViewModel() { }

        public ProviderContactViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedContact)
        {
            RelatedContact = oRelatedContact;

            ContactId = RelatedContact.ItemId.ToString();

            ContactName = RelatedContact.ItemName;

            Enable = RelatedContact.Enable;

            #region CompanyContact

            CC_Value = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_Value).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CC_ValueId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_Value).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CC_CompanyContactType = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CC_CompanyContactTypeId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region PersonContact

            CP_PersonContactType = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_PersonContactType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_PersonContactTypeId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_PersonContactType).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_IdentificationType = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_IdentificationTypeId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationType).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_IdentificationNumber = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationNumber).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_IdentificationNumberId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationNumber).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_IdentificationCity = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationCity).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_IdentificationCityId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationCity).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_IdentificationFile = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_IdentificationFileId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationFile).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_Phone = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Phone).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_PhoneId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Phone).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_Email = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Email).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_EmailId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Email).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            CP_Negotiation = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Negotiation).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            CP_NegotiationId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Negotiation).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region Branch

            BR_Representative = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Representative).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_RepresentativeId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Representative).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_Address = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Address).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_AddressId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Address).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_City = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_City).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_CityId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_City).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_Phone = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Phone).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_PhoneId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Phone).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_Fax = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Fax).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_FaxId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Fax).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_Email = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Email).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_EmailId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Email).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_Website = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Website).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_WebsiteId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Website).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_Latitude = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Latitude).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_LatitudeId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Latitude).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            BR_Longitude = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Longitude).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            BR_LongitudeId = RelatedContact.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Longitude).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

        }
    }
}
