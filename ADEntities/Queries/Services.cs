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

        public List<ServiceViewModel> GetServicesM()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tServicios.Select(p => new ServiceViewModel()
                    {

                        idServicio = p.idServicio,
                        Descripcion = p.Descripcion,
                        Instalacion = p.Instalacion ?? 0,
                        AplicarDescuento = p.AplicarDescuento ?? false,
                        MedidasEstandar = context.tServicioMedidasEstandars.Where(x => x.idServicio == p.idServicio).Select(x => x.idServicioMedidasEstandar).Count()

                    }).OrderBy(p => p.Descripcion).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<ServiceMeasureStandarViewModel> GetServicesME(int idService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tServicioMedidasEstandars.Where(p => p.idServicio == idService).
                        Select(p => new ServiceMeasureStandarViewModel()
                        {

                            idServicioMedidasEstandar = p.idServicioMedidasEstandar,
                            idServicio = p.idServicio,
                            idTipoMedida = p.idTipoMedida,
                            NombreMedida = context.tTipoMedida.Where(x => x.idTipoMedida == p.idTipoMedida).Select(x => x.NombreMedida).FirstOrDefault(),
                            Valor = p.Valor

                        }).OrderBy(p => p.idServicioMedidasEstandar).ToList();
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

        public int AddService(string description, short installation, List<ServiceMeasureStandarViewModel> servicioMedidasEstandar)
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

                    if(servicioMedidasEstandar != null)
                    {
                        foreach (var typeServicioMedidasEstandar in servicioMedidasEstandar)
                        {
                            tServicioMedidasEstandar oServicioMedidasEstandar = new tServicioMedidasEstandar();
                            oServicioMedidasEstandar.idServicio = oServicio.idServicio;
                            oServicioMedidasEstandar.idTipoMedida = typeServicioMedidasEstandar.idTipoMedida;
                            oServicioMedidasEstandar.Valor = typeServicioMedidasEstandar.Valor;
                            context.tServicioMedidasEstandars.Add(oServicioMedidasEstandar);
                        }

                        context.SaveChanges();
                    }
                    

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
                        AplicarDescuento = p.AplicarDescuento ?? false,
                        oMedidasEstandar = p.tServicioMedidasEstandars.Where(x => x.idServicio == p.idServicio).Select(x => new ServiceMeasureStandarViewModel()
                        {
                             idServicio =  x.idServicio,
                             idTipoMedida = x.idTipoMedida,
                             Valor = x.Valor,
                             NombreMedida = context.tTipoMedida.Where(y => y.idTipoMedida == x.idTipoMedida).FirstOrDefault().NombreMedida
                        }).ToList()

                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void UpdateService(int idService, string description, short installation, List<ServiceMeasureStandarViewModel> servicioMedidasEstandar)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tServicio oService = context.tServicios.FirstOrDefault(p => p.idServicio == idService);
                    oService.Descripcion = description;
                    oService.Instalacion = installation;
                    oService.AplicarDescuento = true;

                    if(servicioMedidasEstandar != null)
                    {
                        foreach (var typeServicioMedidasEstandar in servicioMedidasEstandar)
                        {
                            tServicioMedidasEstandar oServicioMedidasEstandar = context.tServicioMedidasEstandars.FirstOrDefault(p => p.idServicio == idService && p.idTipoMedida == typeServicioMedidasEstandar.idTipoMedida);
                            if(oServicioMedidasEstandar != null)
                            {
                                oServicioMedidasEstandar.idServicio = idService;
                                oServicioMedidasEstandar.idTipoMedida = typeServicioMedidasEstandar.idTipoMedida;
                                oServicioMedidasEstandar.Valor = typeServicioMedidasEstandar.Valor;
                            }
                            else
                            {
                                tServicioMedidasEstandar newMeasure = new tServicioMedidasEstandar();

                                newMeasure.idServicio = idService;
                                newMeasure.idTipoMedida = typeServicioMedidasEstandar.idTipoMedida;
                                newMeasure.Valor = typeServicioMedidasEstandar.Valor;
                                context.tServicioMedidasEstandars.Add(newMeasure);

                            }

                        }

                        context.SaveChanges();

                    }

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
