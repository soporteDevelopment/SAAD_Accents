using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace ADSystem
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            //BundleTable.EnableOptimizations = true;

            bundles.Add(new StyleBundle("~/Content/CSS/Access").Include(
                  "~/Content/CSS/normalize.css",
                  "~/Content/CSS/styles.css",
                  "~/Content/CSS/bootstrap.css",
                  "~/Content/CSS/font-awesome.css",
                  "~/Content/CSS/ngProgress.css",
                  "~/Content/CSS/toaster.css"));

            bundles.Add(new StyleBundle("~/Content/CSS/Home").Include(
                  "~/Content/CSS/normalize.css",
                  "~/Content/CSS/styles.css", 
                  "~/Content/CSS/bootstrap.css",
                  "~/Content/CSS/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/CSS/General").Include(
                  "~/Content/CSS/normalize.css",
                  "~/Content/CSS/styles.css",
                  "~/Content/CSS/bootstrap.css",
                  "~/Content/CSS/font-awesome.css",
                  "~/Content/CSS/ngProgress.css",
                  "~/Content/CSS/toaster.css",
                  "~/Scripts/acute.select.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Access").Include(
                   "~/Scripts/angular.js",
                   "~/Scripts/bootstrap.js",
                   "~/Scripts/ngProgress.js",
                   "~/Scripts/toaster.js",
                   "~/Scripts/Class/General.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Home").Include(
                   "~/Scripts/angular.js",
                   "~/Scripts/bootstrap.js",
                   "~/Scripts/angucomplete.js",                   
                   "~/Scripts/ngProgress.js",
                   "~/Scripts/acute.select.js",
                   "~/Scripts/toaster.js",
                   "~/Scripts/Class/General.js",
                   "~/Scripts/Class/Home.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/General").Include(
                   "~/Scripts/angular.js",
                   "~/Scripts/angular-locale_es-mx.js",
                   "~/Scripts/bootstrap.js",
                   "~/Scripts/angucomplete.js",
                   "~/Scripts/ngProgress.js",
                   "~/Scripts/toaster.js",
                   "~/Scripts/acute.select.js",
                   "~/Scripts/main.js",
                   "~/Scripts/Class/General.js",
                   "~/Scripts/Class/Home.js"
                ));

        }

    }
}
