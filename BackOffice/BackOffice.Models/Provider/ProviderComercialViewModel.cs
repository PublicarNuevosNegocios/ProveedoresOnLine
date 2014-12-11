using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderComercialViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedCommercial { get; private set; }

        public string CommercialId { get; set; }

        public string CommercialName { get; set; }

        public bool Enable { get; set; }

        #region Experience

        //public string EX_Value { get; set; }
        //public string EX_ValueId { get; set; }

        //public string EX_CompanyContactType { get; set; }
        //public string EX_CompanyContactTypeId { get; set; }

        #endregion

        public ProviderComercialViewModel() { }

        public ProviderComercialViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedCommercial,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivity,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCustomActivity)
        {
            RelatedCommercial = oRelatedCommercial;

            CommercialId = RelatedCommercial.ItemId.ToString();

            CommercialName = RelatedCommercial.ItemName;

            Enable = RelatedCommercial.Enable;

            //#region CompanyContact

            //CC_Value = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_Value).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CC_ValueId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_Value).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CC_CompanyContactType = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CC_CompanyContactTypeId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CC_CompanyContactType).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //#endregion

            //#region PersonContact

            //CP_PersonContactType = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_PersonContactType).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_PersonContactTypeId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_PersonContactType).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CP_IdentificationType = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationType).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_IdentificationTypeId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationType).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CP_IdentificationNumber = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationNumber).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_IdentificationNumberId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationNumber).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CP_IdentificationCity = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationCity).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_IdentificationCityId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationCity).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CP_IdentificationFile = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationFile).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_IdentificationFileId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_IdentificationFile).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CP_Phone = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Phone).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_PhoneId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Phone).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CP_Email = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Email).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_EmailId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Email).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //CP_Negotiation = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Negotiation).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //CP_NegotiationId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.CP_Negotiation).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //#endregion

            //#region Branch

            //BR_Representative = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Representative).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_RepresentativeId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Representative).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //BR_Address = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Address).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_AddressId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Address).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //BR_City = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_City).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_CityId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_City).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //if (oCities != null && oCities.Count > 0)
            //{
            //    BR_CityName = oCities.
            //        Where(x => x.City.ItemId.ToString() == BR_City).
            //        Select(x => x.Country.ItemName + "," + x.State.ItemName + "," + x.City.ItemName).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();
            //}

            //BR_Phone = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Phone).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_PhoneId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Phone).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //BR_Fax = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Fax).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_FaxId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Fax).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //BR_Email = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Email).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_EmailId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Email).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //BR_Website = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Website).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_WebsiteId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Website).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //BR_Latitude = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Latitude).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_LatitudeId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Latitude).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //BR_Longitude = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Longitude).
            //    Select(y => y.Value).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();
            //BR_LongitudeId = RelatedContact.ItemInfo.
            //    Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.BR_Longitude).
            //    Select(y => y.ItemInfoId.ToString()).
            //    DefaultIfEmpty(string.Empty).
            //    FirstOrDefault();

            //#endregion

            //#region Distributor

            //DT_DistributorType = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorType).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_DistributorTypeId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorType).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();

            //DT_Representative = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_Representative).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_RepresentativeId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_Representative).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();

            //DT_Email = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_Email).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_EmailId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_Email).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();

            //DT_Phone = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_Phone).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_PhoneId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_Phone).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();

            //DT_City = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_City).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_CityId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_City).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();
            //if (oCities != null && oCities.Count > 0)
            //{
            //    DT_CityName = oCities.
            //        Where(x => x.City.ItemId.ToString() == DT_City).
            //        Select(x => x.Country.ItemName + "," + x.State.ItemName + "," + x.City.ItemName).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();
            //}

            //DT_DateIssue = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DateIssue).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_DateIssueId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DateIssue).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();

            //DT_DueDate = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DueDate).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_DueDateId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DueDate).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();

            //DT_DistributorFile = RelatedContact.ItemInfo.
            //       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorFile).
            //       Select(y => y.Value).
            //       DefaultIfEmpty(string.Empty).
            //       FirstOrDefault();
            //DT_DistributorFileId = RelatedContact.ItemInfo.
            //        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumContactInfoType.DT_DistributorFile).
            //        Select(y => y.ItemInfoId.ToString()).
            //        DefaultIfEmpty(string.Empty).
            //        FirstOrDefault();

            //#endregion

        }
    }
}
