using ADEntities.Models;
using ADEntities.Queries;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using ADSystem.Hubs;
using ADSystem.Manager.IManager;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace ADSystem.Manager
{
    /// <summary>
    /// CustomerOriginManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager&lt;ADEntities.Models.tTicket, ADEntities.ViewModels.TicketViewModel&gt;" />
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tTicket, ADEntities.ViewModels.TicketViewModel};" />
    /// <seealso cref="ADSystem.Manager.IManager.ITicketsManager" />
    public class TicketsManager : BaseManager<tTicket, TicketViewModel>, ITicketsManager
    {
        /// <summary>
        /// The fernanda email
        /// </summary>
        public string FernandaEmail = WebConfigurationManager.AppSettings["FernandaEmail"].ToString();
        /// <summary>
        /// The repository
        /// </summary>
        private ITicketsRepository _repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsManager" /> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public TicketsManager(ITicketsRepository repository) : base(repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public override tTicket PrepareAddData(TicketViewModel viewModel)
        {
            return new tTicket()
            {
                Titulo = viewModel.Title,
                Descripcion = viewModel.Description,
                Creado = DateTime.Now,
                CreadoPor = viewModel.CreatedBy,
                Codigo = GenerateCode(),
                idPrioridadTicket = viewModel.idPriorityTicket,
                idTipoTicket = viewModel.idTypeTicket,
                idEstatusTicket = Constants.PendingStatusTicket,
                idUsuarioAsignado = viewModel.idAssignedUser,
                Autorizado = viewModel.Authorized,
                AutorizadoPor = viewModel.AuthorizedBy,
                FechaAutorizado = viewModel.AuthorizedDate,
                Activo = true
            };
        }
        /// <summary>
        /// Prepares the update data.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public override tTicket PrepareUpdateData(tTicket entity, TicketViewModel viewModel)
        {
            entity.Titulo = viewModel.Title;
            entity.Descripcion = viewModel.Description;
            entity.Creado = DateTime.Now;
            entity.CreadoPor = viewModel.CreatedBy;
            entity.Codigo = GenerateCode();
            entity.idPrioridadTicket = viewModel.idPriorityTicket;
            entity.idTipoTicket = viewModel.idTypeTicket;
            entity.idEstatusTicket = Constants.PendingStatusTicket;
            entity.idUsuarioAsignado = viewModel.idAssignedUser;
            entity.Autorizado = viewModel.Authorized;
            entity.AutorizadoPor = viewModel.AuthorizedBy;
            entity.FechaAutorizado = DateTime.Now;
            return entity;
        }
        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override TicketViewModel PrepareSingleReturn(tTicket entity)
        {
            return new TicketViewModel()
            {
                idTicket = entity.idTicket,
                Code = entity.Codigo,
                Title = entity.Titulo,
                Description = entity.Descripcion,
                idStatusTicket = entity.idEstatusTicket,
                Image = entity.Imagen,
                CreatedBy = entity.CreadoPor,
                Created = entity.Creado,
                ModifiedBy = entity.ModificadoPor,
                Modified = entity.Modificado,
                idPriorityTicket = entity.idPrioridadTicket,
                idTypeTicket = entity.idTipoTicket,
                idAssignedUser = entity.idUsuarioAsignado,
                Authorized = entity.Autorizado,
                AuthorizedBy = entity.AutorizadoPor,
                AuthorizedDate = entity.FechaAutorizado
            };
        }
        /// <summary>
        /// Prepares the multiple return.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public override List<TicketViewModel> PrepareMultipleReturn(List<tTicket> entities)
        {
            return entities.Select(p => new TicketViewModel()
            {
                idTicket = p.idTicket,
                Code = p.Codigo,
                Title = p.Titulo,
                Description = p.Descripcion,
                idStatusTicket = p.idEstatusTicket,
                Image = p.Imagen,
                CreatedBy = p.CreadoPor,
                Created = p.Creado,
                ModifiedBy = p.ModificadoPor,
                Modified = p.Modificado,
                idPriorityTicket = p.idPrioridadTicket,
                idTypeTicket = p.idTipoTicket,
                idAssignedUser = p.idUsuarioAsignado,
                Authorized = p.Autorizado,
                AuthorizedBy = p.AutorizadoPor,
                AuthorizedDate = p.FechaAutorizado
            }).ToList();
        }
        /// <summary>
        /// Gets the detail by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public TicketViewModel GetDetailById(int id)
        {
            return _repository.GetDetailById(id);
        }
        /// <summary>
        /// Gets all by status.
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="idStatusCredit">The identifier status credit.</param>
        /// <param name="idAssignedUser"></param>
        /// <returns></returns>
        public List<TicketViewModel> GetAllByStatus(int? idUser, int idStatusCredit, int? idAssignedUser)
        {
            return _repository.GetAllByStatus(idUser, idStatusCredit, idAssignedUser);
        }
        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <returns></returns>
        public string GenerateCode()
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder result = new StringBuilder(4);
            for (int i = 0; i < 4; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Updates the image.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="image"></param>
        public void UpdateImage(int id, string image)
        {
            var ticket = _repository.GetById(id);
            ticket.Imagen = image;

            _repository.Update(ticket);
        }
        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <param name="idAssignedUser">The identifier status ticket.</param>
        /// <param name="estimated">The identifier status ticket.</param>
        /// <param name="filled">The identifier status ticket.</param>
        public void UpdateStatusPending(int id, int idStatusTicket, int idAssignedUser, decimal estimated, decimal filled)
        {
            var ticket = _repository.GetById(id);
            ticket.idEstatusTicket = idStatusTicket;
            ticket.idUsuarioAsignado = idAssignedUser;
            ticket.Estimado = estimated;
            ticket.Completado = filled;

            _repository.Update(ticket);
        }
        /// <summary>
        /// Updates the status active.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <param name="idAssignedUser">The identifier status ticket.</param>
        /// <param name="estimated">The identifier status ticket.</param>
        /// <param name="filled">The identifier status ticket.</param>
        public void UpdateStatusActive(int id, int idStatusTicket, int idAssignedUser, decimal estimated, decimal filled)
        {
            var ticket = _repository.GetById(id);
            ticket.idEstatusTicket = idStatusTicket;
            ticket.Modificado = DateTime.Now;
            ticket.idUsuarioAsignado = idAssignedUser;
            ticket.Estimado = estimated;
            ticket.Completado = filled;

			_repository.Update(ticket);
		}
		/// <summary>
		/// Updates the status closed.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="idStatusTicket">The identifier status ticket.</param>
		/// <param name="idAssignedUser">The identifier status ticket.</param>
		/// <param name="estimated">The identifier status ticket.</param>
		/// <param name="filled">The identifier status ticket.</param>
		public void UpdateStatusClosed(int id, int idStatusTicket, int idAssignedUser, decimal estimated, decimal filled)
		{
			var ticket = _repository.GetById(id);
			ticket.idEstatusTicket = idStatusTicket;
			ticket.Cerrado = DateTime.Now;
			ticket.idUsuarioAsignado = idAssignedUser;
			ticket.Estimado = estimated;
			ticket.Completado = filled;

			_repository.Update(ticket);
        }
        /// <summary>
        /// ds the active.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Deactive(int id)
        {
            var ticket = _repository.GetById(id);
            ticket.Activo = false;

            _repository.Update(ticket);
        }
        /// <summary>
        /// Posts the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        public override TicketViewModel Post(TicketViewModel model)
        {
            var entity = PrepareAddData(model);
            var result = _repository.Add(entity);

            var users = new Users();
            var email = new Email();
            var user = users.GetUser((int)model.idAssignedUser);
            var createdBy = users.GetUser((int)model.CreatedBy);
            var body = $"El ticket {entity.Codigo} - {entity.Titulo} fue asignado a {user.Nombre} {user.Apellidos}";
            body = body + "<br/>";
            body = body + "<br/>";
            body = body + model.Description;
            body = body + "<br/>";
            body = body + "<br/>";
            body = body + "Creado por:<b>" + createdBy.Nombre + " " + createdBy.Apellidos + "</b>";

            body = body + "<form action='http://accentsadmin.com/Tickets/updateAuthorizedBy' method='get'>";
            body = body + $"<input type='hidden' id='idTicket' name='idTicket' value={result.idTicket}>";
            body = body + "<br>";
            body = body + $"<input type='hidden' id='AuthorizedBy' name='AuthorizedBy' value=8>";
            body = body + "<br>";
            body = body + "<br>";
            body = body + "<input type='submit' value='AUTORIZAR' style='background-color: #4CAF50;";
            body = body + "border:none;";
            body = body + "color:white;";
            body = body + "padding: 15px 32px;";
            body = body + "text-align:center;";
            body = body + "text-decoration:none;";
            body = body + "display: inline-block;";
            body = body + "fontSize: 16px;";
            body = body + "margin: 4px 2px;";
            body = body + "cursor: pointer;'>";
            body = body + "</form>";
            body = body + "<br/>";

            //var emails = new List<string>();
            //emails.Add(user.Correo);
            //emails.Add(FernandaEmail);
            //email.SendMail(emails, $"Nuevo ticket {entity.Codigo}", body);

            var envioMail = new SendEmailNotifications()
            {
                SubjectEmail = $"Nuevo ticket {entity.Codigo}",
                BodyEmail = body,
                EmailEnvia = user.Correo + "; " + FernandaEmail,
                EmailConCopiaEnvia = "",
            };
            var emailServiceV2 = new EmailV2();
            emailServiceV2.SendMail(envioMail, false);

            return PrepareSingleReturn(result);
		}

        public void UpdateAuthorized(AuthorizeTicketViewModel authorizeTicket)
        {
            var ticket = _repository.GetById(authorizeTicket.idTicket);

            ticket.idTicket = authorizeTicket.idTicket;
            ticket.AutorizadoPor = authorizeTicket.AuthorizedBy;
            ticket.FechaAutorizado = DateTime.Now;
            ticket.Autorizado = true;

            _repository.Update(ticket);
        }
    }
}