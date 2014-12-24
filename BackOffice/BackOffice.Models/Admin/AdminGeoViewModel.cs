
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminGeoViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal { get; private set; }

        public bool Enable { get; set; }

        #region Geolocalization

        public string GIT_Country { get; set; }
        public string GIT_CountryId { get; set; }

        public string AG_City { get; set; }
        public string AG_CityId { get; set; }

        public string GI_CapitalType { get; set; }
        public string GI_CapitalTypeId { get; set; }

        public string GI_DirespCode { get; set; }
        public string GI_DirespCodeId { get; set; }

        public string GIT_StateId { get; set; }
        public string GIT_State { get; set; }

        #endregion

        public AdminGeoViewModel() { }

        public AdminGeoViewModel(ProveedoresOnLine.Company.Models.Util.GeographyModel oRelatedGeoGraphy)
        {
            #region Geolocalization

            GIT_Country = oRelatedGeoGraphy.Country.ItemName;
            GIT_CountryId = oRelatedGeoGraphy.Country.ItemId.ToString();

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

            GI_DirespCode = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.Value).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_DirespCodeId = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.ItemInfoId.ToString()).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GIT_State = oRelatedGeoGraphy.State.ItemName;
            GIT_StateId = oRelatedGeoGraphy.State.ItemId.ToString();

            #endregion
        }
    }
}
