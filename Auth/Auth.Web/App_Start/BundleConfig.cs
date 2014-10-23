using System.Web;
using System.Web.Optimization;

namespace Auth.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            if (Auth.Web.Controllers.BaseController.AreaName == Auth.Interfaces.Constants.C_WebAreaName)
            {
                //bundle for web scripts

                #region JQery
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/jquery").Include(
                            "~/Areas/Web/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/jqueryval").Include(
                            "~/Areas/Web/Scripts/jquery.validate*"));
                #endregion

                #region Modernizr
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/modernizr").Include(
                            "~/Areas/Web/Scripts/modernizr-*"));
                #endregion

                #region Bootstrap
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/bootstrap").Include(
                          "~/Areas/Web/Scripts/bootstrap.js",
                          "~/Areas/Web/Scripts/respond.js"));
                #endregion

                #region SiteScripts
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/sitescripts").IncludeDirectory(
                          "~/Areas/Web/Scripts/Site",
                          "*.js",
                          true));
                #endregion

                #region Kendo

                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/kendo").Include(
                             "~/Areas/Web/Scripts/kendo/2014.1.318/kendo.web.min.js"));

                bundles.Add(new StyleBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/content/kendo/css").Include(
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.common.min.css",
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.default.min.css"));

                #endregion

                #region Styles
                bundles.Add(new StyleBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/content/css").IncludeDirectory(
                          "~/Areas/Web/Content/Styles",
                          "*.css",
                          true));
                #endregion
            }
            else if (Auth.Web.Controllers.BaseController.AreaName == Auth.Interfaces.Constants.C_MobileAreaName)
            {
                //bundle for mobile scirpts

                #region JQery
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/jquery").Include(
                            "~/Areas/Web/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/jqueryval").Include(
                            "~/Areas/Web/Scripts/jquery.validate*"));
                #endregion

                #region Modernizr
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/modernizr").Include(
                            "~/Areas/Web/Scripts/modernizr-*"));
                #endregion

                #region Bootstrap
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/bootstrap").Include(
                          "~/Areas/Web/Scripts/bootstrap.js",
                          "~/Areas/Web/Scripts/respond.js"));
                #endregion

                #region SiteScripts
                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/sitescripts").IncludeDirectory(
                          "~/Areas/Web/Scripts/Site",
                          "*.js",
                          true));
                #endregion

                #region Kendo

                bundles.Add(new ScriptBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/bundles/kendo").Include(
                             "~/Areas/Web/Scripts/kendo/2014.1.318/kendo.web.min.js"));

                bundles.Add(new StyleBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/content/kendo/css").Include(
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.common.min.css",
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.default.min.css"));

                #endregion

                #region Styles
                bundles.Add(new StyleBundle("~/" + Auth.Web.Controllers.BaseController.AreaName + "/content/css").IncludeDirectory(
                          "~/Areas/Web/Content/Styles",
                          "*.css",
                          true));
                #endregion
            }

        }
    }
}
