using System.Web;
using System.Web.Optimization;

namespace TaskTimer
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* JS */

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            /* JS admin */

            /* table sorter */
            bundles.Add(new ScriptBundle("~/Scripts/tablesorter/js").Include(
                        "~/Scripts/tablesorter/js/jquery.tablesorter.js",
                        "~/Scripts/tablesorter/js/jquery.tablesorter.widgets.js",
                        "~/Scripts/tablesorter/addons/pager/jquery.tablesorter.pager.js"));
            bundles.Add(new StyleBundle("~/Scripts/tablesorter/css").Include(
                       "~/Scripts/tablesorter/css/theme.bootstrap.css",
                       "~/Scripts/tablesorter/addons/pager/jquery.tablesorter.pager.css",
                       "~/Scripts/tablesorter/css/bootstrap.css"));

            
            /* stiles */

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));



            /* styles admin */

            bundles.Add(new StyleBundle("~/Areas/Admin/Content/css").Include(
                "~/Areas/Admin/Content/admin.css",
                "~/Areas/Admin/Content/sitePages.css"));
        }
    }
}