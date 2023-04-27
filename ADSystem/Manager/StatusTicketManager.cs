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
    /// StatusTicketManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tEstatusTicket, ADEntities.ViewModels.StatusTicketViewModel};" />
    /// <seealso cref="ADSystem.Manager.IManager.IStatusTicketManager" />
    public class StatusTicketManager : BaseManager<tEstatusTicket, StatusTicketViewModel>, IStatusTicketManager
    {
        private IStatusTicketRepository repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusTicketManager"/> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public StatusTicketManager(IStatusTicketRepository _repository) : base(_repository)
        {
            repository = _repository;
        }
        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override StatusTicketViewModel PrepareSingleReturn(tEstatusTicket entity)
        {
            return new StatusTicketViewModel()
            {
                idStatusTicket = entity.idEstatusTicket,
                Name = entity.Nombre
            };
        }
        public override List<StatusTicketViewModel> PrepareMultipleReturn(List<tEstatusTicket> entities)
        {
            return entities.Select(p=> new StatusTicketViewModel()
            {
                idStatusTicket = p.idEstatusTicket,
                Name = p.Nombre
            }).ToList();
        }
    }
}