﻿using System.Web;
using System.Web.Optimization;

namespace AddressBook
{
    public class BundleConfig
    {        
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            AddScripts(bundles);

            AddStylesheets(bundles);
        }

        private static void AddScripts(BundleCollection bundles)
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

            bundles.Add(new ScriptBundle("~/bundles/third-party-plugins").Include(
                       "~/Scripts/Libraries/mustache.js",
                       "~/Scripts/Libraries/moment.js",
                       "~/Scripts/Libraries/daterangepicker.js",
                       "~/Scripts/Libraries/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalScript").Include(
                    "~/Scripts/Web/global.js"));            
        }

        private static void AddStylesheets(BundleCollection bundles)
        {
            // Must be included in this order
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/Libraries/css/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-extensions").Include(
                "~/Content/Libraries/css/bootstrap-*"));

            bundles.Add(new StyleBundle("~/Content/datatables-bootstrap").Include(
                "~/Content/Libraries/css/dataTables*"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                "~/Content/Libraries/css/font-awesome.css", new CssRewriteUrlTransform()));

            // SB-admin template stylesheets
            bundles.Add(new StyleBundle("~/Content/sb-admin").Include(
                "~/Content/Libraries/css/sb-admin*"));

            bundles.Add(new StyleBundle("~/Content/third-party-stylesheets").Include(
               "~/Content/Libraries/css/daterangepicker.css",
               "~/Content/Libraries/css/toastr.css"));

            // App stylesheets
            bundles.Add(new StyleBundle("~/Content/app-stylesheets").Include(
                "~/Content/Libraries/css/global.css"));
        }
    }
}
