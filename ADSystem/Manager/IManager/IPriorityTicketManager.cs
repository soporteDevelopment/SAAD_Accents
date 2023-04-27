using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// IPrioridadTicketManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tPrioridadTicket, ADEntities.ViewModels.PriorityTicketViewModel&gt;" />
    public interface IPriorityTicketManager : IBaseManager<tPrioridadTicket, PriorityTicketViewModel>
    {
       
    }
}
