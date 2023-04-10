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
{   /// <summary>
    /// BranchOfficeController
    /// </summary>
    /// <seealso cref="ADSystem.Controllers.BaseController" />
    public class BranchOfficeController : BaseController
    {  
        /// <summary>
        /// The manager
        /// </summary>
        private IBranchOfficeManager _manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public BranchOfficeController(IBranchOfficeManager manager)
        {
            _manager = manager;
        }
        public override ActionResult Index()
        {
            ViewBag.idUser = ((UserViewModel)Session["_User"]).idUsuario;

            return View();
        }
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Branchs = _manager.GetDetail() };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }
            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }
    }
}