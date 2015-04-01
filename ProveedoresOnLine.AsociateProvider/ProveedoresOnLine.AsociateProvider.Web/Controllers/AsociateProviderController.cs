using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProveedoresOnLine.AsociateProvider.Web.Controllers
{
    public partial class AsociateProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
