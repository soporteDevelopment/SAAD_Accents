using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;


namespace ADEntities.Queries
{
    public class Stocks : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosInventarios.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegisters(DateTime dtDateSince, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);

                    var stocks = context.tInventarios.Where(p =>
                      (p.FechaInicio >= dtStart) &&
                      (((p.idSucursal == amazonas) ||
                      (p.idSucursal == guadalquivir) ||
                      (p.idSucursal == textura)))).Select(p => new StockViewModel()
                      {

                          idInventario = p.idInventario,
                          idSucursal = p.idSucursal,
                          Sucursal = p.tSucursale.Nombre,
                          FechaInicio = p.FechaInicio,
                          FechaFin = p.FechaFin,
                          DiferenciasNegativas = p.DiferenciasNegativas,
                          DiferenciasPositivas = p.DiferenciasPositivas,
                          ArticulosInventarioAnterior = p.ArticulosInventarioAnterior,
                          ArticulosInventarioActual = p.ArticulosInventarioActual,
                          CostoInventarioAnterior = p.CostoInventarioAnterior,
                          CostoInventarioActual = p.CostoInventarioActual,
                          Estatus = p.Estatus

                      }).OrderByDescending(p => p.FechaInicio);

                    return stocks.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegisters(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosInventarios.Where(p => p.idInventario == idStock).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegistersDif(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosInventarios.Where(p => (p.idInventario == idStock) && (p.CantidadAnterior > p.CantidadActual)).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<StockViewModel> GetAllStock()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tInventarios.Select(p => new StockViewModel()
                    {
                        idInventario = p.idInventario,
                        idSucursal = p.idSucursal,
                        FechaInicio = p.FechaInicio,
                        FechaFin = p.FechaFin,
                        Estatus = p.Estatus
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public StockViewModel GetStock(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tInventarios.Where(p => p.idInventario == idStock).Select(p => new StockViewModel()
                    {
                        idInventario = p.idInventario,
                        idSucursal = p.idSucursal,
                        FechaInicio = p.FechaInicio,
                        FechaFin = p.FechaFin,
                        Estatus = p.Estatus
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool GetProductInStock(int idStock, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosInventarios.Any(p =>
                        p.idInventario == idStock && p.idProducto == idProduct);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<StockViewModel> GetStocks(DateTime dtDateSince, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);

                    var stocks = context.tInventarios.Where(p =>
                      (p.FechaInicio >= dtStart) &&
                      (((p.idSucursal == amazonas) ||
                      (p.idSucursal == guadalquivir) ||
                      (p.idSucursal == textura)))).Select(p => new StockViewModel()
                      {
                          idInventario = p.idInventario,
                          idSucursal = p.idSucursal,
                          Sucursal = p.tSucursale.Nombre,
                          FechaInicio = p.FechaInicio,
                          FechaFin = p.FechaFin,
                          DiferenciasNegativas = p.DiferenciasNegativas,
                          DiferenciasPositivas = p.DiferenciasPositivas,
                          ArticulosInventarioAnterior = p.ArticulosInventarioAnterior,
                          ArticulosInventarioActual = p.ArticulosInventarioActual,
                          CostoInventarioAnterior = p.CostoInventarioAnterior,
                          CostoInventarioActual = p.CostoInventarioActual,
                          Estatus = p.Estatus
                      }).OrderByDescending(p => p.FechaInicio);

                    return stocks.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void RefreshProduct(ProductStockViewModel product)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var productStock = context.tProductosInventarios.FirstOrDefault(p =>
                        p.idProducto == product.idProducto && p.idInventario == product.idInventario);

                    if (productStock == null)
                    {
                        product.idProductosInventario = this.AddProductStock(new ProductStockViewModel()
                        {
                            idInventario = product.idInventario,
                            idProducto = product.idProducto,
                            CantidadAnterior = product.CantidadAnterior,
                            CantidadActual = product.CantidadActual,
                            Precio = product.Precio,
                            Inventariado = true
                        });
                    }
                    else
                    {
                        productStock.CantidadActual = product.CantidadActual;
                        productStock.Inventariado = true;

                        this.AddBinnacleProduct(product.idInventario, product.idProducto, (int)productStock.CantidadActual);
                    }

                    var stock = context.tInventarios.FirstOrDefault(p => p.idInventario == product.idInventario);

                    var productBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == stock.idSucursal
                                                                                        && p.idProducto == product.idProducto);

                    productBranch.Existencia = product.CantidadActual;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateProductStock(ProductStockViewModel product)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var productStock = context.tProductosInventarios.FirstOrDefault(p =>
                        p.idProductosInventario == product.idProductosInventario);

                    this.AddBinnacleProduct(product.idInventario, product.idProducto, (int)productStock.CantidadActual);

                    productStock.CantidadActual = product.CantidadActual;
                    productStock.Inventariado = true;

                    var stock = context.tInventarios.FirstOrDefault(p => p.idInventario == product.idInventario);

                    var productBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == stock.idSucursal
                                                                               && p.idProducto == product.idProducto);

                    if (productBranch != null)
                    {
                        productBranch.Existencia = product.CantidadActual;
                    }
                    else
                    {
                        tProductosSucursal producto = new tProductosSucursal();

                        producto.idSucursal = (int)stock.idSucursal;
                        producto.idProducto = product.idProducto;
                        producto.Existencia = product.CantidadActual;

                        context.tProductosSucursals.Add(producto);
                    }

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddStock(StockViewModel oStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tInventarios.Where(p => p.idSucursal == oStock.idSucursal && p.Estatus == TypesStock.EstatusPendiente).Any())
                    {
                        throw new Exception("Existe un inventario en progreso.");
                    }
                    else
                    {
                        tInventario oInventario = new tInventario();

                        oInventario.idSucursal = oStock.idSucursal;
                        oInventario.FechaInicio = oStock.FechaInicio;
                        oInventario.FechaFin = oStock.FechaFin;
                        oInventario.Estatus = TypesStock.EstatusPendiente;

                        context.tInventarios.Add(oInventario);
                        context.SaveChanges();

                        //Seleccionar todos los articulos activos y cargarlos al nuevo inventario
                        this.LoadActiveProductsToStock(oInventario.idInventario);
                        this.UpdateActualStockByProducts(oInventario.idInventario, (int)oStock.idSucursal);

                        this.BackUpDataBaseStart(oInventario.idInventario);

                        return oInventario.idInventario;
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetMissingProducts(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tProductosInventarios
                    .Join(context.tProductos,
                        prodinv => prodinv.idProducto,
                        prod => prod.idProducto,
                        (prodinv, prod) => new { ProdInv = prodinv, Prod = prod })
                    .Where(p => p.ProdInv.idInventario == idStock
                                && (p.ProdInv.CantidadAnterior > p.ProdInv.CantidadActual)
                                && p.Prod.Estatus == TypesGeneric.TypesProduct.EstatusActivo)
                    .Select(p => new ProductViewModel()
                    {
                        idProducto = p.Prod.idProducto,
                        Nombre = p.Prod.Nombre,
                        Codigo = p.Prod.Codigo,
                        Descripcion = p.Prod.Descripcion,
                        PrecioVenta = p.Prod.PrecioVenta,
                        PrecioCompra = p.Prod.PrecioCompra,
                        Estatus = (short)p.Prod.Estatus,
                        Marca = p.Prod.tProveedore.NombreEmpresa,
                        Proveedor = p.Prod.tProveedore.NombreEmpresa,
                        urlImagen = p.Prod.TipoImagen == 1
                            ? "/Content/Products/" + p.Prod.NombreImagen + p.Prod.Extension
                            : p.Prod.urlImagen,
                        CantidadAnterior = p.ProdInv.CantidadAnterior,
                        CantidadActual = p.ProdInv.CantidadActual
                    }).ToList();
            }
        }

        public void UpdateActualStockByProducts(int idStock, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductosInventarios.Where(p => p.idInventario == idStock).ToList();

                    foreach (var product in products)
                    {
                        var prod = context.tProductosSucursals.Where(p => p.idProducto == product.idProducto
                                                                          && p.idSucursal == idBranch).FirstOrDefault();
                        prod.Existencia = product.CantidadActual;
                    }

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStock(StockViewModel oStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tInventario oInventario = context.tInventarios.FirstOrDefault(p => p.idInventario == oStock.idInventario);

                    oInventario.FechaFin = DateTime.Now;
                    oInventario.DiferenciasNegativas = oStock.DiferenciasNegativas;
                    oInventario.DiferenciasPositivas = oStock.DiferenciasPositivas;
                    oInventario.ArticulosInventarioAnterior = oStock.ArticulosInventarioAnterior;
                    oInventario.ArticulosInventarioActual = oStock.ArticulosInventarioActual;
                    oInventario.CostoInventarioAnterior = oStock.CostoInventarioAnterior;
                    oInventario.CostoInventarioActual = oStock.CostoInventarioActual;
                    oInventario.Estatus = TypesStock.EstatusFinalizado;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void LoadActiveProductsToStock(int idStock)
        {
            try
            {
                var stock = this.GetStock(idStock);

                var products = new Products();

                var activesProducts = products.GetAllActivesProductByBranch((int)stock.idSucursal);

                //Agregar los productos a la tabla tProductosInventario
                foreach (var product in activesProducts)
                {
                    this.AddProductStock(new ProductStockViewModel()
                    {
                        idInventario = stock.idInventario,
                        idProducto = product.idProducto,
                        CantidadAnterior = (int)product.Stock,
                        CantidadActual = Convert.ToInt32((product.Vista ?? 0)),
                        Precio = product.PrecioVenta,
                        Inventariado = false
                    });
                }
            }
            catch (DbEntityValidationException ex)
            {
                var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                throw newException;
            }
        }

        public int GetAllProductsStockCount(int idStock, string description, int? idProduct, string code, decimal? cost, string category, string color, string material, string brand, bool stockZero)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                IOrderedQueryable<ProductViewModel> products = null;

                if (stockZero)
                {
                    products = context.tProductosInventarios.Where(p => p.idInventario == idStock
                                                                        && (p.tProducto.Descripcion.Contains(
                                                                                description) ||
                                                                            (String.IsNullOrEmpty(description)))
                                                                        && (p.idProducto == idProduct ||
                                                                            idProduct == null)
                                                                        && (p.tProducto.Codigo.Contains(code) ||
                                                                            (String.IsNullOrEmpty(code))) &&
                                                                        (p.tProducto.PrecioVenta == cost ||
                                                                         (cost == null))
                                                                        && (p.tProducto.tCategoria.Nombre.Contains(
                                                                                category) ||
                                                                            (String.IsNullOrEmpty(category))) &&
                                                                        (p.tProducto.Color.Contains(color) ||
                                                                         (String.IsNullOrEmpty(color)))
                                                                        && (p.tProducto.tMateriale.Material
                                                                                .Contains(material) ||
                                                                            (String.IsNullOrEmpty(material)))
                                                                        && (p.tProducto.tProveedore.NombreEmpresa
                                                                                .Contains(
                                                                                    brand) || String.IsNullOrEmpty(
                                                                                brand))
                                                                        && (p.CantidadAnterior == 0)
                        )
                        .Select(p => new ProductStockViewModel()
                        {
                            idProductosInventario = p.idProductosInventario,
                            idProducto = p.tProducto.idProducto,
                            Nombre = p.tProducto.Nombre,
                            Codigo = p.tProducto.Codigo,
                            Descripcion = p.tProducto.Descripcion,
                            PrecioVenta = p.tProducto.PrecioVenta,
                            Estatus = (short)p.tProducto.Estatus,
                            Marca = p.tProducto.tProveedore.NombreEmpresa,
                            Proveedor = p.tProducto.tProveedore.NombreEmpresa,
                            CantidadAnterior = p.CantidadAnterior,
                            CantidadActual = p.CantidadActual,
                            urlImagen = p.tProducto.TipoImagen == 1
                                ? "/Content/Products/" + p.tProducto.NombreImagen + p.tProducto.Extension
                                : p.tProducto.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto)
                                .Select(x => new ProductBranchViewModel()
                                {
                                    idSucursal = x.idSucursal,
                                    Sucursal = x.tSucursale.Nombre,
                                    Existencia = ((int?)x.Existencia) ?? 0
                                }).ToList(),
                            Vista = p.tProducto.tDetalleVistas.Where(v =>
                                    v.tVista.idSucursal == p.tInventario.idSucursal
                                    && v.idProducto == p.idProducto
                                    && v.Estatus == TypesOutProductsDetail.Pendiente)
                                .Sum(v => v.Cantidad),
                            oDetailView = p.tProducto.tDetalleVistas.Where(v =>
                                    v.tVista.idSucursal == p.tInventario.idSucursal
                                    && v.idProducto == p.idProducto
                                    && v.Estatus == TypesOutProductsDetail.Pendiente)
                                .Select(v => new DetailView()
                                {
                                    Remision = v.tVista.Remision,
                                    Cantidad = v.Cantidad
                                }).ToList()
                        }).OrderBy(p => p.Codigo);
                }
                else
                {
                    products = context.tProductosInventarios.Where(p => p.idInventario == idStock
                                                                        && (p.tProducto.Descripcion.Contains(
                                                                                description) ||
                                                                            (String.IsNullOrEmpty(description)))
                                                                        && (p.idProducto == idProduct ||
                                                                            idProduct == null)
                                                                        && (p.tProducto.Codigo.Contains(code) ||
                                                                            (String.IsNullOrEmpty(code))) &&
                                                                        (p.tProducto.PrecioVenta == cost ||
                                                                         (cost == null))
                                                                        && (p.tProducto.tCategoria.Nombre.Contains(
                                                                                category) ||
                                                                            (String.IsNullOrEmpty(category))) &&
                                                                        (p.tProducto.Color.Contains(color) ||
                                                                         (String.IsNullOrEmpty(color)))
                                                                        && (p.tProducto.tMateriale.Material.Contains(
                                                                                material) ||
                                                                            (String.IsNullOrEmpty(material)))
                                                                        && (p.tProducto.tProveedore.NombreEmpresa
                                                                                .Contains(brand) ||
                                                                            String.IsNullOrEmpty(brand))
                        )
                        .Select(p => new ProductStockViewModel()
                        {
                            idProductosInventario = p.idProductosInventario,
                            idProducto = p.tProducto.idProducto,
                            Nombre = p.tProducto.Nombre,
                            Codigo = p.tProducto.Codigo,
                            Descripcion = p.tProducto.Descripcion,
                            PrecioVenta = p.tProducto.PrecioVenta,
                            Estatus = (short)p.tProducto.Estatus,
                            Marca = p.tProducto.tProveedore.NombreEmpresa,
                            Proveedor = p.tProducto.tProveedore.NombreEmpresa,
                            CantidadAnterior = p.CantidadAnterior,
                            CantidadActual = p.CantidadActual,
                            urlImagen = p.tProducto.TipoImagen == 1
                                ? "/Content/Products/" + p.tProducto.NombreImagen + p.tProducto.Extension
                                : p.tProducto.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(
                                x => new ProductBranchViewModel()
                                {
                                    idSucursal = x.idSucursal,
                                    Sucursal = x.tSucursale.Nombre,
                                    Existencia = ((int?)x.Existencia) ?? 0
                                }).ToList(),
                            Vista = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                          && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                          && v.Estatus == TypesOutProductsDetail
                                                                              .Pendiente).Sum(v => v.Cantidad),
                            oDetailView = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                                && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                                && v.Estatus == TypesOutProductsDetail
                                                                                    .Pendiente).Select(v =>
                                new DetailView()
                                {
                                    Remision = v.tVista.Remision,
                                    Cantidad = v.Cantidad
                                }).ToList()
                        }).OrderBy(p => p.Codigo);
                }

                return products.Count();
            }
        }

        public List<ProductViewModel> GetAllProductsStock(int idStock, string description, int? idProduct, string code, decimal? cost, string category, string color, string material, string brand, bool stockZero, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    IOrderedQueryable<ProductViewModel> products = null;

                    if (stockZero)
                    {
                        products = context.tProductosInventarios.Where(p => p.idInventario == idStock
                                                                 && (p.tProducto.Descripcion.Contains(description) ||
                                                                     (String.IsNullOrEmpty(description)))
                                                                 && (p.idProducto == idProduct || idProduct == null)
                                                                 && (p.tProducto.Codigo.Contains(code) ||
                                                                     (String.IsNullOrEmpty(code))) &&
                                                                 (p.tProducto.PrecioVenta == cost || (cost == null))
                                                                 && (p.tProducto.tCategoria.Nombre.Contains(category) ||
                                                                     (String.IsNullOrEmpty(category))) &&
                                                                 (p.tProducto.Color.Contains(color) ||
                                                                  (String.IsNullOrEmpty(color)))
                                                                 && (p.tProducto.tMateriale.Material
                                                                         .Contains(material) ||
                                                                     (String.IsNullOrEmpty(material)))
                                                                 && (p.tProducto.tProveedore.NombreEmpresa.Contains(
                                                                         brand) || String.IsNullOrEmpty(brand))
                                                                 && (p.CantidadAnterior == 0)
                            )
                            .Select(p => new ProductStockViewModel()
                            {
                                idProductosInventario = p.idProductosInventario,
                                idProducto = p.tProducto.idProducto,
                                Nombre = p.tProducto.Nombre,
                                Codigo = p.tProducto.Codigo,
                                Descripcion = p.tProducto.Descripcion,
                                PrecioVenta = p.tProducto.PrecioVenta,
                                Estatus = (short)p.tProducto.Estatus,
                                Marca = p.tProducto.tProveedore.NombreEmpresa,
                                Proveedor = p.tProducto.tProveedore.NombreEmpresa,
                                CantidadAnterior = p.CantidadAnterior,
                                CantidadActual = p.CantidadActual,
                                urlImagen = p.tProducto.TipoImagen == 1
                                    ? "/Content/Products/" + p.tProducto.NombreImagen + p.tProducto.Extension
                                    : p.tProducto.urlImagen, //* file = 1, url = 2 */
                                _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto)
                                    .Select(x => new ProductBranchViewModel()
                                    {
                                        idSucursal = x.idSucursal,
                                        Sucursal = x.tSucursale.Nombre,
                                        Existencia = ((int?)x.Existencia) ?? 0
                                    }).ToList(),
                                Vista = p.tProducto.tDetalleVistas.Where(v =>
                                        v.tVista.idSucursal == p.tInventario.idSucursal
                                        && v.idProducto == p.idProducto
                                        && v.Estatus == TypesOutProductsDetail.Pendiente)
                                    .Sum(v => v.Cantidad),
                                oDetailView = p.tProducto.tDetalleVistas.Where(v =>
                                        v.tVista.idSucursal == p.tInventario.idSucursal
                                        && v.idProducto == p.idProducto
                                        && v.Estatus == TypesOutProductsDetail.Pendiente)
                                    .Select(v => new DetailView()
                                    {
                                        Remision = v.tVista.Remision,
                                        Cantidad = v.Cantidad
                                    }).ToList()
                            }).OrderBy(p => p.Codigo);
                    }
                    else
                    {
                        products = context.tProductosInventarios.Where(p => p.idInventario == idStock
                           && (p.tProducto.Descripcion.Contains(description) || (String.IsNullOrEmpty(description)))
                           && (p.idProducto == idProduct || idProduct == null)
                           && (p.tProducto.Codigo.Contains(code) || (String.IsNullOrEmpty(code))) && (p.tProducto.PrecioVenta == cost || (cost == null))
                           && (p.tProducto.tCategoria.Nombre.Contains(category) || (String.IsNullOrEmpty(category))) && (p.tProducto.Color.Contains(color) || (String.IsNullOrEmpty(color)))
                           && (p.tProducto.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                           && (p.tProducto.tProveedore.NombreEmpresa.Contains(brand) || String.IsNullOrEmpty(brand))
                       )
                       .Select(p => new ProductStockViewModel()
                       {
                           idProductosInventario = p.idProductosInventario,
                           idProducto = p.tProducto.idProducto,
                           Nombre = p.tProducto.Nombre,
                           Codigo = p.tProducto.Codigo,
                           Descripcion = p.tProducto.Descripcion,
                           PrecioVenta = p.tProducto.PrecioVenta,
                           Estatus = (short)p.tProducto.Estatus,
                           Marca = p.tProducto.tProveedore.NombreEmpresa,
                           Proveedor = p.tProducto.tProveedore.NombreEmpresa,
                           CantidadAnterior = p.CantidadAnterior,
                           CantidadActual = p.CantidadActual,
                           urlImagen = p.tProducto.TipoImagen == 1 ? "/Content/Products/" + p.tProducto.NombreImagen + p.tProducto.Extension : p.tProducto.urlImagen, //* file = 1, url = 2 */
                           _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(x => new ProductBranchViewModel()
                           {
                               idSucursal = x.idSucursal,
                               Sucursal = x.tSucursale.Nombre,
                               Existencia = ((int?)x.Existencia) ?? 0
                           }).ToList(),
                           Vista = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                         && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                         && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => v.Cantidad),
                           oDetailView = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                               && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                               && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                                                                               {
                                                                                   Remision = v.tVista.Remision,
                                                                                   Cantidad = v.Cantidad
                                                                               }).ToList()
                       }).OrderBy(p => p.Codigo);
                    }

                    return products.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public InformationStockViewModel GetCompleteInformationStock(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tInventarios.Where(p => p.idInventario == idStock).Select(p => new InformationStockViewModel()
                    {

                        AmountNegatives = p.DiferenciasNegativas,
                        AmountPositives = p.DiferenciasPositivas,
                        AmountOldStock = p.ArticulosInventarioAnterior,
                        AmountNewStock = p.ArticulosInventarioActual,
                        CostOldStock = p.CostoInventarioAnterior,
                        CostNewStock = p.CostoInventarioActual

                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int GetAllProductsStockDifCount(int idStock, string description, string code, decimal? cost, string category, string color, string material, string brand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductosInventarios.Where(p => p.idInventario == idStock
                            && (p.tProducto.Descripcion.Contains(description) || (String.IsNullOrEmpty(description)))
                            && (p.tProducto.Codigo.Contains(code) || (String.IsNullOrEmpty(code))) && (p.tProducto.PrecioVenta == cost || (cost == null))
                            && (p.tProducto.tCategoria.Nombre.Contains(category) || (String.IsNullOrEmpty(category))) && (p.tProducto.Color.Contains(color) || (String.IsNullOrEmpty(color)))
                            && (p.tProducto.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                            && (p.tProducto.tProveedore.NombreEmpresa.Contains(brand) || String.IsNullOrEmpty(brand))
                            && (p.CantidadAnterior > p.CantidadActual)
                        )
                        .Select(p => new ProductStockViewModel()
                        {
                            idProductosInventario = p.idProductosInventario,
                            idProducto = p.tProducto.idProducto,
                            Nombre = p.tProducto.Nombre,
                            Codigo = p.tProducto.Codigo,
                            Descripcion = p.tProducto.Descripcion,
                            PrecioVenta = p.tProducto.PrecioVenta,
                            Estatus = (short)p.tProducto.Estatus,
                            Marca = p.tProducto.tProveedore.NombreEmpresa,
                            Proveedor = p.tProducto.tProveedore.NombreEmpresa,
                            CantidadAnterior = p.CantidadAnterior,
                            CantidadActual = p.CantidadActual,
                            urlImagen = p.tProducto.TipoImagen == 1 ? "/Content/Products/" + p.tProducto.NombreImagen + p.tProducto.Extension : p.tProducto.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(x => new ProductBranchViewModel()
                            {
                                idSucursal = x.idSucursal,
                                Sucursal = x.tSucursale.Nombre,
                                Existencia = ((int?)x.Existencia) ?? 0
                            }).ToList(),
                            Vista = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                          && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                          && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => v.Cantidad),
                            oDetailView = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                                && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                                && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                                                                                {
                                                                                    Remision = v.tVista.Remision,
                                                                                    Cantidad = v.Cantidad
                                                                                }).ToList()
                        }).OrderBy(p => p.Codigo);

                    return products.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductStockViewModel> GetAllProductsStockDif(int idStock, string description, string code, decimal? cost, string category, string color, string material, string brand, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductosInventarios.Where(p => p.idInventario == idStock
                            && (p.tProducto.Descripcion.Contains(description) || (String.IsNullOrEmpty(description)))
                            && (p.tProducto.Codigo.Contains(code) || (String.IsNullOrEmpty(code))) && (p.tProducto.PrecioVenta == cost || (cost == null))
                            && (p.tProducto.tCategoria.Nombre.Contains(category) || (String.IsNullOrEmpty(category))) && (p.tProducto.Color.Contains(color) || (String.IsNullOrEmpty(color)))
                            && (p.tProducto.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                            && (p.tProducto.tProveedore.NombreEmpresa.Contains(brand) || String.IsNullOrEmpty(brand))
                            && (p.CantidadAnterior > p.CantidadActual)
                        )
                        .Select(p => new ProductStockViewModel()
                        {
                            idProductosInventario = p.idProductosInventario,
                            idProducto = p.tProducto.idProducto,
                            Nombre = p.tProducto.Nombre,
                            Codigo = p.tProducto.Codigo,
                            Descripcion = p.tProducto.Descripcion,
                            PrecioVenta = p.tProducto.PrecioVenta,
                            Estatus = (short)p.tProducto.Estatus,
                            Marca = p.tProducto.tProveedore.NombreEmpresa,
                            Proveedor = p.tProducto.tProveedore.NombreEmpresa,
                            CantidadAnterior = p.CantidadAnterior,
                            CantidadActual = p.CantidadActual,
                            urlImagen = p.tProducto.TipoImagen == 1 ? "/Content/Products/" + p.tProducto.NombreImagen + p.tProducto.Extension : p.tProducto.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(x => new ProductBranchViewModel()
                            {
                                idSucursal = x.idSucursal,
                                Sucursal = x.tSucursale.Nombre,
                                Existencia = ((int?)x.Existencia) ?? 0
                            }).ToList(),
                            Vista = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                          && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                          && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => v.Cantidad),
                            oDetailView = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                                && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                                && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                                                                                {
                                                                                    Remision = v.tVista.Remision,
                                                                                    Cantidad = v.Cantidad
                                                                                }).ToList()
                        }).OrderBy(p => p.Codigo);

                    return products.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductStockViewModel> GetAllProductsStockDifForExport(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosInventarios.Where(p => p.idInventario == idStock
                            && (p.CantidadAnterior > p.CantidadActual)
                        )
                        .Select(p => new ProductStockViewModel()
                        {
                            idProductosInventario = p.idProductosInventario,
                            idProducto = p.tProducto.idProducto,
                            Nombre = p.tProducto.Nombre,
                            Codigo = p.tProducto.Codigo,
                            Descripcion = p.tProducto.Descripcion,
                            PrecioVenta = p.tProducto.PrecioVenta,
                            Estatus = (short)p.tProducto.Estatus,
                            Marca = p.tProducto.tProveedore.NombreEmpresa,
                            Proveedor = p.tProducto.tProveedore.NombreEmpresa,
                            CantidadAnterior = p.CantidadAnterior,
                            CantidadActual = p.CantidadActual,
                            urlImagen = p.tProducto.TipoImagen == 1 ? "/Content/Products/" + p.tProducto.NombreImagen + p.tProducto.Extension : p.tProducto.urlImagen, //* file = 1, url = 2 */
                            Fecha = p.tInventario.FechaInicio,
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(x => new ProductBranchViewModel()
                            {
                                idSucursal = x.idSucursal,
                                Sucursal = x.tSucursale.Nombre,
                                Existencia = ((int?)x.Existencia) ?? 0
                            }).ToList(),
                            Vista = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                          && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                          && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => v.Cantidad),
                            oDetailView = p.tProducto.tDetalleVistas.Where(v => v.idProducto == p.idProducto
                                                                                && v.tVista.idSucursal == p.tInventario.idSucursal
                                                                                && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                                                                                {
                                                                                    Remision = v.tVista.Remision,
                                                                                    Cantidad = v.Cantidad
                                                                                }).ToList()
                        }).OrderBy(p => p.Codigo).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddProductStock(ProductStockViewModel oProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProductosInventario oProductStock = new tProductosInventario();

                    oProductStock.idInventario = oProduct.idInventario;
                    oProductStock.idProducto = oProduct.idProducto;
                    oProductStock.CantidadAnterior = oProduct.CantidadAnterior;
                    oProductStock.CantidadActual = oProduct.CantidadActual;
                    oProductStock.Precio = oProduct.Precio;
                    oProductStock.Inventariado = oProduct.Inventariado;

                    context.tProductosInventarios.Add(oProductStock);
                    context.SaveChanges();

                    this.AddBinnacleProduct(oProduct.idInventario, (int)oProduct.idProducto, 0);

                    return oProductStock.idProductosInventario;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateProductStock(int idStock, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProductosInventario oProductStock = context.tProductosInventarios.FirstOrDefault(p => p.idInventario == idStock
                                                                                                         && p.idProducto == idProduct);
                    this.AddBinnacleProduct(idStock, idProduct, (int)oProductStock.CantidadActual);

                    oProductStock.idInventario = idStock;
                    oProductStock.idProducto = idProduct;
                    oProductStock.CantidadActual = oProductStock.CantidadActual + 1;
                    oProductStock.Inventariado = true;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void AddBinnacleProduct(int idStock, int idProduct, int Amount)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                tBitacoraProductosInventario entity = new tBitacoraProductosInventario();

                entity.idInventario = idStock;
                entity.idProducto = idProduct;
                entity.Cantidad = Amount;
                entity.Fecha = DateTime.Now;

                context.tBitacoraProductosInventarios.Add(entity);

                context.SaveChanges();
            }
        }

        public void BackUpDataBaseStart(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tInventario oStock = context.tInventarios.FirstOrDefault(p => p.idInventario == idStock);

                    string branch = context.tInventarios.Join(context.tSucursales,
                        inv => inv.idSucursal,
                        suc => suc.idSucursal,
                        (inv, suc) => new { inv, suc })
                        .Select(insu => insu.suc.Nombre).FirstOrDefault();

                    string sqlCommand = @"BACKUP DATABASE admDB_SAADDB TO DISK = N'{0}'";

                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, @"C:\BACKUP SAADDB STOCK\admDB_SAADDB_STOCK_Start_" + branch + oStock.FechaInicio.Value.ToString("MMyyyy") + ".bak"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void BackUpDataBaseEnd(int idStock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tInventario oStock = context.tInventarios.FirstOrDefault(p => p.idInventario == idStock);

                    string branch = context.tInventarios.Join(context.tSucursales,
                            inv => inv.idSucursal,
                            suc => suc.idSucursal,
                            (inv, suc) => new { inv, suc })
                        .Select(insu => insu.suc.Nombre).FirstOrDefault();

                    string sqlCommand = @"BACKUP DATABASE admDB_SAADDB TO DISK = N'{0}'";

                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, @"C:\BACKUP SAADDB STOCK\admDB_SAADDB_STOCK_End_" + branch + oStock.FechaInicio.Value.ToString("MMyyyy") + ".bak"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public InformationStockViewModel GetInformationStock(int idStock, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    InformationStockViewModel informationStock = new InformationStockViewModel();

                    informationStock.AmountNegatives = context.tProductosInventarios
                        .Where(p => (p.idInventario == idStock && p.CantidadAnterior > p.CantidadActual))
                        .Count();

                    informationStock.AmountPositives = context.tProductosInventarios
                        .Where(p => (p.idInventario == idStock && p.CantidadActual > p.CantidadAnterior))
                        .Count();

                    informationStock.AmountOldStock = context.tProductos.Join(context.tProductosSucursals,
                        prod => prod.idProducto,
                        suc => suc.idProducto,
                        (prod, suc) => new { prod, suc })
                        .Where(ps => ps.prod.Estatus == TypesProduct.EstatusActivo && ps.suc.idSucursal == idBranch)
                        .Select(ps => ps.suc.Existencia).Sum();

                    informationStock.AmountNewStock = context.tProductosInventarios.Join(context.tProductosSucursals,
                        prod => prod.idProducto,
                        suc => suc.idProducto,
                        (prod, suc) => new { prod, suc })
                        .Where(ps => ps.prod.idInventario == idStock && ps.suc.idSucursal == idBranch)
                        .Select(ps => ps.prod.CantidadActual).Sum();

                    informationStock.CostOldStock = context.tProductos.Join(context.tProductosSucursals,
                        prod => prod.idProducto,
                        suc => suc.idProducto,
                        (prod, suc) => new { prod, suc })
                        .Where(ps => ps.prod.Estatus == TypesProduct.EstatusActivo && ps.suc.idSucursal == idBranch)
                        .Select(ps => ps.suc.Existencia * ps.prod.PrecioVenta).Sum();

                    informationStock.CostNewStock = context.tProductosInventarios.Join(context.tProductosSucursals,
                        prod => prod.idProducto,
                        suc => suc.idProducto,
                        (prod, suc) => new { prod, suc })
                        .Where(ps => ps.prod.idInventario == idStock && ps.suc.idSucursal == idBranch)
                        .Select(ps => ps.prod.CantidadActual * ps.prod.Precio).Sum();

                    return informationStock;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

    }
}