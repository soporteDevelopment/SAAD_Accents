using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class ProfilesController : BaseController
    {
        // GET: Profiles
        public override ActionResult Index()
        {
            Session["Controller"] = "Perfiles";

            return View();
        }

        [HttpPost]
        public ActionResult ListProfiles(int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Profiles = tProfiles.GetProfiles(page, pageSize), Count = tProfiles.CountRegisters() };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult AddProfile()
        {
            ViewBag.Actions = JsonConvert.SerializeObject(tActions.GetAllActionsForController());

            return View();
        }

        [HttpPost]
        public ActionResult SaveAddProfile(string profile, List<int> lActions)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tPerfile oPerfil = new tPerfile();

                oPerfil.Perfil = profile;

                if (tProfiles.AddProfile(oPerfil, lActions) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Perfiles") };
                    
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

        public ViewResult UpdateProfile(int idProfile)
        {           
            ProfileViewModel oProfile = tProfiles.GetProfile(idProfile);

            ViewBag.Actions = JsonConvert.SerializeObject(tActions.GetActionsForProfile(idProfile));

            return View(oProfile);
        }

        [HttpPost]
        public ActionResult SaveUpdateProfile(int idProfile, List<int> lActions)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tProfiles.UpdateProfile(idProfile, lActions) == true)
                {

                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Perfiles") };
                    
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

        [HttpPost]
        public ActionResult GetProfiles()
        {

            return Json(tProfiles.GetProfiles());

        }


    }
}