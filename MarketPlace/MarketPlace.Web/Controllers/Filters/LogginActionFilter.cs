using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlace.Web.Controllers.Filters
{
    public class LogginActionFilter : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IActionFilter
    {
        public void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            if ((filterContext.RouteData.Values["controller"] + "_" + filterContext.RouteData.Values["action"]).ToLower() != "home_index" &&
                (!MarketPlace.Models.General.SessionModel.UserIsLoggedIn ||
                !MarketPlace.Models.General.SessionModel.IsUserAuthorized()))
            {
                filterContext.HttpContext.Response.RedirectToRoute
                    (MarketPlace.Models.General.Constants.C_Routes_Default,
                    new
                    {
                        controller = MVC.Home.Name,
                        action = MVC.Home.ActionNames.Index
                    });

                MarketPlace.Models.General.SessionModel.CurrentURL = HttpContext.Current.Request.Url.AbsoluteUri;
                filterContext.HttpContext.Response.End();
            }            
        }
        public void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {           
        }
    }
}