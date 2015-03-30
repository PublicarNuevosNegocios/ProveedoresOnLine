using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProveedoresOnLine.AsociateProvider.Web.Controllers
{
    [ProveedoresOnLine.AsociateProvider.Web.Controllers.Filters.LogginActionFilter]
    public partial class BaseController : Controller
    {
        #region public static properties

        public static string CurrentControllerName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public static string CurrentActionName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }

        #endregion
    }
}
