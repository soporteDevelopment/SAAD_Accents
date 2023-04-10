using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class EraserOutProducts : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tBorradorVistas.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int GetCount(int idUser, short? restricted, string costumer, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tBorradorVistas.Where(
                        p => (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                        ((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                        ((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
                        (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null))
                    ).Select(p => new OutProductsViewModel()
                    {
                        idVista = p.idBorradorVista,
                        idUsuario1 = p.idUsuario1,
                        Vendedor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idClienteFisico = p.idClienteFisico,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        idClienteMoral = p.idClienteMoral,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        idDespachoReferencia = p.idDespachoReferencia,
                        Despacho = p.tDespacho1.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total
                    }).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<OutProductsViewModel> GetEraserOutProducts(int idUser, short? restricted, string costumer, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var sales = context.tBorradorVistas.Where(
                        p => (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                        ((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                        ((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
                        (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null))
                    ).Select(p => new OutProductsViewModel()
                    {
                        idVista = p.idBorradorVista,
                        idUsuario1 = p.idUsuario1,
                        Vendedor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idClienteFisico = p.idClienteFisico,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        idClienteMoral = p.idClienteMoral,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        idDespachoReferencia = p.idDespachoReferencia,
                        Despacho = p.tDespacho1.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total

                    }).OrderByDescending(p => p.Fecha);

                    List<OutProductsViewModel> loutProducts = sales.Skip(page * pageSize).Take(pageSize).ToList();

                    if (restricted == 1)
                    {
                        loutProducts = loutProducts.Where(p => p.idUsuario1 == idUser).ToList();
                    }

                    return loutProducts;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddOutProducts(tBorradorVista oVista)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tBorradorVista tVista = new tBorradorVista();

                    tVista.CantidadProductos = oVista.CantidadProductos;
                    tVista.Fecha = oVista.Fecha;
                    tVista.idClienteFisico = oVista.idClienteFisico;
                    tVista.idClienteMoral = oVista.idClienteMoral;
                    tVista.idDespacho = oVista.idDespacho;
                    tVista.Proyecto = oVista.Proyecto;
                    tVista.idDespachoReferencia = oVista.idDespachoReferencia;
                    tVista.TipoCliente = oVista.TipoCliente;
                    tVista.idSucursal = oVista.idSucursal;
                    tVista.idUsuario1 = oVista.idUsuario1;
                    tVista.Subtotal = oVista.Subtotal;
                    tVista.Total = oVista.Total;
                    tVista.idUsuario1 = oVista.idUsuario1;
                    tVista.idUsuario2 = oVista.idUsuario2;
                    tVista.Flete = oVista.Flete;

                    context.tBorradorVistas.Add(tVista);

                    context.SaveChanges();

                    if (tVista.idBorradorVista > 0)
                    {
                        foreach (var item in oVista.tBorradorDetalleVistas)
                        {
                            tBorradorDetalleVista tdetalleVista = new tBorradorDetalleVista();

                            tdetalleVista.Precio = item.Precio;
                            tdetalleVista.idBorradorVista = tVista.idBorradorVista;
                            tdetalleVista.idSucursal = item.idSucursal;
                            tdetalleVista.idProducto = item.idProducto;
                            tdetalleVista.Cantidad = item.Cantidad;
                            tdetalleVista.Comentarios = item.Comentarios;

                            context.tBorradorDetalleVistas.Add(tdetalleVista);

                        }

                        context.SaveChanges();
                    }

                    return tVista.idBorradorVista;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public void AddServiceOutProducts(List<ServiceOutProductViewModel> lServices)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    foreach (var service in lServices)
                    {

                        tBorradorServicioVista tService = new tBorradorServicioVista();

                        tService.idBorradorVista = service.idVista;
                        tService.idServicio = service.idServicio;
                        tService.Precio = service.Precio;
                        tService.Cantidad = service.Cantidad;
                        tService.Comentarios = service.Comentarios;

                        context.tBorradorServicioVistas.Add(tService);

                        context.SaveChanges();

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public OutProductsViewModel GetOutproduct(int idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var outProducts = context.tBorradorVistas.Where(p => p.idBorradorVista == idView).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idBorradorVista,
                        idUsuario1 = p.idUsuario1,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        sCustomer = (p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos) == " " ? p.tClientesMorale.Nombre : p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Flete = p.Flete,
                        oDetail = p.tBorradorDetalleVistas.Where(x => x.idBorradorVista == p.idBorradorVista).Select(x => new OutProductsDetailViewModel()
                        {
                            idDetalleVista = x.idBorradorDetalleVista,
                            idVista = x.idBorradorVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = x.Cantidad,
                            TipoImagen = x.tProducto.TipoImagen,
                            Comentarios = x.Comentarios,
                            urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen
                        }).ToList()

                    }).FirstOrDefault();

                    return outProducts;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public void UpdateOutProduct(tBorradorVista oVista)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tBorradorVista updVista = context.tBorradorVistas.FirstOrDefault(p => p.idBorradorVista == oVista.idBorradorVista);

                    updVista.idUsuario1 = oVista.idUsuario1;
                    updVista.idUsuario2 = oVista.idUsuario2;
                    updVista.idClienteFisico = oVista.idClienteFisico;
                    updVista.idClienteMoral = oVista.idClienteMoral;
                    updVista.idDespacho = oVista.idDespacho;
                    updVista.Proyecto = oVista.Proyecto;
                    updVista.idDespachoReferencia = oVista.idDespachoReferencia;
                    updVista.TipoCliente = oVista.TipoCliente;
                    updVista.idSucursal = oVista.idSucursal;
                    updVista.Fecha = oVista.Fecha;
                    updVista.CantidadProductos = oVista.CantidadProductos;
                    updVista.Subtotal = oVista.Subtotal;
                    updVista.Total = oVista.Total;
                    updVista.Flete = oVista.Flete;

                    context.SaveChanges();

                    this.DeleteDetailOutProduct(oVista.idBorradorVista);

                    if (oVista.idBorradorVista > 1)
                    {
                        foreach (var item in oVista.tBorradorDetalleVistas)
                        {
                            tBorradorDetalleVista tdetalleVista = new tBorradorDetalleVista();

                            tdetalleVista.Precio = item.Precio;
                            tdetalleVista.idBorradorVista = oVista.idBorradorVista;
                            tdetalleVista.idSucursal = item.idSucursal;
                            tdetalleVista.idProducto = item.idProducto;
                            tdetalleVista.Cantidad = item.Cantidad;
                            tdetalleVista.Comentarios = item.Comentarios;

                            context.tBorradorDetalleVistas.Add(tdetalleVista);

                        }

                        context.SaveChanges();
                    }

                    this.DeleteServicesOutProduct(oVista.idBorradorVista);

                    if (oVista.idBorradorVista > 1)
                    {
                        foreach (var item in oVista.tBorradorServicioVistas)
                        {
                            tBorradorServicioVista tservicioVista = new tBorradorServicioVista();

                            tservicioVista.Precio = item.Precio;
                            tservicioVista.idBorradorVista = oVista.idBorradorVista;
                            tservicioVista.idServicio = item.idServicio;
                            tservicioVista.Precio = item.Precio;
                            tservicioVista.Cantidad = item.Cantidad;
                            tservicioVista.Comentarios = item.Comentarios;

                            context.tBorradorServicioVistas.Add(tservicioVista);

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

        public void DeleteOutProduct(int idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    this.DeleteDetailOutProduct(idView);
                    this.DeleteServicesOutProduct(idView);

                    var oEraserOutProduct = context.tBorradorVistas.FirstOrDefault(p => p.idBorradorVista == idView);

                    if (oEraserOutProduct != null)
                    {
                        context.tBorradorVistas.Remove(oEraserOutProduct);
                    }

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void DeleteDetailOutProduct(int idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var lDetailViews = context.tBorradorDetalleVistas.Where(p => p.idBorradorVista == idView).ToList();

                    foreach (var detail in lDetailViews)
                    {
                        if (detail != null)
                        {
                            context.tBorradorDetalleVistas.Remove(detail);
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

        public void DeleteServicesOutProduct(int idView)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var lServiceViews = context.tBorradorServicioVistas.Where(p => p.idBorradorVista == idView).ToList();

                    foreach (var service in lServiceViews)
                    {
                        if (service != null)
                        {
                            context.tBorradorServicioVistas.Remove(service);
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
    }
}
