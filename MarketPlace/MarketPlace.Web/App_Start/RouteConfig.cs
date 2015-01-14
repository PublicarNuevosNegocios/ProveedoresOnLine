using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MarketPlace.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            #region Route value dictionary by area
            RouteValueDictionary rvdByArea = new RouteValueDictionary();
            rvdByArea.Add("UseNamespaceFallback", false);
            rvdByArea.Add("area", MarketPlace.Models.General.AreaModel.CurrentAreaName);
            rvdByArea.Add("Namespaces", new string[] { "MarketPlace.Web.Areas." + MarketPlace.Models.General.AreaModel.CurrentAreaName + ".*" });
            #endregion

            //ignore route
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //default route
            routes.MapRoute(
                name: MarketPlace.Models.General.Constants.C_Routes_Default,
                url: "{controller}/{action}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            ).DataTokens = rvdByArea;
        }
    }
}
