using ADEntities.Queries;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class BannersController : Controller
    {
        public Banners banners = new Banners();

        // GET: Repairs
        public ActionResult Index()
        {
            Session["Controller"] = "Banners";

            return View();
        }

        [HttpGet]
        public ActionResult Get(string name, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = banners.Get(name, page, pageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    total = result.Item1,
                    banners = result.Item2
                };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new
                {
                    Error = ex.Message
                };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAdd(BannerViewModel banner)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                banner.CreadoPor = ((UserViewModel)Session["_User"]).idUsuario;
                banner.Creado = DateTime.Now;

                var result = ADEntities.Converts.Banners.PrepareAdd(banner);

                var idBanner = banners.Add(result);

                if (idBanner > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        idBanner = idBanner,
                        Message = String.Format("Se agregó un registro al módulo {0}", "Banners")
                    };
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

        public ActionResult Update(int id)
        {
            var detail = banners.GetById(id);

            return View(detail);
        }

        [HttpPatch]
        public ActionResult SaveUpdate(BannerViewModel banner)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                banner.ModificadoPor = ((UserViewModel)Session["_User"]).idUsuario;
                banner.Modificado = DateTime.Now;

                var result = ADEntities.Converts.Banners.PrepareUpdate(banner);

                if (banners.Update(result) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se actualizó un registro en el módulo {0}", "Banners")
                    };
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

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (banners.Delete(id, ((UserViewModel)Session["_User"]).idUsuario, DateTime.Now) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = String.Format("Se eliminó un registro en el módulo {0}", "Banners")
                    };
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

        public void UpdateImage(int id, string image)
        {
            banners.UpdateImage(id, image);
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
                    string pathForSaving = Server.MapPath("~/Content/Banners");
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
                    Message = string.Format("La carga de la imagen falló: {0}, {1}", ex.Message, ex.InnerException)
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