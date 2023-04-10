using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// CatalogPaymentMonthRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository{ADEntities.Models.tPagoMes}" />
    /// <seealso cref="IPaymentMonthRepository" />
    public class PaymentMonthRepository : BaseRepository<tPagoMes>, IPaymentMonthRepository
    {
        private readonly admDB_SAADDBEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public PaymentMonthRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
    }
}