using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class GiftsTable : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMesaRegalos.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<GiftsTableViewModel> GetGiftsTable(DateTime dtDateSince, DateTime dtDateUntil, string remision, string costumer, string codigo, int status, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    var result = context.tMesaRegalos.Where(
                      p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (p.Remision.Contains(remision) || (String.IsNullOrEmpty(remision))) &&
                      (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
                      (((p.idSucursal == amazonas) ||
                      (p.idSucursal == guadalquivir) ||
                      (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      (p.tDetalleMesaRegalos.Where(x => (x.tProducto.Codigo.Contains(codigo) || (String.IsNullOrEmpty(codigo)))).Any()) &&
                      ((p.Estatus == status) || (status == TypesGiftsTable.Todos))
                    ).Select(p => new GiftsTableViewModel()
                    {

                        idMesaRegalo = p.idMesaRegalo,
                        Remision = p.Remision,
                        idNovio = p.idNovio,
                        Novio = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        idNovia = p.idNovia,
                        Novia = p.tClientesFisico1.Nombre + " " + p.tClientesFisico1.Apellidos,
                        FechaBoda = p.FechaBoda,
                        LugarBoda = p.LugarBoda,
                        Latitud = p.Latitud,
                        Longitud = p.Longitud,
                        HoraBoda = p.HoraBoda,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        idVendedor1 = p.idUsuario1,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idVendedor2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        IVA = p.IVA,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesGiftsTable.Saldada ? "grey" :
                                p.Estatus == TypesGiftsTable.Pendiente ? "yellow" :
                                "red"),
                        idVenta = p.idVenta,
                        idNotaCredito = p.idNotaCredito,
                        Comentarios = p.Comentarios,
                        Notificar = p.Notificar,
                        Fecha = p.Fecha,
                        lDetail = context.tDetalleMesaRegalos.Where(d => d.idMesaRegalo == p.idMesaRegalo).Select(d => new DetailGiftsTableViewModel()
                        {

                            idDetalle = d.idDetalle,
                            idMesaRegalo = d.idMesaRegalo,
                            idProducto = d.idProducto,
                            idServicio = d.idServicio,
                            Descripcion = d.Descripcion,
                            Precio = d.Precio,
                            Cantidad = d.Cantidad,
                            Descuento = d.Descuento,
                            Tipo = d.Tipo,
                            Comentario = d.Comentario,
                            Estatus = d.Estatus,
                            sEstatus = (d.Estatus == TypesGiftsTable.PendienteVenta ? "yellow" :
                            d.Estatus == TypesGiftsTable.SaldadoVenta ? "grey" : "")

                        }).ToList()

                    }).OrderByDescending(p => p.Fecha);

                    return result.Skip(page * pageSize).Take(pageSize).ToList();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public string AddGiftsTable(GiftsTableViewModel oGiftTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tMesaRegalo mesaRegalos = new tMesaRegalo();

                    mesaRegalos.idNovio = oGiftTable.idNovio;
                    mesaRegalos.idNovia = oGiftTable.idNovia;
                    mesaRegalos.FechaBoda = oGiftTable.FechaBoda;
                    mesaRegalos.LugarBoda = oGiftTable.LugarBoda;
                    mesaRegalos.Latitud = oGiftTable.Latitud;
                    mesaRegalos.Longitud = oGiftTable.Longitud;
                    mesaRegalos.HoraBoda = oGiftTable.HoraBoda;
                    mesaRegalos.idUsuario1 = oGiftTable.idVendedor1;
                    mesaRegalos.idUsuario2 = oGiftTable.idVendedor2;
                    mesaRegalos.idSucursal = oGiftTable.idSucursal;
                    mesaRegalos.idDespachoReferencia = oGiftTable.idDespachoReferencia;
                    mesaRegalos.CantidadProductos = oGiftTable.CantidadProductos;
                    mesaRegalos.Subtotal = oGiftTable.Subtotal;
                    mesaRegalos.IVA = oGiftTable.IVA;
                    mesaRegalos.Descuento = oGiftTable.Descuento;
                    mesaRegalos.Total = oGiftTable.Total;
                    mesaRegalos.Estatus = TypesGiftsTable.Pendiente;
                    mesaRegalos.idVenta = oGiftTable.idVenta;
                    mesaRegalos.idNotaCredito = oGiftTable.idNotaCredito;
                    mesaRegalos.Comentarios = oGiftTable.Comentarios;
                    mesaRegalos.Notificar = oGiftTable.Notificar;
                    mesaRegalos.Fecha = DateTime.Now;

                    context.tMesaRegalos.Add(mesaRegalos);
                    context.SaveChanges();

                    foreach (var detail in oGiftTable.lDetail)
                    {

                        detail.idMesaRegalo = mesaRegalos.idMesaRegalo;

                        this.AddDetailGiftsTable(detail);

                    }

                    tMesaRegalo mRegalos = context.tMesaRegalos.Where(p => p.idMesaRegalo == mesaRegalos.idMesaRegalo).FirstOrDefault();

                    mRegalos.Remision = this.GenerateNumberRem((int)mesaRegalos.idSucursal, mesaRegalos.idMesaRegalo); ;

                    context.SaveChanges();

                    return mRegalos.Remision;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddDetailGiftsTable(DetailGiftsTableViewModel oDetailGiftTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tDetalleMesaRegalo detalleMesaRegalos = new tDetalleMesaRegalo();

                    detalleMesaRegalos.idMesaRegalo = (int)oDetailGiftTable.idMesaRegalo;
                    detalleMesaRegalos.idProducto = oDetailGiftTable.idProducto;
                    detalleMesaRegalos.idServicio = oDetailGiftTable.idServicio;
                    detalleMesaRegalos.Descripcion = oDetailGiftTable.Descripcion;
                    detalleMesaRegalos.Precio = oDetailGiftTable.Precio;
                    detalleMesaRegalos.Cantidad = oDetailGiftTable.Cantidad;
                    detalleMesaRegalos.Descuento = oDetailGiftTable.Descuento;
                    detalleMesaRegalos.Tipo = oDetailGiftTable.Tipo;
                    detalleMesaRegalos.Comentario = oDetailGiftTable.Comentario;
                    detalleMesaRegalos.Estatus = TypesGiftsTable.PendienteVenta;

                    context.tDetalleMesaRegalos.Add(detalleMesaRegalos);
                    context.SaveChanges();

                    return detalleMesaRegalos.idDetalle;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateGiftsTable(GiftsTableViewModel oGiftTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var mesaRegalos = context.tMesaRegalos.FirstOrDefault(p => p.idMesaRegalo == oGiftTable.idMesaRegalo);

                    mesaRegalos.idNovio = oGiftTable.idNovio;
                    mesaRegalos.idNovia = oGiftTable.idNovia;
                    mesaRegalos.FechaBoda = oGiftTable.FechaBoda;
                    mesaRegalos.LugarBoda = oGiftTable.LugarBoda;
                    mesaRegalos.Latitud = oGiftTable.Latitud;
                    mesaRegalos.Longitud = oGiftTable.Longitud;
                    mesaRegalos.HoraBoda = oGiftTable.HoraBoda;
                    mesaRegalos.idSucursal = oGiftTable.idSucursal;
                    mesaRegalos.idUsuario1 = oGiftTable.idVendedor1;
                    mesaRegalos.idUsuario2 = oGiftTable.idVendedor2;
                    mesaRegalos.idDespachoReferencia = oGiftTable.idDespachoReferencia;
                    mesaRegalos.CantidadProductos = oGiftTable.CantidadProductos;
                    mesaRegalos.Subtotal = oGiftTable.Subtotal;
                    mesaRegalos.IVA = oGiftTable.IVA;
                    mesaRegalos.Descuento = oGiftTable.Descuento;
                    mesaRegalos.Total = oGiftTable.Total;
                    mesaRegalos.Estatus = oGiftTable.Estatus;
                    mesaRegalos.idVenta = oGiftTable.idVenta;
                    mesaRegalos.idNotaCredito = oGiftTable.idNotaCredito;
                    mesaRegalos.Comentarios = oGiftTable.Comentarios;
                    mesaRegalos.Notificar = oGiftTable.Notificar;

                    context.SaveChanges();

                    foreach (var d in oGiftTable.lDetail)
                    {
                        this.UpdateDetailGiftsTable(d);
                    }

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateDetailGiftsTable(DetailGiftsTableViewModel oDetailGiftTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var detalleMesaRegalos = context.tDetalleMesaRegalos.FirstOrDefault(p => p.idDetalle == oDetailGiftTable.idDetalle);

                    detalleMesaRegalos.idProducto = oDetailGiftTable.idProducto;
                    detalleMesaRegalos.idServicio = oDetailGiftTable.idServicio;
                    detalleMesaRegalos.Descripcion = oDetailGiftTable.Descripcion;
                    detalleMesaRegalos.Precio = oDetailGiftTable.Precio;
                    detalleMesaRegalos.Cantidad = oDetailGiftTable.Cantidad;
                    detalleMesaRegalos.Descuento = oDetailGiftTable.Descuento;
                    detalleMesaRegalos.Tipo = oDetailGiftTable.Tipo;
                    detalleMesaRegalos.Comentario = oDetailGiftTable.Comentario;
                    detalleMesaRegalos.Estatus = TypesGiftsTable.PendienteVenta;

                    context.SaveChanges();

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateStatusGiftsTable(int idGiftsTable, short? status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var mesaRegalos = context.tMesaRegalos.FirstOrDefault(p => p.idMesaRegalo == idGiftsTable);

                    mesaRegalos.Estatus = status;

                    context.SaveChanges();

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateStatusDetailGiftsTable(int idDetail, short? status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var mesaRegalos = context.tDetalleMesaRegalos.FirstOrDefault(p => p.idDetalle == idDetail);

                    mesaRegalos.Estatus = status;

                    context.SaveChanges();

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool DeleteGiftsTable(int idGiftsTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var mesaRegalos = context.tMesaRegalos.FirstOrDefault(p => p.idMesaRegalo == idGiftsTable);

                    foreach (var d in mesaRegalos.tDetalleMesaRegalos)
                    {
                        this.DeleteDetailGiftsTable(d.idDetalle);
                    }

                    context.tMesaRegalos.Remove(mesaRegalos);

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool DeleteDetailGiftsTable(int idDetailGiftsTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var detalleMesaRegalos = context.tDetalleMesaRegalos.FirstOrDefault(p => p.idDetalle == idDetailGiftsTable);

                    context.tDetalleMesaRegalos.Remove(detalleMesaRegalos);

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public GiftsTableViewModel GetGiftsTable(int idMesaRegalo)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    return context.tMesaRegalos.Where(p => p.idMesaRegalo == idMesaRegalo).Select(p => new GiftsTableViewModel()
                    {

                        idMesaRegalo = p.idMesaRegalo,
                        Remision = p.Remision,
                        idNovio = p.idNovio,
                        Novio = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        CorreoNovio = p.tClientesFisico.Correo,
                        idNovia = p.idNovia,
                        Novia = p.tClientesFisico1.Nombre + " " + p.tClientesFisico1.Apellidos,
                        CorreoNovia = p.tClientesFisico1.Correo,
                        FechaBoda = p.FechaBoda,
                        LugarBoda = p.LugarBoda,
                        Latitud = p.Latitud,
                        Longitud = p.Longitud,
                        HoraBoda = p.HoraBoda,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        idVendedor1 = p.idUsuario1,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idVendedor2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        CantidadProductos = p.CantidadProductos,
                        SumSubtotal = p.tDetalleMesaRegalos.Where(x => x.idMesaRegalo == p.idMesaRegalo).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum(),
                        Subtotal = p.Subtotal,
                        IVA = p.IVA,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Estatus = p.Estatus,
                        idVenta = p.idVenta,
                        idNotaCredito = p.idNotaCredito,
                        Comentarios = p.Comentarios,
                        Notificar = p.Notificar,
                        Fecha = p.Fecha,
                        lDetail = context.tDetalleMesaRegalos.Where(d => d.idMesaRegalo == p.idMesaRegalo).Select(d => new DetailGiftsTableViewModel()
                        {

                            idDetalle = d.idDetalle,
                            idMesaRegalo = d.idMesaRegalo,
                            idProducto = d.idProducto,
                            Descripcion = d.Descripcion,
                            Producto = (d.idProducto != null) ? new ProductViewModel()
                            {
                                idProducto = d.tProducto.idProducto,
                                Codigo = d.tProducto.Codigo,
                                Descripcion = d.tProducto.Descripcion,
                                urlImagen = d.tProducto.TipoImagen == 1 ? "/Content/Products/" + d.tProducto.NombreImagen + d.tProducto.Extension : d.tProducto.urlImagen,

                            } : null,
                            idServicio = d.idServicio,
                            Precio = d.Precio,
                            Cantidad = d.Cantidad,
                            Descuento = d.Descuento,
                            Tipo = d.Tipo,
                            Comentario = d.Comentario,
                            Estatus = d.Estatus,
                            sEstatus = (d.Estatus == TypesGiftsTable.PendienteVenta ? "yellow" :
                            d.Estatus == TypesGiftsTable.SaldadoVenta ? "grey" : "")

                        }).ToList(),
                        lSales = context.tVentaMesaRegalos.Where(d => d.idMesaRegalos == d.idMesaRegalos).Select(d => new SaleGiftsTableViewModel()
                        {
                            idVenta = d.idVenta,
                            idMesaRegalo = d.idMesaRegalos,
                            Remision = d.Remision,
                            idClienteFisico = d.idClienteFisico,
                            ClienteFisico = d.tClientesFisico.Nombre + " " + d.tClientesFisico.Apellidos,
                            idClienteMoral = d.idClienteMoral,
                            ClienteMoral = d.tClientesMorale.Nombre,
                            idUsuario1 = d.idUsuario1,
                            idUsuario2 = d.idUsuario2,
                            Usuario1 = d.tUsuario.Nombre + " " + d.tUsuario.Apellidos,
                            Usuario2 = d.tUsuario1.Nombre + " " + d.tUsuario1.Apellidos,
                            Fecha = d.Fecha,
                            Descuento = d.Descuento,
                            Total = d.Total,
                            Estatus = d.Estatus,
                            sEstatus = d.Estatus == TypesSales.ventaSaldada ? "grey" :
                                      d.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                      "red",
                            Comentarios = d.Comentarios
                        }).ToList()

                    }).FirstOrDefault();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public GiftsTableViewModel GetGiftsTableForSale(int idMesaRegalo)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    return context.tMesaRegalos.Where(p => p.idMesaRegalo == idMesaRegalo).Select(p => new GiftsTableViewModel()
                    {

                        idMesaRegalo = p.idMesaRegalo,
                        Remision = p.Remision,
                        idNovio = p.idNovio,
                        Novio = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        CorreoNovio = p.tClientesFisico.Correo,
                        idNovia = p.idNovia,
                        Novia = p.tClientesFisico1.Nombre + " " + p.tClientesFisico1.Apellidos,
                        CorreoNovia = p.tClientesFisico1.Correo,
                        FechaBoda = p.FechaBoda,
                        LugarBoda = p.LugarBoda,
                        Latitud = p.Latitud,
                        Longitud = p.Longitud,
                        HoraBoda = p.HoraBoda,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        idVendedor1 = p.idUsuario1,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idVendedor2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        CantidadProductos = p.CantidadProductos,
                        SumSubtotal = p.tDetalleMesaRegalos.Where(x => x.idMesaRegalo == p.idMesaRegalo).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum(),
                        Subtotal = p.Subtotal,
                        IVA = p.IVA,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Estatus = p.Estatus,
                        idVenta = p.idVenta,
                        idNotaCredito = p.idNotaCredito,
                        Comentarios = p.Comentarios,
                        Notificar = p.Notificar,
                        Fecha = p.Fecha,
                        lDetail = context.tDetalleMesaRegalos.Where(d => d.idMesaRegalo == p.idMesaRegalo && d.Estatus == TypesGiftsTable.PendienteVenta).Select(d => new DetailGiftsTableViewModel()
                        {

                            idDetalle = d.idDetalle,
                            idMesaRegalo = d.idMesaRegalo,
                            idProducto = d.idProducto,
                            Descripcion = d.Descripcion,
                            Producto = (d.idProducto != null) ? new ProductViewModel()
                            {
                                idProducto = d.tProducto.idProducto,
                                Codigo = d.tProducto.Codigo,
                                Descripcion = d.tProducto.Descripcion,
                                urlImagen = d.tProducto.TipoImagen == 1 ? "/Content/Products/" + d.tProducto.NombreImagen + d.tProducto.Extension : d.tProducto.urlImagen,

                            } : null,
                            idServicio = d.idServicio,
                            Precio = d.Precio,
                            Cantidad = d.Cantidad,
                            Porcentaje = (d.Precio * d.Cantidad) - (context.tDetalleVentaMesaRegalos.Where(x => x.tVentaMesaRegalo.idMesaRegalos == d.idMesaRegalo && (x.idProducto == d.idProducto || x.idServicio == d.idServicio)).Sum(x => x.Porcentaje) ?? 0),
                            Descuento = d.Descuento,
                            Tipo = d.Tipo,
                            Comentario = d.Comentario,
                            Estatus = d.Estatus,
                            sEstatus = (d.Estatus == TypesGiftsTable.PendienteVenta ? "yellow" :
                            d.Estatus == TypesGiftsTable.SaldadoVenta ? "grey" : "")

                        }).ToList()

                    }).FirstOrDefault();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public GiftsTableViewModel GetGiftsTable(string remision)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    return context.tMesaRegalos.Where(p => p.Remision == remision).Select(p => new GiftsTableViewModel()
                    {

                        idMesaRegalo = p.idMesaRegalo,
                        Remision = p.Remision,
                        idNovio = p.idNovio,
                        Novio = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        CorreoNovio = p.tClientesFisico.Correo,
                        idNovia = p.idNovia,
                        Novia = p.tClientesFisico1.Nombre + " " + p.tClientesFisico1.Apellidos,
                        CorreoNovia = p.tClientesFisico1.Correo,
                        FechaBoda = p.FechaBoda,
                        LugarBoda = p.LugarBoda,
                        Latitud = p.Latitud,
                        Longitud = p.Longitud,
                        HoraBoda = p.HoraBoda,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        idVendedor1 = p.idUsuario1,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idVendedor2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        CantidadProductos = p.CantidadProductos,
                        SumSubtotal = p.tDetalleMesaRegalos.Where(x => x.idMesaRegalo == p.idMesaRegalo).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum(),
                        Subtotal = p.Subtotal,
                        IVA = p.IVA,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Estatus = p.Estatus,
                        idVenta = p.idVenta,
                        idNotaCredito = p.idNotaCredito,
                        Comentarios = p.Comentarios,
                        Notificar = p.Notificar,
                        Fecha = p.Fecha,
                        lDetail = context.tDetalleMesaRegalos.Where(d => d.idMesaRegalo == p.idMesaRegalo).Select(d => new DetailGiftsTableViewModel()
                        {

                            idDetalle = d.idDetalle,
                            idMesaRegalo = d.idMesaRegalo,
                            idProducto = d.idProducto,
                            Descripcion = d.Descripcion,
                            Producto = (d.idProducto != null) ? new ProductViewModel()
                            {
                                idProducto = d.tProducto.idProducto,
                                Codigo = d.tProducto.Codigo,
                                Descripcion = d.tProducto.Descripcion,
                                urlImagen = d.tProducto.TipoImagen == 1 ? "/Content/Products/" + d.tProducto.NombreImagen + d.tProducto.Extension : d.tProducto.urlImagen,

                            } : null,
                            idServicio = d.idServicio,
                            Precio = d.Precio,
                            Cantidad = d.Cantidad,
                            Descuento = d.Descuento,
                            Tipo = d.Tipo,
                            Comentario = d.Comentario,
                            Estatus = d.Estatus,
                            sEstatus = (d.Estatus == TypesGiftsTable.PendienteVenta ? "yellow" :
                            d.Estatus == TypesGiftsTable.SaldadoVenta ? "grey" : "")

                        }).ToList()

                    }).FirstOrDefault();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<DetailGiftsTableViewModel> GetListDetailGiftsTable(int idMesaRegalo)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    return context.tDetalleMesaRegalos.Where(p => p.idMesaRegalo == idMesaRegalo).Select(p => new DetailGiftsTableViewModel()
                    {

                        idDetalle = p.idDetalle,
                        idMesaRegalo = p.idMesaRegalo,
                        idProducto = p.idProducto,
                        idServicio = p.idServicio,
                        Descripcion = p.Descripcion,
                        Precio = p.Precio,
                        Cantidad = p.Cantidad,
                        Descuento = p.Descuento,
                        Tipo = p.Tipo,
                        Comentario = p.Comentario,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesGiftsTable.PendienteVenta ? "yellow" :
                            p.Estatus == TypesGiftsTable.SaldadoVenta ? "grey" : "")

                    }).ToList();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public string GenerateNumberRem(int idBranch, int idGiftsTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    char pad = '0';

                    return string.Concat(string.Concat(month, string.Concat(string.Concat(year + "-", idGiftsTable.ToString().PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T")), "M");
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public string GeneratePrevNumberRem(int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    string ID = (this.GetLastID() + 1).ToString();
                    char pad = '0';

                    return string.Concat(string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T")), "M");
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int GetLastID()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMesaRegalos.Max(p => p.idMesaRegalo);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        //Cerrar Mesa de Regalos

        public decimal GetAmountGiftsTable(int idGiftsTable)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    return context.tVentaMesaRegalos.Where(p => p.idMesaRegalos == idGiftsTable && p.Estatus == TypesGiftsTable.SaldadoVenta).Sum(p => p.Total) ?? 0;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public void GenerateCredit(int idGiftsTable, int idSeller)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tMesaRegalo tGiftsTable = context.tMesaRegalos.FirstOrDefault(p => p.idMesaRegalo == idGiftsTable);

                    tNotasCredito tCredit = new tNotasCredito();

                    tCredit.idVendedor = idSeller;

                    tCredit.idCustomerP = tGiftsTable.idNovio;

                    tCredit.Cantidad = this.GetAmountGiftsTable(idGiftsTable);
                    tCredit.Fecha = DateTime.Now;
                    tCredit.FechaVigencia = DateTime.Now.AddYears(1);
                    tCredit.Comentarios = "Nota de Crédito de la Mesa de Regalos " + tGiftsTable.Remision;
                    tCredit.Estatus = TypesSales.ventaPendiente;

                    context.tNotasCreditoes.Add(tCredit);

                    context.SaveChanges();

                    int iResult = tCredit.idNotaCredito;

                    int? idBranch = context.tMesaRegalos.Where(p => p.idMesaRegalo == idGiftsTable).Select(p => p.idSucursal).FirstOrDefault();

                    //Genera remisión

                    Credits oCredits = new Credits();

                    string remCredit = oCredits.GenerateNumberRem(idBranch ?? 0, (int)tCredit.idNotaCredito);

                    var oCredit = context.tNotasCreditoes.FirstOrDefault(p => p.idNotaCredito == tCredit.idNotaCredito);

                    oCredit.Remision = remCredit;

                    // Se relaciona la Mesa de Regalos con la Nota de Crédito
                    tGiftsTable.idNotaCredito = tCredit.idNotaCredito;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public GiftsTableViewModel GetGiftsTableForClose(int idGiftsTable)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    return context.tMesaRegalos.Where(p => p.idMesaRegalo == idGiftsTable).Select(p => new GiftsTableViewModel()
                    {

                        idMesaRegalo = p.idMesaRegalo,
                        Remision = p.Remision,
                        idNovio = p.idNovio,
                        Novio = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        CorreoNovio = p.tClientesFisico.Correo,
                        idNovia = p.idNovia,
                        Novia = p.tClientesFisico1.Nombre + " " + p.tClientesFisico1.Apellidos,
                        CorreoNovia = p.tClientesFisico1.Correo,
                        FechaBoda = p.FechaBoda,
                        LugarBoda = p.LugarBoda,
                        Latitud = p.Latitud,
                        Longitud = p.Longitud,
                        HoraBoda = p.HoraBoda,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        idVendedor1 = p.idUsuario1,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idVendedor2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        CantidadProductos = p.CantidadProductos,
                        SumSubtotal = p.tDetalleMesaRegalos.Where(x => x.idMesaRegalo == p.idMesaRegalo).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum(),
                        Subtotal = p.Subtotal,
                        IVA = p.IVA,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Estatus = p.Estatus,
                        idVenta = p.idVenta,
                        idNotaCredito = p.idNotaCredito,
                        Comentarios = p.Comentarios,
                        Notificar = p.Notificar,
                        Fecha = p.Fecha,
                        lDetail = context.tDetalleMesaRegalos.Where(d => d.idMesaRegalo == p.idMesaRegalo).Select(d => new DetailGiftsTableViewModel()
                        {

                            idDetalle = d.idDetalle,
                            idMesaRegalo = d.idMesaRegalo,
                            idProducto = d.idProducto,
                            Descripcion = d.Descripcion,
                            Producto = (d.idProducto != null) ? new ProductViewModel()
                            {
                                idProducto = d.tProducto.idProducto,
                                Codigo = d.tProducto.Codigo,
                                Descripcion = d.tProducto.Descripcion,
                                urlImagen = d.tProducto.TipoImagen == 1 ? "/Content/Products/" + d.tProducto.NombreImagen + d.tProducto.Extension : d.tProducto.urlImagen,

                            } : null,
                            idServicio = d.idServicio,
                            Precio = d.Precio,
                            Cantidad = d.Cantidad,
                            Descuento = d.Descuento,
                            Porcentaje = context.tDetalleVentaMesaRegalos.Where(x => x.tVentaMesaRegalo.idMesaRegalos == d.idMesaRegalo && (x.idProducto == d.idProducto || x.idServicio == d.idServicio)).Sum(x => x.Porcentaje) ?? 0,
                            Tipo = d.Tipo,
                            Comentario = d.Comentario,
                            Estatus = d.Estatus,
                            sEstatus = (d.Estatus == TypesGiftsTable.PendienteVenta ? "yellow" :
                            d.Estatus == TypesGiftsTable.SaldadoVenta ? "grey" : "")

                        }).ToList(),
                        lSales = context.tVentaMesaRegalos.Where(d => d.idMesaRegalos == d.idMesaRegalos).Select(d => new SaleGiftsTableViewModel()
                        {
                            idVenta = d.idVenta,
                            idMesaRegalo = d.idMesaRegalos,
                            Remision = d.Remision,
                            idClienteFisico = d.idClienteFisico,
                            ClienteFisico = d.tClientesFisico.Nombre + " " + d.tClientesFisico.Apellidos,
                            idClienteMoral = d.idClienteMoral,
                            ClienteMoral = d.tClientesMorale.Nombre,
                            idUsuario1 = d.idUsuario1,
                            idUsuario2 = d.idUsuario2,
                            Usuario1 = d.tUsuario.Nombre + " " + d.tUsuario.Apellidos,
                            Usuario2 = d.tUsuario1.Nombre + " " + d.tUsuario1.Apellidos,
                            Fecha = d.Fecha,
                            Descuento = d.Descuento,
                            Total = d.Total,
                            Estatus = d.Estatus,
                            sEstatus = d.Estatus == TypesSales.ventaSaldada ? "grey" :
                                      d.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                      "red",
                            Comentarios = d.Comentarios
                        }).ToList()

                    }).FirstOrDefault();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        //Ventas Mesa de Regalos
        public string GenerateNumberRemForSale(int idBranch, int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    char pad = '0';

                    return string.Concat(string.Concat(month, string.Concat(string.Concat(year + "-", idSale.ToString().PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T")), "MV");
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public string GeneratePrevNumberRemForSale(int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    string ID = (this.GetLastIDForSale() + 1).ToString();
                    char pad = '0';

                    return string.Concat(string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T")), "MV");
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int GetLastIDForSale()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVentaMesaRegalos.Max(p => p.idVenta);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

        }

        public void AddTypePayment(int idSale, List<TypePaymentViewModel> lTypePayment)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    foreach (var typePayment in lTypePayment)
                    {

                        tFormaPagoMesaRegalo oFormaPago = new tFormaPagoMesaRegalo();

                        oFormaPago.idVenta = idSale;
                        oFormaPago.FormaPago = typePayment.typesPayment;
                        oFormaPago.TipoTarjeta = typePayment.typesCard;
                        oFormaPago.Banco = typePayment.bank;
                        oFormaPago.Titular = typePayment.holder;
                        oFormaPago.NumCheque = typePayment.numCheck;
                        oFormaPago.NumIFE = typePayment.numIFE;
                        oFormaPago.Cantidad = typePayment.amount;
                        oFormaPago.FechaLimitePago = (String.IsNullOrEmpty(typePayment.dateMaxPayment)) ? DateTime.Now : Convert.ToDateTime(typePayment.dateMaxPayment);
                        oFormaPago.CantidadIVA = typePayment.amountIVA;
                        oFormaPago.IVA = typePayment.IVA;
                        oFormaPago.Estatus = TypesSales.ventaPendiente;

                        context.tFormaPagoMesaRegalos.Add(oFormaPago);

                        context.SaveChanges();

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public string AddSale(SaleGiftsTableViewModel oSale, out int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tVentaMesaRegalo oVenta = new tVentaMesaRegalo();

                    oVenta.idMesaRegalos = oSale.idMesaRegalo;
                    oVenta.idUsuario1 = oSale.idUsuario1;
                    oVenta.idUsuario2 = oSale.idUsuario2;
                    oVenta.idClienteFisico = oSale.idClienteFisico;
                    oVenta.idClienteMoral = oSale.idClienteMoral;
                    oVenta.idDespachoReferencia = oSale.idDespachoReferencia;
                    oVenta.idSucursal = oSale.idSucursal;
                    oVenta.Fecha = oSale.Fecha;
                    oVenta.CantidadProductos = oSale.CantidadProductos;
                    oVenta.Subtotal = oSale.Subtotal;
                    oVenta.Descuento = oSale.Descuento;
                    oVenta.Total = oSale.Total;
                    oVenta.Estatus = oSale.Estatus;
                    oVenta.IVA = oSale.IVA;
                    oVenta.Factura = oSale.Factura;
                    oVenta.Comentarios = oSale.Comentarios;
                    oVenta.NumeroFactura = oSale.NumeroFactura;
                    oVenta.TipoCliente = oSale.TipoCliente;
                    oVenta.idDespacho = oSale.idDespacho;

                    context.tVentaMesaRegalos.Add(oVenta);
                    context.SaveChanges();

                    foreach (var detail in oSale.oDetail)
                    {
                        detail.idVenta = oVenta.idVenta;
                        AddDetailSale(detail);

                        if (detail.Tipo == TypesGiftsTable.producto)
                        {

                            if (ValidatePendingProduct((int)oSale.idMesaRegalo, (int)detail.idProducto))
                            {
                                int idDetail = context.tDetalleMesaRegalos.Where(p => p.idMesaRegalo == oSale.idMesaRegalo && p.idProducto == detail.idProducto).Select(p => p.idDetalle).FirstOrDefault();

                                UpdateStatusDetailGiftsTable(idDetail, TypesGiftsTable.Saldada);
                            }

                        }
                        else if (detail.Tipo == TypesGiftsTable.servicio)
                        {

                            if (ValidatePendingService((int)oSale.idMesaRegalo, (int)detail.idServicio))
                            {
                                int idDetail = context.tDetalleMesaRegalos.Where(p => p.idMesaRegalo == oSale.idMesaRegalo && p.idServicio == detail.idServicio).Select(p => p.idDetalle).FirstOrDefault();

                                UpdateStatusDetailGiftsTable(idDetail, TypesGiftsTable.Saldada);
                            }

                        }
                    }

                    idSale = oVenta.idVenta;

                    tVentaMesaRegalo mVenta = context.tVentaMesaRegalos.Where(p => p.idVenta == oVenta.idVenta).FirstOrDefault();

                    mVenta.Remision = this.GenerateNumberRemForSale((int)oSale.idSucursal, oVenta.idVenta);

                    context.SaveChanges();

                    return mVenta.Remision;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddDetailSale(SaleDetailViewModel oDetail)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tDetalleVentaMesaRegalo oDetalle = new tDetalleVentaMesaRegalo();

                    oDetalle.idVenta = oDetail.idVenta;
                    oDetalle.idProducto = oDetail.idProducto;
                    oDetalle.idServicio = oDetail.idServicio;
                    oDetalle.idCredito = oDetail.idNotaCredito;
                    oDetalle.Descripcion = oDetail.Descripcion;
                    oDetalle.Precio = oDetail.Precio;
                    oDetalle.Cantidad = oDetail.Cantidad;
                    oDetalle.Descuento = oDetail.Descuento;
                    oDetalle.Tipo = oDetail.Tipo;
                    oDetalle.Comentarios = oDetail.Comentarios;
                    oDetalle.Porcentaje = oDetail.Porcentaje;

                    context.tDetalleVentaMesaRegalos.Add(oDetalle);
                    context.SaveChanges();

                    return oDetalle.idDetalleVenta;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool ValidatePendingProduct(int idGiftsTable, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    decimal pending = context.tVentaMesaRegalos.Where(p => p.idMesaRegalos == idGiftsTable).Sum(p => p.tDetalleVentaMesaRegalos.Where(x => x.idProducto == idProduct).Sum(x => x.Porcentaje).Value);

                    decimal? total = context.tMesaRegalos.Where(p => p.idMesaRegalo == idGiftsTable).Select(x => x.tDetalleMesaRegalos.Where(y => y.idProducto == idProduct).Select(y => y.Cantidad * y.Precio).Sum()).FirstOrDefault();

                    if (pending == total)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool ValidatePendingService(int idGiftsTable, int idService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    decimal pending = context.tVentaMesaRegalos.Where(p => p.idMesaRegalos == idGiftsTable).Sum(p => p.tDetalleVentaMesaRegalos.Where(x => x.idServicio == idService).Sum(x => x.Porcentaje).Value);

                    decimal? total = context.tMesaRegalos.Where(p => p.idMesaRegalo == idGiftsTable).Select(x => x.tDetalleMesaRegalos.Where(y => y.idServicio == idService).Select(y => y.Cantidad * y.Precio).Sum()).FirstOrDefault();

                    if (pending == total)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public SaleGiftsTableViewModel GetGiftsTableSale(string remision)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVentaMesaRegalos.Where(p => p.Remision == remision).Select(p => new SaleGiftsTableViewModel()
                    {
                        idVenta = p.idVenta,
                        idMesaRegalo = p.idMesaRegalos,
                        Remision = p.Remision,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idClienteFisico = p.idClienteFisico,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        idClienteMoral = p.idClienteMoral,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        idDespacho = p.idDespacho,
                        Despacho = p.tDespacho.Nombre,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho1.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Estatus = p.Estatus,
                        IVA = p.IVA,
                        Comentarios = p.Comentarios,
                        NumeroFactura = p.NumeroFactura,
                        TipoCliente = p.TipoCliente,
                        oAddress = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? new AddressViewModel()
                        {

                            Correo = p.tClientesFisico.Correo,
                            TelCasa = p.tClientesFisico.Telefono,
                            TelCelular = p.tClientesFisico.TelefonoCelular,
                            Direccion = p.tClientesFisico.Calle + " " + p.tClientesFisico.NumExt + " " + p.tClientesFisico.NumInt + " " + p.tClientesFisico.Colonia + " " + ((p.tClientesFisico.tMunicipio != null) ? p.tClientesFisico.tMunicipio.nombre_municipio : "") + " " + p.tClientesFisico.CP

                        } : (p.TipoCliente == TypesCustomers.MoralCustomer) ? new AddressViewModel()
                        {

                            Correo = p.tClientesMorale.Correo,
                            TelCasa = p.tClientesMorale.Telefono,
                            TelCelular = p.tClientesMorale.TelefonoCelular,
                            Direccion = p.tClientesMorale.Calle + " " + p.tClientesMorale.NumExt + " " + p.tClientesMorale.NumInt + " " + p.tClientesMorale.Colonia + " " + ((p.tClientesMorale.tMunicipio != null) ? p.tClientesMorale.tMunicipio.nombre_municipio : "") + " " + p.tClientesMorale.CP

                        } : new AddressViewModel()
                        {

                            Correo = p.tDespacho.Correo,
                            TelCasa = p.tDespacho.Telefono,
                            TelCelular = "",
                            Direccion = p.tDespacho.Calle + " " + p.tDespacho.NumExt + " " + p.tDespacho.NumInt + " " + p.tDespacho.Colonia + " " + ((p.tDespacho.tMunicipio != null) ? p.tDespacho.tMunicipio.nombre_municipio : "") + " " + p.tDespacho.CP

                        },
                        oDetail = p.tDetalleVentaMesaRegalos.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
                        {

                            idDetalleVenta = x.idDetalleVenta,
                            idVenta = x.idVenta,
                            idProducto = x.idProducto,
                            Codigo = (x.tProducto.Codigo == null) ? x.NotaCredito : x.tProducto.Codigo,
                            idServicio = x.idServicio,
                            Descripcion = x.Descripcion,
                            Precio = x.Precio,
                            Descuento = x.Descuento,
                            Cantidad = x.Cantidad,
                            oProducto = context.tProductos.Where(y => y.idProducto == x.idProducto).Select(y => new ProductViewModel()
                            {
                                idProducto = x.tProducto.idProducto,
                                urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen,
                                Codigo = x.tProducto.Codigo,
                                Descripcion = x.tProducto.Descripcion

                            }).FirstOrDefault(),
                            Comentarios = x.Comentarios

                        }).ToList(),
                        SumSubtotal = p.tDetalleVentaMesaRegalos.Where(x => x.idVenta == p.idVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum()

                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<TypePaymentViewModel> GetTypePayment(int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var payment = context.tFormaPagoes.Where(p => p.idVenta == idSale).Select(p => new TypePaymentViewModel()

                    {
                        idTypePayment = p.idFormaPago,
                        typesPayment = (short)p.FormaPago,
                        sTypePayment = (p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                        p.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
                        p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                        p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                        p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                        p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                        p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                        p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                        "Nota de Crédito"
                        ),
                        sTypesCard = (p.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
                        p.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
                        p.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
                        amount = (decimal)p.Cantidad,
                        maxPayment = (DateTime)p.FechaLimitePago,
                        Estatus = p.Estatus,
                        DatePayment = (DateTime)p.FechaPago,
                        bank = p.Banco.ToUpper(),
                        amountIVA = p.CantidadIVA,
                        IVA = p.IVA,
                        HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == idSale && p.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
                        {

                            FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

                                x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                ""

                            ).FirstOrDefault()),
                            Cantidad = q.Cantidad,
                            amountIVA = q.CantidadIVA,
                            IVA = q.IVA,
                            Restante = q.Restante,
                            Estatus = q.Estatus,
                            Fecha = q.Fecha,
                            idVenta = q.idVenta,
                            idHitorialCredito = q.idHistorialCredito

                        }).OrderBy(q => q.Fecha).ToList()

                    }).ToList();

                    var paymentDistinct = payment.Where(p => p.typesPayment != TypesPayment.iCredito).Distinct().ToList();

                    var paymentToAdd = payment.Where(p => p.typesPayment == TypesPayment.iCredito).ToList();

                    foreach (var pay in paymentToAdd)
                    {

                        paymentDistinct.Add(pay);

                    }

                    return paymentDistinct;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateStatusGiftsTableSale(int idSale, short status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var mesaRegalos = context.tVentaMesaRegalos.FirstOrDefault(p => p.idVenta == idSale);

                    mesaRegalos.Estatus = status;

                    context.SaveChanges();

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }
    }
}