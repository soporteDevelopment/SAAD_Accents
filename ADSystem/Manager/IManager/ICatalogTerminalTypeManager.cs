using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// ICatalogTerminalTypeManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tCatalogoTipoTerminales, ADEntities.ViewModels.CatalogTerminalTypeViewModel&gt;" />
    public interface ICatalogTerminalTypeManager : IBaseManager<tCatalogoTipoTerminale, CatalogTerminalTypeViewModel>
    {

    }
    
}
