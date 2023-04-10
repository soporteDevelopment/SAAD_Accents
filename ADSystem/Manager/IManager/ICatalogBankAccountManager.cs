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
    /// ICatalogBankAccountManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tCatalogoCuentaBancos, ADEntities.ViewModels.CatalogBankAccountViewModel&gt;" />
    public interface ICatalogBankAccountManager : IBaseManager<tCatalogoCuentaBanco, CatalogBankAccountViewModel>
    {
    }
}
