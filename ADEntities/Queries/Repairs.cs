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
    public class Repairs : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReparaciones.Count();
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
                    if (context.tReparaciones.Count() > 0)
                    {
                        return context.tReparaciones.Max(p => p.idReparacion);
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

        public string GeneratePrevNumber()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    string ID = ((this.GetLastID() ?? 0) + 1).ToString();
                    char pad = '0';

                    return string.Concat("RE-" + month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), "REP"));
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

        public List<RepairViewModel> Get(DateTime dtDateSince, DateTime dtDateUntil, string number, string code, int status, int? idUser, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    var repairs = context.tReparaciones.Where(

                      p => (p.FechaSalida >= dtStart && p.FechaSalida <= dtEnd) &&
                      (p.Numero.Contains(number) || (String.IsNullOrEmpty(number))) &&
                      (p.idUsuario == idUser || idUser == null) &&
                      (p.tDetalleReparaciones.Where(x => (x.tProducto.Codigo.Contains(code) || (String.IsNullOrEmpty(code)))).Any()) &&
                      (p.Estatus == status)
                    ).Select(p => new RepairViewModel()
                    {
                        idReparacion = p.idReparacion,
                        Numero = p.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        FechaSalida = p.FechaSalida,
                        Destino = p.Destino,
                        Responsable = p.Responsable,
                        FechaRegreso = p.FechaRegreso,
                        Estatus = p.Estatus,
                        ColorEstatus = (p.Estatus == TypesReceptions.CompletedReception) ? "grey" :
                                       (p.Estatus == TypesReceptions.PendingReception) ? "yellow" :
                                        "red",
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetalle = p.tDetalleReparaciones.Select(o => new DetailRepairViewModel()
                        {
                            idDetalleReparacion = o.idDetalleReparacion,
                            idProducto = o.idProducto,
                            Producto = new ProductViewModel()
                            {
                                idProducto = o.tProducto.idProducto,
                                Codigo = o.tProducto.Codigo,
                                Extension = o.tProducto.Extension,
                                urlImagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            },
                            idSucursal = o.idSucursal,
                            Cantidad = o.Cantidad,
                            Pendiente = o.Pendiente,
                            Comentarios = o.Comentarios
                        }).ToList()
                    }).OrderByDescending(p => p.FechaSalida);

                    List<RepairViewModel> lRepairs = repairs.Skip(page * pageSize).Take(pageSize).ToList();

                    foreach (var repair in lRepairs)
                    {
                        repair.sEstatus = repair.Estatus == TypesReceptions.CompletedReception ? TypesReceptions.TxtCompletedReception : repair.Estatus == TypesReceptions.PendingReception ? TypesReceptions.TxtPendingReception :
                                         TypesReceptions.TxtCanceledReception;
                    }

                    return lRepairs;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int Post(tReparacione repair)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tReparaciones.Add(repair);
                    context.SaveChanges();

                    foreach (var detail in repair.tDetalleReparaciones)
                    {
                        this.DeleteStock(detail.idProducto, detail.idSucursal, detail.Cantidad ?? 0, repair.idUsuario);
                    }

                    return repair.idReparacion;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int Patch(tReparacione repair)
        {
            using (var context = new admDB_SAADDBEntities())
                {
                    var entity = context.tReparaciones.FirstOrDefault(p=>p.idReparacion == repair.idReparacion);

                    if (entity == null)
                    {
                        throw new Exception("El registro no existe");
                    }

                    entity.FechaSalida = repair.FechaSalida;
                    entity.FechaRegreso = repair.FechaRegreso;
                    entity.Destino = repair.Destino;
                    entity.Responsable = repair.Responsable;
                    entity.Comentarios = repair.Comentarios;

                    return context.SaveChanges();
                }
        }

        public void UpdateReceivedAmount(int idRepair, List<tHistoricoReparacione> lhistory, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var history in lhistory)
                    {
                        context.tHistoricoReparaciones.Add(history);

                        var detail = context.tDetalleReparaciones.Where(p => p.idDetalleReparacion == history.idDetalleReparacion)
                            .FirstOrDefault();

                        detail.Pendiente = (detail.Pendiente ?? 0) - history.Cantidad;

                        context.SaveChanges();

                        this.UpdateStock(idRepair, detail.idProducto, history.idSucursal, history.Cantidad ?? 0, idUser);
                    }

                    if (!context.tDetalleReparaciones.Where(p => p.idReparacion == idRepair && p.Pendiente > 0).Any())
                    {
                        this.UpdateStatus(idRepair, TypesRepairs.CompletedRepair);
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStatus(int idRepair, int status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var repair = context.tReparaciones.Where(p => p.idReparacion == idRepair).FirstOrDefault();
                    repair.Estatus = status;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateStock(int idRepair, int idProduct, int idBranch, decimal amount, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var stock = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == idBranch)
                        .FirstOrDefault();

                    this.AddRegisterProduct(idProduct, idBranch, "Se actualiza el inventario por ingreso de reparación" + this.GetById(idRepair).Numero, (stock.Existencia ?? 0), ((stock.Existencia ?? 0) + amount), context.tProductos.FirstOrDefault(p => p.idProducto == idProduct).PrecioVenta, context.tProductos.FirstOrDefault(p => p.idProducto == idProduct).PrecioVenta, String.Empty, (int)idUser);

                    stock.Existencia = (stock.Existencia ?? 0) + amount;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteStock(int idProduct, int idBranch, decimal amount, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var stock = context.tProductosSucursals.Where(p => p.idProducto == idProduct && p.idSucursal == idBranch)
                        .FirstOrDefault();

                    this.AddRegisterProduct(idProduct, idBranch, "Se actualiza el inventario por salida a reparación", (stock.Existencia ?? 0), ((stock.Existencia ?? 0) - amount), context.tProductos.FirstOrDefault(p => p.idProducto == idProduct).PrecioVenta, context.tProductos.FirstOrDefault(p => p.idProducto == idProduct).PrecioVenta, String.Empty, (int)idUser);

                    stock.Existencia = (stock.Existencia ?? 0) - amount;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public RepairViewModel GetById(int idRepair)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tReparaciones.Where(p => p.idReparacion == idRepair).Select(p => new RepairViewModel()
                    {
                        idReparacion = p.idReparacion,
                        Numero = p.Numero,
                        idUsuario = p.idUsuario,
                        Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        FechaSalida = p.FechaSalida,
                        Destino = p.Destino,
                        Responsable = p.Responsable,
                        FechaRegreso = p.FechaRegreso,
                        Estatus = p.Estatus,
                        ColorEstatus = (p.Estatus == TypesReceptions.CompletedReception) ? "grey" :
                            (p.Estatus == TypesReceptions.PendingReception) ? "yellow" :
                            "red",
                        Comentarios = p.Comentarios,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        lDetalle = p.tDetalleReparaciones.Select(o => new DetailRepairViewModel()
                        {
                            idDetalleReparacion = o.idDetalleReparacion,
                            idProducto = o.idProducto,
                            Producto = new ProductViewModel()
                            {
                                idProducto = o.tProducto.idProducto,
                                Codigo = o.tProducto.Codigo,
                                Extension = o.tProducto.Extension,
                                urlImagen = o.tProducto.TipoImagen == 1 ? "/Content/Products/" + o.tProducto.NombreImagen + o.tProducto.Extension : o.tProducto.urlImagen,
                            },
                            idSucursal = o.idSucursal,
                            Cantidad = o.Cantidad,
                            Pendiente = o.Pendiente,
                            Comentarios = o.Comentarios
                        }).ToList()
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}