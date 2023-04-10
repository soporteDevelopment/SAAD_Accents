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
    /// ICatalogDetailViewManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager{ADEntities.Models.tCatalogoDetalleVista, ADEntities.ViewModels.CatalogDetailViewViewModel}" />
    public interface ICatalogDetailViewManager : IBaseManager<tCatalogoDetalleVista, CatalogDetailViewViewModel>
    {
        /// <summary>
        /// Posts the range.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="details">The details.</param>
        void PostRange(int id, List<CatalogDetailViewViewModel> details);
        /// <summary>
        /// Gets the by catalog view identifier.
        /// </summary>
        /// <param name="idView">The identifier view.</param>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <returns></returns>
        CatalogDetailViewViewModel GetByCatalogViewId(int idView, int idCatalog);
        /// <summary>
        /// Gets the by catalog view identifier.
        /// </summary>
        /// <param name="idView">The identifier view.</param>
        /// <returns></returns>
        List<CatalogDetailViewViewModel> GetByCatalogViewId(int idView);
    }
}
