using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// ICatalogBrandRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository&lt;ADEntities.Models.tCatalogoMarcas&gt;" />
    public interface ICatalogBrandRepository : IBaseRepository<tCatalogoMarca>
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
        List<tCatalogoMarca> GetActives();
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
        List<tCatalogoMarca> GetAll(CatalogBrandFilterViewModel filter, int page, int pageSize);
        
    }
}