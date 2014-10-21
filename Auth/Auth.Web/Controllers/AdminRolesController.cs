using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class AdminRolesController : BaseController
    {
        public virtual ActionResult Index()
        {
            //Listar roles


            return View();
        }

        public virtual ActionResult AutorizationUpsert(string Email, string rol)
        {
            //ingresar datos 


            return RedirectToAction(MVC.AdminRoles.ActionNames.Index, MVC.AdminRoles.Name);
        }
    }
}