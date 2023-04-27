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
    public class CatalogTypeMeasureController : BaseController
    {
        public override ActionResult Index()
        {
             Session["Controller"] = "TypeMeasure";

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
                jmResult.oData = new { typeMeasures = tTypeMeasure.GetTypeMeasure() };
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
        public ActionResult ListTypeMeasure(string typeMeasure, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { typeMeasures = tTypeMeasure.GetTypeMeasure(typeMeasure, page, pageSize), Count = tTypeMeasure.GetTotalTypeMeasure(typeMeasure) };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ViewResult AddTypeMeasure()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddTypeMeasure(string nombreMedida)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tTipoMedida oTypeMeasure = new tTipoMedida();


                oTypeMeasure.NombreMedida = nombreMedida;
              

                var idTypeMeasure = tTypeMeasure.AddTypeMeasure(oTypeMeasure);

                if (idTypeMeasure > 0)
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "TipoMedida"), idTypeMeasure };

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

        public ViewResult UpdateTypeMeasure(int idTypeMeasure)
        {
            TypeMeasureViewModel oTypeMeasure = (TypeMeasureViewModel)tTypeMeasure.GetTypeMeasure(idTypeMeasure);

            return View(oTypeMeasure);
        }

        [HttpPatch]
        public ActionResult SaveUpdateTypeMeasure(int idTypeMeasure, string nombreMedida)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                TypeMeasureViewModel oTypeMeasure = new TypeMeasureViewModel();
                oTypeMeasure.idTipoMedida = idTypeMeasure;
                oTypeMeasure.NombreMedida = nombreMedida;



                tTypeMeasure.UpdateTypeMeasure(oTypeMeasure);


                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "TipoMedida") };

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
        public ActionResult DeleteTypeMeasure(int idTypeMeasure)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tTypeMeasure.DeleteTypeMeasure(idTypeMeasure);

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