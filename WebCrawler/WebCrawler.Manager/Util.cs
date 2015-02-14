using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

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

            string[] oSearchParam = SearchParam.Split(new char[] { '/' });

            SearchParam = oSearchParam[0].Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            SearchParam = reg.Replace(SearchParam.ToLower().Replace(" ", ""), "");

            if (oReturn == null)
            {
                //exact search
                oReturn = GeographyValues.Where(x => reg.Replace(x.City.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //like search
                oReturn = GeographyValues.Where(x => x.City.ItemName.ToLower().Replace(" ", "").Contains(SearchParam.ToLower().Replace(" ", ""))).FirstOrDefault();
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

        public static ProveedoresOnLine.Company.Models.Util.CatalogModel ProviderOptions_GetByName(int CatalogId, string SearchParam)
        {
            ProveedoresOnLine.Company.Models.Util.CatalogModel oReturn = null;

            SearchParam = SearchParam.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            SearchParam = reg.Replace(SearchParam.ToLower().Replace(" ", ""), "");

            if (CatalogId == 101)
            {
                if (SearchParam == "cc")
                {
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == "ceduladeciudadania").FirstOrDefault();
                }
                else
                {
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == "pasaporte").FirstOrDefault();
                }
            }
            else if (CatalogId == 210)
            {
                if (SearchParam == "comercialylegal")
                {
                    SearchParam = "legalycomercial";
                }
                else if (SearchParam == "hsesms")
                {
                    SearchParam = "hseq";
                }

                oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
            }

            //if (cols[3].InnerText == "Comercial")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Commercial);
            //}
            //else if (cols[3].InnerText == "Legal")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Legal);
            //}
            //else if (cols[3].InnerText == "Legal Suplente")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Legal);
            //}
            //else if (cols[3].InnerText == "Comercial y Legal")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_CommercialLegal);
            //}
            //else if (cols[3].InnerText == "Financiero")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Finantial);
            //}
            //else if (cols[3].InnerText == "HSE - SMS")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_HSEQ);
            //}
            //else if (cols[3].InnerText == "Jurídico")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Judicial);
            //}
            //else if (cols[3].InnerText == "Técnico")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Technical);
            //}
            //else if (cols[3].InnerText == "Compras y Contratos")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Buy);
            //}
            //else if (cols[3].InnerText == "Seguridad Física")
            //{
            //    CommercialType = Convert.ToString((int)enumCategoryInfoType.CP_Security);
            //}

            return oReturn;
        }

        #endregion
    }
}
