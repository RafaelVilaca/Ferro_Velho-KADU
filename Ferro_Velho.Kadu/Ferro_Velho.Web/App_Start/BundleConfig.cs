using System.Web;
using System.Web.Optimization;

namespace Ferro_Velho.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymask").Include(
             "~/Scripts/jquery-mask.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryprice").Include(
              "~/Scripts/jquery.price_format.js",
              "~/Scripts/jquery.price_format.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymaskmoney").Include(
             "~/Scripts/jquery-maskmoney.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymaskpeso").Include(
             "~/Scripts/jquery-maskpeso.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryprice").Include(
              "~/Scripts/jquery.price_format.js",
              "~/Scripts/jquery.price_format.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/util").Include(
             "~/Scripts/util.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
