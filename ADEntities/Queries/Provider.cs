using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Provider : Base
    {

        public int CountRegistersWithFilters(string provider, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProveedores.Where(p => (p.NombreEmpresa.Contains(provider) || String.IsNullOrEmpty(provider)) && p.Estatus == TypesProvider.EstatusActivo).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ProviderViewModel> GetProviders()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProveedores.Where(p => p.Estatus == TypesProvider.EstatusActivo).Select(p => new ProviderViewModel()
                    {

                        idProveedor = p.idProveedor,
                        Empresa = p.NombreEmpresa,
                        Nombre = p.Nombre,
                        Correo = p.Correo,
                        Telefono = p.Telefono,
                        Credito = p.Credito,
                        DiasCredito = p.DiasCredito,
                        FechaCaptura = p.FechaCaptura,
                        Flete = p.Flete,
                        Importacion = p.Importacion

                    }).OrderBy(p => p.Empresa).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public List<ProviderViewModel> GetProviders(string provider, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var Providers = context.tProveedores.Where(p => (p.NombreEmpresa == provider || String.IsNullOrEmpty(provider)) && p.Estatus == TypesProvider.EstatusActivo).Select(p => new ProviderViewModel()
                    {

                        idProveedor = p.idProveedor,
                        Empresa = p.NombreEmpresa,
                        Nombre = p.Nombre,
                        Correo = p.Correo,
                        Telefono = p.Telefono,
                        Credito = p.Credito,
                        DiasCredito = p.DiasCredito,
                        FechaCaptura = p.FechaCaptura,
                        Flete = p.Flete,
                        Importacion = p.Importacion

                    }).OrderBy(p => p.Empresa);

                    return Providers.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }


        public List<ProviderViewModel> GetProvidersByTypeService(int idTypeService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var Providers = context.tProveedores.Where(p => p.idTipoServicio == idTypeService && p.Estatus == TypesProvider.EstatusActivo).Select(p => new ProviderViewModel()
                    {

                        idProveedor = p.idProveedor,
                        Empresa = p.NombreEmpresa,
                        Nombre = p.Nombre,
                        Correo = p.Correo,
                        Telefono = p.Telefono,
                        Credito = p.Credito,
                        DiasCredito = p.DiasCredito,
                        FechaCaptura = p.FechaCaptura,
                        Flete = p.Flete,
                        Importacion = p.Importacion

                    }).OrderBy(p => p.Empresa);

                    return Providers.ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;


                }
        }


        public ProviderViewModel GetProvider(int idProveedor)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProveedores.Where(p => p.idProveedor == idProveedor).Select(p => new ProviderViewModel
                    {

                        idProveedor = p.idProveedor,
                        Empresa = p.NombreEmpresa,
                        RazonSocial = p.RazonSocial,
                        NombreComercial = p.NombreComercial,
                        DomicilioFiscal = p.DomicilioFiscal,
                        DomicilioEntrega = p.DomicilioEntrega,
                        Nacionalidad = p.Nacionalidad,
                        RFC = p.RFC,
                        RegimenFiscal = p.RegimenFiscal,
                        ProveedorPedidosMedida = p.ProveedorPedidosMedida,
                        idTipoServicio = p.idTipoServicio,
                        SitioWeb = p.SitioWeb,
                        SitioWebCatalogo = p.SitioWebCatalogo,
                        Usuario = p.Usuario,
                        Contrasena = p.Contrasena,
                        Nombre = p.Nombre,
                        Correo = p.Correo,
                        Telefono = p.Telefono,
                        Credito = p.Credito,
                        DiasCredito = p.DiasCredito,
                        oCuentaBancariaINT = new INTBankAccountViewModel()
                        {

                            idBanco = p.tCuentaBancariaINT.idBanco,
                            Datos = p.tCuentaBancariaINT.Datos
                        },
                        oCuentaBancariaMXN = new MXNBankAccountViewModel()
                        {

                            idCuentaBancaria = p.tCuentaBancariaMXN.idCuentaBancaria,
                            Banco = p.tCuentaBancariaMXN.Banco,
                            Titular = p.tCuentaBancariaMXN.Titular,
                            CLABE = p.tCuentaBancariaMXN.CLABE,
                            Cuenta = p.tCuentaBancariaMXN.Cuenta,
                            Sucursal = p.tCuentaBancariaMXN.Sucursal,
                            RFC = p.tCuentaBancariaMXN.RFC

                        },
                        FechaCaptura = p.FechaCaptura,
                        Flete = p.Flete,
                        Importacion = p.Importacion

                    }).First();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public int AddBankMXN(tCuentaBancariaMXN oBankMXN)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tCuentaBancariaMXN tBanco = new tCuentaBancariaMXN();
                    tBanco.Banco = oBankMXN.Banco;
                    tBanco.Titular = oBankMXN.Titular;
                    tBanco.CLABE = oBankMXN.CLABE;
                    tBanco.Cuenta = oBankMXN.Cuenta;
                    tBanco.Sucursal = oBankMXN.Sucursal;
                    tBanco.RFC = oBankMXN.RFC;

                    context.tCuentaBancariaMXNs.Add(tBanco);

                    context.SaveChanges();

                    iResult = tBanco.idCuentaBancaria;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    throw new ApplicationException(string.Format("Se generó la siguiente excepción, Mensaje: {0} , Interno: {1}", newException.Message, newException.InnerException.InnerException.Message));
                }

            return iResult;
        }

        public int AddBankINT(tCuentaBancariaINT oBankINT)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tCuentaBancariaINT tBanco = new tCuentaBancariaINT();
                    tBanco.Datos = tBanco.Datos;

                    context.tCuentaBancariaINTs.Add(tBanco);

                    context.SaveChanges();

                    iResult = tBanco.idBanco;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

            return iResult;
        }

        public int AddProvider(tProveedore oProvider)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tCuentaBancariaMXN tBancoMXN = new tCuentaBancariaMXN();

                    tBancoMXN.Banco = oProvider.tCuentaBancariaMXN.Banco;
                    tBancoMXN.Titular = oProvider.tCuentaBancariaMXN.Titular;
                    tBancoMXN.CLABE = oProvider.tCuentaBancariaMXN.CLABE;
                    tBancoMXN.Cuenta = oProvider.tCuentaBancariaMXN.Cuenta;
                    tBancoMXN.Sucursal = oProvider.tCuentaBancariaMXN.Sucursal;
                    tBancoMXN.RFC = oProvider.tCuentaBancariaMXN.RFC;

                    context.tCuentaBancariaMXNs.Add(tBancoMXN);

                    context.SaveChanges();

                    tCuentaBancariaINT tBancoINT = new tCuentaBancariaINT();

                    tBancoINT.Datos = oProvider.tCuentaBancariaMXN.RFC;

                    context.tCuentaBancariaINTs.Add(tBancoINT);

                    context.SaveChanges();

                    tProveedore tProveedor = new tProveedore();

                    tProveedor.NombreEmpresa = oProvider.NombreEmpresa;
                    tProveedor.RazonSocial = oProvider.RazonSocial;
                    tProveedor.NombreComercial = oProvider.NombreComercial;
                    tProveedor.DomicilioFiscal = oProvider.DomicilioFiscal;
                    tProveedor.DomicilioEntrega = oProvider.DomicilioEntrega;
                    tProveedor.Nacionalidad = oProvider.Nacionalidad;
                    tProveedor.RFC = oProvider.RFC;
                    tProveedor.RegimenFiscal = oProvider.RegimenFiscal;
                    tProveedor.ProveedorPedidosMedida = oProvider.ProveedorPedidosMedida;
                    tProveedor.idTipoServicio = oProvider.idTipoServicio;
                    tProveedor.SitioWeb = oProvider.SitioWeb;
                    tProveedor.SitioWebCatalogo = oProvider.SitioWebCatalogo;
                    tProveedor.Usuario = oProvider.Usuario;
                    tProveedor.Contrasena = oProvider.Contrasena;
                    tProveedor.Nombre = oProvider.Nombre;
                    tProveedor.Correo = oProvider.Correo;
                    tProveedor.Telefono = oProvider.Telefono;
                    tProveedor.Credito = oProvider.Credito;
                    tProveedor.DiasCredito = oProvider.DiasCredito;
                    tProveedor.idCuentaBancariaMXN = (tBancoMXN.idCuentaBancaria == 0) ? (int?)null : tBancoMXN.idCuentaBancaria;
                    tProveedor.idCuentaBancariaINT = (tBancoINT.idBanco == 0) ? (int?)null : tBancoINT.idBanco;
                    tProveedor.FechaCaptura = oProvider.FechaCaptura;
                    tProveedor.Flete = oProvider.Flete;
                    tProveedor.Importacion = oProvider.Importacion;
                    tProveedor.Estatus = TypesProvider.EstatusActivo;

                    context.tProveedores.Add(tProveedor);

                    context.SaveChanges();

                    iResult = tProveedor.idProveedor;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }

            return iResult;
        }

        public bool UpdateProvider(ProviderViewModel oProvider)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tProveedore tProveedor = context.tProveedores.FirstOrDefault(p => p.idProveedor == oProvider.idProveedor);

                    tProveedor.idProveedor = oProvider.idProveedor;
                    tProveedor.NombreEmpresa = oProvider.Empresa;
                    tProveedor.RazonSocial = oProvider.RazonSocial;
                    tProveedor.NombreComercial = oProvider.NombreComercial;
                    tProveedor.DomicilioFiscal = oProvider.DomicilioFiscal;
                    tProveedor.DomicilioEntrega = oProvider.DomicilioEntrega;
                    tProveedor.Nacionalidad = oProvider.Nacionalidad;
                    tProveedor.RFC = oProvider.RFC;
                    tProveedor.RegimenFiscal = oProvider.RegimenFiscal;
                    tProveedor.ProveedorPedidosMedida = oProvider.ProveedorPedidosMedida;
                    tProveedor.idTipoServicio = oProvider.idTipoServicio;
                    tProveedor.SitioWeb = oProvider.SitioWeb;
                    tProveedor.SitioWebCatalogo = oProvider.SitioWebCatalogo;
                    tProveedor.Usuario = oProvider.Usuario;
                    tProveedor.Contrasena = oProvider.Contrasena;
                    tProveedor.Nombre = oProvider.Nombre;
                    tProveedor.Correo = oProvider.Correo;
                    tProveedor.Telefono = oProvider.Telefono;
                    tProveedor.Credito = oProvider.Credito;
                    tProveedor.DiasCredito = oProvider.DiasCredito;
                    tProveedor.tCuentaBancariaMXN = new tCuentaBancariaMXN()
                    {
                        Banco = oProvider.oCuentaBancariaMXN.Banco,
                        Titular = oProvider.oCuentaBancariaMXN.Titular,
                        CLABE = oProvider.oCuentaBancariaMXN.CLABE,
                        Cuenta = oProvider.oCuentaBancariaMXN.Cuenta,
                        Sucursal = oProvider.oCuentaBancariaMXN.Sucursal,
                        RFC = oProvider.oCuentaBancariaMXN.RFC
                    };
                    tProveedor.tCuentaBancariaINT = new tCuentaBancariaINT()
                    {
                        Datos = oProvider.oCuentaBancariaINT.Datos
                    };
                    tProveedor.FechaCaptura = oProvider.FechaCaptura;
                    tProveedor.Flete = oProvider.Flete;
                    tProveedor.Importacion = oProvider.Importacion;

                    context.SaveChanges();

                    return true;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool UpdateStatusProvider(int idProvider, Int16 Status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    if (this.GetProductsForProvider(idProvider) == false)
                    {

                        tProveedore tProveedor = context.tProveedores.Single(p => p.idProveedor == idProvider);

                        tProveedor.Estatus = TypesProduct.EstatusInactivo;

                        context.SaveChanges();

                        return true;

                    }
                    else
                    {

                        return false;

                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public bool GetProductsForProvider(int idProvider)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tProductos.Where(p => p.idProveedor == idProvider && p.Estatus == TypesProduct.EstatusActivo).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

    }
}