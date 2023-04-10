using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// IProviderRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository{ADEntities.Models.tCatalogoProveedor}" />
    public interface ICatalogProviderRepository : IBaseRepository<tCatalogoProveedor>
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        int GetCount(CatalogProviderFilterViewModel filter);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<tCatalogoProveedor> GetAll(CatalogProviderFilterViewModel filter, int page, int pageSize);
        /// <summary>
        /// Gets the actives.
        /// </summary>
        /// <returns></returns>
        List<tCatalogoProveedor> GetActives();
        void Delete(int id, int idUser);
    }
}
