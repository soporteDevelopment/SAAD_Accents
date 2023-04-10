using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    [AuthorizedModule]
    public class BrandsController : BaseController
    {
        // GET: Brands
        public override ActionResult Index()
        {
            Session["Controller"] = "Marcas";

            return View();
        }

        [HttpPost]
        public ActionResult ListBrands(string brand, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Brands = tBrands.GetBrands(brand, page, pageSize), Count = tBrands.CountRegistersWithFilters(brand, page, pageSize) };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ViewResult AddBrand()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveAddBrand(string name, string description)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tMarca oBrand = new tMarca();

                oBrand.Nombre = name;
                oBrand.Descripcion = description;

                if (tBrands.AddBrand(oBrand) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Marcas") };
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

        public ViewResult UpdateBrand(int idBrand)
        {
            BrandViewModel oBrand = (BrandViewModel)tBrands.GetBrand(idBrand);

            return View(oBrand);
        }

        [HttpPost]
        public ActionResult SaveUpdateBrand(int idBrand, string name, string description)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                BrandViewModel oBrand = new BrandViewModel();
                oBrand.idMarca = idBrand;
                oBrand.Nombre = name;
                oBrand.Descripcion = description;
               
                tBrands.UpdateBrand(oBrand);


                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Marcas") };

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