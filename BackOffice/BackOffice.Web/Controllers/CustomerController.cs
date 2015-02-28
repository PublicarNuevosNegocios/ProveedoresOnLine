using BackOffice.Models.General;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class CustomerController : BaseController
    {
        public virtual ActionResult Index()
        {
            string oSearchParam = string.IsNullOrEmpty(Request["SearchParam"]) ? null : Request["SearchParam"];
            string oSearchFilter = string.Join(",", (Request["SearchFilter"] ?? string.Empty).Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            oSearchFilter = string.IsNullOrEmpty(oSearchFilter) ? null : oSearchFilter;

            string oCompanyType = ((int)(BackOffice.Models.General.enumCompanyType.Buyer)).ToString();

            //generic model info
            BackOffice.Models.Provider.ProviderViewModel oModel = new Models.Provider.ProviderViewModel()
            {
                ProviderOptions = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.CatalogGetProviderOptions(),
                SearchFilter = ProveedoresOnLine.Company.Controller.Company.CompanySearchFilter(oCompanyType, oSearchParam, oSearchFilter),
            };

            if (oModel.SearchFilter == null)
                oModel.SearchFilter = new List<GenericFilterModel>();

            return View(oModel);
        }      
    }
}