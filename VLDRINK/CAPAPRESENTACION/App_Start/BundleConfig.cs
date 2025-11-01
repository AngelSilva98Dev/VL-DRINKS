using System.Web;
using System.Web.Optimization;

namespace CAPAPRESENTACION
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                                    "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/complementos").Include("~/Scripts/fontawesome/all.min.js",
                                   "~/Scripts/scripts.js"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                                    "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));


            // --- ⬇️ CORREGIDO ⬇️ ---
            // Las rutas ahora apuntan a ~/Scripts/ en lugar de ~/Scripts/DataTables/
            // y se ha corregido el nombre de 'jquery.dataTables.min.js' a 'dataTables.min.js'

            // Bundle para el JavaScript de DataTables
            bundles.Add(new Bundle("~/bundles/datatables").Include(
                                    "~/Scripts/dataTables.min.js", // Corregido
                                    "~/Scripts/dataTables.bootstrap5.min.js")); // Corregido

            // Bundle para el CSS de DataTables
            // Asumiendo que el CSS está en Content/dataTables.bootstrap5.min.css
            // (Si no, muéstrame tu carpeta Content)
            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                                    "~/Content/dataTables.bootstrap5.min.css"));
        }
    }
}