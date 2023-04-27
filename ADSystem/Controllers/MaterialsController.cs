using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class MaterialsController : BaseController
    {
        // GET: Categories
        public override ActionResult Index()
        {
            Session["Controller"] = "Materiales";

            return View();
        }

        [HttpPost]
        public ActionResult ListMaterials(int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Materials = tMaterials.GetMaterials(page, pageSize), Count = tMaterials.CountRegisters() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ViewResult AddMaterial()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveAddMaterial(string name)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if(tMaterials.MaterialExist(name) == true)
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El material ya se encuentra registrado" };
                }                               
                else if(tMaterials.AddMaterial(name) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Materiales") };
                    
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

        public ViewResult UpdateMaterial(int idMaterial)
        {
            MaterialViewModel oMaterial = (MaterialViewModel)tMaterials.GetMaterial(idMaterial);

            return View(oMaterial);
        }

        [HttpPost]
        public ActionResult SaveUpdateMaterial(int idMaterial, string name)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
               
                tMaterials.UpdateMaterial(idMaterial, name);
                
                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Materiales") };
                
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