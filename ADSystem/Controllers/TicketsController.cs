using ADEntities.Queries;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using ADSystem.Common;
using ADSystem.Helpers;
using ADSystem.Hubs;
using ADSystem.Manager.IManager;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    /// <summary>
    /// TicketsController
    /// </summary>
    /// <seealso cref="ADSystem.Controllers.BaseController" />
    public class TicketsController : BaseController
    {
        /// <summary>
        /// The manager
        /// </summary>
        private ITicketsManager _manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public TicketsController(ITicketsManager manager)
        {
            _manager = manager;
        }

        public override ActionResult Index()
        {
            ViewBag.idUser = ((UserViewModel)Session["_User"]).idUsuario;

            return View();
        }

		/// <summary>
		/// Gets all by status.
		/// </summary>
		/// <param name="idUser">The identifier user.</param>
		/// <param name="idStatusCredit">The identifier status credit.</param>
		/// <param name="idAssignedUser">The identifier assigned user.</param>
		/// <returns></returns>
		[HttpGet]
        public ActionResult GetAllByStatus(int? idUser,int idStatusCredit,int? idAssignedUser)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Tickets =  _manager.GetAllByStatus(idUser, idStatusCredit, idAssignedUser) };
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
        /// Updates the specified identifier status credit.
        /// </summary>
        /// <param name="idTicket">The identifier status credit.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Patch(int idTicket, TicketViewModel model)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                _manager.Patch(idTicket, model);
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
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="idTicket">The identifier ticket.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetById(int idTicket)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Ticket = _manager.GetDetailById(idTicket) };

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
        /// Posts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post(TicketViewModel model)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Ticket registrado", Ticket = _manager.Post(model) };
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
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateStatusPending(int id, int idStatusTicket, int idAssignedUser, decimal estimated, decimal filled)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                _manager.UpdateStatusPending(id, idStatusTicket, idAssignedUser, estimated, filled);

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
                    string pathForSaving = Server.MapPath("~/Content/Tickets");
                    if (this.CreateFolderIfNeeded(pathForSaving))
                    {
                        var imgName = string.Format("{0}{1}", fileName, extension);
                        bmpNew.Save(Path.Combine(pathForSaving, imgName));

                        _manager.UpdateImage(id, imgName);
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
        /// Updates the status active.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateStatusActive(int id, int idStatusTicket, int idAssignedUser, decimal estimated, decimal filled)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                _manager.UpdateStatusActive(id, idStatusTicket, idAssignedUser, estimated, filled);

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
        /// <summary>
        /// Updates the status closed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateStatusClosed(int id, int idStatusTicket, int idAssignedUser, decimal estimated, decimal filled)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                _manager.UpdateStatusClosed(id, idStatusTicket, idAssignedUser, estimated, filled);

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
        /// <summary>
        /// Deactives the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Deactive(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();
            try
            {
                _manager.Deactive(id);
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
        /// <summary>
        /// Updates the authorized by.
        /// </summary>
        /// <param name="idTicket">The identifier ticket.</param>
        /// <param name="authorizedBy">The authorized by.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public string UpdateAuthorizedBy(AuthorizeTicketViewModel authorizeTicket)
        {
            string response;

            try
            {
                _manager.UpdateAuthorized(authorizeTicket);

                response = "Ticket autorizado";
            }
            catch (Exception ex)
            {
                response = "Ocurrío un error. Contacte a soporte.";
            }

            return response;
        }
    }
}
