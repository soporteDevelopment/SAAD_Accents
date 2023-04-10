using ADEntities.Models;
using ADEntities.ViewModels;
using ADSystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// ISaleOriginManager
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tTicket, ADEntities.ViewModels.TicketViewModel&gt;" />
    public interface ITicketsManager : IBaseManager<tTicket,TicketViewModel>
    {
        /// <summary>
        /// Gets all by status.
        /// </summary>
        /// <param name="idStatusCredit">The identifier status credit.</param>
        /// <returns></returns>
        List<TicketViewModel> GetAllByStatus(int? idUser,int idStatusCredit, int? idAssignedUser);
        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <param name="idAssignedUser">The identifier status ticket.</param>
        /// <param name="estimated">The identifier status ticket.</param>
        /// <param name="filled">The identifier status ticket.</param>
        void UpdateStatusPending(int id, int idStatusTicket, int idAssignedUser, decimal estimated, decimal filled);
        /// <summary>
        /// Updates the image.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="Image">The image.</param>
        /// <returns></returns>
        void UpdateImage(int id, string image);
        /// <summary>
        /// Updates the status active.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <param name="idAssignedUser">The identifier status ticket.</param>
        /// <param name="estimated">The identifier status ticket.</param>
        /// <param name="filled">The identifier status ticket.</param>
        /// <returns></returns>
        void UpdateStatusActive(int id, int idStatusTicket,int idAssignedUser,decimal estimated,decimal filled);
        /// <summary>
        /// Updates the status closed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idStatusTicket">The identifier status ticket.</param>
        /// <param name="idAssignedUser">The identifier status ticket.</param>
        /// <param name="estimated">The identifier status ticket.</param>
        /// <param name="filled">The identifier status ticket.</param>
        /// <returns></returns>
        void UpdateStatusClosed(int id, int idStatusTicket, int idAssignedUser,decimal estimated,decimal filled);
        /// <summary>
        /// ds the active.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Deactive(int id);
        /// <summary>
        /// Gets the detail by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TicketViewModel GetDetailById(int id);
        /// <summary>
        /// Updates the authorized.
        /// </summary>
        /// <param name="authorizeTicket">The authorize ticket.</param>
        /// <returns></returns>
        void UpdateAuthorized(AuthorizeTicketViewModel authorizeTicket);
    }
}