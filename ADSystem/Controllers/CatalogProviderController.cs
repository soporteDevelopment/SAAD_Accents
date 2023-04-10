using ADEntities.ViewModels;
using ADSystem.Helpers;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    /// <summary>
    /// CatalogProviderController
    /// </summary>
    /// <seealso cref="ADSystem.Controllers.BaseController" />
    public class CatalogProviderController : BaseController
    {
        public ICatalogProviderManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogViewController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public CatalogProviderController(ICatalogProviderManager _manager)
        {
            manager = _manager;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            Session["Controller"] = "Proveedores";
            return View();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetById(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Catalog = manager.GetById(id) };
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
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetActives()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Providers = manager.GetActives() };
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
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAll(CatalogProviderFilterViewModel filter, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Providers = manager.GetAll(filter, page, pageSize), Count = manager.GetCount(filter) };
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
        public ViewResult Add()
        {
            return View();
        }

        /// <summary>
        /// Posts the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post(CatalogProviderViewModel catalog)
        {
            catalog.CreadoPor = (int)Session["_ID"];
            catalog.Creado = DateTime.Now;

            var result = manager.Post(catalog);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ViewResult Update(int id)
        {
            var provider = manager.GetById(id);
            return View(provider);
        }

        /// <summary>
        /// Saves the update catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Patch(CatalogProviderViewModel catalog)
        {
            catalog.ModificadoPor = (int)Session["_ID"];
            catalog.Modificado = DateTime.Now;

            manager.Patch(catalog.idProveedor, catalog); 
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Saves the update catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult UpdateStatus(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                manager.Delete(id, (int)Session["_ID"]);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Catálogos") };
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