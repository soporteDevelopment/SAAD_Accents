using ADEntities.Queries;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class CustomersVirtualStoreController : Controller
    {
        public CustomersVirtualStore customers = new CustomersVirtualStore();

        // GET: Repairs
        public ActionResult Index()
        {
            Session["Controller"] = "Clientes";

            return View();
        }

        [HttpGet]
        public ActionResult Get(string name, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = customers.Get(name, page, pageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    total = result.Item1,
                    customers = result.Item2
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

        [HttpPatch]
        public ActionResult Status(int id)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (customers.Delete(id) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new
                    {
                        Message = "Se modificó el estatus del cliente"
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
    }
}