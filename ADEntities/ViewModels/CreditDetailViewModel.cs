using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// DetailCreditViewModel
    /// </summary>
    public class CreditDetailViewModel
    {
        /// <summary>
        /// Gets or sets the identifier detalle credito.
        /// </summary>
        /// <value>
        /// The identifier detalle credito.
        /// </value>
        public int idDetalleCredito { get; set; }
        /// <summary>
        /// Gets or sets the identifier producto.
        /// </summary>
        /// <value>
        /// The identifier producto.
        /// </value>
        public int? idProducto { get; set; }
        /// <summary>
        /// Gets or sets the codigo.
        /// </summary>
        /// <value>
        /// The codigo.
        /// </value>
        public string Codigo { get; set; }
        /// <summary>
        /// Gets or sets the identifier servicio.
        /// </summary>
        /// <value>
        /// The identifier servicio.
        /// </value>
        public int? idServicio { get; set; }
        /// <summary>
        /// Gets or sets the precio.
        /// </summary>
        /// <value>
        /// The precio.
        /// </value>
        public decimal? Precio { get; set; }
        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>
        /// The descripcion.
        /// </value>
        public string Descripcion { get; set; }
        /// <summary>
        /// Gets or sets the identifier sucursal.
        /// </summary>
        /// <value>
        /// The identifier sucursal.
        /// </value>
        public int idSucursal { get; set; }
        /// <summary>
        /// Gets or sets the amazonas.
        /// </summary>
        /// <value>
        /// The amazonas.
        /// </value>
        public decimal? Amazonas { get; set; }
        /// <summary>
        /// Gets or sets the guadalquivir.
        /// </summary>
        /// <value>
        /// The guadalquivir.
        /// </value>
        public decimal? Guadalquivir { get; set; }
        /// <summary>
        /// Gets or sets the textura.
        /// </summary>
        /// <value>
        /// The textura.
        /// </value>
        public decimal? Textura { get; set; }
        /// <summary>
        /// Gets or sets the cantidad.
        /// </summary>
        /// <value>
        /// The cantidad.
        /// </value>
        public decimal? Cantidad { get; set; }
        /// <summary>
        /// Gets or sets the tipo.
        /// </summary>
        /// <value>
        /// The tipo.
        /// </value>
        public int Tipo { get; set; }
    }
}