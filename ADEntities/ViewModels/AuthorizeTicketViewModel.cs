using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class AuthorizeTicketViewModel
    {
        /// <summary>
        /// Gets or sets the identifier ticket.
        /// </summary>
        /// <value>
        /// The identifier ticket.
        /// </value>
        public int idTicket { get; set; }
        /// <summary>
        /// Gets or sets the authorized by.
        /// </summary>
        /// <value>
        /// The authorized by.
        /// </value>
        public int AuthorizedBy { get; set; }
    }
}