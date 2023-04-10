using ADEntities.Models;
using ADEntities.ViewModels;
using System.Collections.Generic;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// ITicketsRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository&lt;ADEntities.Models.tTicket&gt;" />
    public interface ITicketsRepository : IBaseRepository<tTicket>
    {
        /// <summary>
        /// Gets all by status.
        /// </summary>
        /// <param name="idStatusCredit">The identifier status credit.</param>
        /// <returns></returns>
        List<TicketViewModel> GetAllByStatus(int? idUsers, int idStatusCredit, int? idAssignedUser);
        /// <summary>
        /// Gets the detail by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TicketViewModel GetDetailById(int id);
        
    }
}