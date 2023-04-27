using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class TypeMeasure : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoMedida.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<TypeMeasureViewModel> GetTypeMeasure()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoMedida.Select(p => new TypeMeasureViewModel()
                    {
                        idTipoMedida = p.idTipoMedida,
                        NombreMedida = p.NombreMedida
                    }).OrderBy(p => p.NombreMedida).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<TypeMeasureViewModel> GetTypeMeasureForName(string typeMeasure)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoMedida.Where(p => p.NombreMedida.Contains(typeMeasure)).Select(p => new TypeMeasureViewModel()
                    {
                        idTipoMedida = p.idTipoMedida,
                        NombreMedida = p.NombreMedida
                    }).OrderBy(p => p.NombreMedida).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int GetTotalTypeMeasure(string typeMeasure)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoMedida.Where(p => p.NombreMedida.Contains(typeMeasure) || String.IsNullOrEmpty(typeMeasure)).Count();
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

      
        
           
                


        public List<TypeMeasureViewModel> GetTypeMeasure(string typeMeasure, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var typeMeasures = context.tTipoMedida
                        .Where(p => p.NombreMedida.Contains(typeMeasure) || String.IsNullOrEmpty(typeMeasure))
                        .Select(p => new TypeMeasureViewModel()
                    {
                            idTipoMedida = p.idTipoMedida,
                            NombreMedida = p.NombreMedida
                        }).OrderBy(p => p.NombreMedida);

                    return typeMeasures.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public TypeMeasureViewModel GetTypeMeasure(int idTipoMedida)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoMedida.Where(p => p.idTipoMedida == idTipoMedida).Select(p => new TypeMeasureViewModel
                    {
                        idTipoMedida = p.idTipoMedida,
                        NombreMedida = p.NombreMedida
                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int AddTypeMeasure(tTipoMedida oTipoMedida)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tTipoMedida tTipoMedida = new tTipoMedida();

                    tTipoMedida.NombreMedida = oTipoMedida.NombreMedida;
                    context.tTipoMedida.Add(tTipoMedida);

                    context.SaveChanges();

                    iResult = tTipoMedida.idTipoMedida;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }

            return iResult;
        }

        public void UpdateTypeMeasure(TypeMeasureViewModel oTypeMeasure)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tTipoMedida tTipoMedida = context.tTipoMedida.FirstOrDefault(p => p.idTipoMedida == oTypeMeasure.idTipoMedida);

                    tTipoMedida.NombreMedida = oTypeMeasure.NombreMedida;
                   

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void DeleteTypeMeasure(int idTypeMeasure)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tTipoMedida tTipoMedida = context.tTipoMedida.FirstOrDefault(p => p.idTipoMedida == idTypeMeasure);
                    context.tTipoMedida.Remove(tTipoMedida);
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
                catch (TypeMeasureException ex)
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

    class TypeMeasureException : Exception
    {
        public string messageEx { get; set; }

        public TypeMeasureException(string message)
        {
            messageEx = message;
        }
    }
}