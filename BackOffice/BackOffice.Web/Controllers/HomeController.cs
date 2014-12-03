using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            //validate user loggin
            if (BackOffice.Models.General.SessionModel.UserIsLoggedIn)
            {
                if (BackOffice.Models.General.SessionModel.UserIsAutorized)
                {
                    //redirect to provider
                    return RedirectToAction(MVC.Provider.ActionNames.Index, MVC.Provider.Name);
                }
                else
                {
                    //user is not autorized
                    ViewData[BackOffice.Models.General.Constants.C_ViewData_UserNotAutorizedText] =
                        BackOffice.Models.General.InternalSettings.Instance
                        [BackOffice.Models.General.Constants.C_Settings_Login_UserNotAutorized].Value.
                        Replace("{UserName}", BackOffice.Models.General.SessionModel.CurrentLoginUser.Email);
                }
            }

            return View();
        }

        public virtual ActionResult LogOutUser()
        {
            base.LogOut();
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }
    }
}