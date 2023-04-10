using ADEntities.Enums;
using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    /// <summary>
    /// BranchCatalogs
    /// </summary>
    /// <seealso cref="ADEntities.Queries.Base" />
    public class BranchCatalogs : Base
    {
        /// <summary>
        /// Gets the specified identifier catalogo.
        /// </summary>
        /// <param name="idCatalogo">The identifier catalogo.</param>
        /// <returns></returns>
        public List<BranchCatalogViewModel> Get(int idCatalogo)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tCatalogoSucursals.Where(p => p.idCatalogo == idCatalogo).Select(p => new BranchCatalogViewModel()
                    {
                        idCatalogoSucursal = p.idCatalogoSucursal,
                        idCatalogo = p.idCatalogo,
                        idSucursal = p.idSucursal,
                        Cantidad = p.Cantidad,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado,
                    }).OrderBy(p => p.idSucursal).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Adds the specified branch catalogs.
        /// </summary>
        /// <param name="branchCatalogs">The branch catalogs.</param>
        /// <returns></returns>
        public int Add(List<tCatalogoSucursal> branchCatalogs)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tCatalogoSucursals.AddRange(branchCatalogs);

                    return context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Updates the specified branch catalogs.
        /// </summary>
        /// <param name="branchCatalogs">The branch catalogs.</param>
        /// <returns></returns>
        public int Update(List<tCatalogoSucursal> branchCatalogs)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var id = branchCatalogs.FirstOrDefault().idCatalogo;

                    var entities = context.tCatalogoSucursals.Where(p => p.idCatalogo == id).ToList();

                    foreach (var entity in entities)
                    {
                        context.tCatalogoSucursals.Remove(entity);
                        context.SaveChanges();
                    }

                    context.tCatalogoSucursals.AddRange(branchCatalogs);

                    return context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Gets the stock.
        /// </summary>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <returns></returns>
        public StockCatalogViewModel GetStock(int idCatalog)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var entities = context.tCatalogoSucursals.Where(p => p.idCatalogo == idCatalog).ToList();

                    var stock = new StockCatalogViewModel()
                    {
                        Amazonas = (entities.Where(p => p.idSucursal == TypesBranch.Amazonas).FirstOrDefault().Cantidad ?? 0) - (context.tCatalogoDetalleVistas.Where(p => p.idCatalogo == idCatalog && p.idSucursal == TypesBranch.Amazonas && p.Estatus == (int)StatusCatalogView.Pending).Sum(p => p.Cantidad - p.Devolucion) ?? 0),
                        Guadalquivir = (entities.Where(p => p.idSucursal == TypesBranch.Guadalquivir).FirstOrDefault().Cantidad ?? 0) - (context.tCatalogoDetalleVistas.Where(p => p.idCatalogo == idCatalog && p.idSucursal == TypesBranch.Guadalquivir && p.Estatus == (int)StatusCatalogView.Pending).Sum(p => p.Cantidad - p.Devolucion) ?? 0),
                        Textura = (entities.Where(p => p.idSucursal == TypesBranch.Textura).FirstOrDefault().Cantidad ?? 0) - (context.tCatalogoDetalleVistas.Where(p => p.idCatalogo == idCatalog && p.idSucursal == TypesBranch.Textura && p.Estatus == (int)StatusCatalogView.Pending).Sum(p => p.Cantidad - p.Devolucion) ?? 0),
                        Vista = (context.tCatalogoDetalleVistas.Where(p => p.idCatalogo == idCatalog && p.Estatus == (int)StatusCatalogView.Pending).Sum(p => p.Cantidad - p.Devolucion) ?? 0)
                    };

                    return stock;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Gets the stock.
        /// </summary>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <returns></returns>
        public decimal GetStockByBranch(int idCatalog, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return ((context.tCatalogoSucursals.Where(p => p.idCatalogo == idCatalog && p.idSucursal == idBranch).FirstOrDefault().Cantidad ?? 0) - (context.tCatalogoDetalleVistas.Where(p => p.idCatalogo == idCatalog && p.idSucursal == idBranch && p.Estatus == (int)StatusCatalogView.Pending).Sum(p => p.Cantidad - p.Devolucion) ?? 0));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}