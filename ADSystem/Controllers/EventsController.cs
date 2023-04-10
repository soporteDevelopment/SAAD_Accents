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
    public class EventsController : BaseController
    {
        public override ActionResult Index()
        {           
            return View();
        }

        public ActionResult EventBody()
        {
            return PartialView("~/Views/Events/Index.cshtml");
        }

        [HttpPost]
        public ActionResult GetEvents()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Events = tEvents.GetEvents(Convert.ToInt32(Session["_ID"])) };
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
        public ActionResult GetEventsToday()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Events = tEvents.GetEventsToday(Convert.ToInt32(Session["_ID"])) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEvent(tEvento newEvent, List<int> users)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                newEvent.idUsuario = Convert.ToInt32(Session["_ID"]);
                int eventid = tEvents.AddEvent(newEvent);

                if(users != null)
                {
                    tUserEvent.AddUserEvent(users, eventid);
                }

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Eventos") };
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
        public ActionResult VerifyEvent(int eventID)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var userID = Convert.ToInt32(Session["_ID"]);

                if (tEvents.VerifyEvent(eventID, userID)){
                    jmResult.success = 1;
                    jmResult.failure = 0;
                }
                else
                {
                    var oEvent = tEvents.GetEventByID(eventID);

                    var user = tUsers.GetUser((int)oEvent.idUsuario);

                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "Evento creado por " + user.Nombre + " " + user.Apellidos };
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

        public ActionResult Update(int idEvent)
        {
            var oEvent = tEvents.GetEventByID(idEvent);

            return View(oEvent);
        }

        [HttpPost]
        public ActionResult UpdateEvent(tEvento updateEvent, List<int> users)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tEvents.UpdateEvent(updateEvent);

                if (users != null)
                {
                    tUserEvent.UpdateUserEvent(users, updateEvent.idEvento);
                }

                jmResult.success = 1;
                jmResult.oData = new { Message = "El evento se actualizó" };
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
        public ActionResult GetEventByID(int eventID)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Event = tEvents.GetEventByID(eventID) };
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
        public ActionResult DeleteEvent(int eventID)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tEvents.CancellEvent(eventID);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Evento eliminado" };
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