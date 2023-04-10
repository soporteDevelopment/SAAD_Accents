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
    /// CustomerOriginManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tSucursale, ADEntities.ViewModels.CustomerOriginStoreViewModel};" />
    /// <seealso cref="ADSystem.Manager.IManager.ICustomerOriginManager" />
    public class CustomerOriginManager : BaseManager<tOrigenCliente, CustomerOriginViewModel>, ICustomerOriginManager
    {

        private ICustomerOriginRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOriginManager"/> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public CustomerOriginManager(ICustomerOriginRepository _repository) : base(_repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override CustomerOriginViewModel PrepareSingleReturn(tOrigenCliente entity)
        {
            return new CustomerOriginViewModel()
            {
                idOrigen = entity.idOrigen,
                Origen = entity.Origen,
                Activo = entity.Activo
            };
        }

        /// <summary>
        /// Prepares the multiple return.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public override List<CustomerOriginViewModel> PrepareMultipleReturn(List<tOrigenCliente> entities)
        {
            return entities.Select(p => new CustomerOriginViewModel()
            {
                idOrigen = p.idOrigen,
                Origen = p.Origen,
                Activo = p.Activo
            }).ToList();
        }
        
    }
}