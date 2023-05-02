using ADEntities.ViewModels;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    public class ProviderPreQuotationController : BaseController
    {
        PDFGenerator _PDFGenerator = new PDFGenerator();

        [HttpGet]
        public ViewResult Index(int id, int detailId)
        {
            Dictionary<String, dynamic> response = new Dictionary<string, dynamic>();
            PreQuotationsViewModel prequotation = tPreQuotations.GetPreQuotationsId(id);
            PreQuotationsDetailsViewModel oDetail = null;


            foreach (var item in prequotation.oDetail)
            {

                if (item.idPreCotizacionDetalle == detailId)
                {
                    oDetail = item;
                    break;
                }

            }

            response.Add("number", prequotation.Numero);
            response.Add("date", prequotation.Fecha);
            response.Add("oDetail", oDetail);

            return View(response);
        }

        public ViewResult CompleteService(int id, int detailId)
        {
            Dictionary<String, dynamic> response = ResponseOptionListPreQuotation(id, detailId);

            return View(response);
        }

        public ViewResult AssignProvider(int id, int detailId)
        {
            Dictionary<String, dynamic> response = ResponseOptionListPreQuotation(id, detailId);

            return View(response);
        }

        public ViewResult GenerateOrderService(int id, int detailId)
        {

            Dictionary<String, dynamic> response = new Dictionary<string, dynamic>();
            PreQuotationsViewModel prequotation = tPreQuotations.GetPreQuotationsId(id);
            PreQuotationsDetailsViewModel oDetail = null;

            foreach (var item in prequotation.oDetail)
            {
                if (item.idPreCotizacionDetalle == detailId)
                {
                    oDetail = item;
                    break;
                }
            }

            List<PreCotDetalleProveedoresViewModel> providers = tPreQuotations.GetProviderService(oDetail.idPreCotizacionDetalle);

            //Filtrar proveedores que esten asignados.  
            List<PreCotDetalleProveedoresViewModel> _filterProviders = new List<PreCotDetalleProveedoresViewModel>();
            foreach (var item in providers)
            {
                if (item.Asignado == 1)
                {
                    _filterProviders.Add(item);
                }
            }
            response.Add("number", prequotation.Numero);
            response.Add("date", prequotation.Fecha);
            response.Add("oDetail", oDetail);
            response.Add("providers", _filterProviders);

            return View(response);
        }

        [HttpPost]
        public ActionResult AddProviderService(List<PreCotDetalleProveedoresViewModel> data, int idPreQuotation)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;

                var emailService = new Email();
                var quotation = _PDFGenerator.GeneratePreQuotation(idPreQuotation);
                PreQuotationsViewModel _quotation = tPreQuotations.GetPreQuotationsId(idPreQuotation);

                tPreQuotations.AddProviderService(data, idUser);

                foreach (var item in data)
                {
                    var email = tPreQuotations.GetEmailProvider(item.idProveedor);
                    emailService.SendMailWithAttachment(email, "Precotización " + _quotation.Numero, "Precotización " + _quotation.Numero, "Precotizacion_" + _quotation.Numero + ".pdf", quotation);

                }

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
        public ActionResult GetProviderService(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idPreQuotation = tPreQuotations.ClonePreQuotation(id);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { ProvidersService = tPreQuotations.GetProviderService(id) };

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
        public ActionResult ComplementProvider(List<PreCotDetalleProveedoresViewModel> data)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                foreach (var item in data)
                {
                    tPreQuotations.ComplementProviderService(item);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la información del proveedor con éxito") };

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
        public ActionResult AssignProviderPreQuotation(List<PreCotDetalleProveedoresViewModel> data)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tPreQuotations.AssingProvider(data);
              
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se guardó la información del proveedor con éxito") };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }
            return Json(jmResult);
        }

        private Dictionary<String, dynamic> ResponseOptionListPreQuotation(int id, int detailId)
        {
            Dictionary<String, dynamic> response = new Dictionary<string, dynamic>();
            PreQuotationsViewModel prequotation = tPreQuotations.GetPreQuotationsId(id);
            PreQuotationsDetailsViewModel oDetail = null;

            foreach (var item in prequotation.oDetail)
            {
                if (item.idPreCotizacionDetalle == detailId)
                {
                    oDetail = item;
                    break;
                }
            }

            List<PreCotDetalleProveedoresViewModel> providers = tPreQuotations.GetProviderService(oDetail.idPreCotizacionDetalle);

            response.Add("number", prequotation.Numero);
            response.Add("date", prequotation.Fecha);
            response.Add("oDetail", oDetail);
            response.Add("providers", providers);

            return response;
        }

        public override ActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}