using DocumentManagement.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentManagement.Web.ControllersApi
{
    public class ProviderApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public ProviderSearchModel ProviderSearch
            (string CustomerSearchVal, string SearchParam, int PageNumber, int RowCount)
        {
            ProviderSearchModel oReturn = new ProviderSearchModel();

            //int oTotalRows;
            ////oReturn.RelatedProvider = DocumentManagement.Provider.Controller.Provider.CustomerSearch
            ////    (SearchParam, PageNumber, RowCount, out oTotalRows);

            //oReturn.TotalRows = oTotalRows;


            return oReturn;

        }
    }
}