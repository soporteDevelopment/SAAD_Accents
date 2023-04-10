using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ADEntities.Queries.Repository
{       /// <summary>
        /// EstatusTicketRepository
        /// </summary>
        /// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tOrigenCliente&gt;" />
        /// <seealso cref="ADEntities.Queries.IRepository.IBranchOfficeRepository" />
    public class BranchOfficeRepository : BaseRepository<tSucursale>, IBranchOfficeRepository
    {
        private readonly admDB_SAADDBEntities context;

		/// <summary>
		/// Initializes a new instance of the <see cref="BranchOfficeRepository"/> class.
		/// </summary>
		/// <param name="_context">The context.</param>
		public BranchOfficeRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
		/// <summary>
		/// Gets the detail by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public List<BranchOfficeViewModel> GetDetail()
		{
			return (from sucursale in context.tSucursales
					select new BranchOfficeViewModel()
					{
						idSucursal = sucursale.idSucursal,
						Nombre = sucursale.Nombre,
						Descripcion = sucursale.Descripcion,
						
					}).ToList();
		}
	}
}