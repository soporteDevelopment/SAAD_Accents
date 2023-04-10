using ADEntities.Queries;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class ShoppingCartsController : Controller
    {
        public ShoppingCarts shoppingCarts = new ShoppingCarts();
        
        [HttpGet]
        public ActionResult Get(int idCustomer)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new
                {
                    shoppingCart = shoppingCarts.Get(idCustomer)
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