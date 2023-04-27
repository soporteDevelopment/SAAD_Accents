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
    /// <seealso cref="ADSystem.Manager.BaseManager{tCatalogoTipoTerminales}" />
    public class CatalogTerminalTypeManager : BaseManager<tCatalogoTipoTerminale, CatalogTerminalTypeViewModel>, ICatalogTerminalTypeManager
    {
        private ICatalogTerminalTypeRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryManager"/> class.
        /// </summary>
        public CatalogTerminalTypeManager(ICatalogTerminalTypeRepository _repository) : base(_repository)
        {
            repository = _repository;
        }
        public override List<CatalogTerminalTypeViewModel> PrepareMultipleReturn(List<tCatalogoTipoTerminale> entities)
        {
            return entities.Select(p => new CatalogTerminalTypeViewModel()
            {
                idCatalogTerminalType = p.IdCatalogoTipoTerminal,
                Name = p.Nombre
            }).ToList();
        }
    }
}