using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// CatalogDetailDevolutionRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository{tCatalogoTipoTerminales}" />
    /// <seealso cref="ICatalogTerminalTypeRepository" />
    public class CatalogTerminalTypeRepository : BaseRepository<tCatalogoTipoTerminale>, ICatalogTerminalTypeRepository
    {
        private readonly admDB_SAADDBEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public CatalogTerminalTypeRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
    }
}