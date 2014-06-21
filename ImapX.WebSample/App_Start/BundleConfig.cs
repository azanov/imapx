using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Transformers;

namespace ImapX.WebSample
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            var css = new StyleBundle("~/Content/css").Include(
                "~/Content/less/base.less");

            css.Transforms.Add(new CssTransformer());

            bundles.Add(css);

            bundles.Add(new ScriptBundle("~/js/forms").Include("~/Scripts/jquery.form.min.js", "~/Scripts/jquery.tr.form.js"));

            bundles.Add(new ScriptBundle("~/js/yepnope").Include("~/Scripts/yepnope.{version}.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}