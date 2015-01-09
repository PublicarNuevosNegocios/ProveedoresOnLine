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

            return RedirectToAction
                (MVC.InternalLogin.ActionNames.oauth2callback,
                MVC.InternalLogin.Name);
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

            //validate user login
            if (Request.Cookies.AllKeys.Any(x => x == oCookieName))
            {
                Google.Apis.IdentityToolkit.v3.IdentityToolkitService service = GetILClient(oAppName);

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
                                                oResponse.Users[0].ProviderUserInfo.Any(x=>!string.IsNullOrEmpty(x.ProviderId))) ?   
                                                    oResponse.Users[0].ProviderUserInfo.Where(x=>!string.IsNullOrEmpty(x.ProviderId)).
                                                    Select(x=>x.ProviderId).DefaultIfEmpty(oResponse.Users[0].LocalId).FirstOrDefault() :  
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
            Google.Apis.IdentityToolkit.v3.IdentityToolkitService service =
                new Google.Apis.IdentityToolkit.v3.IdentityToolkitService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyB2uX9RlXiwvVZd6Dj82NmFrMYObmXnUqQ",
                    ApplicationName = "app 1",
                });

            Google.Apis.IdentityToolkit.v3.Data.Relyingparty oRequestData1 =
                new Google.Apis.IdentityToolkit.v3.Data.Relyingparty()
                {
                    Kind = "identitytoolkit#relyingparty",
                    RequestType = "PASSWORD_RESET",
                    //RequestType = Request["action"],
                    Email = Request["email"],
                    Challenge = Request["challenge"],
                    CaptchaResp = Request["response"],
                    UserIp = Request.UserHostAddress,
                };

            Google.Apis.IdentityToolkit.v3.RelyingpartyResource.GetOobConfirmationCodeRequest oRequest1 = service.Relyingparty.GetOobConfirmationCode(oRequestData1);

            Google.Apis.IdentityToolkit.v3.Data.GetOobConfirmationCodeResponse oResponse1 = oRequest1.Execute();

            return Json(new { kind = "identitytoolkit#GetOobConfirmationCodeResponse", oobCode = oResponse1.OobCode });
        }

        #region private methods

        //create google instance
        Google.Apis.IdentityToolkit.v3.IdentityToolkitService GetILClient(string AppName)
        {
            string oApiKey = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                    [Auth.Interfaces.Constants.C_SettingsModuleName]
                    [Auth.Interfaces.Constants.C_IL_ApiKey.Replace("{AppName}", AppName)].Value;

            string oApiName = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                [Auth.Interfaces.Constants.C_SettingsModuleName]
                                [Auth.Interfaces.Constants.C_IL_ApiName.Replace("{AppName}", AppName)].Value;

            Google.Apis.IdentityToolkit.v3.IdentityToolkitService client =
                new Google.Apis.IdentityToolkit.v3.IdentityToolkitService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    ApiKey = oApiKey,
                    ApplicationName = oApiName,
                });

            return client;
        }

        #endregion
    }
}