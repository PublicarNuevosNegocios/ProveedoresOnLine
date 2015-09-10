using ProveedoresOnLine.ThirdKnowledge.DAL.Controller;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Controller
{
    public class ThirdKnowledgeModule
    {
        public static List<string[]> SimpleRequest(string PeriodPublicId, string IdentificationNumber, string Name, TDQueryModel oQueryToCreate)
        {
            try
            {
                #region Set User Service
                WS_Inspektor.Autenticacion oAuth = new WS_Inspektor.Autenticacion();
                WS_Inspektor.WSInspektorSoapClient oClient = new WS_Inspektor.WSInspektorSoapClient(); 
                #endregion

                List<PlanModel> oPlanModel = new List<PlanModel>();
                PeriodModel oCurrentPeriod = new PeriodModel();
                
                oAuth.UsuarioNombre = ProveedoresOnLine.ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_AuthServiceUser].Value;
                oAuth.UsuarioClave = ProveedoresOnLine.ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_AuthServicePass].Value;

                //WS Request
                string oResutl = oClient.ConsultaInspektor(oAuth, IdentificationNumber, Name);
                
                string[] split = oResutl.Split('#');
                List<string[]> oReturn = new List<string[]>();
                if (split != null)
                {
                    split.All(x =>
                    {
                        oReturn.Add(x.Split('|'));
                        return true;
                    });
                }

                if (oReturn != null)
                {
                    oQueryToCreate = CreateQuery(oQueryToCreate, oReturn);

                    //TODO: INDEXAR
                    QueryInsert(oQueryToCreate);
                }
                else
                {
                    oQueryToCreate.IsSuccess = false;
                    QueryInsert(oQueryToCreate);
                }

                return oReturn;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Config

        public static PlanModel PlanUpsert(PlanModel oPlanModelToUpsert)
        {
            //LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {
                if (oPlanModelToUpsert != null)
                {
                    oPlanModelToUpsert.PlanPublicId = ThirdKnowledgeDataController.Instance.PlanUpsert
                                                      (oPlanModelToUpsert.PlanPublicId,
                                                      oPlanModelToUpsert.CompanyPublicId,
                                                      oPlanModelToUpsert.QueriesByPeriod,
                                                      oPlanModelToUpsert.DaysByPeriod,
                                                      oPlanModelToUpsert.Status,
                                                      oPlanModelToUpsert.InitDate,
                                                      oPlanModelToUpsert.EndDate,
                                                      oPlanModelToUpsert.Enable);
                    oPlanModelToUpsert.RelatedPeriodModel = CalculatePeriods(oPlanModelToUpsert);
                }
                return oPlanModelToUpsert;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<PlanModel> GetAllPlanByCustomer(string CustomerPublicId, bool Enable)
        {
            return ThirdKnowledgeDataController.Instance.GetAllPlanByCustomer(CustomerPublicId, Enable);
        }

        public static List<PeriodModel> GetPeriodByPlanPublicId(string PeriodPublicId, bool Enable)
        {
            return ThirdKnowledgeDataController.Instance.GetPeriodByPlanPublicId(PeriodPublicId, Enable);
        }
        #endregion

        #region MarketPlace

        public static List<PeriodModel> CalculatePeriods(PlanModel oPlanToReCalculate)
        {
            int DiferenceInDays;
            int TotalPeriods = 0;

            oPlanToReCalculate.RelatedPeriodModel = ThirdKnowledgeDataController.Instance.GetPeriodByPlanPublicId(oPlanToReCalculate.PlanPublicId, true);

            if (oPlanToReCalculate.PlanPublicId != null &&
                oPlanToReCalculate.RelatedPeriodModel != null &&
                oPlanToReCalculate.RelatedPeriodModel.Count > 0)
            {
                oPlanToReCalculate.RelatedPeriodModel.All(x =>
                    {
                        ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.PeriodUpsert(
                                                            x.PeriodPublicId,
                                                            oPlanToReCalculate.PlanPublicId,
                                                            oPlanToReCalculate.QueriesByPeriod,
                                                            x.TotalQueries,
                                                            x.InitDate,
                                                            x.EndDate,
                                                            oPlanToReCalculate.Enable);
                        return true;
                    });
            }
            else
            {
                if (oPlanToReCalculate != null)
                {
                    //Get Days from dates interval                                
                    DiferenceInDays = (oPlanToReCalculate.EndDate - oPlanToReCalculate.InitDate).Days;

                    TotalPeriods = DiferenceInDays / oPlanToReCalculate.DaysByPeriod;
                    oPlanToReCalculate.RelatedPeriodModel = new List<PeriodModel>();
                }

                DateTime EndPastPeriod = new DateTime();
                for (int i = 0; i < TotalPeriods; i++)
                {
                    if (i == 0)
                    {
                        oPlanToReCalculate.RelatedPeriodModel.Add(new PeriodModel()
                           {
                               AssignedQueries = oPlanToReCalculate.QueriesByPeriod,
                               InitDate = oPlanToReCalculate.InitDate,
                               EndDate = oPlanToReCalculate.InitDate.AddDays(oPlanToReCalculate.DaysByPeriod),
                               CreateDate = DateTime.Now,
                               LastModify = DateTime.Now,
                               PlanPublicId = oPlanToReCalculate.PlanPublicId,
                               TotalQueries = 0,
                           });
                        EndPastPeriod = oPlanToReCalculate.InitDate.AddDays(oPlanToReCalculate.DaysByPeriod);
                    }
                    else
                    {
                        oPlanToReCalculate.RelatedPeriodModel.Add(new PeriodModel()
                        {
                            AssignedQueries = oPlanToReCalculate.QueriesByPeriod,
                            InitDate = EndPastPeriod,
                            EndDate = i == TotalPeriods ? oPlanToReCalculate.EndDate : EndPastPeriod.AddDays(oPlanToReCalculate.DaysByPeriod),
                            CreateDate = DateTime.Now,
                            LastModify = DateTime.Now,
                            PlanPublicId = oPlanToReCalculate.PlanPublicId,
                            TotalQueries = 0,
                        });
                        EndPastPeriod = EndPastPeriod.AddDays(oPlanToReCalculate.DaysByPeriod);
                    }
                }
                oPlanToReCalculate.RelatedPeriodModel.All(x =>
                {
                    ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.PeriodUpsert(
                                                            x.PeriodPublicId,
                                                            x.PlanPublicId,
                                                            x.AssignedQueries,
                                                            x.TotalQueries,
                                                            x.InitDate,
                                                            x.EndDate,
                                                            oPlanToReCalculate.Enable);
                    return true;
                });
            }
            return oPlanToReCalculate.RelatedPeriodModel;
        }

        public static List<TDCatalogModel> CatalogGetThirdKnowledgeOptions()
        {
            return ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.CatalogGetThirdKnowledgeOptions();
        }

        public static List<Models.PlanModel> GetCurrenPeriod(string CustomerPublicId, bool Enable)
        {
            return ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.GetCurrenPeriod(CustomerPublicId, Enable);
        }

        public static string PeriodoUpsert(PeriodModel oPeriodModel)
        {
            return ProveedoresOnLine.ThirdKnowledge.DAL.Controller.ThirdKnowledgeDataController.Instance.PeriodUpsert(oPeriodModel.PeriodPublicId,
                       oPeriodModel.PlanPublicId, oPeriodModel.AssignedQueries, oPeriodModel.TotalQueries, oPeriodModel.InitDate, oPeriodModel.EndDate, oPeriodModel.Enable);
        }

        #endregion

        #region Queries

        public static TDQueryModel QueryInsert(TDQueryModel QueryModelToUpsert)
        {
            if (QueryModelToUpsert != null &&
                !string.IsNullOrEmpty(QueryModelToUpsert.PeriodPublicId) &&
                QueryModelToUpsert.RelatedQueryInfoModel != null &&
                QueryModelToUpsert.RelatedQueryInfoModel.Count > 0)
            {

                QueryModelToUpsert.QueryPublicId = ThirdKnowledgeDataController.Instance.QueryInsert(QueryModelToUpsert.PeriodPublicId,
                    QueryModelToUpsert.SearchType.ItemId, QueryModelToUpsert.User, QueryModelToUpsert.IsSuccess, true);

                QueryModelToUpsert.RelatedQueryInfoModel.All(qInf =>
                {
                    //LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        qInf.QueryInfoId =
                           ThirdKnowledgeDataController.Instance.QueryInfoInsert
                            (QueryModelToUpsert.QueryPublicId,
                            qInf.ItemInfoType.ItemId, qInf.Value, qInf.LargeValue, true);

                        //oLog.IsSuccess = true;
                    }
                    catch (Exception err)
                    {
                        //oLog.IsSuccess = false;
                        //oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        //oLog.LogObject = plegal;

                        //oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        //{
                        //    LogInfoType = "CompanyPublicId",
                        //    Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        //});

                        //LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return QueryModelToUpsert;
        }

        #endregion

        #region Provate Functions
        public static TDQueryModel CreateQuery(TDQueryModel oQueryToCreate, List<string[]> CollumnsResult)
        {
            #region CreateQuery Process
            string QueryId = CollumnsResult.FirstOrDefault()[0];

            CollumnsResult = CollumnsResult.Where(x => x.Count() > 1).ToList();

            CollumnsResult.All(col =>
            {
                oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                {
                    ItemInfoType = new TDCatalogModel()
                    {
                        ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.QueryId,
                    },
                    Value = QueryId,
                });
                oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                {
                    ItemInfoType = new TDCatalogModel()
                    {
                        ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.Priotity,
                    },
                    Value = col[1],
                });
                oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                {
                    ItemInfoType = new TDCatalogModel()
                    {
                        ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.TypeDocument,
                    },
                    Value = col[2].ToString(),
                });
                oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                {
                    ItemInfoType = new TDCatalogModel()
                    {
                        ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdNumberResult,
                    },
                    Value = col[3].ToString(),
                });
                oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                {
                    ItemInfoType = new TDCatalogModel()
                    {
                        ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.NameResult,
                    },
                    Value = col[4].ToString(),
                });
                oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                {
                    ItemInfoType = new TDCatalogModel()
                    {
                        ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.IdList,
                    },
                    Value = col[5].ToString(),
                });
                oQueryToCreate.RelatedQueryInfoModel.Add(new TDQueryInfoModel()
                {
                    ItemInfoType = new TDCatalogModel()
                    {
                        ItemId = (int)ProveedoresOnLine.ThirdKnowledge.Models.Enumerations.enumThirdKnowledgeColls.ListName,
                    },
                    Value = col[6].ToString(),
                });

                return true;
            });
            #endregion

            return oQueryToCreate;
        }
        #endregion
    }
}
