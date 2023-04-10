using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Brands : Base
    {

        public int CountRegistersWithFilters(string brand, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMarcas.Where(p => p.Nombre.Contains(brand) || String.IsNullOrEmpty(brand)).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<BrandViewModel> GetBrands()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMarcas.Select(p => new BrandViewModel()
                    {

                        idMarca = p.idMarca,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion

                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public BrandViewModel GetBrand(int idBrand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMarcas.Where(p => p.idMarca == idBrand).Select(p => new BrandViewModel()
                    {

                        idMarca = p.idMarca,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion

                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<BrandViewModel> GetBrands(string brand, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var categories = context.tMarcas.Where(p => p.Nombre.Contains(brand) || String.IsNullOrEmpty(brand)).Select(p => new BrandViewModel()
                    {

                        idMarca = p.idMarca,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion

                    }).OrderBy(p => p.Nombre);

                    return categories.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<BrandViewModel> GetBrandCatalogs()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMarcas.Select(p => new BrandViewModel()
                    {

                        idMarca = p.idMarca,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion

                    }).OrderBy(p => p.Nombre).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddBrand(tMarca oBrand)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tMarca tMarca = new tMarca();

                    tMarca.Nombre = oBrand.Nombre;
                    tMarca.Descripcion = oBrand.Descripcion;

                    context.tMarcas.Add(tMarca);

                    context.SaveChanges();

                    iResult = tMarca.idMarca;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

            return iResult;
        }

        public void UpdateBrand(BrandViewModel oBrand)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tMarca tBrand = context.tMarcas.FirstOrDefault(p => p.idMarca == oBrand.idMarca);

                    tBrand.Nombre = oBrand.Nombre;
                    tBrand.Descripcion = oBrand.Descripcion;

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