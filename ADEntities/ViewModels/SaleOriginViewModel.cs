using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// OriginSaleViewModel
    /// </summary>
    public class SaleOriginViewModel
    {
        /// <summary>
        /// Gets or sets the identifier origen.
        /// </summary>
        /// <value>
        /// The identifier origen.
        /// </value>
        public int idOrigen { get; set; }
        /// <summary>
        /// Gets or sets the origen.
        /// </summary>
        /// <value>
        /// The origen.
        /// </value>
        public string Origen { get; set; }
        /// <summary>
        /// Gets or sets the activo.
        /// </summary>
        /// <value>
        /// The activo.
        /// </value>
        public Nullable<bool> Activo { get; set; }
    }
}