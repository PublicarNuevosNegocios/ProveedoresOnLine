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

        public virtual ActionResult LoginProvider(string ProviderPublicId, string FormPublicId)
        {
            return RedirectToAction
                (MVC.ProviderForm.ActionNames.Index,
                MVC.ProviderForm.Name,
                new
                {
                    ProviderPublicId = ProviderPublicId,
                    FormPublicId = FormPublicId
                });
        }
    }
}