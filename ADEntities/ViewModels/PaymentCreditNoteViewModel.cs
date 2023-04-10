using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class PaymentCreditNoteViewModel
    {
        /// <summary>
        /// Gets or sets the identifier forma pago.
        /// </summary>
        /// <value>
        /// The identifier forma pago.
        /// </value>
        public int idFormaPago { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PaymentCreditNoteViewModel"/> is history.
        /// </summary>
        /// <value>
        ///   <c>true</c> if history; otherwise, <c>false</c>.
        /// </value>
        public bool History { get; set; }
    }
}