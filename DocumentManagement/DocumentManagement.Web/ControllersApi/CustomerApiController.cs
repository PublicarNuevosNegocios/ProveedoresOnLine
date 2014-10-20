using DocumentManagement.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentManagement.Web.ControllersApi
{
    public class CustomerApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public CustomerSearchModel CustomerSearch
            (string CustomerSearchVal, string SearchParam, int PageNumber, int RowCount)
        {
            CustomerSearchModel oReturn = new CustomerSearchModel();

            int oTotalRows;
            oReturn.RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerSearch
                (SearchParam, PageNumber, RowCount, out oTotalRows);

            oReturn.TotalRows = oTotalRows;


            return oReturn;

        }

        [HttpPost]
        [HttpGet]
        public FormSearchModel FormSearch
            (string FormSearchVal, string CustomerPublicId, string SearchParam, int PageNumber, int RowCount)
        {
            FormSearchModel oReturn = new FormSearchModel();

            int oTotalRows;
            oReturn.RelatedForm = DocumentManagement.Customer.Controller.Customer.FormSearch
                (CustomerPublicId, SearchParam, PageNumber, RowCount, out oTotalRows);

            oReturn.TotalRows = oTotalRows;


            return oReturn;

        }

        [HttpPost]
        [HttpGet]
        public StepSearchModel StepSearch
            (string StepSearchVal, string FormPublicId)
        {
            StepSearchModel oReturn = new StepSearchModel();

            oReturn.RelatedStep = DocumentManagement.Customer.Controller.Customer.StepGetByFormId(FormPublicId);

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public FieldSearchModel FieldSearch
            (string FieldSearchVal, string StepId)
        {
            FieldSearchModel oReturn = new FieldSearchModel();

            oReturn.RelatedField = DocumentManagement.Customer.Controller.Customer.FieldGetByStepId(Convert.ToInt32(StepId));

            return oReturn;
        }

    }
}