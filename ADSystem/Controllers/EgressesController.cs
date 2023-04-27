using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class EgressesController : BaseController
    {
        // GET: Input
        public override ActionResult Index()
        {
            Session["Controller"] = "Manejo de Efectivo";

            return View();
        }

        [HttpGet]
        public ActionResult ListActivesEgresses(string remission)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Egresses = tEgresses.GetActives(remission),
                    Total = tEgresses.GetActives(remission).Sum(p=>p.Cantidad) + tEgresses.GetActives(remission).Sum(p => p.InternalEgresses.Sum(q=> q.Cantidad))
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
            return PartialView("~/Views/Egresses/Detail.cshtml", new EgressViewModel());
        }

        public ActionResult AddInternal()
        {
            return PartialView("~/Views/Egresses/InternalDetail.cshtml", new EgressViewModel());
        }

        [HttpPost]
        public ActionResult SaveAdd(EgressViewModel entry)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.Egresses.ModelToTable(entry);

                result.Fecha = DateTime.Now;
                result.CreadoPor = (int)Session["_ID"];
                result.Creado = DateTime.Now;
                result.Estatus = 1;

                if (tEgresses.Add(result) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se agregó un registro al módulo {0}", "Manejo de Efectivo")
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

        public ActionResult EgressesById(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.oData = new
                {
                    Egress = tEgresses.GetById(id)
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

        public ActionResult Update(int id)
        {
            var detail = tEgresses.GetById(id);

            return PartialView("~/Views/Egresses/Detail.cshtml", detail);
        }

        [HttpPatch]
        public ActionResult SaveUpdate(EgressViewModel entry)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.Egresses.ModelToTable(entry);

                result.ModificadoPor = (int)Session["_ID"];
                result.Modificado = DateTime.Now;

                if (tEgresses.Update(result) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se agregó un registro al módulo {0}", "Manejo de Efectivo")
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

        [HttpPatch]
        public ActionResult Delete(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tEgresses.Delete(id, (int)Session["_ID"], DateTime.Now);

                jmResult.success = 1;
                jmResult.failure = 0;
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPatch]
        public ActionResult ReportEgresses(int idReport, List<int> egresses)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tEgresses.UpdateReport(idReport, egresses, (int)Session["_ID"], DateTime.Now);

                jmResult.success = 1;
                jmResult.failure = 0;
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