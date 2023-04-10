using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Branches : Base
    {

        public BranchStoreViewModel GetBranch(int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tSucursales.Where(p => p.idSucursal == idBranch).Select(p => new BranchStoreViewModel()
                    {

                        idSucursal = p.idSucursal,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        IVATasa = p.IVATasa

                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<BranchStoreViewModel> GetAllBranches()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tSucursales.Select(p => new BranchStoreViewModel()
                    {

                        idSucursal = p.idSucursal,
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

        public List<BranchStoreViewModel> GetAllActivesBranches()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tSucursales.Where(p => p.idSucursal != TypesBranch.Todas).Select(p => new BranchStoreViewModel()
                    {

                        idSucursal = p.idSucursal,
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

        public BranchStoreViewModel GetBranchForUser(int idUser, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tUsuarios.Where(p => p.idSucursal == idBranch && p.idUsuario == idUser).Select(p => new BranchStoreViewModel()
                    {

                        idSucursal = p.tSucursale.idSucursal,
                        Nombre = p.tSucursale.Nombre,
                        Descripcion = p.tSucursale.Descripcion

                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

    }
}