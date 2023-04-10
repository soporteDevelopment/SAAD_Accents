using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ADSystem.Helpers;
using ADEntities.Queries;
using ADEntities.ViewModels;
using ADSystem.Common;
using System.Text;
using System.Globalization;
using System.IO;
using ADEntities.Queries.TypesGeneric;
using ADEntities.Enums;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class SalesController : BaseController
    {
        PDFGenerator _PDFGenerator = new PDFGenerator();

        // GET: Sales
        public override ActionResult Index()
        {
            Session["Controller"] = "Ventas";
            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);
            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));
            ViewBag.Branches = tBranches.GetAllActivesBranches();
            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        public ActionResult GetCustomer(string name)
        {
            return Json(new { Customers = tMoralCustomers.GetCustomers(name) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRemissions(string remission)
        {
            return Json(new { Sales = tSales.GetRemissions(remission) }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult ListSales()
        {
            return View();
        }

        public ActionResult ListEraserSales()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSales(bool allTime, bool searchPayment, string dtDateSince, string dtDateUntil, string remision, string costumer, int? iduser, string codigo, string project, string billNumber, string voucher, int status, int? statusPayment, decimal? payment, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tSales.GetSales(allTime, searchPayment, Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), remision, costumer, iduser, codigo, project, billNumber, voucher, status, statusPayment, idUser, restricted, payment, amazonas, guadalquivir, textura, page, pageSize), Count = tSales.GetCount(allTime, Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), remision, costumer, iduser, codigo, project, billNumber, voucher, status, statusPayment, idUser, restricted, payment, amazonas, guadalquivir, textura) };
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
        public ActionResult GetEraserSales(string costumer, int? iduser, string project, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tEraserSales.GetEraserSales(costumer, iduser, project, idUser, restricted, amazonas, guadalquivir, textura, page, pageSize), Count = tEraserSales.CountRegisters() };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ViewResult GetEraserSale(int idEraserSale)
        {

            SaleViewModel oSale = tEraserSales.GetEraserSaleForId(idEraserSale);

            ViewBag.IDBranch = oSale.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View(oSale);

        }

        [HttpPost]
        public ActionResult SaveSale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<ProductSaleViewModel> lProductsOut,
            List<TypePaymentViewModel> lTypePayment, int? idView, int? idQuotation, int? idEraserSale, short? idUserChecker)
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

                    //Se agrega la venta y se registrar el detalle
                    string rem = tSales.AddSale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, idView, idQuotation, (int)OriginCurrency.Pesos, out idSale);

                    //Se registran los métodos de pago
                    tSales.AddTypePayment(idSale, lTypePayment);

                    //Si la venta viene de una Salida a Vista se modifica está última
                    if (idView.HasValue)
                    {
                        if (idView > 0)
                        {
                            var lProd = new List<KeyValuePair<int?, decimal?>>();

                            if (idQuotation.HasValue)
                            {
                                if (idQuotation > 0)
                                {
                                    lProductsOut = tVistas.GetOutProductsDetails((int)idView).Select(p => new ProductSaleViewModel()
                                    {
                                        idProducto = p.idProducto,
                                        codigo = p.Codigo,
                                        cantidad = p.Cantidad
                                    }).ToList();
                                }
                            }
                            else if (lProductsOut == null)
                            {
                                lProductsOut = tVistas.GetOutProductsDetails((int)idView).Select(p => new ProductSaleViewModel()
                                {
                                    idProducto = p.idProducto,
                                    codigo = p.Codigo,
                                    cantidad = p.Cantidad
                                }).ToList();
                            }

                            foreach (var product in lProductsOut)
                            {
                                //Se valida que los artículos vendidos no sean mayores a los de la Salida a Vista
                                ProductSaleViewModel prod = lProducts.FirstOrDefault(p => p.idProducto == product.idProducto);

                                if (prod != null)
                                {
                                    var pendingProd = tVistas.GetPendingProduct((int)idView, (int)product.idProducto);

                                    //Si los artículos vendidos son iguales a los pendientes en la Salida a Vista
                                    if (pendingProd.Pendiente == prod.cantidad)
                                    {
                                        lProd.Add(new KeyValuePair<int?, decimal?>(product.idProducto, prod.cantidad));
                                    }
                                    //Si los artículos vendidos son menores a los de la Salida a Vista
                                    else if (pendingProd.Pendiente > prod.cantidad)
                                    {
                                        lProd.Add(new KeyValuePair<int?, decimal?>(product.idProducto, prod.cantidad));
                                    }
                                    else
                                    {
                                        //Si los artículos vendidos son mayores a los pendientes en la Salida a Vista pero los selecciondos son menores a los pendientes
                                        if ((pendingProd.Pendiente < prod.cantidad) && (pendingProd.Pendiente > product.cantidad))
                                        {
                                            lProd.Add(new KeyValuePair<int?, decimal?>(product.idProducto,
                                                pendingProd.Pendiente));
                                        }
                                        //Si los artículos vendidos son iguales a los de la Salida a Vista
                                        else
                                        {
                                            lProd.Add(new KeyValuePair<int?, decimal?>(product.idProducto,
                                                pendingProd.Pendiente));
                                        }
                                    }
                                }
                            }

                            OutProducts oOutProducts = new OutProducts();

                            if (idUserChecker == null)
                            {
                                idUserChecker = Convert.ToInt16(Session["_ID"]);
                            }

                            oOutProducts.UpdateStockSaleOutProducts(rem, (int)idView, lProd, (short)idUserChecker);

                            //Se salda el flete de la Salida a Vista
                            tVistas.UpdateFleteOutProduct(int.Parse(idView.ToString()));
                        }
                    }

                    //Si la venta viene de una Cotización se modifica está última
                    if (idQuotation.HasValue)
                    {
                        if (idQuotation > 0)
                        {
                            QuotationsController oQuotations = new QuotationsController();

                            oQuotations.DeleteQuotation((int)idQuotation);
                        }
                    }

                    //Si la venta viene de una Venta en Borrador se modifica está última
                    if (idEraserSale.HasValue)
                    {
                        if (idEraserSale > 0)
                        {
                            tEraserSales.CloseEraserSale((int)idEraserSale, idBranch);
                        }

                    }

                    this.SendMailSale(idSale);

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
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<ProductSaleViewModel> lProductsOut,
            List<TypePaymentViewModel> lTypePayment, int? idView, int? idQuotation)
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

                    //Se agrega la venta y se registrar el detalle
                    string rem = tSales.AddSale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, idView, idQuotation, (int)OriginCurrency.Pesos, out idSale);

                    //Se registran los métodos de pago
                    tSales.AddTypePayment(idSale, lTypePayment);

                    //Si la venta viene de una Cotización se modifica está última
                    if (idQuotation.HasValue)
                    {
                        if (idQuotation > 0)
                        {
                            QuotationsController oQuotations = new QuotationsController();
                            oQuotations.DeleteQuotation((int)idQuotation);
                        }
                    }

                    this.SendMailSale(idSale);

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

        public int InternalSaveSale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<TypePaymentViewModel> lTypePayment)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idSale = 0;

                string rem = tSales.AddSale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff",
                                CultureInfo.InvariantCulture), amountProducts, subtotal, discount, IVA, total, lProducts, null, null, (int)OriginCurrency.Pesos, out idSale);

                tSales.AddTypePayment(idSale, lTypePayment);

                this.SendMailSale(idSale);

                return idSale;

            }
            catch (Exception ex)
            {

                throw ex;

            }

        }

        [HttpPost]
        public ActionResult SaveEraserSale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, int? idView, decimal IVATasa)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                int idSale = tEraserSales.AddEraserSale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, idView, IVATasa);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó en borrador la venta con exito"), idSale = idSale };

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
        public ActionResult SaveUpdateEraserSale(int idEraserSale, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, int? idView)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tEraserSales.SaveUpdateEraserSale(idEraserSale, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, idView);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó en borrador la venta con exito") };

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
        public ActionResult DeleteEraserSale(int idEraserSale, int idBranch)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tEraserSales.DeleteEraserSale(idEraserSale, idBranch);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Registro borrado." };

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
        public ActionResult GeneratePrevNumberRem(int? idBranch)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (!idBranch.HasValue)
                {

                    idBranch = 0;

                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sRemision = tSales.GeneratePrevNumberRem((int)idBranch) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public ActionResult DetailSale(string remision)
        {
            SaleViewModel oSale = tSales.GetSaleForRemision(remision);
            return PartialView("~/Views/Sales/DetailSale.cshtml", oSale);
        }

        [HttpPost]
        public ActionResult DetailSaleForRemision(string remision)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sale = tSales.GetSaleToCredit(remision) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public PartialViewResult PrintSale(string remision)
        {

            var oSale = tSales.GetSaleForRemision(remision);
            var branch = tBranches.GetBranch(oSale.idSucursal ?? 0);
            if (branch != null)
                oSale.IVATotal = oSale.Subtotal * (branch.IVATasa / 100);

            ViewBag.Descuento = Convert.ToInt16(oSale.oDetail.Exists(p => (p.Descuento > 0)));

            return PartialView(oSale);

        }

        public PartialViewResult PrintSaleWithBankInformation(string remision)
        {
            var oSale = tSales.GetSaleForRemision(remision);

            ViewBag.Descuento = Convert.ToInt16(oSale.oDetail.Exists(p => (p.Descuento > 0)));

            return PartialView(oSale);
        }

        public ActionResult PrintEraserSale(int idEraserSale)
        {

            SaleViewModel oSale = tEraserSales.GetEraserSaleForId(idEraserSale);
            var branch = tBranches.GetBranch(oSale.idSucursal ?? 0);
            if (branch != null)
                oSale.IVATotal = oSale.Subtotal * (branch.IVATasa / 100);

            ViewBag.Descuento = Convert.ToInt16(oSale.oDetail.Exists(p => (p.Descuento > 0)));

            return PartialView(oSale);

        }

        [HttpPost]
        public ActionResult GetTypesPaymentForSale(int idSale)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { TypesPayment = tSales.GetTypePayment(idSale) };
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
        public ActionResult GetTypesPaymentForPrint(int idSale)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { TypesPayment = tSales.GetTypePaymentForPrint(idSale) };

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
        public ActionResult SendBill(string remision, string concept, string name, string phone, string mail, string rfc, string street, string outNum, string inNum, string suburb, string town, string state, string cp)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                BillViewModel bill = new BillViewModel()
                {
                    Nombre = name,
                    Telefono = phone,
                    Correo = mail,
                    RFC = rfc,
                    Calle = street,
                    NumExt = outNum,
                    NumInt = inNum,
                    Colonia = suburb,
                    Municipio = town,
                    Estado = state,
                    CP = cp
                };

                if (tSales.ValidateBill(rfc) == 0)
                {
                    tSales.AddBill(bill);
                }
                else
                {
                    tSales.UpdateBill(bill);
                }

                StringBuilder body = new StringBuilder();

                body.Append("<p>Se ha generado una compra con el número de remisón: <b>" + remision + "</b></p>");
                body.Append("<br/>");
                body.Append("<p>Nombre:&nbsp;<b class='brand-danger'>" + name + "</b></p>");
                body.Append("<p>Teléfono:&nbsp;<b class='brand-danger'>" + phone + "</b></p>");
                body.Append("<p>Correo:&nbsp;<b class='brand-danger'>" + mail + "</b></p>");
                body.Append("<p>RFC:&nbsp;<b class='brand-danger'>" + rfc + "</b></p>");
                body.Append("<p>Calle:&nbsp;<b class='brand-danger'>" + street + "</b></p>");
                body.Append("<p>Núm. Ext:&nbsp;<b class='brand-danger'>" + outNum + "</b></p>");
                body.Append("<p>Núm. Int:&nbsp;<b class='brand-danger'>" + inNum + "</b></p>");
                body.Append("<p>Calle:&nbsp;<b class='brand-danger'>" + street + "</b></p>");
                body.Append("<p>Colonia:&nbsp;<b class='brand-danger'>" + suburb + "</b></p>");
                body.Append("<p>Municipio:&nbsp;<b class='brand-danger'>" + town + "</b></p>");
                body.Append("<p>Estado:&nbsp;<b class='brand-danger'>" + state + "</b></p>");
                body.Append("<p>CP:&nbsp;<b class='brand-danger'>" + cp + "</b></p>");
                body.Append("<br/>");

                var users = tUsers.GetUsersBill().Select(p => p.Correo).ToList();

                if (users != null)
                {
                    var emailService = new Email();
                    emailService.SendMail(users, "Facturar", body.ToString());
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Los datos de la factura se han enviado con exito." };

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
        public ActionResult SendBillPayment(string remision, decimal amount, string concept, string name, string phone, string mail, string rfc, string street, string outNum, string inNum, string suburb, string town, string state, string cp)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                BillViewModel bill = new BillViewModel()
                {
                    Nombre = name,
                    Telefono = phone,
                    Correo = mail,
                    RFC = rfc,
                    Calle = street,
                    NumExt = outNum,
                    NumInt = inNum,
                    Colonia = suburb,
                    Municipio = town,
                    Estado = state,
                    CP = cp
                };

                if (tSales.ValidateBill(rfc) == 0)
                {
                    tSales.AddBill(bill);
                }
                else
                {
                    tSales.UpdateBill(bill);
                }

                StringBuilder body = new StringBuilder();

                body.Append("<p>Se ha generado una compra con el número de remisón: <b>" + remision + "</b></p>");
                body.Append("<p>La factura es por concepto de una forma de pago.</p>");
                body.Append("<p>La factura es por la cantidad de: <b>" + amount + "</b>.</p>");
                body.Append("<br/>");
                body.Append("<p>Concepto:&nbsp;<b class='brand-danger'>" + concept + "</b></p>");
                body.Append("<p>Nombre:&nbsp;<b class='brand-danger'>" + name + "</b></p>");
                body.Append("<p>Teléfono:&nbsp;<b class='brand-danger'>" + phone + "</b></p>");
                body.Append("<p>Correo:&nbsp;<b class='brand-danger'>" + mail + "</b></p>");
                body.Append("<p>RFC:&nbsp;<b class='brand-danger'>" + rfc + "</b></p>");
                body.Append("<p>Calle:&nbsp;<b class='brand-danger'>" + street + "</b></p>");
                body.Append("<p>Núm. Ext:&nbsp;<b class='brand-danger'>" + outNum + "</b></p>");
                body.Append("<p>Núm. Int:&nbsp;<b class='brand-danger'>" + inNum + "</b></p>");
                body.Append("<p>Calle:&nbsp;<b class='brand-danger'>" + street + "</b></p>");
                body.Append("<p>Colonia:&nbsp;<b class='brand-danger'>" + suburb + "</b></p>");
                body.Append("<p>Municipio:&nbsp;<b class='brand-danger'>" + town + "</b></p>");
                body.Append("<p>Estado:&nbsp;<b class='brand-danger'>" + state + "</b></p>");
                body.Append("<p>CP:&nbsp;<b class='brand-danger'>" + cp + "</b></p>");
                body.Append("<br/>");

                var emailService = new Email();

                emailService.SendMail("fernanda@accentsdecoration.com", "Factura", body.ToString());

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Los datos de la factura se han enviado con exito." };
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
        public ActionResult SetStatus(int idSale, short status, string comments)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tSales.SetStatus(idSale, status, comments);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El estatus de la venta se ha actualizado con exito." };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult InsertPayment(int idSale)
        {

            List<CreditHistoryViewModel> oHistory = tSales.GetCreditHistory(idSale);

            ViewBag.idSale = idSale;

            ViewBag.leftover = tSales.GetLeftoverCreditHistory(idSale) - oHistory.Sum(p => p.Cantidad);

            return PartialView("~/Views/Sales/InsertPayment.cshtml", oHistory);

        }

        [HttpPost]
        public ActionResult SaveInsertPayment(int idSale, decimal amount, decimal left, string comments, string dtInsert, int typePayment, int? typeCard, string bank, string holder, string check, int? idCreditNote, string noIFE)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                short status = 2;

                if (idCreditNote != null)
                {
                    if (idCreditNote > 0)
                    {
                        status = 1;
                        Credits oCredits = new Credits();
                        oCredits.SetStatus((int)idCreditNote, status);
                    }
                }

                var resultPayment = tSales.InsertRecordCreditHistory(idSale, amount, left, comments, Convert.ToDateTime(dtInsert), status);

                tSales.AddTypePaymentCredit(resultPayment, typePayment, typeCard, bank, holder, check, idCreditNote);

                var oSale = tSales.GetSaleForIdSale(idSale);

                //Tipo de pago
                var sTypePayment = (typePayment == TypesPayment.iCheque ? TypesPayment.Cheque :
                        typePayment == TypesPayment.iCredito ? TypesPayment.Credito :
                        typePayment == TypesPayment.iDeposito ? TypesPayment.Deposito :
                        typePayment == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                        typePayment == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                        typePayment == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                        typePayment == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                        typePayment == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                        typePayment == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
                        typePayment == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                        "");

                var sTypeCard = "";

                if (typeCard != null)
                {
                    sTypeCard = (typeCard == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
                    typeCard == TypesCard.MasterCard ? TypesCard.sMasterCard :
                    typeCard == TypesCard.Visa ? TypesCard.sVisa : "");
                }

                var sale = _PDFGenerator.GenerateSaleWithPayment(idSale, sTypePayment, sTypeCard, check, idCreditNote, amount, left);
                var detail = tSales.GetSaleForIdSale(idSale);
                var emailService = new Email();

                emailService.SendMailWithAttachmentWithoutCC("fernanda@accentsdecoration.com", "Abono de la Venta " + detail.Remision, "Abono realizado en la venta " + detail.Remision + ".", "Venta " + detail.Remision + ".pdf", sale);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El pago se ha registrado con exito." };
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
        public ActionResult AddPayment(int idSale, decimal amount, string comments, int typePayment, string bank, string holder, string check)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                short status = 2;

                List<CreditHistoryViewModel> oHistory = tSales.GetCreditHistory(idSale);
                var left = (tSales.GetLeftoverCreditHistory(idSale) - oHistory.Sum(p => p.Cantidad)) ?? 0;

                var idHistorialCredito = tSales.InsertRecordCreditHistory(idSale, amount, left, comments, DateTime.Now, status);

                tSales.AddTypePaymentCredit(idHistorialCredito, typePayment, null, bank, holder, check, null);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { idHistorialCredito = idHistorialCredito, Message = "El pago se ha registrado con exito." };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult IndexOutProductSale(int idOutProduct, int idUserChecker, string lProducts)
        {
            Session["Controller"] = "Ventas";

            ViewBag.Seller = (UserViewModel)Session["_User"];

            Dictionary<int, int> iProducts = new Dictionary<int, int>();

            if (lProducts.Length != 0)
            {
                string[] sResult = lProducts.Split('|');

                foreach (var r in sResult)
                {
                    string[] detResult = r.Split('-');
                    iProducts.Add(Convert.ToInt32(detResult[0]), Convert.ToInt32(detResult[1]));
                }
            }

            OutProductsViewModel oProduct = tVistas.GetOutProductsForID(idOutProduct, iProducts);

            oProduct.oServicios = tVistas.GetServiceOutProducts(oProduct.idVista);

            ViewBag.IDBranch = oProduct.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)oProduct.idSucursal);

            ViewBag.IDView = idOutProduct;

            ViewBag.idUserChecker = idUserChecker;

            ViewBag.TypeCustomer = oProduct.TipoCliente;

            ViewBag.Customer = (oProduct.idClienteFisico.HasValue) ? oProduct.idClienteFisico : (oProduct.idClienteMoral.HasValue) ? oProduct.idClienteMoral : oProduct.idDespacho;

            ViewBag.Office = oProduct.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oProduct.Vendedor) ? "" : oProduct.Vendedor, String.IsNullOrEmpty(oProduct.Vendedor2) ? "" : oProduct.Vendedor2 };

            int idSale = tVistas.GetIDSalePending(idOutProduct);

            if (idSale > 0)
            {
                int status = tVistas.GetStatusidVenta(idSale);

                if (status == 1)
                {
                    CreditsController oCredit = new CreditsController();
                    Credits oCreditcs = new Credits();
                    List<CreditDetailViewModel> arrayProducts = new List<CreditDetailViewModel>();
                    string remision = oCreditcs.GetCreditNoteForIdventa(idSale);

                    string credit = string.Empty;
                    if (string.IsNullOrEmpty(remision))
                    {
                        credit = oCredit.SaveAddCredit((int)CreditNoteType.FleteSalidaVista, idSale, (int)oProduct.idUsuario1, 1000.00M, DateTime.Now.ToString("dd/MM/yyyy"), "Salida a Vista", arrayProducts);
                    }
                    else
                    {
                        credit = remision;
                    }
                    ViewBag.Credit = credit;
                }

            }

            return View(oProduct);

        }

        [HttpPost]
        public ActionResult SetStatusCredit(int idCredit, short status, string dateModify, int? idCuenta, string voucher)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (String.IsNullOrEmpty(dateModify))
                {
                    dateModify = DateTime.Now.ToString();
                }

                tSales.SetStatusCreditHistory(idCredit, status, Convert.ToDateTime(dateModify), idCuenta, voucher);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El estatus del pago se ha actualizado con exito." };

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
        public ActionResult GetDatasBill(string rfc)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { DataBill = tSales.GetBill(rfc) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public ActionResult PrintDeposit(string remision, string deposit)
        {

            var detail = tSales.GetSaleToCredit(remision);

            List<CreditHistoryViewModel> oHistory = tSales.GetCreditHistory(detail.idVenta);

            ViewData.Add("remaining", Convert.ToDecimal(tSales.GetLeftoverCreditHistory(detail.idVenta) - oHistory.Sum(p => p.Cantidad)).ToString("C", CultureInfo.CurrentCulture));

            ViewData.Add("deposit", Convert.ToDecimal(deposit).ToString("C", CultureInfo.CurrentCulture));

            ViewData.Add("user", (UserViewModel)Session["_User"]);

            return PartialView(detail);

        }
        public ActionResult PrintDepositOnline(int idSale, string deposit)
        {

            var detail = tSales.GetSaleToCredit(idSale);

            List<CreditHistoryViewModel> oHistory = tSales.GetCreditHistory(detail.idVenta);

            ViewData.Add("remaining", tSales.GetLeftoverCreditHistory(detail.idVenta) - oHistory.Sum(p => p.Cantidad));

            ViewData.Add("deposit", Convert.ToDecimal(deposit));

            ViewData.Add("user", (UserViewModel)Session["_User"]);

            return PartialView("~/Views/Sales/PrintDeposit.cshtml", detail);

        }

        [HttpPost]
        public ActionResult BillSale(string remision)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tSales.BillSale(remision);

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
        public ActionResult SetNumberBill(int idSale, string numberBill)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tSales.SetNumberBill(idSale, numberBill);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se asignó número de factura." };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public ActionResult OpenAddTax(int idSale)
        {

            SaleViewModel oSale = tSales.GetSaleForIdSale(idSale);
            ViewBag.idSale = idSale;
            ViewBag.subtotal = oSale.Subtotal;
            ViewBag.descuento = oSale.Descuento;
            ViewBag.IVA = oSale.IVA;

            return PartialView("~/Views/Sales/AddTax.cshtml");

        }

        [HttpPost]
        public ActionResult AddTax(int idSale, decimal amount, short typePayment, short? typeCard, string bank, string holder, string check, string noIFE)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                SaleViewModel oSale = tSales.GetSaleForIdSale(idSale);

                TypePaymentViewModel oPayment = new TypePaymentViewModel()
                {

                    amount = amount,
                    typesPayment = typePayment,
                    typesCard = typeCard,
                    bank = bank,
                    holder = holder,
                    numCheck = check,
                    numIFE = noIFE,
                    dateMaxPayment = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture)

                };

                List<TypePaymentViewModel> lPayments = new List<TypePaymentViewModel>();

                lPayments.Add(oPayment);

                tSales.AddTypePayment(idSale, lPayments);

                tSales.BillSale(oSale.Remision);

                tSales.UpdateTotalSale(oSale.Remision, amount);

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
        public ActionResult SaveAddTaxPayment(int idSale, int idPayment, decimal amount, decimal amountIVA)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tSales.UpdateTotalSale(idSale, amountIVA);

                tSales.AddIVATypePayment(idPayment, amount, amountIVA);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se agregó el IVA con éxito." };

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
        public ActionResult SaveAddTaxPaymentCredit(int idSale, int idCreditHistory, decimal amount, decimal amountIVA)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tSales.UpdateTotalSale(idSale, amountIVA);

                tSales.AddIVATypePaymentCredit(idCreditHistory, amount, amountIVA);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se agregó el IVA con éxito." };

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
        public ActionResult ModifyPaymentWay(int idSale, List<TypePaymentViewModel> lTypePayment)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tSales.DeleteCreditSale(idSale);

                tSales.AddTypePayment(idSale, lTypePayment);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se registró el cambio con exito." };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public void SendMailSale(int idSale)
        {
            var sale = _PDFGenerator.GenerateSale(idSale);
            var saleDetail = tSales.GetSaleForIdSale(idSale);
            var emailService = new Email();

            string email;

            if (saleDetail.idClienteFisico > 0)
            {
                email = tPhysicalCustomers.GetPhysicalCustomer((int)saleDetail.idClienteFisico).Correo;
            }
            else if (saleDetail.idClienteMoral > 0)
            {
                email = tMoralCustomers.GetMoralCustomer((int)saleDetail.idClienteMoral).Correo;
            }
            else
            {
                email = tOffices.GetOffice((int)saleDetail.idDespacho).Correo;
            }

            if (email.Trim().ToLower() != "noreply@correo.com")
            {
                emailService.SendMailWithAttachment(email, "Venta " + saleDetail.Remision, "Gracias por su compra.", "Venta_" + saleDetail.Remision + ".pdf", sale);
            }
            else
            {
                emailService.SendInternalMailWithAttachment("Venta " + saleDetail.Remision, "Gracias por su compra.", "Venta_" + saleDetail.Remision + ".pdf", sale);
            }
        }

        [HttpPost]
        public ActionResult SendMailSaleAgain(int idSale, string email)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var sale = _PDFGenerator.GenerateSale(idSale);
                var saleDetail = tSales.GetSaleForIdSale(idSale);
                var emailService = new Email();

                if (email.Trim().ToLower() != "noreply@correo.com")
                {
                    emailService.SendMailWithAttachment(email, "Venta " + saleDetail.Remision, "Gracias por su compra.", "Venta" + saleDetail.Remision + ".pdf", sale);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El correo se ha enviado." };
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
        public ActionResult SendMailSaleFromList(int idSale)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                this.SendMailSale(idSale);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se envió el correo con exito") };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult SaveDatePaymentDetail(int idTypePayment, short status, string date, int? idcuenta, string voucher)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                if (String.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString();
                }

                tSales.SaveDatePaymentDetail(idTypePayment, status, Convert.ToDateTime(date), idcuenta, voucher);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó la información de forma de pago") };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult EditSale(int idSale)
        {
            SaleViewModel oSale = this.tSales.GetSaleForIdSale(idSale);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1, String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2 };

            //Se valida que la venta no este siendo editada
            if (!oSale.Editar)
            {
                //Se indica que la venta està siendo validada
                this.tSales.SaleIsEditing(idSale);

                //Se elimina forma de pago
                this.tSales.DeletePaymentWayOfSale(idSale);

                //Se realiza devolución de inventario
                this.tSales.ReturnProducts(idSale, (int)oSale.idSucursal, (int)Session["_ID"]);
            }

            return View(oSale);
        }

        public ActionResult EditUnifySale(int idSale)
        {
            SaleViewModel oSale = this.tSales.GetSaleForIdSale(idSale);

            ViewBag.TypeCustomer = oSale.TipoCliente;
            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;
            ViewBag.Office = oSale.idDespachoReferencia;
            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1, String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2 };

            //Se valida que la venta no este siendo editada
            if (!oSale.Editar)
            {
                //Se indica que la venta està siendo validada
                this.tSales.SaleIsEditing(idSale);
                //Se elimina forma de pago
                this.tSales.DeletePaymentWayOfSale(idSale);
                //Se realiza devolución de inventario
                this.tSales.ReturnProductsUnifySale(idSale, (int)Session["_ID"]);
            }

            return View(oSale);
        }

        [HttpPost]
        public ActionResult SaveEditSale(int idSale, int idBranch, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference,
            int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<TypePaymentViewModel> lTypePayment)
        {
            var jsonMessenger = new JsonMessenger();

            try
            {

                if (lProducts.Any(p => p.cantidad <= 0))
                {
                    throw new Exception("No se permiten cantidades iguales o menores a cero");
                }

                SaleViewModel oSale = new SaleViewModel()
                {
                    idVenta = idSale,
                    idSucursal = idBranch,
                    idUsuario1 = idUser1,
                    idUsuario2 = idUser2,
                    idClienteFisico = idCustomerP,
                    idClienteMoral = idCustomerM,
                    idDespacho = idOffice,
                    idDespachoReferencia = idOfficeReference,
                    Proyecto = project,
                    TipoCliente = typeCustomer,
                    CantidadProductos = amountProducts,
                    Subtotal = subtotal,
                    Descuento = discount,
                    Total = total,
                    IVA = IVA
                };

                this.tSales.SaveEditSale(oSale, lProducts);

                this.tSales.AddTypePayment(idSale, lTypePayment);

                this.tSales.SaleIsEditing(idSale);

                jsonMessenger.success = 1;
                jsonMessenger.failure = 0;
                jsonMessenger.oData = (object)new
                {
                    Message = string.Format(this.Message.msgAdd = "Se actualizó la información de la venta")
                };
            }
            catch (Exception ex)
            {
                jsonMessenger.success = 0;
                jsonMessenger.failure = 1;
                jsonMessenger.oData = (object)new { Error = ex.Message };
            }
            return (ActionResult)this.Json((object)jsonMessenger);
        }

        public ActionResult EditHeaderSale(int idSale)
        {
            SaleViewModel oSale = this.tSales.GetSaleForIdSale(idSale);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1, String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2 };

            return View(oSale);
        }

        [HttpPost]
        public ActionResult SaveEditHeaderSale(SaleViewModel oSale)
        {

            var jsonMessenger = new JsonMessenger();

            try
            {
                this.tSales.SaveEditSale(oSale);
                this.tSales.SaveEditDetailSale(oSale.oDetail);

                jsonMessenger.success = 1;
                jsonMessenger.failure = 0;
                jsonMessenger.oData = (object)new
                {
                    Message = string.Format(this.Message.msgAdd = "Se actualizó la información de la venta")
                };
            }
            catch (Exception ex)
            {
                jsonMessenger.success = 0;
                jsonMessenger.failure = 1;
                jsonMessenger.oData = (object)new { Error = ex.Message };
            }
            return (ActionResult)this.Json((object)jsonMessenger);
        }

        [HttpPost]
        public ActionResult GetAccounts()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Accounts = tSales.GetAccounts() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }
            return Json(jmResult);
        }

        [HttpGet]
        public ActionResult GetPendingSales()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tSales.GetPendingSales() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /*Comienza codigo de la venta de las salidas unificadas*/
        public ActionResult IndexOutUnifyProductSale(string Products)
        {
            List<OutProductsDetailViewModel> productList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OutProductsDetailViewModel>>(Products);

            Session["Controller"] = "Ventas";
            ViewBag.Seller = (UserViewModel)Session["_User"];

            OutUnifyProductsModel oProductsUnify = new OutUnifyProductsModel();
            OutProductsViewModel oProduct = new OutProductsViewModel();
            List<OutProductsViewModel> LOutProductsViewModel = new List<OutProductsViewModel>();
            int cont = 1;
            Dictionary<int, int> iProducts = new Dictionary<int, int>();
            foreach (var view in productList.GroupBy(x => x.idVista).ToList())
            {
                iProducts.Clear();
                foreach (OutProductsDetailViewModel prod in productList.Where(y => y.idVista == view.Key))
                {
                    iProducts.Add((int)prod.idProducto, (int)prod.Venta);
                }

                oProduct = tVistas.GetOutProductsForID(view.Key, iProducts);
                oProduct.oServicios = tVistas.GetServiceOutProducts(oProduct.idVista);
                if (cont == 1)
                {
                    oProductsUnify.idDespachoReferencia = oProduct.idDespachoReferencia;
                    oProductsUnify.TipoCliente = oProduct.TipoCliente;
                    oProductsUnify.Customer = (oProduct.idClienteFisico.HasValue) ? oProduct.idClienteFisico : (oProduct.idClienteMoral.HasValue) ? oProduct.idClienteMoral : oProduct.idDespacho;
                    oProductsUnify.Vendedor = String.IsNullOrEmpty(oProduct.Vendedor) ? "" : oProduct.Vendedor;
                    oProductsUnify.Vendedor2 = String.IsNullOrEmpty(oProduct.Vendedor2) ? "" : oProduct.Vendedor2;
                    oProductsUnify.idUsuario = productList[0].idUsuario;
                    oProductsUnify.Proyecto = oProduct.Proyecto;
                }
                int idSale = tVistas.GetIDSalePending(view.Key);
                if (idSale > 0)
                {
                    int status = tVistas.GetStatusidVenta(idSale);

                    if (status == 1)
                    {
                        CreditsController oCredit = new CreditsController();
                        Credits oCreditcs = new Credits();
                        List<CreditDetailViewModel> arrayProducts = new List<CreditDetailViewModel>();
                        string remision = oCreditcs.GetCreditNoteForIdventa(idSale);

                        string credit = string.Empty;
                        if (string.IsNullOrEmpty(remision))
                        {
                            credit = oCredit.SaveAddCredit((int)CreditNoteType.FleteSalidaVista, idSale, (int)oProduct.idUsuario1, 1000.00M, DateTime.Now.ToString("dd/MM/yyyy"), "Salida a Vista", arrayProducts);
                        }
                        else
                        {
                            credit = remision;
                        }
                        oProduct.RemisionVenta = credit;
                    }

                }
                LOutProductsViewModel.Add(oProduct);
                cont++;
            }

            oProductsUnify.oDetail = LOutProductsViewModel;
            return View(oProductsUnify);
        }

        [HttpPost]
        public ActionResult GeneratePrevNumberRemC()
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sRemision = tSales.GeneratePrevNumberRemC() };

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
        public ActionResult SaveSaleUnify(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<ProductSaleViewModel> lProductsOut,
            List<TypePaymentViewModel> lTypePayment, int? idQuotation)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (lProducts.Any(p => p.cantidad <= 0))
                {
                    throw new Exception("No se permiten cantidades iguales o menores a cero");
                }

                int idSale = 0;

                tProducts.ValidateProductToSaleAvoidNegatives(lProducts);

                string rem = tSales.AddUnifySale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, (int)OriginCurrency.Unified, out idSale);

                tSales.AddTypePayment(idSale, lTypePayment);

                foreach (ProductSaleViewModel producto in lProducts)
                {
                    if (producto.idVista.HasValue)
                    {
                        if (producto.idVista > 0)
                        {
                            if (idQuotation.HasValue)
                            {
                                if (idQuotation > 0)
                                {
                                    lProductsOut = tVistas.GetOutProductsDetails((int)producto.idVista).Select(p => new ProductSaleViewModel()
                                    {
                                        idVista = p.idVista,
                                        idProducto = p.idProducto,
                                        codigo = p.Codigo,
                                        cantidad = p.Cantidad
                                    }).ToList();
                                }
                            }

                            var lProd = new string[lProducts.Where(y => y.idProducto == producto.idProducto && y.idVista == producto.idVista).Count(), 3];
                            int aux = 0;
                            foreach (var product in lProductsOut.Where(x => x.idVista == producto.idVista && x.idProducto == producto.idProducto))
                            {
                                ProductSaleViewModel prod = lProducts.FirstOrDefault(p => p.idProducto == product.idProducto && p.idVista == product.idVista);

                                var pendingProd = tVistas.GetPendingProduct((int)producto.idVista, (int)producto.idProducto);

                                //Si la venta es igual a lo pendiente en la Salida a Vista
                                if (pendingProd.Pendiente == producto.cantidad)
                                {
                                    lProd[aux, 0] = producto.idProducto.ToString();
                                    lProd[aux, 1] = producto.cantidad.ToString();
                                    lProd[aux, 2] = producto.idVista.ToString();
                                }
                                //Si la venta es menor a lo seleccionado en la Salida a Vista
                                else if (pendingProd.Pendiente > producto.cantidad)
                                {
                                    lProd[aux, 0] = producto.idProducto.ToString();
                                    lProd[aux, 1] = producto.cantidad.ToString();
                                    lProd[aux, 2] = producto.idVista.ToString();
                                }
                                //Si la venta y lo seleccionado en la Salida a Vista es igual
                                else
                                {
                                    if ((pendingProd.Pendiente < producto.cantidad) && (pendingProd.Pendiente > product.cantidad))
                                    {
                                        lProd[aux, 0] = product.idProducto.ToString();
                                        lProd[aux, 1] = pendingProd.Pendiente.ToString();
                                        lProd[aux, 2] = product.idVista.ToString();
                                    }
                                    else
                                    {
                                        lProd[aux, 0] = product.idProducto.ToString();
                                        lProd[aux, 1] = pendingProd.Pendiente.ToString();
                                        lProd[aux, 2] = product.idVista.ToString();
                                    }
                                }
                                aux++;
                            }

                            OutProducts oOutProducts = new OutProducts();

                            oOutProducts.UpdateStockSaleOutProducts(rem, lProd, (short)idUser1);

                            tVistas.UpdateFleteOutProduct(int.Parse(producto.idVista.ToString()));

                            tVistas.UpdateSaleFromView(int.Parse(producto.idVista.ToString()), idSale);
                        }
                    }
                }


                if (idQuotation.HasValue)
                {
                    if (idQuotation > 0)
                    {
                        QuotationsController oQuotations = new QuotationsController();

                        oQuotations.DeleteQuotation((int)idQuotation);
                    }
                }

                this.SendMailSale(idSale);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se registró la venta con exito"), sRemision = rem };

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
        public ActionResult GetSalesForChart()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Sales = tSales.GetSalesForChart() };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        //IndexAllStores
        public ActionResult IndexAllStores()
        {
            Session["Controller"] = "Ventas";

            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        //SaveSaleFromAllStores
        [HttpPost]
        public ActionResult SaveSaleAllStore(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, List<TypePaymentViewModel> lTypePayment)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (lProducts.Any(p => p.cantidad <= 0))
                {
                    throw new Exception("No se permiten cantidades iguales o menores a cero");
                }

                int idSale = 0;

                tProducts.ValidateProductToSaleAvoidNegatives(lProducts);

                string rem = tSales.AddUnifySale(idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, (int)OriginCurrency.Unified, out idSale);

                tSales.AddTypePayment(idSale, lTypePayment);

                this.SendMailSale(idSale);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se registró la venta con exito"), sRemision = rem };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        [HttpGet]
        public ActionResult PaymentDetailSale(string remision)
        {
            SaleViewModel oSale = tSales.GetSaleForRemision(remision);
            return PartialView("~/Views/Sales/CreditNoteSale.cshtml", oSale);
        }

        //GetPayment
        [HttpGet]
        public ActionResult GetPaymentsBySale(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Payments = tSales.GetPaymentBySale(id)
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
    }
}