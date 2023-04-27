using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using System.Globalization;
using System.Text;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class EntriesController : BaseController
    {
        // GET: Input
        public override ActionResult Index()
        {
            Session["Controller"] = "Manejo de Efectivo";

            return View();
        }

        [HttpGet]
        public ActionResult ListEntries(string dtDateSince, string dtDateUntil, string deliveredBy)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Entries = tEntries.Get(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), deliveredBy)
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

        [HttpGet]
        public ActionResult ListActivesEntries(string remission)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Entries = tEntries.GetActives(remission),
                    Total = tEntries.GetActives(remission).Sum(p => p.Cantidad)
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
            return PartialView("~/Views/Entries/Detail.cshtml", new EntryViewModel());
        }

        [HttpPost]
        public ActionResult SaveAdd(EntryViewModel entry)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.Entries.ModelToTable(entry);

                result.Fecha = DateTime.Now;
                result.CreadoPor = (int)Session["_ID"];
                result.Creado = DateTime.Now;
                result.Estatus = 1;

                int id = tEntries.Add(result);

                if (id > 0)
                {
                    if (entry.Payments != null)
                    {
                        tSales.UpdatePaymentEntry(id, entry.Payments);
                    }

                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se agregó un registro al módulo {0}", "Manejo de Efectivo")
                    };

                    //Si es entregado por Anna se envía correo
                    if (entry.EntregadaPor == 16)
                    {
                        StringBuilder body = new StringBuilder();
                        body.Append("Se recibío de Anna la cantidad de <b>");
                        body.Append(entry.Cantidad.Value.ToString("C", CultureInfo.CurrentCulture));
                        body.Append("</b><br/>");
                        body.Append("Comentarios: <b>" + entry.Comentarios);
                        body.Append("</b><br/>");

                        var emailService = new Email();

                        emailService.SendMail("fernanda@accentsdecoration.com", "INGRESO", body.ToString());
                    }else if (entry.EntregadaPor > 0)
                    {  
                        StringBuilder body = new StringBuilder();
                        body.Append("Se recibío la cantidad de <b>");
                        body.Append(entry.Cantidad.Value.ToString("C", CultureInfo.CurrentCulture));
                        body.Append("</b><br/>");
                        if (entry.idVenta > 0)
                        {
                            var sale = tSales.GetSaleForIdSale(entry.idVenta ?? 0);
                            body.Append("Venta: <b>" + sale.Remision);
                            body.Append("</b>");
                            body.Append("Vendedor: <b>" + sale.Usuario1);
                            body.Append("</b><br/>");
                        }
                        body.Append("Comentarios: <b>" + entry.Comentarios);
                        body.Append("</b><br/>");

                        var emailService = new Email();

                        emailService.SendMail("fernanda@accentsdecoration.com", "INGRESO", body.ToString());
                    }
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

        public ActionResult EntryById(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.oData = new
                {
                    Entry = tEntries.GetById(id)
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
            var detail = tEntries.GetById(id);

            return PartialView("~/Views/Entries/Detail.cshtml", detail);
        }

        [HttpPatch]
        public ActionResult SaveUpdate(EntryViewModel entry)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.Entries.ModelToTable(entry);

                result.ModificadoPor = (int)Session["_ID"];
                result.Modificado = DateTime.Now;

                if (tEntries.Update(result) > 0)
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
                tEntries.Delete(id, (int)Session["_ID"], DateTime.Now);

                var payments = tSales.GetPaymentByEntry(id);

                if (payments != null)
                {
                    tSales.DeletePaymentEntry(id, payments);
                }

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
        public ActionResult ReportEntries(int idReport, List<int> entries)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tEntries.UpdateReport(idReport, entries, (int)Session["_ID"], DateTime.Now);

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