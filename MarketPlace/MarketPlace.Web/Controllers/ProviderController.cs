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
                oModel.RelatedLiteProvider.RelatedProvider.RelatedCompany = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPCompanyGetBasicInfo(ProviderPublicId);

                oModel.ProviderMenu = GetProviderMenu(oModel);
            }
            return View(oModel);
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
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company Contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información de contacto de empresa",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company persons Contact info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información de personas de contacto",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Branch
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sucursales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Branch
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Sucursales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Distributors
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Distribuidores",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 5,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
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
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
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
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company healty politic
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Salud, medio ambiente y seguridad",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Company healty politic
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Salud de riesgos laborales",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
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
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Tax Info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Impuestas",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //income statement
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Declaración de renta",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //Bank Info
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Información bancaria",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
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
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 0,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //RUT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Registro único tributario",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 1,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //CIFIN
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "CIFIN",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 2,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //SARLAFT
                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "SARLAFT",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 3,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                {
                    Name = "Resoluciones",
                    Url = Url.Action
                        (MVC.Provider.ActionNames.GIProviderInfo,
                        MVC.Provider.Name,
                        new { ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId }),
                    Position = 4,
                    IsSelected =
                        (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
                        oCurrentController == MVC.Provider.Name),
                });

                //get is selected menu
                oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                //add menu
                oReturn.Add(oMenuAux);

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