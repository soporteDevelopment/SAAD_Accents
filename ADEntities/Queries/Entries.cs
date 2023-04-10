using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADEntities.ViewModels;
using ADEntities.Models;

namespace ADEntities.Queries
{
    public class Entries
    {
        //Get
        public List<EntryViewModel> Get(DateTime start, DateTime end, string deliveredBy)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var dtStart = start.Date + new TimeSpan(0, 0, 0);
                var dtEnd = end.Date + new TimeSpan(23, 59, 59);

                return context.tEntradas.Where(p => (p.Fecha >= dtStart && p.Fecha <= dtEnd)
                                            && (p.tUsuario.Nombre.Contains(deliveredBy) || p.EntregadaOtro.Contains(deliveredBy)
                                                || String.IsNullOrEmpty(deliveredBy))
                                            && p.Estatus == 1)
                    .Select(p => new EntryViewModel()
                    {
                        idEntrada = p.idEntrada,
                        Tipo = p.Tipo,
                        idVenta = p.idVenta,
                        Fecha = p.Fecha,
                        EntregadaPor = p.EntregadaPor,
                        Entregada = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        EntregadaOtro = p.EntregadaOtro,
                        Cantidad = p.Cantidad,
                        Comentarios = p.Comentarios,
                        Remision = p.tVenta.Remision,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).OrderByDescending(p=>p.Creado).ToList();
            }
        }

        //GetActives
        public List<EntryViewModel> GetActives(string remission)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tEntradas.Where(p => (p.tVenta.Remision.Contains(remission) || String.IsNullOrEmpty(remission)) 
                                                    && p.Estatus == 1)
                    .Select(p => new EntryViewModel()
                    {
                        idEntrada = p.idEntrada,
                        Tipo = p.Tipo,
                        idVenta = p.idVenta,
                        Fecha = p.Fecha,
                        EntregadaPor = p.EntregadaPor,
                        Entregada = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        EntregadaOtro = p.EntregadaOtro,
                        Cantidad = p.Cantidad,
                        Comentarios = p.Comentarios,
                        Remision = p.tVenta.Remision,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).OrderByDescending(p=>p.Fecha).ToList();
            }
        }

        public EntryViewModel GetById(int id)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tEntradas.Where(p => p.idEntrada == id)
                    .Select(p => new EntryViewModel()
                    {
                        idEntrada = p.idEntrada,
                        Tipo = p.Tipo,
                        idVenta = p.idVenta,
                        Fecha = p.Fecha,
                        EntregadaPor = p.EntregadaPor,
                        EntregadaOtro = p.EntregadaOtro,
                        Cantidad = p.Cantidad,
                        Comentarios = p.Comentarios,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).FirstOrDefault();
            }
        }

        public List<EntryViewModel> GetByReportId(int idReport)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tEntradas.Where(p => p.idReporteCaja == idReport)
                    .Select(p => new EntryViewModel()
                    {
                        idEntrada = p.idEntrada,
                        Tipo = p.Tipo,
                        idVenta = p.idVenta,
                        Fecha = p.Fecha,
                        EntregadaPor = p.EntregadaPor,
                        EntregadaOtro = p.EntregadaOtro,
                        Cantidad = p.Cantidad,
                        Comentarios = p.Comentarios,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).ToList();
            }
        }

        //Post
        public int Add(tEntrada entry)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                context.tEntradas.Add(entry);
                context.SaveChanges();
                return entry.idEntrada;
            }
        }

        //Patch
        public int Update(tEntrada entry)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tEntradas.Find(entry.idEntrada);
                
                entity.EntregadaPor = entry.EntregadaPor;
                entity.EntregadaOtro = entry.EntregadaOtro;
                entity.Cantidad = entry.Cantidad;
                entity.Comentarios = entry.Comentarios;
                entity.ModificadoPor = entry.ModificadoPor;
                entity.Modificado = entry.Modificado;

                return context.SaveChanges();
            }
        }

        //Cancel
        public int Cancel(int id, int modifiedBy, DateTime modified)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tEntradas.Find(id);
                
                entity.ModificadoPor = modifiedBy;
                entity.Modificado = modified;
                entity.Estatus = 0;

                return context.SaveChanges();
            }
        }

        //Delete
        public int Delete(int id, int modifiedBy, DateTime modified)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tEntradas.Find(id);

                entity.idVenta = null;
                entity.ModificadoPor = modifiedBy;
                entity.Modificado = modified;
                entity.Estatus = 0;

                return context.SaveChanges();
            }
        }

        //UpdateReport
        public int UpdateReport(int idReport, List<int> entries, int modifiedBy, DateTime modified)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                foreach (var entry in entries)
                {
                    var entity = context.tEntradas.Find(entry);

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