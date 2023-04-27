using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// CustomerOriginRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tOrigenCliente&gt;" />
    /// <seealso cref="ADEntities.Queries.IRepository.ICustomerOriginRepository" />
    public class CustomerOriginRepository : BaseRepository<tOrigenCliente>, ICustomerOriginRepository
    {
        private readonly admDB_SAADDBEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOriginRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public CustomerOriginRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
    }
}