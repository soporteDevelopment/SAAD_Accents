using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class ProvidersController : BaseController
    {

        public override ActionResult Index()
        {
            Session["Controller"] = "Proveedores";

            return View();
        }

        [HttpPost]
        public ActionResult ListProviders(string provider, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Providers = tProviders.GetProviders(provider, page, pageSize), Count = tProviders.CountRegistersWithFilters(provider, page, pageSize) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);

        }

        [HttpPost]
        public ActionResult ListAllProviders()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Providers = tProviders.GetProviders(), Count = tProviders.GetProviders().Count };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);

        }

        public ViewResult AddProvider()
        {
            return View();
        }      
  
        [HttpPost]
        public ActionResult SaveAddProvider(string company, string rsocial, string ncommerce, string domFiscal, string domEntrega, 
                                            string nacionalidad, string empRFC, string sitioWeb, string sitioWebCat,
                                            string userWeb, string passWeb, string bankMEX, string branchMEX, string clabeMEX, 
                                            decimal? costImport, string countMEX, int? credit, int? creditDays,string dataINT,
                                            string email, decimal? freight, string holderMEX, string name, 
                                            string rfcMEX, string telephone)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tProveedore oProveedor = new tProveedore();
                oProveedor.NombreEmpresa = company;
                oProveedor.RazonSocial = rsocial;
                oProveedor.NombreComercial = ncommerce;
                oProveedor.DomicilioFiscal = domFiscal;
                oProveedor.DomicilioEntrega = domEntrega;
                oProveedor.Nacionalidad = nacionalidad;
                oProveedor.RFC = empRFC;
                oProveedor.SitioWeb = sitioWeb;
                oProveedor.SitioWebCatalogo = sitioWebCat;
                oProveedor.Usuario = userWeb;
                oProveedor.Contrasena = passWeb;
                oProveedor.Nombre = name;
                oProveedor.Correo = email;
                oProveedor.Telefono = telephone;
                oProveedor.Credito = credit;
                oProveedor.DiasCredito = creditDays;
                oProveedor.Flete = costImport;

                tCuentaBancariaMXN oCuentaMXN = new tCuentaBancariaMXN();
                oCuentaMXN.Banco = bankMEX;
                oCuentaMXN.Titular = holderMEX;
                oCuentaMXN.CLABE = clabeMEX;
                oCuentaMXN.Cuenta = countMEX;
                oCuentaMXN.Sucursal = branchMEX;
                oCuentaMXN.RFC = rfcMEX;

                oProveedor.tCuentaBancariaMXN = oCuentaMXN;
                              
                tCuentaBancariaINT oCuentaINT = new tCuentaBancariaINT();

                oCuentaINT.Datos = dataINT;

                oProveedor.tCuentaBancariaINT = oCuentaINT;

                oProveedor.Flete = costImport;
                oProveedor.Importacion = costImport;

                if (tProviders.AddProvider(oProveedor) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Proveedores") };
                    
                }

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ViewResult UpdateProvider(int idProvider)
        {

            ProviderViewModel oUser = tProviders.GetProvider(idProvider);

            return View(oUser);

        }

        [HttpPost]
        public ActionResult SaveUpdateProvider(int idProvider, int? idBankMxn, int? idBankInt, string company, string rsocial, 
                                            string ncommerce, string domFiscal, string domEntrega,
                                            string nacionalidad, string empRFC, string sitioWeb, string sitioWebCat,
                                            string userWeb, string passWeb, string bankMEX, string branchMEX, string clabeMEX, 
                                            decimal? costImport, string countMEX, int? credit, int? creditDays, string dataINT, 
                                            string email, decimal? freight, string holderMEX, string name, 
                                            string rfcMEX, string telephone)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                ProviderViewModel oProveedor = new ProviderViewModel();
                oProveedor.idProveedor = idProvider;
                oProveedor.Empresa = company;
                oProveedor.RazonSocial = rsocial;
                oProveedor.NombreComercial = ncommerce;
                oProveedor.DomicilioFiscal = domFiscal;
                oProveedor.DomicilioEntrega = domEntrega;
                oProveedor.Nacionalidad = nacionalidad;
                oProveedor.RFC = empRFC;
                oProveedor.SitioWeb = sitioWeb;
                oProveedor.SitioWebCatalogo = sitioWebCat;
                oProveedor.Usuario = userWeb;
                oProveedor.Contrasena = passWeb;
                oProveedor.Nombre = name;
                oProveedor.Correo = email;
                oProveedor.Telefono = telephone;
                oProveedor.Credito = credit;
                oProveedor.DiasCredito = creditDays;
                oProveedor.Flete = costImport;

                MXNBankAccountViewModel oCuentaMXN = new MXNBankAccountViewModel();
                oCuentaMXN.idCuentaBancaria = (int)idBankMxn;
                oCuentaMXN.Banco = bankMEX;
                oCuentaMXN.Titular = holderMEX;
                oCuentaMXN.CLABE = clabeMEX;
                oCuentaMXN.Cuenta = countMEX;
                oCuentaMXN.Sucursal = branchMEX;
                oCuentaMXN.RFC = rfcMEX;

                oProveedor.oCuentaBancariaMXN = oCuentaMXN;

                INTBankAccountViewModel oCuentaINT = new INTBankAccountViewModel();
                oCuentaINT.idBanco = (int)idBankInt;
                oCuentaINT.Datos = dataINT;

                oProveedor.oCuentaBancariaINT = oCuentaINT;

                oProveedor.Flete = costImport;
                oProveedor.Importacion = costImport;

                if (tProviders.UpdateProvider(oProveedor) == true)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Proveedores") };
                    
                }

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult GetAllBrandForProviders()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Brands = tBrands.GetBrands() };

            }catch(Exception ex){


                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        [HttpPost]
        public ActionResult UpdateStatusProvider(int idProvider, short bStatus)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                short status = Convert.ToInt16(bStatus);

                if (tProviders.UpdateStatusProvider(idProvider, status) == true) {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se eliminó un registro en el módulo {0}", "Proveedores") };
                    
                }
                else
                {

                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El proveedor tiene productos asignados" };

                }

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

    }
}