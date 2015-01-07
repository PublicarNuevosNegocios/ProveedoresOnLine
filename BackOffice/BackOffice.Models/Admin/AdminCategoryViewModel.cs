
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

        public string AG_City { get; set; }
        public string AG_CityId { get; set; }

        public string GI_CapitalType { get; set; }
        public string GI_CapitalTypeId { get; set; }

        public string GI_CityDirespCode { get; set; }
        public string GI_CityDirespCodeId { get; set; }

        public string GIT_StateId { get; set; }
        public string GIT_State { get; set; }

        public string GIT_StateDirespCode { get; set; }
        public string GIT_StateDirespCodeId { get; set; }

        public int AllTotalRows { get; set; }
        #endregion

        public AdminCategoryViewModel() { }

        public AdminCategoryViewModel(ProveedoresOnLine.Company.Models.Util.GeographyModel oRelatedGeoGraphy)
        {
            #region Geolocalization

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

            #endregion
        }
    }
}
