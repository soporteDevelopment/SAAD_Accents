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
    [AuthorizedModule]
    public class ServicesController : BaseController
    {

        public string Nombre { get; set; }

        // GET: Services
        public override ActionResult Index()
        {
            Session["Controller"] = "Servicios";
            
            return View();
        }

        [HttpPost]
        public ActionResult ListServices(string service,int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Services = tServices.GetServices(service,page, pageSize), Count = tServices.CountRegisters() };
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
        public ActionResult ListAllServices()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Services = tServices.GetServices(), Count = tServices.CountRegisters() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ViewResult AddService()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAllServices()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { services = tServices.GetServicesM() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetServicesMeasureStandar(int idService)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { services = tServices.GetServicesME(idService) };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult SaveAddService(string description, short? installation, List<ServiceMeasureStandarViewModel> servicioMedidasEstandar)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (tServices.ServiceExist(description))
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El despacho ya se encuentra registrado" };
                }
                else if (tServices.AddService(description, installation??0, servicioMedidasEstandar) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Servicios") };
                    
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

        public ViewResult UpdateService(int idService)
        {
            ServiceViewModel oService = (ServiceViewModel)tServices.GetService(idService);

            return View(oService);
        }

        [HttpPost]
        public ActionResult SaveUpdateService(int idService, string description, short? installation, List<ServiceMeasureStandarViewModel> servicioMedidasEstandar)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tServices.UpdateService(idService, description, installation??0, servicioMedidasEstandar);

                jmResult.success = 1;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Servicios") };
                

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public int suma(int a, int b)
        {

            return a + b;

        }

        [HttpPost]
        public virtual ActionResult UploadFile(int qtyRotates)
        {
            JsonMessenger jmResult = new JsonMessenger();

            HttpPostedFileBase myFile = Request.Files["file"];
            bool isUploaded = false;
            string message = "La carga de la imagen falló";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(myFile.FileName);
            string imgName = string.Empty;

            //Image sourceImg = Image.FromStream(myFile.InputStream);
            Image img = Image.FromStream(myFile.InputStream);
            Bitmap bmp = img as Bitmap;
            Graphics g = Graphics.FromImage(bmp);
            Bitmap bmpNew = new Bitmap(bmp);
            g.DrawImage(bmpNew, new Point(0, 0));
            g.Dispose();
            bmp.Dispose();
            img.Dispose();

            switch (qtyRotates)
            {
                case 1:
                    bmpNew.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 2:
                    bmpNew.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 3:
                    bmpNew.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    break;
            }

            if ((myFile != null && myFile.ContentLength != 0) && (extension.ToUpper() == ".PNG" || extension.ToUpper() == ".GIF" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".JPG"))
            {
                string pathForSaving = Server.MapPath("~/Content/Services");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        imgName = string.Format("{0}{1}", fileName, extension);
                        bmpNew.Save(Path.Combine(pathForSaving, imgName));

                        isUploaded = true;
                        message = "Imagen cargada correctamente";                                              

                    }
                    catch (Exception ex)
                    {
                        message = string.Format("La carga de la imagen falló: {0}", ex.Message);
                    }
                }
            }

            return Json(new { isUploaded, message = message, fileName = imgName }, "text/html");
        }

        [HttpPost]
        public virtual ActionResult UploadPDF()
        {
            HttpPostedFileBase myFile = Request.Files["file"];
            bool isUploaded = false;
            string message = "La carga del archivo PDF falló";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(myFile.FileName);
            string pdfName = string.Empty;

            if ((myFile != null && myFile.ContentLength != 0) && (extension.ToUpper() == ".PDF"))
            {
                string pathForSaving = Server.MapPath("~/Content/PDF");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        pdfName = string.Format("{0}{1}", fileName, extension);
                        myFile.SaveAs(Path.Combine(pathForSaving, pdfName));

                        isUploaded = true;
                        message = "Archivo PDF cargado correctamente";
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("La carga del archivo PDF falló: {0}", ex.Message);
                    }
                }
            }

            return Json(new { isUploaded, message = message, fileName = pdfName }, "text/html");
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
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult GetInstallationService(int idService)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tServices.GetInstallationService(idService) == 1)
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Error = "Agregar instalación" };

                }
                else
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;

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

    }
}