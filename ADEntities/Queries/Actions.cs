using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Actions : Base
    {

        public int CountRegistersWithFilters(string module, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities()) try
                {
                    return context.tAcciones.Where(p => p.tControle.Control.Contains(module) || module == string.Empty).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    throw new ApplicationException(string.Format("Se generó la siguiente excepción, Mensaje: {0} , Interno: {1}", newException.Message, ((newException.InnerException != null)? newException.InnerException.InnerException.Message : "")));
                }
        }

        public List<tAccione> GetAllActions()
        {

            using (var context = new admDB_SAADDBEntities()) try
                {
                    return context.tAcciones.Select(p => new tAccione()
                    {

                        idAcciones = p.idAcciones,
                        Accion = p.Accion,
                        MenuPadre = p.MenuPadre,
                        MenuHijo = p.MenuHijo,
                        Ajax = p.Ajax,
                        idControl = p.idControl,
                        tControle = p.tControle,
                        Icono = p.Icono,
                        Estatus = p.Estatus,
                        Descripcion = p.Descripcion

                    }).OrderBy(p => p.Accion).ToList();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ActionsControllerViewModel> GetAllActionsForController()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return (from c in context.tControles
                            select new ActionsControllerViewModel()
                            {

                                idControl = c.idControl,
                                Control = c.Control,
                                lAcciones = c.tAcciones.Select(x => new ActionViewModel()
                                {
                                    idAction = x.idAcciones,
                                    Accion = x.Accion,
                                    Selected = true,
                                    Descripcion = x.Descripcion

                                }).OrderBy(p => p.Accion).ToList()

                            }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ActionViewModel> GetActionsForProfileLog(int idProfile)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    List<ActionViewModel> lActions = new List<ActionViewModel>();

                    lActions = (from a in context.tAcciones
                                join pa in context.tPerfilAcciones on a.idAcciones equals pa.idAccion
                                join c in context.tControles on a.idControl equals c.idControl
                                where pa.idPerfil == idProfile
                                select new ActionViewModel()
                                {

                                    idControl = c.idControl,
                                    Control = c.Control,
                                    idAction = a.idAcciones,
                                    Accion = a.Accion,
                                    Selected = true,
                                    Descripcion = a.Descripcion

                                }).OrderBy(p => p.Control).ThenBy(p => p.Accion).ToList();

                    return lActions;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ActionViewModel> GetActions(string module, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var actions = context.tAcciones.Where(p => p.tControle.Control.Contains(module) || module == string.Empty).Select(p => new ActionViewModel
                    {
                        idAction = p.idAcciones,
                        Accion = p.Accion,
                        idControl = p.idControl,
                        Control = p.tControle.Control
                    }).OrderBy(p => p.Control).ThenBy(p => p.Accion);

                    return actions.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

        }

        public ActionViewModel GetAction(int idAction)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tAcciones.Where(p => p.idAcciones == idAction).Select(p => new ActionViewModel
                    {

                        idAction = p.idAcciones,
                        Accion = p.Accion,
                        Ajax = p.Ajax,
                        MenuPadre = p.MenuPadre,
                        MenuHijo = p.MenuHijo,
                        idControl = p.idControl,
                        Descripcion = p.Descripcion

                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ActionsControllerViewModel> GetActionsForProfile(int idProfile)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var actionsOne = GetActionsForProfileLog(idProfile);

                    var actionsTwo = (from c in context.tControles
                                      select new ActionsControllerViewModel()
                                      {
                                          idControl = c.idControl,
                                          Control = c.Control,
                                          lAcciones = c.tAcciones.Select(x => new ActionViewModel()
                                          {
                                              idAction = x.idAcciones,
                                              Accion = x.Accion,
                                              Selected = false,
                                              Descripcion = x.Descripcion

                                          }).ToList()
                                      }).ToList();

                    foreach (var a in actionsTwo)
                    {
                        foreach (ActionViewModel b in a.lAcciones)
                        {
                            b.Selected = actionsOne.Any(p => p.idAction == b.idAction);
                        }
                    }

                    return actionsTwo;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddAction(tAccione oAction)
        {

            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tAccione tAccion = new tAccione();

                    tAccion.Accion = oAction.Accion;
                    tAccion.MenuPadre = oAction.MenuPadre;
                    tAccion.MenuHijo = oAction.MenuHijo;
                    tAccion.Ajax = oAction.Ajax;
                    tAccion.idControl = oAction.idControl;
                    tAccion.Estatus = TypesAction.True;
                    tAccion.Descripcion = oAction.Descripcion;

                    context.tAcciones.Add(tAccion);

                    context.SaveChanges();

                    iResult = tAccion.idAcciones;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

            return iResult;
        }

        public void UpdateAction(ActionViewModel oAction)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tAccione tAccion = context.tAcciones.FirstOrDefault(p => p.idAcciones == oAction.idAction);

                    tAccion.Accion = oAction.Accion;
                    tAccion.MenuPadre = oAction.MenuPadre;
                    tAccion.MenuHijo = oAction.MenuHijo;
                    tAccion.Ajax = oAction.Ajax;
                    tAccion.idControl = oAction.idControl;
                    tAccion.Icono = oAction.Icon;
                    tAccion.Estatus = 1;
                    tAccion.Descripcion = oAction.Descripcion;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

    }
}