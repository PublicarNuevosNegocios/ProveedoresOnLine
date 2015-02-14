using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Manager
{
    public class Util
    {
        #region Geography

        private static List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oGeographyValues;
        private static List<ProveedoresOnLine.Company.Models.Util.GeographyModel> GeographyValues
        {
            get
            {
                if (oGeographyValues == null)
                {
                    int oTotalCount;
                    oGeographyValues = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeographyAdmin
                        (null, 0, 1000000, out oTotalCount);
                }
                return oGeographyValues;
            }
        }

        public static ProveedoresOnLine.Company.Models.Util.GeographyModel Geography_GetByName(string SearchParam)
        {
            ProveedoresOnLine.Company.Models.Util.GeographyModel oReturn = null;

            if (oReturn == null)
            {
                //exact search
                oReturn = GeographyValues.Where(x => x.City.ItemName.ToLower().Replace(" ", "") == SearchParam.ToLower().Replace(" ", "")).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //like search
                oReturn = GeographyValues.Where(x => x.City.ItemName.Contains(SearchParam)).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //get default city
                oReturn = GeographyValues.Where(x => x.City.ItemId == 1).FirstOrDefault();
            }

            return oReturn;
        }

        #endregion

        #region Provider Options

        private static List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oProviderOptions;
        private static List<ProveedoresOnLine.Company.Models.Util.CatalogModel> ProviderOptions
        {
            get
            {
                if (oProviderOptions == null)
                {
                    oProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions();
                }
                return oProviderOptions;
            }
        }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel ProviderOptions_GetByName(int CatalogId, string SearchParam)
        {
            ProveedoresOnLine.Company.Models.Util.CatalogModel oReturn = null;

            



            return oReturn;
        }

        #endregion
    }
}
