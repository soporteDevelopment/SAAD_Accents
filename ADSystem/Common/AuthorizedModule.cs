using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADEntities.ViewModels;

namespace ADSystem.Common
{
    public class AuthorizedModule : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase httpContext = filterContext.HttpContext;
            if (System.Web.HttpContext.Current.Session != null)
            {

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    if (httpContext.Session.Contents["_User"] != null)
                    {
                        UserViewModel sUsuario = (UserViewModel)httpContext.Session["_User"];
                        string cController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                        string cAction = filterContext.ActionDescriptor.ActionName;
                        List<ActionViewModel> sAction = (List<ActionViewModel>)httpContext.Session["_Actions"];

                        if (sAction.Any(m => m.Control.ToUpper().Trim() == cController.ToUpper().Trim() && m.Accion.ToUpper().Trim() == cAction.ToUpper().Trim()))
                        {
                            var oModulo = sAction.FirstOrDefault(p => p.Control.ToUpper().Trim() == cController.ToUpper().Trim());
                        }
                        else
                        {
                            filterContext.Result = new RedirectResult("~/Home/Index");
                        }

                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
