using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProveedoresOnLine.AsociateProvider.Web.Controllers.Filters
{
    public class LogginActionFilter : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IActionFilter
    {
        public void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            if ((filterContext.RouteData.Values["controller"] + "_" + filterContext.RouteData.Values["action"]).ToLower() != "home_index" &&
                (!ProveedoresOnLine.AsociateProvider.Interfaces.Models.General.SessionModel.UserIsLoggedIn ||
                !ProveedoresOnLine.AsociateProvider.Interfaces.Models.General.SessionModel.UserIsAutorized))
            {
                filterContext.HttpContext.Response.Redirect("~/", true);
                filterContext.HttpContext.Response.End();
            }
        }
        public void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
        }
    }
}