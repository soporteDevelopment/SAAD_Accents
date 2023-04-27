using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// PrioridadTicketViewModel
    /// </summary>
    public class PriorityTicketViewModel
    {
        /// <summary>
        /// Gets or sets the identifier id priority ticket.
        /// </summary>
        /// <value>
        /// The identifier Id Priority ticket.
        /// </value>
        public int idPriorityTicket { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Name { get; set; }
    }
}