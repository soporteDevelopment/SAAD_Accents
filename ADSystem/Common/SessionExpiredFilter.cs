using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ADSystem.Common
{
    public class SessionExpiredFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if (ctx.Session != null)
            {
                string sessionCookie = ctx.Request.Headers["Cookie"];
                if (ctx.Session.Contents["_User"] == null)
                {
                    string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;

                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        JsonResult jr = new JsonResult();
                        jr.Data = new
                        {
                            noLogin = 1,
                            success = 0,
                            failure = 0
                        };
                        filterContext.Result = jr;
                    }
                    else
                    {
                        if (redirectOnSuccess.EndsWith("Access/Close"))
                        {
                            redirectOnSuccess = UrlHelper.GenerateContentUrl("~/Home/Index", filterContext.HttpContext);
                        }
                        string loginUrl = FormsAuthentication.LoginUrl + string.Format("?ReturnUrl={0}", redirectOnSuccess);
                        filterContext.Result = new RedirectResult(loginUrl);
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

    }
}
