using DocumentManagement.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            ProviderSearchModel oModel = new ProviderSearchModel();
            int oTotalRows;
            oModel.Customers = DocumentManagement.Customer.Controller.Customer.CustomerSearch(null, 0, 20, out oTotalRows);
            //oModel.Forms = DocumentManagement.Customer.Controller.Customer.FormSearch(null, 0, 20, out oTotalRows);
            return View(oModel);           
        }
    }
}