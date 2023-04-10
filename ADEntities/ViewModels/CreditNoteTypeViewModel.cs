using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// CreditNoteTypeViewModel
    /// </summary>
    public class CreditNoteTypeViewModel
    {
        /// <summary>
        /// Gets or sets the identifier tipo nota credito.
        /// </summary>
        /// <value>
        /// The identifier tipo nota credito.
        /// </value>
        public int idTipoNotaCredito { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Nombre { get; set; }
    }
}