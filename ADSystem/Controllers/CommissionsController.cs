using ADSystem.Common;
using ADSystem.Helpers;
using System.Web.Mvc;
using ADEntities.Queries;
using System;
using ADEntities.ViewModels;
using ADEntities.Converts;
using CommissionsConvert = ADEntities.Converts.Commissions;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class CommissionsController : Controller
    {
        private ADEntities.Queries.Commissions tCommissions = new ADEntities.Queries.Commissions();

        // GET: Actions
        public ActionResult Index(int idUser)
        {
            Session["Controller"] = "Comisiones";

            ViewBag.idUser = idUser;

            return View("~/Views/Users/Commission.cshtml");
        }

        public ActionResult Get(int idUser)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Commissions = tCommissions.GetCommissions(idUser)
                };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(CommissionViewModel commission)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var entity = CommissionsConvert.ModelToTable(commission);

                tCommissions.Add(entity);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se agregó comisión" };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPatch]
        public ActionResult Update(CommissionViewModel commission)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var entity = CommissionsConvert.ModelToTable(commission);

                tCommissions.Update(entity);

                jmResult.success = 1;
                jmResult.failure = 0;
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult Delete(int idCommission)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tCommissions.Remove(idCommission);

                jmResult.success = 1;
                jmResult.failure = 0;
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