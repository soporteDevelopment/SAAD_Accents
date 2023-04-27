using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Materials : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMateriales.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<MaterialViewModel> GetMaterials(int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMateriales.Select(p => new MaterialViewModel()
                    {

                        idMaterial = p.idMaterial,
                        Material = p.Material

                    }).OrderBy(p => p.Material).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

        }

        public List<MaterialViewModel> GetMaterials()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMateriales.Select(p => new MaterialViewModel()
                    {

                        idMaterial = p.idMaterial,
                        Material = p.Material

                    }).OrderBy(p => p.Material).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<MaterialViewModel> GetMaterials(string material)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMateriales.Where(p => p.Material.Contains(material)).Select(p => new MaterialViewModel()
                    {

                        idMaterial = p.idMaterial,
                        Material = p.Material

                    }).OrderBy(p => p.Material).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddMaterial(string material)
        {

            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tMateriale tMaterial = new tMateriale();

                    tMaterial.Material = material;

                    context.tMateriales.Add(tMaterial);

                    context.SaveChanges();

                    iResult = tMaterial.idMaterial;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

            return iResult;
        }

        public bool MaterialExist(string material)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMateriales.Where(p => p.Material.ToUpper() == material.ToUpper()).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public MaterialViewModel GetMaterial(int idMaterial)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMateriales.Where(p => p.idMaterial == idMaterial).Select(p => new MaterialViewModel()
                    {

                        idMaterial = idMaterial,
                        Material = p.Material

                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public void UpdateMaterial(int idMaterial, string material)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tMateriale tMaterial = context.tMateriales.FirstOrDefault(p => p.idMaterial == idMaterial);

                    tMaterial.Material = material;

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