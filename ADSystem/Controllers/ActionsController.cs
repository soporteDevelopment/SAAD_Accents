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
    [AuthorizedModule]
    public class ActionsController : BaseController
    {
        // GET: Actions
        public override ActionResult Index()
        {
            Session["Controller"] = "Acciones";

            return View();
        }

        [HttpPost]
        public ActionResult ListActions(string module, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Actions = tActions.GetActions(module, page, pageSize), Count = tActions.CountRegistersWithFilters(module, page, pageSize) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);

        }

        public ViewResult AddAction()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult GetControllers()
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                List<ControllerViewModel> oController = tModules.GetControllers();

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Controllers = oController };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }
        
        public ActionResult GetControllers(string module)
        {

            return Json(new { Controllers = tModules.GetControllers(module) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveAddAction(string accion, bool ajax, bool menuHijo, bool menuPadre, int idControl, string descripcion)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tAccione oAction = new tAccione();

                oAction.Accion = accion;
                oAction.MenuPadre = Convert.ToInt16(menuPadre);
                oAction.MenuHijo = Convert.ToInt16(menuHijo);
                oAction.Ajax = Convert.ToInt16(ajax);
                oAction.idControl = idControl;
                oAction.Descripcion = descripcion;

                if (tActions.AddAction(oAction) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Acciones") };
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

        public ViewResult UpdateAction(int idAction)
        {
            ActionViewModel oAction = (ActionViewModel)tActions.GetAction(idAction);

            return View(oAction);
        }

        [HttpPost]
        public ActionResult SaveUpdateAction(int idAction, string accion, bool ajax, bool menuHijo, bool menuPadre, int idControl, string descripcion)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                ActionViewModel oAction = new ActionViewModel();
                oAction.idAction = idAction;
                oAction.Accion = accion;
                oAction.MenuPadre = Convert.ToInt16(menuPadre);
                oAction.MenuHijo = Convert.ToInt16(menuHijo);
                oAction.Ajax = Convert.ToInt16(ajax);
                oAction.idControl = idControl;
                oAction.Descripcion = descripcion;

                tActions.UpdateAction(oAction);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Acciones") };

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