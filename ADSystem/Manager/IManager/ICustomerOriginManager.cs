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
    /// ISaleOriginManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tOrigenVenta, ADEntities.ViewModels.SaleOriginViewModel&gt;" />
    public interface ICustomerOriginManager : IBaseManager<tOrigenCliente, CustomerOriginViewModel>
    {
    }
}
