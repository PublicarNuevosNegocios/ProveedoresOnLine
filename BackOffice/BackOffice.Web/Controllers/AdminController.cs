using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class AdminController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult LogOutUser()
        {         
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }
    }
}