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
    /// CatalogDetailDevolutionRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository{tCatalogoCuentaBancos}" />
    /// <seealso cref="ICatalogBankAccountRepository" />
    public class CatalogBankAccountRepository : BaseRepository<tCatalogoCuentaBanco>, ICatalogBankAccountRepository
    {
        private readonly admDB_SAADDBEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public CatalogBankAccountRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
        
    }
}