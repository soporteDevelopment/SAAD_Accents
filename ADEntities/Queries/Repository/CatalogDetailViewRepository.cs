using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// CatalogDetailViewRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository{ADEntities.Models.tCatalogoDetalleVista}" />
    /// <seealso cref="ADEntities.Queries.IRepository.ICatalogDetailViewRepository" />
    public class CatalogDetailViewRepository : BaseRepository<tCatalogoDetalleVista>, ICatalogDetailViewRepository
    {
        private readonly admDB_SAADDBEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public CatalogDetailViewRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
        /// <summary>
        /// Gets the by catalog view identifier.
        /// </summary>
        /// <param name="idView">The identifier view.</param>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <returns></returns>
        public tCatalogoDetalleVista GetByIdView(int idView, int idCatalog)
        {
            return context.tCatalogoDetalleVistas.Where(p => p.idVista == idView && p.idCatalogo == idCatalog).FirstOrDefault();
        }
        /// <summary>
        /// Gets the by identifier view.
        /// </summary>
        /// <param name="idView">The identifier view.</param>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <returns></returns>
        public List<tCatalogoDetalleVista> GetByIdView(int idView)
        {
            return context.tCatalogoDetalleVistas.Where(p => p.idVista == idView).ToList();
        }



        /// <summary>
        /// Patches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override tCatalogoDetalleVista Update(tCatalogoDetalleVista entity)
        {
            var catalog = context.tCatalogoDetalleVistas.Find(entity.idDetalleVista);

            catalog.Devolucion = entity.Devolucion;
            catalog.Estatus = entity.Estatus;

            context.SaveChanges();

            return catalog;
        }
    }
}