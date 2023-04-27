using ADEntities.Queries;
using ADSystem.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace ADSystem.Controllers
{
    /// <summary>
    /// SalesIntelligenceController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class SalesIntelligenceController : Controller
    {
        /// <summary>
        /// The sales
        /// </summary>
        private SalesReport sales;
        private DailyCutting dailys;
        private Reports reports;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesIntelligenceController" /> class.
        /// </summary>
        public SalesIntelligenceController()
        {
            sales = new SalesReport();
            dailys = new DailyCutting();
            reports = new Reports();
        }

        /// <summary>
        /// Prequotations list view.
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportPreQuotationView()
        {
            return View();
        }


        /// <summary>
        /// Estatus Precotizaciones Report.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="preQoutationNumber">The end date.</param>
        /// <param name="project">The end date.</param>
        /// <param name="typeCustomer">The end date.</param>
        ///  <param name="idCustomer">The end date.</param>
        ///   <param name="idVendor">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreQuotationReport(DateTime startDate, DateTime endDate, string preQoutationNumber, string project,int? typeCustomer,  int? idCustomer, int? idVendor)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Reports = reports.PreQoutationsReport( startDate,  endDate,  preQoutationNumber,  project,  typeCustomer,  idCustomer,  idVendor) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Estatus Prequotaions list XLS.
        /// <summary>
        /// Estatus Precotizaciones Report.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="preQoutationNumber">The end date.</param>
        /// <param name="project">The end date.</param>
        /// <param name="typeCustomer">The end date.</param>
        ///  <param name="idCustomer">The end date.</param>
        ///   <param name="idVendor">The end date.</param>
        /// <returns></returns>
        [HttpGet]
       public void PreQuotationsReportXLS(DateTime startDate, DateTime endDate, string preQoutationNumber, string project, int? typeCustomer, int? idCustomer, int? idVendor)

        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("PRECOTIZACION,CLIENTE FISICO,CLIENTE MORAL,DESPACHO,VENDEDOR,SUCURSAL,PROYECTO,FECHA CREACION,NUEVA,EN COTIZACION,ASIGNADA,EN PROCESO DE VENTA,PENDIENTE DE ORDENAR,ORDENADA");

            var result =  reports.PreQoutationsReport(startDate, endDate, preQoutationNumber, project, typeCustomer, idCustomer, idVendor);


        foreach (var prequotations in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}\n",
                    prequotations.Numero,
                    prequotations.ClienteF,
                    prequotations.ClienteM,
                    prequotations.Despacho,
                    prequotations.Vendedor,
                    prequotations.Sucursal,
                    prequotations.Proyecto,
                    prequotations.FechaCreacion,
                    prequotations.Nueva,
                    prequotations.En_Cotizacion,
                    prequotations.Asignada,
                    prequotations.En_Proceso_de_Venta,
                    prequotations.Pendiente_de_Ordenar,
                    prequotations.Ordenada
                    );
            }

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=PreQuotations " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }
      

        /// <summary>
        /// Prequotations Services list view.
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceReportView()
        {
            return View();
        }

        /// <summary>
        /// Prequotation Services List.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="preQoutationNumber">The end date.</param>
        /// <param name="idService">The end date.</param>
        /// <param name="idProvider">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ServiceReport(DateTime startDate, DateTime endDate, string preQoutationNumber, int? idService, int? idProvider)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Reports = reports.ServiceReport(startDate, endDate, preQoutationNumber, idService, idProvider) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Prequotaions list XLS.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="preQoutationNumber">The end date.</param>
        /// <param name="idService">The end date.</param>
        /// <param name="idProvider">The end date.</param>
        [HttpGet]
        public void PrequotationsServicesSalesXLS(DateTime startDate, DateTime endDate, string preQoutationNumber, int? idService, int? idProvider)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("PRECOTIZACIÓN,SERVICIO,PROVEEDOR,CLIENTE MORAL, CLIENTE FISICO,DESPACHO,NUEVO,ENVIADO A PROVEEDOR,  COTIZADO,ASIGNADO,EN PROCESO DE VENTA,PENDIENTE DE ORDENAR, ORDENADO");

            var result = reports.ServiceReport( startDate,  endDate,  preQoutationNumber, idService, idProvider);

            foreach (var prequotations in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}\n",
                    prequotations.Numero ,
                    prequotations.Servicio,
                    prequotations.Proveedor,
                    prequotations.ClienteM,
                    prequotations.ClienteF,
                    prequotations.Despacho,
                    prequotations.Nuevo,
                    prequotations.EnviadoProveedor,
                    prequotations.Cotizado,
                    prequotations.Asignado,
                    prequotations.EnviadoProcesoVenta,
                    prequotations.PendienteOrdenar,
                    prequotations.Ordenado
                    );
            }

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=Service " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Totals the sales view.
        /// </summary>
        /// <returns></returns>
        public ActionResult TotalSalesView()
        {
            return View();
        }

        /// <summary>
        /// Totals the sales.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TotalSales(DateTime startDate, DateTime endDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Sales = sales.TotalSales(startDate, endDate) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Totals the sales XLS.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        [HttpGet]
        public void TotalSalesXLS(string startDate, string endDate)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("AÑO,MES,SUBTOTAL,DESCUENTO,PAGOCONNOTACREDITO,NOTACREDITOGENERADA,VENTABRUTA,IVAPAGADOPORELCLIENTE,EFECTIVO,COMPASSANNA,COMPASSFERNANDA,BANORTEANNA,BANREGIOANNA," +
                "SINDESTINO,SINCUENTA,VENTACREDITO,TARJETACREDITO,TARJETADEBITO,TRANSFERENCIA,CHEQUE,DEPOSITO,FISCALSINDESTINO,BANREGIOACCENTS,BANORTEACCENTS,VENTABRUTAFISCAL,IVA,VENTANETAFISCAL," +
                "VENTANETA,COMISIONVENDEDORES,COMISIONDESPACHOS,COMISIONTARJETACREDITO");

            var result = sales.TotalSales(firstDate, secondDate);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var sale in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}\n",
                    sale.Tiempo,
                    sale.Mes,
                    (sale.Subtotal ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Descuento ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.PagoConNotaCredito ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.NotaCreditoGenerada ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.VentaBruta ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.IVAPagadoPorElCliente ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Efectivo ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.CompassAnna ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.CompassFernanda ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.BanorteAnna ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.BanregioAnna ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.SinDestino ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.SinCuenta ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.VentaCredito ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.TarjetaCredito ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.TarjetaDebito ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Transferencia ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Cheque ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Deposito ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.FiscalSinDestino ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.BanregioAccents ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.BanorteAccents ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.VentaBrutaFiscal ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.IVAFiscal ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.VentaNetaFiscal ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.VentaNeta ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.ComisionVendedores ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.ComisionDespachos ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.ComisionTarjetaCredito ?? 0).ToString("N", nFI).Replace(",", "")
                    );
            }

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=VentasTotales " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Viewses this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewsView()
        {
            return View();
        }

        /// <summary>
        /// Viewses the specified user.
        /// </summary>
        /// <param name="idSeller">The identifier seller.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="idBranch">The identifier branch.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Views(int? idSeller, DateTime startDate, DateTime endDate, int? idBranch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Views = sales.Views(idSeller, startDate, endDate, idBranch) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Viewses the XLS.
        /// </summary>
        /// <param name="idSeller">The identifier seller.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="idBranch">The identifier branch.</param>
        [HttpGet]
        public void ViewsXLS(int? idSeller, DateTime startDate, DateTime endDate, int? idBranch)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("SUCURSAL,FECHA,VISTA,CLIENTE,VENDEDOR,CANT. PRODUCTOS,ART. DEVUELTOS, ART. PENDIENTES, SUBTOTAL,TOTAL");

            var result = sales.Views(idSeller, startDate, endDate, idBranch);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var sale in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n",
                    sale.Sucursal,
                    sale.Fecha.Value.ToString("dd/MM/yyyy"),
                    sale.Remision,
                    sale.Cliente,
                    sale.Vendedor,
                    sale.CantProductos,
                    (sale.ArticulosDevueltos ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.ArticulosPendientes ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Subtotal ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Total ?? 0).ToString("N", nFI).Replace(",", "")
                    );
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=SalidasVista " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Viewses this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesTypeCustomerView()
        {
            return View();
        }

        /// <summary>
        /// Viewses the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="branch">The branch.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SalesTypeCustomer(int? user, int? branch, DateTime startDate, DateTime endDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Sales = sales.SalesTypeCustomer(user, branch, startDate, endDate) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Viewses this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesCategoriesView()
        {
            return View();
        }

        /// <summary>
        /// Viewses the specified user.
        /// </summary>
        /// <param name="typeSale">The type sale.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="idCategory">The identifier category.</param>
        /// <param name="idSubcategory">The identifier subcategory.</param>
        /// <param name="idService">The identifier service.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SalesCategories(int typeSale, DateTime startDate, DateTime endDate, Nullable<int> idCategory, Nullable<int> idSubcategory, Nullable<int> idService)
        {
            JsonMessenger jmResult = new JsonMessenger();
            const int salesProduct = 1;
            const int salesServices = 2;

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;

                if (typeSale == salesServices)
                {
                    jmResult.oData = new { Sales = sales.SalesServices(startDate, endDate, idService) };
                }
                else if (typeSale == salesProduct)
                {
                    jmResult.oData = new { Sales = sales.SalesProduct(startDate, endDate, idCategory, idSubcategory) };
                }

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saleses the categories XLS.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="idCategory">The identifier category.</param>
        /// <param name="idSubcategory">The identifier subcategory.</param>
        [HttpGet]
        public void SalesProductsXLS(DateTime startDate, DateTime endDate, Nullable<int> idCategory, Nullable<int> idSubcategory)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("AÑO,MES,DIA,REMISION,VENDEDOR,SUCURSAL,CATEGORIA,SUBCATEGORIA,CODIGO,CANTIDAD,PRECIO,DESCUENTO,TOTAL");

            dynamic result = sales.SalesProduct(startDate, endDate, idCategory, idSubcategory);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var sale in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}\n",
                    sale.Tiempo,
                    sale.Mes,
                    sale.Dia,
                    sale.Remision,
                    sale.Vendedor,
                    sale.Sucursal,
                    (sale.Categoria == null) ? "N/A" : sale.Categoria,
                    (sale.Subcategoria == null) ? "N/A" : sale.Subcategoria,
                    (sale.Codigo == null) ? "N/A" : sale.Codigo,
                    sale.Cantidad,
                    (sale.Precio ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Descuento ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Total ?? 0).ToString("N", nFI).Replace(",", "")
                    );
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=VentasCategorias " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Saleses the services XLS.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="idService">The identifier service.</param>
        [HttpGet]
        public void SalesServicesXLS(DateTime startDate, DateTime endDate, Nullable<int> idService)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("AÑO,MES,REMISION,VENDEDOR,SUCURSAL,SERVICIO,CANTIDAD,PRECIO,DESCUENTO,TOTAL");


            dynamic result = sales.SalesServices(startDate, endDate, idService);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var sale in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n",
                    sale.Tiempo,
                    sale.Mes,
                    sale.Remision,
                    sale.Vendedor,
                    sale.Sucursal,
                    sale.Servicio,
                    sale.Cantidad,
                    (sale.Precio ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Descuento ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Total ?? 0).ToString("N", nFI).Replace(",", "")
                    );
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=VentasCategorias " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Quotations this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Quotations()
        {
            return View();
        }

        /// <summary>
        /// Quotationses the sales.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QuotationsSales(DateTime startDate, DateTime endDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Quotations = sales.QuotationsSales(startDate, endDate) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Quotationses the sales XLS.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        [HttpGet]
        public void QuotationsSalesXLS(DateTime startDate, DateTime endDate)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("FECHACOTIZACION,COTIZACION,MONTOCOTIZADO,VENDEDOR,SUCURSAL,CLIENTE,VENTA,PROYECTO,MONTOVENTA");

            var result = sales.QuotationsSales(firstDate, secondDate);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var quotation in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n",
                    quotation.FechaCotizacion.Value.ToString("dd/MM/yyyy"),
                    quotation.Cotizacion,
                    (quotation.TotalCotizacion ?? 0).ToString("N", nFI).Replace(",", ""),
                    quotation.Vendedor,
                    quotation.Sucursal,
                    quotation.Cliente,
                    quotation.Venta,
                    quotation.Proyecto,
                    (quotation.TotalVenta ?? 0).ToString("N", nFI).Replace(",", "")
                    );
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=Cotizaciones " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Credits the sales view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreditSalesView()
        {
            return View();
        }

        /// <summary>
        /// Credits the sales.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreditSales(DateTime startDate, DateTime endDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { CreditSales = sales.CreditSales(startDate, endDate) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Credits the sales XLS.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        [HttpGet]
        public void CreditSalesXLS(DateTime startDate, DateTime endDate)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("AÑO,MES,REMISION,SUCURSAL,CLIENTE,VENDEDOR,TOTAL,CREDITO,PAGADO,PENDIENTE,ULTIMOABONO");

            var result = sales.CreditSales(firstDate, secondDate);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var credits in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\n",
                    credits.Tiempo,
                    credits.Mes,
                    credits.Remision,
                    credits.Sucursal,
                    (credits.Cliente != null) ? credits.Cliente : "N/A",
                    credits.Vendedor,
                    (credits.Total ?? 0).ToString("N", nFI).Replace(",", ""),
                    (credits.Credito ?? 0).ToString("N", nFI).Replace(",", ""),
                    (credits.Pagado ?? 0).ToString("N", nFI).Replace(",", ""),
                    (credits.Pendiente ?? 0).ToString("N", nFI).Replace(",", ""),
                    credits.UltimoAbono
                    );
            }

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=VentasCredito " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Currents the inventory view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CurrentInventoryView()
        {
            return View();
        }

        /// <summary>
        /// Currents the inventory.
        /// </summary>
        /// <param name="idProduct">The identifier product.</param>
        /// <param name="queryDate">The query date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CurrentInventory(int idProduct, DateTime queryDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { CurrentInventory = sales.CurrentInventory(idProduct, queryDate) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Currents the inventory XLS.
        /// </summary>
        /// <param name="idProduct">The identifier product.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        [HttpGet]
        public void CurrentInventoryXLS(int? idProduct, DateTime queryDate)
        {
            var csv = new StringBuilder();

            csv.AppendLine("CODIGO,DESCRIPCION,SUCURSAL,PRECIO COMPRA,PRECIO VENTA,CATEGORIA,SUB CATEGORIA,EXISTENCIA");

            var result = sales.CurrentInventory(idProduct, queryDate);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var inventory in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}\n",
                    inventory.Codigo,
                    inventory.Descripcion,
                    inventory.Sucursal,
                    (inventory.PrecioCompra ?? 0).ToString("N", nFI).Replace(",", ""),
                    (inventory.PrecioVenta ?? 0).ToString("N", nFI).Replace(",", ""),
                    inventory.Categoria,
                    inventory.Subcategoria,
                    inventory.Existencia
                    );
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=InventarioPorDia " + queryDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Saleses the services view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SalesServicesView()
        {
            return View();
        }

        /// <summary>
        /// Saleses the services.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SalesServices(DateTime startDate, DateTime endDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { SalesServices = sales.SalesServices(startDate, endDate, null) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Productses the purchased date view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductsPurchasedDateView()
        {
            return View();
        }

        /// <summary>
        /// Productses the purchased date.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductsPurchasedDate()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { SalesProducts = sales.ProductsPurchasedDate() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Productses the purchased date XLS.
        /// </summary>
        [HttpGet]
        public void ProductsPurchasedDateXLS()
        {
            var csv = new StringBuilder();

            csv.AppendLine("CODIGO,DESCRIPCION,FECHA COMPRA,PRECIO COMPRA,PRECIO VENTA,INVENTARIO");

            var result = sales.ProductsPurchasedDate();

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var sales in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5}\n",
                    sales.Codigo,
                    sales.Descripcion,
                    sales.FechaCompra.Value.ToString("dd/MM/yyyy"),
                    (sales.PrecioCompra ?? 0).ToString("N", nFI).Replace(",", ""),
                    sales.PrecioVenta,
                    sales.Inventario);
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=FechaCompraProductos " + DateTime.Now.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Origins the customers view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OriginCustomersView()
        {
            return View();
        }

        /// <summary>
        /// Origins the customers.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OriginCustomers(DateTime startDate, DateTime endDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { SalesCustomers = sales.OriginCustomers(startDate, endDate) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Origins the customers XLS.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        [HttpGet]
        public void OriginCustomersXLS(DateTime startDate, DateTime endDate)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("NOMBRE,CREADO,CORREO,CREADO POR,ORIGEN");

            var result = sales.OriginCustomers(firstDate, secondDate);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var sales in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4}\n",
                    sales.Nombre.Replace(",", "").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " "),
                    sales.Creado.Value.ToString("dd/MM/yyyy"),
                    sales.Correo,
                    sales.CreadoPor,
                    sales.Origen);
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=OrigenClientes " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

        /// <summary>
        /// Dailies the cuttings view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DailyCutView()
        {
            Session["Controller"] = "CorteDiario";
            return View();
        }
		/// <summary>
		/// Dailies the cuttings.
		/// </summary>
		/// <param name="queryDate">The query date.</param>
		/// <param name="idBranchOffice">The identifier branch office.</param>
		/// <param name="idUser">The identifier user.</param>
		/// <returns></returns>
		[HttpGet]
        public ActionResult DailyCuttings(DateTime queryDate, int? idBranchOffice, int? idUser)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Reports = dailys.GetDailyCutting(queryDate, idBranchOffice, idUser) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

		/// <summary>
		/// Totals the branch office XLS.
		/// </summary>
		/// <param name="queryDate">The query date.</param>
		/// <param name="idBranchOffice">The identifier branch office.</param>
		/// <param name="idUser">The identifier user.</param>
		public void DailyCutXLS(DateTime queryDate, int? idBranchOffice, int? idUser)
        {
            DateTime firstDate = Convert.ToDateTime(queryDate);

           var csv = new StringBuilder();

           csv.AppendLine("FECHA,SUCURSAL,USUARIO,REMISION,FORMA DE PAGO,CANTIDAD");

            var result = dailys.GetDailyCutting(queryDate, idBranchOffice, idUser);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;
                        
            var totalBranch = 0.0M;

            foreach (var branch in result)
            { 
                foreach ( var cut in branch.TerminalsBanks)
                {
                    csv.AppendLine("" + cut.TerminalDestiny);
                    var total = 0.0M;
                    foreach (var c in cut.DailyCuttings)
                    {
                        csv.AppendFormat("{0},{1},{2},{3},{4},{5}\n",
                        c.Fecha,
                        c.Sucursal,
                        c.Usuario,
                        c.Remision,
                        c.FormaPago,
                        (c.Cantidad ?? 0.0M).ToString("N", nFI).Replace(",", ""));

                        total = total + (c.Cantidad ?? 0.0M);
                    }
                    csv.AppendLine(",,,,TOTAL,$" + total.ToString("N", nFI).Replace(",", ""));
                    totalBranch = totalBranch + (total);
                }
            }
               
            csv.AppendLine(",,,,TOTAL GENERAL,$" + totalBranch.ToString("N", nFI).Replace(",", ""));

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=VentasTotales " + firstDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }
    }
}