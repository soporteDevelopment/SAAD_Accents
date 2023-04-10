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
    /// TipoTicketManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager&lt;ADEntities.Models.tTipoTicket, ADEntities.ViewModels.TypeTicketViewModel&gt;" />
    /// <seealso cref="ADSystem.Manager.IManager.ITypeTicketManager" />
    public class TypeTicketManager : BaseManager<tTipoTicket, TypeTicketViewModel>, ITypeTicketManager
    {
        private ITypeTicketRepository repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeTicketManager"/> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public TypeTicketManager(ITypeTicketRepository _repository) : base(_repository)
        {
            repository = _repository;
        }
        public override TypeTicketViewModel PrepareSingleReturn(tTipoTicket entity)
        {
            return new TypeTicketViewModel()
            {
                idTypeTicket = entity.idTipoTicket,
                Name = entity.Nombre
            };
        }
        public override List<TypeTicketViewModel> PrepareMultipleReturn(List<tTipoTicket> entities)
        {
            return entities.Select(p => new TypeTicketViewModel()
            {
                idTypeTicket = p.idTipoTicket,
                Name = p.Nombre
            }).ToList();
        }
    }
}