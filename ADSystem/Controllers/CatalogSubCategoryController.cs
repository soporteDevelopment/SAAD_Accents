using ADEntities.Models;
using ADEntities.Queries;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    public class CatalogSubCategoryController : Controller
    {
        CatalogSubcategory catalogSubcategory;
        public MessagesTemplates Message = new MessagesTemplates();

        public CatalogSubCategoryController()
        {
            catalogSubcategory = new CatalogSubcategory();
        }

        public ViewResult Index(int idCategory)
        {
            Session["Controller"] = "Subcategorias";
            ViewData.Add("idCategory", idCategory);
            return View();
        }

        [HttpGet]
        public ActionResult GetAll(int idCategory)
        {
            JsonMessenger jmResult = new JsonMessenger();
            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { Subcategories = catalogSubcategory.GetSubCategories(idCategory), Count = catalogSubcategory.CountRegisters(idCategory) };

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListSubcategories(int page, int pageSize, int idCategory)
        {
            JsonMessenger jmResult = new JsonMessenger();
            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { Subcategories = catalogSubcategory.GetSubCategories(page, pageSize, idCategory), Count = catalogSubcategory.CountRegisters(idCategory) };

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ViewResult AddSubcategory(int idCategory)
        {
            ViewData.Add("idCategory", idCategory);
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddSubcategory(string name, string description, int idCategory)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tCatalogoSubcategoria oSubcategories = new tCatalogoSubcategoria();

                oSubcategories.Nombre = name;
                oSubcategories.Descripcion = description;
                oSubcategories.idCategoria = idCategory;
                var idSubcategory = catalogSubcategory.AddSubcategory(oSubcategories);

                if (idSubcategory > 0)
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Subcategorías"), idSubcategory };
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

        public ViewResult UpdateSubcategory(int idSubcategory)
        {
            SubcategoryViewModel oSubcategory = (SubcategoryViewModel)catalogSubcategory.GetSubcategory(idSubcategory);
            return View(oSubcategory);
        }

        [HttpPost]
        public ActionResult SaveUpdateSubcategory(int idSubcategory, string name, string description)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tCatalogoSubcategoria oSubcategory = new tCatalogoSubcategoria();
                oSubcategory.idSubcategoria = idSubcategory;
                oSubcategory.Nombre = name;
                oSubcategory.Descripcion = description;

                catalogSubcategory.UpdateSubcategory(oSubcategory);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Subcategorías") };
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
        public ActionResult DeleteSubcategory(int idSubcategory)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                catalogSubcategory.DeleteSubcategory(idSubcategory);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se eliminó la subcategoría" };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public void UpdateImage(int id, string image)
        {
            catalogSubcategory.UpdateImage(id, image);
        }

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

                if ((myFile != null && myFile.ContentLength != 0) && (extension.ToUpper() == ".PNG" || extension.ToUpper() == ".GIF" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".JPG"))
                {
                    string pathForSaving = Server.MapPath("~/Content/CatalogSubcategory");
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
                    Message = "La imagen se cargó correctamente"
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
    }
}