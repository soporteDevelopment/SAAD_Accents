using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADSystem.Helpers;
using ADEntities.Queries;
using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System.IO;
using System.Drawing;
using ADSystem.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class IntelligenceController : BaseController
    {
        public override ActionResult Index()
        {
            Session["Controller"] = "Reportes";


            return RedirectToAction("Sales", "INTELLIGENCE");
        }

        public ActionResult Sales()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSalesReport(string dateSince, string dateUntil, int? seller, string remission,  short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tReports.GetSalesReport(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), seller, remission, amazonas, guadalquivir, textura, page, pageSize), Count = tReports.CountRegistersWithFilters(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), seller, remission, amazonas, guadalquivir, textura, page, pageSize) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };


            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult AddCommission(tReporteVentasComisione oCommission, List<int> sales)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idCommissionSale = tReports.AddCommission(oCommission);

                if (idCommissionSale > 0)
                {

                    if (sales != null)
                    {

                        tSales.UpdatePaidOutSales(idCommissionSale, oCommission.idVendedor, sales);

                    }

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "Se agregó el registro." };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "No se guardo el registro." };
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

        [HttpPost]
        public ActionResult GetCommissionsForSeller(int idSeller, decimal total)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Commissions = tReports.GetCommission(idSeller, total) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult GetCommissions(string date, int idSeller)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Commissions = tReports.GetCommissions(Convert.ToDateTime(date), idSeller), CommissionPayment = tReports.GetCommissions(Convert.ToDateTime(date), idSeller).Sum(p=>p.Cantidad), Bonos = tReports.GetBono(Convert.ToDateTime(date), idSeller) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult DetailSaleReport(string remision)
        {
            SaleViewModel oSale = tSales.GetSaleForRemision(remision);

            return PartialView("~/Views/Intelligence/DetailSaleReport.cshtml", oSale);
        }

        public ActionResult UpdateReportVars(int idSale, int? porcComisionVendedorUno, decimal? cantComisionVendedorUno, string fechaPago1, int? formaPago1, int? porcComisionVendedorDos, decimal? cantComisionVendedorDos, string fechaPago2, int? formaPago2, int? porcComisionDespacho, decimal? cantComisionDespacho, string fechaPago3, int? formaPago3)
        {
            JsonMessenger jmResult = new JsonMessenger();
            DateTime? dtfechaPago1 = null;
            DateTime? dtfechaPago2 = null;
            DateTime? dtfechaPago3 = null;

            try
            {

                tReporteVentasComisione oReports = new tReporteVentasComisione();

                if (!string.IsNullOrWhiteSpace(fechaPago1))
                    dtfechaPago1 = Convert.ToDateTime(fechaPago1);

                if (!string.IsNullOrWhiteSpace(fechaPago2))
                    dtfechaPago2 = Convert.ToDateTime(fechaPago2);

                if (!string.IsNullOrWhiteSpace(fechaPago3))
                    dtfechaPago3 = Convert.ToDateTime(fechaPago3);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Reportes") };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public void ExportReport(int idCommissionSale)
        {
            var sales = tReports.GetSalesReportCSV(idCommissionSale);

            var csv = new StringBuilder();

            csv.AppendLine("FECHA,HORA,VENDEDOR 1,VENDEDOR 2,TIPO DE CLIENTE,CLIENTE,DESPACHO,SUCURSAL,REMISION,SUBTOTAL,DESCUENTO,TOTAL");

            string fileName = "Comisiones";

            var nFI = new NumberFormatInfo();

            nFI.NumberDecimalDigits = 2;

            foreach (var sale in sales)
            {

                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", sale.Fecha.Value.ToString("dd/MM/yyyy"), sale.Hora.Value.ToShortTimeString(), sale.Usuario1, sale.Usuario2, sale.TipoCliente, sale.Cliente, sale.Despacho1, sale.Sucursal, sale.Remision, ((decimal) sale.ImporteVenta).ToString("N", nFI).Replace(",", ""), ((decimal)sale.Descuento).ToString("N", nFI).Replace(",", ""), ((decimal)sale.ImportePagadoCliente).ToString("N", nFI).Replace(",", ""));
                
                fileName = "Comisiones-" + sale.Fecha.Value.ToString("MM/yyyy") + ".csv";

            }

            //Obtener comisión
            var commission = tReports.GetCommission(idCommissionSale);

            //Total Neto Vendido
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", "", "", "", "", "", "", "", "", "", "Total Neto Saldado:", "", (commission.TotalNetoVendido??0).ToString("N", nFI).Replace(",", ""));
            //Porcentaje de comisión
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", "", "", "", "", "", "", "", "", "", "Porcentaje de Comision:", "", (commission.PorcentajeComision??0).ToString("N", nFI).Replace(",", ""));
            //Bonos
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", "", "", "", "", "", "", "", "", "", "Comision:", "", ((decimal)commission.Cantidad).ToString("N", nFI).Replace(",", ""));
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", "", "", "", "", "", "", "", "", "", "Fecha:", "", commission.FechaPago.ToString("dd/MM/yyyy"));
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", "", "", "", "", "", "", "", "", "", "Comentarios:", "", commission.Detalle);
            
            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        [HttpPost]
        public ActionResult SetOmit(int idSale, bool omit)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tReports.SetOmit(idSale, omit);

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

        [HttpPost]
        public ActionResult SetOmitItem(int idDetail, bool omit)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tReports.SetOmitItem(idDetail, omit);

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

        //Reporte Comisiones Despachos
        public ActionResult SalesOffices()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSalesReportOffices(string dateSince, string dateUntil, int? office, string customer, string remission, short? amazonas, short? guadalquivir, short? textura)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tReports.GetSalesReportOffices(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), office, customer, remission, amazonas, guadalquivir, textura), Count = tReports.CountGetSalesReportOffices(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), office, customer, remission, amazonas, guadalquivir, textura) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult AddCommissionOffice(tReporteVentasComisionesDespacho oCommission, List<ReportOfSalesViewModel> sales)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idCommissionSale = tReports.AddCommissionOffice(oCommission);

                if (idCommissionSale > 0)
                {

                    if (sales != null)
                    {

                        tSales.UpdatePaidOutSalesOffice(idCommissionSale, oCommission.idDespacho, sales);

                    }

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "Se agregó el registro." };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "No se guardo el registro." };
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

        [HttpPost]
        public ActionResult GetCommissionsOffice(string dateSince, string dateUntil, int idOffice)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Commissions = tReports.GetCommissionsForOffice(idOffice), CommissionPayment = tReports.GetCommissionsOffice(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), idOffice).Sum(p => p.Total) };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult DetailSaleReportOffice(string remision)
        {
            SaleViewModel oSale = tSales.GetSaleForRemision(remision);

            return PartialView("~/Views/Intelligence/DetailSaleReportOffice.cshtml", oSale);
        }

        public void ExportReportOffice(int idCommissionSale)
        {
            var sales = tReports.GetSalesReportCSVOffice(idCommissionSale);

            var csv = new StringBuilder();

            csv.AppendLine("FECHA, VENDEDOR 1, VENDEDOR 2, TIPO DE CLIENTE, CLIENTE, DESPACHO, PROYECTO, SUCURSAL, REMISION, SUBTOTAL, DESCUENTO, TOTAL, NOTACREDITO, COMISION");

            string fileName = "Comisiones";

            var nFI = new NumberFormatInfo();

            nFI.NumberDecimalDigits = 2;

            foreach (var sale in sales)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}\n", sale.Fecha.Value.ToString("dd/MM/yyyy"), sale.Usuario1, sale.Usuario2, sale.TipoCliente, sale.Cliente, sale.Despacho1, sale.Proyecto, sale.Sucursal, sale.Remision, ((decimal)sale.ImporteVenta).ToString("N", nFI).Replace(",", ""), ((decimal)sale.Descuento).ToString("N", nFI).Replace(",", ""), ((decimal)sale.ImportePagadoCliente).ToString("N", nFI).Replace(",", ""), ((decimal)(sale.TotalNotaCredito ?? 0.0M)).ToString("N", nFI).Replace(",", ""), ((decimal)(sale.ComisionTotal??0.0M)).ToString("N", nFI).Replace(",", ""));
                fileName = "Comisiones-" + sale.Fecha.Value.ToString("MM/yyyy") + ".csv";
            }

            var commission = tReports.GetCommissionOffice(idCommissionSale);
            
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}\n", "", "", "", "", "", "", "", "", "", "", "", "Comision:", "", ((decimal)commission.Total).ToString("N", nFI).Replace(",", ""));
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}\n", "", "", "", "", "", "", "", "", "", "", "", "Fecha:", "", commission.FechaPago.ToString("dd/MM/yyyy"));
            csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}\n", "", "", "", "", "", "", "", "", "", "", "", "Comentarios:", "", commission.Detalle);
            
            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        //Reporte ANA
        public ActionResult SellerActivity()
        {
            return View();
        }        

        public ActionResult QuotationsReport(string dtDateSince, string dtDateUntil, int idseller)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var quotations = tQuotations.GetQuotationsForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);
                var costQuotations = tQuotations.GetQuotationsForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller).Select(p => p.Total).Sum();
                var totalQuotations = tQuotations.GetNumberQuotationsForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);
                var quotationsToSale = tQuotations.GetQuotationsToSaleForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);
                var quotationsSaled = tQuotations.GetQuotationsSaled(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Quotations = quotations, CostQuotations = costQuotations, TotalQuotations = totalQuotations, QuotationsToSale = quotationsToSale, QuotationsSaled = quotationsSaled };

            }catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult SalesReport(string dtDateSince, string dtDateUntil, int idseller)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var sales = tSales.GetSalesForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);
                var saled = tSales.GetSalesForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller).Select(p=>p.Total).Sum();
                var totalSales = tSales.GetNumberSalesForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);               

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = sales, Saled = saled, TotalSales = totalSales };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult OutProductsReport(string dtDateSince, string dtDateUntil, int idseller)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var views = tVistas.GetViewsForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);
                var viewsSaled = tVistas.GetViewsForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller).Select(p=>p.Total).Sum();
                var totalViews = tVistas.GetNumberViewsForSeller(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), idseller);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Views = views,ViewsSaled = viewsSaled, TotalViews = totalViews };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult SaveComments(string dtDate,int idseller, string comments, short typereport)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tReports.SaveComments(Convert.ToDateTime(dtDate), idseller, comments, typereport);

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

        [HttpPost]
        public ActionResult GetComments(string dtDate, int idseller, short typereport)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Comments = tReports.GetComments(Convert.ToDateTime(dtDate), idseller, typereport) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult DeleteComments(int idActivity)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tReports.DeleteComments(idActivity);

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

        //Reporte Total de Ventas
        public ActionResult SummarySales()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSummarySales(string dateSince, string dateUntil, int? office, string customer, short? amazonas, short? guadalquivir, short? textura)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tReports.GetSalesReportOffices(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), office, customer, String.Empty, amazonas, guadalquivir, textura), CommissionOffice = tReports.GetCommissionOfficeSummary(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), office, customer, amazonas, guadalquivir, textura), CommissionSeller = tReports.GetCommissionSellerSummary(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), office, customer, amazonas, guadalquivir, textura) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };


            }

            return Json(jmResult);
        }

        public ActionResult DetailSummarySaleReport(string remision)
        {
            SaleViewModel oSale = tSales.GetSaleForRemision(remision);

            return PartialView("~/Views/Intelligence/DetailSummarySaleReport.cshtml", oSale);
        }

        //Comisiones Especiales
        public ActionResult SpecialCommisions()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSalesFoSpecialReport(string dateSince, string dateUntil, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tReports.GetSalesFoSpecialReport(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), amazonas, guadalquivir, textura, page, pageSize), Count = tReports.CountRegistersWithFilters(Convert.ToDateTime(dateSince), Convert.ToDateTime(dateUntil), null, String.Empty, amazonas, guadalquivir, textura, page, pageSize) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };


            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult AddSpecialCommission(tReporteComisionesEspeciale oCommission)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idCommissionSale = tReports.AddSpecialCommission(oCommission);

                if (idCommissionSale > 0)
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "Se agregó el registro." };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "No se guardo el registro." };
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

        [HttpPost]
        public ActionResult GetSpecialCommissionsForSeller(int idSeller, decimal total)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Commissions = tReports.GetCommission(idSeller, total) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult GetSpecialCommissions(string date, int idSeller)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Commissions = tReports.GetSpecialCommissions(Convert.ToDateTime(date), idSeller), CommissionPayment = tReports.GetSpecialCommissions(Convert.ToDateTime(date), idSeller).Sum(p => p.Cantidad), Bonos = tReports.GetBono(Convert.ToDateTime(date), idSeller) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public void ExportSpecialReport(int idCommissionSale)
        {
            var csv = new StringBuilder();
            
            string fileName = "Comisiones";

            var nFI = new NumberFormatInfo();

            nFI.NumberDecimalDigits = 2;

            //Obtener comisión
            var commission = tReports.GetSpecialCommission(idCommissionSale);

            //Total Neto Vendido
            csv.AppendFormat("{0},{1},{2}\n", "Total Neto Saldado:", "", (commission.TotalNetoVendido ?? 0).ToString("N", nFI).Replace(",", ""));
            //Porcentaje de comisión
            csv.AppendFormat("{0},{1},{2}\n", "Porcentaje de Comision:", "", (commission.PorcentajeComision ?? 0).ToString("N", nFI).Replace(",", ""));
            //Bonos
            csv.AppendFormat("{0},{1},{2}\n", "Comision:", "", ((decimal)commission.Cantidad).ToString("N", nFI).Replace(",", ""));
            csv.AppendFormat("{0},{1},{2}\n", "Fecha:", "", commission.FechaPago.Value.ToString("dd/MM/yyyy"));
            csv.AppendFormat("{0},{1},{2}\n", "Comentarios:", "", commission.Detalle);

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }
    }
}