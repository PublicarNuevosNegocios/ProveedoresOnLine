using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class FBLoginController : BaseController
    {
        public virtual ActionResult StartLogin(string UrlRetorno)
        {
            //get return url
            base.ReturnUrl = base.GetReturnUrl(UrlRetorno);
            return RedirectToAction(MVC.FBLogin.ActionNames.Login, MVC.FBLogin.Name);
        }

        public virtual ActionResult Login()
        {
            //get current application name
            string oAppName = base.GetAppNameByDomain(base.ReturnUrl);
            ViewBag.AppName = oAppName;
            //get fb client 
            DotNetOpenAuth.ApplicationBlock.FacebookClient FBClient = GetFBClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = FBClient.ProcessUserAuthorization();

            if (authorization == null)
            {
                //user is not login
                FBClient.RequestUserAuthorization(scope: new[] { 
                            DotNetOpenAuth.ApplicationBlock.FacebookClient.Scopes.UserAboutMe,
                            DotNetOpenAuth.ApplicationBlock.FacebookClient.Scopes.Email, 
                            DotNetOpenAuth.ApplicationBlock.FacebookClient.Scopes.UserBirthday});
            }
            else
            {
                //get social user info
                DotNetOpenAuth.ApplicationBlock.IOAuth2Graph oauth2Graph = FBClient.GetGraph(
                        authorization,
                        new[] { 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Defaults, 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Email, 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Picture, 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Birthday });

                //create model login
                SessionManager.Models.Auth.User UserToLogin = base.GetUserToLogin(oauth2Graph, SessionManager.Models.Auth.enumProvider.Facebook);

                //login user
                UserToLogin = base.LoginUser(UserToLogin);

                //Add Log
                LogManager.ClientLog.AddLog(new LogManager.Models.LogModel()
                {
                    User = UserToLogin.UserPublicId,
                    Application = Auth.Interfaces.Constants.C_ApplicationName,
                    Source = Request.Url.ToString(),
                    IsSuccess = true,
                    LogObject = UserToLogin,
                });

                //return to site
                Response.Redirect(base.ReturnUrl.ToString());
            }

            return View();
        }

        #region private methods

        //create facebook instance
        DotNetOpenAuth.ApplicationBlock.FacebookClient GetFBClient(string AppName)
        {
            DotNetOpenAuth.ApplicationBlock.FacebookClient client = new DotNetOpenAuth.ApplicationBlock.FacebookClient();

            //appid
            client.ClientIdentifier = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                                    [Auth.Interfaces.Constants.C_FB_AppId.Replace("{AppName}", AppName)].Value;
            //secret key
            client.ClientCredentialApplicator = DotNetOpenAuth.OAuth2.ClientCredentialApplicator.PostParameter
                                (SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                                    [Auth.Interfaces.Constants.C_FB_AppSecret.Replace("{AppName}", AppName)].Value);

            return client;
        }

        #endregion
    }
}

