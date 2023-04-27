using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace ADSystem.Controllers
{
    public class AccessController : BaseController
    {

        // GET: Access
        public override ActionResult Index()
        {
            ViewData.Add("Title","Access");

            return View();
        }

        [HttpPost]
        public ActionResult ValidateUser(string user, string password)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tUsers.AddTryLogin(user, password);

                UserViewModel oUser = tUsers.ValidateUser(user,password);                

                if (oUser != null)
                {
                    
                    if (tUsers.VerifyStateUser(oUser.idUsuario) == Constants.sActivo)
                    {

                        tUsers.CleanTryLogin(oUser.idUsuario);

                        Session.Add("_ID", oUser.idUsuario);

                        Session.Add("_User", oUser);

                        Session.Add("_Main", tModules.GetMain(oUser.idPerfil));

                        Session.Add("_Actions", tActions.GetActionsForProfileLog(oUser.idPerfil));

                        Session.Add("_Profile", oUser.idPerfil);

                        Session.Add("_Sucursal", oUser.idSucursal);

                        Session["page"] = 0;
                        Session["pageSize"] = 20;

                        string link;

                        int count = 0;

                        if (oUser.idPerfil == 8)
                        {

                            count = OutProductsController.GetCountPendingOutProducts();

                        }else
                        {

                            count = OutProductsController.GetCountPendingOutProducts(oUser.idUsuario);

                        }

                        Session.Add("_PendingViews", count);

                        if (count > 0)
                        {
                                                        
                            link = "../OutProducts/ListOutProducts";

                        }else
                        {

                            link = "JavaScript:Void(0);";

                        }

                        Session.Add("_Notification", link);

                        FormsAuthentication.SetAuthCookie(user, false);

                        Session.Add("Controller", "Home");

                        jmResult.success = 1;
                        jmResult.failure = 0;
                        jmResult.oData = new { Profile = oUser.idPerfil };
                        

                    }else{

                        jmResult.success = 0;
                        jmResult.failure = 1;
                        jmResult.oData = new { Error = "Usuario bloquedo." };

                    }

                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "Usuario o Contraseña invalidos." };

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
        public ActionResult SetBranch(int idBranch){

            JsonMessenger jmResult = new JsonMessenger();

            if (tBranches.GetBranchForUser(Convert.ToInt32(Session["_ID"]), idBranch) != null || Convert.ToInt32(Session["_Sucursal"]) == 1)
            {

                Session.Add("_Branch", tBranches.GetBranch(idBranch).Nombre);

                jmResult.success = 1;
                jmResult.failure = 0;

            }else{

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = "No se puede acceder a esta sucursal." };

            }      
      
            return Json(jmResult);

        }

        public ActionResult Close()
        {

            Session.Clear();

            FormsAuthentication.SignOut();            

            return RedirectToAction("Index", "Access");
        }

        public ActionResult RecoverPassword(string email)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                string password = UsersController.GeneratePassword();

                bool bResult = tUsers.ChangePassword(email.Trim(), password);

                if (bResult == true)
                {

                    string message = CreateBodyMail(password);

                    var emailService = new Email();

                    emailService.SendMail(email, "Contraseña", message);

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "La nueva contraseña ha sido enviada a tu cuenta de correo." };

                }
                else
                {

                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El correo no está registrado u ocurrío un error inesperado." };

                }

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { error = "Un error ocurrío durante el envío del correo.\n" + ex.Message };
                
            }

            return Json(jmResult);

        }

        public ActionResult Quotation()
        {
            return View();
        }
    }
}