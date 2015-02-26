using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ProviderController : BaseController
    {

        public virtual ActionResult Index()
        {
            return View();
        }

        #region Provider search

        public virtual ActionResult Search
            (string CompareId,
            string SearchParam,
            string SearchFilter,
            string SearchOrderType,
            string OrderOrientation,
            string PageNumber)
        {
            MarketPlace.Models.Provider.ProviderSearchViewModel oModel = null;

            if (MarketPlace.Models.General.SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId))
            {

                //get basic search model
                oModel = new Models.Provider.ProviderSearchViewModel()
                {
                    ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                    SearchParam = SearchParam,
                    SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                    SearchOrderType = string.IsNullOrEmpty(SearchOrderType) ? MarketPlace.Models.General.enumSearchOrderType.Relevance : (MarketPlace.Models.General.enumSearchOrderType)Convert.ToInt32(SearchOrderType),
                    OrderOrientation = string.IsNullOrEmpty(OrderOrientation) ? false : ((OrderOrientation.Trim().ToLower() == "1") || (OrderOrientation.Trim().ToLower() == "true")),
                    PageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber),
                    ProviderSearchResult = new List<Models.Provider.ProviderLiteViewModel>(),
                };

                //search providers
                int oTotalRowsAux;
                List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> oProviderResult =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearch
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    (int)oModel.SearchOrderType,
                    oModel.OrderOrientation,
                    oModel.PageNumber,
                    oModel.RowCount,
                    out oTotalRowsAux);

                oModel.TotalRows = oTotalRowsAux;
                oModel.ProviderFilterResult =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilter
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    oModel.SearchParam,
                    oModel.SearchFilter);

                //parse view model
                if (oProviderResult != null && oProviderResult.Count > 0)
                {
                    oProviderResult.All(prv =>
                    {
                        oModel.ProviderSearchResult.Add
                            (new MarketPlace.Models.Provider.ProviderLiteViewModel(prv));

                        return true;
                    });

                }

                if (!string.IsNullOrEmpty(CompareId))
                {
                    //get current compare 
                    ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                        CompareGetCompanyBasicInfo
                        (Convert.ToInt32(CompareId.Replace(" ", "")),
                        MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                        MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                    oModel.RelatedCompare = new Models.Compare.CompareViewModel(oCompareResult);
                }
            }

            return View(oModel);
        }

        #endregion

        #region General Info

        public virtual ActionResult GIProviderInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                #region Basic Info
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCompanyGetBasicInfo(ProviderPublicId);

                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.PersonContact);
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }

                #endregion

                #region Legal Info

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.ChaimberOfCommerce);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {                   
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                #endregion
                                
                #region Branch Info
                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.Brach);

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }
                #endregion
                                
                #region Black List Info
                oModel.RelatedBlackListInfo = ProveedoresOnLine.Company.Controller.Company.BlackListGetByCompanyPublicId(ProviderPublicId); 
                #endregion
                                
                #region Tracking Info
                oModel.RelatedTrackingInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetTracking(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId); 
                #endregion
                                
                #region Basic Financial Info
                
                List<GenericItemModel> oFinancial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetLastyearInfoDeta(ProviderPublicId);
                oModel.RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                if (oFinancial != null)
                {
                    oFinancial.All(x =>
                    {
                        oModel.RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(x));
                        return true;
                    });

                    if (oModel.RelatedFinancialBasicInfo != null && oModel.RelatedFinancialBasicInfo.Count > 0)
                    {
                        oModel.RelatedFinancialBasicInfo.FirstOrDefault().BI_JobCapital =
                            (Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_CurrentActive != null).Select(x => x.BI_CurrentActive).FirstOrDefault())
                            - Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_CurrentActive != null).Select(x => x.BI_CurrentPassive).FirstOrDefault())).ToString("#,0,##");
                    }
                }                               
                #endregion

                //Get Engagement info
                                
                #region HSEQ

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies);
                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
                }
 
                #endregion

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult GIPersonContactInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.ContactCompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.PersonContact);
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult GIBranchInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            int oTotalRows;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.ContactCompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.Brach);
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oCities = null;

                oCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, null, 0, 0, out oTotalRows);

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
                oModel.RelatedGeneralInfo = oModel.RelatedGeneralInfo.OrderBy(x => x.BR_IsPrincipal).ToList();
            }
            return View(oModel);
        }

        public virtual ActionResult GIDistributorInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            int oTotalRows;

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.ContactCompanyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();

                oModel.ContactCompanyInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.Distributor);
                oModel.RelatedGeneralInfo = new List<ProviderContactViewModel>();

                List<ProveedoresOnLine.Company.Models.Util.GeographyModel> oCities = null;
                oCities = ProveedoresOnLine.Company.Controller.Company.CategorySearchByGeography(null, null, 0, 0, out oTotalRows);

                if (oModel.ContactCompanyInfo != null)
                {
                    oModel.ContactCompanyInfo.All(x =>
                    {
                        oModel.RelatedGeneralInfo.Add(new ProviderContactViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion

        #region Commercial Info

        public virtual ActionResult CIExperiencesInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCommercialGetBasicInfo(ProviderPublicId, (int)enumCommercialType.Experience);

                oModel.RelatedComercialInfo = new List<ProviderComercialViewModel>();

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial.All(x =>
                {
                    oModel.RelatedComercialInfo.Add(new ProviderComercialViewModel(x));
                    return true;
                });

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HICertificationsInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.Certifications);
                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
                }


                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult HIHealtyPoliticInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyHealtyPolitic);
                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult HIRiskPoliciesInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo
                    (ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies);

                oModel.RelatedHSEQlInfo = new List<ProviderHSEQViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.All(x =>
                    {
                        oModel.RelatedHSEQlInfo.Add(new ProviderHSEQViewModel(x));
                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion

        #region Financial Info

        public virtual ActionResult FIBalanceSheetInfo(string ProviderPublicId, string ViewName, string Year, string Currency)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get request info
                int? oYear = !string.IsNullOrEmpty(Request["Year"]) ?
                    (int?)Convert.ToInt32(Request["Year"].Replace(" ", "")) :
                    null;

                int oCurrencyType = !string.IsNullOrEmpty(Request["Currency"]) ?
                    Convert.ToInt32(Request["Currency"].Replace(" ", "")) :
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_USD].Value);

                string oViewName = !string.IsNullOrEmpty(Request["ViewName"]) ?
                    Request["ViewName"].Replace(" ", "") :
                    MVC.Desktop.Shared.Views.ViewNames._P_FI_Indicators;

                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCompanyGetBasicInfo(ProviderPublicId);

                //get account info
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> olstAccount =
                    ProveedoresOnLine.Company.Controller.Company.CategoryGetFinantialAccounts();

                //get balance info
                List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> oBalanceAux =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPBalanceSheetGetByYear
                    (ProviderPublicId,
                    oYear,
                    oCurrencyType);


                //fill view models
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();
                if (oBalanceAux != null && oBalanceAux.Count > 0)
                {
                    oBalanceAux.All(bs =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(bs));
                        return true;
                    });
                }

                oModel.RelatedBalanceSheetInfo = new List<ProviderBalanceSheetViewModel>();
                if (oBalanceAux != null && oBalanceAux.Count > 0 && olstAccount != null && olstAccount.Count > 0)
                {
                    oModel.RelatedBalanceSheetInfo = GetBalanceSheetViewModel
                        (null,
                        olstAccount,
                        oBalanceAux,
                        oViewName);
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult FITaxInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.TaxInfoType);
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial.All(x =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult FIIncomeStatementInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.IncomeStatementInfoType);
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial.All(x =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult FIBankInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.BankInfoType);
                oModel.RelatedFinancialInfo = new List<ProviderFinancialViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial.All(x =>
                    {
                        oModel.RelatedFinancialInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #region Private Methods

        private List<ProviderBalanceSheetViewModel> GetBalanceSheetViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> lstAccount,
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> lstBalanceSheet,
            string oViewName)
        {
            List<ProviderBalanceSheetViewModel> oReturn = new List<ProviderBalanceSheetViewModel>();

            lstAccount.
                Where(ac =>
                    RelatedAccount != null ?
                        (ac.ParentItem != null && ac.ParentItem.ItemId == RelatedAccount.ItemId) :
                        (ac.ParentItem == null &&
                            (oViewName == MVC.Desktop.Shared.Views.ViewNames._P_FI_Indicators ? ac.ItemId == 4915 : ac.ItemId != 4915))).
                OrderBy(ac => ac.ItemInfo.
                    Where(aci => aci.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Order).
                    Select(aci => Convert.ToInt32(aci.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                All(ac =>
                {
                    ProviderBalanceSheetViewModel oItemToAdd =
                        new Models.Provider.ProviderBalanceSheetViewModel()
                        {
                            RelatedAccount = ac,
                            RelatedBalanceSheetDetail = new List<ProviderBalanceSheetDetailViewModel>(),
                        };

                    int oOrder = 0;
                    decimal oHorizontalValue = 0;
                    string oAccountUnit = ac.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Unit).
                        Select(y => y.Value.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    lstBalanceSheet.
                        //Where(bs => bs.BalanceSheetInfo != null && bs.BalanceSheetInfo.Count > 0).
                        OrderByDescending(bs => bs.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.SH_Year).
                            Select(y => Convert.ToInt32(y.Value)).
                            DefaultIfEmpty(DateTime.Now.Year).
                            FirstOrDefault()).
                        All(bs =>
                        {
                            var oItemDetailToAdd = new ProviderBalanceSheetDetailViewModel()
                            {
                                Order = oOrder,
                            };

                            //get balance to add
                            if (bs.BalanceSheetInfo != null && 
                                bs.BalanceSheetInfo.Count > 0 && 
                                bs.BalanceSheetInfo.Any(bsd => bsd.RelatedAccount.ItemId == ac.ItemId))
                            {
                                //get item to add
                                oItemDetailToAdd = new ProviderBalanceSheetDetailViewModel()
                                {
                                    RelatedBalanceSheetDetail = bs.BalanceSheetInfo.
                                        Where(bsd => bsd.RelatedAccount.ItemId == ac.ItemId).
                                        FirstOrDefault(),
                                };
                            }
                            else
                            {
                                oItemDetailToAdd.RelatedBalanceSheetDetail = new BalanceSheetDetailModel()
                                {
                                    RelatedAccount = ac,
                                    Value = 0,
                                };
                            }

                            #region Eval Vertical Formula

                            string strVerticalFormula = ac.ItemInfo.
                                Where(x => x.ItemInfoType.ItemId == (int)enumCategoryInfoType.AI_VerticalFormula).
                                Select(x => x.LargeValue).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault().ToLower().Replace(" ", "");

                            if (!string.IsNullOrEmpty(strVerticalFormula))
                            {
                                //loop for standar values
                                foreach (var RegexResult in (new Regex("\\[+\\d*\\]+", RegexOptions.IgnoreCase)).Matches(strVerticalFormula))
                                {
                                    int oAccountId = Convert.ToInt32(RegexResult.ToString().Replace("[", "").Replace("]", ""));

                                    decimal oAccountValue = bs.BalanceSheetInfo.
                                        Where(bsd => bsd.RelatedAccount.ItemId == oAccountId).
                                        Select(bsd => bsd.Value != 0 ? bsd.Value : 1).
                                        DefaultIfEmpty(1).
                                        FirstOrDefault();

                                    strVerticalFormula = strVerticalFormula.Replace
                                        (RegexResult.ToString(),
                                        oAccountValue.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")));
                                }
                                oItemDetailToAdd.VerticalValue = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MathEval(strVerticalFormula);
                            }

                            #endregion

                            //add balance detail value
                            oItemToAdd.RelatedBalanceSheetDetail.Add(oItemDetailToAdd);
                            oOrder++;

                            if (oAccountUnit == "$" && oItemDetailToAdd.RelatedBalanceSheetDetail != null)
                            {
                                #region Horizontal value
                                //calc horizontal value
                                if (oOrder == 1)
                                {
                                    oHorizontalValue = oItemDetailToAdd.RelatedBalanceSheetDetail.Value;
                                }
                                else if (oOrder > 1)
                                {
                                    oHorizontalValue = oHorizontalValue - oItemDetailToAdd.RelatedBalanceSheetDetail.Value;
                                }
                                #endregion
                            }

                            return true;
                        });


                    //add horizontal analisis value
                    oItemToAdd.HorizontalValue = oHorizontalValue;

                    //get child values
                    oItemToAdd.ChildBalanceSheet = GetBalanceSheetViewModel
                        (ac,
                        lstAccount,
                        lstBalanceSheet,
                        oViewName);

                    //add account item
                    oReturn.Add(oItemToAdd);

                    return true;
                });

            return oReturn;
        }

        #endregion

        #endregion

        #region Legal Info

        public virtual ActionResult LIChaimberOfCommerceInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.ChaimberOfCommerce);

                List<GenericItemModel> oDesignations = null;

                oDesignations = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.Designations);
                oModel.RelatedDesignationsInfo = new List<ProviderDesignationsViewModel>();
                int oTotalRows;

                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }
                else
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>();
                }
                oModel.RelatedDesignationsInfo = new List<ProviderDesignationsViewModel>();
                if (oDesignations != null && oDesignations.Count > 0)
                {
                    oDesignations.All(x =>
                    {
                        oModel.RelatedDesignationsInfo.Add(new ProviderDesignationsViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LIRutInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.RUT);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LICIFINInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.CIFIN);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LISARLAFTInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.SARLAFT);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();
                List<CatalogModel> oEntitieType = MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where(x => x.CatalogId == 212).Select(x => x).ToList();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LIResolutionInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == ProviderPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == ProviderPublicId)).
                FirstOrDefault();

            //validate provider permisions
            if (oProvider == null)
            {
                //return url provider not allowed
            }
            else
            {
                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.Resoluciones);
                oModel.RelatedLegalInfo = new List<ProviderLegalViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal.All(x =>
                    {
                        oModel.RelatedLegalInfo.Add(new ProviderLegalViewModel(x));
                        return true;
                    });
                }
                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }
        #endregion

        #region Menu

        private List<GenericMenu> GetProviderMenu(ProviderViewModel vProviderInfo)
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            if (vProviderInfo.RelatedLiteProvider != null)
            {
                string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
                string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

                #region GeneralInfo

                //header
                MarketPlace.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información general",
                    Position = 0,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Basic info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información básica",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.GIProviderInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company persons Contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información de personas de contacto",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.GIPersonContactInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIPersonContactInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Branch
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sucursales",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.GIBranchInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIBranchInfo &&
                        oCurrentController == MVC.Provider.Name),
                });


                //Distributors
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Distribuidores",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.GIDistributorInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIDistributorInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Commercial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información comercial",
                    Position = 1,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Experience
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Experiencias",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.CIExperiencesInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.CIExperiencesInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region HSEQ Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "HSEQ",
                    Position = 2,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Certifications
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Certificaciones",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.HICertificationsInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HICertificationsInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company healty politic
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Salud, medio ambiente y seguridad",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.HIHealtyPoliticInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HIHealtyPoliticInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company healty politic
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Salud de riesgos laborales",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.HIRiskPoliciesInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.HIRiskPoliciesInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);
                #endregion

                #region Financial Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información Financiera",
                    Position = 3,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Balancesheet info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Estados financieros",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.FIBalanceSheetInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FIBalanceSheetInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Tax Info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Impuestos",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.FITaxInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FITaxInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //income statement
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Declaración de renta",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.FIIncomeStatementInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FIIncomeStatementInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Bank Info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información bancaria",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.FIBankInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.FIBankInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);
                #endregion

                #region Legal Info

                //header
                oMenuAux = new Models.General.GenericMenu()
                {
                    Name = "Información Legal",
                    Position = 4,
                    ChildMenu = new List<Models.General.GenericMenu>(),
                };

                //Balancesheet info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Cámara de comercio",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.LIChaimberOfCommerceInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LIChaimberOfCommerceInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //RUT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Registro único tributario",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.LIRutInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LIRutInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //CIFIN
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "CIFIN",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.LICIFINInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LICIFINInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //SARLAFT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "SARLAFT",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.LISARLAFTInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LISARLAFTInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Resoluciones",
                    Url = Url.RouteUrl
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.LIResolutionInfo,
                                ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                            }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.LIResolutionInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

                #endregion
            }
            return oReturn;
        }

        #endregion
    }
}