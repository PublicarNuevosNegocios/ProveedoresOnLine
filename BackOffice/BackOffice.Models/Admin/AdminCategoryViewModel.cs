
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminCategoryViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal { get; private set; }

        public bool Enable { get; set; }

        #region Geolocalization

        public string GIT_Country { get; set; }
        public string GIT_CountryId { get; set; }

        public string GIT_CountryDirespCode { get; set; }
        public string GIT_CountryDirespCodeId { get; set; }

        public string GIT_CountryType { get; set; }
        public string GIT_CountryTypeId { get; set; }

        public bool GIT_CountryEnable { get; set; }
               
        public string GIT_StateId { get; set; }
        public string GIT_State { get; set; }

        public string GIT_StateDirespCode { get; set; }
        public string GIT_StateDirespCodeId { get; set; }

        public bool GIT_StateEnable { get; set; }

        public string AG_City { get; set; }
        public string AG_CityId { get; set; }

        public string GI_CapitalType { get; set; }
        public string GI_CapitalTypeId { get; set; }

        public string GI_CityDirespCode { get; set; }
        public string GI_CityDirespCodeId { get; set; }

        public bool GI_CityEnable { get; set; }

        public int AllTotalRows { get; set; }
        #endregion

        #region Banks

        public string B_Bank { get; set; }
        public string B_BankId { get; set; }
        public bool B_BankEnable { get; set; }
        
        public string B_CityId { get; set; }
        public string B_City { get; set; }

        #endregion

        public AdminCategoryViewModel() { }

        public AdminCategoryViewModel(ProveedoresOnLine.Company.Models.Util.GeographyModel oRelatedGeoGraphy)
        {
            #region Geolocalization

            #region Country
            GIT_Country = oRelatedGeoGraphy.Country.ItemName;
            GIT_CountryId = oRelatedGeoGraphy.Country.ItemId.ToString();

            GIT_CountryDirespCode = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.Value).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GIT_CountryDirespCodeId = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_CountryType = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_GeographyType).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_CountryTypeId = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_GeographyType).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_CountryEnable = oRelatedGeoGraphy.Country.Enable;

            #endregion            

            #region State
            GIT_State = oRelatedGeoGraphy.State.ItemName;
            GIT_StateId = oRelatedGeoGraphy.State.ItemId.ToString();

            GIT_StateDirespCode = oRelatedGeoGraphy.State.ItemInfo != null ? oRelatedGeoGraphy.State.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_StateDirespCodeId = oRelatedGeoGraphy.State.ItemInfo != null ? oRelatedGeoGraphy.State.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_StateEnable = oRelatedGeoGraphy.State.Enable;
            #endregion

            #region City
            AG_City = oRelatedGeoGraphy.City.ItemName;
            AG_CityId = oRelatedGeoGraphy.City.ItemId.ToString();

            GI_CapitalType = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_CapitalType).
                             Select(y => y.Value).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_CapitalTypeId = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_CapitalType).
                             Select(y => y.ItemInfoId.ToString()).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_CityDirespCode = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.Value).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_CityDirespCodeId = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.ItemInfoId.ToString()).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;
            
            GI_CityEnable = oRelatedGeoGraphy.City.Enable;

            #endregion

            #endregion
        }

        public AdminCategoryViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oCategory)
        {
            #region Banks

            B_Bank = oCategory.ItemName;
            B_BankId = oCategory.ItemId.ToString();
            B_BankEnable = oCategory.Enable;

            B_City = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId != null).
                            Select(y => y.Value.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            B_CityId = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId != null).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            #endregion
        }
    }
}
