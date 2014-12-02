using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
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

        #region Session methods

        public void LogOut()
        {
            SessionManager.SessionController.Logout();
            Response.Redirect(Url.Action(MVC.Home.ActionNames.Index, MVC.Home.Name));
        }

        #endregion

        #region generic file actions

        public virtual FileResult GetPdfFileBytes(string FilePath)
        {
            byte[] bytes = new byte[] { };
            if (!string.IsNullOrEmpty(FilePath) && FilePath.IndexOf(".pdf") > 0)
            {
                bytes = (new System.Net.WebClient()).DownloadData(FilePath);
            }
            return File(bytes, "application/pdf");
        }

        #endregion
    }
}