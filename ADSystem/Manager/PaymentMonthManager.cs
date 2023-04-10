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
    /// CatalogPaymentMonthManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tPagoMes, ADEntities.ViewModels.PaymentMothViewModel}" />
    /// <seealso cref="ADSystem.Manager.IManager.IPaymentMonthManager" />
    public class PaymentMonthManager : BaseManager<tPagoMes, PaymentMothViewModel>, IPaymentMonthManager
    {
        private IPaymentMonthRepository repository;

        private const bool FalseValue = false;
        private const bool TrueValue = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryManager"/> class.
        /// </summary>
        public PaymentMonthManager(IPaymentMonthRepository _repository) : base(_repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// Prepares the multiple return.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public override List<PaymentMothViewModel> PrepareMultipleReturn(List<tPagoMes> entities)
        {
            return entities.Select(p => new PaymentMothViewModel()
            {
                idPagoMeses = p.idPagoMeses,
                Meses = p.Meses,
                CreadoPor = p.CreadoPor,
                Creado = p.Creado,
                ModificadoPor = p.ModificadoPor,
                Modificado = p.Modificado,
                Activo = p.Activo
            }).ToList();
        }
    }
}