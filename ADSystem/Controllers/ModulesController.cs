using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class ModulesController : BaseController
    {

        // GET: Modules
        public override ActionResult Index()
        {
            Session["Controller"] = "Modulos";

            return View();
        }

        [HttpPost]
        public ActionResult ListModules(int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Controllers =  tModules.GetControllers(page,pageSize), Count = tModules.CountRegisters()};

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public ViewResult AddModule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddModule(string control, string descripcion)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tControle oController = new tControle();

                oController.Control = control;
                oController.Estatus = TypesModule.EstatusActivo;
                oController.Descripcion = descripcion;

                if (tModules.AddController(oController) > 0)
                {

                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Modulos") };
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

        public ViewResult UpdateModule(int idControl)
        {
            ControllerViewModel oControl = (ControllerViewModel) tModules.GetController(idControl);

            return View(oControl);
        }

        [HttpPost]
        public ActionResult SaveUpdateModule(int idControl, string control, string descripcion)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                ControllerViewModel oController = new ControllerViewModel();
                oController.idControl = idControl;
                oController.Control = control;
                oController.Descripcion = descripcion;

                tModules.UpdateController(oController);
                
                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Modulos") };

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