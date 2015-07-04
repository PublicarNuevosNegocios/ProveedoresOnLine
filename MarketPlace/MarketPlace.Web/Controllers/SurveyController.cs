using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using MarketPlace.Models.Survey;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class SurveyController : BaseController
    {
        public virtual ActionResult Index(string SurveyPublicId, string StepId)
        {
            ProviderViewModel oModel = new ProviderViewModel();
            oModel.RelatedSurvey = new Models.Survey.SurveyViewModel
                (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId));

            //get survey info
            if (oModel.RelatedSurvey.RelatedSurvey.ParentSurveyPublicId == null)
            {                
                oModel.RelatedSurvey = new SurveyViewModel(
                    ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetByUser
                    (SurveyPublicId, SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().User));     
            }
            List<GenericItemInfoModel> oAssignedAreas = oModel.RelatedSurvey.RelatedSurvey.SurveyInfo.Where(inf => inf.ItemInfoType.ItemId == (int)enumSurveyInfoType.CurrentArea).Select(inf => inf).ToList();

            //Get Only Rol Area's
            List<GenericItemModel> Areas = new List<GenericItemModel>();
            List<GenericItemModel> Answers = new List<GenericItemModel>();

            if (oAssignedAreas.Count > 0)
            {
                oAssignedAreas.All(ev =>
                {
                    //Get Areas
                    Areas.AddRange(oModel.RelatedSurvey.RelatedSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.Where(x => x.ItemId == Convert.ToInt32(ev.Value)).Select(x => x).ToList());
                    //Get Areas Iteminfo
                    Areas.AddRange(oModel.RelatedSurvey.RelatedSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.Where(x => x.ParentItem != null && x.ParentItem.ItemId == Convert.ToInt32(ev.Value)).Select(x => x).ToList());
                    return true;
                });
                if (Areas.Count > 0)
                {
                    Areas.All(qs =>
                    {
                        Answers.AddRange(oModel.RelatedSurvey.RelatedSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.Where(x => x.ItemType.ItemId == (int)enumSurveyConfigItemType.Answer && x.ParentItem.ItemId == qs.ItemId).Select(x => x).ToList());
                        return true;
                    });
                    if (Answers.Count > 0)
                    {
                        Areas.AddRange(Answers);
                    }
                }
                oModel.RelatedSurvey.RelatedSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem = Areas;
            }
                               
            //get basic provider info
            var olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (SessionModel.CurrentCompany.CompanyPublicId, oModel.RelatedSurvey.RelatedSurvey.RelatedProvider.RelatedCompany.CompanyPublicId);

            var oProvider = olstProvider.
                Where(x => SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.BuyerProvider ?
                            (x.RelatedCompany.CompanyPublicId == oModel.RelatedSurvey.RelatedSurvey.RelatedProvider.RelatedCompany.CompanyPublicId ||
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId)) :
                            (SessionModel.CurrentCompany.CompanyType.ItemId == (int)enumCompanyType.Buyer ?
                            x.RelatedCustomerInfo.Any(y => y.Key == SessionModel.CurrentCompany.CompanyPublicId) :
                            x.RelatedCompany.CompanyPublicId == oModel.RelatedSurvey.RelatedSurvey.RelatedProvider.RelatedCompany.CompanyPublicId)).
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

                oModel.RelatedSurvey.RelatedSurvey.RelatedSurveyItem = oModel.RelatedSurvey.RelatedSurvey.RelatedSurveyItem.
                                                Where(x => x.EvaluatorRoleId == SessionManager.SessionController.POLMarketPlace_MarketPlaceUserLogin.RelatedCompany.FirstOrDefault().RelatedUser.
                                                               Where(y => y.RelatedRole.ItemId != 0).Select(y => y.UserCompanyId).FirstOrDefault()).Select(x => x).ToList();

                //get current StepId
                oModel.RelatedSurvey.CurrentStepId = string.IsNullOrEmpty(StepId) ? oModel.RelatedSurvey.GetSurveyConfigFirstStep() : Convert.ToInt32(StepId.Trim());

                //get survey action menu
                oModel.RelatedSurvey.CurrentActionMenu = GetSurveyAction(oModel);
            }

            return View(oModel);
        }

        public virtual ActionResult SurveyUpsert(string SurveyPublicId, string StepId)
        {
            //get survey request model
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert =
                GetSurveyUpsertRequest(SurveyPublicId);
         
            //upsert survey
            oSurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyInfoUpsert(oSurveyToUpsert);
            oSurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyItemUpsert(oSurveyToUpsert);
            
            //recalculate survey item values
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyRecalculate(SurveyPublicId, SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().RelatedRole.ItemId);

            //redirect
            return RedirectToRoute
                (MarketPlace.Models.General.Constants.C_Routes_Default,
                new
                {
                    controller = MVC.Survey.Name,
                    action = MVC.Survey.ActionNames.Index,
                    SurveyPublicId = SurveyPublicId,
                    StepId = StepId,
                });
        }

        public virtual ActionResult SurveyFinalize(string SurveyPublicId)
        {
            //get survey request model
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert =
                GetSurveyFinalizeRequest(SurveyPublicId);

            //upsert survey
            if (oSurveyToUpsert.SurveyInfo != null && oSurveyToUpsert.SurveyInfo.Count > 0)
            {
                //upsert survey info
                oSurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyInfoUpsert(oSurveyToUpsert);
            }

            //recalculate survey item values
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyRecalculate(SurveyPublicId, SessionModel.CurrentCompany.RelatedUser.FirstOrDefault().UserCompanyId);

            //redirect
            return RedirectToRoute
                (MarketPlace.Models.General.Constants.C_Routes_Default,
                new
                {
                    controller = MVC.Survey.Name,
                    action = MVC.Survey.ActionNames.Index,
                    SurveyPublicId = SurveyPublicId,
                });
        }

        #region Survey request

        private ProveedoresOnLine.SurveyModule.Models.SurveyModel GetSurveyUpsertRequest(string SurveyPublicId)
        {
            //get current survey values
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurvey =
               ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId);

            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                SurveyPublicId = oSurvey.SurveyPublicId,
                RelatedSurveyItem = new List<ProveedoresOnLine.SurveyModule.Models.SurveyItemModel>(),
                SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            #region get request infos

            Request.Form.AllKeys.Where(req => req.Contains("SurveyInfo_")).All(req =>
            {
                string[] strAux = req.Split('_');

                if (strAux.Length >= 2)
                {
                    int oSurveyInfoTypeId = Convert.ToInt32(strAux[1].Replace(" ", ""));
                    int oSurveyInfoId = strAux.Length >= 3 && !string.IsNullOrEmpty(strAux[2]) ? Convert.ToInt32(strAux[2].Replace(" ", "")) : 0;

                    oSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                    {
                        ItemInfoId = oSurveyInfoId,
                        ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                        {
                            ItemId = oSurveyInfoTypeId,
                        },
                        Value = Request[req].Replace(" ", ""),
                        Enable = true,
                    });
                }
                return true;
            });

            #endregion

            #region get request answers

            //loop request for update answers
            Request.Form.AllKeys.Where(req => req.Contains("SurveyItem_")).All(req =>
            {
                string[] strAux = req.Split('_');

                if (strAux.Length >= 2)
                {
                    int oSurveyConfigItemId = Convert.ToInt32(strAux[1].Replace(" ", ""));

                    ProveedoresOnLine.SurveyModule.Models.SurveyItemModel oSurveyItem = oSurvey.RelatedSurveyItem.
                        Where(sit => sit.RelatedSurveyConfigItem.ItemId == oSurveyConfigItemId).
                        FirstOrDefault();

                    ProveedoresOnLine.SurveyModule.Models.SurveyItemModel oSurveyItemToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyItemModel()
                    {
                        ItemId = oSurveyItem != null ? oSurveyItem.ItemId : 0,
                        RelatedSurveyConfigItem = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                        {
                            ItemId = oSurveyConfigItemId,
                        },
                        Enable = true,
                        EvaluatorRoleId = SessionManager.SessionController.POLMarketPlace_MarketPlaceUserLogin.RelatedCompany.FirstOrDefault().RelatedUser.
                                                               Where(y => y.RelatedRole.ItemId != 0).Select(y => y.UserCompanyId).FirstOrDefault(),

                        ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>() 
                        {
                            new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 
                                    (oSurveyItem != null && 
                                    oSurveyItem.ItemInfo != null && oSurveyItem.ItemInfo.Any(r => r.LargeValue == SessionModel.CurrentCompanyLoginUser.RelatedCompany.FirstOrDefault().RelatedUser.FirstOrDefault().UserCompanyId.ToString()) &&
                                    oSurveyItem.ItemInfo.Any(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.Ratting)) ?
                                    oSurveyItem.ItemInfo.
                                        Where(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.Ratting).
                                        Select(sitinf=>sitinf.ItemInfoId).
                                        DefaultIfEmpty(0).
                                        FirstOrDefault() : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyItemInfoType.Ratting,
                                },
                                Value = oSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.
                                    Where(scit=> scit.ItemType.ItemId == (int)enumSurveyConfigItemType.Answer &&                                                
                                                 scit.ItemId.ToString() == Request[req].Replace(" ","")).
                                    Select(scit=>scit.ItemInfo.
                                                    Where(scitinf=>scitinf.ItemInfoType.ItemId == (int)enumSurveyConfigItemInfoType.Weight).
                                                    Select(scitinf=>Convert.ToInt32( scitinf.Value)).
                                                    DefaultIfEmpty(0).
                                                    FirstOrDefault()).
                                    DefaultIfEmpty(0).
                                    FirstOrDefault().ToString(),
                                Enable = true,
                            },
                            new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 
                                    (oSurveyItem != null && 
                                    oSurveyItem.ItemInfo != null && 
                                     oSurveyItem.ItemInfo != null && oSurveyItem.ItemInfo.Any(r => r.LargeValue == SessionModel.CurrentCompanyLoginUser.RelatedCompany.FirstOrDefault().RelatedUser.FirstOrDefault().UserCompanyId.ToString()) &&
                                    oSurveyItem.ItemInfo.Any(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.Answer)) ?
                                    oSurveyItem.ItemInfo.
                                        Where(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.Answer).
                                        Select(sitinf=>sitinf.ItemInfoId).
                                        DefaultIfEmpty(0).
                                        FirstOrDefault() : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyItemInfoType.Answer,
                                },
                                Value = Request[req],
                                Enable = true,
                            },
                            new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 
                                    (oSurveyItem != null && 
                                    oSurveyItem.ItemInfo != null && 
                                     oSurveyItem.ItemInfo != null && oSurveyItem.ItemInfo.Any(r => r.LargeValue == SessionModel.CurrentCompanyLoginUser.RelatedCompany.FirstOrDefault().RelatedUser.FirstOrDefault().UserCompanyId.ToString()) &&
                                    oSurveyItem.ItemInfo.Any(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.DescriptionText)) ?
                                    oSurveyItem.ItemInfo.
                                        Where(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.DescriptionText).
                                        Select(sitinf=>sitinf.ItemInfoId).
                                        DefaultIfEmpty(0).
                                        FirstOrDefault() : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyItemInfoType.DescriptionText,
                                },
                                Value = Request["SurveyItemText_" + oSurveyConfigItemId],
                                Enable = true,
                            },          
                             new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 
                                    (oSurveyItem != null && 
                                    oSurveyItem.ItemInfo != null && 
                                     oSurveyItem.ItemInfo != null && oSurveyItem.ItemInfo.Any(r => r.LargeValue == SessionModel.CurrentCompanyLoginUser.RelatedCompany.FirstOrDefault().RelatedUser.FirstOrDefault().UserCompanyId.ToString()) &&
                                    oSurveyItem.ItemInfo.Any(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.EvaluatorRol)) ?
                                    oSurveyItem.ItemInfo.
                                        Where(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.EvaluatorRol).
                                        Select(sitinf=>sitinf.ItemInfoId).
                                        DefaultIfEmpty(0).
                                        FirstOrDefault() : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyItemInfoType.EvaluatorRol,
                                },
                                Value = SessionManager.SessionController.POLMarketPlace_MarketPlaceUserLogin.RelatedCompany.FirstOrDefault().RelatedUser.
                                                Where(y => y.RelatedRole.ItemId != 0).Select(y => y.RelatedRole.ItemId.ToString()).FirstOrDefault(),
                                Enable = true,
                            },          
                             new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 
                                    (oSurveyItem != null && 
                                     oSurveyItem.ItemInfo != null && oSurveyItem.ItemInfo.Any(r => r.LargeValue == SessionModel.CurrentCompanyLoginUser.RelatedCompany.FirstOrDefault().RelatedUser.FirstOrDefault().UserCompanyId.ToString()) &&
                                    oSurveyItem.ItemInfo != null && 
                                    oSurveyItem.ItemInfo.Any(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.EvaluatorName)) ?
                                    oSurveyItem.ItemInfo.
                                        Where(sitinf=>sitinf.ItemInfoType.ItemId == (int)enumSurveyItemInfoType.EvaluatorName).
                                        Select(sitinf=>sitinf.ItemInfoId).
                                        DefaultIfEmpty(0).
                                        FirstOrDefault() : 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)enumSurveyItemInfoType.EvaluatorName,
                                },
                                Value = SessionManager.SessionController.POLMarketPlace_MarketPlaceUserLogin.RelatedCompany.FirstOrDefault().RelatedUser.
                                             Where(y => y.User != null).Select(y => y.User).FirstOrDefault(),
                                Enable = true,
                            },          
                        },
                    };
                    //add survey item to survey to upsert
                    oSurveyToUpsert.RelatedSurveyItem.Add(oSurveyItemToUpsert);
                }
                return true;
            });

            #endregion

            return oSurveyToUpsert;
        }

        private ProveedoresOnLine.SurveyModule.Models.SurveyModel GetSurveyFinalizeRequest(string SurveyPublicId)
        {
            //get current survey values
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurvey =
               ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId);

            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                SurveyPublicId = oSurvey.SurveyPublicId,
                RelatedSurveyItem = new List<ProveedoresOnLine.SurveyModule.Models.SurveyItemModel>(),
                SurveyInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            #region get request infos

            Request.Form.AllKeys.Where(req => req.Contains("SurveyInfo_")).All(req =>
            {
                string[] strAux = req.Split('_');

                if (strAux.Length >= 2)
                {
                    int oSurveyInfoTypeId = Convert.ToInt32(strAux[1].Replace(" ", ""));
                    int oSurveyInfoId = strAux.Length >= 3 && !string.IsNullOrEmpty(strAux[2]) ? Convert.ToInt32(strAux[2].Replace(" ", "")) : 0;

                    if (oSurveyInfoTypeId == (int)enumSurveyInfoType.Status)
                    {
                        oSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = oSurveyInfoId,
                            ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                            {
                                ItemId = oSurveyInfoTypeId,
                            },
                            Value = ((int)enumSurveyStatus.Close).ToString(),
                            Enable = true,
                        });
                    }
                }
                return true;
            });

            #endregion

            return oSurveyToUpsert;
        }

        #endregion

        #region Menu

        private GenericMenu GetSurveyAction(ProviderViewModel vProviderInfo)
        {
            GenericMenu oReturn = new GenericMenu()
            {
                Name = "Guardar",
                Url = Url.RouteUrl(MarketPlace.Models.General.Constants.C_Routes_Default,
                    new
                    {
                        controller = MVC.Survey.Name,
                        action = MVC.Survey.ActionNames.SurveyUpsert,
                        SurveyPublicId = vProviderInfo.RelatedSurvey.SurveyPublicId,
                        StepId = vProviderInfo.RelatedSurvey.CurrentStepId.ToString(),
                    }),
            };

            //eval if has steps
            if (vProviderInfo.RelatedSurvey.SurveyConfigStepEnable)
            {
                Tuple<int?, int?> oStepInfo = vProviderInfo.RelatedSurvey.GetSurveyConfigSteps();

                if (oStepInfo.Item1 != null)
                {
                    oReturn.LastMenu = new GenericMenu()
                    {
                        Name = "Anterior",
                        Url = Url.RouteUrl(MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Survey.Name,
                                action = MVC.Survey.ActionNames.SurveyUpsert,
                                SurveyPublicId = vProviderInfo.RelatedSurvey.SurveyPublicId,
                                StepId = oStepInfo.Item1.Value.ToString()
                            }),
                    };
                }
                if (oStepInfo.Item2 != null)
                {
                    oReturn.NextMenu = new GenericMenu()
                    {
                        Name = "Siguiente",
                        Url = Url.RouteUrl(MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Survey.Name,
                                action = MVC.Survey.ActionNames.SurveyUpsert,
                                SurveyPublicId = vProviderInfo.RelatedSurvey.SurveyPublicId,
                                StepId = oStepInfo.Item2.Value.ToString()
                            }),
                    };
                }
            }

            return oReturn;
        }

        #endregion
    }
}