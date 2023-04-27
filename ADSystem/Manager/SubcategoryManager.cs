using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.Queries.Repository;
using ADEntities.ViewModels;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager
{
    /// <summary>
    /// SubcategoryManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager&lt;ADEntities.Models.tSubcategoria, ADEntities.ViewModels.SubcategoryViewModel&gt;" />
    /// <seealso cref="ADSystem.Manager.IManager.ISubcategoryManager" />
    public class SubcategoryManager : BaseManager<tSubcategoria, SubcategoryViewModel>, ISubcategoryManager
    {
        private ISubcategoryRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubcategoryManager"/> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public SubcategoryManager(ISubcategoryRepository _repository) : base(_repository)
        {
            repository = _repository;
        }

        /// <summary>
		/// Prepares the add data.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public override tSubcategoria PrepareAddData(SubcategoryViewModel viewModel)
        {
            return new tSubcategoria()
            {
                idCategoria = viewModel.idCategoria,
                Nombre = viewModel.Nombre,
                Descripción = viewModel.Descripcion
            };
        }

        /// <summary>
        /// Prepares the update data.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override tSubcategoria PrepareUpdateData(tSubcategoria entity, SubcategoryViewModel viewModel)
        {
            entity.idCategoria = viewModel.idCategoria;
            entity.Nombre = viewModel.Nombre;
            entity.Descripción = viewModel.Descripcion;

            return entity;
        }

        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override SubcategoryViewModel PrepareSingleReturn(tSubcategoria entity)
        {
            return new SubcategoryViewModel()
            {
                idSubcategoria = entity.idSubcategoria,
                idCategoria = entity.idCategoria,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripción,
                Imagen = entity.Imagen
            };
        }

        public override List<SubcategoryViewModel> PrepareMultipleReturn(List<tSubcategoria> entities)
        {
            return entities.Select(p => new SubcategoryViewModel()
            {
                idSubcategoria = p.idSubcategoria,
                idCategoria = p.idCategoria,
                Nombre = p.Nombre,
                Descripcion = p.Descripción,
                Imagen = p.Imagen
            }).ToList();
        }
    }
}