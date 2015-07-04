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
    public partial class StatsController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult STProviderStats()
        {
            ProviderViewModel oModel = new ProviderViewModel();

            oModel.ProviderMenu = GetStatsMenu();

            return View(oModel);
        }

        public virtual ActionResult STSurveyStats()
        {
            ProviderViewModel oModel = new ProviderViewModel();

            oModel.ProviderMenu = GetStatsMenu();

            return View(oModel);
        }

        public virtual ActionResult STProjectStats()
        {
            ProviderViewModel oModel = new ProviderViewModel();

            oModel.ProviderMenu = GetStatsMenu();

            return View(oModel);
        }

        #region Menu

        private List<GenericMenu> GetStatsMenu()
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

            #region Stats

            MarketPlace.Models.General.GenericMenu oMenuAux;

            //header
            oMenuAux = new GenericMenu()
            {
                Name = "Módulo de Estadísticas",
                Position = 0,
                ChildMenu = new List<GenericMenu>(),
            };

            //Provider Stats
            oMenuAux.ChildMenu.Add(new GenericMenu()
            {
                Name = "Proveedores",
                Url = Url.RouteUrl
                        (MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Stats.Name,
                            action = MVC.Stats.ActionNames.STProviderStats
                        }),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Stats.ActionNames.STProviderStats &&
                    oCurrentController == MVC.Stats.Name)
            });

            //Evalutaion Stats
            oMenuAux.ChildMenu.Add(new GenericMenu()
            {
                Name = "Evaluación de desempeño",
                Url = Url.RouteUrl
                        (MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Stats.Name,
                            action = MVC.Stats.ActionNames.STSurveyStats
                        }),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Stats.ActionNames.STSurveyStats &&
                    oCurrentController == MVC.Stats.Name)
            });

            //Project Stats
            oMenuAux.ChildMenu.Add(new GenericMenu()
            {
                Name = "Proceso de Selección",
                Url = Url.RouteUrl
                        (MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Stats.Name,
                            action = MVC.Stats.ActionNames.STProjectStats,
                        }),
                Position = 0,
                IsSelected =
                    (oCurrentAction == MVC.Stats.ActionNames.STProjectStats &&
                    oCurrentController == MVC.Stats.Name)
            });

            //Other menu stats

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