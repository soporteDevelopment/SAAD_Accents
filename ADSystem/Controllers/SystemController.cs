using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    [AuthorizedModule]
    public class SystemController : BaseController
    {

        // GET: System
        public override ActionResult Index()
        {

            Session["Controller"] = "Sistema";

            SessionStateSection ssTime = new SessionStateSection();

            List<KeyValuePair<string, string>> lSystemVar = new List<KeyValuePair<string, string>>();

            lSystemVar.Add(new KeyValuePair<string, string>("TimeOut", ssTime.Timeout.Minutes.ToString()));

            ViewData.Add("SystemVar", lSystemVar);

            return View();
        }

        [HttpPost]
        public ActionResult SaveChange(string host, string name, string password, int port, int timeOut)
        {
            JsonMessenger jmResult = new JsonMessenger();

            SessionStateSection ssTime = new SessionStateSection();

            ssTime.Timeout = new TimeSpan(0, timeOut, 0);

            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "System") };

            return Json(jmResult);
        }

    }
}
