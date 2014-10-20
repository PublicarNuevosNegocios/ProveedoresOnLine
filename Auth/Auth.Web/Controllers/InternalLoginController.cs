using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class InternalLoginController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}