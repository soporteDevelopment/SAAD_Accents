using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// StatusTicketViewModel
    /// </summary>
    public class StatusTicketViewModel
    {
        /// <summary>
        /// Gets or sets the identifier estatus ticket.
        /// </summary>
        /// <value>
        /// The identifier Status ticket.
        /// </value>
        public int idStatusTicket { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Name { get; set; }
    }
}