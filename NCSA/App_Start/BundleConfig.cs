using System.Web;
using System.Web.Optimization;

namespace NCSA
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        //"~/Scripts/umd/popper.js",
                        "~/Scripts/site.js",
                        "~/Scripts/ContactMessage.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/admin.js"
                        //"~/Scripts/bootstrap.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/dropdown").Include(
                     "~/Scripts/dropdown.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/SASS/main.css",
                      "~/Content/fontawesome-all.css",
                      //"~/Content/bootstrap.css",
                      "~/Content/hint.css"));
        }
    }
}
