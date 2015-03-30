using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProveedoresOnLine.AsociateProvider.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            //validate user loggin
            if (Models.General.SessionModel.UserIsLoggedIn)
            {
                if (Models.General.SessionModel.UserIsAutorized)
                {
                    //redirect
                    return RedirectToAction(MVC.AsociateProvider.ActionNames.Index, MVC.AsociateProvider.Name);
                }
                else
                {
                    //user is not authorized
                    ViewData[Models.General.Constants.C_ViewData_UserNotAutorizedText] =
                        Models.General.InternalSettings.Instance
                        [Models.General.Constants.C_Settings_Login_UserNotAutorized].Value.
                        Replace("{UserName}", Models.General.SessionModel.CurrentLoginUser.Email);
                }
            }

            return View();
        }

        public virtual ActionResult LogOutUser()
        {
            SessionManager.SessionController.Logout();
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }
    }
}
