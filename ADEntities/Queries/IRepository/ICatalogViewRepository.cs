using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// ICatalogViewRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository{ADEntities.Models.tCatalogoVista}" />
    public interface ICatalogViewRepository : IBaseRepository<tCatalogoVista>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ICatalogViewRepository"/> interface.
        /// </summary>
        /// <param name="filter">The filter.</param>
        int GetCount(CatalogViewFilterViewModel filter);
        /// <summary>
        /// Initializes a new instance of the <see cref="ICatalogViewRepository"/> interface.
        /// </summary>
        /// <param name="filter">The filter.</param>
        List<tCatalogoVista> GetAll(CatalogViewFilterViewModel filter);
        /// <summary>
        /// Gets the last identifier.
        /// </summary>
        /// <returns></returns>
        int GetLastID();
    }
}
