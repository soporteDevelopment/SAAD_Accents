using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;


namespace ADSystem.Controllers
{
    internal class CatalogBrand
    {
        public void UpdateBrand(CatalogBrandViewModel oBrand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCatalogoMarca brand = context.tCatalogoMarcas.FirstOrDefault(p => p.IdCatalogoMarca == oBrand.idCatalogBrand);

                    brand.Nombre = oBrand.Name;
                    brand.Descripcion = oBrand.Description;
                    brand.idProveedor = oBrand.idProvider;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}