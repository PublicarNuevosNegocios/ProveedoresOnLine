﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MarketPlace.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static bool EnableSSL
        {
            get
            {
                try
                {
                    return Convert.ToBoolean
                        (System.Configuration.ConfigurationManager.
                            AppSettings["EnableSSL"].
                            ToLower().
                            Replace(" ", ""));
                }
                catch
                {
                    return true;
                }
            }
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            if (EnableSSL)
            {
                //ensure only https navigation
                if (!Context.Request.IsSecureConnection)
                    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"), true);
            }
            FirstRequestInitialization.Initialize(((HttpApplication)source).Context);
        }

        #region Enable web api session read

        private const string _WebApiPrefix = "api";
        private static string _WebApiExecutionPath = String.Format("~/{0}", _WebApiPrefix);

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            }
        }
        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(_WebApiExecutionPath);
        }

        #endregion
    }

    #region RequestInitClass

    class FirstRequestInitialization
    {
        private static bool InitializedAlready = false;
        private static Object LockObj = new Object();

        public static void Initialize(HttpContext context)
        {
            // Eval initialize web site
            if (InitializedAlready)
            {
                return;
            }
            else
            {
                //Initialize only on the first request
                //lock for multithreading app
                lock (LockObj)
                {
                    //set first request executed
                    InitializedAlready = true;

                    //init register standar site objects
                    AreaRegistration.RegisterAllAreas(MarketPlace.Models.General.AreaModel.CurrentAreaName);
                    GlobalConfiguration.Configure(WebApiConfig.Register);
                    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                    RouteConfig.RegisterRoutes(RouteTable.Routes);
                    BundleConfig.RegisterBundles(BundleTable.Bundles);
                }
            }
        }
    }

    #endregion

}
