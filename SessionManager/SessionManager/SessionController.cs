using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager
{
    public class SessionController
    {
        #region Auth

        public static void Logout()
        {
            System.Web.HttpContext.Current.Session.Abandon();
        }

        public static Uri Auth_ReturnUrl
        {
            get
            {
                return (Uri)System.Web.HttpContext.Current.Session[SessionManager.Models.Constants.C_Session_Auth_ReturnUrl];
            }
            set
            {
                System.Web.HttpContext.Current.Session[SessionManager.Models.Constants.C_Session_Auth_ReturnUrl] = value;
            }
        }

        public static SessionManager.Models.Auth.User Auth_UserLogin
        {
            get
            {
                return (SessionManager.Models.Auth.User)System.Web.HttpContext.Current.Session[SessionManager.Models.Constants.C_Session_Auth_UserLogin];
            }
            set
            {
                System.Web.HttpContext.Current.Session[SessionManager.Models.Constants.C_Session_Auth_UserLogin] = value;
            }
        }

        public static SessionManager.Models.POLMarketPlace.MarketPlaceUser MarketPlace_MarketPlaceUserLogin
        {
            get
            {
                return (SessionManager.Models.POLMarketPlace.MarketPlaceUser)System.Web.HttpContext.Current.Session[SessionManager.Models.Constants.C_Session_POLMarketPlace_MarketPlaceUserLogin];
            }
            set
            {
                System.Web.HttpContext.Current.Session[SessionManager.Models.Constants.C_Session_POLMarketPlace_MarketPlaceUserLogin] = value;
            }
        }

        #endregion
    }
}
