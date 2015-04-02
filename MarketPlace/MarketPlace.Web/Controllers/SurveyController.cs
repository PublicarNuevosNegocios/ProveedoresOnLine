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
            //get current survey values
            MarketPlace.Models.Survey.SurveyViewModel oSurvey =
               new Models.Survey.SurveyViewModel(ProveedoresOnLine.SurveyModule.Controller.SurveyModule.SurveyGetById(SurveyPublicId));

            //loop request for update answers
            Request.Form.AllKeys.Where(x => x.Contains("SurveyItem_")).All(req =>
            {
                string[] strAux = Request[req].Split('_');

                if (strAux.Length >= 2)
                {
                    int oSurveyConfigItemId = Convert.ToInt32(strAux[1].Replace(" ", ""));

                    MarketPlace.Models.Survey.SurveyItemViewModel oSurveyItem = oSurvey.GetSurveyItem(oSurveyConfigItemId);

                    if (oSurveyItem == null)
                    {
                        oSurveyItem = new Models.Survey.SurveyItemViewModel(new ProveedoresOnLine.SurveyModule.Models.SurveyItemModel()
                        {
                            RelatedSurveyConfigItem = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                            {
                                ItemId = oSurveyConfigItemId,
                            },
                            Enable = true,
                        });
                    }

                    if (oSurveyItem.RelatedSurveyItem.ItemInfo == null)
                    {
                        oSurveyItem.RelatedSurveyItem.ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>();
                    }



                }

                return true;
            });


            ////eval each survey config item
            //var lstEvaluationArea = oSurvey.GetSurveyConfigItem(enumSurveyConfigItemType.EvaluationArea, null);

            //lstEvaluationArea.All(ea =>
            //{
            //    if(ea.)



            //    return true;
            //});







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