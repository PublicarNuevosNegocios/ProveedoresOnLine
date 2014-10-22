using DocumentManagement.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class ProviderFormController : BaseController
    {
        public virtual ActionResult Index(string ProviderPublicId, string FormPublicId, string StepId)
        {
            int? oStepId = string.IsNullOrEmpty(StepId) ? null : (int?)Convert.ToInt32(StepId.Trim());

            ProviderFormModel oModel = new ProviderFormModel()
            {
                ProviderOptions = DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions(),
                RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId),
                RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, oStepId),
            };

            oModel.RealtedForm = oModel.RealtedCustomer.
                RelatedForm.
                Where(x => x.FormPublicId == FormPublicId).
                FirstOrDefault();

            if (oStepId != null)
            {
                oModel.RealtedStep = oModel.RealtedForm.RelatedStep.
                    Where(x => x.StepId == (int)oStepId).
                    FirstOrDefault();
            }

            return View(oModel);
        }

        public virtual ActionResult LoginProvider(string ProviderPublicId, string FormPublicId, string CustomerPublicId)
        {
            //get Provider info
            DocumentManagement.Provider.Models.Provider.ProviderModel RequestResult = GetLoginRequest();

            DocumentManagement.Provider.Models.Provider.ProviderModel RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetByIdentification
                (RequestResult.IdentificationNumber, RequestResult.IdentificationType.ItemId, CustomerPublicId);

            if (RealtedProvider != null && !string.IsNullOrEmpty(RealtedProvider.ProviderPublicId))
            {
                //get first step
                DocumentManagement.Customer.Models.Customer.CustomerModel RealtedCustomer =
                    DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId);

                //loggin success
                return RedirectToAction
                    (MVC.ProviderForm.ActionNames.Index,
                    MVC.ProviderForm.Name,
                    new
                    {
                        ProviderPublicId = ProviderPublicId,
                        FormPublicId = FormPublicId,
                        StepId = RealtedCustomer.
                            RelatedForm.
                            Where(x => x.FormPublicId == FormPublicId).
                            FirstOrDefault().
                            RelatedStep.OrderBy(x => x.Position).
                            FirstOrDefault().
                            StepId,
                    });
            }
            else
            {
                //loggin failed
                return RedirectToAction
                    (MVC.ProviderForm.ActionNames.Index,
                    MVC.ProviderForm.Name,
                    new
                    {
                        ProviderPublicId = ProviderPublicId,
                        FormPublicId = FormPublicId,
                        StepId = string.Empty
                    });
            }
        }

        public virtual ActionResult UpsertGenericStep(string ProviderPublicId, string FormPublicId, string StepId, string NewStepId)
        {
            //validate upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"] == "true")
            {
                DocumentManagement.Provider.Models.Provider.ProviderModel RequestResult = GetUpsertGenericStepRequest();

            }


            //int? oStepId = string.IsNullOrEmpty(StepId) ? null : (int?)Convert.ToInt32(StepId.Trim());

            //ProviderFormModel oModel = new ProviderFormModel()
            //{
            //    ProviderOptions = DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions(),
            //    RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId),
            //    RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, oStepId),
            //};

            //oModel.RealtedForm = oModel.RealtedCustomer.
            //    RelatedForm.
            //    Where(x => x.FormPublicId == FormPublicId).
            //    FirstOrDefault();

            //if (oStepId != null)
            //{
            //    oModel.RealtedStep = oModel.RealtedForm.RelatedStep.
            //        Where(x => x.StepId == (int)oStepId).
            //        FirstOrDefault();
            //}

            //return View(oModel);

            //save success
            return RedirectToAction
                (MVC.ProviderForm.ActionNames.Index,
                MVC.ProviderForm.Name,
                new
                {
                    ProviderPublicId = ProviderPublicId,
                    FormPublicId = FormPublicId,
                    StepId = StepId
                });
        }

        #region PrivateMethods

        private DocumentManagement.Provider.Models.Provider.ProviderModel GetLoginRequest()
        {
            DocumentManagement.Provider.Models.Provider.ProviderModel oReturn = new DocumentManagement.Provider.Models.Provider.ProviderModel();

            #region StepLogin

            if (!string.IsNullOrEmpty(Request["IdentificationType"]))
            {
                oReturn.IdentificationType = new Provider.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["IdentificationType"]),
                };
            }

            if (!string.IsNullOrEmpty(Request["IdentificationNumber"]))
            {
                oReturn.IdentificationNumber = Request["IdentificationNumber"].Trim();
            }

            #endregion

            return oReturn;
        }

        private DocumentManagement.Provider.Models.Provider.ProviderModel GetUpsertGenericStepRequest()
        {
            DocumentManagement.Provider.Models.Provider.ProviderModel oReturn = new DocumentManagement.Provider.Models.Provider.ProviderModel();

            //loop request
            Dictionary<string, string> ValidRequest = Request.
                Form.
                AllKeys.
                Select(rq => new
                {
                    Key = rq,
                    Value = rq.Split('-').
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault(),
                }).
                Where(rq => !string.IsNullOrEmpty(rq.Value) &&
                            ProviderFormModel.FieldTypes.Any(ft => ft.ToLower().Replace(" ", "") == rq.Value.ToLower().Replace(" ", ""))).
                ToDictionary(k => k.Key, v => v.Value);

            oReturn.RelatedProviderInfo = new List<Provider.Models.Provider.ProviderInfoModel>();

            ValidRequest.All(reqKey =>
                {
                    if (MVC.Shared.Views._P_FieldEmail.IndexOf(reqKey.Value) >= 0)
                    {
                        oReturn.RelatedProviderInfo.Add(GetFieldEmailRequest(reqKey.Key));
                    }
                    return true;
                });

            return oReturn;
        }

        private DocumentManagement.Provider.Models.Provider.ProviderInfoModel GetFieldEmailRequest(string RequestKey)
        {




            return new Provider.Models.Provider.ProviderInfoModel();
        }

        #endregion

    }
}