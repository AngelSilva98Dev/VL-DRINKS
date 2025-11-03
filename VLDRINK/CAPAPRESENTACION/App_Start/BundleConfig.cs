using System.Web;
using System.Web.Optimization;

namespace CAPAPRESENTACION
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                                    "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/complementos").Include(
                                    "~/Scripts/fontawesome/all.min.js",
                                    "~/Scripts/scripts.js"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                                    "~/Scripts/bootstrap.bundle.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                                    "~/Content/bootstrap.css",
                                    "~/Content/site.css"));



            bundles.Add(new Bundle("~/bundles/datatables").Include(
                                    "~/Scripts/dataTables.min.js", 
                                    "~/Scripts/dataTables.bootstrap5.min.js", 
                                    "~/Scripts/dataTables.select.min.js")); 

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                                    "~/Content/dataTables.bootstrap5.min.css", 
                                    "~/Content/select.bootstrap5.min.css")); 
        }
    }
}