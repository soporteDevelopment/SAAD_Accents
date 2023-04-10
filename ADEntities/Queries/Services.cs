using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Services : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tServicios.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ServiceViewModel> GetServices(string service,int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var services = context.tServicios
                        .Where(p => p.Descripcion.Contains(service) || String.IsNullOrEmpty(service))
                        .Select(p => new ServiceViewModel()
                    {

                        idServicio = p.idServicio,
                        Descripcion = p.Descripcion,
                        Instalacion = p.Instalacion ?? 0,
                        AplicarDescuento = p.AplicarDescuento ?? false

                    }).OrderBy(p => p.Descripcion);

                    return services.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ServiceViewModel> GetServices()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tServicios.Select(p => new ServiceViewModel()
                    {

                        idServicio = p.idServicio,
                        Descripcion = p.Descripcion,
                        Instalacion = p.Instalacion ?? 0,
                        AplicarDescuento = p.AplicarDescuento ?? false

                    }).OrderBy(p => p.Descripcion).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ServiceViewModel> GetServices(string service)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tServicios.Where(p => p.Descripcion.Contains(service)).Select(p => new ServiceViewModel()
                    {

                        idServicio = p.idServicio,
                        Descripcion = p.Descripcion,
                        Instalacion = p.Instalacion ?? 0,
                        AplicarDescuento = p.AplicarDescuento ?? false

                    }).OrderBy(p => p.Descripcion).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddService(string description, short installation)
        {

            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tServicio oServicio = new tServicio();
                    oServicio.Descripcion = description.Trim();
                    oServicio.Instalacion = installation;
                    oServicio.AplicarDescuento = true;
                    context.tServicios.Add(oServicio);
                    context.SaveChanges();

                    iResult = oServicio.idServicio;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        public bool ServiceExist(string description)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tServicios.Where(p => p.Descripcion.ToUpper().Trim() == description.ToUpper().Trim()).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public ServiceViewModel GetService(int idService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tServicios.Where(p => p.idServicio == idService).Select(p => new ServiceViewModel()
                    {

                        idServicio = p.idServicio,
                        Descripcion = p.Descripcion,
                        Instalacion = p.Instalacion ?? 0,
                        AplicarDescuento = p.AplicarDescuento ?? false

                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateService(int idService, string description, short installation)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tServicio oService = context.tServicios.FirstOrDefault(p => p.idServicio == idService);
                    oService.Descripcion = description;
                    oService.Instalacion = installation;
                    oService.AplicarDescuento = true;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public short GetInstallationService(int idService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tServicio oService = context.tServicios.FirstOrDefault(p => p.idServicio == idService);
                    return oService.Instalacion ?? 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}
