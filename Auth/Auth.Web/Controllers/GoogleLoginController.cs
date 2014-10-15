using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class GoogleLoginController : BaseController
    {
        public virtual ActionResult Login(string UrlRetorno)
        {
            //get return url
            Uri oReturnUrl = base.GetReturnUrl(UrlRetorno);
            //get current application name
            string oAppName = base.GetAppNameByDomain(oReturnUrl);
            ViewBag.AppName = oAppName;
            //get fb client 
            DotNetOpenAuth.ApplicationBlock.GoogleClient GMClient = GetGMClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = GMClient.ProcessUserAuthorization();

            if (authorization == null)
            {
                //preserve return url before request
                base.ReturnUrl = oReturnUrl;

                //user is not login
                GMClient.RequestUserAuthorization(scope: new[] { 
                            DotNetOpenAuth.ApplicationBlock.GoogleClient.Scopes.PlusMe,
                            DotNetOpenAuth.ApplicationBlock.GoogleClient.Scopes.UserInfo.Email,
                            DotNetOpenAuth.ApplicationBlock.GoogleClient.Scopes.UserInfo.Profile},
                            returnTo: new Uri
                                (Request.Url.GetLeftPart(UriPartial.Authority).ToLower().TrimEnd('/') +
                                Url.Action(MVC.GoogleLogin.ActionNames.LoginCallBack)));
            }
            else
            {
                return RedirectToAction(MVC.GoogleLogin.ActionNames.LoginCallBack);
            }

            return View();
        }

        public virtual ActionResult LoginCallBack()
        {
            //get return url
            Uri oReturnUrl = base.ReturnUrl;

            //get current application name
            string oAppName = base.GetAppNameByDomain(base.ReturnUrl);
            ViewBag.AppName = oAppName;
            //get fb client 
            DotNetOpenAuth.ApplicationBlock.GoogleClient GMClient = GetGMClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = GMClient.ProcessUserAuthorization();

            if (authorization != null)
            {
                DotNetOpenAuth.ApplicationBlock.IOAuth2Graph oauth2Graph = GMClient.GetGraph(authState: authorization);

                //create model login
                SessionManager.Models.Auth.User UserToLogin = base.GetUserToLogin(oauth2Graph, SessionManager.Models.Auth.enumProvider.Google);

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
                Response.Redirect(oReturnUrl.ToString());
            }

            return View();
        }

        #region private methods

        //create google instance
        DotNetOpenAuth.ApplicationBlock.GoogleClient GetGMClient(string AppName)
        {
            DotNetOpenAuth.ApplicationBlock.GoogleClient client = new DotNetOpenAuth.ApplicationBlock.GoogleClient();

            //appid
            client.ClientIdentifier = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                                    [Auth.Interfaces.Constants.C_GM_AppId.Replace("{AppName}", AppName)].Value;
            //secret key
            client.ClientCredentialApplicator = DotNetOpenAuth.OAuth2.ClientCredentialApplicator.PostParameter
                                (SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                                    [Auth.Interfaces.Constants.C_GM_AppSecret.Replace("{AppName}", AppName)].Value);

            return client;
        }

        #endregion
    }
}