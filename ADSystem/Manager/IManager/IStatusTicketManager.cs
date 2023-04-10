using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// IStatusTicketManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tEstatusTicket, ADEntities.ViewModels.StatusTicketViewModel&gt;" />
    public interface IStatusTicketManager : IBaseManager<tEstatusTicket,StatusTicketViewModel>
    {
    }
}