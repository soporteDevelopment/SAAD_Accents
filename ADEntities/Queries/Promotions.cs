using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Promotions : Base
    {
        public Tuple<int, List<PromotionViewModel>> Get(DateTime since, DateTime until, bool active, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = since.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = until.Date + new TimeSpan(23, 59, 59);

                    var promotions = context.tPromociones.Where(p => (p.FechaInicio >= dtStart || p.FechaFin <= dtEnd)
                            && p.Activo == active)
                            .Select(p => new PromotionViewModel()
                            {
                                idPromocion = p.idPromocion,
                                idTipoPromocion = p.idTipoPromocion,
                                Descripcion = p.Descripcion,
                                FechaInicio = p.FechaInicio,
                                FechaFin = p.FechaFin,
                                Descuento = p.Descuento,
                                Costo = p.Costo,
                                Activo = p.Activo,
                                CreadoPor = p.CreadoPor,
                                Creado = p.Creado,
                                ModificadoPor = p.ModificadoPor,
                                Modificado = p.Modificado,
                                TipoPromocion = new TypePromotionViewModel()
                                {
                                    idTipoPromocion = p.tTipoPromocion.idTipoPromocion,
                                    Descripcion = p.tTipoPromocion.Descripcion
                                },
                                DetallePromociones = p.tDetallePromocions.Select(d => new DetailPromotionViewModel()
                                {
                                    idDetallePromocion = d.idDetallePromocion,
                                    idPromocion = d.idPromocion,
                                    idProducto = d.idProducto,
                                    Cantidad = d.Cantidad,
                                    Producto = new ProductViewModel()
                                    {
                                        idProducto = d.tProducto.idProducto,
                                        Nombre = d.tProducto.Nombre,
                                        NombreComercial = d.tProducto.Nombre,
                                        Descripcion = d.tProducto.Descripcion,
                                        PrecioVenta = d.tProducto.PrecioVenta,
                                        PrecioCompra = d.tProducto.PrecioCompra,
                                        idProveedor = d.tProducto.idProveedor,
                                        idCategoria = d.tProducto.idCategoria,
                                        idSubcategoria = d.tProducto.idSubcategoria,
                                        Color = d.tProducto.Color,
                                        idMaterial = d.tProducto.idMaterial,
                                        Medida = d.tProducto.Medida,
                                        Peso = d.tProducto.Peso,
                                        Codigo = d.tProducto.Codigo
                                    }
                                }).ToList()
                            }).OrderBy(p => p.FechaInicio).Skip(page * pageSize).Take(pageSize).ToList();

                    var total = context.tPromociones.Where(p => (p.FechaInicio >= dtStart || p.FechaFin <= dtEnd)
                            && p.Activo == active).Count();

                    return Tuple.Create(total, promotions);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public PromotionViewModel GetById(int id)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tPromociones.Where(p => p.idPromocion == id)
                            .Select(p => new PromotionViewModel()
                            {
                                idPromocion = p.idPromocion,
                                idTipoPromocion = p.idTipoPromocion,
                                Descripcion = p.Descripcion,
                                FechaInicio = p.FechaInicio,
                                FechaFin = p.FechaFin,
                                Descuento = p.Descuento,
                                Costo = p.Costo,
                                Activo = p.Activo,
                                CreadoPor = p.CreadoPor,
                                Creado = p.Creado,
                                ModificadoPor = p.ModificadoPor,
                                Modificado = p.Modificado,
                                DetallePromociones = p.tDetallePromocions.Select(d => new DetailPromotionViewModel()
                                {
                                    idDetallePromocion = d.idDetallePromocion,
                                    idPromocion = d.idPromocion,
                                    idProducto = d.idProducto,
                                    Cantidad = d.Cantidad,
                                    Producto = new ProductViewModel()
                                    {
                                        idProducto = d.tProducto.idProducto,
                                        Nombre = d.tProducto.Nombre,
                                        NombreComercial = d.tProducto.Nombre,
                                        Descripcion = d.tProducto.Descripcion,
                                        PrecioVenta = d.tProducto.PrecioVenta,
                                        PrecioCompra = d.tProducto.PrecioCompra,
                                        idProveedor = d.tProducto.idProveedor,
                                        idCategoria = d.tProducto.idCategoria,
                                        idSubcategoria = d.tProducto.idSubcategoria,
                                        Color = d.tProducto.Color,
                                        idMaterial = d.tProducto.idMaterial,
                                        Medida = d.tProducto.Medida,
                                        Peso = d.tProducto.Peso,
                                        Codigo = d.tProducto.Codigo,
                                        TipoImagen = d.tProducto.TipoImagen,
                                        NombreImagen = d.tProducto.NombreImagen,
                                        urlImagen = d.tProducto.urlImagen,
                                        Extension = d.tProducto.Extension
                                    }
                                }).ToList()
                            }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<PromotionViewModel> GetPromotions(DateTime since, DateTime until, bool active)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = since.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = until.Date + new TimeSpan(23, 59, 59);

                    return context.tPromociones.Where(p => p.FechaInicio >= dtStart && p.FechaFin <= dtEnd)
                            .Select(p => new PromotionViewModel()
                            {
                                idPromocion = p.idPromocion,
                                idTipoPromocion = p.idTipoPromocion,
                                Descripcion = p.Descripcion,
                                FechaInicio = p.FechaInicio,
                                FechaFin = p.FechaFin,
                                Descuento = p.Descuento,
                                Costo = p.Costo,
                                Activo = p.Activo,
                                CreadoPor = p.CreadoPor,
                                Creado = p.Creado,
                                ModificadoPor = p.ModificadoPor,
                                Modificado = p.Modificado,
                                DetallePromociones = p.tDetallePromocions.Select(d => new DetailPromotionViewModel()
                                {
                                    idDetallePromocion = d.idDetallePromocion,
                                    idPromocion = d.idPromocion,
                                    idProducto = d.idProducto,
                                    Cantidad = d.Cantidad,
                                    Producto = new ProductViewModel()
                                    {
                                        idProducto = d.tProducto.idProducto,
                                        Nombre = d.tProducto.Nombre,
                                        NombreComercial = d.tProducto.Nombre,
                                        Descripcion = d.tProducto.Descripcion,
                                        PrecioVenta = d.tProducto.PrecioVenta,
                                        PrecioCompra = d.tProducto.PrecioCompra,
                                        idProveedor = d.tProducto.idProveedor,
                                        idCategoria = d.tProducto.idCategoria,
                                        idSubcategoria = d.tProducto.idSubcategoria,
                                        Color = d.tProducto.Color,
                                        idMaterial = d.tProducto.idMaterial,
                                        Medida = d.tProducto.Medida,
                                        Peso = d.tProducto.Peso,
                                        Codigo = d.tProducto.Codigo
                                    }
                                }).ToList()
                            }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public PromotionViewModel GetMixedComboPromotion()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = DateTime.Now.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = DateTime.Now.Date + new TimeSpan(23, 59, 59);

                    return context.tPromociones.Where(p => p.FechaInicio <= dtStart && p.FechaFin >= dtEnd && p.Activo == TypesPromotions.Active)
                            .Select(p => new PromotionViewModel()
                            {
                                idPromocion = p.idPromocion,
                                idTipoPromocion = p.idTipoPromocion,
                                Descripcion = p.Descripcion,
                                FechaInicio = p.FechaInicio,
                                FechaFin = p.FechaFin,
                                Descuento = p.Descuento,
                                Costo = p.Costo,
                                Activo = p.Activo,
                                CreadoPor = p.CreadoPor,
                                Creado = p.Creado,
                                ModificadoPor = p.ModificadoPor,
                                Modificado = p.Modificado,
                                DetallePromociones = p.tDetallePromocions.Select(d => new DetailPromotionViewModel()
                                {
                                    idDetallePromocion = d.idDetallePromocion,
                                    idPromocion = d.idPromocion,
                                    idProducto = d.idProducto,
                                    Cantidad = d.Cantidad,
                                    Producto = new ProductViewModel()
                                    {
                                        idProducto = d.tProducto.idProducto,
                                        Nombre = d.tProducto.Nombre,
                                        NombreComercial = d.tProducto.Nombre,
                                        Descripcion = d.tProducto.Descripcion,
                                        PrecioVenta = d.tProducto.PrecioVenta,
                                        PrecioCompra = d.tProducto.PrecioCompra,
                                        idProveedor = d.tProducto.idProveedor,
                                        idCategoria = d.tProducto.idCategoria,
                                        idSubcategoria = d.tProducto.idSubcategoria,
                                        Color = d.tProducto.Color,
                                        idMaterial = d.tProducto.idMaterial,
                                        Medida = d.tProducto.Medida,
                                        Peso = d.tProducto.Peso,
                                        Codigo = d.tProducto.Codigo
                                    }
                                }).ToList()
                            }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public ProductPromotionViewModel GetPromotionProduct(DateTime date, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tPromociones.Any(p => (p.FechaInicio <= date && p.FechaFin >= date) && p.Activo == TypesPromotions.Active && p.idTipoPromocion == TypesPromotions.SpecialCombo))
                    {
                        return context.tPromociones.Where(p => (p.FechaInicio <= date && p.FechaFin >= date) && p.Activo == TypesPromotions.Active
                                 && p.idTipoPromocion == TypesPromotions.SpecialCombo)
                                .Select(p => new ProductPromotionViewModel()
                                {
                                    idPromocion = p.idPromocion,
                                    idTipoPromocion = p.idTipoPromocion,
                                    Costo = p.Costo,
                                    Descuento = p.Descuento
                                }).FirstOrDefault();
                    }
                    else
                    {
                        return context.tPromociones.Where(p => (p.FechaInicio <= date && p.FechaFin >= date) && p.Activo == TypesPromotions.Active
                                 && p.tDetallePromocions.Any(d => d.idProducto == idProduct))
                                .Select(p => new ProductPromotionViewModel()
                                {
                                    idPromocion = p.idPromocion,
                                    idTipoPromocion = p.idTipoPromocion,
                                    Costo = p.Costo,
                                    Descuento = p.Descuento,
                                    Cantidad = p.tDetallePromocions.Where(d => d.idProducto == idProduct).FirstOrDefault().Cantidad
                                }).FirstOrDefault();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void Add(PromotionViewModel promotion)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var entity = new tPromocione()
                    {
                        idTipoPromocion = promotion.idTipoPromocion,
                        Descripcion = promotion.Descripcion,
                        FechaInicio = promotion.FechaInicio,
                        FechaFin = promotion.FechaFin,
                        Descuento = promotion.Descuento,
                        Costo = promotion.Costo,
                        Activo = promotion.Activo,
                        CreadoPor = promotion.CreadoPor,
                        Creado = promotion.Creado
                    };

                    context.tPromociones.Add(entity);
                    context.SaveChanges();

                    if (promotion.DetallePromociones != null)
                    {
                        foreach (var detail in promotion.DetallePromociones)
                        {
                            var detailEntity = new tDetallePromocion()
                            {
                                idDetallePromocion = detail.idDetallePromocion,
                                idPromocion = entity.idPromocion,
                                idProducto = detail.idProducto,
                                Cantidad = detail.Cantidad
                            };

                            context.tDetallePromocions.Add(detailEntity);
                        }

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void Update(PromotionViewModel promotion)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var entity = context.tPromociones.FirstOrDefault(p => p.idPromocion == promotion.idPromocion);

                    entity.idTipoPromocion = promotion.idTipoPromocion;
                    entity.Descripcion = promotion.Descripcion;
                    entity.FechaInicio = promotion.FechaInicio;
                    entity.FechaFin = promotion.FechaFin;
                    entity.Descuento = promotion.Descuento;
                    entity.Costo = promotion.Costo;
                    entity.ModificadoPor = promotion.ModificadoPor;
                    entity.Modificado = promotion.Modificado;

                    context.SaveChanges();

                    var detailPromotions = context.tDetallePromocions.Where(p => p.idPromocion == promotion.idPromocion).ToList();

                    if (detailPromotions != null)
                    {
                        context.tDetallePromocions.RemoveRange(detailPromotions);
                        context.SaveChanges();
                    }

                    if (promotion.DetallePromociones != null)
                    {
                        foreach (var detail in promotion.DetallePromociones)
                        {
                            var detailEntity = new tDetallePromocion()
                            {
                                idDetallePromocion = detail.idDetallePromocion,
                                idPromocion = entity.idPromocion,
                                idProducto = detail.idProducto,
                                Cantidad = detail.Cantidad
                            };

                            context.tDetallePromocions.Add(detailEntity);
                        }

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}