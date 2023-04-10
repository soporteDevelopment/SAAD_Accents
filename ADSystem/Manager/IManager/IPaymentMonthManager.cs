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
    /// ICatalogPaymentMonthManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager{tCatalogoDetalleVista, CatalogDetailViewViewModel}" />
    public interface IPaymentMonthManager : IBaseManager<tPagoMes, PaymentMothViewModel>
    {
    }
}
