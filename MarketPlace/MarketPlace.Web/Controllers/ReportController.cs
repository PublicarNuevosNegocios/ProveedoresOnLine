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

        #region Menu

        private List<GenericMenu> GetReportControllerMenu()
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

            #region Menu Usuario

            MarketPlace.Models.General.GenericMenu oMenuAux = new GenericMenu();

            //header
            oMenuAux = new GenericMenu()
            {
                Name = "Mis reportes",
                Position = 0,
                ChildMenu = new List<GenericMenu>(),
            };

            foreach (var module in MarketPlace.Models.General.SessionModel.CurrentUserModules())
            {
                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderDetail)
                {
                    //Gerencial
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Gerencial",
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
                }
                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderRatingCreate)
                {
                    //Evaluacion de deseempeño
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Evaluación de deseempeño",
                        Url = null,
                        Position = 0,
                        IsSelected = false
                    });
                }
                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ProviderSelectionCreate)
                {
                    //Proceso de selección
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Mis Consultas",
                        Url = null,
                        Position = 0,
                        IsSelected = false
                    });
                }
            }


            #endregion

            //get is selected menu
            oMenuAux.IsSelected = oMenuAux.ChildMenu.Any(x => x.IsSelected);

            //add menu
            oReturn.Add(oMenuAux);

            return oReturn;
        }

        #endregion
    }
}