using System.Collections.Generic;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            //validate user loggin
            if (Models.General.SessionModel.UserIsLoggedIn)
            {
                //get user company info

                List<ProveedoresOnLine.Company.Models.Company.CompanyModel> UserCompany =
                    ProveedoresOnLine.Company.Controller.Company.MP_RoleCompanyGetByUser(Models.General.SessionModel.CurrentLoginUser.Email);

                Models.General.SessionModel.InitCompanyLogin(UserCompany);

                //validate user authorized
                if (Models.General.SessionModel.IsUserAuthorized())
                {
                    if (Models.General.SessionModel.CurrentCompanyType == Models.General.enumCompanyType.Provider)
                    {
                        //redirect to provider home
                        return RedirectToRoute
                            (Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.Index
                            });
                    }
                    else if(Models.General.SessionModel.CurrentCompanyType == Models.General.enumCompanyType.BuyerProvider)
                    {
                        //redirect to provider home
                        return RedirectToRoute
                            (Models.General.Constants.C_Routes_Default,
                            new
                            {
                                controller = MVC.Provider.Name,
                                action = MVC.Provider.ActionNames.Index
                            });
                    }                        
                    else
                    {
                        if (Models.General.SessionModel.CurrentURL != null)                        
                            return Redirect(Models.General.SessionModel.CurrentURL);                            
                        
                        //redirect to customer home
                        return RedirectToRoute
                        (Models.General.Constants.C_Routes_Default,
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
                    ViewData[Models.General.Constants.C_ViewData_UserNotAutorizedText] =
                        Models.General.InternalSettings.Instance
                        [Models.General.Constants.C_Settings_Login_UserNotAutorized].Value.
                        Replace("{UserName}", Models.General.SessionModel.CurrentLoginUser.Email);
                }
            }
            else
            {
                return Redirect(
                        Models.General.InternalSettings.Instance
                            [Models.General.Constants.C_Settings_Login_InternalLogin].Value.Replace(
                            "{{UrlRetorno}}",
                            Url.RouteUrl(
                                Models.General.Constants.C_Routes_Default,
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
                Models.General.Constants.C_Routes_Default,
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