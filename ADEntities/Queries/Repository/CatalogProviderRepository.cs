using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
    public class CatalogProviderRepository : BaseRepository<tCatalogoProveedor>, ICatalogProviderRepository
    {
        private readonly admDB_SAADDBEntities context;
        private bool FALSE = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogProviderRepository"/> class.
        /// </summary>
        /// <param name="_context">The context.</param>
        public CatalogProviderRepository(admDB_SAADDBEntities _context) : base(_context)
        {
            context = _context;
        }

        public override tCatalogoProveedor Update(tCatalogoProveedor entity)
        {
            var provider = context.tCatalogoProveedors.Find(entity.idProveedor);

            if (provider != null)
            {
                provider.Nombre = entity.Nombre;
                provider.Correo = entity.Correo;
                provider.Telefono = entity.Telefono;
                provider.SitioWeb = entity.SitioWeb;
                provider.Usuario = entity.Usuario;
                provider.Contrasena = entity.Contrasena;
                provider.Nacional = entity.Nacional;
                provider.Domicilio = entity.Domicilio;
                provider.ModificadoPor = entity.ModificadoPor;
                provider.Modificado = DateTime.Now;

                context.SaveChanges();
            }

            return entity;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public int GetCount(CatalogProviderFilterViewModel filter)
        {
            return context.tCatalogoProveedors.Where(p => (p.Nombre.Contains(filter.Nombre) || String.IsNullOrEmpty(filter.Nombre))
            && (p.Correo.Contains(filter.Correo) || String.IsNullOrEmpty(filter.Correo))
            && (p.Telefono.Contains(filter.Telefono) || String.IsNullOrEmpty(filter.Telefono))).Count();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<tCatalogoProveedor> GetAll(CatalogProviderFilterViewModel filter, int page, int pageSize)
        {
            return context.tCatalogoProveedors.Where(p => (p.Nombre.Contains(filter.Nombre) || String.IsNullOrEmpty(filter.Nombre))
            && (p.Correo.Contains(filter.Correo) || String.IsNullOrEmpty(filter.Correo))
            && (p.Telefono.Contains(filter.Telefono) || String.IsNullOrEmpty(filter.Telefono)))
            .OrderByDescending(p => p.Nombre).Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Gets the actives.
        /// </summary>
        /// <returns></returns>
        public List<tCatalogoProveedor> GetActives()
        {
            return context.tCatalogoProveedors.Where(p => p.Activo == true).ToList();
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id, int idUser)
        {
            var entity = context.tCatalogoProveedors.Find(id);

            if (entity != null)
            {
                entity.Activo = FALSE;
                entity.ModificadoPor = idUser;
                entity.Modificado = DateTime.Now;

                context.SaveChanges();
            }
        }
    }
}