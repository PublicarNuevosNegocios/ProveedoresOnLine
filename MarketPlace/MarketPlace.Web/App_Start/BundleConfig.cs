using System.Web;
using System.Web.Optimization;

namespace MarketPlace.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            switch (MarketPlace.Models.General.AreaModel.CurrentArea)
            {
                case Models.General.enumSiteArea.Desktop:

                    #region Scripts

                    #region JQuery

                    bundles.Add(new ScriptBundle("~/site/scripts/jquery").Include(
                                "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/jquery-{version}.js",
                                "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/jquery-ui-{version}.js"));

                    bundles.Add(new ScriptBundle("~/site/scripts/jqueryval").Include(
                                "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/jquery.validate*"));

                    #endregion

                    #region modernizr

                    // Use the development version of Modernizr to develop with and learn from. Then, when you're
                    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
                    bundles.Add(new ScriptBundle("~/site/scripts/modernizr").Include(
                                "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/modernizr-*"));

                    #endregion

                    #region bootstrap

                    bundles.Add(new ScriptBundle("~/site/scripts/bootstrap").Include(
                              "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/bootstrap.js",
                              "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/respond.js"));

                    #endregion

                    #region Kendo

                    bundles.Add(new ScriptBundle("~/site/scripts/kendo").Include(
                                 "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/kendo/2014.1.318/kendo.web.min.js"));

                    #endregion

                    #region site scripts

                    bundles.Add(new ScriptBundle("~/site/scripts").IncludeDirectory(
                              "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Scripts/Site",
                              "*.js",
                              true));

                    #endregion

                    #endregion

                    #region Styles

                    #region jquery

                    bundles.Add(new StyleBundle("~/site/styles/jquery").
                        Include("~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Content/themes/base/all.css"));

                    #endregion

                    #region bootstrap

                    bundles.Add(new StyleBundle("~/site/styles/bootstrap").
                        Include("~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Content/bootstrap.css"));

                    #endregion

                    #region kendo

                    bundles.Add(new StyleBundle("~/site/styles/kendo").Include(
                              "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Content/kendo/2014.1.318/kendo.common.min.css",
                              "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Content/kendo/2014.1.318/kendo.default.min.css"));

                    #endregion

                    #region site

                    bundles.Add(new StyleBundle("~/site/styles").IncludeDirectory(
                              "~/Areas/" + MarketPlace.Models.General.AreaModel.CurrentAreaName + "/Content/Styles/Site",
                              "*.css",
                              true));

                    #endregion

                    #endregion

                    break;

                default:
                    break;
            }

            //allow bundles in debug mode
            bundles.IgnoreList.Clear();
        }
    }
}
