using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager
{
    /// <summary>
    /// PrioridadTicketManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tEstatusTicket, ADEntities.ViewModels.PriorityTicketViewModel};" />
    /// <seealso cref="ADSystem.Manager.IManager.IStatusTicketManager" />
    public class PriorityTicketManager : BaseManager<tPrioridadTicket, PriorityTicketViewModel>, IPriorityTicketManager
    {
        private IPriorityTicketRepository repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityTicketManager"/> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public PriorityTicketManager(IPriorityTicketRepository _repository) : base(_repository)
        {
            repository = _repository;
        }
        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override PriorityTicketViewModel PrepareSingleReturn(tPrioridadTicket entity)
        {
            return new PriorityTicketViewModel()
            {
                idPriorityTicket = entity.idPrioridadTicket,
                Name = entity.Nombre
            };
        }
        public override List<PriorityTicketViewModel> PrepareMultipleReturn(List<tPrioridadTicket> entities)
        {
            return entities.Select(p => new PriorityTicketViewModel()
            {
                idPriorityTicket = p.idPrioridadTicket,
                Name = p.Nombre
            }).ToList();
        }
    }
}