using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            //validate user loggin
            if (DocumentManagement.Models.General.SessionModel.UserIsLoggedIn)
            {
                if (DocumentManagement.Models.General.SessionModel.UserIsAutorized)
                {
                    //redirect to provider
                    return RedirectToAction(MVC.Provider.ActionNames.Index, MVC.Provider.Name);
                }
                else
                {
                    //user is not autorized
                    ViewData[DocumentManagement.Models.General.Constants.C_ViewData_UserNotAutorizedText] =
                        DocumentManagement.Models.General.InternalSettings.Instance
                        [DocumentManagement.Models.General.Constants.C_Settings_Login_UserNotAutorized].Value.
                        Replace("{UserName}", DocumentManagement.Models.General.SessionModel.CurrentLoginUser.Email);
                }
            }

            return View();
        }
    }
}