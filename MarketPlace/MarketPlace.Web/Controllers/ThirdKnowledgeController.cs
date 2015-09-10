using MarketPlace.Models.Company;
using MarketPlace.Models.Compare;
using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.ThirdKnowledge.Models;
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
            oModel.RelatedThirdKnowledge = new ThirdKnowledgeViewModel(); 
            List<PlanModel> oCurrentPeriodList = new List<PlanModel>();

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

                //Get The Active Plan By Customer 
                oCurrentPeriodList = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetCurrenPeriod(SessionModel.CurrentCompany.CompanyPublicId, true);

                if (oCurrentPeriodList != null && oCurrentPeriodList.Count > 0)
                {
                    oModel.RelatedThirdKnowledge.HasPlan = true;

                    //Get The Most Recently Period When Plan is More Than One
                    oModel.RelatedThirdKnowledge.CurrentPlanModel = oCurrentPeriodList.OrderByDescending(x => x.CreateDate).First();

                    #region Upsert Process
                    //TODO: Put the Alert?
                    if (Request["UpsertRequest"] == "true")
                    {
                        //Set Current Sale
                        #region No Borrar
                        if (oModel.RelatedThirdKnowledge != null)
                        {
                            //Save Query 
                            TDQueryModel oQueryToCreate = new TDQueryModel()
                            {
                                IsSuccess = true,
                                PeriodPublicId = oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                SearchType = new TDCatalogModel()
                                {
                                    ItemId = (int)enumThirdKnowledgeQueryType.Simple,
                                },
                                User = SessionModel.CurrentLoginUser.Email,
                                RelatedQueryInfoModel = new List<TDQueryInfoModel>(),
                            };

                            oModel.RelatedThirdKnowledge.CollumnsResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest(oCurrentPeriodList.FirstOrDefault().
                                            RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                           Request["IdentificationNumber"], Request["Name"], oQueryToCreate);

                            //Set New Score
                            oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().TotalQueries = (oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries + 1);

                            //Period Upsert
                            oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.PeriodoUpsert(
                                oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault());
                        }
                        else
                        {
                            TDQueryModel oQueryToCreate = new TDQueryModel()
                           {
                               IsSuccess = false,
                               PeriodPublicId = oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                               SearchType = new TDCatalogModel()
                               {
                                   ItemId = (int)enumThirdKnowledgeQueryType.Simple,
                               },
                               User = SessionModel.CurrentLoginUser.Email,
                           };
                        } 
                        #endregion

                        TDQueryModel oQueryToCreate1 = new TDQueryModel();
                        oModel.ResultTest = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.TestSimpleRequest(oCurrentPeriodList.FirstOrDefault().
                                            RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                                           Request["IdentificationNumber"], Request["Name"], oQueryToCreate1);
                    }
                    #endregion
                }
                else
                {
                    oModel.RelatedThirdKnowledge.HasPlan = false;
                }
                return View(oModel);
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
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