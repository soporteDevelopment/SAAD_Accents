using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Data.Common;
using System.Reflection;
using System.Data.Entity;
using System.Globalization;

namespace ADEntities.Queries
{
    public class Reports : Base
    {

        public int CountRegistersWithFilters(DateTime? dtDateSince, DateTime? dtDateUntil, int? seller, string remission, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Value.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Value.Date + new TimeSpan(23, 59, 59);

                    var sales = context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      (((p.idUsuario1 == seller) || (p.idUsuario2 == seller)) || (seller == null)) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      (p.Remision.Contains(remission) || String.IsNullOrEmpty(remission)) &&
                      p.Total > 0
                    ).Count();

                    return sales;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ReportOfSalesViewModel> GetSalesReport(DateTime? dtDateSince, DateTime? dtDateUntil, int? seller, string remission, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Value.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Value.Date + new TimeSpan(23, 59, 59);

                    #region Ventas
                    var sales = context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      ((p.idUsuario1 == seller) && (p.idUsuario2 == null)) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      (p.Remision.Contains(remission) || String.IsNullOrEmpty(remission)) &&
                      p.Remision.Length > 0 &&
                      p.Total > 0
                    )
                    .Select(p => new ReportOfSalesViewModel()
                    {
                        Compartida = false,
                        idVenta = p.idVenta,
                        Anio = p.Fecha.Value.Year,
                        Mes = p.Fecha.Value.Month,
                        Semana = SqlFunctions.DatePart("iso_week", p.Fecha),
                        Dia = SqlFunctions.DatePart("Month", p.Fecha) + "/" + SqlFunctions.DatePart("Day", p.Fecha) + "/" + SqlFunctions.DatePart("Year", p.Fecha),
                        Fecha = p.Fecha,
                        Hora = p.Fecha,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespacho = p.idDespacho,
                        Despacho = p.tDespacho.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Remision = p.Remision,
                        TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
                        Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        DespachoCliente = p.tDespacho.Nombre,
                        ImporteVenta = p.Subtotal,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                "red"),
                        IntDescuento = p.Descuento ?? 0,
                        Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
                        IVA = p.IVA,
                        ImportePagadoCliente = p.Total,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
                        {

                            idDetalleVenta = x.idDetalleVenta,
                            idVenta = x.idVenta,
                            idProducto = x.idProducto,
                            idServicio = x.idServicio,
                            idNotaCredito = x.idNotaCredito,
                            NotaCredito = x.NotaCredito,
                            Descripcion = x.Descripcion,
                            Precio = x.Precio,
                            Descuento = x.Descuento ?? 0,
                            Cantidad = x.Cantidad,
                            idPromocion = x.idPromocion,
                            CostoPromocion = x.CostoPromocion,
                            idTipoPromocion = x.idTipoPromocion,
                            idProductoPadre = x.idProductoPadre,
                            oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
                            {
                                idProducto = o.idProducto,
                                urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
                                idProveedor = o.idProveedor,
                                Extension = o.Extension
                            }).FirstOrDefault(),
                            Comentarios = x.Comentarios,
                            Imagen = x.Imagen,
                            Omitir = x.Omitir ?? false
                        }).ToList(),
                        listTypePayment = context.tFormaPagoes.Where(g => g.idVenta == p.idVenta).Select(g => new TypePaymentViewModel()
                        {
                            idTypePayment = g.idFormaPago,
                            typesPayment = (short)g.FormaPago,
                            sTypePayment = (g.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                        g.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
                        g.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                        g.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                        g.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                        g.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                        g.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                        g.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                        g.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                        "Nota de Crédito"
                        ),
                            typesCard = g.TipoTarjeta,
                            sTypesCard = (g.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
                            g.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
                            g.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
                            amount = (decimal)g.Cantidad,
                            maxPayment = (DateTime)g.FechaLimitePago,
                            Estatus = g.Estatus,
                            DatePayment = (DateTime)g.FechaPago,
                            bank = g.Banco.ToUpper(),
                            amountIVA = g.CantidadIVA,
                            IVA = g.IVA,
                            HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == p.idVenta && g.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
                            {
                                idFormaPago = (int)(q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago).FirstOrDefault()),
                                FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

                                    x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                    x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                    x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                    x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                    x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                    x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                    x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                    x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
                                    x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
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
                        }).ToList(),
                        Pagado = p.PagadoUno ?? false,
                        Omitir = p.Omitir ?? false
                    }).OrderBy(p => p.idVenta).ToList();

                    var totalSale = 0.0M;

                    totalSale = sales.Sum(p => p.listTypePayment.Sum(o => o.amount + (o.amountIVA ?? 0)));


                    //Se revisa si existen pagos con American Express para descontar el 10%
                    if (sales != null)
                    {
                        foreach (var sale in sales)
                        {
                            if (sale.listTypePayment.Where(g => g.typesCard == TypesCard.AmericanExpress).Any())
                            {
                                foreach (var typePayment in sale.listTypePayment)
                                {
                                    if (typePayment.typesCard == TypesCard.AmericanExpress)
                                    {
                                        var com = ((typePayment.amount / 110) * 100);
                                        sale.comAmericanExpress = ((sale.comAmericanExpress ?? 0) + (typePayment.amount - com));
                                        typePayment.amount = com;
                                    }
                                }
                            }
                        }

                    }

                    #endregion Ventas

                    #region Ventas Compartidas

                    //VentasCompartidas
                    var salesShared = context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      ((p.idUsuario2 == seller) || ((p.idUsuario1 == seller) && (p.idUsuario2 != null))) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      (p.Remision.Contains(remission) || String.IsNullOrEmpty(remission)) &&
                      p.Remision.Length > 0 &&
                      p.Total > 0
                    )
                    .Select(p => new ReportOfSalesViewModel()
                    {
                        Compartida = true,
                        idVenta = p.idVenta,
                        Anio = p.Fecha.Value.Year,
                        Mes = p.Fecha.Value.Month,
                        Semana = SqlFunctions.DatePart("iso_week", p.Fecha),
                        Dia = SqlFunctions.DatePart("Month", p.Fecha) + "/" + SqlFunctions.DatePart("Day", p.Fecha) + "/" + SqlFunctions.DatePart("Year", p.Fecha),
                        Fecha = p.Fecha,
                        Hora = p.Fecha,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespacho = p.idDespacho,
                        Despacho = p.tDespacho.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Remision = p.Remision,
                        TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
                        Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        DespachoCliente = p.tDespacho.Nombre,
                        ImporteVenta = p.Subtotal,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                "red"),
                        IntDescuento = p.Descuento ?? 0,
                        Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
                        IVA = p.IVA,
                        ImportePagadoCliente = p.Total,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
                        {

                            idDetalleVenta = x.idDetalleVenta,
                            idVenta = x.idVenta,
                            idProducto = x.idProducto,
                            idServicio = x.idServicio,
                            idNotaCredito = x.idNotaCredito,
                            NotaCredito = x.NotaCredito,
                            Descripcion = x.Descripcion,
                            Precio = x.Precio,
                            Descuento = x.Descuento ?? 0,
                            Cantidad = x.Cantidad,
                            idPromocion = x.idPromocion,
                            CostoPromocion = x.CostoPromocion,
                            idTipoPromocion = x.idTipoPromocion,
                            idProductoPadre = x.idProductoPadre,
                            oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
                            {
                                idProducto = o.idProducto,
                                urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
                                idProveedor = o.idProveedor,
                                Extension = o.Extension
                            }).FirstOrDefault(),
                            Comentarios = x.Comentarios,
                            Imagen = x.Imagen,
                            Omitir = x.Omitir ?? false
                        }).ToList(),
                        listTypePayment = context.tFormaPagoes.Where(g => g.idVenta == p.idVenta).Select(g => new TypePaymentViewModel()
                        {
                            idTypePayment = g.idFormaPago,
                            typesPayment = (short)g.FormaPago,
                            sTypePayment = (g.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                        g.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
                        g.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                        g.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                        g.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                        g.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                        g.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                        g.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                        g.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                        "Nota de Crédito"
                        ),
                            typesCard = g.TipoTarjeta,
                            sTypesCard = (g.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
                            g.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
                            g.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
                            amount = (decimal)g.Cantidad,
                            maxPayment = (DateTime)g.FechaLimitePago,
                            Estatus = g.Estatus,
                            DatePayment = (DateTime)g.FechaPago,
                            bank = g.Banco.ToUpper(),
                            amountIVA = g.CantidadIVA,
                            IVA = g.IVA,
                            HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == p.idVenta && g.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
                            {

                                FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

                                    x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                    x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                    x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                    x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                    x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                    x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                    x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                    x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
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
                        }).ToList(),
                        Pagado = (p.idUsuario1 == seller) ? p.PagadoUno ?? false : p.PagadoDos ?? false,
                        Omitir = p.Omitir ?? false
                    }).OrderBy(p => p.idVenta);

                    //Se revisa si existen pagos con American Express para descontar el 10%
                    if (salesShared != null)
                    {
                        foreach (var sale in salesShared)
                        {
                            if (sale.listTypePayment.Where(g => g.typesCard == TypesCard.AmericanExpress).Any())
                            {
                                foreach (var typePayment in sale.listTypePayment)
                                {
                                    if (typePayment.typesCard == TypesCard.AmericanExpress)
                                    {
                                        var com = ((typePayment.amount / 110) * 100);
                                        sale.comAmericanExpress = ((sale.comAmericanExpress ?? 0) + (typePayment.amount - com));
                                        typePayment.amount = com;
                                    }
                                }
                            }
                        }

                    }

                    #endregion Ventas Compartidas
                    if (salesShared != null)
                    {
                        foreach (var cant in salesShared.ToList())
                        {
                            sales.Add(cant);
                        }
                    }

                    //Se eliminan servicios de instalacion y flete
                    foreach(var sale in sales)
                    {
                        foreach(var product in sale.oDetail)
                        {
                            if(product.idServicio == 1 || product.idServicio == 11 
                                || product.idServicio == 25 || product.idServicio == 60 || product.idServicio == 61)
                            {
                                sale.ImportePagadoCliente = sale.ImportePagadoCliente - (product.Precio * product.Cantidad);
                                product.Omitir = true;
                            }
                        }
                    }

                    if((dtDateSince.Value.Month == 4 || dtDateSince.Value.Month == 5) && dtDateSince.Value.Year == 2020){
                        //Se eliminan productos con promocion
                        foreach (var sale in sales)
                        {
                            foreach (var product in sale.oDetail)
                            {
                                if (product.idProducto > 0)
                                {
                                    if (product.idProductoPadre > 0)
                                    {
                                        if (product.Descuento == 25 || product.Descuento == 50)
                                        {
                                            product.Omitir = true;
                                        }
                                    } else if (product.Descuento == 25 || product.Descuento == 50)
                                    {
                                        product.Omitir = true;
                                    }
                                }
                            }
                        }
                    }

                    return sales;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ReportOfSalesViewModel> GetSalesFoSpecialReport(DateTime? dtDateSince, DateTime? dtDateUntil, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Value.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Value.Date + new TimeSpan(23, 59, 59);

                    #region Ventas
                    var sales = context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (p.idUsuario1 != 8 && p.idUsuario1 != 16) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      p.Remision.Length > 0 &&
                      p.Total > 0
                    )
                    .Select(p => new ReportOfSalesViewModel()
                    {
                        Compartida = false,
                        idVenta = p.idVenta,
                        Anio = p.Fecha.Value.Year,
                        Mes = p.Fecha.Value.Month,
                        Semana = SqlFunctions.DatePart("iso_week", p.Fecha),
                        Dia = SqlFunctions.DatePart("Month", p.Fecha) + "/" + SqlFunctions.DatePart("Day", p.Fecha) + "/" + SqlFunctions.DatePart("Year", p.Fecha),
                        Fecha = p.Fecha,
                        Hora = p.Fecha,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespacho = p.idDespacho,
                        Despacho = p.tDespacho.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Remision = p.Remision,
                        TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
                        Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        DespachoCliente = p.tDespacho.Nombre,
                        ImporteVenta = p.Subtotal,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                "red"),
                        IntDescuento = p.Descuento ?? 0,
                        Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
                        IVA = p.IVA,
                        ImportePagadoCliente = p.Total,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
                        {
                            idDetalleVenta = x.idDetalleVenta,
                            idVenta = x.idVenta,
                            idProducto = x.idProducto,
                            idServicio = x.idServicio,
                            idNotaCredito = x.idNotaCredito,
                            NotaCredito = x.NotaCredito,
                            Descripcion = x.Descripcion,
                            Precio = x.Precio,
                            Descuento = x.Descuento ?? 0,
                            Cantidad = x.Cantidad,
                            oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
                            {
                                idProducto = o.idProducto,
                                urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
                                idProveedor = o.idProveedor,
                                Extension = o.Extension
                            }).FirstOrDefault(),
                            Comentarios = x.Comentarios,
                            Imagen = x.Imagen,
                            Omitir = x.Omitir ?? false
                        }).ToList(),
                        listTypePayment = context.tFormaPagoes.Where(g => g.idVenta == p.idVenta).Select(g => new TypePaymentViewModel()
                        {
                            idTypePayment = g.idFormaPago,
                            typesPayment = (short)g.FormaPago,
                            sTypePayment = (g.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                        g.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
                        g.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                        g.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                        g.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                        g.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                        g.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                        g.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                        g.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                        "Nota de Crédito"
                        ),
                            typesCard = g.TipoTarjeta,
                            sTypesCard = (g.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
                            g.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
                            g.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
                            amount = (decimal)g.Cantidad,
                            maxPayment = (DateTime)g.FechaLimitePago,
                            Estatus = g.Estatus,
                            DatePayment = (DateTime)g.FechaPago,
                            bank = g.Banco.ToUpper(),
                            amountIVA = g.CantidadIVA,
                            IVA = g.IVA,
                            HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == p.idVenta && g.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
                            {
                                idFormaPago = (int)(q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago).FirstOrDefault()),
                                FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

                                    x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                    x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                    x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                    x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                    x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                    x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                    x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                    x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
                                    x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
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
                        }).ToList(),
                        Pagado = true,
                        Omitir = false
                    }).OrderBy(p => p.idVenta).ToList();
                    #endregion Ventas

                    return sales;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public CommissionViewModel GetCommission(int idUser, decimal total)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tComisiones.Where(p => p.idUsuario == idUser && (p.LimiteInferior <= total && p.LimiteSuperior >= total)).Select(p => new CommissionViewModel()
                    {
                        idComision = p.idComision,
                        idUsuario = p.idUsuario,
                        LimiteInferior = p.LimiteInferior,
                        LimiteSuperior = p.LimiteSuperior,
                        SueldoMensual = p.SueldoMensual,
                        PorcentajeComision = p.PorcentajeComision,
                        SueldoComision = p.SueldoComision,
                        BonoUno = p.BonoUno,
                        BonoDos = p.BonoDos,
                        BonoTres = p.BonoTres
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ReportOfSalesViewModel> GetSalesReportCSV(int idCommissionSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    DataTable dtObject = new DataTable();
                    List<ReportOfSalesViewModel> subtotal = new List<ReportOfSalesViewModel>();

                    var sales = context.tVentas.Where(
                       p => p.idVentasComisiones == idCommissionSale
                    )
                    .Select(p => new ReportOfSalesViewModel()
                    {
                        idVenta = p.idVenta,
                        Fecha = p.Fecha,
                        Hora = p.Fecha,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
                        Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        Despacho1 = p.tDespacho1.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Remision = p.Remision,
                        ImporteVenta = p.Subtotal,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                "red"),
                        IntDescuento = p.Descuento ?? 0,
                        Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
                        IVA = p.IVA,
                        ImportePagadoCliente = p.Total
                    })
                    .OrderBy(p => p.idVenta).ToList();

                    return sales;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CommissionsSalesViewModel> GetCommissions(DateTime date, int idSeller)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReporteVentasComisiones.Where(p => p.Fecha.Month == date.Month && p.Fecha.Year == date.Year && p.idVendedor == idSeller && p.Concepto == TypesIntelligence.Comision).Select(p => new CommissionsSalesViewModel()
                    {
                        idVentasComisiones = p.idVentasComisiones,
                        idVendedor = p.idVendedor,
                        FormaPago = p.FormaPago,
                        sFormaPago = p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                p.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                                "",
                        NumCheque = p.NumCheque,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaPago = p.FechaPago,
                        sConcepto = "Comisión",
                        Detalle = p.Detalle
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CommissionsSalesViewModel> GetBono(DateTime date, int idSeller)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReporteVentasComisiones.Where(p => p.Fecha.Month == date.Month && p.Fecha.Year == date.Year && p.idVendedor == idSeller && p.Concepto == TypesIntelligence.Bono).Select(p => new CommissionsSalesViewModel()
                    {
                        idVentasComisiones = p.idVentasComisiones,
                        idVendedor = p.idVendedor,
                        FormaPago = p.FormaPago,
                        sFormaPago = p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                p.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                                "",
                        NumCheque = p.NumCheque,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaPago = p.FechaPago,
                        sConcepto = "Bono",
                        Detalle = p.Detalle
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddCommission(tReporteVentasComisione oCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tReporteVentasComisiones.Add(oCommission);

                    context.SaveChanges();

                    return oCommission.idVentasComisiones;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public tReporteVentasComisione GetCommission(int idCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReporteVentasComisiones.Where(p => p.idVentasComisiones == idCommission).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateCommision(tReporteVentasComisione oReporteVentasComisiones)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tReporteVentasComisione otReporteVentasComisiones = context.tReporteVentasComisiones.FirstOrDefault(p => p.idVentasComisiones == oReporteVentasComisiones.idVentasComisiones);

                    if (otReporteVentasComisiones != null)
                    {
                        otReporteVentasComisiones.FormaPago = oReporteVentasComisiones.FormaPago;
                        otReporteVentasComisiones.NumCheque = oReporteVentasComisiones.NumCheque;
                        otReporteVentasComisiones.Cantidad = oReporteVentasComisiones.Cantidad;
                        otReporteVentasComisiones.Fecha = oReporteVentasComisiones.Fecha;
                        oReporteVentasComisiones.Detalle = oReporteVentasComisiones.Detalle;
                        otReporteVentasComisiones.TotalNetoVendido = oReporteVentasComisiones.TotalNetoVendido;
                        otReporteVentasComisiones.PorcentajeComision = oReporteVentasComisiones.PorcentajeComision;

                        context.SaveChanges();
                    }
                    else
                    {
                        this.AddCommission(oReporteVentasComisiones);
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void SetOmit(int idSale, bool omit)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tVenta oSale = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);

                    oSale.Omitir = (omit) ? false : true;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void SetOmitItem(int idDetail, bool omit)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tDetalleVenta oDetail = context.tDetalleVentas.FirstOrDefault(p => p.idDetalleVenta == idDetail);

                    oDetail.Omitir = (omit) ? false : true;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void SaveComments(DateTime dtDate, int idseller, string comments, short typereport)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tActividadVendedore tActivity = new tActividadVendedore();

                    tActivity.Fecha = dtDate;
                    tActivity.idUsuario = idseller;
                    tActivity.Comentarios = comments;
                    tActivity.TipoReporte = typereport;
                    tActivity.Estatus = TypesIntelligence.Active;

                    context.tActividadVendedores.Add(tActivity);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ActivityViewModel> GetComments(DateTime dtDate, int idseller, short typereport)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tActividadVendedores.Where(p => (p.Fecha.Value.Month == dtDate.Month) && p.TipoReporte == typereport).Select(p => new ActivityViewModel()
                    {
                        idActividad = p.idActividad,
                        Fecha = p.Fecha,
                        Comentarios = p.Comentarios,
                        Estatus = p.Estatus,
                        TipoReporte = p.TipoReporte
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteComments(int idActivity)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var activity = context.tActividadVendedores.FirstOrDefault(p => p.idActividad == idActivity);

                    context.tActividadVendedores.Remove(activity);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //Comisiones Despachos
        public int CountGetSalesReportOffices(DateTime? dtDateSince, DateTime? dtDateUntil, int? office, string customer, string remission, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Value.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Value.Date + new TimeSpan(23, 59, 59);

                    return context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      ((p.idDespachoReferencia == office) &&
                      (p.PagadoOffice != true)) &&
                      (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(customer.ToUpper().Trim()) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tClientesMorale.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tDespacho.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim())))) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      (p.Remision.Contains(remission) || String.IsNullOrEmpty(remission)) &&
                      p.Remision.Length > 0 &&
                      p.Total > 0
                    )
                    .Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ReportOfSalesViewModel> GetSalesReportOffices(DateTime? dtDateSince, DateTime? dtDateUntil, int? office, string customer, string remission, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Value.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Value.Date + new TimeSpan(23, 59, 59);

                    #region Ventas
                    var sales = context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      ((p.idDespachoReferencia == office || office == null) &&
                      (p.PagadoOffice != true)) &&
                      (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(customer.ToUpper().Trim()) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tClientesMorale.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tDespacho.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim())))) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      (p.Remision.Contains(remission) || String.IsNullOrEmpty(remission)) &&
                      p.Remision.Length > 0 &&
                      p.Total > 0
                    )
                    .Select(p => new ReportOfSalesViewModel()
                    {
                        Compartida = false,
                        idVenta = p.idVenta,
                        Anio = p.Fecha.Value.Year,
                        Mes = p.Fecha.Value.Month,
                        Semana = SqlFunctions.DatePart("iso_week", p.Fecha),
                        Dia = SqlFunctions.DatePart("Month", p.Fecha) + "/" + SqlFunctions.DatePart("Day", p.Fecha) + "/" + SqlFunctions.DatePart("Year", p.Fecha),
                        Fecha = p.Fecha,
                        Hora = p.Fecha,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idDespacho = p.idDespacho,
                        Despacho = p.tDespacho.Nombre,
                        Despacho1 = p.tDespacho1.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Remision = p.Remision,
                        TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
                        Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                        ClienteMoral = p.tClientesMorale.Nombre,
                        DespachoCliente = p.tDespacho.Nombre,
                        ImporteVenta = p.Subtotal,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                "red"),
                        IntDescuento = p.Descuento ?? 0,
                        Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
                        IVA = p.IVA,
                        ImportePagadoCliente = p.Total,
                        Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        ComisionGeneral = 0,
                        ComisionArterios = 0,
                        ComisionTotal = 0,
                        oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
                        {
                            idDetalleVenta = x.idDetalleVenta,
                            idVenta = x.idVenta,
                            idProducto = x.idProducto,
                            Proveedor = x.tProducto.tProveedore.Nombre,
                            idServicio = x.idServicio,
                            idNotaCredito = x.idNotaCredito,
                            NotaCredito = x.NotaCredito,
                            Descripcion = x.Descripcion,
                            Precio = x.Precio,
                            Descuento = x.Descuento ?? 0,
                            Cantidad = x.Cantidad,
                            oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
                            {
                                idProducto = o.idProducto,
                                urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
                                idProveedor = o.idProveedor,
                                Extension = o.Extension
                            }).FirstOrDefault(),
                            Comentarios = x.Comentarios,
                            Imagen = x.Imagen,
                            Omitir = x.Omitir ?? false
                        }).ToList(),
                        listTypePayment = context.tFormaPagoes.Where(g => g.idVenta == p.idVenta).Select(g => new TypePaymentViewModel()
                        {
                            idTypePayment = g.idFormaPago,
                            typesPayment = (short)g.FormaPago,
                            sTypePayment = (g.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                        g.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
                        g.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                        g.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                        g.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                        g.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                        g.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                        g.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                        g.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                        "Nota de Crédito"
                        ),
                            typesCard = g.TipoTarjeta,
                            sTypesCard = (g.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
                            g.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
                            g.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
                            amount = (decimal)g.Cantidad,
                            maxPayment = (DateTime)g.FechaLimitePago,
                            Estatus = g.Estatus,
                            DatePayment = (DateTime)g.FechaPago,
                            bank = g.Banco.ToUpper(),
                            amountIVA = g.CantidadIVA,
                            IVA = g.IVA,
                            HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == p.idVenta && g.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
                            {
                                idFormaPago = (int)q.tFormaPagoCreditoes.FirstOrDefault(x => x.idHistorialCredito == q.idHistorialCredito).FormaPago,
                                FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

                                    x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                    x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                    x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                    x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                    x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                    x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                    x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                    x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
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
                        }).ToList(),
                        Pagado = p.PagadoOffice ?? false,
                        Omitir = p.Omitir ?? false
                    }).OrderBy(p => p.idVenta).ToList();

                    //Se revisa si existen pagos con American Express para descontar el 10%
                    if (sales != null)
                    {
                        foreach (var sale in sales)
                        {
                            if (sale.listTypePayment.Where(g => g.typesCard == TypesCard.AmericanExpress).Any())
                            {
                                foreach (var typePayment in sale.listTypePayment)
                                {

                                    if (typePayment.typesCard == TypesCard.AmericanExpress)
                                    {

                                        var com = ((typePayment.amount / 110) * 100);

                                        sale.comAmericanExpress = ((sale.comAmericanExpress ?? 0) + (typePayment.amount - com));

                                        typePayment.amount = com;

                                    }
                                }
                            }
                        }

                    }

                    //Se eliminan servicios de instalacion y flete
                    foreach (var sale in sales)
                    {
                        foreach (var product in sale.oDetail)
                        {
                            if (product.idServicio == 1 || product.idServicio == 11
                                || product.idServicio == 25 || product.idServicio == 60 || product.idServicio == 61)
                            {
                                sale.ImportePagadoCliente = sale.ImportePagadoCliente - (product.Precio * product.Cantidad);
                                product.Omitir = true;
                            }
                        }
                    }

                    #endregion Ventas    

                    return sales;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ReportOfSalesViewModel> GetSalesReportCSVOffices(int idCommissionSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    DataTable dtObject = new DataTable();
                    List<ReportOfSalesViewModel> subtotal = new List<ReportOfSalesViewModel>();

                    var sales = context.tVentas.Where(
                       p => p.idVentasComisionesDespacho == idCommissionSale
                    )
                    .Select(p => new ReportOfSalesViewModel()
                    {
                        idVenta = p.idVenta,
                        Fecha = p.Fecha,
                        Hora = p.Fecha,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
                        Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        Despacho1 = p.tDespacho1.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Remision = p.Remision,
                        ImporteVenta = p.Subtotal,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                "red"),
                        IntDescuento = p.Descuento ?? 0,
                        Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
                        IVA = p.IVA,
                        ImportePagadoCliente = p.Total
                    })
                    .OrderBy(p => p.idVenta).ToList();

                    return sales;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddCommissionOffice(tReporteVentasComisionesDespacho oCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tReporteVentasComisionesDespachos.Add(oCommission);

                    context.SaveChanges();

                    return oCommission.idVentasComisiones;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public tReporteVentasComisionesDespacho GetCommissionOffice(int idCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReporteVentasComisionesDespachos.Where(p => p.idVentasComisiones == idCommission).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CommissionsSalesOfficesViewModel> GetCommissionsForOffice(int idOffice)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReporteVentasComisionesDespachos.Where(p => p.Concepto == TypesIntelligence.Comision && p.idDespacho == idOffice).Select(p => new CommissionsSalesOfficesViewModel()
                    {
                        idVentasComisiones = p.idVentasComisiones,
                        idDespacho = p.idDespacho,
                        FormaPago = p.FormaPago,
                        sFormaPago = p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                p.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                                "",
                        NumCheque = p.NumCheque,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaPago = p.FechaPago,
                        sConcepto = "Comisión",
                        Descuento = p.Descuento,
                        Detalle = p.Detalle
                    }).OrderByDescending(p => p.FechaPago).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CommissionsSalesOfficesViewModel> GetCommissionsOffice(DateTime dtDateSince, DateTime dtDateUntil, int idOffice)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    return context.tReporteVentasComisionesDespachos.Where(p => p.FechaPago >= dtStart && p.FechaPago <= dtEnd && p.idDespacho == idOffice && p.Concepto == TypesIntelligence.Comision).Select(p => new CommissionsSalesOfficesViewModel()
                    {
                        idVentasComisiones = p.idVentasComisiones,
                        idDespacho = p.idDespacho,
                        FormaPago = p.FormaPago,
                        sFormaPago = p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                p.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                                "",
                        NumCheque = p.NumCheque,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaPago = p.FechaPago,
                        sConcepto = "Comisión",
                        Descuento = p.Descuento,
                        Detalle = p.Detalle,
                        Total = p.Total
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ReportOfSalesViewModel> GetSalesReportCSVOffice(int idCommissionSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    DataTable dtObject = new DataTable();
                    List<ReportOfSalesViewModel> subtotal = new List<ReportOfSalesViewModel>();

                    var sales = context.tVentas.Where(
                       p => p.idVentasComisionesDespacho == idCommissionSale
                    )
                    .Select(p => new ReportOfSalesViewModel()
                    {
                        idVenta = p.idVenta,
                        Fecha = p.Fecha,
                        Hora = p.Fecha,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
                        Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
                        Despacho1 = p.tDespacho1.Nombre,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Remision = p.Remision,
                        ImporteVenta = p.Subtotal,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                "red"),
                        IntDescuento = p.Descuento ?? 0,
                        Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
                        IVA = p.IVA,
                        ImportePagadoCliente = ((p.Total/116) * 100),
                        ComisionArterios = p.ComisionArteriors,
                        ComisionGeneral = p.ComisionGeneral,
                        ComisionTotal = p.ComisionTotal,
                        TotalNotaCredito = p.TotalNotaCredito,
                        Proyecto = p.Proyecto
                    })
                    .OrderBy(p => p.idVenta).ToList();

                    return sales;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //Comisiones Despacho Suma
        public decimal? GetCommissionOfficeSummary(DateTime? dtDateSince, DateTime? dtDateUntil, int? office, string customer, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Value.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Value.Date + new TimeSpan(23, 59, 59);

                    return context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      ((p.idDespachoReferencia == office || office == null)) &&
                      (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(customer.ToUpper().Trim()) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tClientesMorale.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tDespacho.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim())))) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      p.Remision.Length > 0 &&
                      p.Total > 0
                      && p.idVentasComisionesDespacho > 0
                    )
                    .Sum(p => p.tReporteVentasComisionesDespacho.Total);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //Comisiones Vendedores Suma
        public decimal? GetCommissionSellerSummary(DateTime? dtDateSince, DateTime? dtDateUntil, int? office, string customer, short? amazonas, short? guadalquivir, short? textura)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Value.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Value.Date + new TimeSpan(23, 59, 59);

                    return context.tVentas.Where(
                       p =>
                      (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (((p.idSucursal == amazonas) || (p.idSucursal == guadalquivir) || (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      ((p.idDespachoReferencia == office || office == null)) &&
                      (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(customer.ToUpper().Trim()) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tClientesMorale.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim()))) ||
                      (p.tDespacho.Nombre.ToUpper().Contains(customer) || (String.IsNullOrEmpty(customer.ToUpper().Trim())))) &&
                      ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
                      p.Remision.Length > 0 &&
                      p.Total > 0
                      && p.idVentasComisionesDespacho > 0
                    )
                    .Sum(p => p.tReporteVentasComisione.Cantidad);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        //Comisiones Especiales
        public int AddSpecialCommission(tReporteComisionesEspeciale oCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tReporteComisionesEspeciales.Add(oCommission);

                    context.SaveChanges();

                    return oCommission.idVentasComisiones;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public tReporteComisionesEspeciale GetSpecialCommission(int idCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReporteComisionesEspeciales.Where(p => p.idVentasComisiones == idCommission).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateSpecialCommision(tReporteComisionesEspeciale oReporteVentasComisiones)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tReporteComisionesEspeciale otReporteVentasComisiones = context.tReporteComisionesEspeciales.FirstOrDefault(p => p.idVentasComisiones == oReporteVentasComisiones.idVentasComisiones);

                    if (otReporteVentasComisiones != null)
                    {
                        otReporteVentasComisiones.FormaPago = oReporteVentasComisiones.FormaPago;
                        otReporteVentasComisiones.NumCheque = oReporteVentasComisiones.NumCheque;
                        otReporteVentasComisiones.Cantidad = oReporteVentasComisiones.Cantidad;
                        otReporteVentasComisiones.Fecha = oReporteVentasComisiones.Fecha;
                        oReporteVentasComisiones.Detalle = oReporteVentasComisiones.Detalle;
                        otReporteVentasComisiones.TotalNetoVendido = oReporteVentasComisiones.TotalNetoVendido;
                        otReporteVentasComisiones.PorcentajeComision = oReporteVentasComisiones.PorcentajeComision;

                        context.SaveChanges();
                    }
                    else
                    {
                        this.AddSpecialCommission(oReporteVentasComisiones);
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CommissionsSalesViewModel> GetSpecialCommissions(DateTime date, int idSeller)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReporteComisionesEspeciales.Where(p => p.Fecha.Value.Month == date.Month && p.Fecha.Value.Year == date.Year && p.idVendedor == idSeller && p.Concepto == TypesIntelligence.Comision).Select(p => new CommissionsSalesViewModel()
                    {
                        idVentasComisiones = p.idVentasComisiones,
                        idVendedor = p.idVendedor,
                        FormaPago = (int)p.FormaPago,
                        sFormaPago = p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
                                p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
                                p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
                                p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
                                p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
                                p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
                                p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
                                p.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
                                "",
                        NumCheque = p.NumCheque,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha ?? DateTime.Now,
                        FechaPago = p.FechaPago ?? DateTime.Now,
                        sConcepto = "Comisión",
                        Detalle = p.Detalle
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