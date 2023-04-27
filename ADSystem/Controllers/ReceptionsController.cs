using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class ReceptionsController : BaseController
    {
        // GET: Receptions
        public override ActionResult Index()
        {
            Session["Controller"] = "Recepciones";

            ViewBag.IDBranch = Convert.ToInt32(Session["_Sucursal"]);

            ViewBag.Branch = tBranches.GetBranch(Convert.ToInt32(Session["_Sucursal"]));

            ViewBag.Branches = tBranches.GetAllActivesBranches();

            ViewBag.Seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        public ActionResult Add(tRecepcione reception, List<tDetalleRecepcion> lDetail, int[][] aHistoryReception, int[][] aHistoryTransfer)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var lHistoryReception = new List<KeyValuePair<int,int>>();

                for (var i = 0; i <= aHistoryReception.Length-1; i++)
                {
                    lHistoryReception.Add(new KeyValuePair<int, int>(aHistoryReception[i][0], aHistoryReception[i][1]));
                }

                var lHistoryTransfer = new List<KeyValuePair<int, int>>();

                for (var i = 0; i <= aHistoryTransfer.Length-1; i++)
                {
                    lHistoryTransfer.Add(new KeyValuePair<int, int>(aHistoryTransfer[i][0], aHistoryTransfer[i][1]));
                }

                reception.CreadoPor = ((UserViewModel)Session["_User"]).idUsuario;
                reception.Creado = DateTime.Now;

                tReceptions.Post(reception, lDetail);

                tReceptions.UpdateReceivedAmount((int)reception.idRecepcion, lHistoryReception);

                tTransfers.UpdateReceivedAmount((int)reception.idTransferencia, lHistoryTransfer);

                //Se actualiza inventario
                tReceptions.UpdateStock((int)reception.idRecepcion, lHistoryReception, reception.idUsuario);

                tTransfers.UpdateStock((int)reception.idTransferencia, lHistoryTransfer, reception.idUsuario);
                
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Recepción registrada" };
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

        public ActionResult Get(string dtDateSince, string dtDateUntil, string reception, string transfer, int? iduser, string code, int status, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Receptions = tReceptions.Get(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), reception, transfer, code, status, iduser, amazonas, guadalquivir, textura, page, pageSize), Count = tReceptions.CountRegisters() };

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
                jmResult.oData = new { Number = tReceptions.GeneratePrevNumberRem(idBranch) };

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
                jmResult.oData = new { reception = tReceptions.GetById(idTransfer) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetDetailReception(int idReception)
        {
            var reception = tReceptions.GetById(idReception);

            return PartialView("~/Views/Receptions/Detail.cshtml", reception);
        }

        public ActionResult Update(int idReception)
        {
            var reception = tReceptions.GetById(idReception);

            return View(reception);
        }

        public ActionResult SaveUpdate(tRecepcione reception, List<tDetalleRecepcion> lDetail, List<tHistoricoRecepcion> lHistoryReception, int[][] aHistoryTransfer)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                reception.ModificadoPor = ((UserViewModel)Session["_User"]).idUsuario;
                reception.Modificado = DateTime.Now;

                tReceptions.UpdateReceivedAmount(lHistoryReception);

                var lHistoryTransfer = new List<KeyValuePair<int, int>>();

                for (var i = 0; i <= aHistoryTransfer.Length - 1; i++)
                {
                    lHistoryTransfer.Add(new KeyValuePair<int, int>(aHistoryTransfer[i][0], aHistoryTransfer[i][1]));
                }

                tTransfers.UpdateReceivedAmount((int)reception.idTransferencia, lHistoryTransfer);

                //Se actualiza inventario
                tReceptions.UpdateStock((int)reception.idRecepcion, lHistoryReception);

                tTransfers.UpdateStock((int)reception.idTransferencia, lHistoryTransfer, ((UserViewModel)Session["_User"]).idUsuario);

                tReceptions.Update(reception, lDetail);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Recepción registrada" };
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