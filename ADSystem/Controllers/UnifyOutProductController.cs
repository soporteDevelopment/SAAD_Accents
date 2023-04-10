using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class UnifyOutProductController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            Session["Controller"] = "Unificar Salidas a Vista";

            return View();
        }

        /// <summary>
        /// Comienza sección de codigo para Unificar Salidas a Vistas
        /// </summary>
        /// <returns></returns>
        public ViewResult ListUnifyOutProducts()
        {
            return View();
        }

        /// <summary>
        /// Lists the unify out products.
        /// </summary>
        /// <param name="fechaInicial">The fecha inicial.</param>
        /// <param name="fechaFinal">The fecha final.</param>
        /// <param name="cliente">The cliente.</param>
        /// <param name="iduser">The iduser.</param>
        /// <param name="producto">The producto.</param>
        /// <param name="remision">The remision.</param>
        /// <param name="project">The project.</param>
        /// <param name="amazonas">The amazonas.</param>
        /// <param name="guadalquivir">The guadalquivir.</param>
        /// <param name="textura">The textura.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListUnifyOutProducts(string fechaInicial, string fechaFinal, string cliente, int? iduser, string producto, string remision, string project, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var idUser = ((UserViewModel)Session["_User"]).idUsuario;
                var restricted = ((UserViewModel)Session["_User"]).Restringido;

                var result = tVistas.GetOutProducts(false, idUser, restricted, Convert.ToDateTime(fechaInicial), Convert.ToDateTime(fechaFinal), cliente, iduser, producto, remision, project, 1, amazonas, guadalquivir, textura, page, pageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { outProducts = result.Item1, Count = result.Item2 };
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
        /// Details the unify out products.
        /// </summary>
        /// <param name="remissions">The remissions.</param>
        /// <returns></returns>
        public ActionResult DetailUnifyOutProducts(List<string> remissions)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { outProducts = tVistas.GetMultiOutProductsForRemision(remissions) };
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
        /// Gets the unify pending out products.
        /// </summary>
        /// <param name="remissions">The remissions.</param>
        /// <returns></returns>
        public ActionResult GetUnifyPendingOutProducts(List<string> remissions)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                List<OutProductsViewModel> LisoutProductsFin = new List<OutProductsViewModel>();
                List<OutProductsViewModel> LisoutProductsIni = tVistas.GetMultiOutProductsPending(remissions);
                foreach (OutProductsViewModel view in LisoutProductsIni)
                {
                    view.oServicios = tVistas.GetServiceOutProducts(view.idVista);
                    LisoutProductsFin.Add(view);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { outProducts = LisoutProductsFin };
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
        /// Prints the unify pending out products.
        /// </summary>
        /// <param name="remissions">The remissions.</param>
        /// <returns></returns>
        public ActionResult PrintUnifyPendingOutProducts(string remissions)
        {
            var uno = remissions.Split(',');
            List<string> remision = new List<string>(uno);

            JsonMessenger jmResult = new JsonMessenger();
            List<OutProductsViewModel> ListoutProductsFin = new List<OutProductsViewModel>();

            List<OutProductsViewModel> ListoutProductsIni = tVistas.GetMultiOutProductsPending(remision);
            foreach (OutProductsViewModel view in ListoutProductsIni)
            {
                view.oServicios = tVistas.GetServiceOutProducts(view.idVista);
                ListoutProductsFin.Add(view);
            }

            return PartialView(ListoutProductsFin);
        }

        /// <summary>
        /// Details the unit out products.
        /// </summary>
        /// <param name="remision">The remision.</param>
        /// <returns></returns>
        public ActionResult DetailUnitOutProducts(string remision)
        {
            OutProductsViewModel oOutProducts = tVistas.GetOutProductsForRemision(remision);

            return PartialView("~/Views/UnifyOutProduct/DetailUnitOutProducts.cshtml", oOutProducts);
        }

        /// <summary>
        /// Updates the stock return unify out products.
        /// </summary>
        /// <param name="lProducts">The l products.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateStockReturnUnifyOutProducts(List<OutProductsDetailViewModel> lProducts)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tVistas.CheckStockReturnOutProducts(lProducts);
                tVistas.UpdateStockReturnOutProducts(lProducts);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El estatus de las Salidas a Vista se ha actualizado con exito." };
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