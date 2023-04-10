using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
	/// <summary>
	/// SubcategoryRepository
	/// </summary>
	/// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tSubcategoria&gt;" />
	/// <seealso cref="ADEntities.Queries.IRepository.ISubcategoryRepository" />
	public class SubcategoryRepository : BaseRepository<tSubcategoria>, ISubcategoryRepository
	{
		private readonly admDB_SAADDBEntities context;

		/// <summary>
		/// Initializes a new instance of the <see cref="SubcategoryRepository" /> class.
		/// </summary>
		/// <param name="_context">The context.</param>
		public SubcategoryRepository(admDB_SAADDBEntities _context) : base(_context)
		{
			context = _context;
		}
	}
}