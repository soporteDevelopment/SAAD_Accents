using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// EstatusTicketRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tOrigenCliente&gt;" />
    /// <seealso cref="ADEntities.Queries.IRepository.IStatusTicketRepository" />
    public class StatusTicketRepository : BaseRepository<tEstatusTicket>, IStatusTicketRepository
    {
        private readonly admDB_SAADDBEntities context;

        public StatusTicketRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;

        }

    }
}