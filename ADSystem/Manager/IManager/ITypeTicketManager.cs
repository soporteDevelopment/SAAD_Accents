using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// ITipoTicketManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tTipoTicket, ADEntities.ViewModels.TypeTicketViewModel&gt;" />
    public interface ITypeTicketManager : IBaseManager<tTipoTicket, TypeTicketViewModel>
    {
    }
}