using ADEntities.Queries;
using ADSystem.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Globalization;

namespace ADSystem.Controllers
{
    public class TotalSalesBySellerController : Controller
    {
        private SalesReport sales;

        /// <summary>
        /// Initializes a new instance of the <see cref="TotalSalesBySellerController"/> class.
        /// </summary>
        public TotalSalesBySellerController()
        {
            sales = new SalesReport();
        }
        // GET: TotalSalesBySaller
        public ActionResult Index()
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
        public ActionResult TotalSalesBySeller(int? idSeller, DateTime startDate, DateTime endDate)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 1;
                jmResult.oData = new { Sales = sales.TotalSalesBySeller(idSeller, startDate, endDate) };
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
        public void TotalSalesBySellerXLS(int? idSeller, string startDate, string endDate)
        {
            DateTime firstDate = Convert.ToDateTime(startDate);
            DateTime secondDate = Convert.ToDateTime(endDate);

            var csv = new StringBuilder();

            csv.AppendLine("AÑO,MES,VENDEDOR,SUBTOTAL,DESCUENTO,PAGOCONNOTACREDITO,NOTACREDITOGENERADA,VENTABRUTA,EFECTIVO,COMPASSANNA,COMPASSFERNANDA,BANORTEANNA,BANREGIOANNA,SINDESTINO,SINCUENTA,VENTACREDITO,TARJETACREDITO,TARJETADEBITO,TRANSFERENCIA,CHEQUE,DEPOSITO,FISCALSINDESTINO,BANREGIOACCENTS,BANORTEACCENTS,VENTABRUTAFISCAL,IVA,VENTANETAFISCAL,VENTANETA,COMISIONTARJETACREDITO");

            var result = sales.TotalSalesBySeller(idSeller, firstDate, secondDate);

            var nFI = new NumberFormatInfo();
            nFI.NumberDecimalDigits = 2;

            foreach (var sale in result)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28}\n",
                    sale.Tiempo,
                    sale.Mes,
                    sale.Vendedor,
                    (sale.Subtotal ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.Descuento ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.PagoConNotaCredito ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.NotaCreditoGenerada ?? 0).ToString("N", nFI).Replace(",", ""),
                    (sale.VentaBruta ?? 0).ToString("N", nFI).Replace(",", ""),
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
                    (sale.ComisionTarjetaCredito ?? 0).ToString("N", nFI).Replace(",", "")
                    );
            }


            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=VentasTotales " + firstDate.ToString("MM/yyyy") + "-" + secondDate.ToString("MM/yyyy") + ".csv");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.Write(csv.ToString());
        }

    }
}
