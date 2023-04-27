using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// BaseViewModel
    /// </summary>
    public class BaseViewModel
    {
        /// <summary>
        /// Gets or sets the creado por.
        /// </summary>
        /// <value>
        /// The creado por.
        /// </value>
        public int? CreadoPor {get; set;}

        /// <summary>
        /// Gets or sets the creado.
        /// </summary>
        /// <value>
        /// The creado.
        /// </value>
        public DateTime? Creado { get; set; }

        /// <summary>
        /// Gets or sets the modificado por.
        /// </summary>
        /// <value>
        /// The modificado por.
        /// </value>
        public int? ModificadoPor { get; set; }

        /// <summary>
        /// Gets or sets the modificado.
        /// </summary>
        /// <value>
        /// The modificado.
        /// </value>
        public DateTime? Modificado { get; set; }

    }
}