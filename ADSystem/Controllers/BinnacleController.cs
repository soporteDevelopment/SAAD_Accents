using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class BinnacleController : BaseController
    {
        public override ActionResult Index()
        {
            Session["Controller"] = "Bitácora";

            return View();
        }

        [HttpPost]
        public ActionResult ListRecords(string code, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Records = tBinnacle.GetBinnacle(code, page, pageSize), Count = tBinnacle.CountRegisters(code) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }
    }
}