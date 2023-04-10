using ADSystem.Helpers;
using System;
using System.Web.Mvc;
using ADEntities.ViewModels;
using ADSystem.Common;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class CashReportController : BaseController
    {
        // GET: Input
        public override ActionResult Index()
        {
            Session["Controller"] = "Reporte de Caja";

            return View();
        }

        [HttpGet]
        public ActionResult Get(string dtDateSince, string dtDateUntil, string remission)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Reports = tReportEntries.Get(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), remission)
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

        public ActionResult Add()
        {
            return PartialView("~/Views/CashReport/Add.cshtml");
        }

        [HttpPost]
        public ActionResult SaveAdd(ReportEntriesViewModel report)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.ReportEntries.ModelToTable(report);

                result.Fecha = DateTime.Now;
                result.CreadoPor = (int)Session["_ID"];
                result.Creado = DateTime.Now;
                result.Revisado = false;
                
                jmResult.success = 1;
                jmResult.oData = new
                {
                    idReportEntries = tReportEntries.Add(result),
                    Message = String.Format("Se agregó un registro al módulo {0}", "Manejo de Efectivo")
                };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetDetail(int id)
        {
            var detail = tReportEntries.GetById(id);

            return PartialView("~/Views/CashReport/Detail.cshtml", detail);
        }

        public PartialViewResult Print(int id)
        {
            var detail = tReportEntries.GetById(id);

            return PartialView(detail);
        }

        [HttpPatch]
        public ActionResult UpdateStatus(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var reviewed = tReportEntries.UpdateStatus(id);

                if (reviewed)
                {
                    var entries = tEntries.GetByReportId(id);

                    foreach (var entry in entries)
                    {
                        tSales.SetStatusPaymentByEntry(entry.idEntrada);
                    }
                }

                jmResult.success = 1;
                jmResult.oData = new
                {
                    Reviewed = reviewed,
                    Message = String.Format("Se valida revisión {0}", "Manejo de Efectivo")
                };
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