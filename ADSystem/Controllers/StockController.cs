using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class StockController : BaseController
    {
        // GET: Stock
        public override ActionResult Index()
        {
            Session["Controller"] = "Inventario";

            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetStocks(string dtDateSince, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Stocks = tStocks.GetStocks(Convert.ToDateTime(dtDateSince), amazonas, guadalquivir, textura, page, pageSize), Count = tStocks.CountRegisters(Convert.ToDateTime(dtDateSince), amazonas, guadalquivir, textura) };
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
        public ActionResult AddStock(StockViewModel oStock)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { idStock = tStocks.AddStock(oStock), Message = "Inventario registrado" };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public PartialViewResult ContinueStock(int idStock, int idBranch)
        {
            ViewBag.idStock = idStock;
            ViewBag.idBranch = idBranch;

            ViewBag.Branch = tBranches.GetBranch(idBranch).Nombre;

            return PartialView("~/Views/Stock/ContinueStock.cshtml");
        }

        [HttpPost]
        public ActionResult UpdateStock(StockViewModel oStock)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tStocks.UpdateStock(oStock);

                tStocks.BackUpDataBaseEnd(oStock.idInventario);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Inventario registrado" };

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
        public ActionResult ListProductsStock(int idStock, string description, int? idProduct, string code, decimal? cost, string category, string color, string material, string brand, bool stockZero, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tStocks.GetAllProductsStock(idStock, description, idProduct, code, cost, category, color, material, brand, stockZero, page, pageSize), Count = tStocks.GetAllProductsStockCount(idStock, description, idProduct, code, cost, category, color, material, brand, stockZero) };
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
        public ActionResult ListProductsStockDif(int idStock, string description, string code, decimal? cost, string category, string color, string material, string brand, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tStocks.GetAllProductsStockDif(idStock, description, code, cost, category, color, material, brand, page, pageSize), Count = tStocks.GetAllProductsStockDifCount(idStock, description, code, cost, category, color, material, brand) };
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
        public ActionResult VerifyProduct(int idStock, int idBranch, string idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int iProduct = 0;

                if (Ean13.CheckCode(idProduct))
                {

                    string sProduct = idProduct.TrimStart('0');
                    iProduct = Convert.ToInt32(sProduct.Remove(sProduct.Length - 1));

                    var product = tProducts.GetProduct(iProduct);

                    //Verificar si el producto existe en el inventario actual
                    if (!tStocks.GetProductInStock(idStock, iProduct))
                    {
                        //Si no existe se registra
                        tStocks.AddProductStock(new ProductStockViewModel()
                        {
                            idInventario = idStock,
                            idProducto = iProduct,
                            CantidadAnterior = 0,
                            CantidadActual = 1,
                            Precio = product.PrecioVenta,
                            Inventariado = true
                        });
                        tProducts.UpdateProductBranchExistStock(iProduct, idBranch, 1);
                    }
                    else
                    {
                        //Si ya existe se incrementa el inventario en 1
                        tStocks.UpdateProductStock(idStock, iProduct);
                        tProducts.UpdateProductBranchExistStock(iProduct, idBranch, 1);
                    }

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { idProduct = iProduct };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "Código incorrecto" };
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

        [HttpPost]
        public ActionResult RefreshProduct(ProductStockViewModel product)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tStocks.RefreshProduct(product);

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

        [HttpPost]
        public ActionResult UpdateProductStock(ProductStockViewModel product)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tStocks.UpdateProductStock(product);

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

        [HttpPost]
        public ActionResult GetInformationStock(int idStock, int idBranch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { InformationStock = tStocks.GetInformationStock(idStock, idBranch) };
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
        public ActionResult GetCompleteInformationStock(int idStock)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { InformationStock = tStocks.GetCompleteInformationStock(idStock) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public void ExportDifference(int idStock)
        {
            var products = tStocks.GetAllProductsStockDifForExport(idStock);

            var csv = new StringBuilder();

            csv.AppendLine("CODIGO,DESCRIPCION,INVENTARIO ANTERIOR,INVENTARIO ACTUAL,DIFERENCIAS");

            string fileName = "Inventario";

            foreach (var prod in products)
            {
                csv.AppendFormat("{0},{1},{2},{3},{4}\n", prod.Codigo, this.CleanCsvString(prod.Descripcion), prod.CantidadAnterior, prod.CantidadActual, (prod.CantidadAnterior - prod.CantidadActual));
                fileName = "Inventario-" + prod.Fecha.Value.ToString("MM/yyyy") + ".csv";
            }

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.ContentType = "text/csv";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.Write(csv.ToString());
        }

        public ActionResult ExportCountingProducts(int idStock, int branch, string description, string code, decimal? cost, string category, string color, string material, string brand, int page, int pageSize, bool orderASC)
        {
            var products = tProducts.GetProductsForBranch(idStock, branch, description, code, cost, category, color, material, brand, false, page, pageSize, orderASC);

            return PartialView(products);
        }

        public ViewResult UpdateProduct(int idProduct, int idStock, int idBranch)
        {
            ProductViewModel oProduct = tProducts.GetProduct(idProduct);

            ViewBag.ProductBranch = JsonConvert.SerializeObject(tProducts.GetBranchesForProducts(idProduct));

            ViewBag.idStock = idStock;

            ViewBag.idBranch = idBranch;

            return View(oProduct);
        }

        public ActionResult PrintMissingProducts(int idStock)
        {
            var products = tStocks.GetMissingProducts(idStock);

            return PartialView(products);
        }

    }
}