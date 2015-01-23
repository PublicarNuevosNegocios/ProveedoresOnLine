using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
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
            (string SearchParam,
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
            }

            return View(oModel);
        }

        #endregion

        #region General Info

        public virtual ActionResult GIProviderInfo(string ProviderPublicId)
        {            
            ProviderViewModel oModel = new ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
            };
            List<ProviderModel> oModelpr = new List<ProviderModel>();


            if (!string.IsNullOrEmpty(ProviderPublicId))
            {
                //get provider info                

                //oModel.RelatedProvider = 
                oModelpr = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById(SessionModel.CurrentCompany.CompanyPublicId, ProviderPublicId);
                
                //get provider menu
                //oModel.ProviderMenu = GetProviderMenu(oModel);                
            }
            return View();
        }

        public virtual ActionResult GICompanyContactInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult GIPersonContactInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult GIBranchInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult GIDistributorInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region Commercial Info

        public virtual ActionResult CIExperiencesInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HICertificationsInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult HIHealtyPoliticInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult HIRiskPoliciesInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region Financial Info

        public virtual ActionResult FIBalanceSheetInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult FITaxInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult FIIncomeStatementInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult FIBankInfo(string ProviderPublicId)
        {
            return View();
        }

        #endregion

        #region Legal Info

        public virtual ActionResult LIChaimberOfCommerceInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LIRutInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LICIFINInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LISARLAFTInfo(string ProviderPublicId)
        {
            return View();
        }

        public virtual ActionResult LIResolutionInfo(string ProviderPublicId)
        {
            return View();
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
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.FirstOrDefault().RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //add menu
                oReturn.Add(oMenuAux);

                #endregion

                #region Commercial Info

                #endregion

                #region HSEQ Info

                #endregion

                #region Financial Info

                #endregion

                #region Legal Info

                #endregion

                #region last next menu

                MarketPlace.Models.General.GenericMenu MenuAux = null;

                oReturn.OrderBy(x => x.Position).All(pm =>
                {
                    pm.ChildMenu.OrderBy(x => x.Position).All(sm =>
                    {
                        if (MenuAux != null)
                        {
                            MenuAux.NextMenu = sm;
                        }
                        sm.LastMenu = MenuAux;
                        MenuAux = sm;

                        return true;
                    });

                    return true;
                });

                #endregion

            }
            return oReturn;
        }

        #endregion
    }
}