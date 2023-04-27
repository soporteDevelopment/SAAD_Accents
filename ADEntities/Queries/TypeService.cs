using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class TypeService : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoServicio.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<TypeServiceViewModel> GetTypeService()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoServicio.Select(p => new TypeServiceViewModel()
                    {
                        idTipoServicio = p.idTipoServicio,
                        Descripcion = p.Descripcion,
                        FactorUtilidad = p.FactorUtilidad
                    }).OrderBy(p => p.Descripcion).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<TypeServiceViewModel> GetTypeServiceForName(string typeService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoServicio.Where(p => p.Descripcion.Contains(typeService)).Select(p => new TypeServiceViewModel()
                    {
                        idTipoServicio = p.idTipoServicio,
                        Descripcion = p.Descripcion,
                        FactorUtilidad = p.FactorUtilidad
                    }).OrderBy(p => p.Descripcion).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int GetTotalTypeService(string typeService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoServicio.Where(p => p.Descripcion.Contains(typeService) || String.IsNullOrEmpty(typeService)).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }


     /**   public List<TypeServiceViewModel> GetTypeServices()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoServicio.Select(p => new TypeServiceViewModel()
                        {
                            idTipoServicio = p.idTipoServicio,
                            Descripcion = p.Descripcion,
                            FactorUtilidad = p.FactorUtilidad
                        }).OrderBy(p => p.Descripcion).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }**/

      
        
           
                


        public List<TypeServiceViewModel> GetTypeServices(string typeService, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var typeServices = context.tTipoServicio
                        .Where(p => p.Descripcion.Contains(typeService) || String.IsNullOrEmpty(typeService))
                        .Select(p => new TypeServiceViewModel()
                    {

                            idTipoServicio = p.idTipoServicio,
                            Descripcion = p.Descripcion,
                            FactorUtilidad = p.FactorUtilidad
                        }).OrderBy(p => p.Descripcion);

                    return typeServices.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public TypeServiceViewModel GetTypeService(int idTipoServicio)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoServicio.Where(p => p.idTipoServicio == idTipoServicio).Select(p => new TypeServiceViewModel
                    {
                        idTipoServicio = p.idTipoServicio,
                        Descripcion = p.Descripcion,
                        FactorUtilidad = p.FactorUtilidad
                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int AddTypeService(tTipoServicio oTipoServicio)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tTipoServicio tTipoServicio = new tTipoServicio();

                    tTipoServicio.Descripcion = oTipoServicio.Descripcion;
                    tTipoServicio.FactorUtilidad = oTipoServicio.FactorUtilidad;
                    context.tTipoServicio.Add(tTipoServicio);

                    context.SaveChanges();

                    iResult = tTipoServicio.idTipoServicio;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }

            return iResult;
        }

        public void UpdateTypeService(TypeServiceViewModel oTypeService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tTipoServicio tTipoServicio = context.tTipoServicio.FirstOrDefault(p => p.idTipoServicio == oTypeService.idTipoServicio);

                    tTipoServicio.Descripcion = oTypeService.Descripcion;
                    tTipoServicio.FactorUtilidad = oTypeService.FactorUtilidad;
                   

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void DeleteTypeService(int idTypeService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tTipoServicio tTipoServicio = context.tTipoServicio.FirstOrDefault(p => p.idTipoServicio == idTypeService);
                    context.tTipoServicio.Remove(tTipoServicio);
                    context.SaveChanges();
                    /* if (context.tProductos.Where(p => p.idCategoria == idCategory && p.Estatus == TypesProduct.EstatusActivo).Count() == 0)
                     {
                         context.tCategorias.Remove(tCategoria);
                         context.SaveChanges();
                     }
                     else
                     {
                         throw new CategoriesException("Existen productos registrados a la categoría " + tCategoria.Nombre);
                     }*/
                }
                catch (CategoriesException ex)
                {
                    throw new ApplicationException(string.Format("Se generó la siguiente excepción:{0}", ex.messageEx));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }
    }

    class TypeServiceException : Exception
    {
        public string messageEx { get; set; }

        public TypeServiceException(string message)
        {
            messageEx = message;
        }
    }
}