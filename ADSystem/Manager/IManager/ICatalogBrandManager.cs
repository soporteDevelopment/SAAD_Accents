using ADEntities.Models;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tCatalogoMarcas, ADEntities.ViewModels.CatalogBrandViewModel&gt;" />
    public interface ICatalogBrandManager : IBaseManager<tCatalogoMarca, CatalogBrandViewModel>
    {
        /// <summary>
        /// Gets all by brand.
        /// </summary>
        /// <param name="idCatalogBrand">The identifier catalog brand.</param>
        /// <returns></returns>
        List<CatalogBrandViewModel> GetAllByBrand(int idCatalogBrand);
        /// <summary>
        /// Gets the detail by identifier.
        /// </summary>
        /// <param name="idCatalogBrand">The identifier catalog brand.</param>
        /// <returns></returns>
        CatalogBrandViewModel GetDetailById(int idCatalogBrand);
        /// <summary>
        /// Gets the actives.
        /// </summary>
        /// <returns></returns>
        List<CatalogBrandViewModel> GetActives();
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        int GetCount(CatalogBrandFilterViewModel filter);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<CatalogBrandViewModel> GetAll(CatalogBrandFilterViewModel filter, int page, int pageSize);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idUser">The identifier user.</param>
        void Delete(int id, int idUser);
        /// <summary>
        /// Deactives the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Deactive(int id);

    }
}