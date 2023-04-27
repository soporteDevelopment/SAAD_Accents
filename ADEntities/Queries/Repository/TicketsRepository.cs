using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.Repository
{
	/// <summary>
	/// EstatusTicketRepository
	/// </summary>
	/// <seealso cref="ADEntities.Queries.Repository.BaseRepository&lt;ADEntities.Models.tOrigenCliente&gt;" />
	/// <seealso cref="ADEntities.Queries.IRepository.ITicketsRepository" />
	public class TicketsRepository : BaseRepository<tTicket>, ITicketsRepository
	{
		private readonly admDB_SAADDBEntities context;
		private const int ClosedStatusTicket = 3;

		/// <summary>
		/// Initializes a new instance of the <see cref="TicketsRepository"/> class.
		/// </summary>
		/// <param name="_context">The context.</param>
		public TicketsRepository(admDB_SAADDBEntities _context) : base(_context)
		{
			context = _context;
		}
		/// <summary>
		/// Gets all by status.
		/// </summary>
		/// <param name="idUser">The identifier user.</param>
		/// <param name="idStatusCredit">The identifier status credit.</param>
		/// <param name="idAssignedUser">The identifier assigned user.</param>
		/// <returns></returns>
		public List<TicketViewModel> GetAllByStatus(int? idUser, int idStatusCredit, int? idAssignedUser)
		{
			DateTime? sinceDate = DateTime.Now;

			if (idStatusCredit == ClosedStatusTicket)
			{
				sinceDate = new DateTime(DateTime.Now.AddDays(-5).Year, DateTime.Now.AddDays(-5).Month, DateTime.Now.AddDays(-5).Day, 0, 0, 0);
			}
			else
			{
				sinceDate = null;
			}

			return (from ticket in context.tTickets
					join user in context.tUsuarios on ticket.CreadoPor equals user.idUsuario
					where (ticket.CreadoPor == idUser || idUser == null) && ticket.idEstatusTicket == idStatusCredit
					&& (ticket.idUsuarioAsignado == idAssignedUser || idAssignedUser == null)
					&& (ticket.Creado > sinceDate || sinceDate == null)
					&& ticket.Activo == true
					select new TicketViewModel()
					{
						idTicket = ticket.idTicket,
						Code = ticket.Codigo,
						Title = ticket.Titulo,
						Description = ticket.Descripcion,
						idTypeTicket = ticket.idTipoTicket,
						idPriorityTicket = ticket.idPrioridadTicket,
						idStatusTicket = ticket.idEstatusTicket,
						CreatedBy = ticket.CreadoPor,
						CreatorName = user.Nombre + " " + user.Apellidos,
						Created = ticket.Creado,
						ModifiedBy = ticket.ModificadoPor,
						Modified = ticket.Modificado,
						Image = ticket.Imagen,
						Active = (bool)ticket.Activo,
						Closed = ticket.Cerrado,
						idAssignedUser = ticket.idUsuarioAsignado
					}).ToList();
		}
		/// <summary>
		/// Gets the detail by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public TicketViewModel GetDetailById(int id)
		{
			return (from ticket in context.tTickets
					join user in context.tUsuarios on ticket.CreadoPor equals user.idUsuario
					where ticket.idTicket == id && ticket.Activo == true
					select new TicketViewModel()
					{
						idTicket = ticket.idTicket,
						Code = ticket.Codigo,
						Title = ticket.Titulo,
						Description = ticket.Descripcion,
						idTypeTicket = ticket.idTipoTicket,
						idPriorityTicket = ticket.idPrioridadTicket,
						idStatusTicket = ticket.idEstatusTicket,
						CreatedBy = ticket.CreadoPor,
						CreatorName = user.Nombre + " " + user.Apellidos,
						Created = ticket.Creado,
						ModifiedBy = ticket.ModificadoPor,
						Modified = ticket.Modificado,
						Image = ticket.Imagen,
						Active = (bool)ticket.Activo,
						Closed = ticket.Cerrado,
						idAssignedUser = ticket.idUsuarioAsignado,
						Estimated = ticket.Estimado,
						Filled = ticket.Completado,
						Authorized = ticket.Autorizado,
						AuthorizedBy = ticket.AutorizadoPor,
						AuthorizedDate = ticket.FechaAutorizado
					}).FirstOrDefault();
		}
		/// <summary>
		/// Patches the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public override tTicket Update(tTicket entity)
		{
			var ticket = context.tTickets.Find(entity.idTicket);
			ticket = entity;
			context.SaveChanges();

            return entity;
        }
    }
}