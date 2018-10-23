using System.Web.Optimization;

namespace FSBO.MainSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/bootstrap-overrides.css",
                 "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery/jquery-{version}.js",
                "~/Scripts/bootstrap/bootstrap.js",
                "~/Scripts/knockoutjs/knockout-{version}.js",
                "~/Scripts/knockoutjs/pager.js").IncludeDirectory(
                "~/Scripts/site", "*.js", true).Include(
                "~/Scripts/siteStart.js"));
        }
    }
}
