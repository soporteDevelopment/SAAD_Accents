using ADSystem.Helpers;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    /// <summary>
    /// CatalogBrandController
    /// </summary>
    /// <seealso cref="ADSystem.Controllers.BaseController" />
    public class CatalogBankAccountController : Controller
    {
        public ICatalogBankAccountManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public CatalogBankAccountController(ICatalogBankAccountManager _manager)
        {
            manager = _manager;
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
                var result = manager.Get();

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    Status = result
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
    }
}