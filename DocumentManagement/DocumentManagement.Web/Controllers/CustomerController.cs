using DocumentManagement.Customer.Models.Customer;
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

        #endregion
    }
}