using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADEntities.Queries;
using ADEntities.Converts;
using ADEntities.Models;
using ADEntities.ViewModels;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class RepairsController : BaseController
    {
        public ActionResult GeneratePrevNumber()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Number = tRepairs.GeneratePrevNumber() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        // GET: Repairs
        public override ActionResult Index()
        {
            Session["Controller"] = "Reparaciones";

            return View();
        }

        [HttpPost]
        public ActionResult ListRepairs(string dtDateSince, string dtDateUntil, string number, string code, int status,
            int? idUser, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Repairs = tRepairs.Get(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), number,
                        code, status, idUser, page, pageSize),
                    Count = tRepairs.CountRegisters()
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

        public ViewResult AddRepair()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddRepair(RepairViewModel repair)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.Repairs.ModelToTable(repair);

                if (tRepairs.Post(result) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se agregó un registro al módulo {0}", "Reparaciones")
                    };
                }

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public PartialViewResult UpdateRepair(int idRepair)
        {
            var detail = tRepairs.GetById(idRepair);

            return PartialView("~/Views/Repairs/UpdateRepair.cshtml", detail);
        }

        [HttpPatch]
        public ActionResult SaveUpdateRepair(RepairViewModel repair)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.Repairs.ModelToTable(repair);

                if (tRepairs.Patch(result) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se actualizó un registro en el módulo {0}", "Reparaciones")
                    };
                }
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult RepairById(int idRepair)
        {
            var detail = tRepairs.GetById(idRepair);

            return PartialView("~/Views/Repairs/Detail.cshtml", detail);
        }

        [HttpPost]
        public ActionResult SaveHistoryRepair(int idRepair, List<tHistoricoReparacione> lhistory)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tRepairs.UpdateReceivedAmount(idRepair, lhistory, (int)Session["_ID"]);
                
                jmResult.success = 1;
                jmResult.oData = new
                {
                    Message = "Se actualizó el detalle de la reparación"
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