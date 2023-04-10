using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class CatalogCategory : Base
    {
        private const bool ValueTrue = true;
        private const bool ValueFalse = false;

        public List<CategoryViewModel> GetCategories()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogoCategorias.Where(p => p.Activo == ValueTrue).Select(p => new CategoryViewModel()
                    {
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
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
                    return context.tCatalogoCategorias.Where(p => p.Nombre.Contains(category) && p.Activo == ValueTrue).Select(p => new CategoryViewModel()
                    {
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
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
                    return context.tCatalogoCategorias.Where(p => p.Nombre.Contains(category) || String.IsNullOrEmpty(category) && p.Activo == ValueTrue).Count();
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
                    var categories = context.tCatalogoCategorias
                        .Where(p => p.Nombre.Contains(category) || String.IsNullOrEmpty(category) && p.Activo == ValueTrue)
                        .Select(p => new CategoryViewModel()
                        {
                            idCategoria = p.idCategoria,
                            Nombre = p.Nombre,
                            Descripcion = p.Descripcion,
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
                    return context.tCatalogoCategorias.Where(p => p.idCategoria == idCategoria).Select(p => new CategoryViewModel
                    {
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Imagen = p.Imagen
                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddCategory(tCatalogoCategoria oCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var category = new tCatalogoCategoria();

                    category.Nombre = oCategory.Nombre;
                    category.Descripcion = oCategory.Descripcion;
                    category.Activo = ValueTrue;
                    context.tCatalogoCategorias.Add(category);

                    context.SaveChanges();

                    return category.idCategoria;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateCategory(CategoryViewModel oCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCatalogoCategoria category = context.tCatalogoCategorias.FirstOrDefault(p => p.idCategoria == oCategory.idCategoria);

                    category.Nombre = oCategory.Nombre;
                    category.Descripcion = oCategory.Descripcion;

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
                    tCatalogoCategoria category = context.tCatalogoCategorias.FirstOrDefault(p => p.idCategoria == idCategory);
                    category.Imagen = image;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteCategory(int id)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var category = context.tCatalogoCategorias.FirstOrDefault(p => p.idCategoria == id && p.Activo == ValueTrue);

                    SubCategories oSubcategories = new SubCategories();

                    foreach (var sub in oSubcategories.GetIDSubcategoryByCategory(id))
                    {
                        var subcategory = context.tCatalogoSubcategorias.FirstOrDefault(p => p.idSubcategoria == sub);

                        if (context.tCatalogos.Where(p => p.idSubcategoria == subcategory.idSubcategoria && p.Activo == ValueTrue).Count() == 0)
                        {
                            subcategory.Activo = ValueFalse;
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new CategoriesException("Existen catálogos registrados a la subcategoría " + subcategory.Nombre);
                        }
                    }

                    if (context.tCatalogoCategorias.Where(p => p.idCategoria == id && p.Activo == ValueTrue).Count() == 0)
                    {
                        category.Activo = ValueFalse;
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new CategoriesException("Existen catálogos registrados a la categoría " + category.Nombre);
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
}