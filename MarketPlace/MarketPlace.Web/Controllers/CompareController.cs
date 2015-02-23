using MarketPlace.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class CompareController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        #region Commercial Info

        public virtual ActionResult CIExperiencesCompare(string CompareId, string Currency)
        {

            return View();
        }

        #endregion

        #region HSEQ Info

        public virtual ActionResult HIHSECompare(string CompareId, string Currency)
        {

            return View();
        }

        #endregion

        #region Financial Info

        public virtual ActionResult FIBalanceSheetInfoCompare(string ProviderPublicId, string ViewName, string Year, string Currency)
        {
            return View();
        }

        #endregion

        #region Menu

        //private List<GenericMenu> GetCompareMenu(ProviderViewModel vProviderInfo)
        //{
        //    List<GenericMenu> oReturn = new List<GenericMenu>();

        //    if (vProviderInfo.RelatedLiteProvider != null)
        //    {
        //        string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
        //        string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

        //        #region GeneralInfo

        //        //header
        //        MarketPlace.Models.General.GenericMenu oMenuAux = new Models.General.GenericMenu()
        //        {
        //            Name = "Información general",
        //            Position = 0,
        //            ChildMenu = new List<Models.General.GenericMenu>(),
        //        };

        //        //Basic info
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Información básica",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.GIProviderInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 0,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.GIProviderInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //Company persons Contact info
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Información de personas de contacto",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.GIPersonContactInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 1,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.GIPersonContactInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //Branch
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Sucursales",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.GIBranchInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 2,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.GIBranchInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });


        //        //Distributors
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Distribuidores",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.GIDistributorInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 3,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.GIDistributorInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //get is selected menu
        //        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

        //        //add menu
        //        oReturn.Add(oMenuAux);

        //        #endregion

        //        #region Commercial Info

        //        //header
        //        oMenuAux = new Models.General.GenericMenu()
        //        {
        //            Name = "Información comercial",
        //            Position = 1,
        //            ChildMenu = new List<Models.General.GenericMenu>(),
        //        };

        //        //Experience
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Experiencias",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.CIExperiencesInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 0,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.CIExperiencesInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //get is selected menu
        //        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

        //        //add menu
        //        oReturn.Add(oMenuAux);

        //        #endregion

        //        #region HSEQ Info

        //        //header
        //        oMenuAux = new Models.General.GenericMenu()
        //        {
        //            Name = "HSEQ",
        //            Position = 2,
        //            ChildMenu = new List<Models.General.GenericMenu>(),
        //        };

        //        //Certifications
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Certificaciones",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.HICertificationsInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 0,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.HICertificationsInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //Company healty politic
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Salud, medio ambiente y seguridad",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.HIHealtyPoliticInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 1,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.HIHealtyPoliticInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //Company healty politic
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Salud de riesgos laborales",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.HIRiskPoliciesInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 2,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.HIRiskPoliciesInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //get is selected menu
        //        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

        //        //add menu
        //        oReturn.Add(oMenuAux);
        //        #endregion

        //        #region Financial Info

        //        //header
        //        oMenuAux = new Models.General.GenericMenu()
        //        {
        //            Name = "Información Financiera",
        //            Position = 3,
        //            ChildMenu = new List<Models.General.GenericMenu>(),
        //        };

        //        //Balancesheet info
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Estados financieros",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.FIBalanceSheetInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 0,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.FIBalanceSheetInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //Tax Info
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Impuestos",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.FITaxInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 1,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.FITaxInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //income statement
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Declaración de renta",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.FIIncomeStatementInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 2,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.FIIncomeStatementInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //Bank Info
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Información bancaria",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.FIBankInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 3,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.FIBankInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //get is selected menu
        //        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

        //        //add menu
        //        oReturn.Add(oMenuAux);
        //        #endregion

        //        #region Legal Info

        //        //header
        //        oMenuAux = new Models.General.GenericMenu()
        //        {
        //            Name = "Información Legal",
        //            Position = 4,
        //            ChildMenu = new List<Models.General.GenericMenu>(),
        //        };

        //        //Balancesheet info
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Cámara de comercio",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.LIChaimberOfCommerceInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 0,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.LIChaimberOfCommerceInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //RUT
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Registro único tributario",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.LIRutInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 1,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.LIRutInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //CIFIN
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "CIFIN",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.LICIFINInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 2,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.LICIFINInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //SARLAFT
        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "SARLAFT",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.LISARLAFTInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 3,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.LISARLAFTInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
        //        {
        //            Name = "Resoluciones",
        //            Url = Url.RouteUrl
        //                    (MarketPlace.Models.General.Constants.C_Routes_Default,
        //                    new
        //                    {
        //                        controller = MVC.Provider.Name,
        //                        action = MVC.Provider.ActionNames.LIResolutionInfo,
        //                        ProviderPublicId = vProviderInfo.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId
        //                    }),
        //            Position = 4,
        //            IsSelected =
        //                (oCurrentAction == MVC.Provider.ActionNames.LIResolutionInfo &&
        //                oCurrentController == MVC.Provider.Name),
        //        });

        //        //get is selected menu
        //        oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

        //        //add menu
        //        oReturn.Add(oMenuAux);

        //        #endregion

        //        #region last next menu

        //        MarketPlace.Models.General.GenericMenu MenuAux = null;

        //        oReturn.OrderBy(x => x.Position).All(pm =>
        //        {
        //            pm.ChildMenu.OrderBy(x => x.Position).All(sm =>
        //            {
        //                if (MenuAux != null)
        //                {
        //                    MenuAux.NextMenu = sm;
        //                }
        //                sm.LastMenu = MenuAux;
        //                MenuAux = sm;

        //                return true;
        //            });

        //            return true;
        //        });

        //        #endregion

        //    }
        //    return oReturn;
        //}

        #endregion
    }
}