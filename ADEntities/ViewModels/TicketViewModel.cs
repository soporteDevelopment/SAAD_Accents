using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// EstatusTicketViewModel
    /// </summary>
    public class TicketViewModel
    {
        /// <summary>
        /// Gets or sets the identifier ticket.
        /// </summary>
        /// <value>
        /// The identifier ticket.
        /// </value>
        public int idTicket { get; set; }

        /// <summary>
        /// Gets or sets the codigo.
        /// </summary>
        /// <value>
        /// The codigo.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the titulo.
        /// </summary>
        /// <value>
        /// The titulo.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>
        /// The descripcion.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the identifier type ticket.
        /// </summary>
        /// <value>
        /// The identifier type ticket.
        /// </value>
        public Nullable<int> idTypeTicket { get; set; }
        /// <summary>
        /// Gets or sets the identifier priority tickets.
        /// </summary>
        /// <value>
        /// The identifier priority tickets.
        /// </value>
        public Nullable<int> idPriorityTicket { get; set; }
        /// <summary>
        /// Gets or sets the identifier estatus ticket.
        /// </summary>
        /// <value>
        /// The identifier estatus ticket.
        /// </value>
        public int idStatusTicket { get; set; }
        /// <summary>
        /// Gets or sets the creado por.
        /// </summary>
        /// <value>
        /// The creado por.
        /// </value>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the name of the creator.
        /// </summary>
        /// <value>
        /// The name of the creator.
        /// </value>
        public string CreatorName { get; set; }
        /// <summary>
        /// Gets or sets the creado.
        /// </summary>
        /// <value>
        /// The creado.
        /// </value>
        public Nullable<DateTime> Created { get; set; }
        /// <summary>
        /// Gets or sets the modificado por.
        /// </summary>
        /// <value>
        /// The modificado por.
        /// </value>
        public Nullable<int> ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets the modificado.
        /// </summary>
        /// <value>
        /// The modificado.
        /// </value>
        public Nullable<DateTime> Modified { get; set; }

        /// <summary>
        /// Gets or sets the imagen.
        /// </summary>
        /// <value>
        /// The imagen.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the active.
        /// </summary>
        /// <value>
        /// The active.
        /// </value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the closed.
        /// </summary>
        /// <value>
        /// The closed.
        /// </value>
        public Nullable<DateTime> Closed { get; set; }
        /// <summary>
        /// Gets or sets the assignedUser.
        /// </summary>
        /// <value>
        /// The assignedUser.
        /// </value>
        public Nullable<int> idAssignedUser { get; set; }
        /// <summary>
        /// Gets or sets the estimated.
        /// </summary>
        /// <value>
        /// The estimated.
        /// </value>
        public Nullable<decimal> Estimated { get; set; }
        /// <summary>
        /// Gets or sets the filled.
        /// </summary>
        /// <value>
        /// The filled.
        /// </value>
        public Nullable<decimal> Filled { get; set; }
        /// <summary>
        /// Gets or sets the authorized.
        /// </summary>
        /// <value>
        /// The authorized.
        /// </value>
        public Nullable<bool> Authorized { get; set; }
        /// <summary>
        /// Gets or sets the authorized by.
        /// </summary>
        /// <value>
        /// The authorized by.
        /// </value>
        public Nullable<int> AuthorizedBy { get; set; }
        /// <summary>
        /// Gets or sets the authorized date.
        /// </summary>
        /// <value>
        /// The authorized date.
        /// </value>
        public Nullable<DateTime> AuthorizedDate { get; set; }
    }
}