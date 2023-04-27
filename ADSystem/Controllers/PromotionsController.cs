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
    public class PromotionsController : BaseController
    {
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(string since, string until, bool active, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = tPromotions.Get(Convert.ToDateTime(since), Convert.ToDateTime(until), active, page, pageSize);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Total = result.Item1, Promotions = result.Item2 };
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
        public ActionResult MixedComboPromotion()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var result = tPromotions.GetMixedComboPromotion();
                var show = false;

                if(result != null)
                {
                    show = true;
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Show = show, Promotion = result };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post(PromotionViewModel promotion)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                promotion.CreadoPor = Convert.ToInt32(Session["_ID"]);
                promotion.Creado = DateTime.Now;
                promotion.Activo = Constants.bActivo;

                tPromotions.Add(promotion);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se registró la promoción" };
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
            var promotion = tPromotions.GetById(id);

            return View(promotion);
        }

        [HttpPatch]
        public ActionResult Patch(PromotionViewModel promotion)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                promotion.ModificadoPor = Convert.ToInt32(Session["_ID"]);
                promotion.Modificado = DateTime.Now;

                tPromotions.Update(promotion);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se actualizó la promoción" };
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