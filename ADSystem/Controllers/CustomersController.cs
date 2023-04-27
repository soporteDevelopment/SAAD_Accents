using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class CustomersController : BaseController
    {
        // GET: Customers
        public override ActionResult Index()
        {
            Session["Controller"] = "Clientes";

            return View();
        }

        [HttpPost]
        public ActionResult ListMoralCustomers(string costumer, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Customers = tMoralCustomers.GetMoralCustomers(costumer, page, pageSize), Count = tMoralCustomers.CountRegisters() };

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
        public ActionResult ListAllMoralCustomers()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Customers = tMoralCustomers.GetMoralCustomers() };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ViewResult AddMoralCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddMoralCustomer(string Name, string RFC, string CelPhone, string Phone, string Mail, string WebSite,
            short Nationality, string Street, string OutNumber, string IntNumber, string Suburb, int? Town, int? PC,
            string ContactName, string ContactPhone, string ContactMail, short? Credit, int? TimeOfCredit, decimal? CreditLimit,int idOrigin)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tClientesMorale entity = new tClientesMorale();

                entity.Nombre = Name;
                entity.Calle = Street;
                entity.NumExt = OutNumber;
                entity.NumInt = IntNumber;
                entity.Colonia = Suburb;
                entity.idMunicipio = Town;
                entity.CP = PC;
                entity.Nacionalidad = Nationality;
                entity.RFC = RFC;
                entity.TelefonoCelular = CelPhone;
                entity.Telefono = Phone;
                entity.Correo = Mail;
                entity.SitioWeb = WebSite;
                entity.NombreContacto = ContactName;
                entity.TelefonoContacto = ContactPhone;
                entity.CorreoContacto = ContactMail;
                entity.Credito = Credit ?? 0;
                entity.Plazo = TimeOfCredit ?? 0;
                entity.LimiteCredito = CreditLimit ?? 0;
                entity.FechaAlta = DateTime.Now;
                entity.CreadoPor = (int)Session["_ID"];
                entity.Creado = DateTime.Now;
                entity.idOrigen = idOrigin;

                if (tMoralCustomers.AddMoralCustomer(entity) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Clientes Morales") };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El nombre del cliente ya existe" };
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

        public ViewResult UpdateMoralCustomer(int idCustomer)
        {
            MoralCustomerViewModel oMoralCustomer = tMoralCustomers.GetMoralCustomer(idCustomer);
            return View(oMoralCustomer);
        }

        [HttpPost]
        public ActionResult SaveUpdateMoralCustomer(int idCustomer, string Name, string RFC, string CelPhone, string Phone, string Mail, string WebSite,
            short Nationality, string Street, string OutNumber, string IntNumber, string Suburb, int? Town, int? PC,
            string ContactName, string ContactPhone, string ContactMail, short? Credit, int? TimeOfCredit, decimal? CreditLimit, int idOrigin)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                MoralCustomerViewModel oCustomer = new MoralCustomerViewModel();

                oCustomer.idCliente = idCustomer;
                oCustomer.Nombre = Name;
                oCustomer.Calle = Street;
                oCustomer.NumExt = OutNumber;
                oCustomer.NumInt = IntNumber;
                oCustomer.Colonia = Suburb;
                oCustomer.idMunicipio = Town;
                oCustomer.CP = PC;
                oCustomer.Nacionalidad = Nationality;
                oCustomer.RFC = RFC;
                oCustomer.TelefonoCelular = CelPhone;
                oCustomer.Telefono = Phone;
                oCustomer.Correo = Mail;
                oCustomer.SitioWeb = WebSite;
                oCustomer.NombreContacto = ContactName;
                oCustomer.TelefonoContacto = ContactPhone;
                oCustomer.CorreoContacto = ContactMail;
                oCustomer.Credito = Credit ?? 0;
                oCustomer.Plazo = TimeOfCredit ?? 0;
                oCustomer.LimiteCredito = CreditLimit ?? 0;
                oCustomer.idOrigen = idOrigin; 

                if (tMoralCustomers.UpdateCustomer(oCustomer) == true)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Clientes Morales") };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El nombre del cliente ya existe" };
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
        public ActionResult ListPhysicalCustomers(string costumer, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Customers = tPhysicalCustomers.GetPhysicalCustomers(costumer, page, pageSize), Count = tPhysicalCustomers.CountRegisters() };

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
        public ActionResult ListAllPhysicalCustomers()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Customers = tPhysicalCustomers.GetPhysicalCustomers() };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            var jsonResult = Json(jmResult);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public ViewResult AddPhysicalCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddPhysicalCustomers(string Name, string LastName, DateTime? Birthday, string Genre, string Mail, string CelPhone,
            string Phone, string Street, string OutNumber, string IntNumber, string Suburb, int? Town, int? PC,
            string CardId, string RFC, short? Credit, int? TimeOfCredit, decimal? CreditLimit, string IntermediaryName, string IntermediaryPhone,int idOrigin)
        {

            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tClientesFisico entity = new tClientesFisico();

                entity.Nombre = Name;
                entity.Apellidos = LastName;
                entity.Calle = Street;
                entity.NumExt = OutNumber;
                entity.NumInt = IntNumber;
                entity.Colonia = Suburb;
                entity.idMunicipio = Town;
                entity.CP = PC;
                entity.FechaNacimiento = Convert.ToDateTime(Birthday);
                entity.RFC = RFC;
                entity.TelefonoCelular = CelPhone;
                entity.Telefono = Phone;
                entity.Correo = Mail;
                entity.Sexo = Genre;
                entity.NumeroIFE = CardId;
                entity.Credito = Credit ?? 0;
                entity.Plazo = TimeOfCredit ?? 0;
                entity.LimiteCredito = CreditLimit ?? 0;
                entity.NombreIntermediario = IntermediaryName;
                entity.TelefonoIntermediario = IntermediaryPhone;
                entity.FechaAlta = DateTime.Now;
                entity.CreadoPor = (int)Session["_ID"];
                entity.Creado = DateTime.Now;
                entity.idOrigen = idOrigin;

                if (tPhysicalCustomers.AddPhysicalCustomer(entity) > 0)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Clientes Fisicos") };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El nombre del cliente ya existe" };
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

        public ViewResult UpdatePhysicalCustomer(int idCustomer)
        {
            PhysicalCustomerViewModel oMoralCustomer = tPhysicalCustomers.GetPhysicalCustomer(idCustomer);
            return View(oMoralCustomer);
        }

        [HttpPost]
        public ActionResult SaveUpdatePhysicalCustomer(int idCustomer, string Name, string LastName, DateTime? Birthday, string Genre, string Mail, string CelPhone,
            string Phone, string Street, string OutNumber, string IntNumber, string Suburb, int? Town, int? PC,
            string CardId, string RFC, short? Credit, int? TimeOfCredit, decimal? CreditLimit, string IntermediaryName, string IntermediaryPhone, int idOrigin)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                PhysicalCustomerViewModel entity = new PhysicalCustomerViewModel();

                entity.idCliente = idCustomer;
                entity.Nombre = Name;
                entity.Apellidos = LastName;
                entity.Calle = Street;
                entity.NumExt = OutNumber;
                entity.NumInt = IntNumber;
                entity.Colonia = Suburb;
                entity.idMunicipio = Town;
                entity.CP = PC;
                entity.FechaNacimiento = Convert.ToDateTime(Birthday);
                entity.RFC = RFC;
                entity.TelefonoCelular = CelPhone;
                entity.Telefono = Phone;
                entity.Correo = Mail;
                entity.Sexo = Genre;
                entity.NoIFE = CardId;
                entity.Credito = Credit ?? 0;
                entity.Plazo = TimeOfCredit ?? 0;
                entity.LimiteCredito = CreditLimit ?? 0;
                entity.NombreIntermediario = IntermediaryName;
                entity.TelefonoIntermediario = IntermediaryPhone;
                entity.FechaAlta = DateTime.Now;
                entity.idOrigen = idOrigin;

                if (tPhysicalCustomers.UpdateCustomer(entity) == true)
                {
                    jmResult.success = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Clientes") };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El nombre del cliente ya existe" };
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

        public void UpdateIFE(int idCustomer, string noIFE)
        {
            tPhysicalCustomers.UpdateIFE(idCustomer, noIFE);
        }

        public ActionResult PartialAddMoralCustomer()
        {
            return PartialView("~/Views/Customers/PartialAddMoralCustomer.cshtml");
        }

        public ActionResult PartialAddPhysicalCustomer()
        {
            return PartialView("~/Views/Customers/PartialAddPhysicalCustomer.cshtml");
        }

        [HttpPost]
        public ActionResult DeletePhysicalCustomer(int idCustomer)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tPhysicalCustomers.DeletePhysicalCustomer(idCustomer))
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "El cliente fue eliminado." };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El cliente está relacionado con otros registros." };
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
        public ActionResult DeleteMoralCustomer(int idCustomer)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tMoralCustomers.DeleteMoralCustomer(idCustomer))
                {
                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = "El cliente fue eliminado." };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El cliente está relacionado con otros registros." };
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
        public ActionResult ListAllPhysicalCustomersButNot(int idCustomer)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Customers = tPhysicalCustomers.GetPhysicalCustomerButNot(idCustomer) };
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