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
    /// CatalogDetailDevolutionManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{tCatalogoCuentaBancos}" />
    public class CatalogBankAccountManager : BaseManager<tCatalogoCuentaBanco, CatalogBankAccountViewModel>, ICatalogBankAccountManager
    {
        private ICatalogBankAccountRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryManager"/> class.
        /// </summary>
        public CatalogBankAccountManager(ICatalogBankAccountRepository _repository) : base(_repository)
        {
            repository = _repository;
        }
        public override List<CatalogBankAccountViewModel> PrepareMultipleReturn(List<tCatalogoCuentaBanco> entities)
        {
            return entities.Select(p => new CatalogBankAccountViewModel()
            {
                idCatalogBankAccount = p.IdCatalogoCuentaBanco,
                Name = p.Nombre
            }).ToList();
        }

    }
}