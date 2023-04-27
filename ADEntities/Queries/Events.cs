using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class Events
    {
        public List<EventViewModel> GetEvents(int idUsuario)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = DateTime.Now.AddMonths(-1).Date + new TimeSpan(0, 0, 0);
                    var dtEnd = DateTime.Now.AddMonths(1).Date + new TimeSpan(23, 59, 59);

                    return context.tEventos.Where(p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && p.Cancelado != true).Select(p => new EventViewModel()
                    {
                        idEvento = p.idEvento,
                        Nombre = p.Nombre,
                        Fecha = p.Fecha,
                        TodoDia = p.TodoDia,
                        HoraInicio = p.HoraInicio,
                        HoraFin = p.HoraFin,
                        Lugar = p.Lugar,
                        idUsuario = p.idUsuario,
                        Cancelado = p.Cancelado,
                        Color = p.Color,
                        Fondo = p.Fondo,
                        Fuente = p.Fuente,
                        FechaRecordatorio = p.FechaRecordatorio,
                        HoraRecordatorio = p.HoraRecordatorio,
                        Tipo = p.Tipo,
                        CreadoPor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos
                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<EventViewModel> GetEventsToday(int idUsuario)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = DateTime.Now.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = DateTime.Now.Date + new TimeSpan(23, 59, 59);

                    return context.tEventos.Where(p => p.idUsuario == idUsuario && p.Cancelado != true && (p.Fecha >= dtStart && p.Fecha <= dtEnd)).Select(p => new EventViewModel()
                    {
                        idEvento = p.idEvento,
                        Nombre = p.Nombre,
                        Fecha = p.Fecha,
                        TodoDia = p.TodoDia,
                        HoraInicio = p.HoraInicio,
                        HoraFin = p.HoraFin,
                        Lugar = p.Lugar,
                        idUsuario = p.idUsuario,
                        Cancelado = p.Cancelado,
                        Color = p.Color,
                        Fondo = p.Fondo,
                        Fuente = p.Fuente,
                        FechaRecordatorio = p.FechaRecordatorio,
                        HoraRecordatorio = p.HoraRecordatorio,
                        Tipo = p.Tipo,
                        CreadoPor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos
                    }).Union(context.tEventos.Join(context.tUsuarioEventoes,
                        e => e.idEvento,
                        ue => ue.idEvento,
                        (e, ue) => new EventViewModel()
                        {
                            idEvento = e.idEvento,
                            Nombre = e.Nombre,
                            Fecha = e.Fecha,
                            TodoDia = e.TodoDia,
                            HoraInicio = e.HoraInicio,
                            HoraFin = e.HoraFin,
                            Lugar = e.Lugar,
                            idUsuario = ue.idUsuario,
                            Cancelado = e.Cancelado,
                            Color = e.Color,
                            Fondo = e.Fondo,
                            Fuente = e.Fuente,
                            FechaRecordatorio = e.FechaRecordatorio,
                            HoraRecordatorio = e.HoraRecordatorio,
                            Tipo = e.Tipo,
                            CreadoPor = e.tUsuario.Nombre + " " + e.tUsuario.Apellidos
                        }).Where(use => use.idUsuario == idUsuario && use.Cancelado != true && (use.Fecha >= dtStart && use.Fecha <= dtEnd))
                    ).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public bool VerifyEvent(int eventID, int userID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tEventos.Where(p => p.idEvento == eventID && p.idUsuario == userID).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public EventViewModel GetEventByID(int eventID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tEventos.Where(p => p.idEvento == eventID).Select(p => new EventViewModel()
                    {

                        idEvento = p.idEvento,
                        Nombre = p.Nombre,
                        Fecha = p.Fecha,
                        TodoDia = p.TodoDia,
                        HoraInicio = p.HoraInicio,
                        HoraFin = p.HoraFin,
                        Lugar = p.Lugar,
                        idUsuario = p.idUsuario,
                        Cancelado = p.Cancelado,
                        Color = p.Color,
                        Fondo = p.Fondo,
                        Fuente = p.Fuente,
                        FechaRecordatorio = p.FechaRecordatorio,
                        HoraRecordatorio = p.HoraRecordatorio,
                        Tipo = p.Tipo,
                        SelectedUsers = context.tUsuarioEventoes.Where(ue => ue.idEvento == eventID).Select(ue => ue.idUsuario).ToList()
                    }).OrderBy(p => p.Nombre).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public int AddEvent(tEvento newEvent)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
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

        public void UpdateEvent(tEvento updateEvent)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var entity = context.tEventos.Where(p => p.idEvento == updateEvent.idEvento).FirstOrDefault();

                    if (entity != null)
                    {
                        entity.Nombre = updateEvent.Nombre;
                        entity.Fecha = updateEvent.Fecha;
                        entity.TodoDia = updateEvent.TodoDia;
                        entity.HoraInicio = updateEvent.HoraInicio;
                        entity.HoraFin = updateEvent.HoraFin;
                        entity.Lugar = updateEvent.Lugar;
                        entity.Color = updateEvent.Color;
                        entity.Fondo = updateEvent.Fondo;
                        entity.Fuente = updateEvent.Fuente;
                        entity.FechaRecordatorio = updateEvent.FechaRecordatorio;
                        entity.Tipo = updateEvent.Tipo;
                        entity.HoraRecordatorio = updateEvent.HoraRecordatorio;

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }

        public void CancellEvent(int eventID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var rEvent = context.tEventos.Where(p => p.idEvento == eventID).FirstOrDefault();

                    if (rEvent != null)
                    {
                        rEvent.Cancelado = true;

                        context.SaveChanges();
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