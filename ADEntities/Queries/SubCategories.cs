using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class SubCategories : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tSubcategorias.Count();
        }

        public List<SubcategoryViewModel> GetSubCategories()
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tSubcategorias.Select(p => new SubcategoryViewModel()
                {

                    idSubcategoria = p.idSubcategoria,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripción,
                    Imagen = p.Imagen,
                    idCategoria = p.idCategoria

                }).OrderBy(p => p.Nombre).ToList();
        }

        public List<SubcategoryViewModel> GetSubCategories(int page, int pageSize, int idCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var SubCategories = context.tSubcategorias.Where(p => p.idCategoria == idCategory).Select(p => new SubcategoryViewModel()
                    {
                        idSubcategoria = p.idSubcategoria,
                        idCategoria = p.idCategoria,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripción,
                        Imagen = p.Imagen
                    }).OrderBy(p => p.Nombre);

                    return SubCategories.Skip(page * pageSize).Take(pageSize).ToList();
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
                return context.tSubcategorias.Where(p => p.idSubcategoria == idSubCategoria).Select(p => new SubcategoryViewModel
                {
                    idSubcategoria = p.idSubcategoria,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripción,
                    Imagen = p.Imagen,
                    idCategoria = p.idCategoria
                }).First();
        }

        public List<SubcategoryViewModel> GetSubcategoryByCategory(int idCategoria)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tSubcategorias.Where(p => p.idCategoria == idCategoria).Select(p => new SubcategoryViewModel
                {
                    idSubcategoria = p.idSubcategoria,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripción,
                    Imagen = p.Imagen,
                    idCategoria = p.idSubcategoria
                }).OrderBy(p => p.Nombre).ToList();
        }

        public List<int> GetIDSubcategoryByCategory(int idCategoria)
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tSubcategorias.Where(p => p.idCategoria == idCategoria).Select(p => p.idSubcategoria).ToList();
        }

        public int AddSubcategory(tSubcategoria oSubcategory)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tSubcategoria tSubCategoria = new tSubcategoria();

                    tSubCategoria.Nombre = oSubcategory.Nombre;
                    tSubCategoria.Descripción = oSubcategory.Descripción;
                    tSubCategoria.idCategoria = oSubcategory.idCategoria;
                    context.tSubcategorias.Add(tSubCategoria);

                    context.SaveChanges();

                    iResult = tSubCategoria.idSubcategoria;

                    return iResult;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateSubcategory(tSubcategoria oSubcategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tSubcategoria tSubCategoria = context.tSubcategorias.FirstOrDefault(p => p.idSubcategoria == oSubcategory.idSubcategoria);

                    tSubCategoria.Nombre = oSubcategory.Nombre;
                    tSubCategoria.Descripción = oSubcategory.Descripción;
                    tSubCategoria.idSubcategoria = oSubcategory.idSubcategoria;

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void DeleteSubcategory(int idSubcategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    if (context.tProductos.Where(p => p.idSubcategoria == idSubcategory && p.Estatus == TypesProduct.EstatusActivo).Count() == 0)
                    {
                        tSubcategoria tSubcategoria = context.tSubcategorias.FirstOrDefault(p => p.idSubcategoria == idSubcategory);
                        
                        context.tSubcategorias.Remove(tSubcategoria);

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
                    tSubcategoria tSubCategoria = context.tSubcategorias.FirstOrDefault(p => p.idSubcategoria == id);

                    tSubCategoria.Imagen = image;

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