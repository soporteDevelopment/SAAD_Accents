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
    /// <seealso cref="ADSystem.Manager.BaseManager{tCatalogoDetalleVista, CatalogDetailViewViewModel}" />
    /// <seealso cref="ICatalogDetailViewManager" />
    public class CatalogDetailDevolutionManager : BaseManager<tCatalogoDetalleDevolucion, CatalogDetailDevolutionViewModel>, ICatalogDetailDevolutionManager
    {
        private ICatalogDetailDevolutionRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryManager"/> class.
        /// </summary>
        public CatalogDetailDevolutionManager(ICatalogDetailDevolutionRepository _repository) : base(_repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="category">The category.</param>
        public void Update(int id, CatalogDetailDevolutionViewModel catalog)
        {
            var entity = repository.GetById(id);

            repository.Update(this.PrepareUpdateData(entity, catalog));
        }

        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override tCatalogoDetalleDevolucion PrepareAddData(CatalogDetailDevolutionViewModel viewModel)
        {
            return new tCatalogoDetalleDevolucion()
            {
                idDetalleVista = viewModel.idDetalleVista,
                Devolucion = viewModel.Devolucion,
                Fecha = viewModel.Fecha,
                idVerificador = viewModel.idVerificador,
                Comentarios = viewModel.Comentarios
            };
        }

        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override CatalogDetailDevolutionViewModel PrepareSingleReturn(tCatalogoDetalleDevolucion entity)
        {
            return new CatalogDetailDevolutionViewModel()
            {
                idDetalleDevoluciones = entity.idDetalleDevoluciones,
                idDetalleVista = entity.idDetalleVista,
                Devolucion = entity.Devolucion,
                Fecha = entity.Fecha,
                idVerificador = entity.idVerificador,
                Comentarios = entity.Comentarios
            };
        }
    }
}