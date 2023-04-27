using ADSystem.Common;
using ADSystem.Filters;
using ADSystem.Helpers;
using ADSystem.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class SalesOnLineController: Controller
    {
        public SalesOnLineManager _salesOnLineManager = new SalesOnLineManager();

        public ActionResult Index()
        {
            Session["Controller"] = "Ventas en Línea";

            return View();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sale = _salesOnLineManager.Get(id) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the by seller identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBySellerId()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sales = _salesOnLineManager.GetBySellerId(Convert.ToInt32(Session["_ID"])) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Print(int id)
        {
            return View(_salesOnLineManager.Get(id));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="idCustomer">The identifier customer.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAll(SaleOnLineFilters filters)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = _salesOnLineManager.GetAll(Convert.ToDateTime(filters.Start), Convert.ToDateTime(filters.End), filters.Status, filters.Customer, filters.User, filters.Page, filters.PageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sales = result.Item2, total = result.Item1 };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPatch]
        public ActionResult Status(int id, int status, int? branch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                //var sale = _salesOnLineManager.UpdateStatus(id, status, branch);
                SendNotification(id);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El estatus de la venta ha sido actualizado" };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPatch]
        public ActionResult Bill(int id, string bill)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var sale = _salesOnLineManager.UpdateBill(id, bill);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "La factura ha sido asignada", sale };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPatch]
        public ActionResult AssignedUser(int id, int idUser)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var sale = _salesOnLineManager.UpdateAssignedUser(id, idUser);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Vendedor asignado como responsable", sale };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpPatch]
        public ActionResult UpdateSendingData(int id, string sendingProvider, string guideNumber)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var sale = _salesOnLineManager.UpdateSendingData(id, sendingProvider, guideNumber);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Datos de envío actualizados", sale };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public void SendNotification(int id)
        {
            //var sale = _salesOnLineManager.Get(id);

            //StringBuilder body = new StringBuilder();

            //body.Append("<p>El estatus de su compra " + sale.Remision + " ha sido actualizado.</p>");
            //body.Append("<br/>");
            //body.Append("<p>Estatus actual:&nbsp;<b class='brand-danger'>" + sale.NombreEstatus + "</b></p>");
            //body.Append("<br/>");           

            Email email = new Email();

            //email.SendMail("admin@accentsadmin.com", "admin@accentsadmin.com", "Nuevo estatus de compra", "Hola");
        }
    }
}