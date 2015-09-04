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
    public partial class ThirdKnowledgeController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Clean the season url saved
            if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                MarketPlace.Models.General.SessionModel.CurrentURL = null;


            return View();
        }

        public virtual ActionResult TKSingleSearch()
        {
            ProviderViewModel oModel = new ProviderViewModel();
            try
            {
                oModel.ProviderMenu = GetThirdKnowledgeControllerMenu();

                //Clean the season url saved
                if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                    MarketPlace.Models.General.SessionModel.CurrentURL = null;

                #region Funcion para borrar
                //if (Request["UpsertRequest"] == "true")
                //{
                //    WS_Inspector.Autenticacion oAuth = new WS_Inspector.Autenticacion();
                //    WS_Inspector.WSInspektorSoapClient Client = new WS_Inspector.WSInspektorSoapClient();

                //    oAuth.UsuarioNombre = "W5-Pub1ic@r";
                //    oAuth.UsuarioClave = "D6-E9$C3S6Q#5WW&5@";

                //    oModel.RelatedThirdKnowledge = new List<string[]>();

                //    string oResutl = Client.ConsultaInspektor(oAuth, Request["IdentificationNumber"], Request["Name"]);

                //    string[] split = oResutl.Split('#');
                //    List<string[]> oReturn = new List<string[]>();
                //    if (split != null)
                //    {
                //        split.All(x =>
                //        {
                //            oReturn.Add(x.Split('|'));
                //            return true;
                //        });
                //    }

                //    oModel.RelatedThirdKnowledge = oReturn;
                //}
                #endregion



                //Obtener el Plan del Comprador
                //Realizar la búsqueda
                //Obtener el total de consultas del periodo y sumarle
                //Hacer el Update del periodo consultado

            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }

            return View(oModel);
        }

        #region Menu

        private List<GenericMenu> GetThirdKnowledgeControllerMenu()
        {
            List<GenericMenu> oReturn = new List<GenericMenu>();

            string oCurrentController = MarketPlace.Web.Controllers.BaseController.CurrentControllerName;
            string oCurrentAction = MarketPlace.Web.Controllers.BaseController.CurrentActionName;

            #region Menu Usuario

            MarketPlace.Models.General.GenericMenu oMenuAux = new GenericMenu();
            //Consulta individual 
            //Consulta masiva
            // Mis Consultas
            // Mis reportes

            //header
            oMenuAux = new GenericMenu()
            {
                Name = "Menú Usuario",
                Position = 0,
                ChildMenu = new List<GenericMenu>(),
            };

            foreach (var module in MarketPlace.Models.General.SessionModel.CurrentUserModules())
            {
                if (module == (int)MarketPlace.Models.General.enumMarketPlaceCustomerModules.ThirdKnowledge)
                {
                    //Consulta individual
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Consulta individual",
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.ThirdKnowledge.Name,
                                    action = MVC.ThirdKnowledge.ActionNames.TKSingleSearch
                                }),
                        Position = 0,
                        IsSelected =
                            (oCurrentAction == MVC.ThirdKnowledge.ActionNames.TKSingleSearch &&
                            oCurrentController == MVC.ThirdKnowledge.Name)
                    });

                    //Consulta masiva
                    oMenuAux.ChildMenu.Add(new GenericMenu()
                    {
                        Name = "Consulta masiva",
                        Url = null,
                        Position = 0,
                        IsSelected = false
                    });

                    //Consulta individual
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