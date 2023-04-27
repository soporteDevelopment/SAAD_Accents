using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Categories : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCategorias.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<CategoryViewModel> GetCategories()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCategorias.Select(p => new CategoryViewModel()
                    {
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripción,
                        Imagen = p.Imagen
                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<CategoryViewModel> GetCategoriesForName(string category)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCategorias.Where(p => p.Nombre.Contains(category)).Select(p => new CategoryViewModel()
                    {
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripción,
                        Imagen = p.Imagen
                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int GetTotalCategories(string category)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCategorias.Where(p => p.Nombre.Contains(category) || String.IsNullOrEmpty(category)).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CategoryViewModel> GetCategories(string category, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var categories = context.tCategorias
                        .Where(p => p.Nombre.Contains(category) || String.IsNullOrEmpty(category))
                        .Select(p => new CategoryViewModel()
                    {

                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripción,
                        Imagen = p.Imagen
                    }).OrderBy(p => p.Nombre);

                    return categories.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public CategoryViewModel GetCategory(int idCategoria)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCategorias.Where(p => p.idCategoria == idCategoria).Select(p => new CategoryViewModel
                    {
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripción,
                        Imagen = p.Imagen
                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int AddCategory(tCategoria oCategory)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCategoria tCategoria = new tCategoria();

                    tCategoria.Nombre = oCategory.Nombre;
                    tCategoria.Descripción = oCategory.Descripción;
                    context.tCategorias.Add(tCategoria);

                    context.SaveChanges();

                    iResult = tCategoria.idCategoria;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }

            return iResult;
        }

        public void UpdateCategory(CategoryViewModel oCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCategoria tCategoria = context.tCategorias.FirstOrDefault(p => p.idCategoria == oCategory.idCategoria);

                    tCategoria.Nombre = oCategory.Nombre;
                    tCategoria.Descripción = oCategory.Descripcion;
                    tCategoria.idCategoria = tCategoria.idCategoria;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void UpdateImage(int idCategory, string image)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCategoria tCategoria = context.tCategorias.FirstOrDefault(p => p.idCategoria == idCategory);
                    tCategoria.Imagen = image;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteCategory(int idCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCategoria tCategoria = context.tCategorias.FirstOrDefault(p => p.idCategoria == idCategory);

                    SubCategories oSubcategories = new SubCategories();

                    foreach (var sub in oSubcategories.GetIDSubcategoryByCategory(idCategory))
                    {

                        tSubcategoria tSubcategoria = context.tSubcategorias.FirstOrDefault(p => p.idSubcategoria == sub);

                        if (context.tProductos.Where(p => p.idSubcategoria == tSubcategoria.idSubcategoria && p.Estatus == TypesProduct.EstatusActivo).Count() == 0)
                        {
                            context.tSubcategorias.Remove(tSubcategoria);
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new CategoriesException("Existen productos registrados a la subcategoría " + tSubcategoria.Nombre);
                        }

                    }

                    if (context.tProductos.Where(p => p.idCategoria == idCategory && p.Estatus == TypesProduct.EstatusActivo).Count() == 0)
                    {
                        context.tCategorias.Remove(tCategoria);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new CategoriesException("Existen productos registrados a la categoría " + tCategoria.Nombre);
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
    }

    class CategoriesException : Exception
    {
        public string messageEx { get; set; }

        public CategoriesException(string message)
        {
            messageEx = message;
        }
    }
}