using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Company
{
    public class CompanyUtil
    {
        #region Generic Catalogs

        /// <summary>
        /// general provider options
        /// </summary>
        public static List<ProveedoresOnLine.Company.Models.Util.CatalogModel> ProviderOptions
        {
            get
            {
                if (oProviderOptions == null)
                {
                    oProviderOptions =
                        ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions();
                }
                return oProviderOptions;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oProviderOptions;

        /// <summary>
        /// Get provider option name by item id
        /// </summary>
        /// <param name="vProviderOptionId">item id</param>
        /// <returns></returns>
        public static string GetProviderOptionName(string vProviderOptionId)
        {
            return ProviderOptions.
                        Where(po => po.ItemId.ToString() == vProviderOptionId).
                        Select(po => po.ItemName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
        }
        #endregion

        #region Generic geography

        /// <summary>
        /// all geography cities list
        /// </summary>
        public static List<ProveedoresOnLine.Company.Models.Util.GeographyModel> AllCities
        {
            get
            {
                if (oAllCities == null)
                {
                    int oTotalRows;
                    oAllCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography
                        (null, null, 0, 0, out oTotalRows);
                }
                return oAllCities;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oAllCities;

        /// <summary>
        /// get city name by city id
        /// </summary>
        /// <param name="vCityId">city id</param>
        /// <returns></returns>
        public static string GetCityName(string vCityId)
        {
            return AllCities.
                        Where(ci => ci.City != null && ci.City.ItemId.ToString() == vCityId).
                        Select(ci => ci.City.ItemName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
        }

        #endregion

        #region Generic Activity

        /// <summary>
        /// default activity
        /// </summary>
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> DefaultActivity
        {
            get
            {
                if (oDefaultActivity == null)
                {
                    oDefaultActivity =
                        ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity(null, 0, 0);
                }
                return oDefaultActivity;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oDefaultActivity;

        /// <summary>
        /// custom activity
        /// </summary>
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CustomActivity
        {
            get
            {
                if (oCustomActivity == null)
                {
                    oCustomActivity =
                        ProveedoresOnLine.Company.Controller.Company.CategorySearchByCustomActivity(null, 0, 0);
                }
                return oCustomActivity;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCustomActivity;

        #endregion

        #region HSEQ Category

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> Rule
        {
            get
            {
                if (oRule == null)
                {
                    oRule = ProveedoresOnLine.Company.Controller.Company.CategorySearchByRules(null, 0, 0);
                }
                return oRule;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oRule;

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CompanyRule
        {
            get
            {
                if (oCompanyRule == null)
                {
                    oCompanyRule = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCompanyRules(null, 0, 0);
                }
                return oCompanyRule;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCompanyRule;

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> ARL
        {
            get
            {
                if (oARL == null)
                {
                    oARL = ProveedoresOnLine.Company.Controller.Company.CategorySearchByARLCompany(null, 0, 0);
                }
                return oARL;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oARL;


        #endregion

        #region Financial Category

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> Bank
        {
            get
            {
                if (oBank == null)
                {
                    oBank = ProveedoresOnLine.Company.Controller.Company.CategorySearchByBank(null, 0, 0);
                }
                return oBank;
            }
        }
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oBank;


        #endregion
    }
}
