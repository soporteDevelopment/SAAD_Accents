using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class CatalogTextilesController : BaseController
    {
        public override ActionResult Index()
        {
            Session["Controller"] = "Textiles";

            return View();
        }


        public ViewResult AddCatalogTextiles()
        {
            return View();
        }


        public JsonResult CargarTextiles(List<TextilesViewModel> textiles)
        {
            if (textiles != null && textiles.Count > 0)
            {
                JsonMessenger jmResult = new JsonMessenger();

                try
                {
                    string msgError = "";

                    for (int i = 0; i < textiles.Count; i++)
                    {
                        if (textiles[i].NombreTela == null || textiles[i].NombreTela == "")
                        {                            
                            msgError +=  $"{Environment.NewLine}Nombre Tela no puede estar vació, línea: {(i + 2)}.";
                        }
                        if (textiles[i].Coleccion == null || textiles[i].Coleccion == "")
                        {                            
                            msgError += $"{Environment.NewLine}Colección no puede estar vació, línea: {(i + 2)}.";
                        }
                        if (textiles[i].Precio == null)
                        {                           
                            msgError += $"{Environment.NewLine}Precio no puede estar vació, línea: {(i + 2)}.";

                        }
                        if (textiles[i].Moneda == null || textiles[i].Moneda == "")
                        {                            
                            msgError += $"{Environment.NewLine}Moneda no puede estar vació, línea: {(i + 2)}.";

                        }

                    }

                    if (msgError == null || msgError == "")
                    {
                        tTextiles.AddTextiles(textiles);

                        jmResult.success = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se cargo el archivo de Textiles correctamente") };
                    }
                    else
                    {

                        jmResult.success = 0;
                        jmResult.failure = 1;
                        jmResult.oData = new { Error = msgError };
                       
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
            else
            {
                return Json(new { success = false, message = "No se ha seleccionado ningún archivo" });
            }
        }

        [HttpGet]
        public ActionResult GetAllTextiles()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { textiles = tTextiles.GetTextiles() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }





    }
}