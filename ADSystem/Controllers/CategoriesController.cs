using ADEntities.Models;
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
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class CategoriesController : BaseController
    {
        public override ActionResult Index()
        {
            Session["Controller"] = "Categorias";

            return View();
        }

        [HttpPost]
        public ActionResult ListCategories(string category, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Categories = tCategories.GetCategories(category, page, pageSize), Count = tCategories.GetTotalCategories(category) };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ViewResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddCategory(string name, string description)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tCategoria oCategory = new tCategoria();

                oCategory.Nombre = name;
                oCategory.Descripción = description;

                var idCategory = tCategories.AddCategory(oCategory);

                if (idCategory > 0)
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Categorias"), idCategory };                  

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

        public ViewResult UpdateCategory(int idCategory)
        {
            CategoryViewModel oCategory = (CategoryViewModel)tCategories.GetCategory(idCategory);

            return View(oCategory);
        }

        [HttpPost]
        public ActionResult SaveUpdateCategory(int idCategory, string name, string description)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                CategoryViewModel oCategory = new CategoryViewModel();
                oCategory.idCategoria = idCategory;
                oCategory.Nombre = name;
                oCategory.Descripcion = description;

                tCategories.UpdateCategory(oCategory);


                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Categorias") };
                
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
        public ActionResult DeleteCategory(int idCategory)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {               
                tCategories.DeleteCategory(idCategory);
                
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se eliminó la categoría" };                
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
            tCategories.UpdateImage(id, image);
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
                    string pathForSaving = Server.MapPath("~/Content/Categories");
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