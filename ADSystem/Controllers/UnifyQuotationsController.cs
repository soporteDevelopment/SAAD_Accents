using ADEntities.Enums;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class UnifyQuotationsController : BaseController
    {
        public override ActionResult Index()
        {
            Session["Controller"] = "Cotizaciones";

            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);

            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));

            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        [HttpPost]
        public ActionResult GetQuotations(string costumer, int? iduser, string project, short? amazonas, short? guadalquivir, short? textura, bool dollar, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                var result = tQuotations.GetQuotations(costumer, iduser, project, idUser, restricted, amazonas, guadalquivir, textura, dollar, page, pageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Quotations = result.Item1, Count = result.Item2 };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GenerateUnifiedQuotation(string lQuotations)
        {
            var quotations = JsonConvert.DeserializeObject<List<int>>(lQuotations);

            UnifiedQuotationViewModel quotation = tUnifiedQuotations.GenerateUnifiedQuotation(quotations);

            return View(quotation);
        }

        public ActionResult SaleUnifiedQuotation(string lQuotations)
        {
            var quotations = JsonConvert.DeserializeObject<List<int>>(lQuotations);

            UnifiedQuotationViewModel quotation = tUnifiedQuotations.GenerateUnifiedQuotation(quotations);

            return View(quotation);
        }

        public ActionResult SaleUnifiedQuotationDollar(string lQuotations)
        {
            var quotations = JsonConvert.DeserializeObject<List<int>>(lQuotations);

            UnifiedQuotationViewModel quotation = tUnifiedQuotations.GenerateUnifiedQuotation(quotations);

            return View(quotation);
        }

        [HttpPost]
        public ActionResult PrintUnifiedQuotation(string quotation)
        {
            UnifiedQuotationViewModel oQuotation = JsonConvert.DeserializeObject<UnifiedQuotationViewModel>(quotation);                      

            return PartialView(oQuotation);
        }

        [HttpPost]
        public ActionResult SaveSale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<TypePaymentViewModel> lTypePayment)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (lProducts.Any(p => p.cantidad <= 0))
                {
                    throw new Exception("No se permiten cantidades iguales o menores a cero");
                }

                if (!tSales.VerifySale(idUser1, DateTime.Now))
                {
                    int idSale = 0;

                    //Se valida que la venta no se esté duplicando
                    tProducts.ValidateProductToAvoidNegativesUnifiedSales(lProducts);

                    //Se agrega la venta y se registra el detalle
                    string rem = tSales.AddSale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, null, null, (int)OriginCurrency.Unified, out idSale);

                    //Se registran los métodos de pago
                    tSales.AddTypePayment(idSale, lTypePayment);

                    //Si la venta viene de una Cotización se modifica está última
                    foreach (var quotation in lProducts.GroupBy(p => p.idCotizacion).Select(p => p.Key).ToList())
                    {
                        if (quotation != null)
                        {
                            if (quotation > 0)
                            {
                                QuotationsController oQuotations = new QuotationsController();

                                //Se valida la Salida a Vista ligada a la Cotización
                                tVistas.RelatedOutProductAtQuotation((int)quotation, lProducts.Where(p=>p.idCotizacion == quotation).ToList(), rem, idUser1);

                                oQuotations.DeleteQuotation((int)quotation);
                            }
                        }
                    }

                    SalesController sale = new SalesController();

                    sale.SendMailSale(idSale);

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se registró la venta con exito"), sRemision = rem };
                }
                else
                {
                    throw new System.ArgumentException("Riesgo de duplicar Venta", "Venta");
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
        public ActionResult SaveDollarSale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<TypePaymentViewModel> lTypePayment)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (lProducts.Any(p => p.cantidad <= 0))
                {
                    throw new Exception("No se permiten cantidades iguales o menores a cero");
                }

                if (!tSales.VerifySale(idUser1, DateTime.Now))
                {
                    int idSale = 0;

                    //Se valida que la venta no se esté duplicando
                    tProducts.ValidateProductToSaleAvoidNegatives(lProducts, idBranch);

                    //Se agrega la venta y se registra el detalle
                    string rem = tSales.AddSale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, null, null, (int)OriginCurrency.Dollar, out idSale);

                    //Se registran los métodos de pago
                    tSales.AddTypePayment(idSale, lTypePayment);

                    //Si la venta viene de una Cotización se modifica está última
                    foreach (var quotation in lProducts.GroupBy(p => p.idCotizacion).Select(p => p.Key).ToList())
                    {
                        if (quotation != null)
                        {
                            if (quotation > 0)
                            {
                                QuotationsController oQuotations = new QuotationsController();

                                //Se valida la Salida a Vista ligada a la Cotización
                                tVistas.RelatedOutProductAtQuotation((int)quotation, lProducts.Where(p => p.idCotizacion == quotation).ToList(), rem, idUser1);

                                oQuotations.DeleteQuotation((int)quotation);
                            }
                        }
                    }

                    SalesController sale = new SalesController();

                    sale.SendMailSale(idSale);

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se registró la venta con exito"), sRemision = rem };
                }
                else
                {
                    throw new System.ArgumentException("Riesgo de duplicar Venta", "Venta");
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
    }
}