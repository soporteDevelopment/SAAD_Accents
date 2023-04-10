using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    /// <summary>
    /// CatalogBrandRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tCatalogoMarca&gt;" />
    /// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tCatalogoMarcas&gt;" />
    /// <seealso cref="ADEntities.Queries.IRepository.ICatalogBrandRepository" />
    public class CatalogBrandRepository : BaseRepository<tCatalogoMarca>, ICatalogBrandRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly admDB_SAADDBEntities context;
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogBrandRepository" /> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public CatalogBrandRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }
        /// <summary>
        /// Gets all by brand.
        /// </summary>
        /// <param name="idCatalogBrand">The identifier catalog brand.</param>
        /// <returns></returns>
        public List<CatalogBrandViewModel> GetAllByBrand(int idCatalogBrand)
        {
            return (from brand in context.tCatalogoMarcas
                    join user in context.tUsuarios on brand.CreadoPor equals user.idUsuario
                    where brand.IdCatalogoMarca == idCatalogBrand && brand.Activo == true
                    select new CatalogBrandViewModel()
                    {
                        idCatalogBrand = brand.IdCatalogoMarca,
                        Name = brand.Nombre,
                        Description = brand.Descripcion,
                        idProvider = (int)brand.idProveedor,
                        CreatedBy = (int)brand.CreadoPor,
                        Created = (DateTime)brand.Creado,
                        ModifiedBy = (int)brand.ModificadoPor,
                        Modified = (DateTime)brand.Modificado,
                        Active = (bool)brand.Activo,
                    }).ToList();
        }
        /// <summary>
        /// Gets the detail by identifier.
        /// </summary>
        /// <param name="idCatalogBrand">The identifier catalog brand.</param>
        /// <returns></returns>
        public CatalogBrandViewModel GetDetailById(int idCatalogBrand)
        {
            return (from brand in context.tCatalogoMarcas
                    join user in context.tUsuarios on brand.CreadoPor equals user.idUsuario
                    where brand.IdCatalogoMarca == idCatalogBrand && brand.Activo == true
                    select new CatalogBrandViewModel()
                    {
                        idCatalogBrand = brand.IdCatalogoMarca,
                        Name = brand.Nombre,
                        Description = brand.Descripcion,
                        idProvider = (int)brand.idProveedor,
                        CreatedBy = (int)brand.CreadoPor,
                        Created = (DateTime)brand.Creado,
                        ModifiedBy = (int)brand.ModificadoPor,
                        Modified = (DateTime)brand.Modificado,
                        Active = (bool)brand.Activo,
                    }).FirstOrDefault();
        }
        /// <summary>
        /// Patches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override tCatalogoMarca Update(tCatalogoMarca entity)
        {
            var brand = context.tCatalogoMarcas.Find(entity.IdCatalogoMarca);
            brand = entity;
            context.SaveChanges();

            return entity;
        }
        /// <summary>
        /// Gets the actives.
        /// </summary>
        /// <returns></returns>
        public List<tCatalogoMarca> GetActives()
        {
            return context.tCatalogoMarcas.Where(p => p.Activo == true).ToList();
        }
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public int GetCount(CatalogBrandFilterViewModel filter)
        {
            return context.tCatalogoMarcas.Where(p => (p.Nombre.Contains(filter.Name) || String.IsNullOrEmpty(filter.Name))
            && (p.idProveedor == filter.idProvider || filter.idProvider == null)).Count();
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<tCatalogoMarca> GetAll(CatalogBrandFilterViewModel filter, int page, int pageSize)
        {
            return context.tCatalogoMarcas.Where(p => (p.Nombre.Contains(filter.Name) || String.IsNullOrEmpty(filter.Name))
            && (p.idProveedor == filter.idProvider ||filter.idProvider == null) && p.Activo == true)
            
            .OrderByDescending(p => p.Nombre).Skip(page * pageSize).Take(pageSize).ToList();
        }

    }
}