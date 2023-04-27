using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class BranchOfficeViewModel
    {
        /// <summary>
        /// Gets or sets the identifier sucursal.
        /// </summary>
        /// <value>
        /// The identifier sucursal.
        /// </value>
        public int idSucursal { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>
        /// The descripcion.
        /// </value>
        public string Descripcion { get; set; }

    }
}