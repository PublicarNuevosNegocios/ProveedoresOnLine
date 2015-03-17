using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackOffice.Web.Controllers.Filters
{
    public class LogginActionFilter : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IActionFilter
    {
        public void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            if ((filterContext.RouteData.Values["controller"] + "_" + filterContext.RouteData.Values["action"]).ToLower() != "home_index" &&
                (!BackOffice.Models.General.SessionModel.UserIsLoggedIn ||
                !BackOffice.Models.General.SessionModel.UserIsAutorized))
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