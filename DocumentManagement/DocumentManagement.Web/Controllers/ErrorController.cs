using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class ErrorController : BaseController
    {
        public virtual ActionResult Error()
        {
            return View();
        }
    }
}