using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// CatalogDetailDevolutionViewModel
    /// </summary>
    public class CatalogDetailDevolutionViewModel
    {
        /// <summary>
        /// Gets or sets the identifier detalle devoluciones.
        /// </summary>
        /// <value>
        /// The identifier detalle devoluciones.
        /// </value>
        public int idDetalleDevoluciones { get; set; }
        /// <summary>
        /// Gets or sets the identifier detalle vista.
        /// </summary>
        /// <value>
        /// The identifier detalle vista.
        /// </value>
        public int idDetalleVista { get; set; }
        /// <summary>
        /// Gets or sets the devolucion.
        /// </summary>
        /// <value>
        /// The devolucion.
        /// </value>
        public decimal Devolucion { get; set; }
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        /// <value>
        /// The fecha.
        /// </value>
        public DateTime Fecha { get; set; }
        /// <summary>
        /// Gets or sets the identifier verificador.
        /// </summary>
        /// <value>
        /// The identifier verificador.
        /// </value>
        public int idVerificador { get; set; }
        /// <summary>
        /// Gets or sets the verificador.
        /// </summary>
        /// <value>
        /// The verificador.
        /// </value>
        public string Verificador { get; set; }
        /// <summary>
        /// Gets or sets the comentarios.
        /// </summary>
        /// <value>
        /// The comentarios.
        /// </value>
        public string Comentarios { get; set; }
    }
}