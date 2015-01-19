using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Auth.Web.Controllers
{
    public partial class BaseController : Controller
    {
        #region public static properties

        /// <summary>
        /// Current Area Name
        /// </summary>
        public static string AreaName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AreaName"].ToString().Trim();
            }
        }
        #endregion

        #region public properties

        /// <summary>
        /// Return url
        /// </summary>
        public Uri ReturnUrl
        {
            get
            {
                return SessionManager.SessionController.Auth_ReturnUrl;
            }
            set
            {
                SessionManager.SessionController.Auth_ReturnUrl = value;
            }
        }

        ///<domain,application>
        public Dictionary<string, string> lstDomainApp
        {
            get
            {
                if (olstDomainApp == null)
                {
                    olstDomainApp = new Dictionary<string, string>();

                    XDocument xDoc = XDocument.Parse(SettingsManager.SettingsController.SettingsInstance.ModulesParams
                        [Auth.Interfaces.Constants.C_SettingsModuleName][Auth.Interfaces.Constants.C_AppConfig].Value);

                    xDoc.Descendants("applications").Descendants("key").All(app =>
                    {
                        app.Value.
                            Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                            All(appd =>
                            {
                                string strCurrentDomain = appd.Replace(" ", "").ToLower();
                                if (!lstDomainApp.ContainsKey(strCurrentDomain))
                                {
                                    lstDomainApp.Add(strCurrentDomain, app.Attribute("name").Value);
                                }

                                return true;
                            });
                        return true;
                    });

                    olstDomainApp = olstDomainApp.OrderByDescending(x => x.Key.Length).ToDictionary(k => k.Key, v => v.Value);
                }

                return olstDomainApp;
            }
        }
        private Dictionary<string, string> olstDomainApp;

        #endregion

        #region public methods

        /// <summary>
        /// get app name for return url to use correct login params
        /// </summary>
        /// <param name="OriginUrl">sended return url</param>
        /// <returns></returns>
        public string GetAppNameByDomain(Uri OriginUrl)
        {
            string oRetorno = string.Empty;

            string strUrlToEval = OriginUrl.ToString().Replace(" ", "").ToLower();

            oRetorno = lstDomainApp.
                Where(x => strUrlToEval.StartsWith(x.Key)).
                Select(x => x.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            return oRetorno;
        }

        /// <summary>
        /// get return url 
        /// </summary>
        /// <param name="ReturnUrl">return url to convert</param>
        /// <returns></returns>
        public Uri GetReturnUrl(string ReturnUrl)
        {
            Uri oRetorno = null;
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                //get url from previus page send
                oRetorno = Request.UrlReferrer;
            }
            else
            {
                //get url from request
                oRetorno = new Uri(ReturnUrl);
            }

            return oRetorno;
        }

        #region Login Methods

        public SessionManager.Models.Auth.User LoginUser(SessionManager.Models.Auth.User OriginalInfo)
        {
            SessionManager.Models.Auth.User oRetorno = null;

            //upsert user
            oRetorno = new SessionManager.Models.Auth.User();

            oRetorno.UserPublicId = Auth.DAL.Controller.AuthDataController.Instance.UserUpsert
                (OriginalInfo.Name,
                OriginalInfo.LastName,
                OriginalInfo.Email,
                OriginalInfo.RelatedUserProvider.
                        Select(x => x.ProviderId).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                OriginalInfo.RelatedUserProvider.
                        Select(x => x.Provider).
                        DefaultIfEmpty(SessionManager.Models.Auth.enumProvider.InternalLogin).
                        FirstOrDefault(),
                OriginalInfo.RelatedUserProvider.
                        Select(x => x.ProviderUrl).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault());

            //get temp user info
            oRetorno = Auth.DAL.Controller.AuthDataController.Instance.UserGetById(oRetorno.UserPublicId);

            //insert or update extradata
            OriginalInfo.RelatedUserInfo.All(oui =>
            {
                if (!oRetorno.RelatedUserInfo.Any(cui => oui.UserInfoType == cui.UserInfoType))
                {
                    //create user info
                    Auth.DAL.Controller.AuthDataController.Instance.UserInfoUpsert
                        (null,
                        oRetorno.UserPublicId,
                        oui.UserInfoType,
                        oui.Value,
                        oui.LargeValue);
                }
                else if (oRetorno.RelatedUserInfo.Any
                    (cui => oui.UserInfoType == cui.UserInfoType &&
                            (!string.IsNullOrEmpty(oui.Value) && oui.Value != cui.Value) ||
                            (!string.IsNullOrEmpty(oui.LargeValue) && oui.LargeValue != cui.LargeValue)))
                {
                    //update user info
                    Auth.DAL.Controller.AuthDataController.Instance.UserInfoUpsert
                       (oRetorno.RelatedUserInfo.
                            Where(ui => ui.UserInfoType == oui.UserInfoType).
                            Select(ui => (int?)ui.UserInfoId).
                            DefaultIfEmpty(null).FirstOrDefault(),
                       oRetorno.UserPublicId,
                       oui.UserInfoType,
                       oui.Value,
                       oui.LargeValue);
                }

                return true;
            });

            //get final user info
            oRetorno = Auth.DAL.Controller.AuthDataController.Instance.UserGetById(oRetorno.UserPublicId);

            //save session
            SessionManager.SessionController.Auth_UserLogin = oRetorno;

            return oRetorno;
        }

        public SessionManager.Models.Auth.User GetUserToLogin
            (DotNetOpenAuth.ApplicationBlock.IOAuth2Graph vSocialUser,
            SessionManager.Models.Auth.enumProvider vProvider)
        {
            SessionManager.Models.Auth.User ConvertUser = new SessionManager.Models.Auth.User()
            {
                Name = vSocialUser.FirstName,
                LastName = vSocialUser.LastName,
                Email = vSocialUser.Email,

                RelatedUserProvider = new List<SessionManager.Models.Auth.UserProvider>() 
                { 
                    new SessionManager.Models.Auth.UserProvider() 
                    { 
                        ProviderId = vSocialUser.Id, 
                        Provider = vProvider,
                        ProviderUrl = vSocialUser.Link != null ? vSocialUser.Link.ToString() : null,
                    }
                },

                RelatedUserInfo = new List<SessionManager.Models.Auth.UserInfo>()
                {
                    new SessionManager.Models.Auth.UserInfo()
                    {
                        UserInfoType = SessionManager.Models.Auth.enumUserInfoType.Birthday,
                        Value = vSocialUser.BirthdayDT != null ? vSocialUser.BirthdayDT.Value.ToString("yyyy/MM/dd") : string.Empty,
                    },

                    new SessionManager.Models.Auth.UserInfo()
                    {
                        UserInfoType = SessionManager.Models.Auth.enumUserInfoType.Gender,
                        Value = vSocialUser.GenderEnum == DotNetOpenAuth.ApplicationBlock.HumanGender.Female ? 
                                    ((int)SessionManager.Models.Auth.enumGender.Female).ToString() : 
                                    ((int)SessionManager.Models.Auth.enumGender.Male).ToString(),
                    },

                    new SessionManager.Models.Auth.UserInfo()
                    {
                        UserInfoType = SessionManager.Models.Auth.enumUserInfoType.ProfileImage,
                        Value = vSocialUser.AvatarUrl !=null ? vSocialUser.AvatarUrl.ToString() : string.Empty
                    },
                },
            };

            return ConvertUser;
        }

        #endregion

        #endregion
    }
}