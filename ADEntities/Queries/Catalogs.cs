using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    /// <summary>
    /// Catalogs
    /// </summary>
    public class Catalogs
    {
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int LENGTH = 5;

        /// <summary>
        /// Gets the catalogs.
        /// </summary>
        /// <returns></returns>
        public List<CatalogViewModel> GetCatalogs()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogos.Select(p => new CatalogViewModel()
                    {

                        idCatalogo = p.idCatalogo,
                        Codigo = p.Codigo,
                        Modelo = p.Modelo,
                        Volumen = p.Volumen,
                        Precio = p.Precio,
                        idCategoria = p.idCategoria,
                        idSubcategoria = p.idSubcategoria,
                        Imagen = p.Imagen,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        Activo = p.Activo
                    }).OrderBy(p => p.Modelo).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Counts the registers.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="volumen">The volumen.</param>
        /// <param name="idProvider">The identifier provider.</param>
        /// <param name="idCategory">The identifier category.</param>
        /// <returns></returns>
        public int CountRegisters(string code, string model, string volumen, int? idProvider, int? idCategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogos.Where(p => (p.Codigo.Contains(code) || String.IsNullOrEmpty(code))
                                   && (p.Modelo.Contains(model) || String.IsNullOrEmpty(model))
                                   && (p.Volumen.Contains(volumen) || String.IsNullOrEmpty(volumen))
                                   && (p.idProveedor == idProvider || idProvider == null)
                                   && (p.idCategoria == idCategory || idCategory == null))
                        .Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Gets the catalogs.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="volumen">The volumen.</param>
        /// <param name="idProvider">The identifier provider.</param>
        /// <param name="idCategory">The identifier category.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public List<CatalogViewModel> GetCatalogs(string code, string model, string volumen, int? idProvider, int? idCategory, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var categories = context.tCatalogos
                        .Where(p => (p.Codigo.Contains(code) || String.IsNullOrEmpty(code))
                                   && (p.Modelo.Contains(model) || String.IsNullOrEmpty(model))
                                   && (p.Volumen.Contains(volumen) || String.IsNullOrEmpty(volumen))
                                   && (p.idProveedor == idProvider || idProvider == null)
                                   && (p.idCategoria == idCategory || idCategory == null))
                        .Select(p => new CatalogViewModel()
                        {
                            idCatalogo = p.idCatalogo,
                            Codigo = p.Codigo,
                            Modelo = p.Modelo,
                            Volumen = p.Volumen,
                            Precio = p.Precio,
                            idCategoria = p.idCategoria,
                            idSubcategoria = p.idSubcategoria,
                            Imagen = p.Imagen,
                            CreadoPor = p.CreadoPor,
                            Creado = p.Creado,
                            ModificadoPor = p.ModificadoPor,
                            Modificado = p.Modificado,
                            Activo = p.Activo
                        }).OrderBy(p => p.Modelo);

                    return categories.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Gets the catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public CatalogViewModel GetById(int id)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogos.Where(p => p.idCatalogo == id).Select(p => new CatalogViewModel()
                    {
                        idCatalogo = p.idCatalogo,
                        Codigo = p.Codigo,
                        Modelo = p.Modelo,
                        Volumen = p.Volumen,
                        Precio = p.Precio,
                        idProveedor = p.idProveedor,
                        idCategoria = p.idCategoria,
                        idCatalogoMarca = p.idCatalogoMarca,
                        idSubcategoria = p.idSubcategoria,
                        Imagen = p.Imagen,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        Activo = p.Activo,
                        Categoria = new CategoryViewModel()
                        {
                            idCategoria = p.tCatalogoCategoria.idCategoria,
                            Nombre = p.tCatalogoCategoria.Nombre,
                            Descripcion = p.tCatalogoCategoria.Descripcion,
                            Subcategoria = new SubcategoryViewModel()
                            {
                                idSubcategoria = p.tCatalogoSubcategoria.idSubcategoria,
                                Nombre = p.tCatalogoSubcategoria.Nombre,
                                Descripcion = p.tCatalogoSubcategoria.Descripcion
                            }
                        }
                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Gets the catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public CatalogViewModel GetByCode(string code)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogos.Where(p => p.Codigo.Equals(code)).Select(p => new CatalogViewModel()
                    {
                        idCatalogo = p.idCatalogo,
                        Codigo = p.Codigo,
                        Modelo = p.Modelo,
                        Volumen = p.Volumen,
                        Precio = p.Precio,
                        idProveedor = p.idProveedor,
                        idCatalogoMarca = p.idCatalogoMarca,
                        idCategoria = p.idCategoria,
                        idSubcategoria = p.idSubcategoria,
                        Imagen = p.Imagen,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                        Activo = p.Activo,
                        Categoria = new CategoryViewModel()
                        {
                            idCategoria = p.tCatalogoCategoria.idCategoria,
                            Nombre = p.tCatalogoCategoria.Nombre,
                            Descripcion = p.tCatalogoCategoria.Descripcion,
                            Subcategoria = new SubcategoryViewModel()
                            {
                                idSubcategoria = p.tCatalogoSubcategoria.idSubcategoria,
                                Nombre = p.tCatalogoSubcategoria.Nombre,
                                Descripcion = p.tCatalogoSubcategoria.Descripcion
                            }
                        }
                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Gets the codes.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public List<CatalogViewModel> GetCodes(string code)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogos.Where(p => p.Codigo.Contains(code)).Select(p => new CatalogViewModel()
                    {
                        idCatalogo = p.idCatalogo,
                        Codigo = p.Codigo,
                        Modelo = p.Modelo,
                        Volumen = p.Volumen,
                        Precio = p.Precio,
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Adds the catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        public int AddCatalog(tCatalogo catalog)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    catalog.Codigo = GenerateCode();

                    context.tCatalogos.Add(catalog);
                    context.SaveChanges();
                    iResult = catalog.idCatalogo;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        /// <summary>
        /// Updates the catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        public void UpdateCatalog(tCatalogo catalog)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCatalogo tCatalogs = context.tCatalogos.FirstOrDefault(p => p.idCatalogo == catalog.idCatalogo);

                    tCatalogs.Modelo = catalog.Modelo;
                    tCatalogs.Volumen = catalog.Volumen;
                    tCatalogs.Precio = catalog.Precio;
                    tCatalogs.idCategoria = catalog.idCategoria;
                    tCatalogs.idSubcategoria = catalog.idSubcategoria;
                    tCatalogs.idProveedor = catalog.idProveedor;
                    tCatalogs.idCatalogoMarca = catalog.idCatalogoMarca;
                    tCatalogs.ModificadoPor = catalog.ModificadoPor;
                    tCatalogs.Modificado = catalog.Modificado;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Updates the image.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="image">The image.</param>
        public void UpdateImage(int id, string image)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCatalogo tCatalogs = context.tCatalogos.FirstOrDefault(p => p.idCatalogo == id);

                    tCatalogs.Imagen = image;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <returns></returns>
        public string GenerateCode()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    Random random = new Random();
                    var code = new string(Enumerable.Repeat(CHARS, LENGTH).Select(s => s[random.Next(s.Length)]).ToArray());

                    if (ValidateCode(code))
                    {
                        GenerateCode();
                    }

                    return code;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <exception cref="admDB_SAADDBEntities"></exception>
        public bool ValidateCode(string code)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogos.Any(p => p.Codigo.Equals(code));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}