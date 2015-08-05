using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ReportController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
            {
                MarketPlace.Models.General.SessionModel.CurrentURL = null;
            }
            return View();
        }

        public virtual ActionResult PRGeneral()
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.ProviderMenu = GetReportControllerMenu();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            return View(oModel);
        }

        public virtual ActionResult RP_SV_SurveyGeneralInfoReport()
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.ProviderMenu = GetReportControllerMenu();

            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;

            return View(oModel);
        }

        #region Menu

        private List<GenericMenu> GetReportControllerMenu()
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

            #region Reports

            foreach (var module in MarketPlace.Models.General.SessionModel.CurrentUserModules())
            {
                MarketPlace.Models.General.GenericMenu oMenuAux = new GenericMenu();

                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderDetail)
                {
                    #region Provider Report Menu
                    //header
                    oMenuAux = new GenericMenu()
                    {
                        Name = "Proveedores",
                        Position = 0,
                        ChildMenu = new List<GenericMenu>(),
                    };

                    //Gerencial
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Informe Gerencial",
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.Report.Name,
                                    action = MVC.Report.ActionNames.PRGeneral
                                }),
                        Position = 0,
                        IsSelected =
                            (oCurrentAction == MVC.Report.ActionNames.PRGeneral &&
                            oCurrentController == MVC.Report.Name)
                    });

                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    oReturn.Add(oMenuAux);

                    #endregion
                }
                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderRatingCreate)
                {
                    #region Survey Report
                    //header
                    oMenuAux = new Models.General.GenericMenu()
                    {
                        Name = "Evaluación de Desempeño",
                        Position = 1,
                        ChildMenu = new List<Models.General.GenericMenu>(),
                    };

                    //Información General
                    oMenuAux.ChildMenu.Add(new Models.General.GenericMenu()
                    {
                        Name = "Información General",
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.Report.Name,
                                    action = MVC.Report.ActionNames.RP_SV_SurveyGeneralInfoReport,
                                }),
                        Position = 0,
                        IsSelected =
                            (oCurrentAction == MVC.Report.ActionNames.RP_SV_SurveyGeneralInfoReport &&
                            oCurrentController == MVC.Report.Name),
                    });
                    //get is selected menu
                    oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

                    //add menu
                    oReturn.Add(oMenuAux);
                    #endregion
                }
            }

            #endregion

            return oReturn;
        }

        #endregion
    }
}