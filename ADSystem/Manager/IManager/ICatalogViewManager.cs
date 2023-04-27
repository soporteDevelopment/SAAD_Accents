using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// ICatalogViewManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager{ADEntities.Models.tCatalogoVista, ADEntities.ViewModels.CatalogViewViewModel}" />
    public interface ICatalogViewManager : IBaseManager<tCatalogoVista, CatalogViewViewModel>
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        int GetCount(CatalogViewFilterViewModel filter);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<CatalogViewViewModel> GetAll(CatalogViewFilterViewModel filter);
        /// <summary>
        /// Gets the previous number.
        /// </summary>
        /// <returns></returns>
        string GeneratePreviousNumber();
        /// <summary>
        /// Gets for print.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        CatalogViewPrintViewModel GetForPrint(int id);
        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="idUser">The identifier user.</param>
        void UpdateStatus(int id, short status, int idUser);
        /// <summary>
        /// Posts the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="catalogs">The catalogs.</param>
        /// <param name="idUser">The identifier user.</param>
        /// <returns></returns>
        CatalogViewViewModel Return(int id, int[][] catalogs, int idUser);
    }
}