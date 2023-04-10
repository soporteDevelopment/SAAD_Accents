using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// BranchCatalogViewModel
    /// </summary>
    public class BranchCatalogViewModel
    {
        /// <summary>
        /// Gets or sets the identifier catalogo sucursal.
        /// </summary>
        /// <value>
        /// The identifier catalogo sucursal.
        /// </value>
        public int idCatalogoSucursal { get; set; }
        /// <summary>
        /// Gets or sets the identifier catalogo.
        /// </summary>
        /// <value>
        /// The identifier catalogo.
        /// </value>
        public int idCatalogo { get; set; }
        /// <summary>
        /// Gets or sets the identifier sucursal.
        /// </summary>
        /// <value>
        /// The identifier sucursal.
        /// </value>
        public int idSucursal { get; set; }
        /// <summary>
        /// Gets or sets the cantidad.
        /// </summary>
        /// <value>
        /// The cantidad.
        /// </value>
        public decimal? Cantidad { get; set; }
        /// <summary>
        /// Gets or sets the creado por.
        /// </summary>
        /// <value>
        /// The creado por.
        /// </value>
        public Nullable<int> CreadoPor { get; set; }
        /// <summary>
        /// Gets or sets the creado.
        /// </summary>
        /// <value>
        /// The creado.
        /// </value>
        public Nullable<DateTime> Creado { get; set; }
        /// <summary>
        /// Gets or sets the modificado por.
        /// </summary>
        /// <value>
        /// The modificado por.
        /// </value>
        public Nullable<int> ModificadoPor { get; set; }
        /// <summary>
        /// Gets or sets the modificado.
        /// </summary>
        /// <value>
        /// The modificado.
        /// </value>
        public Nullable<DateTime> Modificado { get; set; }
    }
}