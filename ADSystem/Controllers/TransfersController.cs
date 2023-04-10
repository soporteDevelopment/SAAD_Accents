using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class TransfersController : BaseController
    {
        // GET: Transfers
        public override ActionResult Index()
        {
            Session["Controller"] = "Transferencias";

            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);

            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));

            ViewBag.Branches = tBranches.GetAllActivesBranches();

            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        public ActionResult Add(tTransferencia transfer, List<tDetalleTransferencia> lDetail)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                transfer.CreadoPor = ((UserViewModel)Session["_User"]).idUsuario;
                transfer.Creado = DateTime.Now;

                tTransfers.Post(transfer, lDetail);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Transferencia registrada" };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Get(string dtDateSince, string dtDateUntil, string transfer, int? iduser, string code, int status, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Transfers = tTransfers.Get(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), transfer, code, status, iduser, amazonas, guadalquivir, textura, page, pageSize), Count = tTransfers.CountRegisters() };

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
        public ActionResult GetByProduct(int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { PendingTransfers = tTransfers.GetByProduct(idProduct) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int idTransfer)
        {
            var transfer = tTransfers.GetById(idTransfer);

            return View(transfer);
        }

        public ActionResult SaveUpdate(tTransferencia transfer, List<tDetalleTransferencia> lDetail)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                transfer.ModificadoPor = ((UserViewModel)Session["_User"]).idUsuario;
                transfer.Modificado = DateTime.Now;

                tTransfers.Update(transfer, lDetail);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Transferencia actualizada" };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ActionResult GeneratePrevNumber(int idBranch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Number = tTransfers.GeneratePrevNumberRem(idBranch) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ActionResult GetById(int idTransfer)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Transfer = tTransfers.GetById(idTransfer) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetByNumber(string number)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if ((tTransfers.GetByNumber(number).Estatus == 2) && (!tReceptions.GetReceptionByTransfer(number)))
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Transfer = tTransfers.GetByNumber(number) };
                } else {
                    throw new Exception("La transferencia ha sido cancelada o está relacionada a una recepción.");
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

        public ActionResult GetDetailTransfer(int idTransfer)
        {
            var transfer = tTransfers.GetById(idTransfer);

            return PartialView("~/Views/Transfers/Detail.cshtml", transfer);
        }

        public ActionResult UpdateStock(int idTransfer)
        {
            var transfer = tTransfers.GetById(idTransfer);

            return View(transfer);
        }

        public ActionResult UpdateStatus(int idTransfer, int status)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tTransfers.UpdateStatus(idTransfer, status);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Transferencia actualizada" };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult Print(int idTransfer)
        {
            var transfer = tTransfers.GetById(idTransfer);

            return View(transfer);
        }
    }


}