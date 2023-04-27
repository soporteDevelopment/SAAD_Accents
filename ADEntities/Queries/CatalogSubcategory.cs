using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class CatalogSubcategory : Base
    {
        private const bool ValueTrue = true;
        private const bool ValueFalse = false;

        public int CountRegisters(int idCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tCatalogoSubcategorias.Where(p => p.idCategoria == idCategory && p.Activo == ValueTrue).Count();
        }

        public List<SubcategoryViewModel> GetSubCategories(int idCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tCatalogoSubcategorias.Where(p => p.idCategoria == idCategory && p.Activo == ValueTrue).Select(p => new SubcategoryViewModel()
                {
                    idSubcategoria = p.idSubcategoria,
                    idCategoria = p.idCategoria,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Imagen = p.Imagen                    
                }).OrderBy(p => p.Nombre).ToList();
        }

        public List<SubcategoryViewModel> GetSubCategories(int page, int pageSize, int idCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var subcategories = context.tCatalogoSubcategorias.Where(p => p.idCategoria == idCategory && p.Activo == ValueTrue).Select(p => new SubcategoryViewModel()
                    {
                        idSubcategoria = p.idSubcategoria,
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Imagen = p.Imagen
                    }).OrderBy(p => p.Nombre);

                    return subcategories.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public SubcategoryViewModel GetSubcategory(int idSubCategoria)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tCatalogoSubcategorias.Where(p => p.idSubcategoria == idSubCategoria).Select(p => new SubcategoryViewModel
                {
                    idSubcategoria = p.idSubcategoria,
                    idCategoria = p.idCategoria,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Imagen = p.Imagen                    
                }).First();
        }

        public List<SubcategoryViewModel> GetSubcategoryByCategory(int idCategoria)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tCatalogoSubcategorias.Where(p => p.idCategoria == idCategoria && p.Activo == ValueTrue).Select(p => new SubcategoryViewModel
                {
                    idSubcategoria = p.idSubcategoria,
                    idCategoria = p.idCategoria,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Imagen = p.Imagen
                }).OrderBy(p => p.Nombre).ToList();
        }

        public List<int> GetIDSubcategoryByCategory(int idCategoria)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tCatalogoSubcategorias.Where(p => p.idCategoria == idCategoria && p.Activo == ValueTrue).Select(p => p.idSubcategoria).ToList();
        }

        public int AddSubcategory(tCatalogoSubcategoria oSubcategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var subcategory = new tCatalogoSubcategoria();

                    subcategory.Nombre = oSubcategory.Nombre;
                    subcategory.Descripcion = oSubcategory.Descripcion;
                    subcategory.idCategoria = oSubcategory.idCategoria;
                    subcategory.Activo = ValueTrue;
                    context.tCatalogoSubcategorias.Add(subcategory);

                    context.SaveChanges();

                    return subcategory.idSubcategoria;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateSubcategory(tCatalogoSubcategoria entity)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var subcategory = context.tCatalogoSubcategorias.FirstOrDefault(p => p.idSubcategoria == entity.idSubcategoria);

                    subcategory.Nombre = entity.Nombre;
                    subcategory.Descripcion = entity.Descripcion;
                    subcategory.idSubcategoria = entity.idSubcategoria;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteSubcategory(int id)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    if (context.tCatalogos.Where(p => p.idSubcategoria == id && p.Activo == ValueTrue).Count() == 0)
                    {
                        var subcategory = context.tCatalogoSubcategorias.FirstOrDefault(p => p.idSubcategoria == id);
                        subcategory.Activo = ValueFalse;
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new CategoriesException("Existen productos registrados a la subcategoría");
                    }
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

        public void UpdateImage(int id, string image)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var subcategory = context.tSubcategorias.FirstOrDefault(p => p.idSubcategoria == id);
                    subcategory.Imagen = image;
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