using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Modules : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tControles.Count();
        }

        public List<ControllerViewModel> GetControllers()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tControles.Select(p => new ControllerViewModel()
                    {

                        idControl = p.idControl,
                        Control = p.Control,
                        Descripcion = p.Descripcion

                    }).OrderBy(p => p.Control).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<ControllerViewModel> GetControllers(string controller)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tControles.Where(p => p.Control.Contains(controller)).Select(p => new ControllerViewModel()
                    {

                        idControl = p.idControl,
                        Control = p.Control,
                        Descripcion = p.Descripcion

                    }).OrderBy(p => p.Control).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<ControllerViewModel> GetControllers(int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var controllers = context.tControles.Select(p => new ControllerViewModel()
                    {

                        idControl = p.idControl,
                        Control = p.Control,
                        Descripcion = p.Descripcion

                    }).OrderBy(p => p.Control);

                    return controllers.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public ControllerViewModel GetController(int idController)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var control = (from c in context.tControles
                                   where c.idControl == idController
                                   select new ControllerViewModel()
                                   {

                                       idControl = c.idControl,
                                       Control = c.Control,
                                       Estatus = (short)c.Estatus,
                                       Descripcion = c.Descripcion

                                   }).FirstOrDefault();

                    return (ControllerViewModel)control;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int AddController(tControle tControl)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    context.tControles.Add(tControl);

                    context.SaveChanges();

                    iResult = tControl.idControl;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }

            return iResult;
        }

        public void UpdateController(ControllerViewModel oController)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tControle tControl = context.tControles.FirstOrDefault(p => p.idControl == oController.idControl);

                    tControl.Control = oController.Control;
                    tControl.Descripcion = oController.Descripcion;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<string> GetMain(int idProfile)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    List<string> lMain = new List<string>();

                    lMain = (from a in context.tAcciones
                             join m in context.tControles on a.idControl equals m.idControl
                             join p in context.tPerfilAcciones on a.idAcciones equals p.idAccion
                             join per in context.tPerfiles on p.idPerfil equals per.idPerfil
                             where a.MenuPadre == TypesAction.True && per.idPerfil == idProfile
                             select m.Control).ToList();

                    return lMain;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }
    }
}