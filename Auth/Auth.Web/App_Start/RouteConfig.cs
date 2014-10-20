using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Auth.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            #region Route value dictionary by area
            RouteValueDictionary rvdByArea = new RouteValueDictionary();
            rvdByArea.Add("UseNamespaceFallback", false);
            rvdByArea.Add("area", Auth.Web.Controllers.BaseController.AreaName);
            rvdByArea.Add("Namespaces", new string[] { "Auth.Web.Areas." + Auth.Web.Controllers.BaseController.AreaName + ".*" });
            #endregion

            //ignore route
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //custom routes

            //default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).DataTokens = rvdByArea;
        }
    }
}
