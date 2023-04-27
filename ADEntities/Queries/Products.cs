using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Products : Base
    {
        //
        public bool ValidateProductExist(string codigo)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo && p.Codigo.ToUpper().Trim() == codigo.ToUpper().Trim()).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool ValidateProductExist(int idProducto, string codigo)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                    && p.idProducto != idProducto
                    && p.Codigo.ToUpper().Trim() == codigo.ToUpper().Trim()).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegistersWithFilters(string description, string code, decimal? cost, string category, string subcategory, string color, string material, string brand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category)))
                        && (p.tSubcategoria.Nombre.Trim().ToUpper().Contains(subcategory.Trim().ToUpper()) || (String.IsNullOrEmpty(subcategory)))
                        && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand)))
                        .Count();

                    return products;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegistersWithFiltersWithDamage(string description, string code, decimal? cost, string category, string subcategory, string color, string material, string brand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                                                                 && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                                                                 && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                                                                 && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category)))
                                                                 && (p.tSubcategoria.Nombre.Trim().ToUpper().Contains(subcategory.Trim().ToUpper()) || (String.IsNullOrEmpty(subcategory)))
                                                                 && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                                                                 && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                                                                 && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand))
                                                                 && (p.tDetalleReparaciones.Where(t => t.idProducto == p.idProducto && t.Pendiente > 0).Sum(t => t.Pendiente ?? 0) > 0))
                        .Count();

                    return products;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegistersWithFilters(string description, string code, decimal? cost, string category, string subcategory, string color, string material, string brand, short? branch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category)))
                        && (p.tSubcategoria.Nombre.Trim().ToUpper().Contains(subcategory.Trim().ToUpper()) || (String.IsNullOrEmpty(subcategory)))
                        && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand))
                        && (p.tProductosSucursals.Where(ps => ((ps.idSucursal == branch)) && ps.Existencia > 0).Any()))
                        .Count();

                    return products;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegistersWithFiltersPrintBarCode(string description, string code, decimal? cost, string category, string color, string material, string order, string brand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category))) && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand))
                         && (p.tOrdenProductoes.Any(x => (x.tOrden.Orden == order || x.tOrden.Factura == order) || String.IsNullOrEmpty(order))))
                        .Count();

                    return products;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegistersWithFiltersPrintBarCode(string description, string code, decimal? cost, string category, string color, string material, string order, string brand, short? branch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category))) && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand))
                        && (p.tProductosSucursals.Where(ps => ((ps.idSucursal == branch)) && ps.Existencia > 0).Any())
                         && (p.tOrdenProductoes.Any(x => (x.tOrden.Orden == order || x.tOrden.Factura == order) || String.IsNullOrEmpty(order))))
                        .Count();

                    return products;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int CountRegistersForBranch(int idBranch, string description, string code, decimal? cost, string category, string color, string material, string brand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.tProductosSucursals.Where(x => x.idSucursal == idBranch).Count() > 0)
                        && (p.Descripcion.Contains(description) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Contains(code) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Contains(category) || (String.IsNullOrEmpty(category))) && (p.Color.Contains(color) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Contains(brand) || String.IsNullOrEmpty(brand)))
                        .Count();

                    return products;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProducts()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo)
                        .Select(p => new ProductViewModel()
                        {

                            idProducto = p.idProducto,
                            Nombre = p.Nombre

                        }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProducts(string nameProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo && p.Nombre.Contains(nameProduct))
                        .Select(p => new ProductViewModel()
                        {

                            idProducto = p.idProducto,
                            Nombre = p.Nombre

                        }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetCodigos()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo)
                       .Select(p => new ProductViewModel()
                       {

                           idProducto = p.idProducto,
                           Codigo = p.Codigo

                       }).Distinct().OrderBy(p => p.Codigo).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetCodigos(string codigo)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo && p.Codigo.Contains(codigo))
                       .Select(p => new ProductViewModel()
                       {

                           idProducto = p.idProducto,
                           Codigo = p.Codigo

                       }).Distinct().OrderBy(p => p.Codigo).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetPrecios()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo)
                       .Select(p => new ProductViewModel()
                       {

                           PrecioVenta = p.PrecioVenta

                       }).Distinct().OrderBy(p => p.PrecioVenta).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetColors()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo)
                       .Select(p => new ProductViewModel()
                       {

                           Color = p.Color

                       }).Distinct().OrderBy(p => p.Color).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetColors(string color)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo && p.Color.Contains(color))
                       .Select(p => new ProductViewModel()
                       {

                           Color = p.Color

                       }).Distinct().OrderBy(p => p.Color).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetBrands()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo && p.tProveedore.Estatus == TypesProvider.EstatusActivo)
                       .Select(p => new ProductViewModel()
                       {

                           Marca = p.tProveedore.NombreEmpresa

                       }).Distinct().OrderBy(p => p.Marca).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<BrandViewModel> GetBrands(string brand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProveedores.Where(p => p.NombreEmpresa.Contains(brand) && p.Estatus == TypesProvider.EstatusActivo)
                       .Select(p => new BrandViewModel()
                       {

                           Nombre = p.NombreEmpresa

                       }).Distinct().OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetDescription(string description)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo && p.Descripcion.Contains(description))
                       .Select(p => new ProductViewModel()
                       {

                           Descripcion = p.Descripcion

                       }).Distinct().OrderBy(p => p.Descripcion).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProducts(string description, string code, decimal? cost, string category, string subcategory, string color, string material, string brand, short? branch, int orderBy, bool ascending, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                                                                 && (p.Descripcion.Trim().ToUpper()
                                                                         .Contains(description.Trim().ToUpper()) ||
                                                                     (String.IsNullOrEmpty(description)))
                                                                 && (p.Codigo.Trim().ToUpper()
                                                                         .Contains(code.Trim().ToUpper()) ||
                                                                     (String.IsNullOrEmpty(code))) &&
                                                                 (p.PrecioVenta == cost || (cost == null))
                                                                 && (p.tCategoria.Nombre.Trim().ToUpper()
                                                                         .Contains(category.Trim().ToUpper()) ||
                                                                     (String.IsNullOrEmpty(category)))
                                                                 && (p.tSubcategoria.Nombre.Trim().ToUpper()
                                                                         .Contains(subcategory.Trim().ToUpper()) ||
                                                                     (String.IsNullOrEmpty(subcategory))) &&
                                                                 (p.Color.Trim().ToUpper()
                                                                      .Contains(color.Trim().ToUpper()) ||
                                                                  (String.IsNullOrEmpty(color)))
                                                                 && (p.tMateriale.Material.Contains(material) ||
                                                                     (String.IsNullOrEmpty(material)))
                                                                 && (p.tProveedore.NombreEmpresa.Trim().ToUpper()
                                                                         .Contains(brand.Trim().ToUpper()) ||
                                                                     String.IsNullOrEmpty(brand))
                                                                 && (p.tProductosSucursals.Any(ps =>
                                                                         ps.idSucursal == branch &&
                                                                         ps.Existencia > 0)))
                        .Select(p => new ProductViewModel()
                        {
                            idProducto = p.idProducto,
                            Nombre = p.Nombre,
                            Codigo = p.Codigo,
                            Descripcion = p.Descripcion,
                            PrecioVenta = p.PrecioVenta,
                            PrecioCompra = p.PrecioCompra,
                            Estatus = (short)p.Estatus,
                            Marca = p.tProveedore.NombreEmpresa,
                            Proveedor = p.tProveedore.NombreEmpresa,
                            Medida = p.Medida,
                            urlImagen = p.TipoImagen == 1
                                ? "/Content/Products/" + p.NombreImagen + p.Extension
                                : p.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals
                                .Where(x => x.idProducto == p.idProducto && x.idSucursal == branch &&
                                            (x.Existencia > 0)).Select(x => new ProductBranchViewModel()
                                            {
                                                idSucursal = x.idSucursal,
                                                Sucursal = x.tSucursale.Nombre,
                                                Existencia = ((int?)x.Existencia) ?? 0
                                            }).ToList(),
                            Vista = p.tDetalleVistas
                                .Where(v => v.idProducto == p.idProducto &&
                                            v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v =>
                                    (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                            Taller = p.tDetalleReparaciones.Where(t => t.idProducto == p.idProducto && t.Pendiente > 0)
                                .Sum(t => t.Pendiente ?? 0),
                            oDetailView = p.tDetalleVistas
                                .Where(v => v.idProducto == p.idProducto &&
                                            v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                                            {
                                                Remision = v.tVista.Remision,
                                                Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))
                                            }).ToList(),
                            Total = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto && x.idSucursal == branch)
                                .Sum(x => x.Existencia)
                        });

                    switch (orderBy)
                    {
                        case 1:
                            products = (ascending) ? products.OrderBy(s => s.Codigo) : products.OrderByDescending(s => s.Codigo);
                            break;
                        case 2:
                            products = (ascending) ? products.OrderBy(s => s.PrecioVenta) : products.OrderByDescending(s => s.PrecioVenta);
                            break;
                        case 3:
                            products = (ascending) ? products.OrderBy(s => s.Total) : products.OrderByDescending(s => s.Total);
                            break;
                    }

                    var result = products.ToList();

                    return result.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProductsWithDamage(string description, string code, decimal? cost, string category, string subcategory, string color, string material, string brand, int orderBy, bool ascending, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category)))
                        && (p.tSubcategoria.Nombre.Trim().ToUpper().Contains(subcategory.Trim().ToUpper()) || (String.IsNullOrEmpty(subcategory)))
                        && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand))
                        && (p.tDetalleReparaciones.Where(t => t.idProducto == p.idProducto && t.Pendiente > 0).Sum(t => t.Pendiente ?? 0) > 0))
                        .Select(p => new ProductViewModel()
                        {
                            idProducto = p.idProducto,
                            Nombre = p.Nombre,
                            Codigo = p.Codigo,
                            Descripcion = p.Descripcion,
                            PrecioVenta = p.PrecioVenta,
                            PrecioCompra = p.PrecioCompra,
                            Estatus = (short)p.Estatus,
                            Marca = p.tProveedore.NombreEmpresa,
                            Proveedor = p.tProveedore.NombreEmpresa,
                            Medida = p.Medida,
                            urlImagen = p.TipoImagen == 1 ? "/Content/Products/" + p.NombreImagen + p.Extension : p.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(x => new ProductBranchViewModel()
                            {

                                idSucursal = x.idSucursal,
                                Sucursal = x.tSucursale.Nombre,
                                Existencia = ((int?)x.Existencia) ?? 0

                            }).ToList(),
                            Vista = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                            Taller = p.tDetalleReparaciones.Where(t => t.idProducto == p.idProducto && t.Pendiente > 0).Sum(t => t.Pendiente ?? 0),
                            oDetailView = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                            {

                                Remision = v.tVista.Remision,
                                Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))

                            }).ToList(),
                            Total = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Sum(x => x.Existencia)
                        });

                    switch (orderBy)
                    {
                        case 1:
                            products = (ascending) ? products.OrderBy(s => s.Codigo) : products.OrderByDescending(s => s.Codigo);
                            break;
                        case 2:
                            products = (ascending) ? products.OrderBy(s => s.PrecioVenta) : products.OrderByDescending(s => s.PrecioVenta);
                            break;
                        case 3:
                            products = (ascending) ? products.OrderBy(s => s._Existencias.Sum(p => p.Existencia)) : products.OrderByDescending(s => s._Existencias.Sum(p => p.Existencia));
                            break;
                    }

                    var result = products.ToList();

                    return result.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProducts(string description, string code, decimal? cost, string category, string subcategory, string color, string material, string brand, int orderBy, bool ascending, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code))) && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category)))
                        && (p.tSubcategoria.Nombre.Trim().ToUpper().Contains(subcategory.Trim().ToUpper()) || (String.IsNullOrEmpty(subcategory)))
                        && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand)))
                        .Select(p => new ProductViewModel()
                        {
                            idProducto = p.idProducto,
                            Nombre = p.Nombre,
                            Codigo = p.Codigo,
                            Descripcion = p.Descripcion,
                            PrecioVenta = p.PrecioVenta,
                            PrecioCompra = p.PrecioCompra,
                            Estatus = (short)p.Estatus,
                            Marca = p.tProveedore.NombreEmpresa,
                            Proveedor = p.tProveedore.NombreEmpresa,
                            Medida = p.Medida,
                            urlImagen = p.TipoImagen == 1 ? "/Content/Products/" + p.NombreImagen + p.Extension : p.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(x => new ProductBranchViewModel()
                            {

                                idSucursal = x.idSucursal,
                                Sucursal = x.tSucursale.Nombre,
                                Existencia = ((int?)x.Existencia) ?? 0

                            }).ToList(),
                            Vista = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                            Taller = p.tDetalleReparaciones.Where(t => t.idProducto == p.idProducto && t.Pendiente > 0).Sum(t => t.Pendiente ?? 0),
                            oDetailView = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                            {

                                Remision = v.tVista.Remision,
                                Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))

                            }).ToList(),
                            Total = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Sum(x => x.Existencia)

                        });

                    switch (orderBy)
                    {
                        case 1:
                            products = (ascending) ? products.OrderBy(s => s.Codigo) : products.OrderByDescending(s => s.Codigo);
                            break;
                        case 2:
                            products = (ascending) ? products.OrderBy(s => s.PrecioVenta) : products.OrderByDescending(s => s.PrecioVenta);
                            break;
                        case 3:
                            products = (ascending) ? products.OrderBy(s => s._Existencias.Sum(p => p.Existencia)) : products.OrderByDescending(s => s._Existencias.Sum(p => p.Existencia));
                            break;
                    }

                    var result = products.ToList();

                    return result.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProductsPrintBarCode(string description, string code, decimal? cost, string category, string color, string material, string order, string brand, short? branch, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                         && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                         && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code)))
                         && (p.PrecioVenta == cost || (cost == null))
                         && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category)))
                         && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                         && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                         && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand))
                         && (p.tOrdenProductoes.Any(x => (x.tOrden.Orden == order || x.tOrden.Factura == order)) || String.IsNullOrEmpty(order)))
                         .Select(p => new ProductViewModel()
                         {
                             idProducto = p.idProducto,
                             Nombre = p.Nombre,
                             Codigo = p.Codigo,
                             Descripcion = p.Descripcion,
                             PrecioVenta = p.PrecioVenta,
                             PrecioCompra = p.PrecioCompra,
                             Estatus = (short)p.Estatus,
                             Marca = p.tProveedore.NombreEmpresa,
                             Proveedor = p.tProveedore.NombreEmpresa,
                             urlImagen = p.TipoImagen == 1 ? "/Content/Products/" + p.NombreImagen + p.Extension : p.urlImagen, //* file = 1, url = 2 */
                             _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto && x.idSucursal == branch && (x.Existencia > 0)).Select(x => new ProductBranchViewModel()
                             {

                                 idSucursal = x.idSucursal,
                                 Sucursal = x.tSucursale.Nombre,
                                 Existencia = ((int?)x.Existencia) ?? 0

                             }).ToList(),
                             Vista = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => (v.Cantidad ?? 0) - (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                             oDetailView = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                             {

                                 Remision = v.tVista.Remision,
                                 Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))

                             }).ToList(),
                             Total = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Sum(x => x.Existencia)

                         }).OrderByDescending(p => p.Total);

                    return products.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProductsPrintBarCode(string description, string code, decimal? cost, string category, string color, string material, string order, string brand, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                        && (p.Descripcion.Trim().ToUpper().Contains(description.Trim().ToUpper()) || (String.IsNullOrEmpty(description)))
                        && (p.Codigo.Trim().ToUpper().Contains(code.Trim().ToUpper()) || (String.IsNullOrEmpty(code)))
                        && (p.PrecioVenta == cost || (cost == null))
                        && (p.tCategoria.Nombre.Trim().ToUpper().Contains(category.Trim().ToUpper()) || (String.IsNullOrEmpty(category)))
                        && (p.Color.Trim().ToUpper().Contains(color.Trim().ToUpper()) || (String.IsNullOrEmpty(color)))
                        && (p.tMateriale.Material.Contains(material) || (String.IsNullOrEmpty(material)))
                        && (p.tProveedore.NombreEmpresa.Trim().ToUpper().Contains(brand.Trim().ToUpper()) || String.IsNullOrEmpty(brand))
                        && (p.tOrdenProductoes.Any(x => (x.tOrden.Orden == order || x.tOrden.Factura == order)) || String.IsNullOrEmpty(order)))
                        .Select(p => new ProductViewModel()
                        {
                            idProducto = p.idProducto,
                            Nombre = p.Nombre,
                            Codigo = p.Codigo,
                            Descripcion = p.Descripcion,
                            PrecioVenta = p.PrecioVenta,
                            PrecioCompra = p.PrecioCompra,
                            Estatus = (short)p.Estatus,
                            Marca = p.tProveedore.NombreEmpresa,
                            Proveedor = p.tProveedore.NombreEmpresa,
                            urlImagen = p.TipoImagen == 1 ? "/Content/Products/" + p.NombreImagen + p.Extension : p.urlImagen, //* file = 1, url = 2 */
                            _Existencias = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Select(x => new ProductBranchViewModel()
                            {

                                idSucursal = x.idSucursal,
                                Sucursal = x.tSucursale.Nombre,
                                Existencia = ((int?)x.Existencia) ?? 0

                            }).ToList(),
                            Vista = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v => (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                            oDetailView = p.tDetalleVistas.Where(v => v.idProducto == p.idProducto && v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                            {

                                Remision = v.tVista.Remision,
                                Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))

                            }).ToList(),
                            Total = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto).Sum(x => x.Existencia)

                        }).OrderByDescending(p => p.Total);

                    return products.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProductsForBranch(int idStock, int idBranch, string description, string code, decimal? cost, string category, string color, string material, string brand, bool stockZero, int page, int pageSize, bool orderASC)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    IQueryable<ProductViewModel> products = null;

                    if (stockZero)
                    {
                        products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                                                      && (p.Descripcion.Contains(description) ||
                                                          (String.IsNullOrEmpty(description)))
                                                      && (p.Codigo.Contains(code) || (String.IsNullOrEmpty(code))) &&
                                                      (p.PrecioVenta == cost || (cost == null))
                                                      && (p.tCategoria.Nombre.Contains(category) ||
                                                          (String.IsNullOrEmpty(category))) &&
                                                      (p.Color.Contains(color) || (String.IsNullOrEmpty(color)))
                                                      && (p.tMateriale.Material.Contains(material) ||
                                                          (String.IsNullOrEmpty(material)))
                                                      && (p.tProveedore.NombreEmpresa.Contains(brand) ||
                                                          String.IsNullOrEmpty(brand))
                                                      && (p.tProductosSucursals.Any(s => s.idSucursal == idBranch && s.Existencia == 0)))
                            .Select(p => new ProductViewModel()
                            {

                                idProducto = p.idProducto,
                                Nombre = p.Nombre,
                                Codigo = p.Codigo,
                                Descripcion = p.Descripcion,
                                PrecioVenta = p.PrecioVenta,
                                PrecioCompra = p.PrecioCompra,
                                Estatus = (short)p.Estatus,
                                Marca = p.tProveedore.NombreEmpresa,
                                Proveedor = p.tProveedore.NombreEmpresa,
                                idSucursal = idBranch,
                                urlImagen = p.TipoImagen == 1
                                    ? "/Content/Products/" + p.NombreImagen + p.Extension
                                    : p.urlImagen, //* file = 1, url = 2 */
                                _Existencias = context.tProductosSucursals
                                    .Where(x => x.idProducto == p.idProducto && x.idSucursal == idBranch).Select(x =>
                                        new ProductBranchViewModel()
                                        {
                                            idSucursal = x.idSucursal,
                                            Sucursal = x.tSucursale.Nombre,
                                            Existencia = ((int?)x.Existencia) ?? 0
                                        }).ToList(),
                                Vista = p.tDetalleVistas
                                    .Where(v => v.idProducto == p.idProducto && v.idSucursal == idBranch &&
                                                v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v =>
                                        (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                                oDetailView = p.tDetalleVistas
                                    .Where(v => v.idProducto == p.idProducto &&
                                                v.Estatus == TypesOutProductsDetail.Pendiente).Select(v =>
                                        new DetailView()
                                        {
                                            Remision = v.tVista.Remision,
                                            Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))
                                        }).ToList(),
                                Total = context.tProductosSucursals
                                    .Where(x => x.idProducto == p.idProducto && x.idSucursal == idBranch)
                                    .Sum(x => x.Existencia)
                            });
                    }
                    else
                    {
                        products = context.tProductos.Where(p => p.Estatus == TypesProduct.EstatusActivo
                                                     && (p.Descripcion.Contains(description) ||
                                                         (String.IsNullOrEmpty(description)))
                                                     && (p.Codigo.Contains(code) || (String.IsNullOrEmpty(code))) &&
                                                     (p.PrecioVenta == cost || (cost == null))
                                                     && (p.tCategoria.Nombre.Contains(category) ||
                                                         (String.IsNullOrEmpty(category))) &&
                                                     (p.Color.Contains(color) || (String.IsNullOrEmpty(color)))
                                                     && (p.tMateriale.Material.Contains(material) ||
                                                         (String.IsNullOrEmpty(material)))
                                                     && (p.tProveedore.NombreEmpresa.Contains(brand) ||
                                                         String.IsNullOrEmpty(brand)))
                           .Select(p => new ProductViewModel()
                           {
                               idProducto = p.idProducto,
                               Nombre = p.Nombre,
                               Codigo = p.Codigo,
                               Descripcion = p.Descripcion,
                               PrecioVenta = p.PrecioVenta,
                               PrecioCompra = p.PrecioCompra,
                               Estatus = (short)p.Estatus,
                               Marca = p.tProveedore.NombreEmpresa,
                               Proveedor = p.tProveedore.NombreEmpresa,
                               idSucursal = idBranch,
                               urlImagen = p.TipoImagen == 1
                                   ? "/Content/Products/" + p.NombreImagen + p.Extension
                                   : p.urlImagen, //* file = 1, url = 2 */
                               _Existencias = context.tProductosSucursals
                                   .Where(x => x.idProducto == p.idProducto && x.idSucursal == idBranch).Select(x =>
                                       new ProductBranchViewModel()
                                       {

                                           idSucursal = x.idSucursal,
                                           Sucursal = x.tSucursale.Nombre,
                                           Existencia = ((int?)x.Existencia) ?? 0

                                       }).ToList(),
                               Vista = p.tDetalleVistas
                                   .Where(v => v.idProducto == p.idProducto && v.idSucursal == idBranch &&
                                               v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v =>
                                       (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                               oDetailView = p.tDetalleVistas
                                   .Where(v => v.idProducto == p.idProducto &&
                                               v.Estatus == TypesOutProductsDetail.Pendiente).Select(v =>
                                       new DetailView()
                                       {

                                           Remision = v.tVista.Remision,
                                           Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))

                                       }).ToList(),
                               Total = context.tProductosSucursals
                                   .Where(x => x.idProducto == p.idProducto && x.idSucursal == idBranch)
                                   .Sum(x => x.Existencia)
                           });
                    }

                    if (orderASC)
                    {
                        return products.OrderBy(p => p.Total).Skip(page * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        return products.OrderByDescending(p => p.Total).Skip(page * pageSize).Take(pageSize).ToList();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateCategory(int id, int categoryId, int? subcategoryId)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var producto = context.tProductos.Where(p => p.idProducto == id).FirstOrDefault();

                    if (producto == null)
                    {
                        throw new Exception("El artículo no fue encontrado");
                    }

                    producto.idCategoria = categoryId;
                    producto.idSubcategoria = subcategoryId;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public ProductViewModel GetProduct(int idProducto)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var producto = (from p in context.tProductos
                                    where p.idProducto == idProducto
                                    select new ProductViewModel()
                                    {

                                        idProducto = p.idProducto,
                                        Nombre = p.Nombre,
                                        Descripcion = p.Descripcion,
                                        PrecioVenta = p.PrecioVenta,
                                        PrecioCompra = p.PrecioCompra,
                                        idProveedor = p.idProveedor,
                                        idCategoria = p.idCategoria,
                                        idSubcategoria = p.idSubcategoria,
                                        Color = p.Color,
                                        idMaterial = p.idMaterial,
                                        Medida = p.Medida,
                                        Peso = p.Peso,
                                        Codigo = p.Codigo,
                                        Extension = p.Extension,
                                        urlImagen = p.urlImagen,
                                        TipoImagen = p.TipoImagen,
                                        NombreImagen = p.NombreImagen,
                                        Comentarios = p.Comentarios
                                    }).FirstOrDefault();

                    if (producto.PrecioVenta <= 0)
                    {
                        throw new Exception("El artículo no tiene Precio de Venta");
                    }

                    return (ProductViewModel)producto;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public decimal GetAmountProductByView(int productID, int viewID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVistas.Join(context.tDetalleVistas,
                                          v => v.idVista, dv => dv.idVista, (v, dv) => new { Vista = v, Detalle = dv })
                                          .Where(x => x.Vista.idVista == viewID
                                          && x.Detalle.idProducto == productID && x.Detalle.Estatus == TypesOutProducts.Pendiente).
                                          Select(x => (x.Detalle.Cantidad ?? 0) - ((x.Detalle.Devolucion ?? 0) + (x.Detalle.Venta ?? 0))).DefaultIfEmpty(0).Sum();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public bool ValidateOutProduct(int idBranch, int idProduct, out string msg)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var pendingProducts = context.tVistas.Join(context.tDetalleVistas,
                                          v => v.idVista, dv => dv.idVista, (v, dv) => new { Vista = v, Detalle = dv })
                                          .Where(x => x.Vista.idSucursal == idBranch
                                         && x.Detalle.idProducto == idProduct && x.Detalle.Estatus == TypesOutProducts.Pendiente).
                                          Select(x => (x.Detalle.Cantidad ?? 0) - ((x.Detalle.Devolucion ?? 0) + (x.Detalle.Venta ?? 0))).DefaultIfEmpty(0).Sum();

                    var stockProducts = context.tProductosSucursals.Join(context.tProductos,
                                          ps => ps.idProducto, p => p.idProducto, (ps, p) => new { SucPro = ps, Product = p })
                                          .Where(x => x.SucPro.idProducto == idProduct
                                         && x.SucPro.idSucursal == idBranch && x.Product.idProducto == idProduct).Select(x => x.SucPro.Existencia ?? 0).FirstOrDefault();

                    if ((stockProducts - pendingProducts) == 0)
                    {

                        if (pendingProducts == 0)
                        {

                            msg = "Producto sin inventario.";

                        }
                        else
                        {

                            msg = "El producto está a vista.";

                        }

                        return false;

                    }
                    else
                    {

                        msg = "";
                        return true;

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public decimal GetProductsOutPro(int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVistas.Join(context.tDetalleVistas,
                           v => v.idVista, dv => dv.idVista, (v, dv) => new { Vista = v, Detalle = dv })
                           .Where(x => x.Detalle.idProducto == idProduct && x.Detalle.Estatus == TypesOutProducts.Pendiente).
                           Select(x => (x.Detalle.Cantidad ?? 0) - ((x.Detalle.Devolucion ?? 0) + (x.Detalle.Venta ?? 0))).DefaultIfEmpty(0).Sum();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public decimal GetProductsOutProForEraser(int idProduct, int idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVistas.Join(context.tDetalleVistas,
                           v => v.idVista, dv => dv.idVista, (v, dv) => new { Vista = v, Detalle = dv })
                           .Where(x => x.Vista.idVista != idView && x.Detalle.idProducto == idProduct && x.Detalle.Estatus == TypesOutProducts.Pendiente).
                           Select(x => x.Detalle.Cantidad ?? 0 - (x.Detalle.Devolucion ?? 0 + x.Detalle.Venta ?? 0)).DefaultIfEmpty(0).Sum();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public decimal GetProductsForOutProduct(int idView, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVistas.Join(context.tDetalleVistas,
                           v => v.idVista, dv => dv.idVista, (v, dv) => new { Vista = v, Detalle = dv })
                           .Where(x => x.Detalle.idProducto == idProduct && x.Detalle.Estatus == TypesOutProducts.Pendiente && x.Detalle.idVista != idView)
                           .Select(x => x.Detalle.Cantidad ?? 0 - (x.Detalle.Devolucion ?? 0 + x.Detalle.Venta ?? 0)).DefaultIfEmpty(0).Sum();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<ProductBranchViewModel> GetProductsBranches(int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosSucursals.Where(x => x.idProducto == idProduct).Select(x => new ProductBranchViewModel()
                    {
                        idSucursal = x.idSucursal,
                        Sucursal = x.tSucursale.Nombre,
                        Existencia = x.Existencia - x.tProducto.tDetalleVistas.Where(p => p.Estatus == TypesOutProducts.Pendiente && p.idSucursal == x.idSucursal).Select(p => (p.Cantidad ?? 0) - ((p.Devolucion ?? 0) + (p.Venta ?? 0))).DefaultIfEmpty(0).Sum()
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public decimal GetProductForIdAndView(int idBranch, int idView, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosSucursals.Where(x => x.idSucursal == idBranch && x.idProducto == idProduct).Select(x => new ProductBranchViewModel()
                    {
                        Existencia = x.Existencia - x.tProducto.tDetalleVistas.Where(p => p.Estatus == TypesOutProducts.Pendiente && p.idSucursal == x.idSucursal && p.idVista == idView).Select(p => (p.Cantidad ?? 0) - ((p.Devolucion ?? 0) + (p.Venta ?? 0))).DefaultIfEmpty(0).Sum()
                    }).ToList().Sum(p => p.Existencia) ?? 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public decimal GetProductForIdAndBranch(int idBranch, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductosSucursals.Where(x => x.idSucursal == idBranch && x.idProducto == idProduct).Select(x => new ProductBranchViewModel()
                    {
                        Existencia = x.Existencia - x.tProducto.tDetalleVistas.Where(p => p.Estatus == TypesOutProducts.Pendiente && p.idSucursal == x.idSucursal).Select(p => (p.Cantidad ?? 0) - ((p.Devolucion ?? 0) + (p.Venta ?? 0))).DefaultIfEmpty(0).Sum()
                    }).ToList().Sum(p => p.Existencia) ?? 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<ProductBranchViewModel> GetProductsBranchesForEraserSale(int idProduct, int idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var stock = context.tProductosSucursals.Where(x => x.idProducto == idProduct).Select(x => new ProductBranchViewModel()
                    {
                        idSucursal = x.idSucursal,
                        Sucursal = (x.tSucursale.Nombre),
                        Existencia = (x.Existencia ?? 0)
                    }).ToList();

                    var distribution = context.tDetalleVistas.Where(p => p.idProducto == idProduct && p.idVista != idView && p.Estatus == TypesOutProducts.Pendiente).Select(p => new DistributionViewModel()
                    {
                        idBranch = p.idSucursal,
                        Stock = p.Cantidad ?? 0 - (p.Devolucion ?? 0 + p.Venta ?? 0)
                    }).ToList();

                    return stock.Select(x => new ProductBranchViewModel()
                    {
                        idSucursal = x.idSucursal,
                        Sucursal = x.Sucursal,
                        Existencia = x.Existencia - (distribution.Where(d => d.idBranch == x.idSucursal).Sum(d => d.Stock))
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductBranchViewModel> GetProductsBranchesForQuotation(int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var stock = context.tProductosSucursals.Where(x => x.idProducto == idProduct).Select(x => new ProductBranchViewModel()
                    {
                        idSucursal = x.idSucursal,
                        Sucursal = (x.tSucursale.Nombre),
                        Existencia = (x.Existencia ?? 0)
                    }).ToList();

                    var distribution = context.tDetalleVistas.Where(p => p.idProducto == idProduct && p.Estatus == TypesOutProducts.Pendiente).Select(p => new DistributionViewModel()
                    {
                        idBranch = p.idSucursal,
                        Stock = p.Cantidad ?? 0 - (p.Devolucion ?? 0 + p.Venta ?? 0)
                    }).ToList();

                    return stock.Select(x => new ProductBranchViewModel()
                    {
                        idSucursal = x.idSucursal,
                        Sucursal = x.Sucursal,
                        Existencia = x.Existencia - (distribution.Where(d => d.idBranch == x.idSucursal).Sum(d => d.Stock))
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductBranchViewModel> GetProductsBranchesForOutProduct(int idView, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var stock = context.tProductosSucursals.Where(x => x.idProducto == idProduct).Select(x => new ProductBranchViewModel()
                    {
                        idSucursal = x.idSucursal,
                        Sucursal = (x.tSucursale.Nombre),
                        Existencia = (x.Existencia ?? 0)
                    }).ToList();

                    var distribution = context.tDetalleVistas.Where(p => p.idProducto == idProduct && p.idVista != idView && p.Estatus == TypesOutProducts.Pendiente).Select(p => new DistributionViewModel()
                    {
                        idBranch = p.idSucursal,
                        Stock = (p.Cantidad ?? 0) - ((p.Devolucion ?? 0) + (p.Venta ?? 0))
                    }).ToList();

                    return stock.Select(x => new ProductBranchViewModel()
                    {
                        idSucursal = x.idSucursal,
                        Sucursal = x.Sucursal,
                        Existencia = x.Existencia - (distribution.Where(d => d.idBranch == x.idSucursal).Sum(d => d.Stock))
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdatePrice(int idProduct, decimal? precioCompra, decimal precioVenta)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProducto tProducto = context.tProductos.FirstOrDefault(p => p.idProducto == idProduct);

                    tProducto.PrecioCompra = precioCompra;
                    tProducto.PrecioVenta = precioVenta;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddProduct(tProducto oProduct)
        {
            int iResult = 0;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tProductos.Where(p => p.Codigo.ToUpper().Trim() == oProduct.Codigo.ToUpper().Trim() && p.Estatus == TypesProduct.EstatusActivo).FirstOrDefault() == null)
                    {
                        context.tProductos.Add(oProduct);
                        context.SaveChanges();

                        iResult = oProduct.idProducto;
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        public int UpdateProduct(ProductViewModel oProduct)
        {

            int iResult = 0;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tProductos.Where(p => p.Codigo.ToUpper().Trim() == oProduct.Codigo.ToUpper().Trim() && p.Estatus == TypesProduct.EstatusActivo && p.idProducto != oProduct.idProducto).FirstOrDefault() == null)
                    {
                        tProducto tProducto = context.tProductos.FirstOrDefault(p => p.idProducto == oProduct.idProducto);

                        tProducto.Nombre = oProduct.Nombre;
                        tProducto.Descripcion = oProduct.Descripcion;
                        tProducto.PrecioVenta = oProduct.PrecioVenta;
                        tProducto.PrecioCompra = oProduct.PrecioCompra;
                        tProducto.idProveedor = (int)oProduct.idProveedor;
                        tProducto.idCategoria = oProduct.idCategoria;
                        tProducto.idSubcategoria = oProduct.idSubcategoria;
                        tProducto.Color = oProduct.Color;
                        tProducto.idMaterial = oProduct.idMaterial;
                        tProducto.Medida = oProduct.Medida;
                        tProducto.Peso = oProduct.Peso;
                        tProducto.Comentarios = oProduct.Comentarios;
                        tProducto.Codigo = oProduct.Codigo;
                        tProducto.TipoImagen = oProduct.TipoImagen;
                        tProducto.urlImagen = oProduct.urlImagen;
                        tProducto.NombreImagen = oProduct.NombreImagen;
                        tProducto.ModificadoPor = oProduct.ModificadoPor;
                        tProducto.Modificado = oProduct.Modificado;

                        context.SaveChanges();

                        iResult = 1;
                    }
                    else
                    {
                        iResult = 0;
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        public bool UpdateProductOnLine(ProductViewModel oProduct)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    bool bResult = false;

                    if (context.tProductos.Where(p => p.Codigo == oProduct.Codigo && p.Estatus == TypesProduct.EstatusActivo && p.idProducto != oProduct.idProducto).FirstOrDefault() == null)
                    {

                        tProducto tProducto = context.tProductos.FirstOrDefault(p => p.idProducto == oProduct.idProducto);

                        tProducto.Codigo = oProduct.Codigo;
                        tProducto.Nombre = oProduct.Nombre;
                        tProducto.Descripcion = oProduct.Descripcion;
                        tProducto.PrecioVenta = oProduct.PrecioVenta;
                        tProducto.Comentarios = oProduct.Comentarios;
                        tProducto.idProveedor = (int)oProduct.idProveedor;
                        tProducto.ModificadoPor = oProduct.ModificadoPor;
                        tProducto.Modificado = oProduct.Modificado;

                        context.SaveChanges();

                        bResult = true;
                    }

                    return bResult;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void AddProductBranchExist(int idProduct, List<ProductBranchViewModel> lProductBranch, decimal priceSale, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var product in lProductBranch)
                    {
                        product.Existencia = product.Existencia ?? 0;
                        product.Existencia = (product.Existencia < 0) ? 0 : product.Existencia;

                        this.AddRegisterProduct(idProduct, product.idSucursal, "Se agrega producto directamente", this.GetProductStockForBranch(idProduct, product.idSucursal), (decimal)(this.GetProductStockForBranch(idProduct, product.idSucursal) + product.Existencia), 0, priceSale, String.Empty, (int)idUser);

                        tProductosSucursal oProductoSucursal = new tProductosSucursal();

                        oProductoSucursal.idProducto = idProduct;
                        oProductoSucursal.idSucursal = product.idSucursal;
                        oProductoSucursal.Existencia = product.Existencia;

                        context.tProductosSucursals.Add(oProductoSucursal);

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateProductBranchExist(int idProduct, List<ProductBranchViewModel> lProductBranch, decimal oldpriceSale, decimal priceSale, string justify, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //Verificar Salidas a Vista Pendientes
                    var pendingbranch = context.tDetalleVistas.Where(p => p.idProducto == idProduct && p.Estatus == TypesOutProductsDetail.Pendiente).Select(p => new ProductBranchViewModel()
                    {
                        idSucursal = (int)p.idSucursal,
                        Existencia = p.Cantidad - (p.Devolucion ?? 0 + p.Venta ?? 0)
                    }).ToList();

                    foreach (var product in lProductBranch)
                    {
                        foreach (var pending in pendingbranch)
                        {
                            if (product.idSucursal == pending.idSucursal && product.Existencia < pending.Existencia)
                            {
                                throw new System.ArgumentException("Existen artículos pendientes en Salida a Vista, no se pueden hacer cambios en el inventario.");
                            }
                        }
                    }

                    foreach (var product in lProductBranch)
                    {
                        tProductosSucursal oProductoSucursal = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == product.idSucursal).FirstOrDefault();

                        product.Existencia = product.Existencia ?? 0;
                        product.Existencia = (product.Existencia < 0) ? 0 : product.Existencia;

                        if (oProductoSucursal != null)
                        {
                            this.AddRegisterProduct(idProduct, product.idSucursal, "Se actualiza producto directamente", this.GetProductStockForBranch(idProduct, product.idSucursal), (decimal)product.Existencia, oldpriceSale, priceSale, justify, (int)idUser);

                            oProductoSucursal.Existencia = product.Existencia;

                            context.SaveChanges();
                        }
                        else
                        {
                            this.AddRegisterProduct(idProduct, product.idSucursal, "Se actualiza producto directamente", this.GetProductStockForBranch(idProduct, product.idSucursal), (decimal)product.Existencia, oldpriceSale, priceSale, justify, (int)idUser);

                            tProductosSucursal productoSucursal = new tProductosSucursal();

                            productoSucursal.idProducto = idProduct;
                            productoSucursal.idSucursal = product.idSucursal;
                            productoSucursal.Existencia = product.Existencia ?? 0;

                            context.tProductosSucursals.Add(productoSucursal);

                            context.SaveChanges();
                        }

                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateProductBranchExist(int idProduct, int idBranch, int? stock)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProductosSucursal oProductoSucursal = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == idBranch).FirstOrDefault();

                    if (oProductoSucursal != null)
                    {
                        oProductoSucursal.Existencia = stock;

                        context.SaveChanges();
                    }
                    else
                    {
                        tProductosSucursal productoSucursal = new tProductosSucursal();

                        productoSucursal.idProducto = idProduct;
                        productoSucursal.idSucursal = idBranch;
                        productoSucursal.Existencia = stock ?? 0;

                        context.tProductosSucursals.Add(productoSucursal);

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool UpdateStatusProducto(int idProducto, Int16 Status, int usuarioID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProducto tProducto = context.tProductos.Single(p => p.idProducto == idProducto);

                    tProducto.Estatus = TypesProduct.EstatusInactivo;
                    tProducto.ModificadoPor = usuarioID;
                    tProducto.Modificado = DateTime.Now;

                    context.SaveChanges();

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateProductExtn(ProductViewModel oProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProducto tProducto = context.tProductos.FirstOrDefault(p => p.idProducto == oProduct.idProducto);

                    tProducto.Extension = oProduct.Extension;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void AddProductBranch(int idProduct, int idBranch, int amount)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tProductosSucursal oProductBranch = new tProductosSucursal();

                    oProductBranch.idProducto = idProduct;
                    oProductBranch.idSucursal = idBranch;
                    oProductBranch.Existencia = amount;

                    context.tProductosSucursals.Add(oProductBranch);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateProductBranchExistStock(int idProduct, int idBranch, int Existencia)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProductosSucursal productBranch = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == idBranch).FirstOrDefault();

                    if (productBranch != null)
                    {
                        productBranch.Existencia = productBranch.Existencia + Existencia;
                    }
                    else
                    {
                        tProductosSucursal producto = new tProductosSucursal();

                        producto.idSucursal = idBranch;
                        producto.idProducto = idProduct;
                        producto.Existencia = Existencia;

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

        public void UpdateProductBranchExist(int idProduct, int idBranch, int Existencia)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProductosSucursal productBranch = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == idBranch).FirstOrDefault();

                    if (productBranch != null)
                    {
                        productBranch.Existencia = productBranch.Existencia + Existencia;
                    }
                    else
                    {
                        tProductosSucursal producto = new tProductosSucursal();

                        producto.idSucursal = idBranch;
                        producto.idProducto = idProduct;
                        producto.Existencia = Existencia;

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

        public List<ProductBranchViewModel> GetBranches()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tSucursales.Where(p => p.idSucursal > TypesProduct.SucursalTodas).Select(p => new ProductBranchViewModel
                    {
                        idSucursal = p.idSucursal,
                        Sucursal = p.Nombre,
                        Existencia = 0
                    }).OrderBy(p => p.Sucursal).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductBranchViewModel> GetBranchesForProducts(int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tSucursales.Where(p => p.idSucursal > TypesProduct.SucursalTodas).Select(p => new ProductBranchViewModel
                    {
                        idSucursal = p.idSucursal,
                        Sucursal = p.Nombre,
                        Existencia = context.tProductosSucursals.Where(x => x.idSucursal == p.idSucursal && x.idProducto == idProduct).Select(x => x.Existencia ?? 0).FirstOrDefault()
                    }).OrderBy(p => p.Sucursal).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteProductOrder(int idOrder, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    List<ProductBranchViewModel> oProductsOrder = GeProductsOrder(idOrder, idProduct);

                    foreach (var productOrder in oProductsOrder)
                    {
                        this.DeleteProductBranchExist(idProduct, productOrder.idSucursal, productOrder.Existencia);

                        this.DeleteAmountProductOrder(idOrder, idProduct, productOrder.idSucursal);
                    }

                    var product = context.tOrdenProductoes.Where(p => p.idOrden == idOrder && p.idProducto == idProduct).FirstOrDefault();

                    context.tOrdenProductoes.Remove(product);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteProductBranchExist(int idProduct, int idBranch, decimal? Existencia)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tProductosSucursal oProductoSucursal = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == idBranch).FirstOrDefault();

                    var result = oProductoSucursal.Existencia - Existencia;

                    oProductoSucursal.Existencia = (result < 0) ? 0.0M : result;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductBranchViewModel> GeProductsOrder(int idOrder, int idProduct, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCantidadProductosOrdens.Where(p => p.idOrden == idOrder && p.idProducto == idProduct && p.idSucursal == idBranch).Select(p => new ProductBranchViewModel
                    {

                        idSucursal = p.idSucursal,
                        Existencia = p.Cantidad

                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductBranchViewModel> GeProductsOrder(int idOrder, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCantidadProductosOrdens.Where(p => p.idOrden == idOrder && p.idProducto == idProduct).Select(p => new ProductBranchViewModel
                    {

                        idSucursal = p.idSucursal,
                        Existencia = p.Cantidad

                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteAmountProductOrder(int idOrder, int idProduct, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var productOrder = context.tCantidadProductosOrdens.Where(p => p.idOrden == idOrder && p.idProducto == idProduct && p.idSucursal == idBranch).FirstOrDefault();

                    context.tCantidadProductosOrdens.Remove(productOrder);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetProductsPrintTickets(List<KeyValuePair<int, int>> lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    List<ProductViewModel> listProducts = new List<ProductViewModel>();

                    foreach (KeyValuePair<int, int> a in lProducts)
                    {

                        ProductViewModel product = new ProductViewModel();

                        tProducto tProducto = context.tProductos.FirstOrDefault(p => p.idProducto == a.Key);

                        if (tProducto != null)
                        {

                            product.idProducto = tProducto.idProducto;
                            product.Codigo = tProducto.Codigo;
                            product.Descripcion = tProducto.Descripcion;
                            product.PrecioVenta = tProducto.PrecioVenta;
                            product.Cantidad = a.Value;

                        }

                        listProducts.Add(product);
                    }

                    return listProducts;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetAllProductsPrintTickets(int idOrder)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    List<ProductViewModel> listProducts = new List<ProductViewModel>();

                    var lProducts = context.tOrdenProductoes.Where(o => o.idOrden == idOrder).Select(o => new { ID = o.idProducto, Amount = (int)o.Cantidad, PrecioVenta = o.PrecioVenta ?? 0 }).ToList();

                    foreach (var a in lProducts)
                    {
                        ProductViewModel product = new ProductViewModel();

                        tProducto tProducto = context.tProductos.FirstOrDefault(p => p.idProducto == a.ID);

                        if (tProducto != null)
                        {
                            product.idProducto = tProducto.idProducto;
                            product.Codigo = tProducto.Codigo;
                            product.Descripcion = tProducto.Descripcion;
                            product.PrecioVenta = a.PrecioVenta;
                            product.Cantidad = a.Amount;
                        }

                        listProducts.Add(product);
                    }

                    return listProducts;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteOnlyProductOrder(int idOrder, int idProduct, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    List<ProductBranchViewModel> oProductsOrder = GeProductsOrder(idOrder, idProduct, idBranch);

                    foreach (var productOrder in oProductsOrder)
                    {
                        this.DeleteProductBranchExist(idProduct, productOrder.idSucursal, productOrder.Existencia);
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void ValidateProductToSaleAvoidNegatives(List<ProductSaleViewModel> lProducts, int idSucursal)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var prod in lProducts)
                    {
                        if (prod.Tipo == TypesSales.producto)
                        {
                            var stock = context.tProductosSucursals.Where(p => p.idProducto == prod.idProducto && p.idSucursal == idSucursal).Select(p => p.Existencia).FirstOrDefault();

                            if (stock != null)
                            {
                                if ((stock - prod.cantidad) < 0)
                                {
                                    throw new System.ArgumentException("El inventario no permite negativos");
                                }
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

        public void ValidateProductToAvoidNegativesUnifiedSales(List<ProductSaleViewModel> lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var prod in lProducts)
                    {
                        if (prod.Tipo == TypesSales.producto)
                        {
                            var stock = context.tProductosSucursals.Where(p => p.idProducto == prod.idProducto && p.idSucursal == prod.idSucursal).Select(p => p.Existencia).FirstOrDefault();

                            if (stock != null)
                            {
                                if ((stock - prod.cantidad) < 0)
                                {
                                    throw new System.ArgumentException("El inventario no permite negativos");
                                }
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

        //Se agregar para validar los productos de las salidas a vistas unificadas.
        public void ValidateProductToSaleAvoidNegatives(List<ProductSaleViewModel> lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var prod in lProducts)
                    {
                        if (prod.Tipo == TypesSales.producto)
                        {
                            var stock = context.tProductosSucursals.Where(p => p.idProducto == prod.idProducto && p.idSucursal == prod.idSucursal).Select(p => p.Existencia).FirstOrDefault();

                            if (stock != null)
                            {
                                if ((stock - prod.cantidad) < 0)
                                {
                                    throw new System.ArgumentException("El inventario no permite negativos");
                                }
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

        public bool ValidateOutProductUnify(int idBranch, int idProduct, int idVista, out string msg)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var pendingProducts = context.tVistas.Join(context.tDetalleVistas,
                                          v => v.idVista, dv => dv.idVista, (v, dv) => new { Vista = v, Detalle = dv })
                                          .Where(x => x.Vista.idSucursal == idBranch
                                         && x.Detalle.idProducto == idProduct && x.Detalle.Estatus == TypesOutProducts.Pendiente).
                                          Select(x => (x.Detalle.Cantidad ?? 0) - ((x.Detalle.Devolucion ?? 0) + (x.Detalle.Venta ?? 0))).DefaultIfEmpty(0).Sum();

                    var stockProducts = context.tProductosSucursals.Join(context.tProductos,
                                          ps => ps.idProducto, p => p.idProducto, (ps, p) => new { SucPro = ps, Product = p })
                                          .Where(x => x.SucPro.idProducto == idProduct
                                         && x.SucPro.idSucursal == idBranch && x.Product.idProducto == idProduct).Select(x => x.SucPro.Existencia ?? 0).FirstOrDefault();

                    if ((stockProducts - pendingProducts) == 0)
                    {

                        if (pendingProducts == 0)
                        {
                            msg = "Producto sin inventario.";
                            return false;
                        }
                        else
                        {
                            msg = "El producto está a vista.";

                            var idusuario = context.tVistas.Where(x => x.idVista == idVista).Select(x => x.idUsuario1).FirstOrDefault();
                            //Se genera codigo para devolver los productos de la salida a vista para continuar con la venta 
                            //La devolucion de realizara con la remision de la salida a vista

                            OutProducts oOutProducts = new OutProducts();

                            OutProductsDetailViewModel model = new OutProductsDetailViewModel();
                            model.idVista = idVista;
                            model.idProducto = idProduct;
                            model.Devolucion = pendingProducts;
                            model.idUsuario = idusuario;
                            List<OutProductsDetailViewModel> LiOutProducts = new List<OutProductsDetailViewModel>();
                            LiOutProducts.Add(model);
                            oOutProducts.UpdateStockReturnOutProducts(LiOutProducts);
                            return true;
                        }
                    }
                    else
                    {
                        msg = "";
                        return true;
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ProductViewModel> GetAllActivesProductByBranch(int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p =>
                            p.Estatus == 1 &&
                            p.tProductosSucursals.Any(ps => (ps.idSucursal == idBranch)))
                        .Select(p => new ProductViewModel()
                        {
                            idProducto = p.idProducto,
                            Nombre = p.Nombre,
                            Codigo = p.Codigo,
                            Descripcion = p.Descripcion,
                            PrecioVenta = p.PrecioVenta,
                            PrecioCompra = p.PrecioCompra,
                            Estatus = (short)p.Estatus,
                            Marca = p.tProveedore.NombreEmpresa,
                            Proveedor = p.tProveedore.NombreEmpresa,
                            urlImagen = p.TipoImagen == 1
                                ? "/Content/Products/" + p.NombreImagen + p.Extension
                                : p.urlImagen, //* file = 1, url = 2 */
                            Stock = context.tProductosSucursals
                                .Where(x => x.idProducto == p.idProducto && x.idSucursal == idBranch).Select(x => ((int?)x.Existencia) ?? 0).FirstOrDefault(),
                            Vista = p.tDetalleVistas
                                .Where(v => v.idProducto == p.idProducto &&
                                            v.tSucursale.idSucursal == idBranch &&
                                            v.Estatus == TypesOutProductsDetail.Pendiente).Sum(v =>
                                    (v.Cantidad ?? 0) - ((v.Devolucion ?? 0) + (v.Venta ?? 0))),
                            Taller = p.tDetalleReparaciones.Where(t => t.idProducto == p.idProducto && t.Pendiente > 0)
                                .Sum(t => t.Pendiente ?? 0),
                            oDetailView = p.tDetalleVistas
                                .Where(v => v.idProducto == p.idProducto &&
                                            v.tSucursale.idSucursal == idBranch &&
                                            v.Estatus == TypesOutProductsDetail.Pendiente).Select(v => new DetailView()
                                            {
                                                Remision = v.tVista.Remision,
                                                Cantidad = ((v.Cantidad ?? 0) - (((v.Devolucion ?? 0) - (v.Venta ?? 0))))
                                            }).ToList(),
                            Total = context.tProductosSucursals.Where(x => x.idProducto == p.idProducto &&
                                                                           x.idSucursal == idBranch)
                                .Sum(x => x.Existencia)
                        }).OrderBy(s => s.Codigo).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}