using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using ADSystem.Manager;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class CreditsController : BaseController
    {
        // GET: Credits
        public override ActionResult Index()
        {
            Session["Controller"] = "Créditos";
            return View();
        }

        [HttpPost]
        public ActionResult ListCredits(string dtDateSince, string dtDateUntil, string remision, string costumer, string codigo, string comments, int status, short? amazonas, short? guadalquivir, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Credits = tCredits.GetCredits(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), remision, costumer, codigo, comments, status, amazonas, guadalquivir, page, pageSize), Count = tCredits.CountRegisters() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ViewResult AddCredit()
        {
            ViewBag.idUser = Convert.ToInt32(Session["_ID"]);
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddCredit(int? idCreditNoteType, int? idSale, int idSeller, int? idCustomerP, int? idCustomerM, int? idOffice, decimal amount, string dtDate, string comments, List<CreditDetailViewModel> lProducts)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                string remCredit;
                int? userId = null;

                //ValidateSale
                if (idSale != null)
                {
                    if(tCredits.ValidateIfSaleHasCreditPayment((int)idSale)){
                        throw new Exception("La venta cuenta con crédito pendiente por saldar");
                    }
                }

                if (Session != null)
                {
                    userId = Convert.ToInt32(Session["_ID"]);
                }

                var idCredit = tCredits.AddCredit(idCreditNoteType, idSale, idSeller, idCustomerP, idCustomerM, idOffice, amount, Convert.ToDateTime(dtDate), Convert.ToDateTime(dtDate).AddYears(1), comments, lProducts, userId, out remCredit);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sMessage = String.Format(Message.msgAdd = "Se registró la nota con exito"), idCreditNote = idCredit, sRemision = remCredit, finalDate = Convert.ToDateTime(dtDate).AddYears(1).ToString("MM/dd/yyyy") };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public string SaveAddCredit(int? idCreditNoteType, int? idSale, int idSeller, decimal amount, string dtDate, string comments, List<CreditDetailViewModel> lProducts)
        {            
            try
            {
                string remCredit;
                int? userId = null;

                //ValidateSale
                if (idSale != null)
                {
                    if (tCredits.ValidateIfSaleHasCreditPayment((int)idSale))
                    {
                        throw new Exception("La venta cuenta con crédito pendiente por saldar");
                    }
                }

                if (Session != null)
                {
                    userId = Convert.ToInt32(Session["_ID"]);
                }
                var idCredit = tCredits.AddCredit(idCreditNoteType, idSale, idSeller, null, null, null, amount, DateTime.Now, DateTime.Now.AddYears(1), comments, lProducts, userId, out remCredit);

                return remCredit;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaveAddCreditFromPayment(int? idSale, decimal amount)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                string remCredit;
                int? userId = null;

                //ValidateSale
                if (idSale != null)
                {
                    if (tCredits.ValidateIfSaleHasCreditPayment((int)idSale))
                    {
                        throw new Exception("La venta cuenta con crédito pendiente por saldar");
                    }
                }

                if (Session != null)
                {
                    userId = Convert.ToInt32(Session["_ID"]);
                }
                var idCredit = tCredits.AddCreditFromPayment(idSale, amount, userId, out remCredit);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sMessage = String.Format(Message.msgAdd = "Se registró la nota con exito"), idCreditNote = idCredit, sRemision = remCredit, finalDate = DateTime.Now.AddYears(1).ToString("MM/dd/yyyy") };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetCreditForRemision(string remision)
        {
            return Json(new { Remision = tCredits.GetCreditForRemision(remision) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCreditNote(string remision)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                CreditNoteViewModel credit = tCredits.GetCreditNote(remision);

                if (credit != null)
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { CreditNote = credit };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "No se encontrarón registros." };
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

        public ActionResult PrintCredit(int idCreditNote)
        {
            CreditNoteViewModel oCredit = tCredits.GetSingleCreditForRemision(idCreditNote);
            return PartialView(oCredit);
        }

        public ActionResult DetailCredit(int idNotaCredito)
        {
            CreditNoteViewModel oCredit = tCredits.GetSingleCreditForRemision(idNotaCredito);
            return PartialView("~/Views/Credits/DetailCredit.cshtml", oCredit);
        }

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <param name="idNotaCredito">The identifier nota credito.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDetail(int idNotaCredito)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Detail = tCredits.GetCreditDetail(idNotaCredito) };
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
        public ActionResult UpdateStatus(int idCreditNote, short status)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tCredits.SetStatus(idCreditNote, status);
                
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Estatus modificado" };                
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetCreditForIdVenta(int idventa)
        {
            return Json(new { Remision = tCredits.GetCreditNoteForIdventa(idventa) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the credit note.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCreditNote(CreditNoteRequestViewModel request)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int? userId = null;

                if (Session != null)
                {
                    userId = Convert.ToInt32(Session["_ID"]);
                }

                CreditNoteManager creditNote = new CreditNoteManager();
                request.CreadoPor = userId;
                var result = creditNote.Add(request);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Se registra la Nota de Credito " + result.RemisionCredito };
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