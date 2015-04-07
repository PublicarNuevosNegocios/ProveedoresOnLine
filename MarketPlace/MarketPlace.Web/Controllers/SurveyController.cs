using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
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

            //get survey info
            oModel.RelatedSurvey = new Models.Survey.SurveyViewModel
                (ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId));

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
            oSurveyToUpsert = ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyItemUpsert(oSurveyToUpsert);

            //recalculate survey item values
            ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyRecalculate(SurveyPublicId);

            //eval redirect
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

        #region Survey pusert request

        private ProveedoresOnLine.SurveyModule.Models.SurveyModel GetSurveyUpsertRequest(string SurveyPublicId)
        {
            //get current survey values
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurvey =
               ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId);

            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                SurveyPublicId = oSurvey.SurveyPublicId,
                RelatedSurveyItem = new List<ProveedoresOnLine.SurveyModule.Models.SurveyItemModel>(),
            };

            #region get request object

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

                        ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>() 
                        {
                            new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 
                                    (oSurveyItem != null && 
                                    oSurveyItem.ItemInfo != null && 
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