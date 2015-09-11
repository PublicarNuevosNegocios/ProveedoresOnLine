using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.SurveyModule.Models;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MarketPlace.Web.ControllersApi
{
    public class ThirdKnowledgeApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public ProviderViewModel TKSingleSearch(string TKSingleSearch)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedThirdKnowledge = new MarketPlace.Models.Company.ThirdKnowledgeViewModel();
            List<PlanModel> oCurrentPeriodList = new List<PlanModel>();

            try
            {
                //Get The Active Plan By Customer 
                oCurrentPeriodList = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetCurrenPeriod(SessionModel.CurrentCompany.CompanyPublicId, true);

                if (oCurrentPeriodList != null && oCurrentPeriodList.Count > 0)
                {
                    oModel.RelatedThirdKnowledge.HasPlan = true;

                    //Get The Most Recently Period When Plan is More Than One
                    oModel.RelatedThirdKnowledge.CurrentPlanModel = oCurrentPeriodList.OrderByDescending(x => x.CreateDate).First();

                    #region Upsert Process
                    //TODO: Put the Alert?
                    if (System.Web.HttpContext.Current.Request["UpsertRequest"] == "true")
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
                                           System.Web.HttpContext.Current.Request["IdentificationNumber"], System.Web.HttpContext.Current.Request["Name"], oQueryToCreate);

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
                    }
                    #endregion
                }
                else
                {
                    oModel.RelatedThirdKnowledge.HasPlan = false;
                }
                return oModel;
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }
    }
}
