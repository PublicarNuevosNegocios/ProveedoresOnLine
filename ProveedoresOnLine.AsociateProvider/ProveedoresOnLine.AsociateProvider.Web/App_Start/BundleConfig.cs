using System.Web;
using System.Web.Optimization;

namespace ProveedoresOnLine.AsociateProvider.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts

            #region JQuery

            bundles.Add(new ScriptBundle("~/site/scripts/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/site/scripts/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            #endregion

            #region modernizr

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/site/scripts/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            #endregion

            #region bootstrap

            bundles.Add(new ScriptBundle("~/site/scripts/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            #endregion

            #region site scripts

            bundles.Add(new ScriptBundle("~/site/scripts").IncludeDirectory(
                      "~/Scripts/Site",
                      "*.js",
                      true));

            #endregion

            #endregion

            #region Styles

            #region jquery

            bundles.Add(new StyleBundle("~/site/styles/jquery").Include("~/Content/themes/base/all.css"));

            #endregion

            #region bootstrap

            bundles.Add(new StyleBundle("~/site/styles/bootstrap").Include("~/Content/bootstrap.css"));

            #endregion
            
            #region site

            bundles.Add(new StyleBundle("~/site/styles").IncludeDirectory(
                      "~/Content/Styles/Site",
                      "*.css",
                      true));

            #endregion

            #endregion

            //allow bundles in debug mode
            bundles.IgnoreList.Clear();
        }
    }
}