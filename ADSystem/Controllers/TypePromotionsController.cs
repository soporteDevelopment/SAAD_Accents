using ADEntities.Queries;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class TypePromotionsController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var typePromotions = new TypePromotions();

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { TypePromotions = typePromotions.Get() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }
    }
}