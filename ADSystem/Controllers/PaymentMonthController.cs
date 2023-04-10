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
    /// PaymentMonthController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class PaymentMonthController : Controller
    {
        public IPaymentMonthManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogViewController"/> class.
        /// </summary>
        /// <param name="_manager">The manager.</param>
        public PaymentMonthController(IPaymentMonthManager _manager)
        {
            manager = _manager;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { PaymentMonths = manager.Get() };
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