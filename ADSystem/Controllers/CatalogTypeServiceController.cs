using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class CatalogTypeServiceController : BaseController
    {
        public override ActionResult Index()
        {
             Session["Controller"] = "TypeService";

            return View();
        }



        [HttpGet]
        public ActionResult GetAll()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { typeServices = tTypeService.GetTypeService() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListTypeService(string typeService, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { typeServices = tTypeService.GetTypeServices(typeService, page, pageSize), Count = tTypeService.GetTotalTypeService(typeService) };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ViewResult AddTypeService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddTypeService(string description, decimal factorUtilidad)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tTipoServicio oTypeService = new tTipoServicio();


                oTypeService.Descripcion = description;
                oTypeService.FactorUtilidad = factorUtilidad;

                var idTypeService = tTypeService.AddTypeService(oTypeService);

                if (idTypeService > 0)
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "TipoServicio"), idTypeService };

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

        public ViewResult UpdateTypeService(int idTypeService)
        {
            TypeServiceViewModel oTypeService = (TypeServiceViewModel)tTypeService.GetTypeService(idTypeService);

            return View(oTypeService);
        }

        [HttpPatch]
        public ActionResult SaveUpdateTypeService(int idTypeService, string description, decimal factorUtilidad)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                TypeServiceViewModel oTypeService = new TypeServiceViewModel();
                oTypeService.idTipoServicio = idTypeService;
                oTypeService.Descripcion = description;
                oTypeService.FactorUtilidad = factorUtilidad;



                tTypeService.UpdateTypeService(oTypeService);


                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "TipoServicio") };

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
        public ActionResult DeleteTypeService(int idTypeService)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tTypeService.DeleteTypeService(idTypeService);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se eliminó el tipo de servicio" };
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