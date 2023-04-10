using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// ICatalogDetailViewRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository{ADEntities.Models.tCatalogoDetalleVista}" />
    public interface ICatalogDetailViewRepository : IBaseRepository<tCatalogoDetalleVista>
    {
        /// <summary>
        /// Gets the by catalog view identifier.
        /// </summary>
        /// <param name="idCatalogView">The identifier catalog view.</param>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <returns></returns>
        tCatalogoDetalleVista GetByIdView(int idView, int idCatalog);
        /// <summary>
        /// Gets the by identifier view.
        /// </summary>
        /// <param name="idView">The identifier view.</param>
        /// <returns></returns>
        List<tCatalogoDetalleVista> GetByIdView(int idView);
    }
}
