using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class CatalogsController : BaseController
    {
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
        /// Lists the catalogs.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="model">The model.</param>
        /// <param name="volumen">The volumen.</param>
        /// <param name="idProvider">The identifier provider.</param>
        /// <param name="idCategory">The identifier category.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListCatalogs(string code, string model, string volumen, int? idProvider, int? idCategory, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Catalogs = tCatalogs.GetCatalogs(code, model, volumen, idProvider, idCategory, page, pageSize), Total = tCatalogs.CountRegisters(code, model, volumen, idProvider, idCategory) };
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
        /// Gets the by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetById(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Catalog = tCatalogs.GetById(id) };
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
        /// Gets the by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetByCode(string code)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Catalog = tCatalogs.GetByCode(code) };
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
        /// Getcodeses the specified codigo.
        /// </summary>
        /// <param name="codigo">The codigo.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCodes(string code)
        {
            return Json(new { Catalogs = tCatalogs.GetCodes(code) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the catalog.
        /// </summary>
        /// <returns></returns>
        public ViewResult AddCatalog()
        {
            return View();
        }

        /// <summary>
        /// Gets the brand catalogs.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBrandCatalogs()
        {
            JsonMessenger jmResult = new JsonMessenger();

            List<BrandViewModel> oBrand = tBrands.GetBrandCatalogs();

            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { Brand = oBrand };

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the add catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAddCatalog(CatalogViewModel catalog)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                catalog.CreadoPor = (int)Session["_ID"];
                catalog.Creado = DateTime.Now;

                var idCatalog = tCatalogs.AddCatalog(ADEntities.Converts.Catalogs.PrepareAdd(catalog));

                if (idCatalog > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Catálogos"), idCatalog };
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
        /// Updates the catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ViewResult UpdateCatalog(int id)
        {
            CatalogViewModel oCatalog = (CatalogViewModel)tCatalogs.GetById(id);
            return View(oCatalog);
        }

        /// <summary>
        /// Saves the update catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        [HttpPatch]
        public ActionResult SaveUpdateCatalog(CatalogViewModel catalog)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                catalog.ModificadoPor = (int)Session["_ID"];
                catalog.Modificado = DateTime.Now;

                tCatalogs.UpdateCatalog(ADEntities.Converts.Catalogs.PrepareUpdate(catalog));

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
        /// Updates the image.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="image">The image.</param>
        public void UpdateImage(int id, string image)
        {
            tCatalogs.UpdateImage(id, image);
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult UploadFile(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                HttpPostedFileBase myFile = Request.Files["file"];
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(myFile.FileName);
                string imgName = string.Empty;

                Image img = Image.FromStream(myFile.InputStream);
                Bitmap bmp = img as Bitmap;
                Graphics g = Graphics.FromImage(bmp);
                Bitmap bmpNew = new Bitmap(bmp);
                g.DrawImage(bmpNew, new Point(0, 0));
                g.Dispose();
                bmp.Dispose();
                img.Dispose();

                if ((myFile != null && myFile.ContentLength != 0) && (extension.ToUpper() == ".PNG" || extension.ToUpper() == ".GIF" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".JPG" || extension.ToUpper() == ".SVG"))
                {
                    string pathForSaving = Server.MapPath("~/Content/Catalogs");
                    if (this.CreateFolderIfNeeded(pathForSaving))
                    {
                        imgName = string.Format("{0}{1}", fileName, extension);
                        bmpNew.Save(Path.Combine(pathForSaving, imgName));

                        UpdateImage(id, imgName);
                    }
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Message = String.Format("La imagen se cargó correctamente")
                };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new
                {
                    Message = string.Format("La carga de la imagen falló: {0}", ex.Message)
                };
            }

            return Json(jmResult);
        }

        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Prints the ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintTicket(CatalogTicketViewModel ticket)
        {
            CatalogViewModel catalog = (CatalogViewModel)tCatalogs.GetById(ticket.idCatalogo);
            catalog.Cantidad = ticket.Cantidad;

            return PartialView(catalog);
        }

        /// <summary>
        /// Barcodes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Barcode(string id)
        {
            return File(DrawEan13(id), "image/jpg");
        }

        /// <summary>
        /// Draws the ean13.
        /// </summary>
        /// <param name="valueToCode">The value to code.</param>
        /// <returns></returns>
        private static byte[] DrawEan13(string valueToCode)
        {
            var resultado = new Byte[] { };

            using (var stream = new MemoryStream())
            {
                var bitmap = new Bitmap(200, 60);

                var grafic = Graphics.FromImage(bitmap);

                grafic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.SystemColors.Control),
                new Rectangle(0, 0, 200, 60));

                // Create an instance of the Ean13 Class.        
                Ean13 upc = new Ean13();

                upc.CodePedido = valueToCode;
                upc.Scale = 1.5F;

                upc.DrawEan13Barcode(grafic, new System.Drawing.Point(0, 0));

                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                stream.Seek(0, SeekOrigin.Begin);

                resultado = stream.ToArray();
            }

            return resultado;
        }
    }
}