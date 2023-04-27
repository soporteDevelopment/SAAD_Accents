using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class OrdersController : BaseController
    {
        // GET: Orders
        public override ActionResult Index()
        {
            Session["Controller"] = "Ordenes";

            return View();
        }

        [HttpPost]
        public ActionResult ListOrders(int searchType, string dtDateSince, string dtDateUntil, int? status, string order, string bill, string brand, string code, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Orders = tOrders.GetOrders(searchType, Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), status, order, bill, brand, code, page, pageSize), Count = tOrders.CountRegistersWithFilters(searchType, Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), order, bill, brand, code, page, pageSize) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public PartialViewResult AddOrder()
        {

            ViewBag.Enterprise = JsonConvert.SerializeObject(tProviders.GetProviders());

            return PartialView("AddOrder");

        }

        [HttpPost]
        public ActionResult SaveAddOrder(string order, string factura, int idEmpresa, string fechaCompra, string dollar, string fechaEntrega, string fechaCaptura, int idCoin)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tOrden oOrder = new tOrden();

                oOrder.Orden = order;
                oOrder.Factura = factura;
                oOrder.idEmpresa = idEmpresa;
                oOrder.FechaCompra = Convert.ToDateTime(fechaCompra);
                oOrder.CostoDolar = Convert.ToDecimal(dollar);
                oOrder.FechaEntrega = Convert.ToDateTime(fechaEntrega);
                oOrder.FechaCaptura = Convert.ToDateTime(fechaCaptura);
                oOrder.Estatus = ADEntities.Queries.TypesGeneric.TypesOrder.Pending;
                oOrder.TipoMoneda = idCoin;

                if (tOrders.AddOrder(oOrder) > 0)
                {

                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Orders") };

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

        public ViewResult UpdateOrder(int idOrder)
        {

            ViewBag.Enterprise = JsonConvert.SerializeObject(tProviders.GetProviders());

            OrderViewModel oOrder = (OrderViewModel)tOrders.GetOrden(idOrder);

            return View(oOrder);

        }

        [HttpPost]
        public ActionResult SaveUpdateOrder(int idOrder, string factura, string order, int idEmpresa, string fechaCompra, string fechaEntrega, string fechaCaptura, int idCoin)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tOrden oOrden = new tOrden();

                oOrden.idOrden = idOrder;
                oOrden.Orden = order;
                oOrden.Factura = factura;
                oOrden.idEmpresa = idEmpresa;
                oOrden.FechaCompra = Convert.ToDateTime(fechaCompra);
                oOrden.FechaEntrega = Convert.ToDateTime(fechaEntrega);
                oOrden.FechaCaptura = Convert.ToDateTime(fechaCaptura);
                oOrden.TipoMoneda = idCoin;

                tOrders.UpdateOrder(oOrden);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Ordenes") };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ActionResult UpdateOrderStatus(int idOrder, short status)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tOrders.UpdateStock(idOrder, status, (int)Session["_ID"]);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Ordenes") };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult IndexOrdersProducts(int idOrder, int idProvider)
        {
            Session["idOrder"] = idOrder;
            if (idProvider != 0)
                Session["idProvedor"] = idProvider;

            return View();
        }

        [HttpPost]
        public ActionResult ListOrdersProducts(int idOrder, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Orders = tOrders.GetOrdersProducts(idOrder, page, pageSize), Count = tOrders.CountOrderProductRegisters(idOrder) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult AddOrderProduct()
        {
            ViewBag.Products = JsonConvert.SerializeObject(tProducts.GetProducts());

            return View();
        }

        [HttpPost]
        public ActionResult SaveAddOrderProduct(int idOrder, int idProduct, int cantidad, decimal precioCompra, decimal precioVenta, int existenciaAmazonas, int existenciaGuadalquivir, int existenciaTextura)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (precioVenta > 0)
                {
                    tOrdenProducto oOrderProduct = new tOrdenProducto();

                    oOrderProduct.idOrden = idOrder;
                    oOrderProduct.idProducto = idProduct;
                    oOrderProduct.Cantidad = cantidad;
                    oOrderProduct.PrecioCompra = precioCompra;
                    oOrderProduct.PrecioVenta = precioVenta;

                    if (tOrders.AddOrderProduct(oOrderProduct) > 0)
                    {

                        List<tCantidadProductosOrden> lQuantityProductsOrder = new List<tCantidadProductosOrden>();

                        lQuantityProductsOrder.Add(new tCantidadProductosOrden()
                        {
                            idOrden = idOrder,
                            idProducto = idProduct,
                            idSucursal = Constants.Amazonas,
                            Cantidad = existenciaAmazonas
                        });

                        lQuantityProductsOrder.Add(new tCantidadProductosOrden()
                        {
                            idOrden = idOrder,
                            idProducto = idProduct,
                            idSucursal = Constants.Guadalquivir,
                            Cantidad = existenciaGuadalquivir
                        });

                        lQuantityProductsOrder.Add(new tCantidadProductosOrden()
                        {
                            idOrden = idOrder,
                            idProducto = idProduct,
                            idSucursal = Constants.Textura,
                            Cantidad = existenciaTextura
                        });

                        tOrders.AddQuantityOrderProducts(idOrder, lQuantityProductsOrder, precioVenta, (int)Session["_ID"]);

                        if (tOrders.GetOrderStatus(idOrder) == ADEntities.Queries.TypesGeneric.TypesOrder.Active)
                        {
                            tProducts.UpdatePrice(idProduct, precioCompra, precioVenta);
                        }

                    }

                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Orders") };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Message = "El precio de venta debe ser mayor a 0" };
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

        public ViewResult UpdateOrderProduct(int idOrder, int idProduct)
        {

            ProductOrderViewModel oOrderProduct = (ProductOrderViewModel)tOrders.GetOrderProducts(idOrder, idProduct);

            ViewBag.idOrder = idOrder;
            ViewBag.CantidadSucursales = JsonConvert.SerializeObject(oOrderProduct._CantidadProductos);

            return View(oOrderProduct);

        }

        [HttpPost]
        public ActionResult SaveUpdateOrderProduct(int idOrder, int idProduct, string upProduct, string nameProducto, int cantidad, decimal precioCompra, decimal precioVenta, List<QuantityProductsOrderViewModel> lCantidadProductos)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tOrdenProducto oOrderProduct = new tOrdenProducto();

                oOrderProduct.idOrden = idOrder;
                oOrderProduct.idProducto = idProduct;
                oOrderProduct.Cantidad = cantidad;
                oOrderProduct.PrecioCompra = precioCompra;
                oOrderProduct.PrecioVenta = precioVenta;

                tProducto oProduct = new tProducto();

                oProduct.Nombre = nameProducto;
                oProduct.PrecioCompra = precioCompra;
                oProduct.PrecioVenta = precioVenta;

                tOrders.UpdateOrderProduct(idProduct, oOrderProduct, oProduct);

                tOrders.UpdateQuantityOrderProducts(idOrder, idProduct, upProduct, lCantidadProductos);

                if (tOrders.GetOrderStatus(idOrder) == ADEntities.Queries.TypesGeneric.TypesOrder.Active)
                {
                    tProducts.UpdatePrice(idProduct, precioCompra, precioVenta);
                }

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Ordenes") };

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
        public ActionResult DeleteProductOrder(int idOrder, int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tProducts.DeleteProductOrder(idOrder, idProduct);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se eliminó el producto de la orden") };

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
        public ActionResult DeleteOrder(int idOrder)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tOrders.DeleteOrder(idOrder);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se eliminó la orden") };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetOrdersForName(string order)
        {
            return Json(new { Orders = tOrders.GetOrders(order) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBillsForName(string bill)
        {
            return Json(new { Bills = tOrders.GetBills(bill) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBrandsForName(string brand)
        {
            return Json(new { Brands = tOrders.GetBrands(brand) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CheckCurrency(DateTime date)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var jsonString = tOrders.GetHistoryRange("USD", "MXN", date.ToString("yyyy-MM-dd"));
                var data = JObject.Parse(jsonString);
                string response = null;
                foreach (var e in data)
                {
                    response = e.Value.First.First.ToString();
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { conversion = response };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return JsonConvert.SerializeObject(jmResult);
        }

        [HttpGet]
        public ActionResult ListCoinType()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { CoinTypes = tOrders.getCoinTypes() };

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