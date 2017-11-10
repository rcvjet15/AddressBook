using System.Web;
using System.Web.Optimization;

namespace AddressBook
{
    public class BundleConfig
    {        
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Must be included in this order
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/Libraries/jquery-{version}.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Libraries/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/Libraries/jquery-ui-{version}.js",
                        "~/Scripts/Libraries/jquery-easing.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/Libraries/bootstrap.bundle.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-datables").Include(
                       "~/Scripts/Libraries/jquery.dataTables.js",
                       "~/Scripts/Libraries/dataTables.bootstrap4.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Libraries/modernizr-*"));

            // SB-admin template scripts
            bundles.Add(new ScriptBundle("~/bundles/sb-admin").Include(
                        "~/Scripts/Libraries/sb-admin*"));
            
            bundles.Add(new ScriptBundle("~/bundles/globalScript").Include(
                    "~/Scripts/global.js"));

            // Include every js script that is used for each view
            bundles.Add(new ScriptBundle("~/bundles/appScripts")
                .IncludeDirectory("~/Scripts/Web", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css")
                .IncludeDirectory("~/Content", "*.css", true));
        }
    }
}
