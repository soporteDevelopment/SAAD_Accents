using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Profiles : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tPerfiles.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ProfileViewModel> GetProfiles()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tPerfiles.Where(p => p.Estatus == TypesProfile.EstatusActivo).Select(p => new ProfileViewModel()
                    {

                        idPerfil = p.idPerfil,
                        Perfil = p.Perfil

                    }).OrderBy(p => p.Perfil).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ProfileViewModel> GetProfiles(int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var Profiles = context.tPerfiles.Where(p => p.Estatus == TypesProfile.EstatusActivo).Select(p => new ProfileViewModel()
                    {

                        idPerfil = p.idPerfil,
                        Perfil = p.Perfil

                    }).OrderBy(p => p.Perfil);

                    return Profiles.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddProfile(tPerfile oProfile, List<int> lActions)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tPerfile tProfile = new tPerfile();

                    tProfile.Perfil = oProfile.Perfil;
                    tProfile.Estatus = TypesProfile.EstatusActivo;

                    context.tPerfiles.Add(tProfile);

                    context.SaveChanges();

                    if (tProfile.idPerfil > 0)
                    {
                        foreach (int action in lActions)
                        {
                            tPerfilAccione oPerfilAcciones = new tPerfilAccione();

                            oPerfilAcciones.idPerfil = tProfile.idPerfil;
                            oPerfilAcciones.idAccion = action;

                            context.tPerfilAcciones.Add(oPerfilAcciones);

                            context.SaveChanges();
                        }
                    }

                    return tProfile.idPerfil;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public ProfileViewModel GetProfile(int idProfile)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tPerfiles.Where(p => p.idPerfil == idProfile).Select(p => new ProfileViewModel()
                    {

                        idPerfil = idProfile,
                        Perfil = p.Perfil

                    }).SingleOrDefault();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateProfile(int idProfile, List<int> lActions)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    List<tPerfilAccione> lTable = context.tPerfilAcciones.Where(p => p.idPerfil == idProfile).ToList();

                    foreach (var t in lTable)
                    {

                        context.tPerfilAcciones.Remove(t);

                        context.SaveChanges();

                    }

                    foreach (int action in lActions)
                    {
                        tPerfilAccione oPerfilAcciones = new tPerfilAccione();

                        oPerfilAcciones.idPerfil = idProfile;
                        oPerfilAcciones.idAccion = action;

                        context.tPerfilAcciones.Add(oPerfilAcciones);

                        context.SaveChanges();
                    }

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

    }
}