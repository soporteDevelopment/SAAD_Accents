using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// IProviderManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager{ADEntities.Models.tCatalogoProveedor, ADEntities.ViewModels.CatalogProviderViewModel}" />
    public interface ICatalogProviderManager : IBaseManager<tCatalogoProveedor, CatalogProviderViewModel>
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
        List<CatalogProviderViewModel> GetAll(CatalogProviderFilterViewModel filter, int page, int pageSize);
        /// <summary>
        /// Gets the actives.
        /// </summary>
        /// <returns></returns>
        List<CatalogProviderViewModel> GetActives();
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id, int idUser);
    }
}
