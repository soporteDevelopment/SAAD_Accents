using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// CatalogViewPrintViewModel
    /// </summary>
    /// <seealso cref="ADEntities.ViewModels.BaseViewModel" />
    public class CatalogViewPrintViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the identifier vista.
        /// </summary>
        /// <value>
        /// The identifier vista.
        /// </value>
        public int idVista { get; set; }
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public string Numero { get; set; }
        /// <summary>
        /// Gets or sets the identifier usuario.
        /// </summary>
        /// <value>
        /// The identifier usuario.
        /// </value>
        public int idUsuario { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
        public string Usuario { get; set; }
        /// <summary>
        /// Gets or sets the identifier cliente fisico.
        /// </summary>
        /// <value>
        /// The identifier cliente fisico.
        /// </value>
        public string Cliente { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public AddressViewModel Direccion { get; set; }        
        /// <summary>
        /// Gets or sets the identifier sucursal.
        /// </summary>
        /// <value>
        /// The identifier sucursal.
        /// </value>
        public int idSucursal { get; set; }
        /// <summary>
        /// Gets or sets the sucursal.
        /// </summary>
        /// <value>
        /// The sucursal.
        /// </value>
        public string Sucursal { get; set; }
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        /// <value>
        /// The fecha.
        /// </value>
        public DateTime Fecha { get; set; }
        /// <summary>
        /// Gets or sets the cantidad productos.
        /// </summary>
        /// <value>
        /// The cantidad productos.
        /// </value>
        public decimal CantidadProductos { get; set; }
        /// <summary>
        /// Gets or sets the subtotal.
        /// </summary>
        /// <value>
        /// The subtotal.
        /// </value>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public decimal Total { get; set; }
        /// <summary>
        /// Gets or sets the estatus.
        /// </summary>
        /// <value>
        /// The estatus.
        /// </value>
        public int Estatus { get; set; }
        /// <summary>
        /// Gets or sets the tipo cliente.
        /// </summary>
        /// <value>
        /// The tipo cliente.
        /// </value>
        public byte TipoCliente { get; set; }
        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public List<CatalogDetailViewViewModel> Detalle { get; set; }
    }
}