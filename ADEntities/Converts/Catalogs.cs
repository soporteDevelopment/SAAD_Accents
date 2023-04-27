using ADEntities.Models;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ADEntities.Converts
{
    /// <summary>
    /// Catalogs Convert
    /// </summary>
    public class Catalogs
    {
        /// <summary>
        /// Prepares the add.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static tCatalogo PrepareAdd(CatalogViewModel entity)
        {
            tCatalogo result = new tCatalogo();

            result.Modelo = entity.Modelo;
            result.Volumen = entity.Volumen;
            result.Precio = entity.Precio;
            result.idCategoria = entity.idCategoria;
            result.idSubcategoria = entity.idSubcategoria;
            result.idProveedor = entity.idProveedor;
            result.idCatalogoMarca = entity.idCatalogoMarca;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;
            result.Activo = true;

            return result;
        }

        /// <summary>
        /// Prepares the update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static tCatalogo PrepareUpdate(CatalogViewModel entity)
        {
            tCatalogo result = new tCatalogo();

            result.idCatalogo = entity.idCatalogo;
            result.Modelo = entity.Modelo;
            result.Volumen = entity.Volumen;
            result.Precio = entity.Precio;
            result.idCategoria = entity.idCategoria;
            result.idSubcategoria = entity.idSubcategoria;
            result.idCatalogoMarca = entity.idCatalogoMarca;
            result.idProveedor = entity.idProveedor;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;
            result.Activo = true;

            return result;
        }

        /// <summary>
        /// Tables to model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static CatalogViewModel TableToModel(tCatalogo entity)
        {
            CatalogViewModel result = new CatalogViewModel();

            result.idCatalogo = entity.idCatalogo;
            result.Modelo = entity.Modelo;
            result.Volumen = entity.Volumen;
            result.Precio = entity.Precio;
            result.idCategoria = entity.idCategoria;
            result.idSubcategoria = entity.idSubcategoria;
            result.idProveedor = entity.idProveedor;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;
            result.Activo = entity.Activo;

            return result;
        }

        /// <summary>
        /// Models to table.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static tCatalogo ModelToTable(CatalogViewModel model)
        {
            tCatalogo result = new tCatalogo();

            result.idCatalogo = model.idCatalogo;
            result.Modelo = model.Modelo;
            result.Volumen = model.Volumen;
            result.Precio = model.Precio;
            result.idCategoria = model.idCategoria;
            result.idSubcategoria = model.idSubcategoria;
            result.idProveedor = model.idProveedor;
            result.CreadoPor = model.CreadoPor;
            result.Creado = model.Creado;
            result.ModificadoPor = model.ModificadoPor;
            result.Modificado = model.Modificado;
            result.Activo = model.Activo;

            return result;
        }
    }
}