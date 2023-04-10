using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace ADSystem.Manager
{
	/// <summary>
	/// StatusTicketManager
	/// </summary>
	/// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tCatalogoMarcas, ADEntities.ViewModels.CatalogBrandViewModel};" />
	/// <seealso cref="ADSystem.Manager.IManager.IStatusTicketManager" />
	public class CatalogBrandManager : BaseManager<tCatalogoMarca, CatalogBrandViewModel>, ICatalogBrandManager
	{
		private ICatalogBrandRepository repository;
		/// <summary>
		/// Initializes a new instance of the <see cref="StatusTicketManager"/> class.
		/// </summary>
		/// <param name="_repository">The repository.</param>
		public CatalogBrandManager(ICatalogBrandRepository _repository) : base(_repository)
		{
			repository = _repository;
		}
		/// <summary>
		/// Gets the detail by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public CatalogBrandViewModel GetDetailById(int idCatalogBrand)
		{
			return repository.GetDetailById(idCatalogBrand);
		}
		/// <summary>
		/// Gets all by brand.
		/// </summary>
		/// <param name="idCatalogBrand">The identifier catalog brand.</param>
		/// <returns></returns>
		public List<CatalogBrandViewModel> GetAllByBrand(int idCatalogBrand)
		{
			return repository.GetAllByBrand(idCatalogBrand);
		}
		/// <summary>
		/// Gets the actives.
		/// </summary>
		/// <returns></returns>
		public List<CatalogBrandViewModel> GetActives()
		{
			return PrepareMultipleReturn(repository.GetActives());
		}
		public override tCatalogoMarca PrepareAddData(CatalogBrandViewModel viewModel)
		{
			return new tCatalogoMarca()
			{
				IdCatalogoMarca = viewModel.idCatalogBrand,
				Nombre = viewModel.Name,
				Descripcion = viewModel.Description,
				idProveedor = viewModel.idProvider,
				Activo = true,
				Creado = DateTime.Now,
				CreadoPor = viewModel.CreatedBy
			};
		}
		public override List<CatalogBrandViewModel> PrepareMultipleReturn(List<tCatalogoMarca> entities)
		{
			return entities.Select(p => new CatalogBrandViewModel()
			{
				idCatalogBrand = p.IdCatalogoMarca,
				Name = p.Nombre,
				idProvider = (int)p.idProveedor,

			}).ToList();
		}
		public override CatalogBrandViewModel PrepareSingleReturn(tCatalogoMarca entity)
		{
			return new CatalogBrandViewModel()
			{
				idCatalogBrand = entity.IdCatalogoMarca,
				Name = entity.Nombre,
				Description = entity.Descripcion,
				idProvider = (int)entity.idProveedor
			};
		}
		/// <summary>
		/// Prepares the update data.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public override tCatalogoMarca PrepareUpdateData(tCatalogoMarca entity, CatalogBrandViewModel viewModel)
		{
			entity.IdCatalogoMarca = viewModel.idCatalogBrand;
			entity.Nombre = viewModel.Name;
			entity.Modificado = viewModel.Modified;
			entity.ModificadoPor = viewModel.ModifiedBy;
			entity.Descripcion = viewModel.Description;
			entity.idProveedor = viewModel.idProvider;


			return entity;
		}
		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		public int GetCount(CatalogBrandFilterViewModel filter)
		{
			return repository.GetCount(filter);
		}
		/// <summary>
		/// Gets all.
		/// </summary>
		/// <param name="filter">The filter.</param>
		public List<CatalogBrandViewModel> GetAll(CatalogBrandFilterViewModel filter, int page, int pageSize)
		{
			return PrepareMultipleReturn(repository.GetAll(filter, page, pageSize));
		}
		public void Delete(int id, int idUser)
		{
			repository.Delete(id);
		}
		/// <summary>
		/// Deactives the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Deactive(int id)
		{
			var catalogBrand = repository.GetById(id);
			catalogBrand.Activo = false;

			repository.Update(catalogBrand);
		}
	}
}