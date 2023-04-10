using ADEntities.Queries;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class MetropolitanAreasController : Controller
    {
        public MetropolitanAreas metropolitanAreas = new MetropolitanAreas();

        // GET: MetropolitanArea
        public ActionResult Index()
        {
            Session["Controller"] = "Área Metropolitana";

            return View();
        }

        [HttpGet]
        public ActionResult Get(int? idTown, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = metropolitanAreas.Get(idTown, page, pageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Total = result.Item1,
                    Towns = ADEntities.Converts.MetropolitanAreas.TableToListModel(result.Item2)
                };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new
                {
                    Error = ex.Message
                };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAdd(MetropolitanAreaViewModel metropolitanArea)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = ADEntities.Converts.MetropolitanAreas.PrepareAdd(metropolitanArea);

                var idMetropolitanArea = metropolitanAreas.Add(result);

                if (idMetropolitanArea > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        idMetropolitanArea,
                        Message = String.Format("Se agregó un registro al módulo {0}", "Área Metropolitana")
                    };
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

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (metropolitanAreas.Delete(id) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se eliminó un registro en el módulo {0}", "Área Metropolitana")
                    };
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