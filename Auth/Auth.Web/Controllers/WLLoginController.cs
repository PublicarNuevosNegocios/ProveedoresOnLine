using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class WLLoginController : BaseController
    {
        public virtual ActionResult StartLogin(string UrlRetorno)
        {
            //get return url
            base.ReturnUrl = base.GetReturnUrl(UrlRetorno);
            return RedirectToAction(MVC.WLLogin.ActionNames.Login, MVC.WLLogin.Name);
        }

        public virtual ActionResult Login()
        {
            //get current application name
            string oAppName = base.GetAppNameByDomain(base.ReturnUrl);
            ViewBag.AppName = oAppName;

            //get wl client 
            DotNetOpenAuth.ApplicationBlock.WindowsLiveClient WLClient = GetWLClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = WLClient.ProcessUserAuthorization();

            if (authorization == null)
            {
                //user is not login
                WLClient.RequestUserAuthorization(scope: new[] { 
                            DotNetOpenAuth.ApplicationBlock.WindowsLiveClient.Scopes.Basic,
                            DotNetOpenAuth.ApplicationBlock.WindowsLiveClient.Scopes.Emails,
                            DotNetOpenAuth.ApplicationBlock.WindowsLiveClient.Scopes.Birthday});
            }
            else
            {
                //get social user info
                DotNetOpenAuth.ApplicationBlock.IOAuth2Graph oauth2Graph = WLClient.GetGraph(
                        authorization, null);

                //create model login
                SessionManager.Models.Auth.User UserToLogin = base.GetUserToLogin(oauth2Graph, SessionManager.Models.Auth.enumProvider.WindowsLive);

                //login user
                UserToLogin = base.LoginUser(UserToLogin);

                //Add Log
                //CarvajalLog.LogController Log = new CarvajalLog.LogController();
                //Log.SaveLog(new CarvajalLog.Models.AuthLogModel()
                //{
                //    UserId = UserToLogin.UserId,
                //    LogAction = UserToLogin.GetType().ToString(),
                //    IsSuccessfull = 1,
                //    ErrorMessage = "el usuario inició sesión correctamente",
                //});

                //return to site
                Response.Redirect(base.ReturnUrl.ToString());
            }
            return View();
        }

        #region private methods

        //create windows live instance
        DotNetOpenAuth.ApplicationBlock.WindowsLiveClient GetWLClient(string AppName)
        {
            DotNetOpenAuth.ApplicationBlock.WindowsLiveClient client =
                new DotNetOpenAuth.ApplicationBlock.WindowsLiveClient();

            //appid
            client.ClientIdentifier = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                        [Auth.Interfaces.Constants.C_SettingsModuleName]
                        [Auth.Interfaces.Constants.C_WL_AppId.Replace("{AppName}", AppName)].Value;
            //secret key
            client.ClientCredentialApplicator = DotNetOpenAuth.OAuth2.ClientCredentialApplicator.PostParameter
                        (SettingsManager.SettingsController.SettingsInstance.ModulesParams
                            [Auth.Interfaces.Constants.C_SettingsModuleName]
                            [Auth.Interfaces.Constants.C_WL_AppSecret.Replace("{AppName}", AppName)].Value);

            return client;
        }


        #endregion
    }
}