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
    public class Receptions : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tRecepciones.Count();
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
                    if (context.tRecepciones.Count() > 0)
                    {
                        return context.tRecepciones.Max(p => p.idRecepcion);
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

                    return string.Concat("RE-" + month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public string GenerateNumberRem(int idBranch, int idReception)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    char pad = '0';

                    return string.Concat("RE-" + month, string.Concat(string.Concat(year + "-", idReception.ToString().PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ReceptionViewModel> Get(DateTime dtDateSince, DateTime dtDateUntil, string reception, string transfer, string code, int status, int? idUser, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    var receptions = context.tRecepciones.Where(
                      p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (p.Numero.Contains(reception) || (String.IsNullOrEmpty(reception))) &&
                      (p.tTransferencia.Numero.Contains(transfer) || (String.IsNullOrEmpty(transfer))) &&
                      (p.idUsuario == idUser || idUser == null) &&
                      (((p.idSucursal == amazonas) ||
                      (p.idSucursal == guadalquivir) ||
                      (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
                      (p.tDetalleRecepcions.Any(x => (x.tProducto.Codigo.Contains(code) || (String.IsNullOrEmpty(code))))) &&
                      ((p.Estatus == status || status == 0))
                    ).Select(p => new ReceptionViewModel()
                    {
                        idRecepcion = p.idRecepcion,
                        Numero = p.Numero,
                        idTransferencia = p.idTransferencia,
                        Transferencia = p.tTransferencia.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        ColorEstatus = (p.Estatus == TypesReceptions.CompletedReception) ? "grey" :
                                       (p.Estatus == TypesReceptions.PendingReception) ? "yellow" :
                                        "red",
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleRecepcions.Select(o => new DetailReceptionViewModel()
                        {
                            idDetalleRecepcion = o.idDetalleRecepcion,
                            idRecepcion = o.idRecepcion,
                            idProducto = o.idProducto,
                            Cantidad = o.Cantidad,
                            CantidadRecibida = o.CantidadRecibida,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).OrderByDescending(p => p.Fecha);

                    List<ReceptionViewModel> lReceptions = receptions.Skip(page * pageSize).Take(pageSize).ToList();

                    foreach (var recept in lReceptions)
                    {
                        recept.sEstatus = recept.Estatus == TypesReceptions.CompletedReception ? TypesReceptions.TxtCompletedReception : recept.Estatus == TypesReceptions.PendingReception ? TypesReceptions.TxtPendingReception :
                                         TypesReceptions.TxtCanceledReception;
                    }

                    return lReceptions;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void Update(tRecepcione reception, List<tDetalleRecepcion> lDetail)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var oReception = context.tRecepciones.Where(p => p.idRecepcion == reception.idRecepcion)
                        .FirstOrDefault();

                    oReception.CantidadProductos = (oReception.CantidadProductos ?? 0) + reception.CantidadProductos;

                    if (lDetail.Where(p => p.Cantidad != p.CantidadRecibida).Any())
                    {
                        reception.Estatus = TypesReceptions.PendingReception;
                    }
                    else
                    {
                        reception.Estatus = TypesReceptions.CompletedReception;
                    }

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                   
                }
        }

        public int Post(tRecepcione reception, List<tDetalleRecepcion> lDetail)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    if (lDetail.Where(p => p.Cantidad != p.CantidadRecibida).Any())
                    {
                        reception.Estatus = TypesReceptions.PendingReception;
                    }
                    else
                    {
                        reception.Estatus = TypesReceptions.CompletedReception;
                    }
                    reception.Fecha = DateTime.Now;
                    context.tRecepciones.Add(reception);
                    context.SaveChanges();

                    foreach (var detail in lDetail)
                    {
                        detail.idRecepcion = reception.idRecepcion;

                        context.tDetalleRecepcions.Add(detail);
                    }

                    context.SaveChanges();

                    return reception.idRecepcion;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void UpdateReceivedAmount(int idReception, List<KeyValuePair<int, int>> lHistory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var reception = context.tRecepciones.Where(p => p.idRecepcion == idReception).FirstOrDefault();

                    if (reception != null)
                    {
                        var tDetail = context.tDetalleRecepcions.Where(p => p.idRecepcion == idReception).ToList();

                        foreach (var history in lHistory)
                        {
                            foreach (var tproduct in tDetail)
                            {
                                if (history.Key == tproduct.idProducto)
                                {
                                    tHistoricoRecepcion hreception = new tHistoricoRecepcion();
                                    hreception.idDetalleRecepcion = tproduct.idDetalleRecepcion;
                                    hreception.Cantidad = history.Value;
                                    hreception.Creado = DateTime.Now;

                                    context.tHistoricoRecepcions.Add(hreception);
                                    context.SaveChanges();
                                }
                            }
                        }

                        if (!tDetail.Where(p => p.Cantidad != p.CantidadRecibida).Any())
                        {
                            this.UpdateStatus(idReception, TypesGeneric.TypesTransfers.CompletedTransfer);
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void UpdateStatus(int idReception, int status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var reception = context.tRecepciones.Where(p => p.idRecepcion == idReception).FirstOrDefault();
                    reception.Estatus = status;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void UpdateStock(int idReception, List<KeyValuePair<int, int>> lHistoryReception, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var reception = context.tRecepciones.Where(p => p.idRecepcion == idReception).Select(p => new ReceptionViewModel()
                    {
                        idRecepcion = p.idRecepcion,
                        Numero = p.Numero,
                        idTransferencia = p.idTransferencia,
                        Transferencia = p.tTransferencia.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleRecepcions.Select(o => new DetailReceptionViewModel()
                        {
                            idDetalleRecepcion = o.idDetalleRecepcion,
                            idRecepcion = o.idRecepcion,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadRecibida = o.CantidadRecibida,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).FirstOrDefault();

                    foreach (var history in lHistoryReception)
                    {
                        foreach (var rdetail in reception.lDetail)
                        {
                            if (history.Key == rdetail.idProducto)
                            {
                                var product = context.tProductosSucursals.Where(p =>
                                        p.idProducto == rdetail.idProducto && p.idSucursal == reception.idSucursal)
                                    .FirstOrDefault();

                                this.AddRegisterProduct((int)rdetail.idProducto, (int)reception.idSucursal, "Se actualiza el inventario por recepción " + reception.Numero, (decimal)product.Existencia, (decimal)(product.Existencia + history.Value), context.tProductos.FirstOrDefault(p=>p.idProducto == product.idProducto).PrecioVenta, context.tProductos.FirstOrDefault(p => p.idProducto == product.idProducto).PrecioVenta, String.Empty, (int)idUser);

                                product.Existencia = product.Existencia + history.Value;

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

        public void UpdateStock(int idReception, List<tHistoricoRecepcion> lHistoryReception)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var reception = context.tRecepciones.Where(p => p.idRecepcion == idReception).Select(p => new ReceptionViewModel()
                    {
                        idRecepcion = p.idRecepcion,
                        Numero = p.Numero,
                        idTransferencia = p.idTransferencia,
                        Transferencia = p.tTransferencia.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleRecepcions.Select(o => new DetailReceptionViewModel()
                        {
                            idDetalleRecepcion = o.idDetalleRecepcion,
                            idRecepcion = o.idRecepcion,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadRecibida = o.CantidadRecibida,
                            Costo = o.Costo,
                            Estatus = o.Estatus,
                            Comentarios = o.Comentarios,
                            CreadoPor = o.CreadoPor,
                            Creado = o.Creado,
                            ModificadoPor = o.ModificadoPor,
                            Modificado = o.Modificado
                        }).ToList()
                    }).FirstOrDefault();

                    foreach (var history in lHistoryReception)
                    {
                        foreach (var rdetail in reception.lDetail)
                        {
                            if (history.idDetalleRecepcion == rdetail.idDetalleRecepcion)
                            {
                                var product = context.tProductosSucursals.Where(p =>
                                        p.idProducto == rdetail.idProducto && p.idSucursal == reception.idSucursal)
                                    .FirstOrDefault();

                                product.Existencia = product.Existencia + history.Cantidad;

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

        public ReceptionViewModel GetById(int idReception)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tRecepciones.Where(p => p.idRecepcion == idReception).Select(p => new ReceptionViewModel()
                    {
                        idRecepcion = p.idRecepcion,
                        Numero = p.Numero,
                        idTransferencia = p.idTransferencia,
                        Transferencia = p.tTransferencia.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        CostoTotal = p.CostoTotal,
                        Estatus = p.Estatus,
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetail = p.tDetalleRecepcions.Select(o => new DetailReceptionViewModel()
                        {
                            idDetalleRecepcion = o.idDetalleRecepcion,
                            idRecepcion = o.idRecepcion,
                            idProducto = o.idProducto,
                            Codigo = o.tProducto.Codigo,
                            Descripcion = o.tProducto.Descripcion,
                            Imagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            Cantidad = o.Cantidad,
                            CantidadRecibida = o.CantidadRecibida,
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

        public void UpdateReceivedAmount(List<tHistoricoRecepcion> lHistory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var history in lHistory)
                    {
                        var detail = context.tDetalleRecepcions
                            .Where(p => p.idDetalleRecepcion == history.idDetalleRecepcion).FirstOrDefault();

                        if (detail != null)
                        {
                            //Se actualiza el detalle
                            detail.CantidadRecibida = (detail.CantidadRecibida ?? 0) + history.Cantidad;

                            //Se registra movimiento
                            tHistoricoRecepcion hreception = new tHistoricoRecepcion();
                            hreception.idDetalleRecepcion = detail.idDetalleRecepcion;
                            hreception.Cantidad = history.Cantidad;
                            hreception.Creado = DateTime.Now;

                            context.tHistoricoRecepcions.Add(hreception);
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

        public bool GetReceptionByTransfer(string transfer)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tRecepciones.Where(p => p.tTransferencia.Numero == transfer).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }
    }
}