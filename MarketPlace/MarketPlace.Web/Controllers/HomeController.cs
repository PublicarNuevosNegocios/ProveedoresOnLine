using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            //validate user loggin
            if (MarketPlace.Models.General.SessionModel.UserIsLoggedIn)
            {
                //if (BackOffice.Models.General.SessionModel.UserIsAutorized)
                //{
                //    //redirect to provider
                //    return RedirectToAction(MVC.Provider.ActionNames.Index, MVC.Provider.Name);
                //}
                //else
                //{
                //    //user is not autorized
                //    ViewData[BackOffice.Models.General.Constants.C_ViewData_UserNotAutorizedText] =
                //        BackOffice.Models.General.InternalSettings.Instance
                //        [BackOffice.Models.General.Constants.C_Settings_Login_UserNotAutorized].Value.
                //        Replace("{UserName}", BackOffice.Models.General.SessionModel.CurrentLoginUser.Email);
                //}
            }
            else
            {
                return Redirect(
                        MarketPlace.Models.General.InternalSettings.Instance
                            [MarketPlace.Models.General.Constants.C_Settings_Login_InternalLogin].Value.Replace(
                            "{{UrlRetorno}}",
                            Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Routes_Default,
                                new
                                {
                                    controller = MVC.Home.Name,
                                    action = MVC.Home.ActionNames.Index
                                },
                                Request.Url.Scheme)));
            }

            return View();
        }

        public virtual ActionResult LogOutUser()
        {
            SessionManager.SessionController.Logout();

            return RedirectToRoute(Url.RouteUrl(
                MarketPlace.Models.General.Constants.C_Routes_Default,
                    new
                    {
                        controller = MVC.Home.Name,
                        action = MVC.Home.ActionNames.Index
                    },
                    Request.Url.Scheme));
        }
    }
}