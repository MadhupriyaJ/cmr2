using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;


namespace FramworkMain
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            

            bundles.Add(new StyleBundle("~/assets/css").Include(
                "~/assets/global/font/font.css",
                "~/assets/global/plugins/font-awesome/css/font-awesome.min.css",
                "~/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                "~/assets/global/plugins/bootstrap/css/bootstrap.min.css",
                "~/assets/global/plugins/uniform/css/uniform.default.css",
                "~/assets/global/plugins/select2/select2.css",
                "~/assets/admin/pages/css/login-soft.css",
                "~/assets/global/css/components-rounded.css",
                "~/assets/admin/layout3/css/themes/default.css",
                "~/assets/admin/layout3/css/custom.css",
                "~/assets/admin/pages/css/lock.css"
                /***************************************************************************/
                /*************************** Add extra Css *********************************/
                /***************************************************************************/
            ));

            bundles.Add(new StyleBundle("~/assets/layout").Include(
                  "~/assets/admin/layout3/css/layout.css"
                ));

            bundles.Add(new StyleBundle("~/assets/rlayout").Include(
                  "~/assets/admin/layout3/css/layout-rtl.css"
                ));

          
            bundles.Add(new ScriptBundle("~/assets/js").Include(
                     "~/assets/global/plugins/jquery.min.js",
                     "~/assets/global/plugins/jquery-migrate.min.js",
                     "~/assets/global/plugins/jquery-ui/jquery-ui.min.js",
                     "~/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                     "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                     "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                     "~/assets/global/plugins/jquery.blockui.min.js",
                     "~/assets/global/plugins/jquery.cokie.min.js",
                     "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js",
                     "~/assets/global/plugins/backstretch/jquery.backstretch.min.js",
                     "~/assets/global/plugins/select2/select2.min.js",
                     "~/assets/global/plugins/uniform/jquery.uniform.min.js",
                     "~/assets/global/scripts/metronic.js",
                     "~/assets/admin/layout3/scripts/layout.js",
                     "~/assets/admin/layout3/scripts/custom.js",
                     "~/assets/admin/layout3/scripts/demo.js",
                     "~/assets/global/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
                /***************************************************************************/
                /*************************** Add extra Js *********************************/
                /***************************************************************************/
                     ).ForceOrdered());

        }
    }
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }


    internal static class BundleExtensions
    {
        public static Bundle ForceOrdered(this Bundle sb)
        {
            sb.Orderer = new AsIsBundleOrderer();
            return sb;
        }
    }
}