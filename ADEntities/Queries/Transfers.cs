using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class Transfers : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTransferencias.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int? GetLastID()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tTransferencias.Count() > 0)
                    {
                        return context.tTransferencias.Max(p => p.idTransferencia);
                    }
                    else
                    {
                        return 0;
                    }
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

                    string ID = ((this.GetLastID() ?? 0) + 1).ToString();
                    char pad = '0';

                    return string.Concat("TR-" + month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public string GenerateNumberRem(int idBranch, int idTransfer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    char pad = '0';

                    return string.Concat("TR-" + month, string.Concat(string.Concat(year + "-", idTransfer.ToString().PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<TransferViewModel> Get(DateTime dtDateSince, DateTime dtDateUntil, string transfer, string code, int status, int? idUser, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    var transfers = context.tTransferencias.Where(

                      p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (p.Numero.Contains(transfer) || (String.IsNullOrEmpty(transfer))) &&
                      (p.idUsuario == idUser || idUser == null) &&
                      (((p.idSucursalOrigen == amazonas) ||
                      (p.idSucursalOrigen == guadalquivir) ||
                      (p.idSucursalOrigen == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      (p.tDetalleTransferencias.Any(x => (x.tProducto.Codigo.Contains(code) || (String.IsNullOrEmpty(code))))) &&
                      ((p.Estatus == status || status == 0))
                    ).Select(p => new TransferViewModel()
                    {
                        idTransferencia = p.idTransferencia,
                        Numero = p.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = context.tUsuarios.Where(u => u.idUsuario == p.idUsuario).Select(u => u.Nombre + " " + u.Apellidos).FirstOrDefault(),
                        idSucursalOrigen = p.idSucursalOrigen,
                        SucursalOrigen = p.tSucursale.Nombre,
                        idSucursalDestino = p.idSucursalDestino,
                        SucursalDestino = p.tSucursale1.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        ColorEstatus = (p.Estatus == TypesTransfers.CompletedTransfer) ? "grey" :
                                       (p.Estatus == TypesTransfers.PendingTransfer) ? "yellow" :
                                        "red",
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        Update = !context.tHistoricoTransferencias.Where(o => o.tDetalleTransferencia.idTransferencia == p.idTransferencia).Any(),
                        lDetail = p.tDetalleTransferencias.Select(o => new DetailTransferViewModel()
                        {
                            idDetalleTransferencia = o.idDetalleTransferencia,
                            idTransferencia = o.idTransferencia,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadEnviada = o.CantidadEnviada,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).OrderByDescending(p => p.Fecha);

                    List<TransferViewModel> lTransfers = transfers.Skip(page * pageSize).Take(pageSize).ToList();

                    foreach (var trans in lTransfers)
                    {
                        trans.sEstatus = trans.Estatus == TypesTransfers.CompletedTransfer ? TypesTransfers.TxtCompletedTransfer : trans.Estatus == TypesTransfers.PendingTransfer ? TypesTransfers.TxtPendingTransfer :
                                         TypesTransfers.TxtCanceledTransfer;
                    }

                    return lTransfers;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool GetByProduct(int idProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tTransferencias.Where(p=> p.Estatus == TypesTransfers.PendingTransfer 
                        && p.tDetalleTransferencias.Any(o=>o.idProducto == idProduct)).Count() > 0)
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

        public int Post(tTransferencia transfer, List<tDetalleTransferencia> lDetail)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    transfer.Estatus = TypesTransfers.PendingTransfer;
                    context.tTransferencias.Add(transfer);
                    context.SaveChanges();

                    foreach (var detail in lDetail)
                    {
                        detail.idTransferencia = transfer.idTransferencia;
                        context.tDetalleTransferencias.Add(detail);
                    }

                    var number = GenerateNumberRem((int)transfer.idSucursalOrigen, transfer.idTransferencia);
                    transfer.Numero = number;
                    context.SaveChanges();

                    return transfer.idTransferencia;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStock(int idTransfer, List<KeyValuePair<int, int>> lHistoryTransfer, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var transfer = context.tTransferencias.Where(p => p.idTransferencia == idTransfer).Select(p => new TransferViewModel()
                    {
                        idTransferencia = p.idTransferencia,
                        Numero = p.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = context.tUsuarios.Where(u => u.idUsuario == p.idUsuario).Select(u => u.Nombre + " " + u.Apellidos).FirstOrDefault(),
                        idSucursalOrigen = p.idSucursalOrigen,
                        SucursalOrigen = p.tSucursale.Nombre,
                        idSucursalDestino = p.idSucursalDestino,
                        SucursalDestino = p.tSucursale1.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleTransferencias.Select(o => new DetailTransferViewModel()
                        {
                            idDetalleTransferencia = o.idDetalleTransferencia,
                            idTransferencia = o.idTransferencia,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadEnviada = o.CantidadEnviada,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).FirstOrDefault();

                    foreach (var history in lHistoryTransfer)
                    {
                        foreach (var tdetail in transfer.lDetail)
                        {
                            if (history.Key == tdetail.idProducto)
                            {
                                var product = context.tProductosSucursals.Where(p =>
                                        p.idProducto == tdetail.idProducto && p.idSucursal == transfer.idSucursalOrigen)
                                    .FirstOrDefault();

                                this.AddRegisterProduct((int)tdetail.idProducto, (int)transfer.idSucursalOrigen, "Se actualiza el inventario por transferencia " + transfer.Numero, (decimal)product.Existencia, (decimal)(product.Existencia - history.Value), context.tProductos.FirstOrDefault(p => p.idProducto == product.idProducto).PrecioVenta, context.tProductos.FirstOrDefault(p => p.idProducto == product.idProducto).PrecioVenta, String.Empty, (int)idUser);

                                product.Existencia = product.Existencia - history.Value;

                                context.SaveChanges();
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

        public void Update(tTransferencia transfer, List<tDetalleTransferencia> lDetail)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var transferupdate = context.tTransferencias.Where(p => p.idTransferencia == transfer.idTransferencia).FirstOrDefault();

                    if (transferupdate != null)
                    {
                        transferupdate.idSucursalDestino = transfer.idSucursalDestino;
                        transferupdate.idUsuario = transfer.idUsuario;
                        transferupdate.CantidadProductos = transfer.CantidadProductos;
                        transferupdate.CostoTotal = transfer.CostoTotal;
                        transferupdate.Comentarios = transfer.Comentarios;
                        transferupdate.ModificadoPor = transfer.ModificadoPor;
                        transferupdate.Modificado = transfer.Modificado;

                        context.SaveChanges();
                    }

                    this.DeleteDetail(transfer.idTransferencia);

                    foreach (var detail in lDetail)
                    {
                        detail.idTransferencia = transfer.idTransferencia;
                        context.tDetalleTransferencias.Add(detail);
                    }

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteDetail(int idTransfer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var detail = context.tDetalleTransferencias.Where(p => p.idTransferencia == idTransfer);

                    if (detail != null)
                    {
                        foreach (var item in detail)
                        {
                            context.tDetalleTransferencias.Remove(item);
                        }
                    }

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateReceivedAmount(int idTransfer, List<KeyValuePair<int, int>> lHistory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var transfer = context.tTransferencias.Where(p => p.idTransferencia == idTransfer).FirstOrDefault();

                    if (transfer != null)
                    {
                        var tDetail = context.tDetalleTransferencias.Where(p => p.idTransferencia == idTransfer).ToList();

                        foreach (var history in lHistory)
                        {
                            foreach (var tproduct in tDetail)
                            {
                                if (history.Key == tproduct.idProducto)
                                {
                                    tproduct.CantidadEnviada = (tproduct.CantidadEnviada ?? 0) + history.Value;

                                    tHistoricoTransferencia hreception = new tHistoricoTransferencia();
                                    hreception.idDetalleTransferencia = tproduct.idDetalleTransferencia;
                                    hreception.Cantidad = history.Value;
                                    hreception.Creado = DateTime.Now;

                                    context.tHistoricoTransferencias.Add(hreception);
                                    context.SaveChanges();
                                }
                            }
                        }

                        if (!tDetail.Where(p => p.Cantidad != p.CantidadEnviada).Any())
                        {
                            this.UpdateStatus(idTransfer, TypesGeneric.TypesTransfers.CompletedTransfer);
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStatus(int idTransfer, int status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var transfer = context.tTransferencias.Where(p => p.idTransferencia == idTransfer).FirstOrDefault();
                    transfer.Estatus = status;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void UpdateStock(int idTransfer, List<tHistoricoTransferencia> lHistoryTransfer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var transfer = context.tTransferencias.Where(p => p.idTransferencia == idTransfer).Select(p => new TransferViewModel()
                    {
                        idTransferencia = p.idTransferencia,
                        Numero = p.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = context.tUsuarios.Where(u => u.idUsuario == p.idUsuario).Select(u => u.Nombre + " " + u.Apellidos).FirstOrDefault(),
                        idSucursalOrigen = p.idSucursalOrigen,
                        SucursalOrigen = p.tSucursale.Nombre,
                        idSucursalDestino = p.idSucursalDestino,
                        SucursalDestino = p.tSucursale1.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleTransferencias.Select(o => new DetailTransferViewModel()
                        {
                            idDetalleTransferencia = o.idDetalleTransferencia,
                            idTransferencia = o.idTransferencia,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadEnviada = o.CantidadEnviada,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).FirstOrDefault();

                    foreach (var history in lHistoryTransfer)
                    {
                        foreach (var tdetail in transfer.lDetail)
                        {
                            if (history.idDetalleTransferencia == tdetail.idDetalleTransferencia)
                            {
                                var product = context.tProductosSucursals.Where(p =>
                                        p.idProducto == tdetail.idProducto && p.idSucursal == transfer.idSucursalOrigen)
                                    .FirstOrDefault();

                                product.Existencia = product.Existencia - history.Cantidad;

                                context.SaveChanges();
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

        public TransferViewModel GetByNumber(string number)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTransferencias.Where(p => p.Numero == number).Select(p => new TransferViewModel()
                    {
                        idTransferencia = p.idTransferencia,
                        Numero = p.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = context.tUsuarios.Where(u => u.idUsuario == p.idUsuario).Select(u => u.Nombre + " " + u.Apellidos).FirstOrDefault(),
                        idSucursalOrigen = p.idSucursalOrigen,
                        SucursalOrigen = p.tSucursale.Nombre,
                        idSucursalDestino = p.idSucursalDestino,
                        SucursalDestino = p.tSucursale1.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleTransferencias.Select(o => new DetailTransferViewModel()
                        {
                            idDetalleTransferencia = o.idDetalleTransferencia,
                            idTransferencia = o.idTransferencia,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadEnviada = o.CantidadEnviada,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public TransferViewModel GetById(int idTransfer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTransferencias.Where(p => p.idTransferencia == idTransfer).Select(p => new TransferViewModel()
                    {
                        idTransferencia = p.idTransferencia,
                        Numero = p.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = context.tUsuarios.Where(u => u.idUsuario == p.idUsuario).Select(u => u.Nombre + " " + u.Apellidos).FirstOrDefault(),
                        idSucursalOrigen = p.idSucursalOrigen,
                        SucursalOrigen = p.tSucursale.Nombre,
                        idSucursalDestino = p.idSucursalDestino,
                        SucursalDestino = p.tSucursale1.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleTransferencias.Select(o => new DetailTransferViewModel()
                        {
                            idDetalleTransferencia = o.idDetalleTransferencia,
                            idTransferencia = o.idTransferencia,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadEnviada = o.CantidadEnviada,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateReceivedAmount(List<tHistoricoTransferencia> lHistory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var history in lHistory)
                    {
                        var detail = context.tDetalleTransferencias.Where(p => p.idTransferencia == history.idDetalleTransferencia).FirstOrDefault();
                        if (detail != null)
                        {
                            detail.CantidadEnviada = detail.CantidadEnviada ?? 0 + history.Cantidad;

                            tHistoricoTransferencia hreception = new tHistoricoTransferencia();
                            hreception.idDetalleTransferencia = detail.idDetalleTransferencia;
                            hreception.Cantidad = history.Cantidad;
                            hreception.Creado = DateTime.Now;

                            context.tHistoricoTransferencias.Add(hreception);
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