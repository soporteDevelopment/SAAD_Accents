using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Converts
{
    public class BranchCatalogs
    {
        public static tCatalogoSucursal PrepareAdd(BranchCatalogViewModel entity)
        {
            tCatalogoSucursal result = new tCatalogoSucursal();

            result.idCatalogo = entity.idCatalogo;
            result.idSucursal = entity.idSucursal;
            result.Cantidad = entity.Cantidad;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;

            return result;
        }

        public static tCatalogoSucursal PrepareUpdate(BranchCatalogViewModel entity)
        {
            tCatalogoSucursal result = new tCatalogoSucursal();

            result.idCatalogo = entity.idCatalogo;
            result.idSucursal = entity.idSucursal;
            result.Cantidad = entity.Cantidad;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;

            return result;
        }

        public static List<tCatalogoSucursal> PrepareAddMultiple(List<BranchCatalogViewModel> entities)
        {
            return entities.Select(p => new tCatalogoSucursal()
            {
                idCatalogo = p.idCatalogo,
                idSucursal = p.idSucursal,
                Cantidad = p.Cantidad,
                CreadoPor = p.CreadoPor,
                Creado = p.Creado
            }).ToList();
        }

        public static List<tCatalogoSucursal> PrepareUpdateMultiple(List<BranchCatalogViewModel> entities)
        {
            return entities.Select(p => new tCatalogoSucursal()
            {
                idCatalogo = p.idCatalogo,
                idSucursal = p.idSucursal,
                Cantidad = p.Cantidad,
                ModificadoPor = p.ModificadoPor,
                Modificado = p.Modificado
            }).ToList();
        }
    }
}