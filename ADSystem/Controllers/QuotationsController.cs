using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using ADEntities.Queries;
using System.Diagnostics;
using System.IO;
using Pechkin;
using System.Web.Hosting;
using ADEntities.Enums;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class QuotationsController : BaseController
    {
        PDFGenerator _PDFGenerator = new PDFGenerator();

        public override ActionResult Index()
        {
            Session["Controller"] = "Cotizaciones";

            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);

            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));

            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        public ActionResult ListQuotations()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetQuotations(bool allTime,string dtDateSince, string dtDateUntil, string number, string costumer, int? iduser, string project, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Quotations = tQuotations.GetQuotations(allTime,Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil),number, costumer, iduser, project, idUser, restricted, amazonas, guadalquivir, textura, page, pageSize), Count = tQuotations.CountRegisters(number, costumer, iduser, project, idUser, restricted, amazonas, guadalquivir, textura) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ViewResult GetQuotation(int idQuotation)
        {
            QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

            ViewBag.IDBranch = oSale.idSucursal;

            ViewBag.IDView = oSale.idVista;

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View("~/Views/Sales/GetQuotation.cshtml", oSale);
        }

        public ViewResult GetQuotationFromOutProductToSale(int idQuotation)
        {
            QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

            ViewBag.IDBranch = oSale.idSucursal;

            ViewBag.IDView = oSale.idVista;

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View("~/Views/Sales/GetQuotationOutProduct.cshtml", oSale);
        }

        #region Dollar
        public ActionResult Dollar()
        {
            Session["Controller"] = "Cotizaciones";

            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);

            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));

            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        public ViewResult GetQuotationInDollar(int idQuotation)
        {

            QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

            ViewBag.IDBranch = oSale.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View("~/Views/Sales/GetQuotationInDollar.cshtml", oSale);
        }

        [HttpPost]
        public ActionResult SaveQuotationInDollar(string number, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, bool send)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idQuotation = tQuotations.AddQuotationDollar(number, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, comments);

                if (send)
                {
                    this.SendMailQuotationSale(idQuotation);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la cotización con exito"), idQuotation = idQuotation };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ViewResult GetEraserQuotationDollar(int idQuotation)
        {
            QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

            ViewBag.IDBranch = oSale.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View(oSale);
        }

        [HttpPost]
        public ActionResult SaveUpdateEraserQuotationInDollar(int idQuotation, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
           string dateSale, int amountProducts, decimal subtotal, decimal discount, decimal total, List<ProductSaleViewModel> lProducts, string comments)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tQuotations.UpdateQuotationDollar(idQuotation, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, total, lProducts, comments);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó en borrador la cotización con exito"), idQuotation = idQuotation };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }
        #endregion

        public ViewResult GetEraserQuotation(int idQuotation)
        {
            QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

            ViewBag.IDBranch = oSale.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View(oSale);
        }

        [HttpPost]
        public ActionResult SaveQuotation(string number, int idUser1, int? idUser2, int? origin, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, bool send, decimal IVATasa)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idQuotation = tQuotations.AddQuotation(number, idUser1, idUser2, origin, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, comments, IVATasa);

                if (send)
                {
                    this.SendMailQuotationSale(idQuotation);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la cotización con exito"), idQuotation = idQuotation };
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
        public ActionResult SaveUpdateEraserQuotation(int idQuotation, int idUser1, int? idUser2, int? origin, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
           string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, decimal IVATasa)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tQuotations.UpdateQuotationProductSale(idQuotation, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, comments, IVATasa);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó en borrador la cotización con exito") };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ViewResult GetEraserQuotationView(int idQuotation)
        {
            QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.idVista = oSale.idVista;

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View(oSale);
        }

        [HttpPost]
        public ActionResult SaveQuotationFromView(string number, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal IVATasa, decimal total, List<ProductSaleViewModel> lProducts, string comments, bool send, int? idView)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idQuotation = tQuotations.AddQuotationFromView(number, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, IVATasa, total, lProducts, comments, idView);

                if (send)
                {
                    this.SendMailQuotationSale(idQuotation);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la cotización con exito"), idQuotation = idQuotation };
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
        public ActionResult SaveEraserQuotationFromView(int idQuotation, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, int idView, decimal IVATasa)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tQuotations.UpdateQuotationFromView(idQuotation, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, comments, idView, IVATasa);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la cotización con exito"), idQuotation = idQuotation };
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
        public ActionResult SaveQuotationFromUnifiedView(string number, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, bool send, decimal IVATasa)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idQuotation = tQuotations.AddQuotationFromUnifiedView(number, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, comments, IVATasa);

                if (send)
                {
                    this.SendMailQuotationSale(idQuotation);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la cotización con exito"), idQuotation = idQuotation };
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
        public ActionResult SaveEraserQuotationFromUnifiedView(int idQuotation, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, decimal IVATasa)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tQuotations.UpdateQuotationFromUnifiedView(idQuotation, idUser1, idUser2, idCustomerP, idCustomerM, idOffice, project, typeCustomer, idOfficeReference, idBranch, dateSale, amountProducts, subtotal, discount, IVA, total, lProducts, comments, IVATasa);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la cotización con exito"), idQuotation = idQuotation };
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
        public ActionResult DeleteQuotation(int idQuotation)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tQuotations.DeleteQuotation(idQuotation);

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

        public ActionResult PrintQuotation(int idQuotation)
        {
            QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

            var branch = tBranches.GetBranch(oSale.idSucursal ?? 0);
            if (branch != null)
                oSale.IVATotal = oSale.Subtotal * (branch.IVATasa / 100);

            ViewBag.Descuento = Convert.ToInt16(oSale.oDetail.Exists(p => (p.Descuento > 0)));

            return PartialView(oSale);
        }

        public FileContentResult PrintPDFQuotation(int idQuotation)
        {
            try
            {
                QuotationViewModel oSale = tQuotations.GetQuotationId(idQuotation);

                string bodyTemplate = String.Empty;
                StringBuilder body = new StringBuilder();

                using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Views/Quotations/TemplateQuotation.html")))
                {
                    bodyTemplate = reader.ReadToEnd();
                }

                bodyTemplate = bodyTemplate.Replace("[BODY]", body.ToString());

                byte[] file = new SimplePechkin(new GlobalConfig()).Convert(System.Text.Encoding.UTF8.GetBytes(body.ToString()));

                return File(file, "application/pdf", "myPDF.pdf");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void SendMailQuotationSale(int idQuotation)
        {
            QuotationViewModel _quotation = tQuotations.GetQuotationId(idQuotation);
            var quotation = _PDFGenerator.GenerateQuotation(idQuotation);
            var emailService = new Email();

            string email;

            if (_quotation.idClienteFisico > 0)
            {
                email = tPhysicalCustomers.GetPhysicalCustomer((int)_quotation.idClienteFisico).Correo;
            }
            else if (_quotation.idClienteMoral > 0)
            {
                email = tMoralCustomers.GetMoralCustomer((int)_quotation.idClienteMoral).Correo;
            }
            else
            {
                email = tOffices.GetOffice((int)_quotation.idDespacho).Correo;
            }

            if (email.Trim().ToLower() != "noreply@correo.com")
            {
                emailService.SendMailWithAttachment(email, "Cotización " + _quotation.Numero, "Cotización " + _quotation.Numero, "Cotizacion_" + _quotation.Numero + ".pdf", quotation);
			}
            else
            {
                emailService.SendInternalMailWithAttachment("Cotización " + _quotation.Numero, "Cotización " + _quotation.Numero, "Cotizacion_" + _quotation.Numero + ".pdf", quotation);
            }
        }

        [HttpPost]
        public ActionResult SendMailQuotationSaleAgain(int idQuotation, string email)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                QuotationViewModel _quotation = tQuotations.GetQuotationId(idQuotation);
                var quotation = _PDFGenerator.GenerateQuotation(idQuotation);
                var emailService = new Email();

                if (email.Trim().ToLower() != "noreply@correo.com")
                {
                    emailService.SendMailWithAttachment(email, "Cotización " + _quotation.Numero, "Cotización " + _quotation.Numero, "Cotizacion_" + _quotation.Numero + ".pdf", quotation);
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

        public ActionResult CreateQuotationFromOutProduct(string remision)
        {
            OutProductsViewModel outProduct = tVistas.GetOutProductsPending(remision);

            outProduct.oDetail = outProduct.oDetail.FindAll(p => p.Cantidad > 0);

            var branch = tBranches.GetBranch(outProduct.idSucursal ?? 0);            

            if (branch != null)
                ViewBag.IVATasa = branch.IVATasa;

            ViewBag.IDBranch = outProduct.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)outProduct.idSucursal);

            ViewBag.TypeCustomer = outProduct.TipoCliente;

            ViewBag.IDView = outProduct.idVista;

            ViewBag.Customer = (outProduct.idClienteFisico.HasValue) ? outProduct.idClienteFisico : (outProduct.idClienteMoral.HasValue) ? outProduct.idClienteMoral : outProduct.idDespacho;

            ViewBag.Office = outProduct.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(outProduct.Vendedor) ? "" : outProduct.Vendedor, String.IsNullOrEmpty(outProduct.Vendedor2) ? "" : outProduct.Vendedor2 };

            return View(outProduct);
        }

        [HttpPost]
        public ActionResult GeneratePrevNumberRem()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sNumber = tQuotations.GeneratePrevNumberRem() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        //Comienza codigo de generar cotización de salidas a vistas unificadas
        public ActionResult CreateQuotationFromOutUnifyProduct(string remissions)
        {
            var spRemision = remissions.Split(',');
            List<string> LiRemision = new List<string>(spRemision);
            int i = 0;
            List<OutProductsViewModel> LProducts = tVistas.GetMultiOutProductsPending(LiRemision);
            List<OutProductsViewModel> LOutProductsViewModel = new List<OutProductsViewModel>();
            OutUnifyProductsModel oProductsUnify = new OutUnifyProductsModel();

            foreach (OutProductsViewModel oProduct in LProducts)
            {

                oProduct.oDetail = oProduct.oDetail.FindAll(p => p.Cantidad > 0);

                if (i == 0)
                {

                    oProductsUnify.idDespachoReferencia = oProduct.idDespachoReferencia;
                    oProductsUnify.TipoCliente = oProduct.TipoCliente;
                    oProductsUnify.Customer = (oProduct.idClienteFisico.HasValue) ? oProduct.idClienteFisico : (oProduct.idClienteMoral.HasValue) ? oProduct.idClienteMoral : oProduct.idDespacho;
                    oProductsUnify.Vendedor = String.IsNullOrEmpty(oProduct.Vendedor) ? "" : oProduct.Vendedor;
                    oProductsUnify.Vendedor2 = String.IsNullOrEmpty(oProduct.Vendedor2) ? "" : oProduct.Vendedor2;
                    oProductsUnify.Proyecto = oProduct.Proyecto;
                }
                i++;
                LOutProductsViewModel.Add(oProduct);

            }
            oProductsUnify.oDetail = LOutProductsViewModel;
            return View(oProductsUnify);
        }

        public ViewResult GetEraserQuotationUnify(int idQuotation)
        {
            UnifiedQuotationViewModel UnifySale = new UnifiedQuotationViewModel();
            List<QuotationViewModel> listDetail = new List<QuotationViewModel>();
            QuotationViewModel oQuotation = tQuotations.GetQuotationId(idQuotation);

            UnifySale.idCotizacion = oQuotation.idCotizacion;
            UnifySale.TipoCliente = oQuotation.TipoCliente;
            UnifySale.Customer = (oQuotation.idClienteFisico.HasValue) ? oQuotation.idClienteFisico : (oQuotation.idClienteMoral.HasValue) ? oQuotation.idClienteMoral : oQuotation.idDespacho;
            UnifySale.idDespachoReferencia = oQuotation.idDespachoReferencia;
            UnifySale.Vendedor = String.IsNullOrEmpty(oQuotation.Usuario1) ? "" : oQuotation.Usuario1.Trim();
            UnifySale.Vendedor2 = String.IsNullOrEmpty(oQuotation.Usuario2) ? "" : oQuotation.Usuario2.Trim();
            UnifySale.Comentarios = oQuotation.Comentarios;
            UnifySale.Proyecto = oQuotation.Proyecto;
            UnifySale.Descuento = oQuotation.Descuento;
            UnifySale.IVA = oQuotation.IVA;
            listDetail.Add(oQuotation);
            UnifySale.oDetail = listDetail;

            return View(UnifySale);
        }

        public ViewResult GetQuotationUnify(int idQuotation)
        {
            UnifiedQuotationViewModel UnifyQuotation = new UnifiedQuotationViewModel();
            List<QuotationViewModel> listQuotations = new List<QuotationViewModel>();
            QuotationViewModel oQuotation = tQuotations.GetQuotationId(idQuotation);

            UnifyQuotation.idCotizacion = oQuotation.idCotizacion;
            UnifyQuotation.TipoCliente = oQuotation.TipoCliente;
            UnifyQuotation.Customer = (oQuotation.idClienteFisico.HasValue) ? oQuotation.idClienteFisico : (oQuotation.idClienteMoral.HasValue) ? oQuotation.idClienteMoral : oQuotation.idDespacho;
            UnifyQuotation.idDespachoReferencia = oQuotation.idDespachoReferencia;
            UnifyQuotation.Vendedor = String.IsNullOrEmpty(oQuotation.Usuario1) ? "" : oQuotation.Usuario1.Trim();
            UnifyQuotation.Vendedor2 = String.IsNullOrEmpty(oQuotation.Usuario2) ? "" : oQuotation.Usuario2.Trim();
            UnifyQuotation.Comentarios = oQuotation.Comentarios;
            UnifyQuotation.Proyecto = oQuotation.Proyecto;
            UnifyQuotation.Descuento = oQuotation.Descuento;
            UnifyQuotation.IVA = oQuotation.IVA;
            UnifyQuotation.idUsuario = oQuotation.idUsuario1;
            List<DetailQuotationViewModel> listCredit = new List<DetailQuotationViewModel>();

            foreach (DetailQuotationViewModel oDetail in oQuotation.oDetail)
            {
                if (oDetail.idProducto != null && oDetail.idVista != null)
                {
                    int id = tVistas.GetIDSalePending((int)oDetail.idVista);
                    if (id > 0)
                    {
                        int status = tVistas.GetStatusidVenta(id);

                        if (status == 1)
                        {
                            CreditsController oCredit = new CreditsController();
                            Credits oCreditcs = new Credits();
                            List<CreditDetailViewModel> arrayProducts = new List<CreditDetailViewModel>();
                            string remision = oCreditcs.GetCreditNoteForIdventa(id);

                            if (string.IsNullOrEmpty(remision))
                            {
                                remision = oCredit.SaveAddCredit((int)CreditNoteType.FleteSalidaVista, id, (int)oQuotation.idUsuario1, 1000.00M, DateTime.Now.ToString("dd/MM/yyyy"), "Salida a Vista", arrayProducts);
                            }

                            var creditNote = tCredits.GetCreditNote(remision);

                            listCredit.Add(new DetailQuotationViewModel()
                            {
                                idCredito = creditNote.idNotaCredito,
                                Credito = creditNote.RemisionCredito,
                                Precio = -creditNote.Cantidad,
                                Comentarios = creditNote.Comentarios
                            });
                        }

                    }
                }
            }

            oQuotation.oDetail.AddRange(listCredit);
            listQuotations.Add(oQuotation);
            UnifyQuotation.oDetail = listQuotations;

            return View("~/Views/Sales/GetQuotationUnify.cshtml", UnifyQuotation);
        }

        [HttpPost]
        public ActionResult GetBranchInfo(int IDBranch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { IVATasa = tQuotations.GetBranchInfo(IDBranch) };
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
        public ActionResult CreateDuplicateQuotation(int idQuotation)
        {
            var quotation = tQuotations.DuplicateQuotation(idQuotation);

            QuotationViewModel oSale = tQuotations.GetQuotationId(quotation);

            ViewBag.IDBranch = oSale.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)oSale.idSucursal);

            ViewBag.TypeCustomer = oSale.TipoCliente;

            ViewBag.Customer = (oSale.idClienteFisico.HasValue) ? oSale.idClienteFisico : (oSale.idClienteMoral.HasValue) ? oSale.idClienteMoral : oSale.idDespacho;

            ViewBag.Office = oSale.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oSale.Usuario1) ? "" : oSale.Usuario1.Trim(), String.IsNullOrEmpty(oSale.Usuario2) ? "" : oSale.Usuario2.Trim() };

            return View("GetEraserQuotation", oSale);
        }
    }
}