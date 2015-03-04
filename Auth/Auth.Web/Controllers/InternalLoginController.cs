using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class InternalLoginController : BaseController
    {
        public virtual ActionResult Index(string UrlRetorno)
        {
            //get return url
            Uri oReturnUrl = base.GetReturnUrl(UrlRetorno);

            //get current application name
            string oAppName = base.GetAppNameByDomain(oReturnUrl);
            ViewBag.AppName = oAppName;

            //get Cookie name
            string oCookieName = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                    [Auth.Interfaces.Constants.C_IL_Cookie.Replace("{AppName}", oAppName)].Value;

            //preserve return url before request
            base.ReturnUrl = oReturnUrl;

            return RedirectToAction
                (MVC.InternalLogin.ActionNames.oauth2callback,
                MVC.InternalLogin.Name,
                new { mode = "select" });
        }

        public virtual ActionResult oauth2callback()
        {
            //get current application name
            string oAppName = base.GetAppNameByDomain(base.ReturnUrl);
            ViewBag.AppName = oAppName;

            //get Cookie name
            string oCookieName = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                    [Auth.Interfaces.Constants.C_IL_Cookie.Replace("{AppName}", oAppName)].Value;

            //get callback url
            string oCallbackUrl = Url.Action(MVC.InternalLogin.ActionNames.oauth2callback, MVC.InternalLogin.Name);
            ViewBag.CallbackUrl = oCallbackUrl;

            //validate user login
            if (Request.Cookies.AllKeys.Any(x => x == oCookieName) &&
                oCallbackUrl == Request.Url.PathAndQuery)
            {
                //get service client
                Google.Apis.IdentityToolkit.v3.IdentityToolkitService service = GetILClient(false, oAppName);

                //validate client login
                Google.Apis.IdentityToolkit.v3.Data.IdentitytoolkitRelyingpartyGetAccountInfoRequest oRequestData =
                    new Google.Apis.IdentityToolkit.v3.Data.IdentitytoolkitRelyingpartyGetAccountInfoRequest()
                    {
                        IdToken = Request.Cookies[oCookieName].Value,
                    };

                Google.Apis.IdentityToolkit.v3.RelyingpartyResource.GetAccountInfoRequest oRequest = service.Relyingparty.GetAccountInfo(oRequestData);

                Google.Apis.IdentityToolkit.v3.Data.GetAccountInfoResponse oResponse = oRequest.Execute();

                if (oResponse != null && oResponse.Users != null && oResponse.Users.Count > 0)
                {
                    //user is logged in

                    //create model login
                    SessionManager.Models.Auth.User UserToLogin = new SessionManager.Models.Auth.User()
                    {
                        Name = oResponse.Users[0].DisplayName,
                        LastName = "",
                        Email = oResponse.Users[0].Email,

                        RelatedUserProvider = new List<SessionManager.Models.Auth.UserProvider>() 
                        { 
                            new SessionManager.Models.Auth.UserProvider() 
                            { 
                                ProviderId = (oResponse.Users[0].ProviderUserInfo != null && 
                                                oResponse.Users[0].ProviderUserInfo.Any(x=>!string.IsNullOrEmpty(x.FederatedId))) ?   
                                                    oResponse.Users[0].ProviderUserInfo.Where(x=>!string.IsNullOrEmpty(x.FederatedId)).
                                                    Select(x=>x.FederatedId).DefaultIfEmpty(oResponse.Users[0].LocalId).FirstOrDefault() :  
                                                oResponse.Users[0].LocalId, 
                                Provider = SessionManager.Models.Auth.enumProvider.InternalLogin,
                                ProviderUrl = null,
                            }
                        },

                        RelatedUserInfo = new List<SessionManager.Models.Auth.UserInfo>()
                        {
                            new SessionManager.Models.Auth.UserInfo()
                            {
                                UserInfoType = SessionManager.Models.Auth.enumUserInfoType.ProfileImage,
                                Value = oResponse.Users[0].PhotoUrl,
                            },
                        },
                    };

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
            }

            return View();
        }

        public virtual JsonResult oobActionUrl()
        {
            //get current application name
            string oAppName = base.GetAppNameByDomain(base.ReturnUrl);
            ViewBag.AppName = oAppName;

            //validate reset password
            if (!string.IsNullOrEmpty(Request["action"]) &&
                Request["action"].Trim().ToLower() == "resetpassword")
            {
                //get service client
                Google.Apis.IdentityToolkit.v3.IdentityToolkitService service = GetILClient(true, oAppName);

                //get reset password url
                Google.Apis.IdentityToolkit.v3.Data.Relyingparty oRequestData =
                    new Google.Apis.IdentityToolkit.v3.Data.Relyingparty()
                    {
                        Kind = "identitytoolkit#relyingparty",
                        RequestType = "PASSWORD_RESET",
                        Email = Request["email"],
                        Challenge = Request["challenge"],
                        CaptchaResp = Request["response"],
                        UserIp = Request.UserHostAddress,
                    };

                Google.Apis.IdentityToolkit.v3.RelyingpartyResource.GetOobConfirmationCodeRequest oRequest =
                    service.Relyingparty.GetOobConfirmationCode(oRequestData);

                Google.Apis.IdentityToolkit.v3.Data.GetOobConfirmationCodeResponse oResponse = oRequest.Execute();

                //email to regenerate psw
                string oUrlRegeneratePsw = Request.Url.ToString().Replace(Request.Url.PathAndQuery, string.Empty) +
                    Url.Action
                    (MVC.InternalLogin.ActionNames.oauth2callback,
                    MVC.InternalLogin.Name,
                    new
                    {
                        mode = "resetPassword",
                        oobCode = oResponse.OobCode,
                    });

                //Send regenerate psw email
                MessageModule.Client.Controller.ClientController.CreateMessage
                    (new MessageModule.Client.Models.ClientMessageModel()
                    {
                        Agent = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                                    [Auth.Interfaces.Constants.C_IL_RememberEmailAgent.Replace("{AppName}", oAppName)].Value,
                        User = "SystemUser",
                        ProgramTime = DateTime.Now,
                        MessageQueueInfo = new System.Collections.Generic.List<Tuple<string, string>>()
                        {
                            new Tuple<string,string>("To",Request["email"]),
                            new Tuple<string,string>("RememberUrl",oUrlRegeneratePsw),
                        },
                    });


                //return success service
                return Json(new { success = true, kind = "identitytoolkit#GetOobConfirmationCodeResponse", oobCode = oResponse.OobCode, email = Request["email"] });
            }
            //return unsuccess service
            return Json(new { success = false, kind = string.Empty, oobCode = string.Empty, email = Request["email"] });
        }

        public virtual ActionResult ResetPassword(string Success, string Email)
        {
            return View();
        }

        #region private methods

        //create google instance
        Google.Apis.IdentityToolkit.v3.IdentityToolkitService GetILClient(bool IsService, string AppName)
        {
            Google.Apis.IdentityToolkit.v3.IdentityToolkitService client = null;

            if (IsService)
            {
                //get service client 

                string serviceAccountEmail = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                        [Auth.Interfaces.Constants.C_SettingsModuleName]
                        [Auth.Interfaces.Constants.C_IL_Service_Email.Replace("{AppName}", AppName)].Value;

                string p12FileLocation = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                        [Auth.Interfaces.Constants.C_SettingsModuleName]
                        [Auth.Interfaces.Constants.C_IL_Service_p12File.Replace("{AppName}", AppName)].Value;

                string oApiName = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                        [Auth.Interfaces.Constants.C_SettingsModuleName]
                        [Auth.Interfaces.Constants.C_IL_ApiName.Replace("{AppName}", AppName)].Value;

                var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2
                    (p12FileLocation,
                    "notasecret",
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable);

                Google.Apis.Auth.OAuth2.ServiceAccountCredential credential = new Google.Apis.Auth.OAuth2.ServiceAccountCredential(
                   new Google.Apis.Auth.OAuth2.ServiceAccountCredential.Initializer(serviceAccountEmail)
                   {
                       Scopes = new[] { "https://www.googleapis.com/auth/identitytoolkit" }
                   }.FromCertificate(certificate));

                client =
                    new Google.Apis.IdentityToolkit.v3.IdentityToolkitService(new Google.Apis.Services.BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = oApiName,
                    });
            }
            else
            {
                //get web client

                string oApiKey = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                        [Auth.Interfaces.Constants.C_SettingsModuleName]
                        [Auth.Interfaces.Constants.C_IL_ApiKey.Replace("{AppName}", AppName)].Value;

                string oApiName = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                                    [Auth.Interfaces.Constants.C_IL_ApiName.Replace("{AppName}", AppName)].Value;

                client =
                    new Google.Apis.IdentityToolkit.v3.IdentityToolkitService(new Google.Apis.Services.BaseClientService.Initializer()
                    {
                        ApiKey = oApiKey,
                        ApplicationName = oApiName,
                    });
            }

            return client;
        }

        #endregion
    }
}