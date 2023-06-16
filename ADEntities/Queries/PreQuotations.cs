using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace ADEntities.Queries
{
    public class PreQuotations : Base
    {
        public int CountRegisters(string number, string costumer, int? iduserforsearch, string project, int idUser, short? restricted, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tPreCotizacions.Where(
                        p => (p.Proyecto.ToUpper().Contains(project.ToUpper()) || String.IsNullOrEmpty(project)) &&
                             (p.Numero.ToUpper().Contains(number.ToUpper()) || String.IsNullOrEmpty(number)) &&
                        ((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
                        (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())) ||
                        ((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                        ((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))))) &&
                        (((p.tSucursale.idSucursal == amazonas) || (p.tSucursale.idSucursal == guadalquivir) || (p.tSucursale.idSucursal == textura)) ||
                        (amazonas == null && guadalquivir == null && textura == null)
                        && (p.idEstatus == TypesQuotations.activo))).Count();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

        }

        public List<PreQuotationsViewModel> GetPreQuotations(bool allTime, DateTime dtDateSince, DateTime dtDateUntil, string number, string costumer, int? iduserforsearch, string project, int idUser, short? restricted, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
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
                        dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                        dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);
                    }
                    IOrderedQueryable<PreQuotationsViewModel> preQuotations;

                    preQuotations = context.tPreCotizacions.Where(
                    p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                    (p.Proyecto.ToUpper().Contains(project.ToUpper()) || String.IsNullOrEmpty(project)) &&
                    (p.Numero.ToUpper().Contains(number.ToUpper()) || String.IsNullOrEmpty(number)) &&
                    ((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
                    (((p.tSucursale.idSucursal == amazonas) ||
                    (p.tSucursale.idSucursal == guadalquivir) ||
                    (p.tSucursale.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null) &&
                    (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())) ||
                    ((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                    ((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())
                    )))))


                ).Select(p => new PreQuotationsViewModel()
                {

                    idPreCotizacion = p.idPreCotizacion,
                    Numero = p.Numero,
                    idUsuario1 = p.idUsuario1,
                    Usuario1 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                    idUsuario2 = p.idUsuario2,
                    Usuario2 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                    idClienteFisico = p.idClienteFisico,
                    ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                    idClienteMoral = p.idClienteMoral,
                    ClienteMoral = p.tClientesMorale.Nombre,
                    idDespacho = p.idDespacho,
                    Proyecto = p.Proyecto,
                    idDespachoReferencia = p.idDespachoReferencia,
                    Despacho = p.tDespacho.Nombre,
                    DespachoReferencia = p.tDespacho1.Nombre,
                    idSucursal = p.idSucursal,
                    Sucursal = p.tSucursale.Nombre,
                    Fecha = p.Fecha,
                    CantidadProductos = p.CantidadProductos,
                    Estatus = p.idEstatus,
                    sEstatus = context.tEstatusPrecotizacions.Where(o => o.idEstatus == p.idEstatus).FirstOrDefault().Nombre,
                    oDetail = p.tPreCotizacionDetalles.Where(x => x.idPreCotizacion == p.idPreCotizacion).Select(x => new PreQuotationsDetailsViewModel()
                    {
                        idPreCotizacionDetalle = x.idPreCotizacionDetalle,
                        idPreCotizacion = x.idPreCotizacion,
                        idServicio = x.idServicio,
                        NombreTipoServicio = context.tTipoServicio.Where(o => o.idTipoServicio == x.idServicio).FirstOrDefault().Descripcion,
                        Descripcion = x.Descripcion,
                        Cantidad = x.Cantidad,
                        Imagen = x.Imagen != null ? "/content/services/" + x.Imagen : null,
                        PDF = x.PDF,
                        Comentarios = x.Comentarios,
                        idEstatus = x.idEstatusServicio,
                        NombreEstatus = context.tEstatusServicios.Where(e => e.idEstatusServicio == x.idEstatusServicio).FirstOrDefault().Nombre,
                        measures = x.tPreCotDetalleMedidas.Where(z => z.idPreCotizacionDetalle == x.idPreCotizacionDetalle).Select(z => new PreCotDetalleMedidasViewModel()
                        {
                            idPreCotDetalleMedidas = z.idPreCotDetalleMedidas,
                            idPreCotizacionDetalle = z.idPreCotizacionDetalle,
                            idServicio = z.idServicio,
                            idTipoMedida = z.idTipoMedida,
                            NombreTipoMedida = context.tTipoMedida.Where(w => w.idTipoMedida == z.idTipoMedida).FirstOrDefault().NombreMedida,
                            Valor = z.Valor
                        }).ToList(),
                        fabrics = x.tPreCotDetalleTipoTelas.Where(a => a.idPreCotizacionDetalle == x.idPreCotizacionDetalle).Select(a => new PreCotDetalleTipoTelaViewModel()
                        {
                            idPreCotDetalleTipoTela = a.idPreCotDetalleTipoTela,
                            idPreCotizacionDetalle = a.idPreCotizacionDetalle,
                            idServicio = a.idServicio,
                            idTextiles = a.idTextiles,
                            NombreTextiles = context.tTextiles.Where(t => t.idTextiles == a.idTextiles).FirstOrDefault().NombreTela,
                            CostoPorMts = a.CostoPorMts,
                            ValorMts = a.ValorMts,
                            CostoTotal = a.CostoTotal
                        }).ToList()
                    }).ToList(),
                }).OrderByDescending(p => p.Fecha);

                    List<PreQuotationsViewModel> lQuotations = preQuotations.Skip(page * pageSize).Take(pageSize).ToList();

                    if (restricted == 1)
                    {
                        lQuotations = lQuotations.Where(p => p.idUsuario1 == idUser).ToList();
                    }

                    return lQuotations;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //pre cotizacion en estatus 1 "nueva", servicios en estatus 1 "nuevo"
        public int AddPreQuotation(PreQuotationsViewModel data)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var pEstatusNuevo = context.tEstatusPrecotizacions.Where(o => o.Nombre == "NUEVA").FirstOrDefault().idEstatus;

                    tPreCotizacion oPreQuotation = new tPreCotizacion();

                    oPreQuotation.Numero = data.Numero;
                    oPreQuotation.idUsuario1 = data.idUsuario1;
                    oPreQuotation.idUsuario2 = data.idUsuario2;
                    oPreQuotation.idClienteFisico = data.idClienteFisico;
                    oPreQuotation.idClienteMoral = data.idClienteMoral;
                    oPreQuotation.idDespacho = data.idDespacho;
                    oPreQuotation.Proyecto = data.Proyecto;
                    oPreQuotation.idDespachoReferencia = data.idDespachoReferencia;
                    oPreQuotation.idSucursal = data.idSucursal;
                    oPreQuotation.Fecha = Convert.ToDateTime(data.Fecha);
                    oPreQuotation.CantidadProductos = data.CantidadProductos;
                    oPreQuotation.Total = data.Total;
                    oPreQuotation.Comentarios = data.Comentarios;
                    oPreQuotation.TipoCliente = data.TipoCliente;
                    oPreQuotation.idEstatus = pEstatusNuevo;

                    context.tPreCotizacions.Add(oPreQuotation);
                    context.SaveChanges();

                    //enviar a historial precotizacion nueva
                    this.HistoryPreQuotation(oPreQuotation.idPreCotizacion, pEstatusNuevo);

                    //buscar idEstatus de servicio (nuevo)
                    int idEstatusServicioNew = context.tEstatusServicios.Where(o => o.Nombre == "NUEVO").FirstOrDefault().idEstatusServicio;

                    if (data.oDetail != null)
                    {
                        foreach (var detail in data.oDetail)
                        {
                            detail.idEstatus = idEstatusServicioNew;
                            detail.idPreCotizacion = oPreQuotation.idPreCotizacion;
                            this.AddPreQuotationDetail(detail);
                        }
                    }
                    return oPreQuotation.idPreCotizacion;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //pre cotizacion en estatus 2 "en cotizacion", servicio en estatus 2 "enviado a proveedor"
        public void AddProviderService(List<PreCotDetalleProveedoresViewModel> data, int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int? idPreCotDetalle = data[0].idPreCotizacionDetalle;
                    var TelasServicio = context.tPreCotDetalleTipoTelas.Where(o => o.idPreCotizacionDetalle == idPreCotDetalle).ToList();

                    int sEnviadoProveedor = context.tEstatusServicios.Where(o => o.Nombre == "ENVIADO A PROVEEDOR").FirstOrDefault().idEstatusServicio;
                    int sEstatusNuevo = context.tEstatusServicios.Where(o => o.Nombre == "NUEVO").FirstOrDefault().idEstatusServicio;

                    var pEnCotizacion = context.tEstatusPrecotizacions.Where(o => o.Nombre == "EN COTIZACION").FirstOrDefault().idEstatus;
                    int pEstatusNuevo = context.tEstatusPrecotizacions.Where(o => o.Nombre == "NUEVA").FirstOrDefault().idEstatus;

                    var PreQuotationDetail = context.tPreCotizacionDetalles.Find(idPreCotDetalle);
                    var PreQuotation = context.tPreCotizacions.Find(PreQuotationDetail.idPreCotizacion);

                    string Servicio = context.tServicios.Where(o => o.idServicio == PreQuotationDetail.idServicio).FirstOrDefault().Descripcion;
                    string NoPreQuotation = context.tPreCotizacions.Where(o => o.idPreCotizacion == PreQuotationDetail.idPreCotizacion).FirstOrDefault().Numero;
                    string NombreNotificacion =  Servicio + "DE "+ NoPreQuotation + " PENDIENTE RESPUESTA DE PROVEEDOR" ;
                    if(NombreNotificacion.Length > 100)
                        NombreNotificacion = NombreNotificacion.Substring(0, 100);

                    var DateNotification = DateTime.Now.Date.AddDays(7);

                    foreach (var item in data)
                    {
                        tPreCotDetalleProveedore Proveedores = new tPreCotDetalleProveedore();

                        Proveedores.idPreCotizacionDetalle = item.idPreCotizacionDetalle;
                        Proveedores.idServicio = item.idTipoServicio;
                        Proveedores.idProveedor = item.idProveedor;
                        Proveedores.CostoFabricacion = null;
                        Proveedores.DiasFabricacion = null;
                        Proveedores.ComentariosProveedor = null;
                        Proveedores.ComentariosComprador = null;
                        Proveedores.Asignado = null;
                        Proveedores.Enviado = 1;

                        context.tPreCotDetalleProveedores.Add(Proveedores);
                        context.SaveChanges();

                        foreach(var item2 in TelasServicio)
                        {
                            tPreCotProveedoresTela TelasProvedores = new tPreCotProveedoresTela();

                            TelasProvedores.idPreCotDetalleProveedores = Proveedores.idPreCotDetalleProveedores;
                            TelasProvedores.idServicio = item2.idServicio;
                            TelasProvedores.idProveedor = Proveedores.idProveedor;
                            TelasProvedores.idTextiles = item2.idTextiles;
                            TelasProvedores.ValorMts = null;

                            context.tPreCotProveedoresTelas.Add(TelasProvedores);
                            context.SaveChanges();
                        }
                    }

                    if (PreQuotationDetail.idEstatusServicio == sEstatusNuevo)
                    {
                        PreQuotationDetail.idEstatusServicio = sEnviadoProveedor;

                        if (PreQuotation.idEstatus == pEstatusNuevo)
                        {
                            PreQuotation.idEstatus = pEnCotizacion;
                            context.SaveChanges();

                            this.HistoryPreQuotation(PreQuotation.idPreCotizacion, pEnCotizacion);
                        }

                        var idEvento = this.AddNotification(NombreNotificacion, idUser, DateNotification);
                        PreQuotationDetail.idEvento = idEvento;
                        context.SaveChanges();

                        this.HistoryService(PreQuotationDetail.idPreCotizacion, PreQuotationDetail.idPreCotizacionDetalle, sEnviadoProveedor);
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

        }

        //servicio en estatus 3 "cotiazado"
        public void ComplementProviderService(PreCotDetalleProveedoresViewModel data)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int sCotizado = context.tEstatusServicios.Where(o => o.Nombre == "COTIZADO").FirstOrDefault().idEstatusServicio;
                    var ProveedorServicio = context.tPreCotDetalleProveedores.Find(data.idPreCotDetalleProveedores);

                    ProveedorServicio.CostoFabricacion = data.CostoFabricacion;
                    ProveedorServicio.DiasFabricacion = data.DiasFabricacion;
                    ProveedorServicio.ComentariosProveedor = data.ComentariosProveedor;
                    ProveedorServicio.ComentariosComprador = data.ComentariosComprador;
                    ProveedorServicio.Asignado = 0;

                    context.SaveChanges();

                    var idProveedorServicio = ProveedorServicio.idPreCotDetalleProveedores;

                    if(data.TelasProveedores != null)
                    {

                        foreach(var item in data.TelasProveedores)
                        {
                            var UpdateProvTelas = context.tPreCotProveedoresTelas.Find(item.idPreCotProveedoresTelas);

                            UpdateProvTelas.idPreCotDetalleProveedores = item.idPreCotDetalleProveedores;
                            UpdateProvTelas.idServicio = item.idServicio;
                            UpdateProvTelas.idProveedor = item.idProveedor;
                            UpdateProvTelas.idTextiles = item.idTextiles;
                            UpdateProvTelas.ValorMts = item.ValorMts;

                            context.SaveChanges();
                        }
                    }

                    var PreQuotationDetail = context.tPreCotizacionDetalles.Find(data.idPreCotizacionDetalle);

                    if (PreQuotationDetail.idEstatusServicio != sCotizado)
                    {
                        PreQuotationDetail.idEstatusServicio = sCotizado;
                        context.SaveChanges();

                        this.CancelNotification(PreQuotationDetail.idEvento);
                        this.HistoryService(PreQuotationDetail.idPreCotizacion, PreQuotationDetail.idPreCotizacionDetalle, sCotizado);
                    }

                    var pEnCotizacion = context.tEstatusPrecotizacions.Where(o => o.Nombre == "EN COTIZACION").FirstOrDefault().idEstatus;
                    var PreQuotation = context.tPreCotizacions.Find(PreQuotationDetail.idPreCotizacion);

                    if (PreQuotation.idEstatus != pEnCotizacion)
                    {
                        PreQuotation.idEstatus = pEnCotizacion;
                        context.SaveChanges();

                        this.HistoryPreQuotation(PreQuotation.idPreCotizacion, pEnCotizacion);
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //servicio en estatus 4 "asignado", pre cotizacion en estatus 3 "asignada" solo si todos los servicios estan en estatus 4 "asignado"
        public void AssingProvider(List<PreCotDetalleProveedoresViewModel> data)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int sAsignado = context.tEstatusServicios.Where(o => o.Nombre == "ASIGNADO").FirstOrDefault().idEstatusServicio;

                    int? idPreQuotationDetail = 0;
                    foreach (var item in data)
                    {
                        idPreQuotationDetail = item.idPreCotizacionDetalle;
                        var DetailProvider = context.tPreCotDetalleProveedores.Find(item.idPreCotDetalleProveedores);

                        DetailProvider.Asignado = 1;
                        context.SaveChanges();

                        if (item.TelasProveedores != null && item.TelasProveedores.Count > 0)
                        {

                            foreach (var item2 in item.TelasProveedores)
                            {
                                var tela = context.tPreCotDetalleTipoTelas.Where(a => a.idTextiles == item2.idTextiles && a.idPreCotizacionDetalle == item.idPreCotizacionDetalle).FirstOrDefault();

                                if (tela != null)
                                {
                                    if(item2.ValorMts != null && item2.ValorMts > 0)
                                    {
                                        tela.ValorMts = item2.ValorMts;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }

                    }
                    var ProviderUnasigned = context.tPreCotDetalleProveedores.Where(o => o.Asignado != 1 && o.idPreCotizacionDetalle == idPreQuotationDetail).ToList();
                    foreach(var item in ProviderUnasigned)
                    {
                        var TelasProveedores = context.tPreCotProveedoresTelas.Where(a => a.idPreCotDetalleProveedores == item.idPreCotDetalleProveedores && a.ValorMts == null || a.ValorMts == 0).ToList();
                        context.tPreCotProveedoresTelas.RemoveRange(TelasProveedores);
                        context.SaveChanges();
                    }


                    var PreQuotationDetail = context.tPreCotizacionDetalles.Find(idPreQuotationDetail);
                    if (PreQuotationDetail.idEstatusServicio != sAsignado)
                    {
                        PreQuotationDetail.idEstatusServicio = sAsignado;
                        context.SaveChanges();

                        //enviar a historial
                        this.HistoryService(PreQuotationDetail.idPreCotizacion, PreQuotationDetail.idPreCotizacionDetalle, sAsignado);
                    }

                    //validar si todo el detalle esta asignado
                    //var Asing = this.ValidateAsign(PreQuotationDetail.idPreCotizacion);

                    //si esta asignado cambiar estatus a precotizacion
                    //if (!Asing)
                    //{
                    var pEstatusAsign = context.tEstatusPrecotizacions.Where(o => o.Nombre == "ASIGNADA").FirstOrDefault().idEstatus;
                    var PreQuotation = context.tPreCotizacions.Find(PreQuotationDetail.idPreCotizacion);
                    if(PreQuotation.idEstatus != pEstatusAsign)
                    {

                        PreQuotation.idEstatus = pEstatusAsign;
                        context.SaveChanges();

                        //enviar a historial
                        this.HistoryPreQuotation(PreQuotation.idPreCotizacion, pEstatusAsign);
                    }

                    //}

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //pre cotizacion cambia a estatus 4 "en proceso de venta", servicios cambian a estatus 5 "en proceso de venta"
        public string SendCotizacion(int id)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int sAsignado = context.tEstatusServicios.Where(o => o.Nombre == "ASIGNADO").FirstOrDefault().idEstatusServicio;

                    var NumeroCotizacion = context.PrecotizacionCotizacion(id).FirstOrDefault();
                    var msg = "Se ha generado la cotizacion # " + NumeroCotizacion;

                    //obtener estatus 
                    var pProcesoVenta = context.tEstatusPrecotizacions.Where(o => o.Nombre == "EN PROCESO DE VENTA").FirstOrDefault().idEstatus;
                    int sProcesoVenta = context.tEstatusServicios.Where(o => o.Nombre == "EN PROCESO DE VENTA").FirstOrDefault().idEstatusServicio;

                    //obtener precotizacion y su detalle 
                    var PreQuotation = context.tPreCotizacions.Find(id);
                    var Detail = context.tPreCotizacionDetalles.Where(o => o.idPreCotizacion == id).ToList();

                    //cambiar estatus a precotizacion a 4 "en proceso de venta"
                    PreQuotation.idEstatus = pProcesoVenta;
                    context.SaveChanges();
                    //enviar a historial
                    this.HistoryPreQuotation(PreQuotation.idPreCotizacion, pProcesoVenta);

                    //cambiar estatus al detalle que este asignado a 5 "en proceso de venta"
                    foreach(var item in Detail)
                    {
                        if(item.idEstatusServicio == sAsignado)
                        {
                            item.idEstatusServicio = sProcesoVenta;
                            context.SaveChanges();

                            //enviar a historial
                            this.HistoryService(item.idPreCotizacion, item.idPreCotizacionDetalle, sProcesoVenta);
                        }
                    }

                    return msg;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //pre cotizacion cambia a estatus 5 "pendiente de ordenar", servicios cambian a estatus 6 "pendiente de ordenar"
        public void ConfirmSale(int? idCotizacion, int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //buscar idprecotizacion desde tabla relacional
                    var RelacionPrecot = context.tRelCotPrecots.Where(o => o.idCotizacion == idCotizacion).FirstOrDefault();
                    if(RelacionPrecot != null)
                    {
                        //obtener estatus 
                        var pPendienteOrdenar = context.tEstatusPrecotizacions.Where(o => o.Nombre == "PENDIENTE DE ORDENAR").FirstOrDefault().idEstatus;
                        int sPendienteOrdenar = context.tEstatusServicios.Where(o => o.Nombre == "PENDIENTE DE ORDENAR").FirstOrDefault().idEstatusServicio;

                        var PreQuotation = context.tPreCotizacions.Find(RelacionPrecot.idPreCotizacion);
                        var Detail = context.tPreCotizacionDetalles.Where(o => o.idPreCotizacion == PreQuotation.idPreCotizacion).ToList();

                        //cambiar estatus a precotizacion
                        PreQuotation.idEstatus = pPendienteOrdenar;
                        context.SaveChanges();

                        //enviar historial precotizacion
                        this.HistoryPreQuotation(PreQuotation.idPreCotizacion, pPendienteOrdenar);

                        //obtener id de la venta con idcotizacion
                        var Sale = context.tVentas.Where(o => o.idCotizacion == idCotizacion).FirstOrDefault();
                        if(Sale != null)
                        {
                            //obtener el detalle de la venta con idventa 
                            var SaleDetail = context.tDetalleVentas.Where(o => o.idVenta == Sale.idVenta).ToList();
                            if(SaleDetail != null && SaleDetail.Count > 0)
                            {

                                //obtener los ids servicios iterar los servicios junto con el detalle
                                foreach(var ventaServicio in SaleDetail)
                                {
                                    //cambiar estatus a detalle de precotizacion
                                    foreach (var preQuotationServicio in Detail)
                                    {
                                        if(ventaServicio.idServicio == preQuotationServicio.idServicio)
                                        {
                                            preQuotationServicio.idEstatusServicio = sPendienteOrdenar;
                                            context.SaveChanges();

                                            //enviar a historial servicio
                                            this.HistoryService(preQuotationServicio.idPreCotizacion, preQuotationServicio.idPreCotizacionDetalle, sPendienteOrdenar);
                                        }
                                    }
                                }
                            }
                        }


                        //generar notificacion con fecha actual 
                        string Notification = "PRECOTIZACION " + PreQuotation.Numero + " PENDIENTE DE ORDENAR";
                        var DateNotification = DateTime.Now.Date;
                        var DateNotificationAfter = DateTime.Now.Date.AddDays(7);
                        this.AddNotification(Notification, idUser, DateNotification);

                        //generar notificacion con fecha 7 dias despues de la fecha actual
                        Notification += " HACE 5 DIAS";
                        var idEvento = this.AddNotification(Notification, idUser, DateNotificationAfter);
                        PreQuotation.idEvento = idEvento;
                        context.SaveChanges();
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //pre cotizacion cambia a estatus 6 "ordenada, servicios cambian a estatus 7 "ordenado"
        public List<PreCotDetalleProveedoresViewModel> OrderedPreQuotation(int idPreQuotationDetail, int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var pOrdenada = context.tEstatusPrecotizacions.Where(o => o.Nombre == "ORDENADA").FirstOrDefault().idEstatus;
                    var sOrdenado = context.tEstatusServicios.Where(o => o.Nombre == "ORDENADO").FirstOrDefault().idEstatusServicio;
                    int sPendienteOrdenar = context.tEstatusServicios.Where(o => o.Nombre == "PENDIENTE DE ORDENAR").FirstOrDefault().idEstatusServicio;

                    var Detail = context.tPreCotizacionDetalles.Find(idPreQuotationDetail);
                    var PreQuotation = context.tPreCotizacions.Find(Detail.idPreCotizacion);

                    //cambiar estatus a detalle 
                    if(Detail.idEstatusServicio == sPendienteOrdenar)
                    {
                        Detail.idEstatusServicio = sOrdenado;
                        context.SaveChanges();
                        //generar notificacion
                        var DiasFabricacion = context.tPreCotDetalleProveedores
                            .Where(p => p.idPreCotizacionDetalle == Detail.idPreCotizacionDetalle && p.Asignado == 1)
                            .OrderByDescending(p => p.DiasFabricacion)
                            .FirstOrDefault().DiasFabricacion;

                        if (DiasFabricacion > 5)
                            DiasFabricacion += -5;
                        string Servicio = context.tServicios.Where(o => o.idServicio == Detail.idServicio).FirstOrDefault().Descripcion;
                        string Notification = "EN LOS PROXIMOS DIAS SERA ENTREGADO EL SERVICIO " + Servicio + " DE LA PRECOTIZACION " + PreQuotation.Numero;

                        if (Notification.Length > 100)
                            Notification = Notification.Substring(0, 100);

                        var DateNofitification = DateTime.Now.Date.AddDays((double)DiasFabricacion);

                        this.AddNotification(Notification, idUser, DateNofitification);
                        this.HistoryService(Detail.idPreCotizacion, Detail.idPreCotizacionDetalle, sOrdenado);
                    }
                    var ordered = ValidateOrdered(PreQuotation.idPreCotizacion);
                    //validar si todo el detalle esta ordenado cambiar el estatus a la precotizacion
                    if (!ordered)
                    {
                        PreQuotation.idEstatus = pOrdenada;
                        context.SaveChanges();

                        //enviar a historial
                        this.HistoryPreQuotation(PreQuotation.idPreCotizacion, pOrdenada);  
                    }
                    //cancelar notificacion
                    this.CancelNotification(PreQuotation.idEvento);


                    //retornar lista de proveedores asignados
                    var Providers = context.tPreCotDetalleProveedores.Where(o => o.idPreCotizacionDetalle == idPreQuotationDetail && o.Asignado == 1).ToList().OrderByDescending(o => o.idPreCotDetalleProveedores);
                    List<PreCotDetalleProveedoresViewModel> DetailProviders = new List<PreCotDetalleProveedoresViewModel>();

                    foreach(var item in Providers)
                    {
                        PreCotDetalleProveedoresViewModel DetailProvidersItem = new PreCotDetalleProveedoresViewModel();
                        DetailProvidersItem.idPreCotDetalleProveedores = item.idPreCotDetalleProveedores;
                        DetailProvidersItem.idPreCotizacionDetalle = item.idPreCotizacionDetalle;
                        DetailProvidersItem.idTipoServicio = item.idServicio;
                        DetailProvidersItem.idProveedor = item.idProveedor;
                        DetailProvidersItem.Proveedor = context.tProveedores.Find(item.idProveedor).Nombre;
                        DetailProvidersItem.CostoFabricacion = item.CostoFabricacion;
                        DetailProvidersItem.DiasFabricacion = item.DiasFabricacion;
                        DetailProvidersItem.ComentariosProveedor = item.ComentariosProveedor;
                        DetailProvidersItem.ComentariosComprador = item.ComentariosComprador;
                        DetailProvidersItem.Asignado = item.Asignado;
                        DetailProvidersItem.Enviado = item.Enviado;

                        DetailProviders.Add(DetailProvidersItem);
                    }

                    return DetailProviders;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void AddPreQuotationDetail(PreQuotationsDetailsViewModel detail)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {


                    tPreCotizacionDetalle oDetallePreCotizacion = new tPreCotizacionDetalle();

                    if (!String.IsNullOrEmpty(detail.Imagen))
                    {
                        char delimiter = '/';
                        var imagen = detail.Imagen.Split(delimiter);
                        oDetallePreCotizacion.Imagen = imagen[3];
                    }

                    oDetallePreCotizacion.idEstatusServicio = detail.idEstatus;
                    oDetallePreCotizacion.idPreCotizacion = detail.idPreCotizacion;
                    oDetallePreCotizacion.idServicio = detail.idServicio;
                    oDetallePreCotizacion.Descripcion = detail.Descripcion;
                    oDetallePreCotizacion.Comentarios = detail.Comentarios;
                    oDetallePreCotizacion.Cantidad = detail.Cantidad;
                    //oDetallePreCotizacion.Imagen = detail.Imagen;
                    oDetallePreCotizacion.PDF = detail.PDF;

                    context.tPreCotizacionDetalles.Add(oDetallePreCotizacion);
                    context.SaveChanges();

                    //enviar a historial servicio nuevo
                    this.HistoryService(oDetallePreCotizacion.idPreCotizacion, oDetallePreCotizacion.idPreCotizacionDetalle, oDetallePreCotizacion.idEstatusServicio);

                    if (detail.measures != null)
                    {
                        foreach (var medidas in detail.measures)
                        {
                            medidas.idPreCotizacionDetalle = oDetallePreCotizacion.idPreCotizacionDetalle;
                            this.AddPreQuoDetailMeasures(medidas);
                        }
                    }

                    if (detail.fabrics != null )
                    {
                        foreach (var telas in detail.fabrics)
                        {
                            telas.idPreCotizacionDetalle = oDetallePreCotizacion.idPreCotizacionDetalle;
                            this.AddPreQuoDetailFabrics(telas);
                        }
                    }


                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void AddPreQuoDetailMeasures(PreCotDetalleMedidasViewModel measures)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tPreCotDetalleMedida oDetalleMedida = new tPreCotDetalleMedida();

                    oDetalleMedida.idPreCotizacionDetalle = measures.idPreCotizacionDetalle;
                    oDetalleMedida.idServicio = measures.idServicio;
                    oDetalleMedida.idTipoMedida = measures.idTipoMedida;
                    oDetalleMedida.Valor = measures.Valor;

                    context.tPreCotDetalleMedidas.Add(oDetalleMedida);
                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

        }

        public void AddPreQuoDetailFabrics(PreCotDetalleTipoTelaViewModel fabrics)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tPreCotDetalleTipoTela oDetalleTela = new tPreCotDetalleTipoTela();

                    oDetalleTela.idPreCotizacionDetalle = fabrics.idPreCotizacionDetalle;
                    oDetalleTela.idServicio = fabrics.idServicio;
                    oDetalleTela.idTextiles = fabrics.idTextiles;
                    oDetalleTela.CostoPorMts = fabrics.CostoPorMts;
                    oDetalleTela.ValorMts = fabrics.ValorMts;
                    oDetalleTela.CostoTotal = fabrics.CostoTotal;

                    context.tPreCotDetalleTipoTelas.Add(oDetalleTela);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public PreQuotationsViewModel GetPreQuotationsId(int idPreQuotation)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var preQuotation = context.tPreCotizacions.Where(p => p.idPreCotizacion == idPreQuotation).Select(p => new PreQuotationsViewModel()
                    {
                        idPreCotizacion = p.idPreCotizacion,
                        Numero = p.Numero,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idClienteFisico = p.idClienteFisico,
                        ClienteFisico = (p.idClienteFisico != null) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : p.tClientesMorale.Nombre,
                        idClienteMoral = p.idClienteMoral,
                        idDespacho = p.idDespacho,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        Despacho = p.tDespacho.Nombre,
                        DespachoReferencia = p.tDespacho1.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Total = p.Total,
                        TipoCliente = p.TipoCliente,
                        Comentarios = p.Comentarios,
                        Estatus = p.idEstatus,
                        sEstatus = context.tEstatusPrecotizacions.Where(o => o.idEstatus == p.idEstatus).FirstOrDefault().Nombre,
                        oDetail = p.tPreCotizacionDetalles.Where(x => x.idPreCotizacion == p.idPreCotizacion).Select(x => new PreQuotationsDetailsViewModel()
                        {
                            idPreCotizacionDetalle = x.idPreCotizacionDetalle,
                            idPreCotizacion = x.idPreCotizacion,
                            idServicio = x.idServicio,
                            NombreTipoServicio = context.tTipoServicio.Where(o => o.idTipoServicio == x.idServicio).FirstOrDefault().Descripcion,
                            Descripcion = x.Descripcion,
                            Cantidad = x.Cantidad,
                            Imagen = x.Imagen != null ? "/content/services/" + x.Imagen : null,
                            PDF = x.PDF,
                            Comentarios = x.Comentarios,
                            idEstatus = x.idEstatusServicio,
                            NombreEstatus = context.tEstatusServicios.Where(e => e.idEstatusServicio == x.idEstatusServicio).FirstOrDefault().Nombre,
                            measures = x.tPreCotDetalleMedidas.Where(z => z.idPreCotizacionDetalle == x.idPreCotizacionDetalle).Select(z => new PreCotDetalleMedidasViewModel()
                            {
                                idPreCotDetalleMedidas = z.idPreCotDetalleMedidas,
                                idPreCotizacionDetalle = z.idPreCotizacionDetalle,
                                idServicio = z.idServicio,
                                idTipoMedida = z.idTipoMedida,
                                NombreTipoMedida = context.tTipoMedida.Where(w => w.idTipoMedida == z.idTipoMedida).FirstOrDefault().NombreMedida,
                                Valor = z.Valor
                            }).ToList(),
                            fabrics = x.tPreCotDetalleTipoTelas.Where(a => a.idPreCotizacionDetalle == x.idPreCotizacionDetalle).Select(a => new PreCotDetalleTipoTelaViewModel()
                            {
                                idPreCotDetalleTipoTela = a.idPreCotDetalleTipoTela,
                                idPreCotizacionDetalle = a.idPreCotizacionDetalle,
                                idServicio = a.idServicio,
                                idTextiles = a.idTextiles,
                                NombreTextiles = context.tTextiles.Where(t => t.idTextiles == a.idTextiles).FirstOrDefault().NombreTela,
                                CostoPorMts = a.CostoPorMts,
                                ValorMts = a.ValorMts,
                                CostoTotal = a.CostoTotal
                            }).ToList()
                        }).ToList(),
                    }).FirstOrDefault();

                    return preQuotation;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public PreQuotationsDetailsViewModel GetPreQuotationDetailById(int idDetail)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var oDeatil = context.tPreCotizacionDetalles.Where(p => p.idPreCotizacionDetalle == idDetail).Select(p => new PreQuotationsDetailsViewModel()
                    {
                        idPreCotizacionDetalle = p.idPreCotizacionDetalle,
                        idPreCotizacion = p.idPreCotizacionDetalle,
                        idServicio = p.idPreCotizacionDetalle,
                        idEstatus = p.idPreCotizacionDetalle,
                        NombreEstatus = "",
                        DescripcionEstatus = "",
                        NombreTipoServicio = context.tTipoServicio.Where(o => o.idTipoServicio == p.idServicio).FirstOrDefault().Descripcion,
                        Descripcion = p.Descripcion,
                        Comentarios = p.Comentarios,
                        Cantidad = p.Cantidad,
                        Imagen = p.Imagen != null ? "/content/services/" + p.Imagen : null,
                        measures = p.tPreCotDetalleMedidas.Where(z => z.idPreCotizacionDetalle == p.idPreCotizacionDetalle).Select(z => new PreCotDetalleMedidasViewModel()
                        {
                            idPreCotDetalleMedidas = z.idPreCotDetalleMedidas,
                            idPreCotizacionDetalle = z.idPreCotizacionDetalle,
                            idServicio = z.idServicio,
                            idTipoMedida = z.idTipoMedida,
                            NombreTipoMedida = context.tTipoMedida.Where(w => w.idTipoMedida == z.idTipoMedida).FirstOrDefault().NombreMedida,
                            Valor = z.Valor
                        }).ToList(),
                        fabrics = p.tPreCotDetalleTipoTelas.Where(a => a.idPreCotizacionDetalle == p.idPreCotizacionDetalle).Select(a => new PreCotDetalleTipoTelaViewModel()
                        {
                            idPreCotDetalleTipoTela = a.idPreCotDetalleTipoTela,
                            idPreCotizacionDetalle = a.idPreCotizacionDetalle,
                            idServicio = a.idServicio,
                            idTextiles = a.idTextiles,
                            NombreTextiles = context.tTextiles.Where(t => t.idTextiles == a.idTextiles).FirstOrDefault().NombreTela,
                            CostoPorMts = a.CostoPorMts,
                            ValorMts = a.ValorMts,
                            CostoTotal = a.CostoTotal
                        }).ToList()
                    }).FirstOrDefault();

                    return oDeatil;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public async void UpdatePreQuotation(PreQuotationsViewModel data)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var idPreQuotation = data.idPreCotizacion;
                    var PreQuotation = context.tPreCotizacions.Find(idPreQuotation);
                    var pEstatusNuevo = context.tEstatusPrecotizacions.Where(o => o.Nombre == "NUEVA").FirstOrDefault().idEstatus;

                    //actualizar datos de precotizacion (update)
                    PreQuotation.idUsuario1 = data.idUsuario1;
                    PreQuotation.idUsuario2 = data.idUsuario2;
                    PreQuotation.idClienteFisico = data.idClienteFisico;
                    PreQuotation.idClienteMoral = data.idClienteMoral;
                    PreQuotation.idDespachoReferencia = data.idDespachoReferencia;
                    PreQuotation.idSucursal = data.idSucursal;
                    PreQuotation.Fecha = data.Fecha;
                    PreQuotation.CantidadProductos = data.CantidadProductos;
                    PreQuotation.Total = data.Total;
                    PreQuotation.idDespacho = data.idDespacho;
                    PreQuotation.TipoCliente = data.TipoCliente;
                    PreQuotation.Proyecto = data.Proyecto;
                    PreQuotation.idEstatus = pEstatusNuevo;
                    PreQuotation.Comentarios = data.Comentarios;
                    PreQuotation.Numero = data.Numero;

                    context.SaveChanges();

                    //eliminar todo el detalle 
                    await this.DeletePreQuotationDetail(idPreQuotation);

                    int idEstatusServicioNew = context.tEstatusServicios.Where(o => o.Nombre == "NUEVO").FirstOrDefault().idEstatusServicio;


                    //insertar todo el detalle actualizado
                    foreach (var detail in data.oDetail)
                    {
                        detail.idEstatus = idEstatusServicioNew;
                        this.AddPreQuotationDetail(detail);
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public async void DeletePreQuotation(int id)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    await this.DeletePreQuotationDetail(id);
                    this.DeleteHistryPreQuotation(id);

                    var PreQuotation = context.tPreCotizacions.Find(id);
                    context.tPreCotizacions.Remove(PreQuotation);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public async Task DeletePreQuotationDetail(int idPreQuotation)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //lista de todo el detalle a editar
                    var oDeatil = context.tPreCotizacionDetalles.Where(p => p.idPreCotizacion == idPreQuotation).ToList();

                    //eliminar medidas y telas de cada detalle
                    if (oDeatil.Count > 0)
                    {
                        foreach (var item in oDeatil)
                        {
                            var medidas = context.tPreCotDetalleMedidas.Where(p => p.idPreCotizacionDetalle == item.idPreCotizacionDetalle).ToList();
                            var telas = context.tPreCotDetalleTipoTelas.Where(t => t.idPreCotizacionDetalle == item.idPreCotizacionDetalle).ToList();

                            context.tPreCotDetalleMedidas.RemoveRange(medidas);
                            context.tPreCotDetalleTipoTelas.RemoveRange(telas);

                            //eliminar historial
                            this.DeleteHistoryService(item.idPreCotizacionDetalle);

                            //eliminar el detalle
                            context.tPreCotizacionDetalles.Remove(item);
                        }
                        await context.SaveChangesAsync();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int ClonePreQuotation(int idPreQuotation)
        {
            int idReturn = 0;
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var pEstatusNuevo = context.tEstatusPrecotizacions.Where(o => o.Nombre == "NUEVA").FirstOrDefault().idEstatus;
                    int idEstatusServicioNew = context.tEstatusServicios.Where(o => o.Nombre == "NUEVO").FirstOrDefault().idEstatusServicio;

                    var Numero = this.GeneratePrevNumberRem();

                    tPreCotizacion ClonePreQuotation = new tPreCotizacion();
                    var PreQuotation = context.tPreCotizacions.Find(idPreQuotation);


                    ClonePreQuotation.Numero = Numero;
                    ClonePreQuotation.idUsuario1 = PreQuotation.idUsuario1;
                    ClonePreQuotation.idUsuario2 = PreQuotation.idUsuario2;
                    ClonePreQuotation.idClienteFisico = PreQuotation.idClienteFisico;
                    ClonePreQuotation.idClienteMoral = PreQuotation.idClienteMoral;
                    ClonePreQuotation.idDespacho = PreQuotation.idDespacho;
                    ClonePreQuotation.Proyecto = PreQuotation.Proyecto;
                    ClonePreQuotation.idDespachoReferencia = PreQuotation.idDespachoReferencia;
                    ClonePreQuotation.idSucursal = PreQuotation.idSucursal;
                    ClonePreQuotation.Fecha = PreQuotation.Fecha;
                    ClonePreQuotation.CantidadProductos = PreQuotation.CantidadProductos;
                    ClonePreQuotation.Total = PreQuotation.Total;
                    ClonePreQuotation.Comentarios = PreQuotation.Comentarios;
                    ClonePreQuotation.TipoCliente = PreQuotation.TipoCliente;
                    ClonePreQuotation.idEstatus = pEstatusNuevo;

                    context.tPreCotizacions.Add(ClonePreQuotation);
                    context.SaveChanges();

                    var idClonePreQuotation = ClonePreQuotation.idPreCotizacion;
                    this.HistoryPreQuotation(idClonePreQuotation, pEstatusNuevo);

                    var ClonePreQuotDetail = context.tPreCotizacionDetalles.Where(p => p.idPreCotizacion == idPreQuotation).ToList();

                    foreach (var detailItemClone in ClonePreQuotDetail)
                    {
                        tPreCotizacionDetalle NewDetallePreCotizacion = new tPreCotizacionDetalle();

                        NewDetallePreCotizacion.idPreCotizacion = idClonePreQuotation;
                        NewDetallePreCotizacion.idEstatusServicio = idEstatusServicioNew;
                        NewDetallePreCotizacion.idServicio = detailItemClone.idServicio;
                        NewDetallePreCotizacion.Descripcion = detailItemClone.Descripcion;
                        NewDetallePreCotizacion.Comentarios = detailItemClone.Comentarios;
                        NewDetallePreCotizacion.Cantidad = detailItemClone.Cantidad;
                        NewDetallePreCotizacion.Imagen = detailItemClone.Imagen;
                        NewDetallePreCotizacion.PDF = detailItemClone.PDF;

                        context.tPreCotizacionDetalles.Add(NewDetallePreCotizacion);
                        context.SaveChanges();
                        this.HistoryService(idClonePreQuotation, NewDetallePreCotizacion.idPreCotizacionDetalle, idEstatusServicioNew);

                        var idPreQuotDetail = NewDetallePreCotizacion.idPreCotizacionDetalle;
                        var medidas = context.tPreCotDetalleMedidas.Where(p => p.idPreCotizacionDetalle == detailItemClone.idPreCotizacionDetalle).ToList();
                        var telas = context.tPreCotDetalleTipoTelas.Where(t => t.idPreCotizacionDetalle == detailItemClone.idPreCotizacionDetalle).ToList();

                        foreach (var itemMedidas in medidas)
                        {
                            tPreCotDetalleMedida NewPreQuotMedidas = new tPreCotDetalleMedida();

                            NewPreQuotMedidas.idPreCotizacionDetalle = idPreQuotDetail;
                            NewPreQuotMedidas.idServicio = itemMedidas.idServicio;
                            NewPreQuotMedidas.idTipoMedida = itemMedidas.idTipoMedida;
                            NewPreQuotMedidas.Valor = itemMedidas.Valor;

                            context.tPreCotDetalleMedidas.Add(NewPreQuotMedidas);
                            context.SaveChanges();

                        }

                        foreach (var itemTelas in telas)
                        {
                            tPreCotDetalleTipoTela NewPreQuotTelas = new tPreCotDetalleTipoTela();

                            NewPreQuotTelas.idPreCotizacionDetalle = idPreQuotDetail;
                            NewPreQuotTelas.idServicio = itemTelas.idServicio;
                            NewPreQuotTelas.idTextiles = itemTelas.idTextiles;
                            NewPreQuotTelas.CostoPorMts = itemTelas.CostoPorMts;
                            NewPreQuotTelas.ValorMts = itemTelas.ValorMts;
                            NewPreQuotTelas.CostoTotal = itemTelas.CostoTotal;

                            context.tPreCotDetalleTipoTelas.Add(NewPreQuotTelas);
                            context.SaveChanges();
                        }
                    }
                    idReturn = idClonePreQuotation;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
            return idReturn;
        }

        public List<PreCotDetalleProveedoresViewModel> GetProviderService(int id)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var ProviderService = context.tPreCotDetalleProveedores.Where(p => p.idPreCotizacionDetalle == id).Select(p => new PreCotDetalleProveedoresViewModel
                    {
                        idPreCotDetalleProveedores = p.idPreCotDetalleProveedores,
                        idPreCotizacionDetalle = p.idPreCotizacionDetalle,
                        idTipoServicio = p.idServicio,
                        idProveedor = p.idProveedor,
                        TipoProveedor = context.tTipoServicio.Where(e => e.idTipoServicio == context.tProveedores.Where(r => r.idProveedor == p.idProveedor).FirstOrDefault().idTipoServicio).FirstOrDefault().Descripcion,
                        Proveedor = context.tProveedores.Where(r => r.idProveedor == p.idProveedor).FirstOrDefault().Nombre,
                        CostoFabricacion = p.CostoFabricacion,
                        DiasFabricacion = p.DiasFabricacion,
                        ComentariosComprador = p.ComentariosComprador,
                        ComentariosProveedor = p.ComentariosProveedor,
                        Asignado = p.Asignado,
                        Enviado = p.Enviado,
                        TelasProveedores = context.tPreCotProveedoresTelas.Where(o => o.idPreCotDetalleProveedores == p.idPreCotDetalleProveedores).Select(o => new PreCotProveedoresTelasViewModel()
                        {
                            idPreCotProveedoresTelas = o.idPreCotProveedoresTelas,
                            idPreCotDetalleProveedores = o.idPreCotDetalleProveedores,
                            idServicio = o.idServicio,
                            idProveedor = o.idProveedor,
                            idTextiles = o.idTextiles,
                            ValorMts = o.ValorMts,
                            CostoPorMts = context.tTextiles.Where(t => t.idTextiles == o.idTextiles).FirstOrDefault().Precio,

                        }).ToList()
                    }).ToList();

                    if(ProviderService != null && ProviderService.Count > 0)
                    {
                        foreach(var item in ProviderService)
                        {
                            var tipoServicio = context.tProveedores.Find(item.idProveedor).idTipoServicio;
                            var factorUtilidad = context.tTipoServicio.Where(o => o.idTipoServicio == tipoServicio).FirstOrDefault().FactorUtilidad;
                            var costoFabricacionPublico = item.CostoFabricacion * factorUtilidad;

                            decimal? costoTotalTelas = 0;

                            foreach(var telas in item.TelasProveedores)
                            {
                                var costoTela = context.tTextiles.Find(telas.idTextiles).Precio;
                                var totalTela = (telas.ValorMts != null ? telas.ValorMts : 0) * costoTela;
                                costoTotalTelas += totalTela;
                            }

                            item.CostoPublico = costoTotalTelas + costoFabricacionPublico;
                        }
                    }

                    return ProviderService;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void HistoryService(int idPreQuotation, int idPreQuotDetail, int? idEstatus)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tHistEstatusServicio LastEstatus = context.tHistEstatusServicios.Where(o => o.idPrecotizacionDetalle == idPreQuotDetail && o.FechaFinal == null).FirstOrDefault();
                    if (LastEstatus != null)
                        LastEstatus.FechaFinal = DateTime.Now;


                    tHistEstatusServicio HistService = new tHistEstatusServicio();

                    HistService.idPreCotizacion = idPreQuotation;
                    HistService.idPrecotizacionDetalle = idPreQuotDetail;
                    HistService.idEstatusServicio = idEstatus;
                    HistService.FechaInicio = DateTime.Now;
                    HistService.FechaFinal = null;

                    context.tHistEstatusServicios.Add(HistService);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void HistoryPreQuotation( int idPreQuotation, short idEstatus)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tHistEstatusPreCot LastEstatus = context.tHistEstatusPreCots.Where(o => o.idPrecotizacion == idPreQuotation && o.FechaFinal == null).FirstOrDefault();
                    if (LastEstatus != null)
                        LastEstatus.FechaFinal = DateTime.Now;


                    tHistEstatusPreCot HistPreCot = new tHistEstatusPreCot();

                    HistPreCot.idPrecotizacion = idPreQuotation;
                    HistPreCot.idEstatus = idEstatus;
                    HistPreCot.FechaInicio = DateTime.Now;
                    HistPreCot.FechaFinal = null;

                    context.tHistEstatusPreCots.Add(HistPreCot);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteHistoryService(int IdService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var deleteHistory = context.tHistEstatusServicios.Where(x => x.idPrecotizacionDetalle == IdService).ToList();
                    context.tHistEstatusServicios.RemoveRange(deleteHistory);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteHistryPreQuotation(int idPreQuotation)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var deleteHistory = context.tHistEstatusPreCots.Where(x => x.idPrecotizacion == idPreQuotation);
                    context.tHistEstatusPreCots.RemoveRange(deleteHistory);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int GetLastID()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var lastId = context.tPreCotizacions.Max(p => (int?)p.idPreCotizacion) ?? 0;

                    return lastId;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public string GeneratePrevNumberRem()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    string ID = (this.GetLastID() + 1).ToString();
                    char pad = '0';

                    return string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), "PCOT"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool ValidateAsign(int idPreQuotation)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    int sAsignado = context.tEstatusServicios.Where(o => o.Nombre == "ASIGNADO").FirstOrDefault().idEstatusServicio;
                    var Detail = context.tPreCotizacionDetalles.Where(p => p.idPreCotizacion == idPreQuotation).ToList();

                    foreach (var item in Detail)
                    {
                        if (item.idEstatusServicio != sAsignado)
                        {
                            //si regresa true uno de los servicios no esta asignado y NO se cambia estatus de precotizacion
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
        }

        public bool ValidateOrdered(int idPreQuotation)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var sOrdenado = context.tEstatusServicios.Where(o => o.Nombre == "ORDENADO").FirstOrDefault().idEstatusServicio;
                    var Detail = context.tPreCotizacionDetalles.Where(p => p.idPreCotizacion == idPreQuotation).ToList();

                    foreach (var item in Detail)
                    {
                        if (item.idEstatusServicio != sOrdenado)
                        {
                            //si regresa true uno de los servicios no esta ordenado y NO se cambia estatus de precotizacion
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
        }

        public string GetEmailProvider(int? idProvider)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProveedores.Where(o => o.idProveedor == idProvider).FirstOrDefault().Correo;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddNotification(string Notification, int idUser, DateTime DateNotification)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tEvento newEvent = new tEvento();

                    newEvent.Nombre = Notification;
                    newEvent.Fecha = DateNotification;
                    newEvent.HoraInicio = null;
                    newEvent.Lugar = "APP";
                    newEvent.idUsuario = idUser;
                    newEvent.Cancelado = null;
                    newEvent.HoraFin = null;
                    newEvent.TodoDia = true;
                    newEvent.Color = null;
                    newEvent.Fondo = null;
                    newEvent.Fuente = null;
                    newEvent.FechaRecordatorio = DateNotification;
                    newEvent.HoraRecordatorio = null;
                    newEvent.Tipo = null;

                    context.tEventos.Add(newEvent);
                    context.SaveChanges();

                    return newEvent.idEvento;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void CancelNotification(int? idEvento)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tEvento Evento = context.tEventos.Where(o => o.idEvento == idEvento).FirstOrDefault();
                    if(Evento != null)
                    {
                        Evento.Cancelado = true;
                        context.SaveChanges();
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public decimal? GetUnitCost(int idPreCotDetalleProveedores)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var CostoFabricacion = context.tPreCotDetalleProveedores.Find(idPreCotDetalleProveedores).CostoFabricacion;
                    var FabricsProvider = context.tPreCotProveedoresTelas.Where(f => f.idPreCotDetalleProveedores == idPreCotDetalleProveedores).ToList();

                    decimal? total = 0;
                    if(FabricsProvider != null && FabricsProvider.Count > 0)
                    {
                        foreach(var item in FabricsProvider)
                        {
                            var CostoXMts = context.tTextiles.Find(item.idTextiles).Precio;
                            if(CostoXMts != null && CostoXMts > 0)
                            {
                                total += item.ValorMts * CostoXMts;
                            }
                        }
                    }
                    return total + CostoFabricacion;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public decimal? GetWidthFabric(int? idTextil)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var AnchoTela = context.tTextiles.Find(idTextil).AnchoTela;

                    return AnchoTela;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

    }
}