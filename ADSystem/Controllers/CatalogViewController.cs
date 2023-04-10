using ADEntities.ViewModels;
using ADSystem.Common;
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
    /// CatalogViewController
    /// </summary>
    /// <seealso cref="ADSystem.Controllers.BaseController" />
    [SessionExpiredFilter]
    public class CatalogViewController : BaseController
    {
        public ICatalogViewManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogViewController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public CatalogViewController(ICatalogViewManager _manager)
        {
            manager = _manager;
        }


        /// <summary>
        /// Gets the previous number.
        /// </summary>
        /// <param name="idBranch">The identifier branch.</param>
        /// <returns></returns>
        public ActionResult GetPreviousNumber()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Number = manager.GeneratePreviousNumber() };
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
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            Session["Controller"] = "Catálogos";
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
        /// Gets the detail by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDetailById(int id)
        {
            var catalogView = manager.GetById(id);

            return View("~/Views/Catalogview/Detail.cshtml", catalogView);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAll(CatalogViewFilterViewModel filter)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Catalogs = manager.GetAll(filter), Count = manager.GetCount(filter) };
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
        public ActionResult Post(CatalogViewViewModel catalog)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                catalog.idUsuario = (int)Session["_ID"];
                catalog.CreadoPor = (int)Session["_ID"];
                catalog.Creado = DateTime.Now;

                var result = manager.Post(catalog);

                if (result.idVista > 0)
                {
                    Email(result.idVista);
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Catálogos"), idCatalogView = result.idVista };
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

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ViewResult Update(int id)
        {
            var oCatalog = manager.GetById(id);
            return View(oCatalog);
        }

        /// <summary>
        /// Saves the update catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult SaveUpdateCatalog(int id, CatalogViewViewModel catalog)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                catalog.ModificadoPor = (int)Session["_ID"];
                catalog.Modificado = DateTime.Now;

                manager.Patch(id, catalog);

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

        /// <summary>
        /// Saves the update catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult UpdateStatus(int id, short status)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                manager.UpdateStatus(id, status, (int)Session["_ID"]);

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

        /// <summary>
        /// Prints the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Print(int id)
        {
            var catalog = manager.GetForPrint(id);

            return View(catalog);
        }

        /// <summary>
        /// Prints the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Email(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var _PDFGenerator = new PDFGenerator();
                var catalog = manager.GetForPrint(id);
                var catalogPDF = _PDFGenerator.GenerateCatalogView(catalog);
                var emailService = new Email();

                if (catalog.Direccion.Correo.Trim().ToLower() != "noreply@correo.com")
                {
                    emailService.SendMailWithAttachment(catalog.Direccion.Correo, "Préstamo " + catalog.Numero, "Gracias por su visita.", "Prestamo_" + catalog.Numero + ".pdf", catalogPDF);
				}
				else
				{
                    emailService.SendInternalMailWithAttachment("Préstamo " + catalog.Numero, "Gracias por su visita.", "Prestamo_" + catalog.Numero + ".pdf", catalogPDF);
                }

                jmResult.success = 1;
                jmResult.oData = new { Message = "Correo enviado" };
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
        public ActionResult Return(int id, int[][] catalogs)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                manager.Return(id, catalogs, (int)Session["_ID"]);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizarón las devoluciones {0}", "Catálogos") };
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