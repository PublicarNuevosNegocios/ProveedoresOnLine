using DocumentManagement.Customer.Models.Customer;
using DocumentManagement.Customer.Models.Form;
using DocumentManagement.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class CustomerController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult UpsertCustomer(string CustomerPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                CustomerOptions = DocumentManagement.Customer.Controller.Customer.CatalogGetCustomerOptions(),
                RelatedCustomer = new CustomerModel() { CustomerPublicId = CustomerPublicId },
            };

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oModel.RelatedCustomer = GetCustomerRequest();

                oModel.RelatedCustomer.CustomerPublicId = DocumentManagement.Customer.Controller.Customer.CustomerUpsert(oModel.RelatedCustomer);

                return RedirectToAction(MVC.Customer.ActionNames.UpsertCustomer, MVC.Customer.Name, new { CustomerPublicId = oModel.RelatedCustomer.CustomerPublicId });
            }

            if (!string.IsNullOrEmpty(oModel.RelatedCustomer.CustomerPublicId))
                oModel.RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(oModel.RelatedCustomer.CustomerPublicId);

            return View(oModel);
        }

        public virtual ActionResult ListForm(string CustomerPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
            };

            return View(oModel);
        }

        public virtual ActionResult UpsertForm(string CustomerPublicId, string FormPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
                RelatedForm = new FormModel() { FormPublicId = FormPublicId },
            };

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oModel.RelatedForm = GetFormRequest();

                oModel.RelatedForm.FormPublicId = DocumentManagement.Customer.Controller.Customer.FormUpsert
                    (oModel.RelatedCustomer.CustomerPublicId,
                    oModel.RelatedForm);

                return RedirectToAction(MVC.Customer.ActionNames.UpsertForm, MVC.Customer.Name,
                    new
                    {
                        CustomerPublicId = oModel.RelatedCustomer.CustomerPublicId,
                        FormPublicId = oModel.RelatedForm.FormPublicId,
                    });
            }

            if (!string.IsNullOrEmpty(oModel.RelatedForm.FormPublicId))
            {
                oModel.RelatedForm = DocumentManagement.Customer.Controller.Customer.
                    CustomerGetByFormId(oModel.RelatedForm.FormPublicId).
                    RelatedForm.
                    Where(x => x.FormPublicId == FormPublicId).
                    FirstOrDefault();
            }

            return View(oModel);
        }

        public virtual ActionResult UpsertFormLogo(string CustomerPublicId, string FormPublicId, HttpPostedFileBase UploadFile)
        {
            //get folder
            string strFolder = Server.MapPath(DocumentManagement.Models.General.Constants.C_Settings_File_TempDirectory);

            if (!System.IO.Directory.Exists(strFolder))
                System.IO.Directory.CreateDirectory(strFolder);

            //get File
            string strFile = strFolder.TrimEnd('\\') +
                "\\Logo_" +
                CustomerPublicId + "_" +
                DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpeg";


            UploadFile.SaveAs(strFile);

            return RedirectToAction(MVC.Customer.ActionNames.UpsertForm, MVC.Customer.Name, new
            {
                CustomerPublicId = CustomerPublicId,
                FormPublicId = FormPublicId
            });
        }

        public virtual ActionResult UploadProvider(string CustomerPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
            };

            return View(oModel);
        }

        #region private methods

        private CustomerModel GetCustomerRequest()
        {
            CustomerModel oReturn = new CustomerModel();

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oReturn.CustomerPublicId = Request["CustomerPublicId"];
                oReturn.Name = Request["Name"];
                oReturn.IdentificationType = new Customer.Models.Util.CatalogModel() { ItemId = Convert.ToInt32(Request["IdentificationType"]), };
                oReturn.IdentificationNumber = Request["IdentificationNumber"];
            }

            return oReturn;
        }

        private FormModel GetFormRequest()
        {
            FormModel oReturn = new FormModel();

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oReturn.FormPublicId = Request["FormPublicId"];
                oReturn.Name = Request["Name"];
                oReturn.TermsAndConditions = Request["TermsAndConditions"];
            }

            return oReturn;
        }

        #endregion
    }
}