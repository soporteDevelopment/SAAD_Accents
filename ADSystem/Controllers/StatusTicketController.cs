using ADEntities.Queries;
using ADSystem.Common;
using ADSystem.Helpers;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    public class StatusTicketController : Controller
    {
        //public StatusTicket statusticket = new StatusTicket();
        public IStatusTicketManager manager;
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public StatusTicketController(IStatusTicketManager _manager)
        {
            manager = _manager;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = manager.Get();

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Status = result
                };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new
                {
                    Error = ex.Message
                };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        //GET: StatusTicket
        //public ActionResult Get(int idStatusTicket, string Name)
        //{
        //    JsonMessenger jmResult = new JsonMessenger();

        //    try
        //    {
        //        var result = StatusTicket.Get(idStatusTicket, Name);
        //        jmResult.success = 1;
        //        jmResult.failure = 0;
        //        jmResult.oData = new
        //        {
        //            total = result.Item1,
        //            statusticket = result.Item2,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        jmResult.success = 0;
        //        jmResult.failure = 1;
        //        jmResult.oData = new
        //        {
        //            Error = ex.Message
        //        };
        //    }
        //    return Json(jmResult, JsonRequestBehavior.AllowGet);
        //}


    }
}