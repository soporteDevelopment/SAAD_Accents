using ADEntities.Enums;
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
    /// CatalogDetailViewManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tCatalogoDetalleVista, ADEntities.ViewModels.CatalogDetailViewViewModel}" />
    /// <seealso cref="ADSystem.Manager.IManager.ICatalogDetailViewManager" />
    public class CatalogDetailViewManager : BaseManager<tCatalogoDetalleVista, CatalogDetailViewViewModel>, ICatalogDetailViewManager
    {
        private ICatalogDetailViewRepository repository;

        private const bool FalseValue = false;
        private const bool TrueValue = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryManager"/> class.
        /// </summary>
        public CatalogDetailViewManager(ICatalogDetailViewRepository _repository) : base(_repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// Posts the range.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="details">The details.</param>
        public void PostRange(int id, List<CatalogDetailViewViewModel> details)
        {
            foreach (var detail in details)
            {
                detail.idVista = id;
                repository.Add(this.PrepareAddData(detail));
            }
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="category">The category.</param>
        public void Update(int id, CatalogDetailViewViewModel detail)
        {
            var entity = repository.GetById(id);

            repository.Update(this.PrepareUpdateData(entity, detail));
        }

        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override tCatalogoDetalleVista PrepareAddData(CatalogDetailViewViewModel viewModel)
        {
            return new tCatalogoDetalleVista()
            {
                idVista = viewModel.idVista,
                idCatalogo = viewModel.idCatalogo,
                Precio = viewModel.Precio,
                Cantidad = viewModel.Cantidad,
                Devolucion = 0,
                idSucursal = viewModel.idSucursal,
                Comentarios = viewModel.Comentarios,
                Estatus = (short)StatusCatalogView.Pending
            };
        }

        /// <summary>
        /// Gets the by catalog view identifier.
        /// </summary>
        /// <param name="idView">The identifier view.</param>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <returns></returns>
        public CatalogDetailViewViewModel GetByCatalogViewId(int idView, int idCatalog)
        {
            var entity = repository.GetByIdView(idView, idCatalog);

            return PrepareSingleReturn(entity);
        }

        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override List<CatalogDetailViewViewModel> PrepareMultipleReturn(List<tCatalogoDetalleVista> entities)
        {
            return entities.Select(p => new CatalogDetailViewViewModel()
            {
                idDetalleVista = p.idDetalleVista,
                idVista = p.idVista,
                idCatalogo = p.idCatalogo,
                Precio = p.Precio,
                Cantidad = p.Cantidad,
                Devolucion = p.Devolucion,
                idSucursal = p.idSucursal,
                Comentarios = p.Comentarios,
                Estatus = p.Estatus
            }).ToList();
        }

        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override CatalogDetailViewViewModel PrepareSingleReturn(tCatalogoDetalleVista entity)
        {
            return new CatalogDetailViewViewModel()
            {
                idDetalleVista = entity.idDetalleVista,
                idVista = entity.idVista,
                idCatalogo = entity.idCatalogo,
                Precio = entity.Precio,
                Cantidad = entity.Cantidad,
                Devolucion = entity.Devolucion,
                idSucursal = entity.idSucursal,
                Comentarios = entity.Comentarios,
                Estatus = entity.Estatus
            };
        }

        /// <summary>
        /// Prepares the update data.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public override tCatalogoDetalleVista PrepareUpdateData(tCatalogoDetalleVista entity, CatalogDetailViewViewModel viewModel)
        {
            entity.idDetalleVista = viewModel.idDetalleVista;
            entity.idVista = viewModel.idVista;
            entity.idCatalogo = viewModel.idCatalogo;
            entity.Precio = viewModel.Precio;
            entity.Cantidad = viewModel.Cantidad;
            entity.Devolucion = viewModel.Devolucion;
            entity.idSucursal = viewModel.idSucursal;
            entity.Comentarios = viewModel.Comentarios;
            entity.Estatus = viewModel.Estatus;

            return entity;
        }

        /// <summary>
        /// Gets the by catalog view identifier.
        /// </summary>
        /// <param name="idView">The identifier view.</param>
        /// <returns></returns>
        public List<CatalogDetailViewViewModel> GetByCatalogViewId(int idView)
        {
            var entities = repository.GetByIdView(idView);

            return PrepareMultipleReturn(entities);
        }
    }
}