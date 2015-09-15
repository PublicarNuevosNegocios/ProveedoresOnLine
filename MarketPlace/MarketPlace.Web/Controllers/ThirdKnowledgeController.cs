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

                //Get The Active Plan By Customer 
                oCurrentPeriodList = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetCurrenPeriod(SessionModel.CurrentCompany.CompanyPublicId, true);

                if (oCurrentPeriodList != null && oCurrentPeriodList.Count > 0)
                {
                    oModel.RelatedThirdKnowledge.HasPlan = true;

                    //Get The Most Recently Period When Plan is More Than One
                    oModel.RelatedThirdKnowledge.CurrentPlanModel = oCurrentPeriodList.OrderByDescending(x => x.CreateDate).First();

                    #region Upsert Process                    
                    //if (Request["UpsertRequest"] == "true")
                    //{
                    //    //Set Current Sale
                    //    #region No Borrar
                    //    if (oModel.RelatedThirdKnowledge != null)
                    //    {
                    //        //Save Query 
                    //        TDQueryModel oQueryToCreate = new TDQueryModel()
                    //        {
                    //            IsSuccess = true,
                    //            PeriodPublicId = oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                    //            SearchType = new TDCatalogModel()
                    //            {
                    //                ItemId = (int)enumThirdKnowledgeQueryType.Simple,
                    //            },
                    //            User = SessionModel.CurrentLoginUser.Email,
                    //            RelatedQueryInfoModel = new List<TDQueryInfoModel>(),
                    //        };

                    //        oModel.RelatedThirdKnowledge.CollumnsResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest(oCurrentPeriodList.FirstOrDefault().
                    //                        RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                    //                       Request["IdentificationNumber"], Request["Name"], oQueryToCreate);

                    //        //Set New Score
                    //        oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().TotalQueries = (oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault().TotalQueries + 1);

                    //        //Period Upsert
                    //        oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.PeriodoUpsert(
                    //            oCurrentPeriodList.FirstOrDefault().RelatedPeriodModel.FirstOrDefault());
                    //    }
                    //    else
                    //    {
                    //        TDQueryModel oQueryToCreate = new TDQueryModel()
                    //       {
                    //           IsSuccess = false,
                    //           PeriodPublicId = oModel.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel.FirstOrDefault().PeriodPublicId,
                    //           SearchType = new TDCatalogModel()
                    //           {
                    //               ItemId = (int)enumThirdKnowledgeQueryType.Simple,
                    //           },
                    //           User = SessionModel.CurrentLoginUser.Email,
                    //       };
                    //    } 
                    //    #endregion                       
                    //}
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

        public virtual ActionResult TKThirdKnowledgeSearch(
            string ProviderPublicId,
            string OrderOrientation,
            string PageNumber,
            string InitDate,
            string EndDate)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThidKnowledgeSearch = new ThirdKnowledgeViewModel();
            List<ProveedoresOnLine.ThirdKnowledge.Models.TDQueryModel> oQueryModel = new List<TDQueryModel>();

            int TotalRows = 0;

            oQueryModel = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnoledgeSearch(
                SessionModel.CurrentCompany.CompanyPublicId,
                OrderOrientation == "1" ? true : false,
                Convert.ToInt32(PageNumber),
                20,
                out TotalRows);

            oModel.ProviderMenu = GetThirdKnowledgeControllerMenu();

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
                        Url = Url.RouteUrl
                                (MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.ThirdKnowledge.Name,
                                    action = MVC.ThirdKnowledge.ActionNames.TKThirdKnowledgeSearch
                                }),
                        Position = 0,
                        IsSelected =
                            (oCurrentAction == MVC.ThirdKnowledge.ActionNames.TKThirdKnowledgeSearch &&
                            oCurrentController == MVC.ThirdKnowledge.Name)
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