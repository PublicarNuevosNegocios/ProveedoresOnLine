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
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedContactInfo { get; set; }

        #region CompanyContact
        public string CI_ContactName { get; set; }

        public string CI_ContactType { get; set; }

        public string CI_Value { get; set; } 
        #endregion
        
        #region PersonContact
        public string PC_ContactName { get; set; }

        public string PC_RepresentantType { get; set; }

        public string PC_IdentificationType { get; set; }

        public string PC_IdentificationNumber { get; set; }

        public string PC_ExpeditionDocumentCity { get; set; }

        public string PC_TelephoneNumber { get; set; }

        public string PC_Email { get; set; }

        public string PC_Negotiation { get; set; }

        public string PC_LegalFile { get; set; } 
        #endregion

        #region Brach

        public string BR_Name { get; set; } 
        public string BR_Representative { get; set; }        
        public string BR_Address { get; set; }        
        public string BR_City { get; set; }        
        public string BR_Phone { get; set; }
        public string BR_Fax { get; set; }
        public string BR_Email { get; set; }
        public string BR_Website { get; set; }
        public string BR_Latitude { get; set; }
        public string BR_Longitude { get; set; }
        public string BR_IsPrincipal { get; set; }
        #endregion

        #region Distributor

        public string DT_SocialReazon { get; set; }       
        public string DT_DistributorType { get; set; }       
        public string DT_Representative { get; set; }
        public string DT_Email { get; set; }
        public string DT_Phone { get; set; }
        public string DT_City { get; set; }        
        public string DT_DateIssue { get; set; }
        public string DT_DueDate { get; set; }
        public string DT_DistributorFile { get; set; }       

        #endregion

        public ProviderContactViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedInfo, List<CatalogModel> oOptions)
        {
            RelatedContactInfo = oRelatedInfo;

            #region CompanyContact
            CI_ContactName = oRelatedInfo.ItemName;

            CI_ContactType = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.CompanyContactType).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CI_ContactType = !string.IsNullOrEmpty(CI_ContactType) && oOptions != null && oOptions.Count > 0 ?
                oOptions.Where(x => x.ItemId.ToString() == CI_ContactType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            CI_Value = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Value).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault(); 

            #endregion

            #region PersonContact

            PC_ContactName = oRelatedInfo.ItemName;

            PC_RepresentantType = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.PersonContactType).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            PC_RepresentantType = !string.IsNullOrEmpty(PC_RepresentantType) && oOptions != null && oOptions.Count > 0 ?
                oOptions.Where(x => x.ItemId.ToString() == PC_RepresentantType).Select(x => x.ItemName).FirstOrDefault() : "N/A";
            
            PC_IdentificationType = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationType).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            PC_IdentificationType = !string.IsNullOrEmpty(PC_IdentificationType) && oOptions != null && oOptions.Count > 0 ?
                oOptions.Where(x => x.ItemId.ToString() == PC_IdentificationType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            PC_IdentificationNumber = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationNumber).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            PC_ExpeditionDocumentCity = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationCity).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            PC_TelephoneNumber = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Phone).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            PC_Email = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Email).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            PC_Negotiation = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.Negotiation).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            PC_LegalFile = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.IdentificationFile).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();
            #endregion           
        }

        public ProviderContactViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedInfo, 
            List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oCities
            , List<CatalogModel> oOptions)
        {
            #region Branch

            BR_Name = oRelatedInfo.ItemName;

            BR_Representative = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Representative).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            BR_Address = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Address).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            BR_City = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            BR_City = !string.IsNullOrEmpty(BR_City) ?
                oCities.Where(x => x.City.ItemId == Convert.ToInt32(BR_City)).Select(x => x.City.ItemName).FirstOrDefault() : string.Empty;

            BR_Phone = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Phone).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            BR_Fax = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Fax).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            BR_Email = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Email).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            BR_Website = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Website).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            BR_IsPrincipal = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.BR_IsPrincipal).
               Select(y => y.Value == "1" ? "Si" : "No").
               DefaultIfEmpty("No").
               FirstOrDefault();

            #endregion

            #region Distributor

            DT_SocialReazon = oRelatedInfo.ItemName;

            DT_DistributorType = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_DistributorType).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();

            DT_DistributorType = !string.IsNullOrEmpty(DT_DistributorType) && oOptions != null && oOptions.Count > 0 ?
                oOptions.Where(x => x.ItemId.ToString() == DT_DistributorType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            DT_Representative = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_Representative).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();

            DT_Email = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_Email).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();

            DT_Phone = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_Phone).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();

            DT_City = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_City).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();
            DT_City = !string.IsNullOrEmpty(DT_City) ? 
                oCities.Where(x => x.City.ItemId == Convert.ToInt32(DT_City)).Select(x => x.City.ItemName).FirstOrDefault() : string.Empty;

            DT_DateIssue = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_DateIssue).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();        

            DT_DistributorFile = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.D_DistributorFile).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();
            #endregion
        }
        
        public ProviderContactViewModel() {}
    }   
}
