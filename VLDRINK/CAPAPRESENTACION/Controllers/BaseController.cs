using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CAPAPRESENTACION.Controllers
{

    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (User.Identity.IsAuthenticated && Session["UserCorreo"] == null)
            {

                FormsAuthentication.SignOut();


                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    }
                );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}