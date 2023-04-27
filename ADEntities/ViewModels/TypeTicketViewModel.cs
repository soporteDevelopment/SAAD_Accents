using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class TypeTicketViewModel
    {
        /// <summary>
        /// Gets or sets the identifier tipo ticket.
        /// </summary>
        /// <value>
        /// The identifier Tipo ticket.
        /// </value>
        public int idTypeTicket { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Name { get; set; }
    }
}