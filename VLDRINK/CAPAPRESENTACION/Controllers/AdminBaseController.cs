using System.Web.Mvc;
using System.Web.Routing;

namespace CAPAPRESENTACION.Controllers
{
    // Este controlador hereda de 'BaseController'

    public class AdminBaseController : BaseController
    {
        // Este método se ejecuta ANTES que cualquier acción del 'MantenedorController'
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Primero, ejecuta la lógica del padre (revisar si está logueado)
            base.OnActionExecuting(filterContext);

            // Si la lógica del padre ya decidió redirigir (ej. al Login),
            // no hacemos nada más.
            if (filterContext.Result != null)
                return;

            // --- ESTA ES LA NUEVA REGLA ---
            // Revisamos si el usuario de la sesión NO es Admin
            if (Session["UserEsAdmin"] == null || (bool)Session["UserEsAdmin"] == false)
            {
                // Si no es Admin, lo "echamos" al Home (Dashboard)
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Index" }
                    }
                );
            }
        }
    }
}