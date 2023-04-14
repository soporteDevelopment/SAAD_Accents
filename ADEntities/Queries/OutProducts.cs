using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class OutProducts : Base
    {
        public OutProductsViewModel GetViewForIdView(int idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    var views = context.tVistas.Where(p => p.idVista == idView).Select(p => new OutProductsViewModel()
                    {
                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = p.idUsuario1,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre + " " + cf.Apellidos).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        sCustomer = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho1.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        Restante = p.tDetalleVistas.Sum(dt => dt.Precio * ((dt.Cantidad ?? 0) - ((dt.Devolucion ?? 0) + (dt.Venta ?? 0)))),
                        Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        idVerificador = p.idVerificador,
                        Verificador = (from v in context.tUsuarios where v.idUsuario == p.idVerificador select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        iEstatus = p.Estatus,
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
                            Correo = p.tDespacho1.Correo,
                            TelCasa = p.tDespacho1.Telefono,
                            TelCelular = "",
                            Direccion = p.tDespacho1.Calle + " " + p.tDespacho1.NumExt + " " + p.tDespacho1.NumInt + " " + p.tDespacho1.Colonia + " " + ((p.tDespacho1.tMunicipio != null) ? p.tDespacho1.tMunicipio.nombre_municipio : "") + " " + p.tDespacho1.CP
                        },
                        oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista).Select(x => new OutProductsDetailViewModel()
                        {
                            idDetalleVista = x.idDetalleVista,
                            idVista = x.idVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = x.Cantidad,
                            Pendiente = x.Cantidad - ((x.Devolucion ?? 0) + (x.Venta ?? 0)),
                            Devolucion = x.Devolucion ?? 0,
                            Venta = x.Venta ?? 0,
                            Extension = x.tProducto.Extension,
                            TipoImagen = x.tProducto.TipoImagen,
                            urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen,
                            Imagen = x.tProducto.TipoImagen == 1 ? x.tProducto.NombreImagen + x.tProducto.Extension : "",
                            iEstatus = x.Estatus,
                            Comentarios = x.Comentarios,
                            oHistoryDetail = context.tDetalleDevoluciones.Where(d => d.idDetalleVista == x.idDetalleVista).Select(d => new DetailDevOutProductViewModel()
                            {
                                idDetalleDevoluciones = d.idDetalleDevoluciones,
                                idDetalleVista = d.idDetalleVista,
                                Devolucion = d.Devolucion,
                                Venta = d.Venta,
                                Remision = d.Remision,
                                Fecha = d.Fecha,
                                idVerificador = d.idVerificador,
                                Verificador = (d.idVerificador > 0) ? context.tUsuarios.Where(u => u.idUsuario == d.idVerificador).Select(u => u.Nombre + " " + u.Apellidos).FirstOrDefault() : ""
                            }).ToList()
                        }).ToList()
                    }).FirstOrDefault();

                    return views;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool VerifyOutProducts(int idUser, DateTime verifyDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var view = context.tVistas.OrderByDescending(p => p.Fecha).FirstOrDefault(p => p.idUsuario1 == idUser && p.Estatus == TypesOutProducts.Pendiente);

                    if (view != null)
                    {
                        var totalMinute = verifyDate.Subtract(view.Fecha ?? DateTime.Now);

                        return context.tVistas.Any(p => p.idUsuario1 == idUser && totalMinute.TotalMinutes <= 2);
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

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVistas.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public int AddOutProducts(tVista oVista, out string rem)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tVista tVista = new tVista();

                    tVista.CantidadProductos = oVista.CantidadProductos;
                    tVista.Estatus = oVista.Estatus;
                    tVista.Fecha = DateTime.Now;//oVista.Fecha;
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
                    tVista.idVerificador = oVista.idVerificador;

                    context.tVistas.Add(tVista);

                    context.SaveChanges();

                    tVista objVista = context.tVistas.Where(p => p.idVista == tVista.idVista).FirstOrDefault();

                    objVista.Remision = this.GenerateNumberRem((int)tVista.idSucursal, objVista.idVista);

                    rem = objVista.Remision;

                    context.SaveChanges();

                    if (tVista.idVista > 1)
                    {
                        foreach (var item in oVista.tDetalleVistas)
                        {
                            this.AddRegisterProduct((int)item.idProducto, (int)oVista.idSucursal, "Producto sale a vista " + rem, this.GetProductStockForBranch((int)item.idProducto, (int)oVista.idSucursal), this.GetProductStockForBranch((int)item.idProducto, (int)oVista.idSucursal), item.Precio, item.Precio, String.Empty, (int)oVista.idUsuario1);

                            tDetalleVista tdetalleVista = new tDetalleVista();

                            tdetalleVista.Precio = item.Precio;
                            tdetalleVista.idVista = tVista.idVista;
                            tdetalleVista.idSucursal = item.idSucursal;
                            tdetalleVista.idProducto = item.idProducto;
                            tdetalleVista.Cantidad = item.Cantidad;
                            tdetalleVista.Estatus = item.Estatus;
                            tdetalleVista.Comentarios = item.Comentarios;

                            context.tDetalleVistas.Add(tdetalleVista);
                        }

                        context.SaveChanges();
                    }

                    return tVista.idVista;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public int GetCount(int idUser, short? restricted, DateTime fechaInicial, DateTime fechaFinal, string cliente, int? iduserforsearch, string producto, string remision, string project, int status, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var datInicio = Convert.ToDateTime(fechaInicial).Date + new TimeSpan(0, 0, 0);
                    var datFinal = Convert.ToDateTime(fechaFinal).Date + new TimeSpan(23, 59, 59);

                    int count = 0;

                    if (status == TypesOutProducts.Todos)
                    {

                        count = context.tVistas.Where(p => (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(cliente.ToUpper().Trim()) || (string.IsNullOrEmpty(cliente)))
                                                              || (p.tClientesMorale.Nombre.Contains(cliente) || (string.IsNullOrEmpty(cliente)))
                                                              || (p.tDespacho1.Nombre.Contains(cliente) || (string.IsNullOrEmpty(cliente))))
                                                              && (p.tDetalleVistas.Where(x => (x.tProducto.Codigo.Contains(producto) || string.IsNullOrEmpty(producto))).Any())
                                                              && ((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null)
                                                              && (p.Remision.Contains(remision) || string.IsNullOrEmpty(remision))
                                                              && (p.Proyecto.ToUpper().Contains(project.ToUpper()) || string.IsNullOrEmpty(project))
                                                              && (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null))
                                                              ).Count();

                    }
                    else
                    {

                        count = context.tVistas.Where(p => (p.Fecha >= datInicio && p.Fecha <= datFinal)
                                                              && (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).Contains(cliente) || (string.IsNullOrEmpty(cliente)))
                                                              || (p.tClientesMorale.Nombre.Contains(cliente) || (string.IsNullOrEmpty(cliente)))
                                                              || (p.tDespacho1.Nombre.Contains(cliente) || (string.IsNullOrEmpty(cliente))))
                                                              && (p.tDetalleVistas.Where(x => (x.tProducto.Codigo.Contains(producto) || string.IsNullOrEmpty(producto))).Any())
                                                               && ((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null)
                                                              && (p.Remision.Contains(remision) || string.IsNullOrEmpty(remision))
                                                              && (p.Proyecto.ToUpper().Contains(project.ToUpper()) || string.IsNullOrEmpty(project))
                                                              && (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                                                              (p.Estatus == status)
                                                              ).Count();
                    }

                    return count;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int GetCount(int idUser, short? restricted, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tVistas.Where(p =>
                    (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)))
                    .Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public Tuple<List<OutProductsViewModel>, int> GetOutProducts(bool allTime, int idUser, short? restricted, DateTime fechaInicial, DateTime fechaFinal, string cliente, int? iduserforsearch, string producto, string remision, string project, int status, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    DateTime dtStart;
                    DateTime dtEnd;

                    if (allTime)
                    {
                        dtStart = DateTime.Now.AddYears(-25) + new TimeSpan(0, 0, 0);
                        dtEnd = DateTime.Now + new TimeSpan(23, 59, 59);
                    }
                    else
                    {
                        dtStart = Convert.ToDateTime(fechaInicial).Date + new TimeSpan(0, 0, 0);
                        dtEnd = Convert.ToDateTime(fechaFinal).Date + new TimeSpan(23, 59, 59);
                    }

                    IQueryable<OutProductsViewModel> outProducts = context.tVistas.Where(p => (p.Fecha >= dtStart && p.Fecha <= dtEnd)
                                                          && (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).Contains(cliente) || (string.IsNullOrEmpty(cliente)))
                                                          || (p.tClientesMorale.Nombre.Contains(cliente) || (string.IsNullOrEmpty(cliente)))
                                                          || (p.tDespacho1.Nombre.Contains(cliente) || (string.IsNullOrEmpty(cliente))))
                                                          && (p.tDetalleVistas.Where(x => (x.tProducto.Codigo.Contains(producto) || string.IsNullOrEmpty(producto))).Any())
                                                           && ((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null)
                                                          && (p.Remision.Contains(remision) || string.IsNullOrEmpty(remision))
                                                          && (p.Proyecto.ToUpper().Contains(project.ToUpper()) || string.IsNullOrEmpty(project))
                                                          && (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                                                          ((p.Estatus == status) || (status == TypesOutProducts.Todos))
                                                          ).Select(p => new OutProductsViewModel()
                                                          {
                                                              idVista = p.idVista,
                                                              remision = p.Remision,
                                                              RemisionVenta = p.tVenta.Remision,
                                                              idUsuario1 = (int)p.idUsuario1,
                                                              Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                                                              Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                                                              ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre + " " + cf.Apellidos).FirstOrDefault(),
                                                              ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                                                              idDespacho = (int)p.idDespacho,
                                                              Despacho = p.tDespacho1.Nombre,
                                                              Proyecto = p.Proyecto,
                                                              idDespachoReferencia = p.idDespachoReferencia,
                                                              DespachoReferencia = p.tDespacho.Nombre,
                                                              TipoCliente = p.TipoCliente,
                                                              idSucursal = (int)p.idSucursal,
                                                              Fecha = (DateTime)p.Fecha,
                                                              CantidadProductos = (int)p.CantidadProductos,
                                                              ProductosRestantes = p.CantidadProductos - (p.tDetalleVistas.Sum(dt => dt.Devolucion ?? 0) + p.tDetalleVistas.Sum(dt => dt.Venta ?? 0)),
                                                              Subtotal = (decimal)p.Subtotal,
                                                              Total = (decimal)p.Total,
                                                              Restante = p.tDetalleVistas.Sum(dt => dt.Precio * ((dt.Cantidad ?? 0) - ((dt.Devolucion ?? 0) + (dt.Venta ?? 0)))),
                                                              iEstatus = p.Estatus,
                                                              Estatus = p.Estatus == TypesOutProducts.Pendiente ? "Pendiente" : "Entregado",
                                                              sEstatus = (p.Estatus == TypesOutProducts.Entregado ? "grey" : "yellow")
                                                          }).OrderByDescending(p => p.Fecha);

                    var total = outProducts.Count();

                    List<OutProductsViewModel> loutProducts = outProducts.Skip(page * pageSize).Take(pageSize).ToList();

                    if (restricted == 1)
                    {
                        loutProducts = loutProducts.Where(p => p.idUsuario1 == idUser).ToList();
                    }

                    return new Tuple<List<OutProductsViewModel>, int>(loutProducts, total);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public object GetNumberViewsForSeller(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    return context.tVistas.Where(
                       p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && p.idUsuario1 == idSeller
                    ).Count();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<OutProductsViewModel> GetViewsForSeller(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    var lViews = context.tVistas.Where(
                       p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && p.idUsuario1 == idSeller
                    ).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        idUsuario1 = p.idUsuario1,
                        Vendedor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Vendedor2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        Proyecto = p.Proyecto,
                        Despacho = p.tDespacho1.Nombre,
                        DespachoReferencia = p.tDespacho.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        TipoCliente = p.TipoCliente,
                        iEstatus = p.Estatus,
                        oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista).Select(x => new OutProductsDetailViewModel()
                        {

                            idDetalleVista = x.idDetalleVista,
                            idVista = x.idVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = x.Cantidad,
                            Pendiente = x.Cantidad - ((x.Devolucion ?? 0) + (x.Venta ?? 0)),
                            Devolucion = x.Devolucion ?? 0,
                            Venta = x.Venta ?? 0,
                            Extension = x.tProducto.Extension,
                            TipoImagen = x.tProducto.TipoImagen,
                            urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen,
                            iEstatus = x.Estatus,
                            Comentarios = x.Comentarios,
                            oHistoryDetail = context.tDetalleDevoluciones.Where(d => d.idDetalleVista == x.idDetalleVista).Select(d => new DetailDevOutProductViewModel()
                            {

                                idDetalleDevoluciones = d.idDetalleDevoluciones,
                                idDetalleVista = d.idDetalleVista,
                                Devolucion = d.Devolucion,
                                Venta = d.Venta,
                                Remision = d.Remision,
                                Fecha = d.Fecha,
                                idVerificador = d.idVerificador
                            }).ToList()

                        }).ToList(),
                        SumSubtotal = p.tDetalleVistas.Where(x => x.idVista == p.idVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum()

                    }).OrderByDescending(p => p.Fecha).ToList();

                    foreach (var view in lViews)
                    {
                        view.sEstatus = view.iEstatus == TypesOutProductsDetail.Pendiente ? "yellow" : view.iEstatus == TypesSales.ventaSaldada ? "grey" :
                                       "";
                    }

                    return lViews;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<OutProductsDetailViewModel> GetOutProductsDetails(int idVista)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var outProductsDetails = (from d in context.tDetalleVistas
                                              where d.idVista == idVista
                                              select new OutProductsDetailViewModel()
                                              {
                                                  idVista = d.idVista,
                                                  idProducto = d.idProducto,
                                                  idDetalleVista = d.idDetalleVista,
                                                  Precio = d.Precio,
                                                  Cantidad = d.Cantidad,
                                                  Estatus = d.Estatus == TypesOutProductsDetail.Pendiente ? "Pendiente" : "Entregado",
                                                  Sucursal = (from s in context.tSucursales where s.idSucursal == d.idSucursal select s.Nombre).FirstOrDefault()
                                              }).OrderBy(d => d.idDetalleVista);

                    return outProductsDetails.ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<VistaViewModel> GetVistas(int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var listOfVistas = context.tVistas.Select(p => new VistaViewModel()
                    {
                        Id = p.idVista,
                        Remision = p.Remision,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre + " " + cf.Apellidos).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        Fecha = (DateTime)p.Fecha
                    }).OrderBy(p => p.Id);

                    return listOfVistas.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<VistaDetalleViewModel> GetVistaDetalle(int idVista)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var vistaDetalle = (from d in context.tDetalleVistas
                                        where d.idVista == idVista
                                        select new VistaDetalleViewModel()
                                        {
                                            Id = d.idDetalleVista,
                                            Nombre = (from p in context.tProductos where p.idProducto == d.idProducto select p.Nombre).FirstOrDefault(),
                                            PrecioUnitario = d.Precio,
                                            Cantidad = d.Cantidad,
                                            Comentarios = d.Comentarios
                                        }).OrderBy(d => d.Id);
                    return vistaDetalle.ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public static int GetCountPendingOutProducts()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var today = DateTime.Today.AddDays(-2);

                    return context.tVistas.Where(p => (p.Fecha.Value < today) && (p.Estatus == TypesOutProducts.Pendiente)).Count();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public static int GetCountPendingOutProducts(int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var today = DateTime.Today.AddDays(-2);

                    return context.tVistas.Where(p => ((p.idUsuario1 == idUser) || (p.idUsuario2 == idUser)) && (p.Fecha.Value < today) && (p.Estatus == TypesOutProducts.Pendiente)).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public static List<OutProductsViewModel> GetPendingOutProducts()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var today = DateTime.Today.AddDays(-2);

                    return context.tVistas.Where(p => (p.Fecha.Value < today) && (p.Estatus == TypesOutProducts.Pendiente)).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = (int)p.idUsuario1,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = (int)p.idDespachoReferencia,
                        Vendedor = p.tUsuario.Nombre,
                        idSucursal = (int)p.idSucursal,
                        Fecha = (DateTime)p.Fecha,
                        CantidadProductos = (int)p.CantidadProductos,
                        Subtotal = (decimal)p.Subtotal,
                        Total = (decimal)p.Total,
                        Estatus = p.Estatus == TypesOutProducts.Pendiente ? "Pendiente" : "Entregado"

                    }).OrderBy(p => p.idVista).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public static List<OutProductsViewModel> GetPendingOutProducts(int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var today = DateTime.Today.AddDays(-2);

                    return context.tVistas.Where(p => ((p.idUsuario1 == idUser) || (p.idUsuario2 == idUser)) && (p.Fecha.Value < today) && (p.Estatus == TypesOutProducts.Pendiente)).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = (int)p.idUsuario1,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = (int)p.idDespachoReferencia,
                        idSucursal = (int)p.idSucursal,
                        Fecha = (DateTime)p.Fecha,
                        CantidadProductos = (int)p.CantidadProductos,
                        Subtotal = (decimal)p.Subtotal,
                        Total = (decimal)p.Total,
                        Estatus = p.Estatus == TypesOutProducts.Pendiente ? "Pendiente" : "Entregado"

                    }).OrderBy(p => p.idVista).ToList();
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

                    return string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public string GenerateNumberRem(int idBranch, int idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    char pad = '0';

                    return string.Concat(month, string.Concat(string.Concat(year + "-", idView.ToString().PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
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
                    return context.tVistas.Max(p => p.idVista);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public OutProductsViewModel GetOutProductsForRemision(string remision)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var outProducts = context.tVistas
                        .Where(p => p.Remision.ToUpper() == remision.ToUpper())
                        .Select(p => new OutProductsViewModel()
                        {

                            idVista = p.idVista,
                            remision = p.Remision,
                            idUsuario1 = p.idUsuario1,
                            idClienteFisico = p.idClienteFisico,
                            idClienteMoral = p.idClienteMoral,
                            ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre + " " + cf.Apellidos).FirstOrDefault(),
                            ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                            Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                            sCustomer = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho1.Nombre,
                            idDespacho = p.idDespacho,
                            Proyecto = p.Proyecto,
                            idDespachoReferencia = p.idDespachoReferencia,
                            DespachoReferencia = p.tDespacho.Nombre,
                            TipoCliente = p.TipoCliente,
                            idSucursal = p.idSucursal,
                            Sucursal = p.tSucursale.Nombre,
                            Fecha = p.Fecha,
                            CantidadProductos = p.CantidadProductos,
                            Subtotal = p.Subtotal,
                            Total = p.Total,
                            Restante = p.tDetalleVistas.Sum(dt => dt.Precio * ((dt.Cantidad ?? 0) - ((dt.Devolucion ?? 0) + (dt.Venta ?? 0)))),
                            Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                            Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                            idVerificador = p.idVerificador,
                            Verificador = (from v in context.tUsuarios where v.idUsuario == p.idVerificador select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                            iEstatus = p.Estatus,
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
                                Correo = p.tDespacho1.Correo,
                                TelCasa = p.tDespacho1.Telefono,
                                TelCelular = "",
                                Direccion = p.tDespacho1.Calle + " " + p.tDespacho1.NumExt + " " + p.tDespacho1.NumInt + " " + p.tDespacho1.Colonia + " " + ((p.tDespacho1.tMunicipio != null) ? p.tDespacho1.tMunicipio.nombre_municipio : "") + " " + p.tDespacho1.CP
                            },
                            oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista).Select(x => new OutProductsDetailViewModel()
                            {
                                idDetalleVista = x.idDetalleVista,
                                idVista = x.idVista,
                                idProducto = x.idProducto,
                                Codigo = x.tProducto.Codigo,
                                Descripcion = x.tProducto.Descripcion,
                                Precio = x.Precio,
                                Cantidad = x.Cantidad,
                                Pendiente = x.Cantidad - ((x.Devolucion ?? 0) + (x.Venta ?? 0)),
                                Devolucion = x.Devolucion ?? 0,
                                Venta = x.Venta ?? 0,
                                Extension = x.tProducto.Extension,
                                TipoImagen = x.tProducto.TipoImagen,
                                urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen,
                                iEstatus = x.Estatus,
                                Comentarios = x.Comentarios,
                                oHistoryDetail = context.tDetalleDevoluciones.Where(d => d.idDetalleVista == x.idDetalleVista).Select(d => new DetailDevOutProductViewModel()
                                {
                                    idDetalleDevoluciones = d.idDetalleDevoluciones,
                                    idDetalleVista = d.idDetalleVista,
                                    Devolucion = d.Devolucion,
                                    Venta = d.Venta,
                                    Remision = d.Remision,
                                    Fecha = d.Fecha,
                                    idVerificador = d.idVerificador,
                                    Verificador = (d.idVerificador > 0) ? context.tUsuarios.Where(u => u.idUsuario == d.idVerificador).Select(u => u.Nombre + " " + u.Apellidos).FirstOrDefault() : ""
                                }).ToList()
                                //RemisionVenta = (x.Estatus == TypesOutProductsDetail.Venta) ? context.tVentas.Join(context.tDetalleVentas, s => s.idVenta, d => d.idVenta, (s, d) => new { Sale = s, Detail = d }).Where(sd => sd.Detail.idProducto == x.idProducto).Select(sd => sd.Sale.Remision).FirstOrDefault() : String.Empty
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

        public OutProductsViewModel GetOutProductsPending(string remision)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var outProducts = context.tVistas.Where(p => p.Remision.ToUpper() == remision.ToUpper()).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = p.idUsuario1,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        sCustomer = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho1.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        Restante = p.tDetalleVistas.Sum(dt => dt.Precio * ((dt.Cantidad ?? 0) - ((dt.Devolucion ?? 0) + (dt.Venta ?? 0)))),
                        Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        idVerificador = p.idVerificador,
                        iEstatus = p.Estatus,
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

                            Correo = p.tDespacho1.Correo,
                            TelCasa = p.tDespacho1.Telefono,
                            TelCelular = "",
                            Direccion = p.tDespacho1.Calle + " " + p.tDespacho1.NumExt + " " + p.tDespacho1.NumInt + " " + p.tDespacho1.Colonia + " " + ((p.tDespacho1.tMunicipio != null) ? p.tDespacho1.tMunicipio.nombre_municipio : "") + " " + p.tDespacho1.CP

                        },
                        oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista && p.Estatus == TypesOutProductsDetail.Pendiente).Select(x => new OutProductsDetailViewModel()
                        {
                            idDetalleVista = x.idDetalleVista,
                            idVista = x.idVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = (x.Cantidad - ((x.Devolucion ?? 0) + (x.Venta ?? 0))),
                            Devolucion = x.Devolucion ?? 0,
                            Venta = x.Venta ?? 0,
                            Extension = x.tProducto.Extension,
                            TipoImagen = x.tProducto.TipoImagen,
                            urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen,
                            iEstatus = x.Estatus,
                            Comentarios = x.Comentarios,
                            oHistoryDetail = context.tDetalleDevoluciones.Where(d => d.idDetalleVista == x.idDetalleVista).Select(d => new DetailDevOutProductViewModel()
                            {

                                idDetalleDevoluciones = d.idDetalleDevoluciones,
                                idDetalleVista = d.idDetalleVista,
                                Devolucion = d.Devolucion,
                                Venta = d.Venta,
                                Remision = d.Remision,
                                Fecha = d.Fecha,
                                idVerificador = d.idVerificador
                            }).ToList()

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

        public int GetStatusidVenta(int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var oPayment = context.tFormaPagoes.Where(p => p.idVenta == idSale).FirstOrDefault();

                    return (int)oPayment.Estatus;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public OutProductsViewModel GetOutProductsForID(int idOutProduct, Dictionary<int, int> lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var listProducts = lProducts.Select(p => p.Key).ToList();

                    var outProducts = context.tVistas.Where(p => p.idVista == idOutProduct).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = p.idUsuario1,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        sCustomer = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        idVerificador = p.idVerificador,
                        oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista && listProducts.Contains((int)x.idProducto)).Select(x => new OutProductsDetailViewModel()
                        {

                            idDetalleVista = x.idDetalleVista,
                            idVista = x.idVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = x.Cantidad,
                            Comentarios = x.Comentarios,
                            Remision = p.Remision

                        }).ToList()


                    }).FirstOrDefault();

                    foreach (var d in outProducts.oDetail)
                    {

                        d.Cantidad = lProducts.Where(o => o.Key == d.idProducto).Select(o => o.Value).First();
                    }

                    return outProducts;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool SetStatus(int idOutProducts, short status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    bool result;

                    if (context.tDetalleVistas.Where(p => p.idVista == idOutProducts && p.Estatus == TypesOutProductsDetail.Pendiente).Any() == false)
                    {
                        tVista oOutProducts = context.tVistas.FirstOrDefault(p => p.idVista == idOutProducts);

                        oOutProducts.Estatus = status;

                        context.SaveChanges();

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                    return result;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStockReturnOutProducts(int idOutProducts, int[][] lProducts, short idUser)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var prod in lProducts)
                    {
                        int prodID = prod[0];
                        int prodCant = prod[1];

                        tDetalleVista oProductDetail =
                                context.tDetalleVistas.FirstOrDefault(p =>
                                    p.idVista == idOutProducts && p.idProducto == prodID);

                        if (oProductDetail.Cantidad < (((oProductDetail.Devolucion ?? 0) + (oProductDetail.Venta ?? 0)) + prodCant))
                        {
                            throw new Exception("Se esta excediendo el número de devoluciónes permitidas");
                        }
                    }

                    tVista oOutProducts = context.tVistas.FirstOrDefault(p => p.idVista == idOutProducts);

                    if (oOutProducts.Estatus == TypesOutProducts.Pendiente)
                    {
                        foreach (var prod in lProducts)
                        {
                            int prodID = prod[0];
                            int prodCant = prod[1];

                            this.AddRegisterProduct(prodID, (int)oOutProducts.idSucursal, "Producto regresa a la sucursal de la vista " + oOutProducts.Remision, this.GetProductStockForBranch(prodID, (int)oOutProducts.idSucursal), this.GetProductStockForBranch(prodID, (int)oOutProducts.idSucursal), context.tProductos.FirstOrDefault(p => p.idProducto == prodID).PrecioVenta, context.tProductos.FirstOrDefault(p => p.idProducto == prodID).PrecioVenta, String.Empty, (int)oOutProducts.idUsuario1);

                            tDetalleVista oProductDetail =
                                context.tDetalleVistas.FirstOrDefault(p =>
                                    p.idVista == idOutProducts && p.idProducto == prodID);

                            var dev = oProductDetail.Devolucion ?? 0;

                            oProductDetail.Devolucion = dev + prodCant;

                            dev = oProductDetail.Devolucion ?? 0;
                            var ven = oProductDetail.Venta ?? 0;
                            var result = dev + ven;

                            //Se cambia el Estatus a Entregado solo si la suma de los campos Devolucion y Venta son igual al campo Cantidad
                            oProductDetail.Estatus = (oProductDetail.Cantidad == result)
                                ? TypesOutProductsDetail.Entregado
                                : TypesOutProductsDetail.Pendiente;

                            context.SaveChanges();

                            //Se inserta el detalle en la tabla tDetalleDevoluciones
                            tDetalleDevolucione oDetalleDevoluciones = new tDetalleDevolucione()
                            {
                                idDetalleVista = oProductDetail.idDetalleVista,
                                Devolucion = prodCant,
                                Venta = 0,
                                Remision = "",
                                Fecha = DateTime.Now,
                                idVerificador = idUser
                            };

                            context.tDetalleDevoluciones.Add(oDetalleDevoluciones);
                            context.SaveChanges();
                        }

                        //Si ya no existen productos pendientes se cambia el estatus general de la Salida a Vista por entregado
                        if (context.tDetalleVistas.Where(p =>
                                p.idVista == idOutProducts &&
                                p.Estatus == TypesOutProductsDetail.Pendiente).Count() == 0)
                        {
                            tVista rOutProducts = context.tVistas.FirstOrDefault(p => p.idVista == idOutProducts);
                            rOutProducts.Estatus = TypesOutProducts.Entregado;
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

        public void UpdateStockSaleOutProducts(string remision, int idOutProducts, List<KeyValuePair<int?, decimal?>> lProducts, short idUser)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tVista oOutProducts = context.tVistas.FirstOrDefault(p => p.idVista == idOutProducts);

                    foreach (var prod in lProducts)
                    {

                        int prodID = (int)prod.Key;
                        int prodCant = (int)prod.Value;

                        tDetalleVista oProductDetail = context.tDetalleVistas.FirstOrDefault(p => p.idVista == idOutProducts && p.idProducto == prodID);

                        if (oProductDetail != null)
                        {

                            oProductDetail.Venta = (oProductDetail.Venta ?? 0) + prodCant;

                            var dev = oProductDetail.Devolucion ?? 0;
                            var ven = oProductDetail.Venta ?? 0;

                            var result = dev + ven;

                            oProductDetail.Estatus = (oProductDetail.Cantidad == result) ? TypesOutProductsDetail.Entregado : TypesOutProductsDetail.Pendiente;

                            context.SaveChanges();

                            //Se inserta el detalle en la tabla tDetalleDevoluciones
                            tDetalleDevolucione oDetalleDevoluciones = new tDetalleDevolucione()
                            {

                                idDetalleVista = oProductDetail.idDetalleVista,
                                Devolucion = 0,
                                Venta = prodCant,
                                Remision = remision,
                                Fecha = DateTime.Now,
                                idVerificador = idUser
                            };

                            context.tDetalleDevoluciones.Add(oDetalleDevoluciones);

                            context.SaveChanges();

                        }

                    }


                    //Si ya no existen productos pendiente cambia el estatus general de la Salida a Vista por entregado
                    if (context.tDetalleVistas.Where(p => p.idVista == idOutProducts && p.Estatus == TypesOutProductsDetail.Pendiente).Count() == 0)
                    {

                        tVista rOutProducts = context.tVistas.FirstOrDefault(p => p.idVista == idOutProducts);

                        rOutProducts.Estatus = TypesOutProducts.Entregado;

                        context.SaveChanges();

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void ValidatePurchaseSale(int idDetailView)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var productView = context.tDetalleVistas.Where(p => p.idDetalleVista == idDetailView).Select(p =>
                    new OutProductsDetailViewModel()
                    {
                        idProducto = p.idProducto,
                        Precio = p.Precio,
                        Cantidad = p.Cantidad
                    }).FirstOrDefault();

                if (productView != null)
                {
                    var purchaseSale = context.tProductos.Where(p => p.idProducto == productView.idProducto)
                        .Select(p => p.PrecioVenta).FirstOrDefault();

                    if (productView.Precio != purchaseSale)
                    {
                        throw new Exception("El producto debe ser reetiquetado");
                    }
                }
            }
        }

        public void ReturnProducts(int idView, int idBranch)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    List<OutProductsDetailViewModel> lOutProductDetail = context.tDetalleVistas.Where(p => p.idVista == idView).Select(p => new OutProductsDetailViewModel()
                    {

                        idProducto = p.idProducto,
                        Cantidad = p.Cantidad

                    }).ToList();


                    foreach (var product in lOutProductDetail)
                    {

                        tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == idBranch && p.idProducto == product.idProducto);

                        context.SaveChanges();

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<ServiceOutProductViewModel> GetServiceOutProducts(int idView)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    var outProductsDetails = (from d in context.tServicioVistas
                                              where d.idVista == idView && d.Estatus == TypesOutProducts.Pendiente
                                              select new ServiceOutProductViewModel()
                                              {

                                                  idServicioVista = d.idServicioVista,
                                                  idVista = d.idVista,
                                                  idServicio = d.idServicio,
                                                  Descripcion = d.tServicio.Descripcion,
                                                  Precio = d.Precio,
                                                  Cantidad = d.Cantidad

                                              }).OrderBy(d => d.idServicioVista);

                    return outProductsDetails.ToList();

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

                        tServicioVista tService = new tServicioVista();

                        tService.idVista = service.idVista;
                        tService.idServicio = service.idServicio;
                        tService.Precio = service.Precio;
                        tService.Cantidad = service.Cantidad;
                        tService.Estatus = TypesOutProducts.Pendiente;
                        tService.Comentarios = service.Comentarios;

                        context.tServicioVistas.Add(tService);

                        context.SaveChanges();

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void UpdateStatusServiceOutProducts(List<ServiceOutProductViewModel> lServices)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    foreach (var service in lServices)
                    {

                        tServicioVista tService = context.tServicioVistas.FirstOrDefault(p => p.idServicioVista == service.idServicioVista);

                        tService.Estatus = TypesOutProducts.Entregado;

                        context.SaveChanges();

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void UpdateStatusServiceOutProducts(int idOutProduct)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    List<tServicioVista> lServices = context.tServicioVistas.Where(p => p.idVista == idOutProduct).ToList();

                    foreach (var service in lServices)
                    {

                        tServicioVista tService = context.tServicioVistas.FirstOrDefault(p => p.idServicioVista == service.idServicioVista);

                        tService.Estatus = TypesOutProducts.Entregado;

                        context.SaveChanges();

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<ProductSaleViewModel> GetPendingProducts(int idOutProduct, List<KeyValuePair<int?, int?>> lProducts)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    List<ProductSaleViewModel> lResult = new List<ProductSaleViewModel>();

                    foreach (var product in lProducts)
                    {
                        ProductSaleViewModel record = context.tDetalleVistas.Where(p => p.idVista == idOutProduct && p.idProducto == product.Key && p.Estatus == TypesOutProducts.Entregado).Select(p => new ProductSaleViewModel()
                        {

                            idProducto = p.idProducto,
                            cantidad = (((p.Cantidad - (p.Devolucion + p.Venta)) - product.Value) < 0) ? 0 : ((p.Cantidad - (p.Devolucion + p.Venta)) - product.Value)

                        }).FirstOrDefault();

                        lResult.Add(record);
                    }

                    return lResult;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void AddFleteOutProduct(int idOutProduct, int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var oOutProduct = context.tVistas.FirstOrDefault(p => p.idVista == idOutProduct);

                    oOutProduct.idVenta = idSale;
                    oOutProduct.EstatusFlete = TypesOutFleteEstatus.Pendiente;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void UpdateFleteOutProduct(int idOutProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var oOutProduct = context.tVistas.FirstOrDefault(p => p.idVista == idOutProduct);

                    oOutProduct.EstatusFlete = TypesOutFleteEstatus.Aplicado;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int GetIDSalePending(int idOutProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int iResult = 0;

                    var oOutProduct = context.tVistas.Where(p => p.idVista == idOutProduct && p.EstatusFlete == TypesOutFleteEstatus.Pendiente).FirstOrDefault();

                    if (oOutProduct != null)
                    {

                        iResult = (int)oOutProduct.idVenta;

                    }

                    return iResult;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<OutProductsViewModel> GetMultiOutProductsForRemision(List<string> remissions)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var outProducts = context.tVistas.Where(p => remissions.Contains(p.Remision.ToUpper())).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = p.idUsuario1,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        sCustomer = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho1.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        Restante = p.tDetalleVistas.Sum(dt => dt.Precio * ((dt.Cantidad ?? 0) - ((dt.Devolucion ?? 0) + (dt.Venta ?? 0)))),
                        Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        idVerificador = p.idVerificador,
                        iEstatus = p.Estatus,
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

                            Correo = p.tDespacho1.Correo,
                            TelCasa = p.tDespacho1.Telefono,
                            TelCelular = "",
                            Direccion = p.tDespacho1.Calle + " " + p.tDespacho1.NumExt + " " + p.tDespacho1.NumInt + " " + p.tDespacho1.Colonia + " " + ((p.tDespacho1.tMunicipio != null) ? p.tDespacho1.tMunicipio.nombre_municipio : "") + " " + p.tDespacho1.CP

                        },
                        oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista).Select(x => new OutProductsDetailViewModel()
                        {
                            idDetalleVista = x.idDetalleVista,
                            idVista = x.idVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = x.Cantidad,
                            Pendiente = x.Cantidad - ((x.Devolucion ?? 0) + (x.Venta ?? 0)),
                            Devolucion = x.Devolucion ?? 0,
                            Venta = x.Venta ?? 0,
                            Extension = x.tProducto.Extension,
                            TipoImagen = x.tProducto.TipoImagen,
                            urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen,
                            iEstatus = x.Estatus,
                            Comentarios = x.Comentarios,
                            oHistoryDetail = context.tDetalleDevoluciones.Where(d => d.idDetalleVista == x.idDetalleVista).Select(d => new DetailDevOutProductViewModel()
                            {

                                idDetalleDevoluciones = d.idDetalleDevoluciones,
                                idDetalleVista = d.idDetalleVista,
                                Devolucion = d.Devolucion,
                                Venta = d.Venta,
                                Remision = d.Remision,
                                Fecha = d.Fecha,
                                idVerificador = d.idVerificador
                            }).ToList()

                        }).ToList()

                    }).ToList();

                    return outProducts;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public List<OutProductsViewModel> GetMultiOutProductsPending(List<string> remissions)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    var outProducts = context.tVistas.Where(p => remissions.Contains(p.Remision.ToUpper())).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = p.idUsuario1,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        sCustomer = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho1.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        Restante = p.tDetalleVistas.Sum(dt => dt.Precio * ((dt.Cantidad ?? 0) - ((dt.Devolucion ?? 0) + (dt.Venta ?? 0)))),
                        Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        idVerificador = p.idVerificador,
                        iEstatus = p.Estatus,
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

                            Correo = p.tDespacho1.Correo,
                            TelCasa = p.tDespacho1.Telefono,
                            TelCelular = "",
                            Direccion = p.tDespacho1.Calle + " " + p.tDespacho1.NumExt + " " + p.tDespacho1.NumInt + " " + p.tDespacho1.Colonia + " " + ((p.tDespacho1.tMunicipio != null) ? p.tDespacho1.tMunicipio.nombre_municipio : "") + " " + p.tDespacho1.CP

                        },
                        oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista && p.Estatus == TypesOutProductsDetail.Pendiente).Select(x => new OutProductsDetailViewModel()
                        {
                            idDetalleVista = x.idDetalleVista,
                            idVista = x.idVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = (x.Cantidad - ((x.Devolucion ?? 0) + (x.Venta ?? 0))),
                            Devolucion = x.Devolucion ?? 0,
                            Venta = x.Venta ?? 0,
                            Extension = x.tProducto.Extension,
                            TipoImagen = x.tProducto.TipoImagen,
                            urlImagen = x.tProducto.TipoImagen == 1 ? "/Content/Products/" + x.tProducto.NombreImagen + x.tProducto.Extension : x.tProducto.urlImagen,
                            iEstatus = x.Estatus,
                            Comentarios = x.Comentarios,
                            Remision = p.Remision,
                            oHistoryDetail = context.tDetalleDevoluciones.Where(d => d.idDetalleVista == x.idDetalleVista).Select(d => new DetailDevOutProductViewModel()
                            {

                                idDetalleDevoluciones = d.idDetalleDevoluciones,
                                idDetalleVista = d.idDetalleVista,
                                Devolucion = d.Devolucion,
                                Venta = d.Venta,
                                Remision = d.Remision,
                                Fecha = d.Fecha,
                                idVerificador = d.idVerificador
                            }).ToList()

                        }).ToList()

                    }).ToList();

                    return outProducts;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //Pending Product
        public OutProductsDetailViewModel GetPendingProduct(int idOutProduct, int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tDetalleVistas.Where(p => p.idVista == idOutProduct && p.idProducto == idProduct).Select(p => new OutProductsDetailViewModel()
                    {
                        idDetalleVista = p.idDetalleVista,
                        idVista = p.idVista,
                        idProducto = p.idProducto,
                        Codigo = p.tProducto.Codigo,
                        Descripcion = p.tProducto.Descripcion,
                        Precio = p.Precio,
                        Cantidad = p.Cantidad,
                        Devolucion = p.Devolucion,
                        Venta = p.Venta,
                        Comentarios = p.Comentarios,
                        Pendiente = p.Cantidad - ((p.Devolucion ?? 0) + (p.Venta ?? 0))
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /*COMIENZA CODIGO DE LAS SALIDAS A VISTA  UNIFICADAS*/
        public void UpdateStockReturnOutProducts(List<OutProductsDetailViewModel> lProducts)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (OutProductsDetailViewModel prod in lProducts.ToList())
                    {
                        this.AddRegisterProduct((int)prod.idProducto, (int)this.GetViewForIdView(prod.idVista).idSucursal, "Producto regresa a la sucursal de la vista " + this.GetViewForIdView(prod.idVista).remision, this.GetProductStockForBranch((int)prod.idProducto, (int)this.GetViewForIdView(prod.idVista).idSucursal), this.GetProductStockForBranch((int)prod.idProducto, (int)this.GetViewForIdView(prod.idVista).idSucursal), prod.Precio, prod.Precio, String.Empty, (int)this.GetViewForIdView(prod.idVista).idUsuario1);

                        tDetalleVista oProductDetail = context.tDetalleVistas.FirstOrDefault(p => p.idVista == prod.idVista && p.idProducto == prod.idProducto);

                        var dev = oProductDetail.Devolucion ?? 0;

                        oProductDetail.Devolucion = dev + prod.Devolucion;

                        dev = oProductDetail.Devolucion ?? 0;
                        var ven = oProductDetail.Venta ?? 0;
                        var result = dev + ven;

                        //Se cambia el Estatus a Entregado solo si la suma de los campos Devolucion y Venta son igual al campo Cantidad
                        oProductDetail.Estatus = (oProductDetail.Cantidad == result) ? TypesOutProductsDetail.Entregado : TypesOutProductsDetail.Pendiente;

                        context.SaveChanges();

                        //Se inserta el detalle en la tabla tDetalleDevoluciones
                        tDetalleDevolucione oDetalleDevoluciones = new tDetalleDevolucione()
                        {
                            idDetalleVista = oProductDetail.idDetalleVista,
                            Devolucion = prod.Devolucion,
                            Venta = 0,
                            Remision = "",
                            Fecha = DateTime.Now,
                            idVerificador = prod.idUsuario
                        };

                        context.tDetalleDevoluciones.Add(oDetalleDevoluciones);
                        context.SaveChanges();

                    }
                    foreach (OutProductsDetailViewModel prod in lProducts.ToList())
                    {
                        //Si ya no existen productos pendientes se cambia el estatus general de la Salida a Vista por entregado
                        if (context.tDetalleVistas.Where(p => p.idVista == prod.idVista && p.Estatus == TypesOutProductsDetail.Pendiente).Count() == 0)
                        {

                            tVista rOutProducts = context.tVistas.FirstOrDefault(p => p.idVista == prod.idVista);
                            rOutProducts.Estatus = TypesOutProducts.Entregado;
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

        public OutProductsViewModel GetUnifyOutProductsForID(int idOutProduct, Dictionary<int, int> lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var listProducts = lProducts.Select(p => p.Key).ToList();

                    var outProducts = context.tVistas.Where(p => listProducts.Contains(p.idVista)).Select(p => new OutProductsViewModel()
                    {

                        idVista = p.idVista,
                        remision = p.Remision,
                        idUsuario1 = p.idUsuario1,
                        ClienteFisico = (from cf in context.tClientesFisicos where cf.idCliente == p.idClienteFisico select cf.Nombre).FirstOrDefault(),
                        ClienteMoral = (from cm in context.tClientesMorales where cm.idCliente == p.idClienteMoral select cm.Nombre).FirstOrDefault(),
                        Despacho = (from cd in context.tDespachoes where cd.idDespacho == p.idDespacho select cd.Nombre).FirstOrDefault(),
                        sCustomer = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Total = p.Total,
                        Vendedor = (from v in context.tUsuarios where v.idUsuario == p.idUsuario1 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        Vendedor2 = (from v in context.tUsuarios where v.idUsuario == p.idUsuario2 select v.Nombre + " " + v.Apellidos).FirstOrDefault(),
                        idVerificador = p.idVerificador,
                        oDetail = p.tDetalleVistas.Where(x => x.idVista == p.idVista && listProducts.Contains((int)x.idProducto)).Select(x => new OutProductsDetailViewModel()
                        {

                            idDetalleVista = x.idDetalleVista,
                            idVista = x.idVista,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            Descripcion = x.tProducto.Descripcion,
                            Precio = x.Precio,
                            Cantidad = x.Cantidad,
                            Comentarios = x.Comentarios

                        }).ToList()


                    }).FirstOrDefault();

                    foreach (var d in outProducts.oDetail)
                    {
                        d.Cantidad = lProducts.Where(o => o.Key == d.idProducto).Select(o => o.Value).First();
                    }

                    return outProducts;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStockSaleOutProducts(string remision, string[,] lProducts, short idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    for (int j = 0; j < lProducts.GetLength(0); j++)
                    {
                        int prodID = int.Parse(lProducts[j, 0]);
                        decimal prodCant = decimal.Parse(lProducts[j, 1]);
                        int idView = int.Parse(lProducts[j, 2]);

                        tDetalleVista oProductDetail = context.tDetalleVistas.FirstOrDefault(p => p.idVista == idView && p.idProducto == prodID);

                        if (oProductDetail != null)
                        {
                            if (oProductDetail.Estatus == TypesOutProductsDetail.Entregado)
                            {
                                oProductDetail.Devolucion = 0;
                            }

                            oProductDetail.Venta = (oProductDetail.Venta ?? 0) + prodCant;

                            var dev = oProductDetail.Devolucion ?? 0;
                            var ven = oProductDetail.Venta ?? 0;

                            var result = dev + ven;

                            oProductDetail.Estatus = (oProductDetail.Cantidad == result) ? TypesOutProductsDetail.Entregado : TypesOutProductsDetail.Pendiente;

                            context.SaveChanges();

                            //Se inserta el detalle en la tabla tDetalleDevoluciones
                            tDetalleDevolucione oDetalleDevoluciones = new tDetalleDevolucione()
                            {
                                idDetalleVista = oProductDetail.idDetalleVista,
                                Devolucion = 0,
                                Venta = prodCant,
                                Remision = remision,
                                Fecha = DateTime.Now,
                                idVerificador = idUser
                            };

                            context.tDetalleDevoluciones.Add(oDetalleDevoluciones);

                            context.SaveChanges();
                        }

                        //Si ya no existen productos pendiente cambia el estatus general de la Salida a Vista por entregado
                        if (context.tDetalleVistas.Where(p => p.idVista == idView && p.Estatus == TypesOutProductsDetail.Pendiente).Count() == 0)
                        {
                            tVista rOutProducts = context.tVistas.FirstOrDefault(p => p.idVista == idView);
                            rOutProducts.Estatus = TypesOutProducts.Entregado;
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

        public void UpdateSaleFromView(int idOutProduct, int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var outProduct = context.tVistas.FirstOrDefault(p => p.idVista == idOutProduct);

                    if (outProduct != null)
                    {
                        outProduct.idVenta = idSale;

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void SaveEditOutView(OutProductsViewModel oView)
        {
            using (var context = new admDB_SAADDBEntities())

                try
                {
                    var outProduct = context.tVistas.FirstOrDefault(p => p.idVista == oView.idVista);

                    if (outProduct == null) throw new ArgumentNullException(nameof(outProduct));

                    outProduct.TipoCliente = oView.TipoCliente;
                    outProduct.idClienteFisico = oView.idClienteFisico;
                    outProduct.idClienteMoral = oView.idClienteMoral;
                    outProduct.idDespacho = oView.idDespacho;
                    outProduct.idDespachoReferencia = oView.idDespachoReferencia;
                    outProduct.Proyecto = oView.Proyecto;
                    outProduct.idUsuario1 = oView.idUsuario1;
                    outProduct.idUsuario2 = oView.idUsuario2;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /*Se valida Salida a Vista ligada a cotización*/
        public void RelatedOutProductAtQuotation(int idQuotation, List<ProductSaleViewModel> lProducts, string rem, int idUser)
        {
            using (var context = new admDB_SAADDBEntities())

                try
                {
                    if (context.tCotizacions.Any(p => p.idCotizacion == idQuotation))
                    {
                        //Se obtiene el detalle de la Cotización
                        var qDetail = context.tCotizacionDetalles
                            .Where(p => p.idCotizacion == idQuotation).Select(p => p.idVista).ToList();

                        //Se obtiene el detalle de la Salida a Vista
                        var detail = context.tDetalleVistas.Where(p => qDetail.Any(v => v == p.idVista)).ToList();

                        //Se compara el detalle de la Venta con el Detalle de Salida a Vista
                        if (detail != null)
                        {
                            foreach (var product in detail)
                            {
                                foreach (var qProduct in lProducts)
                                {
                                    var lProd = new string[1, 3];

                                    //Se valida que el artículo vendido pertenezca a la Salida a Vista
                                    if ((product.idVista == qProduct.idVista) && (product.idProducto == qProduct.idProducto))
                                    {
                                        //Si la cantidad vendida es igual a la pendiente
                                        if ((this.GetPendingProduct(product.idVista, (int)product.idProducto).Pendiente) ==
                                            qProduct.cantidad)
                                        {
                                            lProd[0, 0] = product.idProducto.ToString();
                                            lProd[0, 1] = qProduct.cantidad.ToString();
                                            lProd[0, 2] = product.idVista.ToString();
                                        }
                                        //Si la cantidad vendida es menor a la pendiente
                                        else if ((this.GetPendingProduct(product.idVista, (int)product.idProducto).Pendiente) >
                                                 qProduct.cantidad)
                                        {
                                            lProd[0, 0] = product.idProducto.ToString();
                                            lProd[0, 1] = qProduct.cantidad.ToString();
                                            lProd[0, 2] = product.idVista.ToString();
                                        }
                                        //Si la cantidad vendida es mayor a la pendiente
                                        else if ((this.GetPendingProduct(product.idVista, (int)product.idProducto)
                                                     .Pendiente) <
                                                 qProduct.cantidad)
                                        {
                                            lProd[0, 0] = product.idProducto.ToString();
                                            lProd[0, 1] = this.GetPendingProduct(product.idVista, (int)product.idProducto).Pendiente.ToString();
                                            lProd[0, 2] = product.idVista.ToString();
                                        }

                                        this.UpdateStockSaleOutProducts(rem, lProd, (short)idUser);
                                    }
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
        /// <summary>
        /// Checks the stock return products.
        /// </summary>
        /// <param name="idOutProducts">The identifier out products.</param>
        /// <param name="lProducts">The l products.</param>
        /// <exception cref="System.Exception">Se esta excediendo el número de devoluciónes permitidas</exception>
        public void CheckStockReturnProducts(int idOutProducts, int[][] lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                foreach (var prod in lProducts)
                {
                    int prodID = prod[0];
                    int prodCant = prod[1];

                    tDetalleVista oProductDetail =
                            context.tDetalleVistas.FirstOrDefault(p =>
                                p.idVista == idOutProducts && p.idProducto == prodID);

                    if (oProductDetail.Cantidad < (((oProductDetail.Devolucion ?? 0) + (oProductDetail.Venta ?? 0)) + prodCant))
                    {
                        throw new Exception("Se esta excediendo el número de devoluciónes permitidas");
                    }

                }

            }

        }
        /// <summary>
        /// Checks the stock return out products.
        /// </summary>
        /// <param name="lProducts">The l products.</param>
        /// <exception cref="System.Exception">Se esta excediendo el número de devoluciónes permitidas</exception>
        public void CheckStockReturnOutProducts(List<OutProductsDetailViewModel> lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                foreach (OutProductsDetailViewModel prod in lProducts.ToList())
                {
                    tDetalleVista oProductDetail = context.tDetalleVistas.FirstOrDefault(p => p.idVista == prod.idVista && p.idProducto == prod.idProducto);

                    var dev = oProductDetail.Devolucion ?? 0;

                    oProductDetail.Devolucion = dev + prod.Devolucion;

                    dev = oProductDetail.Devolucion ?? 0;
                    var ven = oProductDetail.Venta ?? 0;
                    var result = dev + ven;

                    //Se compara la Cantidad con la suma de Devolucion y Venta si cantidad sale menor se manda una exception
                    if(oProductDetail.Cantidad < result)
                    {
                        throw new Exception("Se esta excediendo el número de devoluciónes permitidas");
                    }

                }
               
            }

        }
    }
}
