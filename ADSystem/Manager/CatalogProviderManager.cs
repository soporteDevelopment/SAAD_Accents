using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager
{
    /// <summary>
    /// ProviderManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tCatalogoProveedor, ADEntities.ViewModels.CatalogProviderViewModel}" />
    /// <seealso cref="ADSystem.Manager.IManager.ICatalogProviderManager" />
    public class CatalogProviderManager : BaseManager<tCatalogoProveedor, CatalogProviderViewModel>, ICatalogProviderManager
    {
        private ICatalogProviderRepository repository;
        private bool TRUE = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryManager"/> class.
        /// </summary>
        public CatalogProviderManager(ICatalogProviderRepository _repository) : base(_repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public int GetCount(CatalogProviderFilterViewModel filter)
        {
            return repository.GetCount(filter);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<CatalogProviderViewModel> GetAll(CatalogProviderFilterViewModel filter, int page, int pageSize)
        {
            return PrepareMultipleReturn(repository.GetAll(filter, page, pageSize));
        }

        /// <summary>
        /// Gets the actives.
        /// </summary>
        /// <returns></returns>
        public List<CatalogProviderViewModel> GetActives()
        {
            return PrepareMultipleReturn(repository.GetActives());
        }

        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public override tCatalogoProveedor PrepareAddData(CatalogProviderViewModel viewModel)
        {
            return new tCatalogoProveedor()
            {
                Nombre = viewModel.Nombre,
                Correo = viewModel.Correo,
                Telefono = viewModel.Telefono,
                SitioWeb = viewModel.SitioWeb,
                Usuario = viewModel.Usuario,
                Contrasena = viewModel.Contrasena,
                Nacional = viewModel.Nacional,
                Domicilio = viewModel.Domicilio,
                CreadoPor = viewModel.CreadoPor,
                Creado = viewModel.Creado,
                Activo = TypesCatalogProvider.Active
            };
        }

        /// <summary>
        /// Prepares the update data.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public override tCatalogoProveedor PrepareUpdateData(tCatalogoProveedor entity, CatalogProviderViewModel viewModel)
        {
            entity.Nombre = viewModel.Nombre;
            entity.Correo = viewModel.Correo;
            entity.Telefono = viewModel.Telefono;
            entity.SitioWeb = viewModel.SitioWeb;
            entity.Usuario = viewModel.Usuario;
            entity.Contrasena = viewModel.Contrasena;
            entity.Nacional = viewModel.Nacional;
            entity.Domicilio = viewModel.Domicilio;
            entity.ModificadoPor = viewModel.ModificadoPor;
            entity.Modificado = viewModel.Modificado;

            return entity;
        }

        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override CatalogProviderViewModel PrepareSingleReturn(tCatalogoProveedor entity)
        {
            return new CatalogProviderViewModel()
            {
                idProveedor = entity.idProveedor,
                Nombre = entity.Nombre,
                Correo = entity.Correo,
                Telefono = entity.Telefono,
                SitioWeb = entity.SitioWeb,
                Usuario = entity.Usuario,
                Contrasena = entity.Contrasena,
                Nacional = entity.Nacional ?? TRUE,
                Domicilio = entity.Domicilio,
                CreadoPor = entity.CreadoPor,
                Creado = entity.Creado,
                Activo = entity.Activo
            };
        }

        /// <summary>
        /// Prepares the multiple return.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public override List<CatalogProviderViewModel> PrepareMultipleReturn(List<tCatalogoProveedor> entities)
        {
            return entities.Select(p => new CatalogProviderViewModel()
            {
                idProveedor = p.idProveedor,
                Nombre = p.Nombre,
                Correo = p.Correo,
                Telefono = p.Telefono,
                SitioWeb = p.SitioWeb,
                Usuario = p.Usuario,
                Contrasena = p.Contrasena,
                Nacional = p.Nacional ?? TRUE,
                Domicilio = p.Domicilio,
                CreadoPor = p.CreadoPor,
                Creado = p.Creado,
                Activo = p.Activo
            }).ToList();
        }

        public void Delete(int id, int idUser)
        {
            repository.Delete(id);
        }
    }
}