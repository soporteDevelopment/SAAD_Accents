using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    /// <summary>
    /// BranchCatalogsController
    /// </summary>
    /// <seealso cref="ADSystem.Controllers.BaseController" />
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class BranchCatalogsController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            Session["Controller"] = "Sucursales Catálogos";
            return View();
        }

        /// <summary>
        /// Gets the by catalog identifier.
        /// </summary>
        /// <param name="idCatalogo">The identifier catalogo.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetByCatalogId(int idCatalog)
        {
            JsonMessenger jmResult = new JsonMessenger();

            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { BranchCatalogs = tBranchCatalogs.Get(idCatalog) };

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the stock.
        /// </summary>
        /// <param name="idCatalogo">The identifier catalogo.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStock(int idCatalog)
        {
            JsonMessenger jmResult = new JsonMessenger();

            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { Stock = tBranchCatalogs.GetStock(idCatalog) };

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the stock by branch.
        /// </summary>
        /// <param name="idCatalog">The identifier catalog.</param>
        /// <param name="idBranch">The identifier branch.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStockByBranch(int idCatalog, int idBranch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { Stock = tBranchCatalogs.GetStockByBranch(idCatalog, idBranch) };

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the add branch catalog.
        /// </summary>
        /// <param name="branchCatalogs">The branch catalogs.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAddBranchCatalog(List<BranchCatalogViewModel> branchCatalogs)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                foreach(var branchCatalog in branchCatalogs) {
                    branchCatalog.ModificadoPor = (int)Session["_ID"];
                    branchCatalog.Modificado = DateTime.Now;
                }

                if ((tBranchCatalogs.Add(ADEntities.Converts.BranchCatalogs.PrepareAddMultiple(branchCatalogs)) > 0))
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Sucursales Catálogos") };
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
        /// Saves the update branch catalog.
        /// </summary>
        /// <param name="branchCatalogs">The branch catalogs.</param>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult SaveUpdateBranchCatalog(List<BranchCatalogViewModel> branchCatalogs)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                foreach (var branchCatalog in branchCatalogs)
                {
                    branchCatalog.ModificadoPor = (int)Session["_ID"];
                    branchCatalog.Modificado = DateTime.Now;
                }

                tBranchCatalogs.Update(ADEntities.Converts.BranchCatalogs.PrepareUpdateMultiple(branchCatalogs));

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Sucursales Catálogos") };
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