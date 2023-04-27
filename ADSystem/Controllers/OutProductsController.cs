using ADEntities.Models;
using ADEntities.Queries;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class OutProductsController : BaseController
    {
        PDFGenerator _PDFGenerator = new PDFGenerator();

        // GET: Sale
        public override ActionResult Index()
        {
            Session["Controller"] = "SALIDAS A VISTA";

            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);

            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));

            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        public ActionResult GetCustomer(string name)
        {
            return Json(new { Customers = tMoralCustomers.GetCustomers(name) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveAddOutProducts(int? idEraserView, int? idClienteFisico, int? idClienteMoral, int? idOffice, string project, byte typeCustomer, int? idOfficeReference,
                                               int idSucursal, DateTime Fecha, int CantidadProductos,
                                               decimal Subtotal, decimal Total, short? idVendedor,
                                               short? idVendedor2, int? idVerificador, List<ShoppingCartViewModel> Items, bool freight)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var listaArticulos = new List<ShoppingCartViewModel>();

                UserViewModel uvmUser = (UserViewModel)Session["_User"];

                //Verificar ultima Salida a Vista
                if (!tVistas.VerifyOutProducts(uvmUser.idUsuario, DateTime.Now))
                {

                    tVista oVista = new tVista();

                    List<tDetalleVista> listaDetalleVista = new List<tDetalleVista>();

                    List<ServiceOutProductViewModel> lServices = new List<ServiceOutProductViewModel>();

                    oVista.Remision = "";
                    oVista.idUsuario1 = uvmUser.idUsuario;
                    oVista.idClienteFisico = idClienteFisico;
                    oVista.idClienteMoral = idClienteMoral;
                    oVista.idDespacho = idOffice;
                    oVista.Proyecto = project;
                    oVista.idDespachoReferencia = idOfficeReference;
                    oVista.TipoCliente = typeCustomer;
                    oVista.idSucursal = idSucursal;
                    oVista.Fecha = Fecha;
                    oVista.CantidadProductos = CantidadProductos;
                    oVista.Subtotal = Subtotal;
                    oVista.Total = Total;
                    oVista.Estatus = TypesOutProducts.Pendiente;
                    oVista.idUsuario1 = idVendedor;
                    oVista.idUsuario2 = idVendedor2;
                    oVista.idVerificador = idVerificador;

                    //Cargamos el listado de articulos a salir
                    foreach (var item in Items)
                    {
                        if (item.servicio == false)
                        {
                            tDetalleVista oDetalleVista = new tDetalleVista();

                            oDetalleVista.Cantidad = item.cantidad;
                            oDetalleVista.idProducto = item.idProducto;
                            oDetalleVista.idSucursal = idSucursal;
                            oDetalleVista.Precio = item.prec;
                            oDetalleVista.Estatus = TypesOutProductsDetail.Pendiente;
                            oDetalleVista.Comentarios = item.comentarios;

                            listaDetalleVista.Add(oDetalleVista);
                        }
                        else
                        {
                            ServiceOutProductViewModel oService = new ServiceOutProductViewModel();

                            oService.idServicio = item.idProducto;
                            oService.Precio = item.prec;
                            oService.Cantidad = item.cantidad;
                            oService.Comentarios = item.comentarios;

                            lServices.Add(oService);
                        }
                    }

                    oVista.tDetalleVistas = listaDetalleVista;

                    string rem = String.Empty;

                    //Agregamos servicios
                    int idView = tVistas.AddOutProducts(oVista, out rem);

                    if (idView > 0)
                    {
                        foreach (var service in lServices)
                        {
                            service.idVista = idView;
                        }

                        tVistas.AddServiceOutProducts(lServices);
                    }

                    if (idView > 0)
                    {
                        this.SendMailOutProducts(rem);

                        if (idEraserView.HasValue)
                        {
                            this.DeleteEraserOutProducts((int)idEraserView);
                        }

                        if (freight == true)
                        {
                            SalesController oSale = new SalesController();
                            List<ProductSaleViewModel> lProducto = new List<ProductSaleViewModel>();

                            lProducto.Add(new ProductSaleViewModel()
                            {
                                idProducto = null,
                                codigo = "",
                                desc = "FLETE A VISTA",
                                prec = 1000.00M,
                                descuento = 0.0M,
                                cantidad = 1,
                                idServicio = 1,
                                idCredito = null,
                                credito = 0,
                                comentarios = ""
                            });

                            List<TypePaymentViewModel> lTypePayment = new List<TypePaymentViewModel>();

                            lTypePayment.Add(new TypePaymentViewModel()
                            {
                                typesPayment = 4,
                                amount = 1000.00M
                            });

                            var SaleResult = oSale.InternalSaveSale((int)idVendedor, idVendedor2, idClienteFisico, idClienteMoral, idOffice, project, typeCustomer,
                            idOfficeReference, idSucursal, 1, 1000.00M, 0.0M, 0, 1000.00M,
                            lProducto, lTypePayment);

                            tVistas.AddFleteOutProduct(idView, SaleResult);
                        }

                        jmResult.success = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Salidas a Vista"), sRemision = rem };
                    }
                }else
                {
                    throw new System.ArgumentException("Riesgo de duplicar Salida a Vista", "Salidas a Vista");
                }
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new
                {
                    Error = ex.Message
                };
            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult SaveAddEraserOutProducts(int? idClienteFisico, int? idClienteMoral, int? idOffice, string project, byte typeCustomer, int? idOfficeReference,
                                               int idSucursal, DateTime Fecha, int CantidadProductos,
                                               decimal Subtotal, decimal Total, short? idVendedor,
                                               short? idVendedor2, bool flete, List<ShoppingCartViewModel> Items)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var listaArticulos = new List<ShoppingCartViewModel>();

                UserViewModel uvmUser = (UserViewModel)Session["_User"];

                tBorradorVista oVista = new tBorradorVista();

                List<tBorradorDetalleVista> listaDetalleVista = new List<tBorradorDetalleVista>();

                List<ServiceOutProductViewModel> lServices = new List<ServiceOutProductViewModel>();

                oVista.idUsuario1 = uvmUser.idUsuario;
                oVista.idClienteFisico = idClienteFisico;
                oVista.idClienteMoral = idClienteMoral;
                oVista.idDespacho = idOffice;
                oVista.Proyecto = project;
                oVista.idDespachoReferencia = idOfficeReference;
                oVista.TipoCliente = typeCustomer;
                oVista.idSucursal = idSucursal;
                oVista.Fecha = Fecha;
                oVista.CantidadProductos = CantidadProductos;
                oVista.Subtotal = Subtotal;
                oVista.Total = Total;
                oVista.idUsuario1 = idVendedor;
                oVista.idUsuario2 = idVendedor2;
                oVista.Flete = flete;

                //Cargamos el listado de articulos a salir
                foreach (var item in Items)
                {
                    if (item.servicio == false)
                    {

                        tBorradorDetalleVista oDetalleVista = new tBorradorDetalleVista();

                        oDetalleVista.Cantidad = item.cantidad;
                        oDetalleVista.idProducto = item.idProducto;
                        oDetalleVista.idSucursal = idSucursal;
                        oDetalleVista.Precio = item.prec;
                        oDetalleVista.Comentarios = item.comentarios;

                        listaDetalleVista.Add(oDetalleVista);

                    }
                    else
                    {

                        ServiceOutProductViewModel oService = new ServiceOutProductViewModel();

                        oService.idServicio = item.idProducto;
                        oService.Precio = item.prec;
                        oService.Cantidad = item.cantidad;
                        oService.Comentarios = item.comentarios;

                        lServices.Add(oService);

                    }
                }

                oVista.tBorradorDetalleVistas = listaDetalleVista;

                int idView = tBorradorVistas.AddOutProducts(oVista);

                if (idView > 0)
                {

                    foreach (var service in lServices)
                    {

                        service.idVista = idView;

                    }

                    tBorradorVistas.AddServiceOutProducts(lServices);

                }

                if (idView > 0)
                {

                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó en borrador la salida a vista con exito") };

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

        public ViewResult ListOutProducts()
        {
            return View();
        }       

        public ActionResult ListEraserOutProducts()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetEraserOutProducts(string costumer, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { OutProducts = tBorradorVistas.GetEraserOutProducts(idUser, restricted, costumer, amazonas, guadalquivir, textura, page, pageSize), Count = tBorradorVistas.GetCount(idUser, restricted, costumer, amazonas, guadalquivir, textura) };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ActionResult GetEraserOutProductForUpdate(int idView)
        {

            OutProductsViewModel oOutProducts = tBorradorVistas.GetOutproduct(idView);

            ViewBag.IDBranch = oOutProducts.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)oOutProducts.idSucursal);

            ViewBag.TypeCustomer = oOutProducts.TipoCliente;

            ViewBag.IDView = idView;

            ViewBag.Customer = (oOutProducts.idClienteFisico.HasValue) ? oOutProducts.idClienteFisico : (oOutProducts.idClienteMoral.HasValue) ? oOutProducts.idClienteMoral : oOutProducts.idDespacho;

            ViewBag.Office = oOutProducts.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oOutProducts.Vendedor) ? "" : oOutProducts.Vendedor, String.IsNullOrEmpty(oOutProducts.Vendedor2) ? "" : oOutProducts.Vendedor2 };

            return View(oOutProducts);

        }

        [HttpPost]
        public ActionResult SaveUpdateEraserOutProducts(int idEraserView, int? idClienteFisico, int? idClienteMoral, int? idOffice, string project,
                                               byte typeCustomer, int? idOfficeReference,
                                               int idSucursal, DateTime Fecha, int CantidadProductos,
                                               decimal Subtotal, decimal Total, short? idVendedor,
                                               short? idVendedor2, List<ShoppingCartViewModel> Items)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                var listaArticulos = new List<ShoppingCartViewModel>();

                UserViewModel uvmUser = (UserViewModel)Session["_User"];

                tBorradorVista oVista = new tBorradorVista();

                List<tBorradorDetalleVista> lDetalleVista = new List<tBorradorDetalleVista>();

                List<tBorradorServicioVista> lServices = new List<tBorradorServicioVista>();

                oVista.idBorradorVista = idEraserView;
                oVista.idUsuario1 = uvmUser.idUsuario;
                oVista.idClienteFisico = idClienteFisico;
                oVista.idClienteMoral = idClienteMoral;
                oVista.idDespacho = idOffice;
                oVista.Proyecto = project;
                oVista.idDespachoReferencia = idOfficeReference;
                oVista.TipoCliente = typeCustomer;
                oVista.idSucursal = idSucursal;
                oVista.Fecha = Fecha;
                oVista.CantidadProductos = CantidadProductos;
                oVista.Subtotal = Subtotal;
                oVista.Total = Total;
                oVista.idUsuario1 = idVendedor;
                oVista.idUsuario2 = idVendedor2;

                //Cargamos el listado de articulos a salir
                foreach (var item in Items)
                {
                    if (item.servicio == false)
                    {

                        tBorradorDetalleVista oDetalleVista = new tBorradorDetalleVista();

                        oDetalleVista.Cantidad = item.cantidad;
                        oDetalleVista.idProducto = item.idProducto;
                        oDetalleVista.idSucursal = idSucursal;
                        oDetalleVista.Precio = item.prec;
                        oDetalleVista.Comentarios = item.comentarios;

                        lDetalleVista.Add(oDetalleVista);

                    }
                    else
                    {

                        tBorradorServicioVista oService = new tBorradorServicioVista();

                        oService.idServicio = item.idProducto;
                        oService.Precio = item.prec;
                        oService.Cantidad = item.cantidad;
                        oService.Comentarios = item.comentarios;

                        lServices.Add(oService);

                    }
                }

                oVista.tBorradorDetalleVistas = lDetalleVista;

                oVista.tBorradorServicioVistas = lServices;

                tBorradorVistas.UpdateOutProduct(oVista);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó en borrador la salida a vista con exito") };

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
        public ActionResult DeleteEraserOutProducts(int idView)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tBorradorVistas.DeleteOutProduct(idView);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Registro eliminado" };

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
        public ActionResult GetOutProductsDetails(int idVista)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { outProductsDetails = tVistas.GetOutProductsDetails(idVista) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ViewResult OutProductsList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetVistas(int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { listOfVistas = tVistas.GetVistas(page, pageSize), Count = tVistas.CountRegisters() };
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
        public ActionResult GetVistaDetalle(int idVista)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { VistaProducts = tVistas.GetVistaDetalle(idVista) };
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
        public ActionResult ListOutProducts(bool allTime, string fechaInicial, string fechaFinal, string cliente, int? iduser, string producto, string remision, string project, int status, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                var result = tVistas.GetOutProducts(allTime, idUser, restricted, Convert.ToDateTime(fechaInicial), Convert.ToDateTime(fechaFinal), cliente, iduser, producto, remision, project, status, amazonas, guadalquivir, textura, page, pageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { outProducts = result.Item1, Count = result.Item2};
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
        public ActionResult GeneratePrevNumberRem(int idBranch)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sRemision = tVistas.GeneratePrevNumberRem(idBranch) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public ActionResult PrintOutProducts(string remision)
        {

            OutProductsViewModel oView = tVistas.GetOutProductsForRemision(remision);

            oView.oServicios = tVistas.GetServiceOutProducts(oView.idVista);

            return PartialView(oView);

        }

        public ActionResult PrintPendingOutProducts(string remision)
        {

            OutProductsViewModel oView = tVistas.GetOutProductsPending(remision);

            oView.oServicios = tVistas.GetServiceOutProducts(oView.idVista);

            return PartialView(oView);

        }

        [HttpPost]
        public ActionResult SetStatus(int idOutProducts, short status)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tVistas.SetStatus(idOutProducts, status))
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "El estatus de la Salida a Vista se ha actualizado con exito." };

                }
                else
                {

                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "Existen productos pendientes." };

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

        public ActionResult DetailOutProducts(string remision)
        {
            OutProductsViewModel oOutProducts = tVistas.GetOutProductsForRemision(remision);

            return PartialView("~/Views/OutProducts/DetailOutProducts.cshtml", oOutProducts);
        }

        [HttpPost]
        public ActionResult UpdateStockReturnOutProducts(int idOutProducts, short? idUser, int[][] lProducts)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tVistas.UpdateStockReturnOutProducts(idOutProducts, lProducts, (short)idUser);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El estatus de la Salida a Vista se ha actualizado con exito." };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public static int GetCountPendingOutProducts() => OutProducts.GetCountPendingOutProducts();

        public static int GetCountPendingOutProducts(int idUser) => OutProducts.GetCountPendingOutProducts(idUser);

        [HttpPost]
        public ActionResult GetPendingOutProducts()
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if ((int)Session["_Profile"] == Constants.profileAdmin)
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Pendings = OutProducts.GetPendingOutProducts() };

                }
                else
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Pendings = OutProducts.GetPendingOutProducts((int)Session["_ID"]) };

                }

            }
            catch (Exception ex)
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public void SendMailOutProducts(string remision)
        {
            OutProductsViewModel oView = tVistas.GetOutProductsForRemision(remision);
            var view = _PDFGenerator.GenerateView(oView.idVista);
            var emailService = new Email();

            string email;

            if (oView.idClienteFisico > 0)
            {
                email = tPhysicalCustomers.GetPhysicalCustomer((int)oView.idClienteFisico).Correo;
            }
            else if (oView.idClienteMoral > 0)
            {
                email = tMoralCustomers.GetMoralCustomer((int)oView.idClienteMoral).Correo;
            }
            else
            {
                email = tOffices.GetOffice((int)oView.idDespacho).Correo;
            }

            if (email.Trim().ToLower() != "noreply@correo.com")
            {
                emailService.SendMailWithAttachment(email, "Salida a Vista " + oView.remision, "Salida a Vista " + oView.remision, "SalidaVista_" + oView.remision + ".pdf", view);
            }
            else
            {
                emailService.SendInternalMailWithAttachment("Salida a Vista " + oView.remision, "Salida a Vista " + oView.remision, "SalidaVista_" + oView.remision + ".pdf", view);
            }
        }

        [HttpPost]
        public ActionResult SendMailOutProductsAgain(string remision, string email)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                OutProductsViewModel oView = tVistas.GetOutProductsForRemision(remision);
                var view = _PDFGenerator.GenerateView(oView.idVista);
                var emailService = new Email();

                if (email.Trim().ToLower() != "noreply@correo.com")
                {
                    emailService.SendMailWithAttachment(email, "Salida a Vista " + oView.remision, "Salida a Vista " + oView.remision, "SalidaVista_" + oView.remision + ".pdf", view);
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
        public ActionResult ValidatePurchaseSale(int idDetailView)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tVistas.ValidatePurchaseSale(idDetailView);

                jmResult.success = 1;
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }              

        public ActionResult EditHeaderView(int idView)
        {
            OutProductsViewModel oView = this.tVistas.GetViewForIdView(idView);

            ViewBag.TypeCustomer = oView.TipoCliente;

            ViewBag.Customer = (oView.idClienteFisico.HasValue) ? oView.idClienteFisico : (oView.idClienteMoral.HasValue) ? oView.idClienteMoral : oView.idDespacho;

            ViewBag.Office = oView.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(oView.Vendedor) ? "" : oView.Vendedor, String.IsNullOrEmpty(oView.Vendedor2) ? "" : oView.Vendedor2 };

            return View(oView);
        }

        [HttpPost]
        public ActionResult SaveEditHeaderView(OutProductsViewModel oView)
        {

            var jsonMessenger = new JsonMessenger();

            try
            {
                this.tVistas.SaveEditOutView(oView);
                

                jsonMessenger.success = 1;
                jsonMessenger.failure = 0;
                jsonMessenger.oData = (object)new
                {
                    Message = string.Format(this.Message.msgAdd = "Se actualizó la información de la salida a vista")
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
    }
}