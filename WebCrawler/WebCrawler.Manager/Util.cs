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

            SearchParam = SearchParam.ToLower().Replace("d.c", "");

            string[] oSearchParam = SearchParam.Split(new char[] { '/', ',' });

            SearchParam = oSearchParam[0].Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            SearchParam = reg.Replace(SearchParam.Replace(" ", ""), "");

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

        #region Bank

        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oBankValues;
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> BankValues
        {
            get
            {
                if (oBankValues == null)
                {
                    int oTotalCount;
                    oBankValues = ProveedoresOnLine.Company.Controller.Company.CategorySearchByBankAdmin
                        (null, 0, 1000000, out oTotalCount);
                }
                return oBankValues;
            }
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel Bank_GetByName(string SearchParam)
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = null;

            string[] oSearchParam = SearchParam.Split(new char[] { '_' });

            SearchParam = oSearchParam[1].Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            SearchParam = reg.Replace(SearchParam.ToLower().Replace(" ", ""), "");

            if (oReturn == null)
            {
                //exact result
                oReturn = BankValues.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //like search
                oReturn = BankValues.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //get default bank info
                oReturn = BankValues.Where(x => x.ItemId == 1).FirstOrDefault();
            }

            return oReturn;
        }

        #endregion

        #region ICA

        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oICAValues;
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> ICAValues
        {
            get
            {
                if (oICAValues == null)
                {
                    int oTotalCount;
                    oICAValues = ProveedoresOnLine.Company.Controller.Company.CategorySearchByICA
                        (null, 0, 1000000, out oTotalCount);
                }
                return oICAValues;
            }
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel ICA_GetByName(string SearchParam)
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = null;

            SearchParam = SearchParam.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            SearchParam = reg.Replace(SearchParam.ToLower().Replace(" ", ""), "");

            if (oReturn == null)
            {
                //Exact search
                oReturn = ICAValues.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //Like search
                oReturn = ICAValues.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //Get default value
                oReturn = ICAValues.Where(x => x.ItemId == 1).FirstOrDefault();
            }

            return oReturn;
        }

        #endregion

        #region Certification Company

        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCertificationCompany;
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CertificationCompany
        {
            get
            {
                if (oCertificationCompany == null)
                {
                    int oTotalCount;
                    oCertificationCompany = ProveedoresOnLine.Company.Controller.Company.CategorySearchByCompanyRulesAdmin
                        (null, 0, 1000000, out oTotalCount);
                }
                return oCertificationCompany;
            }
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel CertificationCompany_GetByName(string SearchParam)
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = null;

            SearchParam = SearchParam.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            SearchParam = reg.Replace(SearchParam.ToLower().Replace(" ", ""), "");

            if (oReturn == null)
            {
                //exact search
                oReturn = CertificationCompany.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //like search
                oReturn = CertificationCompany.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //get default certification company
                oReturn = CertificationCompany.Where(x => x.ItemId == 1).FirstOrDefault();
            }

            return oReturn;
        }

        #endregion

        #region Certifications

        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCertifications;
        private static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> Certifications
        {
            get
            {
                if (oCertifications == null)
                {
                    int oTotalCount;
                    oCertifications = ProveedoresOnLine.Company.Controller.Company.CategorySearchByRulesAdmin
                        (null, 0, 1000000, out oTotalCount);
                }

                return oCertifications;
            }
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel Certifications_GetByName(string SearchParam)
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = null;

            SearchParam = SearchParam.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            SearchParam = reg.Replace(SearchParam.ToLower().Replace(" ", ""), "");

            if (oReturn == null)
            {
                //exact search
                oReturn = Certifications.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //like search
                oReturn = Certifications.Where(x => reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
            }

            if (oReturn == null)
            {
                //get default certification
                oReturn = Certifications.Where(x => x.ItemId == 1).FirstOrDefault();
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
                else if (SearchParam == "pp")
                {
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == "pasaporte").FirstOrDefault();
                }
            }
            
            if (CatalogId == 210)
            {
                if (SearchParam == "comercialylegal")
                {
                    SearchParam = "legalycomercial";
                }
                else if (SearchParam == "hsesms")
                {
                    SearchParam = "hseq";
                }

                if (oReturn == null)
                {
                    //exact search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();   
                }

                if (oReturn == null)
                {
                    //like search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //get default value
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId).FirstOrDefault();
                }
            }

            if (CatalogId == 219)
            {
                if (oReturn == null)
                {
                    //Exact search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //like search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //get default value
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId).FirstOrDefault();
                }
            }

            if (CatalogId == 213)
            {
                if (oReturn == null)
                {
                    //Exact search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //like search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //get default value
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId).FirstOrDefault();
                }
            }

            if (CatalogId == 214)
            {
                if (oReturn == null)
                {
                    //Exact search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //like search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //get default value
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId).FirstOrDefault();
                }
            }

            if (CatalogId == 215)
            {
                if (oReturn == null)
                {
                    //Exact search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "") == SearchParam).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //like search
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId && reg.Replace(x.ItemName.ToLower().Replace(" ", ""), "").Contains(SearchParam)).FirstOrDefault();
                }

                if (oReturn == null)
                {
                    //get default value
                    oReturn = ProviderOptions.Where(x => x.CatalogId == CatalogId).FirstOrDefault();
                }
            }

            return oReturn;
        }

        #endregion
    }
}
