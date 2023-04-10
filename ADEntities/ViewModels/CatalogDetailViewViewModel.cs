using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// CatalogDetailViewViewModel
    /// </summary>
    public class CatalogDetailViewViewModel
    {
        /// <summary>
        /// Gets or sets the identifier detalle vista.
        /// </summary>
        /// <value>
        /// The identifier detalle vista.
        /// </value>
        public int idDetalleVista { get; set; }
        /// <summary>
        /// Gets or sets the identifier vista.
        /// </summary>
        /// <value>
        /// The identifier vista.
        /// </value>
        public int idVista { get; set; }
        /// <summary>
        /// Gets or sets the identifier catalogo.
        /// </summary>
        /// <value>
        /// The identifier catalogo.
        /// </value>
        public int idCatalogo { get; set; }
        /// <summary>
        /// Gets or sets the imagen.
        /// </summary>
        /// <value>
        /// The imagen.
        /// </value>
        public string Imagen { get; set; }
        /// <summary>
        /// Gets or sets the catalogo.
        /// </summary>
        /// <value>
        /// The catalogo.
        /// </value>
        public CatalogViewModel Catalogo { get; set; }
        /// <summary>
        /// Gets or sets the precio.
        /// </summary>
        /// <value>
        /// The precio.
        /// </value>
        public decimal Precio { get; set; }
        /// <summary>
        /// Gets or sets the cantidad.
        /// </summary>
        /// <value>
        /// The cantidad.
        /// </value>
        public decimal Cantidad { get; set; }
        /// <summary>
        /// Gets or sets the identifier sucursal.
        /// </summary>
        /// <value>
        /// The identifier sucursal.
        /// </value>
        public int idSucursal { get; set; }
        /// <summary>
        /// Gets or sets the devolucion.
        /// </summary>
        /// <value>
        /// The devolucion.
        /// </value>
        public Nullable<decimal> Pendiente { get; set; }
        /// <summary>
        /// Gets or sets the devolucion.
        /// </summary>
        /// <value>
        /// The devolucion.
        /// </value>
        public Nullable<decimal> Devolucion { get; set; }
        /// <summary>
        /// Gets or sets the cantidad devolucion.
        /// </summary>
        /// <value>
        /// The cantidad devolucion.
        /// </value>
        public Nullable<decimal> CantidadDevolucion { get; set; }
        /// <summary>
        /// Gets or sets the comentarios.
        /// </summary>
        /// <value>
        /// The comentarios.
        /// </value>
        public string Comentarios { get; set; }
        /// <summary>
        /// Gets or sets the estatus.
        /// </summary>
        /// <value>
        /// The estatus.
        /// </value>
        public int Estatus { get; set; }
        /// <summary>
        /// Gets or sets the devolution.
        /// </summary>
        /// <value>
        /// The devolution.
        /// </value>
        public List<CatalogDetailDevolutionViewModel> Detalle { get; set; }
    }
}