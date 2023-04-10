using System;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
using ADSystem.Helpers;
using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using ADSystem.Common;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class UsersController : BaseController
    {
        // GET: Users
        public override ActionResult Index()
        {
            Session["Controller"] = "Usuarios";

            return View();
        }

        [HttpPost]
        public ActionResult ListUsers(int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Users = tUsers.GetUsers(page, pageSize), Count = tUsers.CountRegisters() };

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
        public ActionResult ListAllUsers()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Users = tUsers.GetUsers() };

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
        public ActionResult ListAllBranchOffices()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Branchs = tUsers.GetUsers() };

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
        public ActionResult ListTotalUsers()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Users = tUsers.GetAllUsers() };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }
        [HttpGet]
        public ActionResult GetAttentionTicket()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Users = tUsers.GetAttentionTicket() };

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
        public ActionResult ListAllUsersButNot(int? idUser)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (idUser == null)
                {
                    idUser = Convert.ToInt32(Session["_ID"]);
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Users = tUsers.GetUsersButNot((int)idUser) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListUsersForName(string name)
        {
            return Json(new { Users = tUsers.GetUsers(name) }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult AddUser()
        {
            ViewBag.Academic = JsonConvert.SerializeObject(tUsers.GetAcademicDegree());

            return View();
        }

        [HttpPost]
        public ActionResult SaveAddUser(string name, string lastname, string dtBirth, string street, string extNum, string intNum,
            string district, int? idTown, int? CP, string telephone, string telcel, string mail, string sex, int? idAcademicDegree,
            string grade, int idProfile, string password, int? branch, string dtInput, short? seller, bool? bill, short? restricted,bool? attentionTicket)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tUsuario oUser = new tUsuario();
                oUser.Nombre = name;
                oUser.Apellidos = lastname;
                oUser.FechaNacimiento = Convert.ToDateTime(dtBirth);
                oUser.Calle = street;
                oUser.NumExt = extNum;
                oUser.NumInt = intNum;
                oUser.Colonia = district;
                oUser.idMunicipio = idTown;
                oUser.CP = CP;
                oUser.Telefono = telephone;
                oUser.TelefonoCelular = telcel;
                oUser.Correo = mail;
                oUser.Sexo = sex;
                oUser.idNivelAcademico = idAcademicDegree;
                oUser.Titulo = grade;
                oUser.idPerfil = idProfile;
                oUser.Contrasena = password;
                oUser.idSucursal = branch;
                oUser.Estatus = TypesUser.EstatusActivo;
                oUser.FechaIngreso = Convert.ToDateTime(dtInput);
                oUser.Ventas = seller;
                oUser.Facturar = bill;
                oUser.Restringido = restricted;
                oUser.AtencionTicket = attentionTicket;

                if (tUsers.AddUser(oUser) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Usuarios") };
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

        public ViewResult UpdateUser(int idUser)
        {

            ViewBag.Academic = JsonConvert.SerializeObject(tUsers.GetAcademicDegree());

            UserViewModel oUser = tUsers.GetUser(idUser);

            return View(oUser);

        }

        [HttpPost]
        public ActionResult SaveUpdateUser(int idUser, string name, string lastname, string dtBirth, string street, string extNum, string intNum,
            string district, int? idTown, int? CP, string telephone, string telcel, string mail, string sex, int? idAcademicDegree,
            string grade, int idProfile, int? branch, string dtInput, short? seller, bool? bill, short? restricted,bool? attentionTicket)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                tUsuario oUser = new tUsuario();
                oUser.idUsuario = idUser;
                oUser.Nombre = name;
                oUser.Apellidos = lastname;
                oUser.FechaNacimiento = Convert.ToDateTime(dtBirth);
                oUser.Calle = street;
                oUser.NumExt = extNum;
                oUser.NumInt = intNum;
                oUser.Colonia = district;
                oUser.idMunicipio = idTown;
                oUser.CP = CP;
                oUser.Telefono = telephone;
                oUser.TelefonoCelular = telcel;
                oUser.Correo = mail;
                oUser.Sexo = sex;
                oUser.idNivelAcademico = idAcademicDegree;
                oUser.Titulo = grade;
                oUser.idPerfil = idProfile;
                oUser.idSucursal = branch;
                oUser.FechaIngreso = Convert.ToDateTime(dtInput);
                oUser.Ventas = seller;
                oUser.Facturar = bill;
                oUser.Restringido = restricted;
                oUser.AtencionTicket = attentionTicket;

                if (tUsers.UpdateUser(oUser) == true)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Usuarios") };
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
        public ActionResult UpdateStatusUser(int idUser, short Status)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (tUsers.UpdateStatusUser(idUser, Status) >= 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Usuarios") };
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
        public ActionResult UpdatePassword(string password, string newPassword)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var ID = Convert.ToInt32(Session["_ID"]);

                if (tUsers.ValidatePassword(ID, password))
                {
                    if (tUsers.ChangePassword(ID, newPassword))
                    {
                        jmResult.success = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Usuarios") };
                    }
                    else
                    {
                        jmResult.success = 0;
                        jmResult.failure = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "No se actualizó el registro en el módulo {0}", "Usuarios") };
                    }
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "No se actualizó el registro en el módulo {0}, contraseña incorrecta", "Usuarios") };
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
        public ActionResult UpdatePasswordFromUser(int idUser, string password)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tUsers.ChangePassword(idUser, password))
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Usuarios") };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "No se actualizó el registro en el módulo {0}", "Usuarios") };
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

        public ViewResult UpdateUserActual()
        {
            ViewBag.Academic = JsonConvert.SerializeObject(tUsers.GetAcademicDegree());

            UserViewModel loginUser = (UserViewModel)Session["_User"];

            UserViewModel oUser = tUsers.GetUser(loginUser.idUsuario);

            return View("UpdateUser", oUser);
        }

        public static string GeneratePassword()
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(10);
            for (int i = 0; i < 10; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        [HttpPost]
        public override ActionResult GetBranch()
        {
            return Json(tBranches.GetAllBranches());
        }
    }
}