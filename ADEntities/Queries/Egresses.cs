using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADEntities.ViewModels;
using ADEntities.Models;

namespace ADEntities.Queries
{
    public class Egresses
    {
        //Get
        public List<EgressViewModel> Get(DateTime start, DateTime end, string receivedBy)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var dtStart = start.Date + new TimeSpan(0, 0, 0);
                var dtEnd = end.Date + new TimeSpan(23, 59, 59);

                return context.tSalidas.Where(p => (p.Fecha >= dtStart && p.Fecha <= dtEnd)
                                                  && p.fkSalida == null
                                                  && (p.tUsuario.Nombre.Contains(receivedBy) || p.RecibidaOtro.Contains(receivedBy)
                                                  || String.IsNullOrEmpty(receivedBy))
                                                  && p.Estatus == 1)
                    .Select(p => new EgressViewModel()
                    {
                        idSalida = p.idSalida,
                        Fecha = p.Fecha,
                        RecibidaPor = p.RecibidaPor,
                        Recibida = p.tUsuario2.Nombre + " " + p.tUsuario2.Apellidos,
                        RecibidaOtro = p.RecibidaOtro,
                        Tipo = p.Tipo,
                        idEntrada = p.idEntrada,
                        idVenta = p.idVenta,
                        Cantidad = p.Cantidad,
                        Remision = p.tVenta.Remision,
                        Comentarios = p.Comentarios,
                        fkSalida = p.fkSalida,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        InternalEgresses = context.tSalidas.Where(q => q.fkSalida == p.idSalida
                                                                      && p.Estatus == 1)
                            .Select(q => new InternalEgressViewModel()
                            {
                                idSalida = q.idSalida,
                                Fecha = q.Fecha,
                                RecibidaPor = q.RecibidaPor,
                                Recibida = q.tUsuario2.Nombre + " " + q.tUsuario2.Apellidos,
                                RecibidaOtro = q.RecibidaOtro,
                                Tipo = q.Tipo,
                                idEntrada = q.idEntrada,
                                idVenta = q.idVenta,
                                Cantidad = q.Cantidad,
                                Remision = q.tVenta.Remision,
                                Comentarios = q.Comentarios,
                                fkSalida = q.fkSalida,
                                Estatus = q.Estatus,
                                CreadoPor = q.CreadoPor,
                                Creado = q.Creado,
                                ModificadoPor = q.ModificadoPor,
                                Modificado = q.Modificado
                            }).ToList()
                    }).OrderByDescending(p => p.Creado).ToList();
            }
        }

        public List<EgressViewModel> GetActives(string remission)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tSalidas.Where(p => (p.tVenta.Remision.Contains(remission) || String.IsNullOrEmpty(remission))
                                                  && p.fkSalida == null
                                                  && p.Estatus == 1)
                    .Select(p => new EgressViewModel()
                    {
                        idSalida = p.idSalida,
                        Fecha = p.Fecha,
                        RecibidaPor = p.RecibidaPor,
                        Recibida = p.tUsuario2.Nombre + " " + p.tUsuario2.Apellidos,
                        RecibidaOtro = p.RecibidaOtro,
                        Tipo = p.Tipo,
                        idEntrada = p.idEntrada,
                        idVenta = p.idVenta,
                        Cantidad = p.Cantidad,
                        Remision = p.tVenta.Remision,
                        Comentarios = p.Comentarios,
                        fkSalida = p.fkSalida,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        InternalEgresses = context.tSalidas.Where(q => q.fkSalida == p.idSalida
                                                                      && p.Estatus == 1)
                            .Select(q => new InternalEgressViewModel()
                            {
                                idSalida = q.idSalida,
                                Fecha = q.Fecha,
                                RecibidaPor = q.RecibidaPor,
                                Recibida = q.tUsuario2.Nombre + " " + q.tUsuario2.Apellidos,
                                RecibidaOtro = q.RecibidaOtro,
                                Tipo = q.Tipo,
                                idEntrada = q.idEntrada,
                                idVenta = q.idVenta,
                                Cantidad = q.Cantidad,
                                Remision = q.tVenta.Remision,
                                Comentarios = q.Comentarios,
                                fkSalida = q.fkSalida,
                                Estatus = q.Estatus,
                                CreadoPor = q.CreadoPor,
                                Creado = q.Creado,
                                ModificadoPor = q.ModificadoPor,
                                Modificado = q.Modificado
                            }).ToList()
                    }).OrderByDescending(p => p.Creado).ToList();
            }
        }

        public EgressViewModel GetById(int id)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tSalidas.Where(p => p.idSalida == id)
                    .Select(p => new EgressViewModel()
                    {
                        idSalida = p.idSalida,
                        Fecha = p.Fecha,
                        RecibidaPor = p.RecibidaPor,
                        RecibidaOtro = p.RecibidaOtro,
                        Tipo = p.Tipo,
                        idEntrada = p.idEntrada,
                        idVenta = p.idVenta,
                        Cantidad = p.Cantidad,
                        Remision = p.tVenta.Remision,
                        Comentarios = p.Comentarios,
                        fkSalida = p.fkSalida,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).FirstOrDefault();
            }
        }

        //Post
        public int Add(tSalida egress)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                context.tSalidas.Add(egress);
                return context.SaveChanges();
            }
        }

        //Patch
        public int Update(tSalida egress)
        {
            using (var context = new admDB_SAADDBEntities())
            {

                var entity = context.tSalidas.Find(egress.idSalida);

                entity.Fecha = egress.Fecha;
                entity.RecibidaPor = egress.RecibidaPor;
                entity.RecibidaOtro = egress.RecibidaOtro;
                entity.Tipo = egress.Tipo;
                entity.idVenta = egress.idVenta;
                entity.Cantidad = egress.Cantidad;
                entity.Comentarios = egress.Comentarios;
                entity.fkSalida = egress.fkSalida;
                entity.ModificadoPor = egress.ModificadoPor;
                entity.Modificado = egress.Modificado;

                return context.SaveChanges();
            }
        }

        //Delete
        public int Delete(int id, int modifiedBy, DateTime modified)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var internalEgresses = context.tSalidas.Where(p => p.fkSalida == id);

                if (internalEgresses.Count() > 0)
                {
                    foreach (var internalEgress in internalEgresses)
                    {
                        internalEgress.ModificadoPor = modifiedBy;
                        internalEgress.Modificado = modified;
                        internalEgress.Estatus = 0;
                    }
                }

                var entity = context.tSalidas.Find(id);

                entity.ModificadoPor = modifiedBy;
                entity.Modificado = modified;
                entity.Estatus = 0;

                return context.SaveChanges();
            }
        }

        //UpdateReport
        public int UpdateReport(int idReport, List<int> egresses, int modifiedBy, DateTime modified)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                foreach (var egress in egresses)
                {
                    var internalEgresses = context.tSalidas.Where(p => p.fkSalida == egress);

                    if (internalEgresses.Count() > 0)
                    {
                        foreach (var internalEgress in internalEgresses)
                        {
                            var entityEgress = context.tSalidas.Find(internalEgress.idSalida);

                            entityEgress.ModificadoPor = modifiedBy;
                            entityEgress.Modificado = modified;
                            entityEgress.Estatus = 0;
                        }
                    }

                    var entity = context.tSalidas.Find(egress);

                    entity.idReporteCaja = idReport;
                    entity.ModificadoPor = modifiedBy;
                    entity.Modificado = modified;
                    entity.Estatus = 0;
                }

                return context.SaveChanges();
            }
        }
    }
}