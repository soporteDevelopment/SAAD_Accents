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
	/// CategoryManager
	/// </summary>
	/// <seealso cref="ADSystem.Manager.BaseManager&lt;ADEntities.Models.tCategoria, ADEntities.ViewModels.CategoryViewModel&gt;" />
	/// <seealso cref="ADSystem.Manager.IManager.ICategoryManager" />
	public class CategoryManager : BaseManager<tCategoria, CategoryViewModel>, ICategoryManager
	{
		private ICategoryRepository repository;

		/// <summary>
		/// Initializes a new instance of the <see cref="CategoryManager"/> class.
		/// </summary>
		/// <param name="_repository">The repository.</param>
		public CategoryManager(ICategoryRepository _repository) : base(_repository)
		{
			repository = _repository;
		}

		/// <summary>
		/// Prepares the add data.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public override tCategoria PrepareAddData(CategoryViewModel viewModel)
		{
			return new tCategoria()
			{
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
		public override tCategoria PrepareUpdateData(tCategoria entity, CategoryViewModel viewModel)
		{
			entity.Nombre = viewModel.Nombre;
			entity.Descripción = viewModel.Descripcion;

			return entity;
		}

		/// <summary>
		/// Prepares the single return.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public override CategoryViewModel PrepareSingleReturn(tCategoria entity)
		{
			return new CategoryViewModel()
			{
				idCategoria = entity.idCategoria,
				Nombre = entity.Nombre,
				Descripcion = entity.Descripción,
				Imagen = entity.Imagen
			};
		}

		public override List<CategoryViewModel> PrepareMultipleReturn(List<tCategoria> entities)
		{
			return entities.Select(p => new CategoryViewModel()
			{
				idCategoria = p.idCategoria,
				Nombre = p.Nombre,
				Descripcion = p.Descripción,
				Imagen = p.Imagen
			}).ToList();
		}
	}
}