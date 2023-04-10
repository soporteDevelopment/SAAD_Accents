using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// PrioridadTicketRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tOrigenCliente&gt;" />
    /// <seealso cref="ADEntities.Queries.IRepository.IPriorityTicketRepository" />
    public class PriorityTicketRepository : BaseRepository<tPrioridadTicket>, IPriorityTicketRepository
    {
        private readonly admDB_SAADDBEntities context;

        public PriorityTicketRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;

        }

    }
}