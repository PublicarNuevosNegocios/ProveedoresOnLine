using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using Microsoft.Reporting.WebForms;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            ProviderViewModel oModel = new ProviderViewModel();
            return View(oModel);
        }

        #region Provider search

        public virtual ActionResult Search
            (string ProjectPublicId,
            string CompareId,
            string SearchParam,
            string SearchFilter,
            string SearchOrderType,
            string OrderOrientation,
            string PageNumber)
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

                #region Providers
                //search providers
                int oTotalRowsAux;
                List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> oProviderResult =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchNew
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    (int)oModel.SearchOrderType,
                    oModel.OrderOrientation,
                    oModel.PageNumber,
                    oModel.RowCount,
                    out oTotalRowsAux);

                oModel.TotalRows = oTotalRowsAux;
                
                List<GenericFilterModel> oFilterModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilterNew
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false);

                if (oFilterModel != null)
                {
                    oModel.ProviderFilterResult = oFilterModel.Where(x => x.CustomerPublicId == MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId).ToList();
                }

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

                #endregion

                if (!string.IsNullOrEmpty(ProjectPublicId))
                {
                    #region Project

                    //get current project
                    ProveedoresOnLine.ProjectModule.Models.ProjectModel oProjectResult = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                        ProjectGetByIdLite
                        (ProjectPublicId,
                        MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                    if (oProjectResult != null && !string.IsNullOrEmpty(oProjectResult.ProjectPublicId))
                    {
                        oModel.RelatedProject = new Models.Project.ProjectViewModel(oProjectResult);
                    }
                    #endregion
                }
                else
                {
                    #region Compare
                    if (!string.IsNullOrEmpty(CompareId))
                    {
                        //get current compare 
                        ProveedoresOnLine.CompareModule.Models.CompareModel oCompareResult = ProveedoresOnLine.CompareModule.Controller.CompareModule.
                            CompareGetCompanyBasicInfo
                            (Convert.ToInt32(CompareId.Replace(" ", "")),
                            MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email,
                            MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                        if (oCompareResult != null && oCompareResult.CompareId > 0)
                        {
                            oModel.RelatedCompare = new Models.Compare.CompareViewModel(oCompareResult);
                        }
                    }

                    #endregion

                    #region Project config

                    //get project config items
                    List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> oProjectConfigResult = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.
                        MPProjectConfigGetByCustomer(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                    if (oProjectConfigResult != null && oProjectConfigResult.Count > 0)
                    {
                        oModel.RelatedProjectConfig = new List<Models.Project.ProjectConfigViewModel>();

                        oProjectConfigResult.All(pjc =>
                        {
                            oModel.RelatedProjectConfig.Add(new Models.Project.ProjectConfigViewModel(pjc));
                            return true;
                        });
                    }

                    #endregion
                }
            }

            return View(oModel);
        }

        #endregion

        #region General Info

        public virtual ActionResult GIProviderInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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
                oModel.RelatedBlackListInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListGetBasicInfo(ProviderPublicId);


                #endregion

                #region Tracking Info
                oModel.RelatedTrackingInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetTracking(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);
                #endregion

                #region Basic Financial Info

                List<GenericItemModel> oFinancial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetLastyearInfoDeta(ProviderPublicId);
                oModel.RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                if (oFinancial != null)
                {
                    decimal oExchange;
                    oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemName));

                    oFinancial.All(x =>
                    {
                        oModel.RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(x, oExchange));
                        return true;
                    });

                    if (oModel.RelatedFinancialBasicInfo != null && oModel.RelatedFinancialBasicInfo.Count > 0)
                    {
                        oModel.RelatedFinancialBasicInfo.FirstOrDefault().BI_JobCapital =
                            ((Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentActive)).Select(x => x.BI_CurrentActive).DefaultIfEmpty("0").FirstOrDefault())
                            - Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentPassive)).Select(x => x.BI_CurrentPassive).DefaultIfEmpty("0").FirstOrDefault()))).ToString("#,0.##");
                    }
                }
                #endregion

                #region K_Contract Info
                List<GenericItemModel> oRelatedKContractInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.KRecruitment, true);

                oModel.RelatedKContractInfo = new List<ProviderFinancialViewModel>();
                if (oRelatedKContractInfo != null)
                {
                    oRelatedKContractInfo.All(x =>
                    {
                        oModel.RelatedKContractInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }
                #endregion

                //Get Engagement info                                
                #region HSEQ

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies);
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null
                && oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Count > 0)
                {
                    List<GenericItemModel> basicCert = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.Certifications);
                    if (basicCert != null && basicCert.Count > 0)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.AddRange(basicCert);
                    }
                }

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

                oModel.RelatedCertificationBasicInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetSpecificCert(ProviderPublicId);

                #endregion

                #region Black List



                #endregion

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult GIPersonContactInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;
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

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;
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
                oModel.RelatedGeneralInfo = oModel.RelatedGeneralInfo.OrderByDescending(x => x.BR_IsPrincipal).ToList();
            }
            return View(oModel);
        }

        public virtual ActionResult GIDistributorInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

        public virtual ActionResult GITrackingInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;
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
                ////get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedTrackingInfo = new List<GenericItemModel>();

                //oModel.RelatedTrackingInfo = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCustomerProviderGetAllTracking(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion

        #region Legal Info

        public virtual ActionResult LIChaimberOfCommerceInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;
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

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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
                    oModel.RelatedLegalInfo = oModel.RelatedLegalInfo.OrderByDescending(x => Convert.ToDateTime(x.SF_ProcessDate)).ToList();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult LIResolutionInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

        #region Financial Info

        public virtual ActionResult FIBalanceSheetInfo(string ProviderPublicId, string ViewName, string Year, string Currency)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

                int oCurrencyValidate = 0;

                int oCurrencyType = !string.IsNullOrEmpty(Request["Currency"]) && int.TryParse(Request["Currency"].ToString(), out oCurrencyValidate) == true ?
                    Convert.ToInt32(Request["Currency"].Replace(" ", "")) :
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_COP].Value);

                string oViewName = !string.IsNullOrEmpty(Request["ViewName"]) ?
                    Request["ViewName"].Replace(" ", "") :
                    MVC.Desktop.Shared.Views.ViewNames._P_FI_Indicators;

                //get provider view model
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCompanyGetBasicInfo(ProviderPublicId);

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
                if (oBalanceAux != null && oBalanceAux.Count > 0)
                {
                    List<BalanceSheetModel> oBalancetemp = new List<BalanceSheetModel>();


                    foreach (var item in oBalanceAux)
                    {
                        if (item.BalanceSheetInfo.Count == 0)
                        {
                            oBalancetemp.Add(item);
                        }
                    }

                    int cont = 0;
                    foreach (var item in oBalancetemp)
                    {
                        if (cont < 1)
                        {
                            oBalanceAux.Remove(item);
                            cont++;
                        }


                    }

                    oModel.RelatedBalanceSheetInfo = GetBalanceSheetViewModel
                        (null,
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

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

                    oModel.RelatedFinancialInfo = oModel.RelatedFinancialInfo.OrderByDescending(x => Convert.ToInt32(!string.IsNullOrEmpty(x.TX_Year) ? x.TX_Year : string.Empty)).ToList();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult FIIncomeStatementInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

        public virtual ActionResult FIKContract(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;
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

                oModel.RelatedLiteProvider.RelatedProvider.RelatedFinantial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPFinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.KRecruitment);
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
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> lstBalanceSheet,
            string oViewName)
        {
            List<ProviderBalanceSheetViewModel> oReturn = new List<ProviderBalanceSheetViewModel>();

            MarketPlace.Models.Company.CompanyUtil.FinancialAccount.
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
                                        oAccountValue.ToString("0.0").Replace(",", "."));
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

        #region Commercial Info

        public virtual ActionResult CIExperiencesInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCommercialGetBasicInfo
                    (ProviderPublicId,
                    (int)enumCommercialType.Experience,
                    MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);

                oModel.RelatedComercialInfo = new List<ProviderComercialViewModel>();
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial != null
                    && oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial.Count > 0)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial.All(x =>
                    {
                        oModel.RelatedComercialInfo.Add(new ProviderComercialViewModel(x));
                        return true;
                    });
                }
                else
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedCommercial = new List<GenericItemModel>();

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HICertificationsInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

                    oModel.RelatedHSEQlInfo = oModel.RelatedHSEQlInfo.OrderByDescending(x => !string.IsNullOrEmpty(x.CH_Year) ? Convert.ToInt32(x.CH_Year).ToString() : "0").ToList();
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
        }

        public virtual ActionResult HIRiskPoliciesInfo(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

                List<GenericItemModel> certARL = new List<GenericItemModel>();
                List<GenericItemModel> certAccident = new List<GenericItemModel>();
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = new List<GenericItemModel>();

                certAccident = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo
                   (ProviderPublicId, (int)enumHSEQType.CertificatesAccident);

                if (certAccident != null)
                {
                    foreach (var item in certAccident)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Add(item);
                    }
                }


                certARL = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCertificationGetBasicInfo
                    (ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies);

                if (certARL != null)
                {
                    foreach (var item in certARL)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Add(item);
                    }
                }


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

        #region Survey        
        public virtual ActionResult SVSurveySearch
            (string ProviderPublicId,
            string SearchOrderType,
            string OrderOrientation,
            string PageNumber,
            string InitDate,
            string EndDate)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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
                oModel.ProviderMenu = GetProviderMenu(oModel);

                oModel.RelatedSurveySearch = new Models.Survey.SurveySearchViewModel()
                {
                    SearchOrderType = string.IsNullOrEmpty(SearchOrderType) ? MarketPlace.Models.General.enumSurveySearchOrderType.LastModify : (MarketPlace.Models.General.enumSurveySearchOrderType)Convert.ToInt32(SearchOrderType),
                    OrderOrientation = string.IsNullOrEmpty(OrderOrientation) ? false : ((OrderOrientation.Trim().ToLower() == "1") || (OrderOrientation.Trim().ToLower() == "true")),
                    PageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber),
                    SurveySearchResult = new List<Models.Survey.SurveyViewModel>(),
                };

                if (MarketPlace.Models.General.SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId))
                {
                    //search survey
                    int oTotalRowsAux;
                    List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> oSurveyResults = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveySearch
                            (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                            oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId,
                            (int)oModel.RelatedSurveySearch.SearchOrderType,
                            oModel.RelatedSurveySearch.OrderOrientation,
                            oModel.RelatedSurveySearch.PageNumber,
                            oModel.RelatedSurveySearch.RowCount,
                            out oTotalRowsAux);

                    //Validar q no tenga evaluaciones
                    if (SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().RelatedRole.ParentItem == null)
                    {
                        if (oSurveyResults != null)
                        {
                            oSurveyResults = oSurveyResults.Where(x => x.ParentSurveyPublicId == null).Select(x => x).ToList();
                        }
                    }
                    else
                    {
                        if (oSurveyResults != null)
                        {
                            List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> oChildSurvey = new List<ProveedoresOnLine.SurveyModule.Models.SurveyModel>();
                            oSurveyResults.All(x =>
                                {
                                    oChildSurvey.Add(ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(x.SurveyPublicId, SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().User));

                                    oChildSurvey.All(y =>
                                    {
                                        if (y != null && y.ParentSurveyPublicId == x.SurveyPublicId && y.SurveyInfo.Count == 0)
                                            y.SurveyInfo = x.SurveyInfo;

                                        return true;
                                    });
                                    return true;
                                });
                            oSurveyResults = oChildSurvey.Where(x => x != null).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate)
                        && oSurveyResults != null && oSurveyResults.Count > 0)
                    {
                        oSurveyResults = oSurveyResults.Where(x =>
                                                       Convert.ToDateTime(x.CreateDate.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(InitDate) &&
                                                       Convert.ToDateTime(x.CreateDate.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(EndDate)).
                                                        Select(x => x).ToList();

                    }
                    oModel.RelatedSurveySearch.TotalRows = oTotalRowsAux;

                    //parse view model
                    if (oSurveyResults != null && oSurveyResults.Count > 0)
                    {
                        //Get the Average
                        decimal Average = 0;
                        //Get ClosedSurve
                        List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> ClosedSurvey = oSurveyResults.Where(x => x.SurveyInfo.
                                                        Where(y => y.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).
                                                        Select(y => y.Value == ((int)enumSurveyStatus.Close).ToString()).FirstOrDefault()).
                                                        Select(x => x).ToList();
                        oSurveyResults.All(srv =>
                        {
                            oModel.RelatedSurveySearch.SurveySearchResult.Add
                                (new MarketPlace.Models.Survey.SurveyViewModel(srv));
                            return true;
                        });

                        if (oModel.RelatedSurveySearch.SurveySearchResult != null && oModel.RelatedSurveySearch.SurveySearchResult.Count > 0)
                        {
                            oModel.RelatedSurveySearch.SurveySearchResult.All(sv =>
                            {
                                if (sv.SurveyStatus == enumSurveyStatus.Close)
                                    Average = (Average += Convert.ToDecimal(sv.SurveyRating.ToString("#,0.##")));
                                return true;
                            });
                            Average = Average != 0 ? Average / oModel.RelatedSurveySearch.SurveySearchResult.Where(x => x.SurveyStatus == enumSurveyStatus.Close).Count() : 0;
                        }

                        //Set the Average
                        oModel.RelatedSurveySearch.SurveySearchResult.FirstOrDefault().Average = Average;
                        if (!string.IsNullOrEmpty(InitDate) && !string.IsNullOrEmpty(EndDate))
                        {
                            oModel.RelatedSurveySearch.SurveySearchResult.FirstOrDefault().FilterDateIni = Convert.ToDateTime(InitDate);
                            oModel.RelatedSurveySearch.SurveySearchResult.FirstOrDefault().FilterEndDate = Convert.ToDateTime(EndDate);
                        }
                    }
                }
            }
            #region report generator
            if (Request["UpsertRequest"] == "true")
            {
                List<ReportParameter> parameters = new List<ReportParameter>();
                ProviderModel oToInsert = new ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                    },
                    RelatedReports = new List<GenericItemModel>(),
                };
                oToInsert.RelatedReports.Add(this.GetSurveyReportFilterRequest());
                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPReportUpsert(oToInsert);

                parameters.Add(new ReportParameter("currentCompanyName", SessionModel.CurrentCompany.CompanyName));
                parameters.Add(new ReportParameter("currentCompanyTipoDni", SessionModel.CurrentCompany.IdentificationType.ItemName));
                parameters.Add(new ReportParameter("currentCompanyDni", SessionModel.CurrentCompany.IdentificationNumber));
                parameters.Add(new ReportParameter("currentCompanyLogo", SessionModel.CurrentCompany_CompanyLogo));
                parameters.Add(new ReportParameter("providerName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName.ToString()));
                parameters.Add(new ReportParameter("providerTipoDni", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName.ToString()));
                parameters.Add(new ReportParameter("providerDni", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber.ToString()));
                //order items reports
                if (oToInsert.RelatedReports != null)
                {
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("remarks",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_Observation).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault()));
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("actionPlan",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ImprovementPlan).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault()));
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("dateStart", Convert.ToDateTime(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                 (int)MarketPlace.Models.General.enumSurveyInfoType.RP_InitDateReport).Select(y => y.Value).
                                 DefaultIfEmpty(string.Empty).
                                 FirstOrDefault()).ToString("dd/MM/yyyy")));
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("dateEnd", Convert.ToDateTime(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                           (int)MarketPlace.Models.General.enumSurveyInfoType.RP_EndDateReport).Select(y => y.Value).
                           DefaultIfEmpty(string.Empty).
                           FirstOrDefault()).ToString("dd/MM/yyyy")));
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("average",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportAverage).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault()));
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("reportDate",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportDate).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault()));
                        return true;
                    });
                    oToInsert.RelatedReports.All(x =>
                    {
                        parameters.Add(new ReportParameter("responsible",
                            x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                            (int)MarketPlace.Models.General.enumSurveyInfoType.RP_ReportResponsable).Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault()));
                        return true;
                    });
                }
                parameters.Add(new ReportParameter("author", SessionModel.CurrentCompanyLoginUser.RelatedUser.Name.ToString() + " " + SessionModel.CurrentCompanyLoginUser.RelatedUser.LastName.ToString()));

                Tuple<byte[], string, string> report = ProveedoresOnLine.Reports.Controller.ReportModule.CP_SurveyReportDetail(
                                                    (int)enumReportType.RP_SurveyReport,
                                                    enumCategoryInfoType.PDF.ToString(),
                                                    parameters,
                                                    MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "SV_Report_SurveyDetail.rdlc");
                parameters = null;
                return File(report.Item1, report.Item2, report.Item3);
            }
            #endregion
            return View(oModel);
        }

        public virtual ActionResult SVSurveyDetail(string ProviderPublicId, string SurveyPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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
                oModel.ProviderMenu = GetProviderMenu(oModel);
                //get survey info
                oModel.RelatedSurvey = new Models.Survey.SurveyViewModel
                    (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId));
            }
            return View(oModel);
        }

        public virtual ActionResult SVSurveyEvaluatorDetail(string ProviderPublicId, string SurveyPublicId, string User)
        {           
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            if (Request["DownloadReport"] == "true")
            {
                return File(Convert.FromBase64String(Request["File"]), Request["MimeType"], Request["FileName"]);
            }
            else
            {                
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
                if (oProvider != null)                
                {
                    //get provider view model
                    oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);
                    oModel.ProviderMenu = GetProviderMenu(oModel);
                    //get survey info
                    oModel.RelatedSurvey = new Models.Survey.SurveyViewModel
                        (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(SurveyPublicId, User));
                }
                oModel.SurveytReportModel = new GenericReportModel();
                oModel.SurveytReportModel = Report_SurveyEvaluatorDetail(oModel);
               
                return View(oModel);
            }
        }

        public virtual ActionResult SVSurveyReport(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

                oModel.RelatedLiteProvider.RelatedProvider.RelatedReports = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPReportGetBasicInfo(ProviderPublicId, (int)enumReportType.RP_SurveyReport);
                oModel.RelatedReportInfo = new List<ProviderReportsViewModel>();

                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedReports != null)
                {
                    oModel.RelatedLiteProvider.RelatedProvider.RelatedReports.All(x =>
                    {
                        oModel.RelatedReportInfo.Add(new ProviderReportsViewModel(x));
                        return true;
                    });
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }


            return View(oModel);
        }

        public virtual ActionResult SVSurveyProgram(string ProviderPublicId, string SurveyPublicId, string ProjectPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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
                oModel.RelatedLiteProvider = new ProviderLiteViewModel(oProvider);

                if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
                {
                    ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyToUpsert = GetSurveyUpsertRequest();
                    SurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyUpsert(SurveyToUpsert);
                }
                if (!string.IsNullOrEmpty(SurveyPublicId))//si es editar
                {
                    if (oProvider != null)
                    {
                        oModel.RelatedSurvey = new Models.Survey.SurveyViewModel(ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId));
                        if (oModel.RelatedSurvey != null)
                        {
                            oModel.RelatedSurvey.RelatedSurvey.ChildSurvey = new List<SurveyModel>();
                            List<string> Evaluators = oModel.RelatedSurvey.SurveyEvaluatorList.GroupBy(x => x).Select(grp => grp.First()).ToList();

                            Evaluators.All(evt =>
                            {
                                oModel.RelatedSurvey.RelatedSurvey.ChildSurvey.Add(
                                (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser(SurveyPublicId, evt)));
                                return true;
                            });
                        }


                    }
                }
                else if (!string.IsNullOrEmpty(ProjectPublicId))
                {
                    oModel.RelatedProject = ProveedoresOnLine.ProjectModule.Controller.ProjectModule.ProjectGetById(ProjectPublicId, SessionModel.CurrentCompany.CompanyPublicId);
                }

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            return View(oModel);
        }
        #endregion

        #region Reports
        public virtual ActionResult RPGerencial(string ProviderPublicId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

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

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPCompanyGetBasicInfo(ProviderPublicId);

                oModel.ContactCompanyInfo = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.PersonContact);
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

                oModel.RelatedLiteProvider.RelatedProvider.RelatedLegal = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPLegalGetBasicInfo(ProviderPublicId, (int)enumLegalType.ChaimberOfCommerce);
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

                oModel.ContactCompanyInfo = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPContactGetBasicInfo(ProviderPublicId, (int)enumContactType.Brach);

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

                oModel.RelatedBlackListInfo = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_BlackListGetBasicInfo(ProviderPublicId);

                #endregion

                #region Tracking Info

                oModel.RelatedTrackingInfo = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPCustomerProviderGetTracking(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);

                #endregion

                #region Basic Financial Info

                List<GenericItemModel> oFinancial = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPFinancialGetLastyearInfoDeta(ProviderPublicId);
                oModel.RelatedFinancialBasicInfo = new List<ProviderFinancialBasicInfoViewModel>();

                if (oFinancial != null)
                {
                    decimal oExchange;
                    oExchange = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemInfo.FirstOrDefault().ValueName),
                                Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_COP].Value),
                                Convert.ToInt32(oFinancial.FirstOrDefault().ItemName));

                    oFinancial.All(x =>
                    {
                        oModel.RelatedFinancialBasicInfo.Add(new ProviderFinancialBasicInfoViewModel(x, oExchange));
                        return true;
                    });

                    if (oModel.RelatedFinancialBasicInfo != null && oModel.RelatedFinancialBasicInfo.Count > 0)
                    {
                        oModel.RelatedFinancialBasicInfo.FirstOrDefault().BI_JobCapital =
                            ((Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentActive)).Select(x => x.BI_CurrentActive).DefaultIfEmpty("0").FirstOrDefault())
                            - Convert.ToDecimal(oModel.RelatedFinancialBasicInfo.Where(x => !string.IsNullOrWhiteSpace(x.BI_CurrentPassive)).Select(x => x.BI_CurrentPassive).DefaultIfEmpty("0").FirstOrDefault()))).ToString("#,0.##");
                    }
                }
                #endregion

                #region K_Contract Info

                List<GenericItemModel> oRelatedKContractInfo = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_FinancialGetBasicInfo(ProviderPublicId, (int)enumFinancialType.KRecruitment, true);

                oModel.RelatedKContractInfo = new List<ProviderFinancialViewModel>();
                if (oRelatedKContractInfo != null)
                {
                    oRelatedKContractInfo.All(x =>
                    {
                        oModel.RelatedKContractInfo.Add(new ProviderFinancialViewModel(x));
                        return true;
                    });
                }

                #endregion

                //Get Engagement info                                
                #region HSEQ

                oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.CompanyRiskPolicies);
                if (oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification != null
                && oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.Count > 0)
                {
                    List<GenericItemModel> basicCert = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPCertificationGetBasicInfo(ProviderPublicId, (int)enumHSEQType.Certifications);
                    if (basicCert != null && basicCert.Count > 0)
                    {
                        oModel.RelatedLiteProvider.RelatedProvider.RelatedCertification.AddRange(basicCert);
                    }
                }

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

                oModel.RelatedCertificationBasicInfo = ProveedoresOnLine.Reports.Controller.ReportModule.C_Report_MPCertificationGetSpecificCert(ProviderPublicId);

                #endregion

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }

            #region Report Generator

            if (Request["DownloadReport"] == "true")
            {
                List<ReportParameter> parameters = new List<ReportParameter>();
                ProviderModel oToInsert = new ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = ProviderPublicId,
                    },
                    RelatedReports = new List<GenericItemModel>
                    {
                        new GenericItemModel(){
                            ItemId = 0,
                            ItemName = "GerencialReport",
                            ItemType = new CatalogModel(){
                                ItemId = (int)MarketPlace.Models.General.enumReportType.RP_GerencialReport,
                            },
                            Enable = true,
                        },
                    },
                };

                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPReportUpsert(oToInsert);

                #region Set Parameters

                //CustomerInfo
                parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
                parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
                parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
                parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));

                //ProviderInfo
                parameters.Add(new ReportParameter("ProviderName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
                parameters.Add(new ReportParameter("ProviderIdentificationType", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));
                parameters.Add(new ReportParameter("ProviderIdentificationNumber", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

                #region Basic Info

                if (!string.IsNullOrEmpty(oModel.RelatedGeneralInfo.Where(x => x.PC_RepresentantType == "Legal").Select(x => x.PC_ContactName).FirstOrDefault()))
                    parameters.Add(new ReportParameter("Representant", oModel.RelatedGeneralInfo.Where(x => x.PC_RepresentantType == "Legal").Select(x => x.PC_ContactName).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("Representant", "NA"));

                if (oModel.RelatedLegalInfo.Count > 0 && !string.IsNullOrEmpty(oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber)
                    && !string.IsNullOrWhiteSpace(oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber))
                    parameters.Add(new ReportParameter("InscriptionNumber", oModel.RelatedLegalInfo.FirstOrDefault().CP_InscriptionNumber));
                else
                    parameters.Add(new ReportParameter("InscriptionNumber", "NA"));


                if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Address).FirstOrDefault()))
                    parameters.Add(new ReportParameter("Address", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Address).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("Address", "NA"));

                if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_City).FirstOrDefault()))
                    parameters.Add(new ReportParameter("City", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_City).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("City", "NA"));

                if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Phone).FirstOrDefault()))
                    parameters.Add(new ReportParameter("Phone", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Phone).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("Phone", "NA"));

                if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Fax).FirstOrDefault()))
                    parameters.Add(new ReportParameter("Fax", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Fax).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("Fax", "NA"));

                if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Website).FirstOrDefault()))
                    parameters.Add(new ReportParameter("WebSite", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Website).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("WebSite", "NA"));

                if (!string.IsNullOrWhiteSpace(oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Email).FirstOrDefault()))
                    parameters.Add(new ReportParameter("Email", oModel.RelatedGeneralInfo.Where(x => x.BR_IsPrincipal == true).Select(x => x.BR_Email).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("Email", "NA"));

                if (oModel.RelatedLegalInfo.Count > 0 && !string.IsNullOrWhiteSpace(oModel.RelatedLegalInfo.FirstOrDefault().CP_SocialObject))
                    parameters.Add(new ReportParameter("SocialObject", oModel.RelatedLegalInfo.FirstOrDefault().CP_SocialObject));
                else
                    parameters.Add(new ReportParameter("SocialObject", "NA"));

                #endregion

                #region Finacial Info

                if (oModel.RelatedFinancialBasicInfo != null && oModel.RelatedFinancialBasicInfo.Count > 0 && string.IsNullOrEmpty(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalActive != null).Select(x => x.BI_TotalActive).DefaultIfEmpty("").FirstOrDefault()))
                    parameters.Add(new ReportParameter("TotalActive", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalActive != null).Select(x => x.BI_TotalActive).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("TotalActive", "NA"));


                if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPassive != null).Select(x => x.BI_TotalPassive).DefaultIfEmpty("").FirstOrDefault()))
                    parameters.Add(new ReportParameter("TotalPasive", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPassive != null).Select(x => x.BI_TotalPassive).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("TotalPasive", "NA"));

                if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPatrimony != null).Select(x => x.BI_TotalPatrimony).DefaultIfEmpty("").FirstOrDefault()))
                    parameters.Add(new ReportParameter("TotalPatrimony", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_TotalPatrimony != null).Select(x => x.BI_TotalPatrimony).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("TotalPatrimony", "NA"));


                if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_OperationIncome != null).Select(x => x.BI_OperationIncome).DefaultIfEmpty("").FirstOrDefault()))
                    parameters.Add(new ReportParameter("OperationIncome", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_OperationIncome != null).Select(x => x.BI_OperationIncome).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("OperationIncome", "NA"));


                if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_IncomeBeforeTaxes != null).Select(x => x.BI_IncomeBeforeTaxes).DefaultIfEmpty("").FirstOrDefault()))
                    parameters.Add(new ReportParameter("IncomeBeforeTaxes", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_IncomeBeforeTaxes != null).Select(x => x.BI_IncomeBeforeTaxes).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("IncomeBeforeTaxes", "NA"));

                parameters.Add(new ReportParameter("JobCapital", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_JobCapital != null).Select(x => x.BI_JobCapital).DefaultIfEmpty("NA").FirstOrDefault()));

                parameters.Add(new ReportParameter("Altman", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_Altman != null).Select(x => x.BI_Altman).DefaultIfEmpty("NA").FirstOrDefault()));

                if (oModel.RelatedFinancialBasicInfo != null && !string.IsNullOrWhiteSpace(oModel.RelatedFinancialBasicInfo.Where(x => x.BI_ExerciseUtility != null).Select(x => x.BI_ExerciseUtility).DefaultIfEmpty("").FirstOrDefault()))
                    parameters.Add(new ReportParameter("ExcerciseUtility", oModel.RelatedFinancialBasicInfo.Where(x => x.BI_ExerciseUtility != null).Select(x => x.BI_ExerciseUtility).FirstOrDefault()));
                else
                    parameters.Add(new ReportParameter("ExcerciseUtility", "NA"));

                #endregion

                #region HSEQ Info

                if (oModel.RelatedHSEQlInfo != null && oModel.RelatedHSEQlInfo.Count > 0)
                {
                    parameters.Add(new ReportParameter("SystemOccupationalHazards", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards));
                    parameters.Add(new ReportParameter("RateARL", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL));
                    parameters.Add(new ReportParameter("LTIFResult", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult));
                }
                else
                {
                    parameters.Add(new ReportParameter("SystemOccupationalHazards", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_SystemOccupationalHazards));
                    parameters.Add(new ReportParameter("RateARL", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_RateARL));
                    parameters.Add(new ReportParameter("LTIFResult", oModel.RelatedHSEQlInfo.FirstOrDefault() == null || oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult == null ? "NA" : oModel.RelatedHSEQlInfo.FirstOrDefault().CR_LTIFResult));
                }

                #endregion

                DataTable data = new DataTable();
                data.Columns.Add("EvaluationCriteria");
                data.Columns.Add("Provider");
                data.Columns.Add("Consultant");
                data.Columns.Add("Builder");

                DataRow row;
                foreach (var item in oModel.RelatedKContractInfo.Where(x => x != null))
                {
                    row = data.NewRow();

                    row["EvaluationCriteria"] = item.FK_RoleType;
                    row["Provider"] = item.FK_TotalScore;
                    row["Consultant"] = item.FK_TotalOrgCapacityScore;
                    row["Builder"] = item.FK_TotalKValueScore;

                    data.Rows.Add(row);
                }

                #endregion

                Tuple<byte[], string, string> GerencialReport = ProveedoresOnLine.Reports.Controller.ReportModule.CP_GerencialReport(
                                                                enumCategoryInfoType.PDF.ToString(),
                                                                data,
                                                                parameters,
                                                                MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "C_Report_GerencialInfo.rdlc");

                parameters = null;
                return File(GerencialReport.Item1, GerencialReport.Item2, GerencialReport.Item3);
            }

            #endregion

            return View(oModel);
        }

        public virtual ActionResult RPGeneral
            (string SearchParam,
            string SearchFilter)
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            MarketPlace.Models.Provider.ProviderSearchViewModel oModel = null;

            List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> oProviderResult;

            if (MarketPlace.Models.General.SessionModel.CurrentCompany != null &&
                !string.IsNullOrEmpty(MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId))
            {

                //get basic search model
                oModel = new Models.Provider.ProviderSearchViewModel()
                {
                    ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                    SearchParam = SearchParam,
                    SearchFilter = SearchFilter == null ? null : (SearchFilter.Trim(new char[] { ',' }).Length > 0 ? SearchFilter.Trim(new char[] { ',' }) : null),
                    SearchOrderType = MarketPlace.Models.General.enumSearchOrderType.Relevance,
                    OrderOrientation = false,
                    PageNumber = 0,
                    ProviderSearchResult = new List<Models.Provider.ProviderLiteViewModel>(),
                };

                #region Providers
                //search providers
                int oTotalRowsAux;
                oProviderResult =
                    ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchNew
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    (int)oModel.SearchOrderType,
                    oModel.OrderOrientation,
                    oModel.PageNumber,
                    //oModel.RowCount,
                    65000,
                    out oTotalRowsAux);

                oModel.TotalRows = oTotalRowsAux;

                List<GenericFilterModel> oFilterModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchFilterNew
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    oModel.SearchParam,
                    oModel.SearchFilter,
                    MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCompanyInfoType.OtherProviders).Select(x => x.Value).FirstOrDefault() == "1" ? true : false);

                if (oFilterModel != null)
                {
                    oModel.ProviderFilterResult = oFilterModel.Where(x => x.CustomerPublicId == MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId).ToList();
                }

                //Branch Info

                //parse view model
                if (oProviderResult != null && oProviderResult.Count > 0)
                {
                    oProviderResult.All(prv =>
                    {
                        prv.RelatedCommercial = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPContactGetBasicInfo(prv.RelatedCompany.CompanyPublicId, (int)enumContactType.Brach);
                        return true;
                    });
                }

                #endregion


                #region Crete Excel

                //Write the document
                StringBuilder data = new StringBuilder();
                string strSep = ";";

                oProviderResult.All(x =>
                {
                    if (oProviderResult.IndexOf(x) == 0)
                    {
                        data.AppendLine
                        ("\"" + "Razon Social" + "\"" + strSep +
                            "\"" + "Tipo Identificacion" + "\"" + strSep +
                            "\"" + "Numero Identificacion" + "\"" + strSep +
                            "\"" + "Sucursal" + "\"" + strSep +
                            "\"" + "Telefono" + "\"" + strSep +
                            "\"" + "Ciudad" + "\"" + strSep +
                            "\"" + "Representante" + "\"");
                        data.AppendLine
                            ("\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                            "\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                            "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Address).Select(y => y.Value).DefaultIfEmpty(string.Empty).FirstOrDefault() : string.Empty) + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Phone).Select(y => y.Value).DefaultIfEmpty(string.Empty).FirstOrDefault() : string.Empty) + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? MarketPlace.Models.Company.CompanyUtil.GetCityName(x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(y => y.Value).DefaultIfEmpty(string.Empty).FirstOrDefault()) : string.Empty) + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Representative).Select(y => y.Value).DefaultIfEmpty(string.Empty).FirstOrDefault() : string.Empty) + "\"");

                    }
                    else
                    {
                        data.AppendLine
                            ("\"" + x.RelatedCompany.CompanyName + "\"" + "" + strSep +
                            "\"" + x.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                            "\"" + x.RelatedCompany.IdentificationNumber + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Address).Select(y => y.Value).DefaultIfEmpty("").FirstOrDefault() : string.Empty) + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Phone).Select(y => y.Value).DefaultIfEmpty("").FirstOrDefault() : string.Empty) + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? MarketPlace.Models.Company.CompanyUtil.GetCityName(x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_City).Select(y => y.Value).DefaultIfEmpty(string.Empty).FirstOrDefault()) : string.Empty) + "\"" + strSep +
                            "\"" + (x.RelatedCommercial != null ? x.RelatedCommercial.FirstOrDefault().ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.B_Representative).Select(y => y.Value).DefaultIfEmpty("").FirstOrDefault() : string.Empty) + "\"");
                    }
                    return true;
                });

                byte[] buffer = Encoding.ASCII.GetBytes(data.ToString().ToCharArray());

                #endregion

                return File(buffer, "application/csv", "Proveedores_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
            }

            return null;
        }

        #endregion

        #region Pivate Functions
        private GenericReportModel Report_SurveyEvaluatorDetail(ProviderViewModel oModel)
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            GenericReportModel oReporModel = new GenericReportModel();

            //CustomerInfo
            parameters.Add(new ReportParameter("CustomerName", SessionModel.CurrentCompany.CompanyName));
            parameters.Add(new ReportParameter("CustomerIdentification", SessionModel.CurrentCompany.IdentificationNumber));
            parameters.Add(new ReportParameter("CustomerIdentificationType", SessionModel.CurrentCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("CustomerImage", SessionModel.CurrentCompany_CompanyLogo));
            //ProviderInfo
            parameters.Add(new ReportParameter("ProviderName", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName));
            parameters.Add(new ReportParameter("ProviderIdentificationType", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName));
            parameters.Add(new ReportParameter("ProviderIdentificationNumber", oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber));

            //SurveyInfo
            parameters.Add(new ReportParameter("SurveyConfigName", oModel.RelatedSurvey.SurveyConfigName));
            parameters.Add(new ReportParameter("SurveyRating", oModel.RelatedSurvey.SurveyRating.ToString()));
            parameters.Add(new ReportParameter("SurveyStatusName", oModel.RelatedSurvey.SurveyStatusName));
            parameters.Add(new ReportParameter("SurveyIssueDate", oModel.RelatedSurvey.SurveyIssueDate));
            parameters.Add(new ReportParameter("SurveyEvaluator", oModel.RelatedSurvey.SurveyEvaluator));
            parameters.Add(new ReportParameter("SurveyLastModify", oModel.RelatedSurvey.SurveyLastModify));          
            parameters.Add(new ReportParameter("SurveyResponsible", oModel.RelatedSurvey.SurveyResponsible));
            
            
            if (oModel.RelatedSurvey.SurveyRelatedProject == null)
            {
                parameters.Add(new ReportParameter("SurveyRelatedProject", "NA"));
            }
            else
            {
                parameters.Add(new ReportParameter("SurveyRelatedProject", oModel.RelatedSurvey.SurveyRelatedProject));
            }

            DataTable data = new DataTable();
            data.Columns.Add("Area");
            data.Columns.Add("Question");
            data.Columns.Add("Answer");
            data.Columns.Add("QuestionRating");
            data.Columns.Add("QuestionWeight");
            data.Columns.Add("QuestionDescription");

            DataRow row;
            foreach (var EvaluationArea in
                        oModel.RelatedSurvey.GetSurveyConfigItem(MarketPlace.Models.General.enumSurveyConfigItemType.EvaluationArea, null))
            {
                var lstQuestion = oModel.RelatedSurvey.GetSurveyConfigItem
                    (MarketPlace.Models.General.enumSurveyConfigItemType.Question, EvaluationArea.SurveyConfigItemId);

                row = data.NewRow();
                row["Area"] = EvaluationArea.Name;

                foreach (var Question in lstQuestion)
                {
                    row["Question"] = Question.Order + " " + Question.Name;

                    var QuestionInfo = oModel.RelatedSurvey.GetSurveyItem(Question.SurveyConfigItemId);
                    var lstAnswer = oModel.RelatedSurvey.GetSurveyConfigItem
                        (MarketPlace.Models.General.enumSurveyConfigItemType.Answer, Question.SurveyConfigItemId);

                    foreach (var Answer in lstAnswer)
                    {
                        if (QuestionInfo != null && QuestionInfo.Answer == Answer.SurveyConfigItemId)
                        {
                            row["Answer"] = Answer.Name;
                        }
                        else
                        {
                            row["Answer"] = "Sin Responder";
                            row["QuestionRating"] = "Sin Responder"; 
                        }
                    }

                    if (QuestionInfo != null)
                    {
                        row["QuestionRating"] = QuestionInfo.Ratting;
                    }
                    else
                    {
                        row["QuestionRating"] = "NA";
                    }          
                         
                    row["QuestionWeight"] = Question.Weight;
                                        
                    if(QuestionInfo != null && QuestionInfo.DescriptionText != null)
                    {
                        row["QuestionDescription"] = QuestionInfo.DescriptionText;
                    }
                    else
                    {
                        row["QuestionDescription"] = "-";                        
                    }
                    
                }

                data.Rows.Add(row);
            }

            Tuple<byte[], string, string> EvaluatorDetailReport = ProveedoresOnLine.Reports.Controller.ReportModule.SV_EvaluatorDetailReport(
                                                               enumCategoryInfoType.PDF.ToString(),
                                                               data,
                                                               parameters,
                                                               MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.MP_CP_ReportPath].Value.Trim() + "SV_Report_EvaluatorDetail.rdlc");

            oReporModel.File = EvaluatorDetailReport.Item1;
            oReporModel.MimeType = EvaluatorDetailReport.Item2;
            oReporModel.FileName = EvaluatorDetailReport.Item3;

            return oReporModel;
        }

        private ProveedoresOnLine.SurveyModule.Models.SurveyModel GetSurveyUpsertRequest()
        {
            List<Tuple<string, int, int, int>> EvaluatorsRoleObj = new List<Tuple<string, int, int, int>>();
            List<string> EvaluatorsEmail = new List<string>();

            #region Parent Survey
            //Armar el Survey Model Del Papá
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oReturn = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                ChildSurvey = new List<ProveedoresOnLine.SurveyModule.Models.SurveyModel>(),
                SurveyPublicId = System.Web.HttpContext.Current.Request["SurveyPublicId"],
                RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                {
                    RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"],
                    }
                },
                RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                {
                    ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["SurveyConfigId"].Trim()),
                },
                Enable = true,
                User = SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().User, //Responsable               
                SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>()
            };

            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("SurveyInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');

                //Set Parent Survey Info
                if (strSplit.Length >= 3)
                {
                    oReturn.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = !string.IsNullOrEmpty(strSplit[2]) ? Convert.ToInt32(strSplit[2].Trim()) : 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = System.Web.HttpContext.Current.Request[req],
                        Enable = true,
                    });

                    //Get Evaluator Rol info
                    if (Convert.ToInt32(strSplit[1].Trim()) == (int)enumSurveyInfoType.Evaluator)
                        EvaluatorsRoleObj.Add(new Tuple<string, int, int, int>(System.Web.HttpContext.Current.Request[req],
                                                                                Convert.ToInt32(strSplit[4].Trim()),
                                                                                Convert.ToInt32(strSplit[2].Trim()),
                                                                                Convert.ToInt32(strSplit[5].Trim())));
                }
                return true;
            });
            #endregion

            if (EvaluatorsRoleObj != null && EvaluatorsRoleObj.Count > 0)
            {
                EvaluatorsEmail = new List<string>();
                EvaluatorsEmail = EvaluatorsRoleObj.GroupBy(x => x.Item1).Select(grp => grp.First().Item1).ToList();

                #region Child Survey
                //Set survey by evaluators
                EvaluatorsEmail.All(x =>
                {
                    oReturn.ChildSurvey.Add(new SurveyModel()
                    {
                        SurveyPublicId = System.Web.HttpContext.Current.Request["SurveyPublicId"],
                        RelatedProvider = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel()
                        {
                            RelatedCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = System.Web.HttpContext.Current.Request["ProviderPublicId"],
                            }
                        },
                        RelatedSurveyConfig = new ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel()
                        {
                            ItemId = Convert.ToInt32(System.Web.HttpContext.Current.Request["SurveyConfigId"].Trim()),
                        },
                        Enable = true,
                        User = x,//Evaluator,                     
                        SurveyInfo = new List<GenericItemInfoModel>()
                    });
                    return true;
                });

                //Set SurveyChild Info
                oReturn.ChildSurvey.All(it =>
                {
                    List<Tuple<int, int, int>> AreaIdList = new List<Tuple<int, int, int>>();
                    AreaIdList.AddRange(EvaluatorsRoleObj.Where(y => y.Item1 == it.User).Select(y => new Tuple<int, int, int>(y.Item2, y.Item3, y.Item4)).ToList());
                    AreaIdList = AreaIdList.GroupBy(x => x.Item1).Select(grp => grp.First()).ToList();
                    if (AreaIdList != null)
                    {
                        AreaIdList.All(a =>
                        {
                            it.SurveyInfo.Add(new GenericItemInfoModel()
                            {
                                ItemInfoId = a.Item2 != null ? a.Item2 : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyInfoType.CurrentArea
                                },
                                Value = a.Item1.ToString() + "_" + a.Item3.ToString(),
                                Enable = true,
                            });
                            return true;
                        });

                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Project
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Project).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });

                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.IssueDate
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.IssueDate).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Reminder
                            },
                            Value = "false",
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Contract
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Contract).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Status
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Comments
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.Responsible
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        it.SurveyInfo.Add(new GenericItemInfoModel()
                        {
                            ItemInfoId = 0,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = (int)enumSurveyInfoType.ExpirationDate
                            },
                            Value = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.ExpirationDate).Select(x => x.Value).FirstOrDefault(),
                            Enable = true,
                        });
                        List<GenericItemInfoModel> oEvaluators = new List<GenericItemInfoModel>();
                        oEvaluators = oReturn.SurveyInfo.Where(x => x.ItemInfoType.ItemId == (int)enumSurveyInfoType.Evaluator).Select(x => x).ToList();
                        oEvaluators = oEvaluators.GroupBy(x => x.Value).Select(grp => grp.First()).ToList();

                        //EvaluatorsRoleObj.GroupBy(x => x.Item1).Select(grp => grp.First().Item1).ToList();
                        if (oEvaluators.Count > 0)
                        {
                            oEvaluators.All(x =>
                            {
                                it.SurveyInfo.Add(new GenericItemInfoModel()
                                {
                                    ItemInfoId = 0,
                                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)enumSurveyInfoType.Evaluator
                                    },
                                    Value = x.Value,
                                    Enable = true,
                                });
                                return true;
                            });
                        }
                    }
                    return true;
                });
                #endregion
            }
            return oReturn;
        }

        private ProveedoresOnLine.Company.Models.Util.GenericItemModel GetSurveyReportFilterRequest()
        {
            ProveedoresOnLine.Company.Models.Util.GenericItemModel oReturn = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
            {
                ItemId = 0,
                ItemName = "SurveyFilterReport",
                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)enumReportType.RP_SurveyReport,
                },
                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),

                Enable = true,

            };

            System.Web.HttpContext.Current.Request.Form.AllKeys.Where(x => x.Contains("SurveyInfo_")).All(req =>
            {
                string[] strSplit = req.Split('_');
                if (strSplit.Length > 0)
                {
                    oReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = 0,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(strSplit[1].Trim())
                        },
                        Value = System.Web.HttpContext.Current.Request[req],
                        Enable = true,
                    });
                }
                return true;
            });

            return oReturn;
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

                foreach (var module in MarketPlace.Models.General.SessionModel.CurrentUserModules())
                {
                    MarketPlace.Models.General.GenericMenu oMenuAux;

                    if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderDetail)
                    {
                        #region GeneralInfo

                        //header
                        oMenuAux = new Models.General.GenericMenu()
                        {
                            Name = "Información General",
                            Position = 0,
                            ChildMenu = new List<Models.General.GenericMenu>(),
                        };

                        //Basic info
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Resumen del Proveedor",
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
                            Name = "Información de Personas de Contacto",
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
                            Name = "Representación y/o Distribuciones",
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

                        //Seguimientos
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Seguimientos",
                            Url = Url.RouteUrl
                                    (MarketPlace.Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.GITrackingInfo,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 3,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.GITrackingInfo &&
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
                            Position = 1,
                            ChildMenu = new List<Models.General.GenericMenu>(),
                        };

                        //Balancesheet info
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Cámara de Comercio",
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
                            Name = "Registro Único Tributario",
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

                        #region Financial Info

                        //header
                        oMenuAux = new Models.General.GenericMenu()
                        {
                            Name = "Información Financiera",
                            Position = 2,
                            ChildMenu = new List<Models.General.GenericMenu>(),
                        };

                        //Balancesheet info
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Estados Financieros",
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
                            Name = "Declaración de Renta",
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
                            Name = "Información Bancaria",
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

                        //income statement
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "K Contratación",
                            Url = Url.RouteUrl
                                    (MarketPlace.Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.FIKContract,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 2,
                            IsSelected =
                                (oCurrentAction == MVC.Provider.ActionNames.FIKContract &&
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
                            Name = "Información Comercial",
                            Position = 3,
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
                            Name = "Información HSEQ",
                            Position = 4,
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
                            Name = "Política de seguridad, salud y Medio Ambiente",
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
                            Name = "Sistema de Riesgos Laborales",
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

                        #region Reports

                        //header
                        oMenuAux = new Models.General.GenericMenu()
                        {
                            Name = "Reportes",
                            Position = 6,
                            ChildMenu = new List<Models.General.GenericMenu>(),
                        };

                        //Gerencial Report
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Reporte Gerencial",
                            Url = Url.RouteUrl
                                    (MarketPlace.Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.RPGerencial,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                    (oCurrentAction == MVC.Provider.ActionNames.RPGerencial &&
                                    oCurrentController == MVC.Provider.Name),
                        });

                        //get is selected menu
                        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                        //add menu
                        oReturn.Add(oMenuAux);

                        #endregion
                    }

                    if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderBasicInfo)
                    {
                        #region GeneralInfo

                        //header
                        oMenuAux = new Models.General.GenericMenu()
                        {
                            Name = "Información General",
                            Position = 0,
                            ChildMenu = new List<Models.General.GenericMenu>(),
                        };

                        //Basic info
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Resumen del Proveedor",
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

                        //get is selected menu
                        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                        //add menu
                        oReturn.Add(oMenuAux);

                        #endregion
                    }

                    if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderRatingView)
                    {
                        #region Survey Info

                        //header
                        oMenuAux = new Models.General.GenericMenu()
                        {
                            Name = "Evaluación de desempeño",
                            Position = 5,
                            ChildMenu = new List<Models.General.GenericMenu>(),
                        };

                        //survey list
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Lista de evaluaciónes",
                            Url = Url.RouteUrl
                                    (MarketPlace.Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.SVSurveySearch,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 0,
                            IsSelected =
                                ((oCurrentAction == MVC.Provider.ActionNames.SVSurveySearch ||
                                oCurrentAction == MVC.Provider.ActionNames.SVSurveyDetail) &&
                                oCurrentController == MVC.Provider.Name),
                        });


                        //survey list
                        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                        {
                            Name = "Reportes de evaluaciónes",
                            Url = Url.RouteUrl
                                    (MarketPlace.Models.General.Constants.C_Routes_Default,
                                    new
                                    {
                                        controller = MVC.Provider.Name,
                                        action = MVC.Provider.ActionNames.SVSurveyReport,
                                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                    }),
                            Position = 1,
                            IsSelected =
                                 (oCurrentAction == MVC.Provider.ActionNames.SVSurveyReport &&
                                oCurrentController == MVC.Provider.Name),
                        });

                        if (MarketPlace.Models.General.SessionModel.CurrentUserModules().Any(x => x == (int)enumMarketPlaceCustomerModules.ProviderRatingCreate))
                        {
                            //survey list
                            oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                            {
                                Name = "Programar evaluación",
                                Url = Url.RouteUrl
                                        (MarketPlace.Models.General.Constants.C_Routes_Default,
                                        new
                                        {
                                            controller = MVC.Provider.Name,
                                            action = MVC.Provider.ActionNames.SVSurveyProgram,
                                            ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
                                        }),
                                Position = 2,
                                IsSelected =
                                     (oCurrentAction == MVC.Provider.ActionNames.SVSurveyProgram &&
                                    oCurrentController == MVC.Provider.Name),
                            });

                        }

                        //get is selected menu
                        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                        //add menu
                        oReturn.Add(oMenuAux);

                        #endregion
                    }
                }

            }
            return oReturn;
        }

        #endregion
    }
}