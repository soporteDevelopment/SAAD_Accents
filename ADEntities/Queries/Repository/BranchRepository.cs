using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// BranchRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository{ADEntities.Models.tSucursale}" />
    /// <seealso cref="IBranchRepository" />
    public class BranchRepository : BaseRepository<tSucursale>, IBranchRepository
    {
        private readonly admDB_SAADDBEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public BranchRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
    }
}