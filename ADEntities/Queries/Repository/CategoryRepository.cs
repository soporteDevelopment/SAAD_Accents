using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// CategoryRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tCategoria&gt;" />
    /// <seealso cref="ICategoryRepository" />
    public class CategoryRepository : BaseRepository<tCategoria>, ICategoryRepository
    {
        private readonly admDB_SAADDBEntities context;

		/// <summary>
		/// Initializes a new instance of the <see cref="BranchRepository" /> class.
		/// </summary>
		/// <param name="_context">The context.</param>
		public CategoryRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
    }
}