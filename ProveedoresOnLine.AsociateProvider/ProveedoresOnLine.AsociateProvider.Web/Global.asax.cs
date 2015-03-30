using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProveedoresOnLine.AsociateProvider.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

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

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (EnableSSL)
            {
                //ensure only https navigation
                if (!Context.Request.IsSecureConnection)
                    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"), true);
            }
        }
    }
}