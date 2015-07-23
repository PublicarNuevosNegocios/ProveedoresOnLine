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
                //get user company info

                List<ProveedoresOnLine.Company.Models.Company.CompanyModel> UserCompany =
                    ProveedoresOnLine.Company.Controller.Company.MP_RoleCompanyGetByUser(MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email);

                MarketPlace.Models.General.SessionModel.InitCompanyLogin(UserCompany);

                //validate user authorized
                if (MarketPlace.Models.General.SessionModel.IsUserAuthorized())
                {
                    if (MarketPlace.Models.General.SessionModel.CurrentCompanyType == MarketPlace.Models.General.enumCompanyType.Provider)
                    {
                        //redirect to provider home
                        return RedirectToRoute
                            (MarketPlace.Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.Index
                            });
                    }
                    else
                    {
                        if (MarketPlace.Models.General.SessionModel.CurrentURL != null)
                        {
                            return Redirect(MarketPlace.Models.General.SessionModel.CurrentURL);
                        }

                        //redirect to customer home
                        return RedirectToRoute
                        (MarketPlace.Models.General.Constants.C_Routes_Default,
                        new
                        {
                            controller = MVC.Customer.Name,
                            action = MVC.Customer.ActionNames.Index
                        });
                    }
                }
                else
                {
                    //user is not autorized
                    ViewData[MarketPlace.Models.General.Constants.C_ViewData_UserNotAutorizedText] =
                        MarketPlace.Models.General.InternalSettings.Instance
                        [MarketPlace.Models.General.Constants.C_Settings_Login_UserNotAutorized].Value.
                        Replace("{UserName}", MarketPlace.Models.General.SessionModel.CurrentLoginUser.Email);
                }
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

            return RedirectToRoute(
                MarketPlace.Models.General.Constants.C_Routes_Default,
                    new
                    {
                        controller = MVC.Home.Name,
                        action = MVC.Home.ActionNames.Index
                    });
        }

        public virtual ActionResult TermsAndConditions()
        {
            return View();
        }
    }
}