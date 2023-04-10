using ADEntities.Models;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    public class TypeTicketRepository : BaseRepository<tTipoTicket>, ITypeTicketRepository
    {
        private readonly admDB_SAADDBEntities context;

        public TypeTicketRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;

        }

    }
}