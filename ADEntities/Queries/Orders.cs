using ADEntities.Enums;
using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Orders : Base
    {

        public int CountRegistersWithFilters(int searchType, DateTime dtDateSince, DateTime dtDateUntil, string order, string factura, string brand, string code, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    DateTime dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    DateTime dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);
                    IQueryable<tOrden> orders = null;

                    switch (searchType)
                    {
                        case (int)OrderDateType.purchased:
                            orders = context.tOrdens.Where(p =>
                    (p.FechaCompra >= dtStart && p.FechaCompra <= dtEnd) &&
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                        case (int)OrderDateType.delivery:
                            orders = context.tOrdens.Where(p =>
                    (p.FechaEntrega >= dtStart && p.FechaEntrega <= dtEnd) &&
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                        case (int)OrderDateType.capture:
                            orders = context.tOrdens.Where(p =>
                    (p.FechaCaptura >= dtStart && p.FechaCaptura <= dtEnd) &&
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                        default:
                            orders = context.tOrdens.Where(p =>
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                    }

                    return orders.Select(p => new OrderViewModel()
                    {

                        idOrden = p.idOrden,
                        Orden = p.Orden,
                        Factura = p.Factura,
                        idEmpresa = p.idEmpresa,
                        FechaCompra = p.FechaCompra,
                        FechaEntrega = p.FechaEntrega,
                        FechaCaptura = p.FechaCaptura

                    }).OrderByDescending(p => p.FechaEntrega).Count();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<OrderViewModel> GetOrders()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdens.Select(p => new OrderViewModel()
                    {

                        idOrden = p.idOrden,
                        Orden = p.Orden,
                        Factura = p.Factura,
                        idEmpresa = p.idEmpresa,
                        FechaEntrega = p.FechaEntrega,
                        FechaCaptura = p.FechaCaptura

                    }).OrderByDescending(p => p.FechaEntrega).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<OrderViewModel> GetOrders(int searchType, DateTime dtDateSince, DateTime dtDateUntil, int? status, string order, string factura, string brand, string code, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    DateTime dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    DateTime dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);
                    IQueryable<tOrden> orders = null;

                    switch (searchType)
                    {
                        case (int)OrderDateType.purchased:
                            orders = context.tOrdens.Where(p =>
                    (p.FechaCompra >= dtStart && p.FechaCompra <= dtEnd) &&
                    ((p.Estatus == status) || status == null) &&
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                        case (int)OrderDateType.delivery:
                            orders = context.tOrdens.Where(p =>
                    (p.FechaEntrega >= dtStart && p.FechaEntrega <= dtEnd) &&
                    ((p.Estatus == status) || status == null) &&
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                        case (int)OrderDateType.capture:
                            orders = context.tOrdens.Where(p =>
                    (p.FechaCaptura >= dtStart && p.FechaCaptura <= dtEnd) &&
                    ((p.Estatus == status) || status == null) &&
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                        default:
                            orders = context.tOrdens.Where(p =>
                    ((p.Estatus == status) || status == null) &&
                    (p.Orden.Contains(order) || (String.IsNullOrEmpty(order))) &&
                    (p.Factura.Contains(factura) || (String.IsNullOrEmpty(factura))) &&
                    (p.tProveedore.NombreEmpresa.Contains(brand) || (String.IsNullOrEmpty(brand))) &&
                    (p.tOrdenProductoes.Any(prod => prod.tProducto.Codigo.Contains(code)) || (string.IsNullOrEmpty(code))));
                            break;
                    }

                    return orders.Select(p => new OrderViewModel()
                    {

                        idOrden = p.idOrden,
                        Orden = p.Orden,
                        Factura = p.Factura,
                        FechaCompra = p.FechaCompra,
                        FechaCaptura = p.FechaCaptura,
                        FechaEntrega = p.FechaEntrega,
                        idEmpresa = p.tProveedore.idProveedor,
                        Empresa = p.tProveedore.NombreEmpresa,
                        Estatus = p.Estatus ?? 0,
                        sEstatus = ((p.Estatus ?? 0) == TypesGeneric.TypesOrder.Pending) ? "red" :
                                        "green"

                    }).OrderByDescending(p => p.FechaEntrega).Skip(page * pageSize).Take(pageSize).ToList();

                    //return orderr.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public OrderViewModel GetOrden(int idOrden)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdens.Where(p => p.idOrden == idOrden).Select(p => new OrderViewModel()
                    {

                        idOrden = p.idOrden,
                        Orden = p.Orden,
                        Factura = p.Factura,
                        idEmpresa = p.idEmpresa,
                        FechaCompra = p.FechaCompra,
                        FechaEntrega = p.FechaEntrega,
                        FechaCaptura = p.FechaCaptura,
                        Estatus = p.Estatus ?? 0,
                        Dolar = p.CostoDolar,
                        Moneda = p.TipoMoneda

                    }).First();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public int AddOrder(tOrden oOrden)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tOrdens.Add(oOrden);

                    context.SaveChanges();

                    iResult = oOrden.idOrden;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }

            return iResult;
        }

        public bool UpdateOrder(tOrden oOrden)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tOrden otOrden = context.tOrdens.FirstOrDefault(p => p.idOrden == oOrden.idOrden);

                    otOrden.Orden = oOrden.Orden;
                    otOrden.Factura = oOrden.Factura;
                    otOrden.idEmpresa = oOrden.idEmpresa;
                    oOrden.FechaCompra = oOrden.FechaCompra;
                    otOrden.FechaEntrega = oOrden.FechaEntrega;
                    otOrden.FechaCaptura = oOrden.FechaCaptura;
                    oOrden.Estatus = oOrden.Estatus;
                    oOrden.TipoMoneda = oOrden.TipoMoneda;

                    context.SaveChanges();

                    return true;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public int CountOrderProductRegisters(int idOrden)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tOrdenProductoes.Where(p => p.idOrden == idOrden).Count();

        }

        public ProductOrderViewModel GetOrderProducts(int idOrden, int idProducto)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdenProductoes.Where(p => p.idOrden == idOrden && p.idProducto == idProducto).Select(p => new ProductOrderViewModel()
                    {

                        idOrden = p.idOrden,
                        idProducto = p.idProducto,
                        Codigo = p.tProducto.Codigo,
                        Producto = p.tProducto.Nombre,
                        Cantidad = (int)p.Cantidad,
                        PrecioCompra = (decimal)p.PrecioCompra,
                        PrecioVenta = (decimal)p.PrecioVenta,
                        _CantidadProductos = context.tCantidadProductosOrdens.Where(x => x.idOrden == p.idOrden
                                             && x.idProducto == p.idProducto).Select(x => new QuantityProductsOrderViewModel()
                                             {

                                                 idOrden = p.idOrden,
                                                 idProducto = p.idProducto,
                                                 idSucursal = x.idSucursal,
                                                 Sucursal = x.tSucursale.Nombre,
                                                 Cantidad = (int)x.Cantidad

                                             }).OrderBy(x => x.idSucursal).ToList()

                    }).FirstOrDefault();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<ProductOrderViewModel> GetOrdersProducts(int idOrden, int page, int pageSize)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var orders = (from op in context.tOrdenProductoes
                                  group op by new ProductOrderViewModel
                                  {
                                      idOrdenProducto = op.idOrdenProducto,
                                      idOrden = op.idOrden,
                                      idProducto = op.idProducto,
                                      Orden = op.tOrden.Orden,
                                      Factura = op.tOrden.Factura,
                                      Producto = op.tProducto.Nombre,
                                      Codigo = op.tProducto.Codigo,
                                      Descripcion = op.tProducto.Descripcion,
                                      Cantidad = (int)op.Cantidad,
                                      PrecioCompra = (decimal)op.PrecioCompra,
                                      PrecioVenta = (decimal)op.PrecioVenta,
                                      urlImagen = op.tProducto.TipoImagen == 1 ? "/Content/Products/" + op.tProducto.NombreImagen + op.tProducto.Extension : op.tProducto.urlImagen

                                  } into ogp
                                  select new ProductOrderViewModel
                                  {
                                      idOrdenProducto = ogp.Key.idOrdenProducto,
                                      idOrden = ogp.Key.idOrden,
                                      idProducto = ogp.Key.idProducto,
                                      Orden = ogp.Key.Orden,
                                      Factura = ogp.Key.Factura,
                                      Producto = ogp.Key.Producto,
                                      Codigo = ogp.Key.Codigo,
                                      Descripcion = ogp.Key.Descripcion,
                                      Cantidad = ogp.Key.Cantidad,
                                      PrecioCompra = ogp.Key.PrecioCompra,
                                      PrecioVenta = ogp.Key.PrecioVenta,
                                      urlImagen = ogp.Key.urlImagen

                                  }).Where(p => p.idOrden == idOrden).OrderBy(p => p.idOrdenProducto);

                    return orders.Skip(page * pageSize).Take(pageSize).ToList();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public ProductOrderViewModel GetOrdersProduct(int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdenProductoes.Where(p => p.idProducto == idProduct).Select(p => new ProductOrderViewModel()
                    {
                        idOrden = p.idOrden,
                        idProducto = p.idProducto,
                        Codigo = p.tProducto.Codigo,
                        Cantidad = (int)p.Cantidad,
                        PrecioCompra = (decimal)p.PrecioCompra,
                        PrecioVenta = (decimal)p.PrecioVenta
                    }).First();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public int GetProductBranch(int idProduct, int idBranch)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    iResult = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == idBranch).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }

            return iResult;
        }

        public int AddOrderProduct(tOrdenProducto oOrdenProduct)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tOrdenProductoes.Add(oOrdenProduct);

                    context.SaveChanges();

                    iResult = oOrdenProduct.idOrdenProducto;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }

            return iResult;
        }

        public bool UpdateOrderProduct(int idProduct, tOrdenProducto oOrdenProduct, tProducto oProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tOrdenProducto otOrdenProduct = context.tOrdenProductoes.FirstOrDefault(p => p.idOrden == oOrdenProduct.idOrden && p.idProducto == idProduct);

                    otOrdenProduct.idProducto = oOrdenProduct.idProducto;
                    otOrdenProduct.PrecioCompra = oOrdenProduct.PrecioCompra;
                    otOrdenProduct.PrecioVenta = oOrdenProduct.PrecioVenta;
                    otOrdenProduct.Cantidad = oOrdenProduct.Cantidad;

                    context.SaveChanges();

                    if (context.tOrdens.Where(p => p.idOrden == oOrdenProduct.idOrden).FirstOrDefault().Estatus == TypesGeneric.TypesOrder.Active)
                    {
                        tProducto otProduct = context.tProductos.FirstOrDefault(p => p.idProducto == idProduct);

                        otProduct.Nombre = oProduct.Nombre;
                        otProduct.PrecioCompra = oProduct.PrecioCompra;
                        otProduct.PrecioVenta = oProduct.PrecioVenta;

                        context.SaveChanges();
                    }

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void AddQuantityOrderProducts(int idOrder, List<tCantidadProductosOrden> lQuantityOrderProducts, decimal? precioVenta, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    Products tProducts = new Products();
                    var order = this.GetOrden(idOrder);
                    var productId = lQuantityOrderProducts.Select(o => o.idProducto).FirstOrDefault();
                    tProducto tProduct = context.tProductos.FirstOrDefault(p => p.idProducto == productId);

                    foreach (var product in lQuantityOrderProducts)
                    {
                        context.tCantidadProductosOrdens.Add(product);
                        context.SaveChanges();

                        //Si la orden se marca como Activa se modifica el inventario
                        if (GetOrderStatus(idOrder) == TypesGeneric.TypesOrder.Active)
                        {
                            if (this.GetProductBranch(product.idProducto, product.idSucursal) == 0)
                            {
                                this.AddRegisterProduct(product.idProducto, product.idSucursal, "Se agrega nuevo producto dentro de la orden " + order.Factura + order.Orden, 0, (decimal)product.Cantidad, 0, precioVenta, String.Empty, (int)idUser);
                                tProducts.AddProductBranch(product.idProducto, product.idSucursal, (int)product.Cantidad);
                            }
                            else
                            {
                                this.AddRegisterProduct(product.idProducto, product.idSucursal, "Se agrega producto dentro de la orden " + order.Factura + order.Orden, this.GetProductStockForBranch(product.idProducto, product.idSucursal), (decimal)(this.GetProductBranch(product.idProducto, product.idSucursal) + (int)product.Cantidad), tProduct.PrecioVenta, precioVenta, String.Empty, (int)idUser);
                                tProducts.UpdateProductBranchExist(product.idProducto, product.idSucursal, (int)product.Cantidad);
                            }
                        }
                        else
                        {
                            if (this.GetProductBranch(product.idProducto, product.idSucursal) == 0)
                            {
                                tProducts.AddProductBranch(product.idProducto, product.idSucursal, 0);
                            }
                            else
                            {
                                tProducts.UpdateProductBranchExist(product.idProducto, product.idSucursal, 0);
                            }
                        }
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateQuantityOrderProducts(int idOrder, int idProduct, string upProduct, List<QuantityProductsOrderViewModel> lQuantityOrderProducts)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    Products tproduct = new Products();

                    foreach (var product in lQuantityOrderProducts)
                    {
                        if (GetOrderStatus(idOrder) == TypesGeneric.TypesOrder.Active)
                        {
                            tproduct.DeleteOnlyProductOrder(idOrder, idProduct, product.idSucursal);
                        }

                        tCantidadProductosOrden oCantidadProductosOrden = context.tCantidadProductosOrdens.Where(p => p.idProducto == idProduct && p.idSucursal == product.idSucursal && p.idOrden == idOrder).FirstOrDefault();

                        oCantidadProductosOrden.Cantidad = product.Cantidad;
                        context.SaveChanges();

                        if (GetOrderStatus(idOrder) == TypesGeneric.TypesOrder.Active)
                        {
                            tproduct.UpdateProductBranchExist(idProduct, product.idSucursal, product.Cantidad);
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public short GetOrderStatus(int idOrder)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdens.Where(p => p.idOrden == idOrder).Select(p => p.Estatus ?? 0).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<OrderViewModel> GetOrders(string order)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdens.Where(p => p.Orden.Contains(order)).Select(p => new OrderViewModel()
                    {
                        idOrden = p.idOrden,
                        Orden = p.Orden
                    }).Distinct().OrderBy(p => p.Orden).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<OrderViewModel> GetBills(string bill)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdens.Where(p => p.Factura.Contains(bill)).Select(p => new OrderViewModel()
                    {
                        idOrden = p.idOrden,
                        Factura = p.Factura
                    }).Distinct().OrderBy(p => p.Factura).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProviderViewModel> GetBrands(string brand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProveedores.Where(p => p.NombreEmpresa.Contains(brand)).Select(p => new ProviderViewModel()
                    {
                        idProveedor = p.idProveedor,
                        Nombre = p.NombreEmpresa
                    }).Distinct().OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteOrder(int idOrder)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    List<ProductOrderViewModel> products = GetProductsOrder(idOrder);

                    foreach (var product in products)
                    {
                        Products tProducts = new Products();
                        tProducts.DeleteProductOrder(idOrder, product.idProducto);
                    }

                    tOrden order = context.tOrdens.Where(p => p.idOrden == idOrder).FirstOrDefault();
                    context.tOrdens.Remove(order);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductOrderViewModel> GetProductsOrder(int idOrder)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdenProductoes.Where(p => p.idOrden == idOrder).Select(p => new ProductOrderViewModel()
                    {
                        idProducto = p.idProducto,
                        PrecioCompra = (decimal)p.PrecioCompra,
                        PrecioVenta = (decimal)p.PrecioVenta
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public ProductOrderViewModel GetProductByOrder(int idOrder, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tOrdenProductoes.Where(p => p.idOrden == idOrder && p.idProducto == idProduct).Select(p => new ProductOrderViewModel()
                    {
                        idProducto = p.idProducto,
                        PrecioCompra = p.PrecioCompra ?? 0M,
                        PrecioVenta = p.PrecioVenta ?? 0M
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStock(int idOrder, short status, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var lproducts = context.tCantidadProductosOrdens.Where(p => p.idOrden == idOrder).Select(p => new ProductOrderViewModel()
                    {
                        idOrden = p.idOrden,
                        idSucursal = p.idSucursal,
                        idProducto = p.idProducto,
                        Cantidad = p.Cantidad ?? 0
                    }).ToList();

                    var order = this.GetOrden(idOrder);

                    if (lproducts != null)
                    {
                        Products tProducts = new Products();

                        foreach (var product in lproducts)
                        {
                            tProducto tProduct = context.tProductos.FirstOrDefault(p => p.idProducto == product.idProducto);

                            if (status == TypesGeneric.TypesOrder.Active)
                            {
                                //Se actualiza el precio de venta
                                var orderProduct = this.GetProductByOrder(idOrder, product.idProducto);

                                if (this.GetProductBranch(product.idProducto, product.idSucursal) == 0)
                                {
                                    this.AddRegisterProduct(product.idProducto, product.idSucursal, "Se agrega nuevo producto dentro de la orden " + order.Factura + order.Orden, 0, (decimal)product.Cantidad, 0, orderProduct.PrecioVenta, String.Empty, (int)idUser);
                                    tProducts.AddProductBranch(product.idProducto, product.idSucursal, product.Cantidad);
                                }
                                else
                                {
                                    this.AddRegisterProduct(product.idProducto, product.idSucursal, "Se agrega producto dentro de la orden " + order.Factura + order.Orden, this.GetProductStockForBranch(product.idProducto, product.idSucursal), (this.GetProductStockForBranch(product.idProducto, product.idSucursal) + product.Cantidad), product.PrecioVenta, orderProduct.PrecioVenta, String.Empty, (int)idUser);
                                    tProducts.UpdateProductBranchExist(product.idProducto, product.idSucursal, product.Cantidad);
                                }

                                tProducts.UpdatePrice(orderProduct.idProducto, orderProduct.PrecioCompra, orderProduct.PrecioVenta);
                            }
                            else
                            {
                                tProducts.DeleteProductBranchExist(product.idProducto, product.idSucursal, product.Cantidad);
                            }
                        }
                    }

                    order.Estatus = (order.Estatus == TypesGeneric.TypesOrder.Active) ? TypesGeneric.TypesOrder.Pending : TypesGeneric.TypesOrder.Active;

                    this.UpdateStatusOrder(idOrder, order.Estatus);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStatusOrder(int idOrder, short status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tOrden order = context.tOrdens.Where(p => p.idOrden == idOrder).FirstOrDefault();

                    order.Estatus = status;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public string GetHistoryRange(string from, string to, string startDate)
        {
            try
            {
                string FreeBaseUrl = "https://free.currencyconverterapi.com/api/v6/";

                string url = FreeBaseUrl + "convert?q=" + from + "_" + to + "&compact=ultra&date=" + startDate + "&apiKey=6586648a6b323d625046";

                String response = "";
                using (var webClient = new System.Net.WebClient())
                {
                    response = webClient.DownloadString(url);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public List<CoinTypeViewModel> getCoinTypes()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoMonedas.Select(p => new CoinTypeViewModel()
                    {
                        Id = p.idTipoMoneda,
                        Clave = p.Clave,
                        Descripcion = p.Descripcion
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

    }
}