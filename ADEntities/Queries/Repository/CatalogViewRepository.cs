using ADEntities.Enums;
using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// CatalogViewRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository{ADEntities.Models.tCatalogoVista}" />
    /// <seealso cref="ADEntities.Queries.IRepository.ICatalogViewRepository" />
    public class CatalogViewRepository : BaseRepository<tCatalogoVista>, ICatalogViewRepository
    {
        private readonly admDB_SAADDBEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public CatalogViewRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ICatalogViewRepository" /> interface.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public int GetCount(CatalogViewFilterViewModel filter)
        {
            DateTime startDate = Convert.ToDateTime(filter.DateSince);
            DateTime endDate = Convert.ToDateTime(filter.DateSince);

            return context.tCatalogoVistas.Where(p => (p.Fecha >= startDate && p.Fecha <= endDate)
            //&& (p.Numero.Contains(filter.Number) || String.IsNullOrEmpty(filter.Number))
            && (p.Estatus == filter.Status)).Count();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<tCatalogoVista> GetAll(CatalogViewFilterViewModel filter)
        {
            DateTime startDate = Convert.ToDateTime(filter.DateSince).Date + new TimeSpan(0, 0, 0);
            DateTime endDate = Convert.ToDateTime(filter.DateUntil).Date + new TimeSpan(23, 59, 59);

            return context.tCatalogoVistas.Where(p => (p.Fecha >= startDate && p.Fecha <= endDate)
            && (p.Numero.Contains(filter.Number) || String.IsNullOrEmpty(filter.Number))
            && (p.Estatus == filter.Status)).OrderByDescending(p => p.Fecha).Skip(filter.Page * filter.PageSize).Take(filter.PageSize).ToList();
        }

        /// <summary>
        /// Gets the last identifier.
        /// </summary>
        /// <returns></returns>
        public int GetLastID()
        {            
            if (context.tCatalogoVistas.Count() > 0)
            {
                return context.tCatalogoVistas.Max(p => p.idVista);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override tCatalogoVista Update(tCatalogoVista entity)
        {
            var catalog = context.tCatalogoVistas.Find(entity.idVista);

            catalog.Estatus = entity.Estatus;
            catalog.ModificadoPor = entity.ModificadoPor;
            catalog.Modificado = entity.Modificado;

            context.SaveChanges();

            return catalog;
        }
    }
}