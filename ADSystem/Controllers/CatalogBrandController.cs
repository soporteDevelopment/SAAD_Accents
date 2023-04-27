using ADEntities.Queries;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using ADSystem.Common;
using ADSystem.Helpers;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    /// <summary>
    /// CatalogBrandController
    /// </summary>
    /// <seealso cref="ADSystem.Controllers.BaseController" />
    public class CatalogBrandController : BaseController
    {
        public ICatalogBrandManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public CatalogBrandController(ICatalogBrandManager _manager)
        {
            manager = _manager;
        }

        public override ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAll(CatalogBrandFilterViewModel filter, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Brands = manager.GetAll(filter, page, pageSize), Count = manager.GetCount(filter) };
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
        /// Gets the by identifier.
        /// </summary>
        /// <param name="idCatalogBrand">The identifier catalog brand.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetById(int idCatalogBrand)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Brand = manager.GetDetailById(idCatalogBrand) };

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
        /// Adds this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Posts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post(CatalogBrandViewModel model)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                model.CreatedBy = (int)Session["_ID"];

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Marca registrada", Marca = manager.Post(model) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }
            return Json(jmResult);
        }
        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ViewResult Update(int id)
        {
            var catalogBrand = manager.GetById(id);

            return View(catalogBrand);
        }
        /// <summary>
        /// Patches the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Patch(CatalogBrandViewModel catalog)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                catalog.ModifiedBy = (int)Session["_ID"];
                catalog.Modified = DateTime.Now;

                manager.Patch(catalog.idCatalogBrand, catalog);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Marca actualizada" };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }
            return Json(jmResult);
        }
        /// <summary>
        /// Deactives the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Deactive(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                manager.Deactive(id);
                jmResult.success = 1;
                jmResult.failure = 0;
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
