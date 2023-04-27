using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    public class DiscountsController : BaseController
    {
        // GET: Discounts
        public override ActionResult Index()
        {
            Session["Controller"] = "Discounts";

            return View();
        }

        [HttpPost]
        public ActionResult GetDiscount(decimal amount)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = null; /*new { Discount = tDiscounts.GetDiscount(amount) };*/

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
        public ActionResult GetDiscountOffice(decimal amount)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = null; /*new { Discount = tDiscounts.GetDiscountOffice(amount) };*/

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
        public ActionResult GetDiscountSpecial(decimal amount)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = null; /*new { Discount = tDiscounts.GetDiscountSpecial(amount) };*/

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