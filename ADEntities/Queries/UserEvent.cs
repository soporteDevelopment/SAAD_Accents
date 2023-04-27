using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class UserEvent
    {
        public List<UserEventViewModel> GetUserEvents(int idUsuario)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarioEventoes.Where(p => p.idUsuario == idUsuario).Select(p => new UserEventViewModel()
                    {
                        idUsuarioEvento = p.idUsuarioEvento,
                        idEvento = p.idEvento,
                        idUsuario = p.idUsuario
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public UserEventViewModel GetEventByID(int eventID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarioEventoes.Where(p => p.idEvento == eventID).Select(p => new UserEventViewModel()
                    {

                        idUsuarioEvento = p.idUsuarioEvento,
                        idEvento = p.idEvento,
                        idUsuario = p.idUsuario
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void AddUserEvent(List<int> users, int eventID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var user in users)
                    {
                        tUsuarioEvento userEvent = new tUsuarioEvento();
                        userEvent.idUsuario = user;
                        userEvent.idEvento = eventID;

                        context.tUsuarioEventoes.Add(userEvent);

                        context.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateUserEvent(List<int> users, int eventID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var user in users)
                    {
                        this.DeleteUserEvent(eventID);
                    }

                    this.AddUserEvent(users, eventID);
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteUserEvent(int eventID)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    var usersEvent = context.tUsuarioEventoes.Where(p => p.idEvento == eventID).ToList();

                    if (usersEvent != null)
                    {
                        foreach (var userEvent in usersEvent)
                        {
                            context.tUsuarioEventoes.Remove(userEvent);
                        }

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