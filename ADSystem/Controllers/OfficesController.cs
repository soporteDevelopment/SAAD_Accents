using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class OfficesController : BaseController
    {
        // GET: Office
        public override ActionResult Index()
        {
            Session["Controller"] = "Despachos";

            return View();
        }

        [HttpPost]
        public ActionResult ListOffices(string offices,int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Offices = tOffices.GetOffices(offices, page, pageSize), Count = tOffices.CountRegisters() };
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
        public ActionResult ListAllOffices()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Offices = tOffices.GetOffices(), Count = tOffices.CountRegisters() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ViewResult AddOffice()
        {
            return View();
        }

        public ActionResult PartialAddOffice()
        {

            return PartialView("~/Views/Offices/PartialAddOffice.cshtml");

        }

        [HttpPost]
        public ActionResult SaveAddOffice(string name, string phone, string street, string extNum, string intNum, string neighborhood, int? idTown, int? cp, string email, decimal? percentage,int idOrigin)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (tOffices.OfficeExist(name) == true)
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El despacho ya se encuentra registrado" };
                }
                else if (tOffices.AddOffice(name, phone, street, extNum, intNum, neighborhood,(int)idTown, (int)cp, email, (decimal) percentage,(int)Session["_ID"], idOrigin) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Despachos") };
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

        public ViewResult UpdateOffice(int idOffice)
        {
            OfficeViewModel oOffice = (OfficeViewModel)tOffices.GetOffice(idOffice);

            if (oOffice.idMunicipio != null)
            {
                ViewBag.idEstado = tStates.GetIdStateForTown((int)oOffice.idMunicipio);
            }else
            {
                ViewBag.idEstado = 0;
            }

            return View(oOffice);
        }

        [HttpPost]
        public ActionResult SaveUpdateOffice(int idOffice, string name, string phone, string street, string extNum, string intNum, string neighborhood, int? idTown, int? cp, string email, decimal? percentage, int idOrigin)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tOffices.UpdateOffice(idOffice, name, phone, street, extNum, intNum, neighborhood,(int)idTown,(int)cp, email, (decimal)percentage,idOrigin);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Despachos") };
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
        public ActionResult DeleteOffice(int idDespacho)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tOffices.DeleteOffice(idDespacho))
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "El despacho fue eliminado" };

                }
                else
                {

                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El despacho esta relacionado con otros registros" };

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