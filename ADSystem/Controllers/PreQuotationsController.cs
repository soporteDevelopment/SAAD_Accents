using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]

    public class PreQuotationsController : BaseController
    {

        PDFGenerator _PDFGenerator = new PDFGenerator();

        public override ActionResult Index()
        {
            Session["Controller"] = "PreCotizaciones";

            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);

            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));

            ViewBag.Seller = ((UserViewModel)Session["_User"])?.NombreCompleto;

            return View();
        }


        public ActionResult ListPreQuotations()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPreQuotations(bool allTime, string dtDateSince, string dtDateUntil, string number, string costumer, int? iduser, string project, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { PreQuotation = tPreQuotations.GetPreQuotations(allTime, Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), number, costumer, iduser, project, idUser, restricted, amazonas, guadalquivir, textura, page, pageSize), Count = tPreQuotations.CountRegisters( number,  costumer, iduser,  project,  idUser,  restricted,  amazonas,  guadalquivir,  textura,  page,  pageSize) };
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
        public ActionResult SavePreQuotation(PreQuotationsViewModel data)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var emailService = new Email();

                int idPreQuotation = tPreQuotations.AddPreQuotation(data);
                var quotation = _PDFGenerator.GeneratePreQuotation(idPreQuotation);

                PreQuotationsViewModel _quotation = tPreQuotations.GetPreQuotationsId(idPreQuotation);

                string email;

                if (_quotation.idClienteFisico > 0)
                {
                    email = tPhysicalCustomers.GetPhysicalCustomer((int)_quotation.idClienteFisico).Correo;
                }
                else if (_quotation.idClienteMoral > 0)
                {
                    email = tMoralCustomers.GetMoralCustomer((int)_quotation.idClienteMoral).Correo;
                }
                else
                {
                    email = tOffices.GetOffice((int)_quotation.idDespacho).Correo;
                }

                if (email.Trim().ToLower() != "noreply@correo.com")
                {
                    emailService.SendMailWithAttachment(email, "Precotización " + _quotation.Numero, "Precotización " + _quotation.Numero, "Precotizacion_" + _quotation.Numero + ".pdf", quotation);
                }
                else
                {
                    emailService.SendInternalMailWithAttachment("Precotización " + _quotation.Numero, "Precotización " + _quotation.Numero, "Precotizacion_" + _quotation.Numero + ".pdf", quotation);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la pre-cotización con exito"), idQuotation = idPreQuotation };
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
        public ActionResult UpdatePreQuotation(PreQuotationsViewModel data)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tPreQuotations.UpdatePreQuotation(data);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizo la precotizacion con exito") };
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
        public ActionResult SendCotizacion(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var msg = tPreQuotations.SendCotizacion(id);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = msg) };
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
        public ActionResult DeletePreQuotation(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tPreQuotations.DeletePreQuotation(id);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se elimino la precotizacion con exito") };
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
        public ActionResult GeneratePrevNumberRem()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sNumber = tPreQuotations.GeneratePrevNumberRem() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpGet]
        public ViewResult GetEraserPreQuotation(int id)
        {

            PreQuotationsViewModel prequotation = tPreQuotations.GetPreQuotationsId(id);

            ViewBag.IDBranch = prequotation.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)prequotation.idSucursal);

            ViewBag.TypeCustomer = prequotation.TipoCliente;

            ViewBag.Customer = (prequotation.idClienteFisico.HasValue) ? prequotation.idClienteFisico : (prequotation.idClienteMoral.HasValue) ? prequotation.idClienteMoral : prequotation.idDespacho;

            ViewBag.Office = prequotation.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(prequotation.Usuario1) ? "" : prequotation.Usuario1.Trim(), String.IsNullOrEmpty(prequotation.Usuario2) ? "" : prequotation.Usuario2.Trim() };

            return View(prequotation);
        }

        [HttpGet]
        public ActionResult ClonePreQuotation(int idPreQuotation)
        {
            JsonMessenger jmResult = new JsonMessenger();

            int idNewPreQuotation = tPreQuotations.ClonePreQuotation(idPreQuotation);

            PreQuotationsViewModel prequotation = tPreQuotations.GetPreQuotationsId(idNewPreQuotation);

            ViewBag.IDBranch = prequotation.idSucursal;

            ViewBag.Branch = tBranches.GetBranch((int)prequotation.idSucursal);

            ViewBag.TypeCustomer = prequotation.TipoCliente;

            ViewBag.Customer = (prequotation.idClienteFisico.HasValue) ? prequotation.idClienteFisico : (prequotation.idClienteMoral.HasValue) ? prequotation.idClienteMoral : prequotation.idDespacho;

            ViewBag.Office = prequotation.idDespachoReferencia;

            ViewBag.Users = new string[2] { String.IsNullOrEmpty(prequotation.Usuario1) ? "" : prequotation.Usuario1.Trim(), String.IsNullOrEmpty(prequotation.Usuario2) ? "" : prequotation.Usuario2.Trim() };

            return View("GetEraserPreQuotation", prequotation);

        }

        [HttpPost]
        public ActionResult AddProviderService(List<PreCotDetalleProveedoresViewModel> data)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                tPreQuotations.AddProviderService(data, idUser);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregaron los proveedores al servicio") };
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
        public ActionResult ComplementProviderService(PreCotDetalleProveedoresViewModel data)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tPreQuotations.ComplementProviderService(data);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se complemento la informacion del proveedor") };
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
        public FileContentResult DownloadPDF(int idPreQuotation)
        {
            var quotation = _PDFGenerator.GeneratePreQuotation(idPreQuotation);
            return File(quotation.ToArray(), "application/pdf", "Precotización-Folio.pdf");
        }

        [HttpPost]
        public ActionResult GenerateOrder(int idPreQuotationDetail, int idPreQuotation)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;

                // Crear un MemoryStream que contendrá el archivo ZIP
                var archivoZipStream = new MemoryStream();

                //cambiar estatus
                var proveedores = tPreQuotations.OrderedPreQuotation(idPreQuotationDetail, idUser);
                //obtener los proveedores asignados

                // Crear un objeto ZipArchive
                using (var archivoZip = new ZipArchive(archivoZipStream, ZipArchiveMode.Create, true))
                {
                    var i = 1;
                    foreach (var item in proveedores)
                    {
                        // Agregar el archivo PDF al archivo ZIP
                        var entry = archivoZip.CreateEntry(item.Proveedor + ".pdf");
                        using (var entryStream = entry.Open())
                        {

                            var archivo = _PDFGenerator.GenerateOrder(idPreQuotation, item.Proveedor, idPreQuotationDetail, i, item.idPreCotDetalleProveedores).ToArray();
                            entryStream.Write(archivo, 0, archivo.Length);
                        }
                        i++;
                    }
                }

                // Devolver el archivo ZIP como un archivo descargable
                archivoZipStream.Position = 0;
                return File(archivoZipStream, "application/zip", "archivosPDF.zip");

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